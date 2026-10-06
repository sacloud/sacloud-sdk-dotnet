namespace Sacloud.Sdk.AppRunShared.Samples.Operations;

public sealed class VersionsOperations(AppRunSharedClient client)
{
    private readonly AppRunSharedClient _client = client;

    public async Task ListVersionsAsync()
    {
        Console.Write("バージョン一覧を取得するアプリケーションIDを入力してください: ");
        var id = Console.ReadLine();
        var versions = await _client.Applications[id].Versions.GetAsync(config =>
        {
            config.QueryParameters.PageNum = 1;
            config.QueryParameters.PageSize = 50;
            config.QueryParameters.SortField = "created_at";
            config.QueryParameters.SortOrder = Applications.Item.Versions.GetSort_orderQueryParameterType.Desc;
        });

        Console.WriteLine("\nバージョン一覧:");
        Console.WriteLine("Meta:");
        Console.WriteLine($"  PageNum: {versions.Meta.PageNum}");
        Console.WriteLine($"  PageSize: {versions.Meta.PageSize}");
        Console.WriteLine($"  ObjectTotal: {versions.Meta.ObjectTotal}");
        Console.WriteLine($"  SortField: {versions.Meta.SortField}");
        Console.WriteLine($"  SortOrder: {versions.Meta.SortOrder}");
        Console.WriteLine("Data:");
        foreach (var version in versions.Data)
        {
            Console.WriteLine($"- Id: {version.Id}");
            Console.WriteLine($"  Name: {version.Name}");
            Console.WriteLine($"  CreatedAt: {version.CreatedAt}");
            Console.WriteLine($"  Status: {version.Status}");
        }
    }

    public async Task GetVersionDetailsAsync()
    {
        Console.Write("バージョン詳細を取得するアプリケーションIDを入力してください: ");
        var applicationId = Console.ReadLine();
        Console.Write("詳細を取得するバージョンIDを入力してください: ");
        var versionId = Console.ReadLine();
        var version = await _client.Applications[applicationId].Versions[versionId].GetAsync();
        Console.WriteLine("バージョン詳細:");
        Console.WriteLine($"ID: {version.Id}");
        Console.WriteLine($"Name: {version.Name}");
        Console.WriteLine($"CreatedAt: {version.CreatedAt}");
        Console.WriteLine($"TimeoutSeconds: {version.TimeoutSeconds}");
        Console.WriteLine($"MinScale: {version.MinScale}");
        Console.WriteLine($"MaxScale: {version.MaxScale}");
        Console.WriteLine($"Port: {version.Port}");
        Console.WriteLine($"ScaleTargetConcurrency: {version.ScaleTargetConcurrency}");
        Console.WriteLine($"Status: {version.Status}");

        Console.WriteLine("Components:");
        foreach (var component in version.Components)
        {
            Console.WriteLine($"- Name: {component.Name}");
            Console.WriteLine($"  MaxCpu: {component.MaxCpu}");
            Console.WriteLine($"  MaxMemory: {component.MaxMemory}");

            Console.WriteLine("  DeploySource:");
            if (component.DeploySource.ContainerRegistry is not null)
            {
                Console.WriteLine("    ContainerRegistry:");
                Console.WriteLine($"      Image: {component.DeploySource.ContainerRegistry.Image}");
                Console.WriteLine($"      Server: {component.DeploySource.ContainerRegistry.Server}");
                Console.WriteLine($"      Username: {component.DeploySource.ContainerRegistry.Username}");
            }

            if (component.Env is not null)
            {
                Console.WriteLine("  Env:");
                foreach (var env in component.Env)
                {
                    Console.WriteLine($"    - Key: {env.Key}");
                    Console.WriteLine($"      Value: {env.Value}");
                }
            }

            if (component.Secret is not null)
            {
                Console.WriteLine("  Secret:");
                foreach (var secret in component.Secret)
                {
                    Console.WriteLine($"    - Key: {secret.Key}");
                }
            }

            if (component.Probe is not null)
            {
                Console.WriteLine("  Probe:");
                if (component.Probe.HttpGet is not null)
                {
                    Console.WriteLine("    HttpGet:");
                    Console.WriteLine($"      Path: {component.Probe.HttpGet.Path}");
                    Console.WriteLine($"      Port: {component.Probe.HttpGet.Port}");

                    if (component.Probe.HttpGet.Headers is not null)
                    {
                        Console.WriteLine("      Headers:");
                        foreach (var header in component.Probe.HttpGet.Headers)
                        {
                            Console.WriteLine($"      - Name: {header.Name}");
                            Console.WriteLine($"        Value: {header.Value}");
                        }
                    }
                }
            }
        }
    }

    public async Task DeleteVersionAsync()
    {
        Console.Write("バージョンを削除するアプリケーションIDを入力してください: ");
        var applicationId = Console.ReadLine();
        Console.Write("削除するバージョンIDを入力してください: ");
        var versionId = Console.ReadLine();
        await _client.Applications[applicationId].Versions[versionId].DeleteAsync();
        Console.WriteLine($"バージョンを削除しました: applicationId={applicationId}, versionId={versionId}");
    }

    public async Task GetVersionStatusAsync()
    {
        Console.Write("バージョンステータスを取得するアプリケーションIDを入力してください: ");
        var applicationId = Console.ReadLine();
        Console.Write("ステータスを取得するバージョンIDを入力してください: ");
        var versionId = Console.ReadLine();
        var versionStatus = await _client.Applications[applicationId].Versions[versionId].Status.GetAsync();

        Console.WriteLine("バージョンステータス詳細:");
        if (versionStatus.Status.HasValue)
        {
            Console.WriteLine($"バージョンステータス: {versionStatus.Status.Value}");
        }
        else
        {
            Console.WriteLine("ステータスが見つかりませんでした。");
        }
        Console.WriteLine($"メッセージ: {versionStatus.Message}");
    }
}
