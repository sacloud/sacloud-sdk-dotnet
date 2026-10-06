using Sacloud.Sdk.APIGateway.Models;
using Sacloud.Sdk.APIGateway.Samples.Extensions;

namespace Sacloud.Sdk.APIGateway.Samples.Samples;

class OidcSample(APIGatewayClient client) : ISample
{
    private readonly APIGatewayClient client = client
        ?? throw new ArgumentNullException(nameof(client));

    private Guid oidcId = Guid.Empty;

    public string Description { get; } = "OIDCカテゴリのサンプルを実行";

    public async Task RunAsync()
    {
        Console.WriteLine("\nOIDC認証のサンプルを実行します");
        try
        {
            // サンプルの実行
            Console.WriteLine("\n---------[OIDC認証サンプル]---------");

            // POST OIDC認証を登録する
            Console.WriteLine("\nPOST /oidc | OIDC認証登録API");
            var createOidcResponse = await client.Oidc.PostAsync(new Models.Oidc
            {
                Name = "sample-oidc",
                AuthenticationMethods = [Oidc_authenticationMethods.AccessToken],
                Issuer = "https://issuer.example.com",
                ClientId = "sample-client-id",
                ClientSecret = "sample-client-secret",
                Scopes = ["openid", "profile"],
                HideCredentials = false,
                TokenAudiences = ["sample-audience"],
                UseSession = false
            });
            var oidc = createOidcResponse.Apigw.Oidc;
            oidcId = oidc.Id
                ?? throw new NullReferenceException("OIDC認証登録APIのレスポンスにOIDC認証IDが含まれていません。");

            Console.WriteLine("OIDC認証の登録が完了しました。");
            WriteOidc(oidc, 2);

            // GET OIDC認証一覧を取得する
            Console.WriteLine("\nGET /oidc | OIDC認証一覧照会API");
            var listOidcResponse = await client.Oidc.GetAsync();

            Console.WriteLine("OIDC認証一覧の取得が完了しました。");
            Console.WriteLine("  - Oidcs:");
            if (listOidcResponse.Apigw.Oidcs is null || listOidcResponse.Apigw.Oidcs.Count <= 0)
            {
                Console.WriteLine("    - (null)");
            }
            else
            {
                foreach (var listedOidc in listOidcResponse.Apigw.Oidcs)
                {
                    WriteOidc(listedOidc, 4);
                }
            }

            // GET 指定したOIDC認証の詳細情報を取得する
            Console.WriteLine("\nGET /oidc/{oidcId} | OIDC認証詳細照会API");
            var getOidcResponse = await client.Oidc[oidcId].GetAsync();
            var oidcDetail = getOidcResponse.Apigw.Oidc;

            Console.WriteLine("指定したOIDC認証の詳細情報の取得が完了しました。");
            WriteOidc(oidcDetail, 2);
            Console.WriteLine("    - Services:");
            if (oidcDetail.Services is null || oidcDetail.Services.Count <= 0)
            {
                Console.WriteLine("      - (null)");
            }
            else
            {
                foreach (var service in oidcDetail.Services)
                {
                    Console.WriteLine($"      - Id: {service.Id.FormatNullable()}");
                    Console.WriteLine($"        - Name: {service.Name.FormatNullable()}");
                }
            }

            // PUT 指定したOIDC認証を更新する
            Console.WriteLine("\nPUT /oidc/{oidcId} | OIDC認証更新API");
            await client.Oidc[oidcId].PutAsync(new Models.Oidc
            {
                Name = "updated-sample-oidc",
                AuthenticationMethods = [Oidc_authenticationMethods.AuthorizationCodeFlow],
                Issuer = "https://updated-issuer.example.com",
                ClientId = "updated-sample-client-id",
                ClientSecret = "updated-sample-client-secret",
                Scopes = ["openid", "email"],
                HideCredentials = true,
                TokenAudiences = ["updated-sample-audience"],
                UseSession = true
            });

            Console.WriteLine("指定したOIDC認証の更新が完了しました。(レスポンスボディ無し)");
        }
        finally
        {
            // DELETE 指定したOIDC認証を削除する
            if (oidcId != Guid.Empty)
            {
                Console.WriteLine("\nDELETE /oidc/{oidcId} | OIDC認証削除API");
                await client.Oidc[oidcId].DeleteAsync();
                Console.WriteLine("指定したOIDC認証の削除が完了しました。(レスポンスボディ無し)");
            }

            Console.WriteLine("\nOIDC認証のサンプルの実行が完了しました");
        }
    }

    private void WriteOidc(Models.Oidc oidc, int indent = 2)
    {
        Console.WriteIndentedLine($"- Id: {oidc.Id.FormatNullable()}", indent);
        Console.WriteIndentedLine($"- Name: {oidc.Name.FormatNullable()}", indent + 2);
        Console.WriteIndentedLine($"- Issuer: {oidc.Issuer.FormatNullable()}", indent + 2);
        Console.WriteIndentedLine($"- ClientId: {oidc.ClientId.FormatNullable()}", indent + 2);
        Console.WriteIndentedLine($"- HideCredentials: {oidc.HideCredentials.FormatNullable()}", indent + 2);
        Console.WriteIndentedLine($"- UseSession: {oidc.UseSession.FormatNullable()}", indent + 2);
        Console.WriteIndentedLine($"- CreatedAt: {oidc.CreatedAt.FormatNullable()}", indent + 2);
        Console.WriteIndentedLine($"- UpdatedAt: {oidc.UpdatedAt.FormatNullable()}", indent + 2);
        Console.WriteIndentedLine("- AuthenticationMethods:", indent + 2);
        if (oidc.AuthenticationMethods is null || oidc.AuthenticationMethods.Count <= 0)
        {
            Console.WriteIndentedLine("- (null)", indent + 4);
        }
        else
        {
            foreach (var method in oidc.AuthenticationMethods)
            {
                Console.WriteIndentedLine($"- {method.FormatNullable()}", indent + 4);
            }
        }
        WriteStringList("Scopes", oidc.Scopes, indent + 2);
        WriteStringList("TokenAudiences", oidc.TokenAudiences, indent + 2);
    }

    private void WriteStringList(string title, List<string>? values, int indent = 2)
    {
        Console.WriteIndentedLine($"- {title}:", indent);
        if (values is null || values.Count <= 0)
        {
            Console.WriteIndentedLine("- (null)", indent + 2);
        }
        else
        {
            foreach (var value in values)
            {
                Console.WriteIndentedLine($"- {value.FormatNullable()}", indent + 2);
            }
        }
    }

};
