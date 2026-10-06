using Sacloud.Sdk.APIGateway.Models;
using Sacloud.Sdk.APIGateway.Samples.Extensions;

namespace Sacloud.Sdk.APIGateway.Samples.Samples;

class ServiceSample(APIGatewayClient client) : ISample
{
    private readonly APIGatewayClient client = client
        ?? throw new ArgumentNullException(nameof(client));

    private bool isCreatedSubscription = false;

    private Guid subscriptionId = Guid.Empty;
    private Guid oidcId = Guid.Empty;
    private Guid groupId = Guid.Empty;
    private Guid serviceId = Guid.Empty;
    private Guid routeId = Guid.Empty;

    private Guid domainId = Guid.Empty;
    private string domainName = string.Empty;

    private ServiceRouteSample? serviceRouteSample = null;
    private ServiceRouteAuthorizationSample? serviceRouteAuthorizationSample = null;
    private ServiceRouteRequestSample? serviceRouteRequestSample = null;
    private ServiceRouteResponseSample? serviceRouteResponseSample = null;

    public string Description { get; } = "サービスカテゴリのサンプルを実行";

    public async Task RunAsync()
    {
        Console.WriteLine("\nサービスのサンプルを実行します");
        try
        {
            // サンプルで利用する依存リソースの作成
            Console.WriteLine("\n---------[事前準備]---------");

            // サンプル用契約の選択
            Console.WriteLine("\n既存の契約一覧を取得中...");
            var listSubscriptionsResponse = await client.Subscriptions.GetAsync();
            if (listSubscriptionsResponse.Apigw.Subscriptions is null || listSubscriptionsResponse.Apigw.Subscriptions.Count <= 0)
            {
                throw new NullReferenceException("契約一覧照会APIのレスポンスに契約情報が含まれていません。");
            }
            Console.WriteLine("\n---------[契約の選択]---------");

            Console.WriteLine("  0. 新規に契約する");
            for (int i = 1; i <= listSubscriptionsResponse.Apigw.Subscriptions.Count; i++)
            {
                Console.WriteLine($"  {i}. {listSubscriptionsResponse.Apigw.Subscriptions[i - 1].Name} ");
            }
            Console.Write("\nサンプル実行で使用する契約IDを入力してください（ cancel でキャンセル）: ");

            var input = Console.ReadLine();
            if (string.Equals(input, "cancel", StringComparison.OrdinalIgnoreCase)) return;
            if (!int.TryParse(input, out int choice)
                || choice < 0
                || choice > listSubscriptionsResponse.Apigw.Subscriptions.Count)
            {
                Console.WriteLine("\n無効な番号が選択されました。");
                return;
            }
            if (choice is 0)
            {
                isCreatedSubscription = true;

                Console.WriteLine("\nサンプル用契約の作成に使用する有効なプランを取得中...");
                var listPlansResponse = await client.Plans.GetAsync();
                var firstPlan = listPlansResponse.Apigw.Plans?.FirstOrDefault()
                    ?? throw new NullReferenceException("利用可能なプランが見つかりませんでした。");
                var planId = firstPlan.Id
                    ?? throw new NullReferenceException("プラン一覧照会APIのレスポンスにプランIDが含まれていません。");

                Console.WriteLine("利用可能なプランが見つかりました。");

                Console.WriteLine("\nサンプル用契約の契約中...");
                var subscriptionName = "sample-service-subscription";
                await client.Subscriptions.PostAsync(new SubscriptionCreate
                {
                    Name = subscriptionName,
                    PlanId = planId
                });

                Console.WriteLine("サービスカテゴリのサンプル用契約を契約しました。");

                Console.WriteLine("\nサンプル用契約の契約IDを取得中...");
                listSubscriptionsResponse = await client.Subscriptions.GetAsync();
                var createdSubscription = listSubscriptionsResponse.Apigw.Subscriptions?
                    .FirstOrDefault(x => x.Name == subscriptionName && x.PlanId == planId)
                    ?? throw new NullReferenceException("作成した契約が契約一覧に見つかりませんでした。");
                subscriptionId = createdSubscription.Id
                    ?? throw new NullReferenceException("契約一覧照会APIのレスポンスに契約IDが含まれていません。");

                Console.WriteLine("サービス登録APIに使用する契約IDを取得しました。");
            }
            else
            {
                subscriptionId = listSubscriptionsResponse.Apigw.Subscriptions[choice - 1].Id
                    ?? throw new NullReferenceException("契約一覧照会APIのレスポンスに契約IDが含まれていません。");
            }

            // サンプル用OIDC認証設定の作成
            Console.WriteLine("\nサンプル用OIDC認証設定の作成中...");
            var createOidcResponse = await client.Oidc.PostAsync(new Models.Oidc
            {
                Name = "sample-service-oidc",
                AuthenticationMethods = [Oidc_authenticationMethods.AccessToken],
                Issuer = "https://example.com",
                ClientId = "sample-service-client-id",
                ClientSecret = "sample-service-client-secret",
                Scopes = ["openid", "profile"],
                HideCredentials = false,
                TokenAudiences = ["sample-service-audience"],
                UseSession = false
            });
            oidcId = createOidcResponse.Apigw.Oidc.Id
                ?? throw new NullReferenceException("OIDC認証登録APIのレスポンスにOIDC認証IDが含まれていません。");

            Console.WriteLine("サンプル用OIDC認証設定を作成しました。");

            // サンプル用グループの作成
            Console.WriteLine("\nサンプル用グループの作成中...");
            var createGroupResponse = await client.Groups.PostAsync(new Group
            {
                Name = "sample-service-group",
                Tags = ["Sample", "CreatedByServiceSample"]
            });
            groupId = createGroupResponse.Apigw.Group.Id
                ?? throw new NullReferenceException("Group登録APIのレスポンスにグループIDが含まれていません。");

            Console.WriteLine("サンプル用グループを作成しました。");

            // サンプル用ドメインの作成
            Console.WriteLine("\nPOST /domains | Domain登録API");
            var createDomainResponse = await client.Domains.PostAsync(new Domain
            {
                DomainName = "example.com"
            });
            domainId = createDomainResponse.Apigw.Domain.Id
                ?? throw new NullReferenceException("Domain登録APIのレスポンスにドメインIDが含まれていません。");
            domainName = createDomainResponse.Apigw.Domain.DomainName
                ?? throw new NullReferenceException("Domain登録APIのレスポンスにドメイン名が含まれていません。");

            Console.WriteLine("サンプル用ドメインを作成しました。");

            // サンプルの実行
            Console.WriteLine("\n---------[サービスサンプル]--------");

            var serviceName = "sample-service";

            // POST サービスを登録する
            Console.WriteLine("\nPOST /services | Service登録API");
            var createServiceResponse = await client.Services.PostAsync(new ServiceDetailRequest
            {
                Name = serviceName,
                Tags = ["Sample", "CreatedByAPI"],
                Protocol = Service_protocol.Https,
                Host = domainName,
                Path = "/",
                Port = 443,
                Retries = 1,
                ConnectTimeout = 60000,
                WriteTimeout = 60000,
                ReadTimeout = 60000,
                Authentication = Service_authentication.Oidc,
                Oidc = new OidcSummary
                {
                    Id = oidcId
                },
                CorsConfig = new CorsConfig
                {
                    Credentials = true,
                    AccessControlAllowOrigins = "https://example.com",
                    AccessControlAllowMethods = [HTTPMethod.GET, HTTPMethod.POST],
                    AccessControlAllowHeaders = "Content-Type,Authorization",
                    AccessControlExposedHeaders = "X-Request-Id",
                    MaxAge = 300,
                    PreflightContinue = false,
                    PrivateNetwork = false
                },
                ObjectStorageConfig = new ObjectStorageConfig
                {
                    BucketName = "sample-service-bucket",
                    Endpoint = "example.com",
                    Region = "jp-north-1",
                    AccessKeyID = "sample-access-key-id",
                    SecretAccessKey = "sample-secret-access-key",
                    FolderName = "sample-folder",
                    UseDocumentIndex = true
                },
                Subscription = new ServiceSubscriptionRequest
                {
                    Id = subscriptionId
                }
            });
            var createdService = createServiceResponse.Apigw.Service;
            serviceId = createdService.Id
                ?? throw new NullReferenceException("サービス登録APIのレスポンスにサービスIDが含まれていません。");

            Console.WriteLine("サービスの登録が完了しました。");
            WriteServiceCommon(createdService, 2);
            Console.WriteLine("  - Subscription:");
            if (createdService.Subscription is null)
            {
                Console.WriteLine("    - (null)");
            }
            else
            {
                Console.WriteLine($"    - Id: {createdService.Subscription.Id.FormatNullable()}");
            }

            // GET サービス一覧の取得
            Console.WriteLine("\nGET /services | Service一覧照会API");
            var listServicesResponse = await client.Services.GetAsync();

            Console.WriteLine("サービス一覧の取得が完了しました。");
            Console.WriteLine("  - Services:");
            if (listServicesResponse.Apigw.Services is null || listServicesResponse.Apigw.Services.Count <= 0)
            {
                Console.WriteLine("    - (null)");
            }
            else
            {
                foreach (var listedService in listServicesResponse.Apigw.Services)
                {
                    WriteServiceDetailResponse(listedService, 4);
                }
            }

            // GET 指定したサービスの詳細情報を取得する
            Console.WriteLine("\nGET /services/{serviceId} | Service詳細照会API");
            var getServiceResponse = await client.Services[serviceId].GetAsync();

            Console.WriteLine("指定したサービスの詳細情報の取得が完了しました。");
            WriteServiceDetailResponse(getServiceResponse.Apigw.Service, 2);

            // PUT 指定したサービスを更新する
            Console.WriteLine("\nPUT /services/{serviceId} | Service更新API");
            await client.Services[serviceId].PutAsync(new ServiceDetail
            {
                Name = $"updated-{serviceName}",
                Tags = ["Sample", "UpdatedByAPI"],
                Protocol = Service_protocol.Https,
                Host = domainName,
                Path = "/updated",
                Port = 443,
                Retries = 2,
                ConnectTimeout = 60000,
                WriteTimeout = 60000,
                ReadTimeout = 60000,
                Authentication = Service_authentication.Oidc,
                Oidc = new OidcSummary
                {
                    Id = oidcId
                },
                CorsConfig = new CorsConfig
                {
                    Credentials = true,
                    AccessControlAllowOrigins = "https://example.com",
                    AccessControlAllowMethods = [HTTPMethod.GET, HTTPMethod.PUT],
                    AccessControlAllowHeaders = "Content-Type,Authorization",
                    AccessControlExposedHeaders = "X-Request-Id,X-Trace-Id",
                    MaxAge = 600,
                    PreflightContinue = false,
                    PrivateNetwork = false
                },
                ObjectStorageConfig = new ObjectStorageConfig
                {
                    BucketName = "sample-service-bucket",
                    Endpoint = "example.com",
                    Region = "jp-north-1",
                    AccessKeyID = "updated-sample-access-key-id",
                    SecretAccessKey = "updated-sample-secret-access-key",
                    FolderName = "updated-sample-folder",
                    UseDocumentIndex = true
                }
            });

            Console.WriteLine("指定したサービスの更新が完了しました。(レスポンスボディ無し)");

            // 子リソースのサンプル準備
            serviceRouteSample = new ServiceRouteSample(client, serviceId, domainName);
            serviceRouteAuthorizationSample = new ServiceRouteAuthorizationSample(client);
            serviceRouteRequestSample = new ServiceRouteRequestSample(client);
            serviceRouteResponseSample = new ServiceRouteResponseSample(client);

            // /services/{serviceId}/routes のサンプルを実行する
            routeId = await serviceRouteSample.RunAsync();

            // /services/{serviceId}/routes/{routeId}/authorization のサンプルを実行する
            await serviceRouteAuthorizationSample.RunAsync(serviceId, routeId, groupId);

            // /services/{serviceId}/routes/{routeId}/request のサンプルを実行する
            await serviceRouteRequestSample.RunAsync(serviceId, routeId);

            // /services/{serviceId}/routes/{routeId}/response のサンプルを実行する
            await serviceRouteResponseSample.RunAsync(serviceId, routeId);
        }
        finally
        {
            // DELETE 指定したルートを削除する
            if (serviceRouteSample is not null)
            {
                await serviceRouteSample.DeleteAsync();
            }

            // DELETE 指定したサービスを削除する
            if (serviceId != Guid.Empty)
            {
                Console.WriteLine("\nDELETE /services/{serviceId} | Service削除API");
                await client.Services[serviceId].DeleteAsync();

                Console.WriteLine("指定したサービスの削除が完了しました。(レスポンスボディ無し)");
            }

            // サービスサンプルで利用した依存リソースの削除
            Console.WriteLine("\n---------[クリーンアップ]---------");

            // サンプル用契約の解約
            if (isCreatedSubscription && subscriptionId != Guid.Empty)
            {
                Console.WriteLine("\nサンプル用契約の解約中...");
                await client.Subscriptions[subscriptionId].DeleteAsync();
                Console.WriteLine("サンプル用契約の解約が完了しました。");
            }

            // サンプル用OIDC認証設定の削除
            if (oidcId != Guid.Empty)
            {
                Console.WriteLine("\nサンプル用OIDC認証設定の削除中...");
                await client.Oidc[oidcId].DeleteAsync();
                Console.WriteLine("サンプル用OIDC認証設定の削除が完了しました。");
            }

            // サンプル用グループの削除
            if (groupId != Guid.Empty)
            {
                Console.WriteLine("\nサンプル用グループの削除中...");
                await client.Groups[groupId].DeleteAsync();
                Console.WriteLine("サンプル用グループの削除が完了しました。");
            }

            // サンプル用ドメインの削除
            if (domainId != Guid.Empty)
            {
                Console.WriteLine("\nサンプル用ドメインの削除中...");
                await client.Domains[domainId].DeleteAsync();
                Console.WriteLine("サンプル用ドメインの削除が完了しました。");
            }

            Console.WriteLine("\nサービスのサンプルの実行が完了しました");
        }
    }

    private void WriteServiceDetailResponse(ServiceDetailResponse service, int indent = 2)
    {
        WriteServiceCommon(service, indent);
        Console.WriteIndentedLine("- Subscription:", indent + 2);
        if (service.Subscription is null)
        {
            Console.WriteIndentedLine("- (null)", indent + 4);
        }
        else
        {
            Console.WriteIndentedLine($"- Id: {service.Subscription.Id.FormatNullable()}", indent + 4);
            Console.WriteIndentedLine($"- Name: {service.Subscription.Name.FormatNullable()}", indent + 4);
        }
    }

    private void WriteServiceCommon(Service service, int indent = 2)
    {
        Console.WriteIndentedLine($"- Id: {service.Id.FormatNullable()}", indent);
        Console.WriteIndentedLine($"- Name: {service.Name.FormatNullable()}", indent + 2);
        Console.WriteIndentedLine($"- Host: {service.Host.FormatNullable()}", indent + 2);
        Console.WriteIndentedLine($"- Path: {service.Path.FormatNullable()}", indent + 2);
        Console.WriteIndentedLine($"- Protocol: {service.Protocol.FormatNullable()}", indent + 2);
        Console.WriteIndentedLine($"- Port: {service.Port.FormatNullable()}", indent + 2);
        Console.WriteIndentedLine($"- Authentication: {service.Authentication.FormatNullable()}", indent + 2);

        Console.WriteIndentedLine("- Oidc:", indent + 2);
        if (service.Oidc is null)
        {
            Console.WriteIndentedLine("- (null)", indent + 4);
        }
        else
        {
            Console.WriteIndentedLine($"- Id: {service.Oidc.Id.FormatNullable()}", indent + 4);
            Console.WriteIndentedLine($"- Name: {service.Oidc.Name.FormatNullable()}", indent + 4);
        }

        var corsConfig = (service as ServiceDetail)?.CorsConfig;
        Console.WriteIndentedLine("- CorsConfig:", indent + 2);
        if (corsConfig is null)
        {
            Console.WriteIndentedLine("- (null)", indent + 4);
        }
        else
        {
            Console.WriteIndentedLine($"- Credentials: {corsConfig.Credentials.FormatNullable()}", indent + 4);
            Console.WriteIndentedLine($"- AccessControlAllowOrigins: {corsConfig.AccessControlAllowOrigins.FormatNullable()}", indent + 4);
            Console.WriteIndentedLine($"- AccessControlAllowHeaders: {corsConfig.AccessControlAllowHeaders.FormatNullable()}", indent + 4);
            Console.WriteIndentedLine($"- AccessControlExposedHeaders: {corsConfig.AccessControlExposedHeaders.FormatNullable()}", indent + 4);
            Console.WriteIndentedLine($"- MaxAge: {corsConfig.MaxAge.FormatNullable()}", indent + 4);
            Console.WriteIndentedLine($"- PreflightContinue: {corsConfig.PreflightContinue.FormatNullable()}", indent + 4);
            Console.WriteIndentedLine($"- PrivateNetwork: {corsConfig.PrivateNetwork.FormatNullable()}", indent + 4);

            Console.WriteIndentedLine("- AccessControlAllowMethods:", indent + 4);
            if (corsConfig.AccessControlAllowMethods is null || corsConfig.AccessControlAllowMethods.Count <= 0)
            {
                Console.WriteIndentedLine("- (null)", indent + 6);
            }
            else
            {
                foreach (var method in corsConfig.AccessControlAllowMethods)
                {
                    Console.WriteIndentedLine($"- {method.FormatNullable()}", indent + 6);
                }
            }
        }

        var objectStorageConfig = (service as ServiceDetail)?.ObjectStorageConfig;
        Console.WriteIndentedLine("- ObjectStorageConfig:", indent + 2);
        if (objectStorageConfig is null)
        {
            Console.WriteIndentedLine("- (null)", indent + 4);
        }
        else
        {
            Console.WriteIndentedLine($"- BucketName: {objectStorageConfig.BucketName.FormatNullable()}", indent + 4);
            Console.WriteIndentedLine($"- Endpoint: {objectStorageConfig.Endpoint.FormatNullable()}", indent + 4);
            Console.WriteIndentedLine($"- Region: {objectStorageConfig.Region.FormatNullable()}", indent + 4);
            Console.WriteIndentedLine($"- AccessKeyID: {objectStorageConfig.AccessKeyID.FormatNullable()}", indent + 4);
            Console.WriteIndentedLine($"- SecretAccessKey: {objectStorageConfig.SecretAccessKey.FormatNullable()}", indent + 4);
            Console.WriteIndentedLine($"- FolderName: {objectStorageConfig.FolderName.FormatNullable()}", indent + 4);
            Console.WriteIndentedLine($"- UseDocumentIndex: {objectStorageConfig.UseDocumentIndex.FormatNullable()}", indent + 4);
        }

        Console.WriteIndentedLine($"- Retries: {service.Retries.FormatNullable()}", indent + 2);
        Console.WriteIndentedLine($"- ConnectTimeout: {service.ConnectTimeout.FormatNullable()}", indent + 2);
        Console.WriteIndentedLine($"- WriteTimeout: {service.WriteTimeout.FormatNullable()}", indent + 2);
        Console.WriteIndentedLine($"- ReadTimeout: {service.ReadTimeout.FormatNullable()}", indent + 2);
        Console.WriteIndentedLine($"- RouteHost: {service.RouteHost.FormatNullable()}", indent + 2);
        Console.WriteIndentedLine($"- CreatedAt: {service.CreatedAt.FormatNullable()}", indent + 2);
        Console.WriteIndentedLine($"- UpdatedAt: {service.UpdatedAt.FormatNullable()}", indent + 2);

        Console.WriteIndentedLine("- Tags:", indent + 2);
        if (service.Tags is null || service.Tags.Count <= 0)
        {
            Console.WriteIndentedLine("- (null)", indent + 4);
        }
        else
        {
            foreach (var tag in service.Tags)
            {
                Console.WriteIndentedLine($"- {tag.FormatNullable()}", indent + 4);
            }
        }
    }
}
