using Sacloud.Sdk.AppRunShared.Models;
using Sacloud.Sdk.AppRunShared.Models.Request;

namespace Sacloud.Sdk.AppRunShared.Samples.Operations;

public sealed class ApplicationsOperations(AppRunSharedClient client)
{
    private readonly AppRunSharedClient _client = client;

    private readonly double DELAY_SECONDS = 0.5;

    public async Task ListApplicationsAsync()
    {
        var applications = await _client.Applications.GetAsync(config =>
        {
            config.QueryParameters.PageNum = 1;
            config.QueryParameters.PageSize = 50;
            config.QueryParameters.SortField = "created_at";
            config.QueryParameters.SortOrder = Applications.GetSort_orderQueryParameterType.Desc;
        });

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS));

        Console.WriteLine("\nアプリケーション一覧:");
        Console.WriteLine($"Meta:");
        Console.WriteLine($"  PageNum: {applications.Meta.PageNum}");
        Console.WriteLine($"  PageSize: {applications.Meta.PageSize}");
        Console.WriteLine($"  ObjectTotal: {applications.Meta.ObjectTotal}");
        Console.WriteLine($"  SortField: {applications.Meta.SortField}");
        Console.WriteLine($"  SortOrder: {applications.Meta.SortOrder}");
        Console.WriteLine($"Data:");
        foreach (var app in applications.Data)
        {
            Console.WriteLine($"- Id: {app.Id}");
            Console.WriteLine($"  Name: {app.Name}");
            Console.WriteLine($"  Status: {app.Status}");
            Console.WriteLine($"  PublicUrl: {app.PublicUrl}");
            Console.WriteLine($"  CreatedAt: {app.CreatedAt}");
        }
    }

    public async Task CreateApplicationAsync()
    {
        var body = new CreateApplicationBody
        {
            Name = "YOUR_APPLICATION_NAME", // アプリケーション名を指定
            TimeoutSeconds = 60,
            Port = 8080,
            MinScale = 0,
            MaxScale = 10,
            ScaleTargetConcurrency = 100,
            Components = new List<CreateApplicationBody_components>
            {
                new CreateApplicationBody_components
                {
                    Name = "YOUR_COMPONENT_NAME", // コンポーネント名を指定
                    MaxCpu = CreateApplicationBody_components_max_cpu.ZeroFive,
                    MaxMemory = CreateApplicationBody_components_max_memory.OneGi,
                    DeploySource = new CreateApplicationBody_components_deploy_source
                    {
                        ContainerRegistry = new CreateApplicationBody_components_deploy_source_container_registry
                        {
                            Image = "YOUR_CONTAINER_IMAGE", // コンテナイメージを指定
                            Server = "YOUR_REGISTRY_SERVER", // コンテナレジストリのサーバー名を指定
                            Username = "YOUR_REGISTRY_USERNAME", // コンテナレジストリのユーザー名を指定
                            Password = "YOUR_REGISTRY_PASSWORD" // コンテナレジストリのパスワードを指定
                        }
                    },
                    Env = new List<EnvItem>
                    {
                        new EnvItem
                        {
                            Key = "YOUR_ENV_VAR_NAME", // 環境変数名を指定
                            Value = "YOUR_ENV_VAR_VALUE" // 環境変数の値を指定
                        }
                    },
                    Secret = new List<CreateApplicationBody_components_secret>
                    {
                        new CreateApplicationBody_components_secret
                        {
                            Key = "YOUR_SECRET_NAME", // シークレット名を指定
                            Value = "YOUR_SECRET_VALUE" // シークレット値を指定
                        }
                    },
                    Probe = new CreateApplicationBody_components_probe
                    {
                        HttpGet = new CreateApplicationBody_components_probe_http_get
                        {
                            Path = "/health", // ヘルスチェック用のパスを指定
                            Port = 8080,
                            Headers = new List<CreateApplicationBody_components_probe_http_get_headers>
                            {
                                new CreateApplicationBody_components_probe_http_get_headers
                                {
                                    Name = "YOUR_HEADER_NAME", // ヘッダー名を指定
                                    Value = "YOUR_HEADER_VALUE" // ヘッダー値を指定
                                }
                            }
                        }
                    }
                }
            }
        };
        var application = await _client.Applications.PostAsync(body);

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS));

        Console.WriteLine("\n作成されたアプリケーション:");
        Console.WriteLine($"ID: {application.Id}");
        Console.WriteLine($"Name: {application.Name}");
        Console.WriteLine($"CreatedAt: {application.CreatedAt}");
        Console.WriteLine($"PublicUrl: {application.PublicUrl}");
        Console.WriteLine($"ResourceId: {application.ResourceId}");
        Console.WriteLine($"TimeoutSeconds: {application.TimeoutSeconds}");
        Console.WriteLine($"MinScale: {application.MinScale}");
        Console.WriteLine($"MaxScale: {application.MaxScale}");
        Console.WriteLine($"Port: {application.Port}");
        Console.WriteLine($"ScaleTargetConcurrency: {application.ScaleTargetConcurrency}");
        Console.WriteLine($"Status: {application.Status}");
        Console.WriteLine("Components:");
        foreach (var component in application.Components)
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

    public async Task GetApplicationDetailsAsync()
    {
        Console.Write("詳細を取得するアプリケーションID: ");
        var id = Console.ReadLine();

        var application = await _client.Applications[id].GetAsync();

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS));

        Console.WriteLine("\nアプリケーション詳細:");
        Console.WriteLine($"ID: {application.Id}");
        Console.WriteLine($"Name: {application.Name}");
        Console.WriteLine($"CreatedAt: {application.CreatedAt}");
        Console.WriteLine($"PublicUrl: {application.PublicUrl}");
        Console.WriteLine($"ResourceId: {application.ResourceId}");
        Console.WriteLine($"TimeoutSeconds: {application.TimeoutSeconds}");
        Console.WriteLine($"MinScale: {application.MinScale}");
        Console.WriteLine($"MaxScale: {application.MaxScale}");
        Console.WriteLine($"Port: {application.Port}");
        Console.WriteLine($"ScaleTargetConcurrency: {application.ScaleTargetConcurrency}");
        Console.WriteLine($"Status: {application.Status}");
        Console.WriteLine("Components:");
        foreach (var component in application.Components)
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

    public async Task UpdateApplicationAsync()
    {
        Console.Write("変更するアプリケーションID: ");
        var id = Console.ReadLine();

        var body = new PatchApplicationBody
        {
            TimeoutSeconds = 60,
            Port = 8080,
            MinScale = 1,
            MaxScale = 10,
            ScaleTargetConcurrency = 100,
            AllTrafficAvailable = false,
            Components = new List<PatchApplicationBody_components>
            {
                new PatchApplicationBody_components
                {
                    Name = "YOUR_COMPONENT_NAME", // コンポーネント名を指定
                    MaxCpu = PatchApplicationBody_components_max_cpu.ZeroFive,
                    MaxMemory = PatchApplicationBody_components_max_memory.OneGi,
                    DeploySource = new PatchApplicationBody_components_deploy_source
                    {
                        ContainerRegistry = new PatchApplicationBody_components_deploy_source_container_registry
                        {
                            Image = "YOUR_IMAGE_NAME", // コンテナイメージを指定
                            Server = "YOUR_REGISTRY_SERVER", // コンテナレジストリのサーバー名を指定
                            Username = "YOUR_REGISTRY_USERNAME", // コンテナレジストリのユーザー名を指定
                            Password = "YOUR_REGISTRY_PASSWORD", // コンテナレジストリのパスワードを指定
                            Action = ContainerRegistryAction.Keep
                        }
                    },
                    Env = new List<EnvItem>
                    {
                        new EnvItem
                        {
                            Key = "YOUR_ENV_VAR_NAME", // 環境変数名を指定
                            Value = "YOUR_ENV_VAR_VALUE" // 環境変数の値を指定
                        }
                    },
                    Secret = new List<PatchApplicationBody_components_secret>
                    {
                        new PatchApplicationBody_components_secret
                        {
                            Key = "YOUR_SECRET_KEY", // シークレット名を指定
                            Value = "YOUR_SECRET_VALUE" // シークレット値を指定
                        }
                    },
                    Probe = new PatchApplicationBody_components_probe
                    {
                        HttpGet = new PatchApplicationBody_components_probe_http_get
                        {
                            Path = "/health", // ヘルスチェック用のパスを指定
                            Port = 8080,
                            Headers = new List<PatchApplicationBody_components_probe_http_get_headers>
                            {
                                new PatchApplicationBody_components_probe_http_get_headers
                                {
                                    Name = "YOUR_HEADER_NAME", // ヘッダー名を指定
                                    Value = "YOUR_HEADER_VALUE" // ヘッダー値を指定
                                }
                            }
                        }
                    }
                }
            }
        };
        var application = await _client.Applications[id].PatchAsync(body);

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS));

        Console.WriteLine("\n変更したアプリケーション:");
        Console.WriteLine($"ID: {application.Id}");
        Console.WriteLine($"Name: {application.Name}");
        Console.WriteLine($"UpdatedAt: {application.UpdatedAt}");
        Console.WriteLine($"PublicUrl: {application.PublicUrl}");
        Console.WriteLine($"ResourceId: {application.ResourceId}");
        Console.WriteLine($"TimeoutSeconds: {application.TimeoutSeconds}");
        Console.WriteLine($"MinScale: {application.MinScale}");
        Console.WriteLine($"MaxScale: {application.MaxScale}");
        Console.WriteLine($"Port: {application.Port}");
        Console.WriteLine($"ScaleTargetConcurrency: {application.ScaleTargetConcurrency}");
        Console.WriteLine($"Status: {application.Status}");
        Console.WriteLine("Components:");
        foreach (var component in application.Components)
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

    public async Task DeleteApplicationAsync()
    {
        Console.Write("削除するアプリケーションIDを入力してください: ");
        var id = Console.ReadLine();
        await _client.Applications[id].DeleteAsync();

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS));

        Console.WriteLine($"\nアプリケーションを削除しました: {id}");
    }

    public async Task GetApplicationStatusAsync()
    {
        Console.Write("ステータスを取得するアプリケーションIDを入力してください: ");
        var id = Console.ReadLine();
        var status = await _client.Applications[id].Status.GetAsync();

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS));

        Console.WriteLine("\nアプリケーションステータス詳細:");
        if (status.Status.HasValue)
        {
            Console.WriteLine($"ステータス: {status.Status.Value}");
        }
        else
        {
            Console.WriteLine("ステータスが見つかりませんでした。");
        }

        Console.WriteLine($"メッセージ: {status.Message}");
    }
}
