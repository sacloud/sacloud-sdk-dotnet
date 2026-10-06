using Sacloud.Sdk.APIGateway.Models;
using Sacloud.Sdk.APIGateway.Samples.Extensions;

namespace Sacloud.Sdk.APIGateway.Samples.Samples;

class ServiceRouteRequestSample(APIGatewayClient client)
{
    private readonly APIGatewayClient client = client
        ?? throw new ArgumentNullException(nameof(client));

    public async Task RunAsync(Guid serviceId, Guid routeId)
    {
        // PUT 指定したサービスのルートのリクエスト変換情報を更新する
        Console.WriteLine("\nPUT /services/{serviceId}/routes/{routeId}/request | Route Request変換情報更新API");
        await client.Services[serviceId].Routes[routeId].Request.PutAsync(new RequestTransformation
        {
            HttpMethod = HTTPMethod.PUT,
            Allow = new RequestAllowDetail
            {
                Body = ["request.id", "request.type", "payload.message"]
            },
            Remove = new RequestRemoveDetail
            {
                HeaderKeys = ["X-Remove-Header"],
                QueryParams = ["removeQuery"],
                Body = ["payload.internal"]
            },
            Rename = new RequestRenameDetail
            {
                Headers =
                [
                    new RequestRenameDetail_headers
                    {
                        From = "X-Request-Id",
                        To = "X-Correlation-Id"
                    }
                ],
                QueryParams =
                [
                    new RequestRenameDetail_queryParams
                    {
                        From = "userId",
                        To = "memberId"
                    }
                ],
                Body =
                [
                    new RequestRenameDetail_body
                    {
                        From = "payload.message",
                        To = "payload.content"
                    }
                ]
            },
            Replace = new RequestModificationDetail
            {
                Headers =
                [
                    new RequestModificationDetail_headers
                    {
                        Key = "X-Api-Version",
                        Value = "2026-06"
                    }
                ],
                QueryParams =
                [
                    new RequestModificationDetail_queryParams
                    {
                        Key = "locale",
                        Value = "ja-JP"
                    }
                ],
                Body =
                [
                    new RequestModificationDetail_body
                    {
                        Key = "payload.priority",
                        Value = "high"
                    }
                ]
            },
            Add = new RequestModificationDetail
            {
                Headers =
                [
                    new RequestModificationDetail_headers
                    {
                        Key = "X-Added-Header",
                        Value = "added"
                    }
                ],
                QueryParams =
                [
                    new RequestModificationDetail_queryParams
                    {
                        Key = "trace",
                        Value = "enable"
                    }
                ],
                Body =
                [
                    new RequestModificationDetail_body
                    {
                        Key = "payload.addedKey",
                        Value = "addedValue"
                    }
                ]
            },
            Append = new RequestModificationDetail
            {
                Headers =
                [
                    new RequestModificationDetail_headers
                    {
                        Key = "X-Append-Header",
                        Value = "|append"
                    }
                ],
                QueryParams =
                [
                    new RequestModificationDetail_queryParams
                    {
                        Key = "tags",
                        Value = "sample"
                    }
                ],
                Body =
                [
                    new RequestModificationDetail_body
                    {
                        Key = "payload.comment",
                        Value = "-appended"
                    }
                ]
            }
        });

        Console.WriteLine("指定したルートのリクエスト変換情報の更新が完了しました。(レスポンスボディ無し)");

        // GET 指定したサービスのルートのリクエスト変換情報を取得する
        Console.WriteLine("\nGET /services/{serviceId}/routes/{routeId}/request | Route Request変換情報取得API");
        var getRequestTransformationResponse = await client.Services[serviceId].Routes[routeId].Request.GetAsync();
        var transformation = getRequestTransformationResponse.Apigw.RequestTransformation;

        Console.WriteLine("指定したルートのリクエスト変換情報の取得が完了しました。");
        Console.WriteLine("  - RequestTransformation:");
        if (transformation is null)
        {
            Console.WriteLine("    - (null)");
        }
        else
        {
            Console.WriteLine($"    - HttpMethod: {transformation.HttpMethod.FormatNullable()}");
            WriteStringList("Allow.Body", transformation.Allow?.Body, 4);
            WriteStringList("Remove.HeaderKeys", transformation.Remove?.HeaderKeys, 4);
            WriteStringList("Remove.QueryParams", transformation.Remove?.QueryParams, 4);
            WriteStringList("Remove.Body", transformation.Remove?.Body, 4);
        }
    }

    private void WriteStringList(string title, List<string>? values, int indent = 2)
    {
        Console.WriteIndentedLine($"- {title}:", indent);
        if (values is null || values.Count <= 0)
        {
            Console.WriteIndentedLine("- (null)", indent + 2);
            return;
        }

        foreach (var value in values)
        {
            Console.WriteIndentedLine($"- {value.FormatNullable()}", indent + 2);
        }
    }
}
