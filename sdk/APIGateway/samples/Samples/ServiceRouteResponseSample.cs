using Sacloud.Sdk.APIGateway.Models;
using Sacloud.Sdk.APIGateway.Samples.Extensions;

namespace Sacloud.Sdk.APIGateway.Samples.Samples;

class ServiceRouteResponseSample(APIGatewayClient client)
{
    private readonly APIGatewayClient client = client
        ?? throw new ArgumentNullException(nameof(client));

    public async Task RunAsync(Guid serviceId, Guid routeId)
    {
        // PUT 指定したサービスのルートのレスポンス変換情報を更新する
        Console.WriteLine("\nPUT /services/{serviceId}/routes/{routeId}/response | Route Response変換情報更新API");
        await client.Services[serviceId].Routes[routeId].Response.PutAsync(new ResponseTransformation
        {
            Allow = new ResponseAllowDetail
            {
                JsonKeys = ["result.code", "result.message", "result.data"]
            },
            Remove = new ResponseRemoveDetail
            {
                IfStatusCode = [400, 500],
                HeaderKeys = ["X-Internal-Header"],
                JsonKeys = ["result.debug"]
            },
            Rename = new ResponseRenameDetail
            {
                IfStatusCode = [200],
                Headers =
                [
                    new ResponseRenameDetail_headers
                    {
                        From = "X-Request-Id",
                        To = "X-Response-Id"
                    }
                ],
                Json =
                [
                    new ResponseRenameDetail_json
                    {
                        From = "result.message",
                        To = "result.description"
                    }
                ]
            },
            Replace = new ResponseReplaceDetail
            {
                IfStatusCode = [200],
                Headers =
                [
                    new ResponseReplaceDetail_headers
                    {
                        Key = "Cache-Control",
                        Value = "no-store"
                    }
                ],
                Json =
                [
                    new ResponseReplaceDetail_json
                    {
                        Key = "result.code",
                        Value = "SUCCESS"
                    }
                ],
                Body = """{"message":"replaced body by sample"}"""
            },
            Add = new ResponseModificationDetail
            {
                IfStatusCode = [201],
                Headers =
                [
                    new ResponseModificationDetail_headers
                    {
                        Key = "X-Added-Response-Header",
                        Value = "added"
                    }
                ],
                Json =
                [
                    new ResponseModificationDetail_json
                    {
                        Key = "result.added",
                        Value = "true"
                    }
                ]
            },
            Append = new ResponseModificationDetail
            {
                IfStatusCode = [200],
                Headers =
                [
                    new ResponseModificationDetail_headers
                    {
                        Key = "X-Append-Response-Header",
                        Value = "|append"
                    }
                ],
                Json =
                [
                    new ResponseModificationDetail_json
                    {
                        Key = "result.message",
                        Value = "-appended"
                    }
                ]
            }
        });

        Console.WriteLine("指定したルートのレスポンス変換情報の更新が完了しました。(レスポンスボディ無し)");

        // GET 指定したサービスのルートのレスポンス変換情報を取得する
        Console.WriteLine("\nGET /services/{serviceId}/routes/{routeId}/response | Route Response変換情報取得API");
        var getResponseTransformationResponse = await client.Services[serviceId].Routes[routeId].Response.GetAsync();
        var transformation = getResponseTransformationResponse.Apigw.ResponseTransformation;

        Console.WriteLine("指定したルートのレスポンス変換情報の取得が完了しました。");
        Console.WriteLine("  - ResponseTransformation:");
        if (transformation is null)
        {
            Console.WriteLine("    - (null)");
            return;
        }
        WriteIntList("Remove.IfStatusCode", transformation.Remove?.IfStatusCode, 4);
        WriteIntList("Rename.IfStatusCode", transformation.Rename?.IfStatusCode, 4);
        WriteIntList("Replace.IfStatusCode", transformation.Replace?.IfStatusCode, 4);
        WriteIntList("Add.IfStatusCode", transformation.Add?.IfStatusCode, 4);
        WriteIntList("Append.IfStatusCode", transformation.Append?.IfStatusCode, 4);
    }

    private void WriteIntList(string title, List<int?>? values, int indent = 2)
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
