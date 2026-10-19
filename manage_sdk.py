#!/usr/bin/env python3
"""Test, pack, and publish SDK modules in this repository."""

from __future__ import annotations

import argparse
import json
import os
import re
import shlex
import shutil
import subprocess
import sys
import urllib.error
import urllib.request
import xml.etree.ElementTree as ET
import zipfile
from dataclasses import dataclass
from pathlib import Path


ROOT = Path(__file__).resolve().parent
SDK_ROOT = ROOT / "sdk"
NUGET_SOURCE = "https://api.nuget.org/v3/index.json"


class SdkScriptError(Exception):
    pass


@dataclass(frozen=True)
class SdkModule:
    name: str
    source_project: Path
    test_project: Path
    version: str

    @property
    def package_id(self) -> str:
        return f"Sacloud.Sdk.{self.name}"

    @property
    def package_path(self) -> Path:
        return ROOT / "artifacts" / "packages" / self.name / f"{self.package_id}.{self.version}.nupkg"

    @property
    def tag(self) -> str:
        return f"{self.package_id}-v{self.version}"


def run_command(
    command: list[str],
    *,
    capture_output: bool = False,
    check: bool = True,
    hidden_values: tuple[str, ...] = (),
) -> subprocess.CompletedProcess[str]:
    display_command = shlex.join(command)
    for value in hidden_values:
        if value:
            display_command = display_command.replace(value, "***")
    print(f"$ {display_command}", flush=True)
    result = subprocess.run(
        command,
        cwd=ROOT,
        text=True,
        capture_output=capture_output,
        check=False,
    )
    if check and result.returncode != 0:
        if capture_output and result.stderr:
            print(result.stderr, file=sys.stderr, end="")
        raise SdkScriptError(f"Command failed with exit code {result.returncode}.")
    return result


def read_version(project: Path) -> str:
    try:
        root = ET.parse(project).getroot()
    except (ET.ParseError, OSError) as error:
        raise SdkScriptError(f"Could not read project file: {project}") from error

    versions = [
        element.text.strip()
        for group in root.findall("PropertyGroup")
        for element in group.findall("Version")
        if element.text and element.text.strip()
    ]
    if not versions or not re.fullmatch(r"[0-9A-Za-z.+-]+", versions[0]):
        raise SdkScriptError(f"A literal package Version is required in {project}.")
    return versions[0]


def module_order(module: SdkModule) -> tuple[bool, str]:
    return module.name != "Shared", module.name


def discover_modules() -> dict[str, SdkModule]:
    modules: dict[str, SdkModule] = {}
    for source_project in sorted(SDK_ROOT.glob("*/src/*.csproj")):
        name = source_project.stem
        module_dir = source_project.parent.parent
        test_project = module_dir / "tests" / f"{name}.Tests.csproj"
        modules[name] = SdkModule(
            name=name,
            source_project=source_project,
            test_project=test_project,
            version=read_version(source_project),
        )
    if not modules:
        raise SdkScriptError(f"No SDK projects found under {SDK_ROOT}.")
    return dict(sorted(modules.items(), key=lambda item: module_order(item[1])))


def select_modules(target: str, modules: dict[str, SdkModule]) -> list[SdkModule]:
    if target == "all":
        selected = list(modules.values())
    elif target in modules:
        selected = [modules[target]]
    else:
        available = ", ".join([*modules, "all"])
        raise SdkScriptError(f"Unknown SDK module '{target}'. Available modules: {available}")
    return sorted(selected, key=module_order)


def test_modules(modules: list[SdkModule]) -> None:
    for module in modules:
        if not module.test_project.is_file():
            raise SdkScriptError(f"Test project not found: {module.test_project}")
        run_command(["dotnet", "test", str(module.test_project), "--configuration", "Release"])


def pack_module(module: SdkModule) -> None:
    output_dir = module.package_path.parent
    output_dir.mkdir(parents=True, exist_ok=True)
    run_command(
        [
            "dotnet",
            "pack",
            str(module.source_project),
            "--configuration",
            "Release",
            "--output",
            str(output_dir),
        ]
    )
    if not module.package_path.is_file():
        raise SdkScriptError(f"Expected package was not created: {module.package_path}")


def element_text(parent: ET.Element, name: str) -> str | None:
    for element in parent:
        if element.tag.rsplit("}", 1)[-1] == name:
            return element.text.strip() if element.text else None
    return None


def read_package_metadata(package_path: Path) -> dict[str, str]:
    try:
        with zipfile.ZipFile(package_path) as package:
            nuspec_paths = [name for name in package.namelist() if name.lower().endswith(".nuspec")]
            if len(nuspec_paths) != 1:
                raise SdkScriptError(f"Expected one .nuspec file in {package_path}.")
            nuspec = ET.fromstring(package.read(nuspec_paths[0]))
    except (OSError, zipfile.BadZipFile, ET.ParseError) as error:
        raise SdkScriptError(f"Could not read package metadata from {package_path}.") from error

    metadata = next(
        (element for element in nuspec if element.tag.rsplit("}", 1)[-1] == "metadata"),
        None,
    )
    if metadata is None:
        raise SdkScriptError(f"Package metadata not found in {package_path}.")

    values = {
        name: element_text(metadata, name)
        for name in ("id", "version", "description", "authors", "license", "tags", "copyright")
    }
    repository = next(
        (element for element in metadata if element.tag.rsplit("}", 1)[-1] == "repository"),
        None,
    )
    if repository is not None:
        for key in ("type", "url"):
            value = repository.attrib.get(key)
            if value:
                values[f"repository {key}"] = value
    return {key: value for key, value in values.items() if value}


def check_modules(modules: list[SdkModule]) -> None:
    for module in modules:
        if not module.package_path.is_file():
            raise SdkScriptError(
                f"Package not found: {module.package_path}. Run 'pack {module.name}' first."
            )
        print(f"{module.package_path.relative_to(ROOT)}")
        for key, value in read_package_metadata(module.package_path).items():
            print(f"  {key}: {value}")


def require_publish_prerequisites() -> str:
    api_key = os.environ.get("NUGET_API_KEY", "")
    if not api_key:
        raise SdkScriptError("Set the NUGET_API_KEY environment variable before publishing.")
    if shutil.which("gh") is None:
        raise SdkScriptError("GitHub CLI 'gh' is required for publishing.")
    run_command(["gh", "auth", "status"])
    remote = run_command(["git", "remote", "get-url", "origin"], capture_output=True)
    if remote.returncode != 0:
        raise SdkScriptError("Git remote 'origin' is required for publishing.")
    # Ensure there are no uncommitted changes under the sdk/ directory before publishing.
    status = run_command(["git", "status", "--porcelain", "--", "sdk"], capture_output=True)
    if status.stdout.strip():
        raise SdkScriptError("Publishing requires no uncommitted changes under sdk/. Commit or stash those changes first.")
    return api_key


def current_commit() -> str:
    result = run_command(["git", "rev-parse", "HEAD"], capture_output=True)
    return result.stdout.strip()


def validate_tags(modules: list[SdkModule], head: str) -> dict[str, bool]:
    remote_url = run_command(["git", "remote", "get-url", "origin"], capture_output=True).stdout.strip()
    remote_tags: dict[str, str] = {}
    for module in modules:
        tag = module.tag
        local = run_command(
            ["git", "rev-parse", "--verify", f"refs/tags/{tag}^{{commit}}"],
            capture_output=True,
            check=False,
        )
        if local.returncode == 0 and local.stdout.strip() != head:
            raise SdkScriptError(f"Local tag '{tag}' points to a different commit.")

        remote = run_command(
            ["git", "ls-remote", "--tags", remote_url, f"refs/tags/{tag}", f"refs/tags/{tag}^{{}}"],
            capture_output=True,
        )
        refs = {
            fields[1]: fields[0]
            for line in remote.stdout.splitlines()
            if len(fields := line.split()) == 2
        }
        remote_commit = refs.get(f"refs/tags/{tag}^{{}}", refs.get(f"refs/tags/{tag}"))
        if remote_commit and remote_commit != head:
            raise SdkScriptError(f"Remote tag '{tag}' points to a different commit.")
        remote_tags[tag] = remote_commit is not None
    return remote_tags


def ensure_tag(module: SdkModule, remote_tag_exists: bool, head: str) -> None:
    tag_ref = f"refs/tags/{module.tag}^{{commit}}"
    local = run_command(["git", "rev-parse", "--verify", tag_ref], capture_output=True, check=False)
    if local.returncode != 0:
        if remote_tag_exists:
            run_command(["git", "fetch", "origin", f"refs/tags/{module.tag}:refs/tags/{module.tag}"])
        else:
            run_command(
                ["git", "tag", "--annotate", module.tag, "--message", f"Release {module.package_id} {module.version}", head]
            )
    run_command(["git", "push", "origin", module.tag])


def update_github_release(module: SdkModule) -> None:
    title = f"{module.package_id} v{module.version}"
    notes = f"Release of {module.package_id} version {module.version}."
    view = run_command(
        ["gh", "release", "view", module.tag, "--json", "tagName"],
        capture_output=True,
        check=False,
    )
    if view.returncode == 0:
        run_command(["gh", "release", "edit", module.tag, "--title", title, "--notes", notes])
        run_command(["gh", "release", "upload", module.tag, str(module.package_path), "--clobber"])
    elif "not found" in view.stderr.lower() or "404" in view.stderr:
        run_command(
            [
                "gh",
                "release",
                "create",
                module.tag,
                str(module.package_path),
                "--verify-tag",
                "--title",
                title,
                "--notes",
                notes,
            ]
        )
    else:
        raise SdkScriptError(f"Could not inspect GitHub release '{module.tag}': {view.stderr.strip()}")


def is_published(module: SdkModule) -> bool:
    url = f"https://api.nuget.org/v3-flatcontainer/{module.package_id.lower()}/index.json"
    try:
        with urllib.request.urlopen(url, timeout=30) as response:
            versions = json.loads(response.read())
    except urllib.error.HTTPError as error:
        if error.code == 404:
            return False
        raise SdkScriptError(f"Could not check NuGet versions for {module.package_id}: {error}") from error
    except (urllib.error.URLError, TimeoutError, json.JSONDecodeError) as error:
        raise SdkScriptError(f"Could not check NuGet versions for {module.package_id}: {error}") from error
    return any(version.casefold() == module.version.casefold() for version in versions.get("versions", []))


def publish_modules(modules: list[SdkModule]) -> None:
    pending_modules = []
    for module in modules:
        if is_published(module):
            print(f"Skipping {module.package_id} {module.version}: already published on NuGet.")
        else:
            pending_modules.append(module)
    if not pending_modules:
        return

    api_key = require_publish_prerequisites()
    head = current_commit()
    remote_tags = validate_tags(pending_modules, head)

    for module in pending_modules:
        pack_module(module)

    for module in pending_modules:
        ensure_tag(module, remote_tags[module.tag], head)
        update_github_release(module)
        run_command(
            [
                "dotnet",
                "nuget",
                "push",
                str(module.package_path),
                "--api-key",
                api_key,
                "--source",
                NUGET_SOURCE,
                "--skip-duplicate",
            ],
            hidden_values=(api_key,),
        )


def build_parser(modules: dict[str, SdkModule]) -> argparse.ArgumentParser:
    parser = argparse.ArgumentParser(
        description="Run SDK module tests, build and inspect NuGet packages, or publish them."
    )
    commands = parser.add_subparsers(dest="command", required=True)
    module_help = f"SDK module name or 'all' (available: {', '.join([*modules, 'all'])})"
    command_help = {
        "test": (
            "Run module tests in Release configuration",
            "Runs dotnet test in Release configuration for the selected SDK module's tests project.",
        ),
        "pack": (
            "Build NuGet packages",
            "Packs the selected SDK module's src project in Release configuration and saves the package under artifacts/packages/<module>/.",
        ),
        "check": (
            "Inspect local NuGet package metadata",
            "Reads the .nuspec from the .nupkg under artifacts/packages/<module>/ without installing it, then displays the package ID, version, description, authors, and other metadata. Run pack first.",
        ),
        "publish": (
            "Publish to NuGet and update the GitHub release",
            "Builds the packages, creates and pushes {PackageId}-v{Version} tags, then creates or updates GitHub releases and publishes the packages to NuGet. Shared is processed first. Requires NUGET_API_KEY, an authenticated GitHub CLI (gh), and a clean Git working tree.",
        ),
    }
    for command, (help_text, description) in command_help.items():
        subparser = commands.add_parser(command, help=help_text, description=description)
        subparser.add_argument("module", help=module_help)
    return parser


def main() -> int:
    try:
        modules = discover_modules()
        args = build_parser(modules).parse_args()
        selected = select_modules(args.module, modules)
        if args.command == "test":
            test_modules(selected)
        elif args.command == "pack":
            for module in selected:
                pack_module(module)
        elif args.command == "check":
            check_modules(selected)
        else:
            publish_modules(selected)
    except SdkScriptError as error:
        print(f"error: {error}", file=sys.stderr)
        return 1
    return 0


if __name__ == "__main__":
    raise SystemExit(main())