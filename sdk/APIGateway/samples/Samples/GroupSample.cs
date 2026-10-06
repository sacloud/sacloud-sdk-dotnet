using Sacloud.Sdk.APIGateway.Models;
using Sacloud.Sdk.APIGateway.Samples.Extensions;

namespace Sacloud.Sdk.APIGateway.Samples.Samples;

class GroupSample(APIGatewayClient client) : ISample
{
    private readonly APIGatewayClient client = client
        ?? throw new ArgumentNullException(nameof(client));

    private Guid groupId = Guid.Empty;

    public string Description { get; } = "グループカテゴリのサンプルを実行";

    public async Task RunAsync()
    {
        Console.WriteLine("\nグループのサンプルを実行します");
        try
        {
            // サンプルの実行
            Console.WriteLine("\n---------[グループサンプル]---------");

            // POST グループを登録する
            Console.WriteLine("\nPOST /groups | Group登録API");
            var createGroupResponse = await client.Groups.PostAsync(new Group
            {
                Name = "sample-group",
                Tags = ["Sample", "CreatedByAPI"]
            });
            groupId = createGroupResponse.Apigw.Group.Id
                ?? throw new NullReferenceException("グループ登録APIのレスポンスにグループIDが含まれていません。");

            Console.WriteLine("グループの登録が完了しました。");
            WriteGroupDetails(createGroupResponse.Apigw.Group, 2);

            // GET グループ一覧を取得する
            Console.WriteLine("\nGET /groups | Group一覧照会API");
            var listGroupsResponse = await client.Groups.GetAsync();

            Console.WriteLine("グループ一覧の取得が完了しました。");
            Console.WriteLine("  - Groups:");
            foreach (var group in listGroupsResponse.Apigw.Groups)
            {
                WriteGroupDetails(group, 4);
            }

            // GET 指定したグループの詳細情報を取得する
            Console.WriteLine("\nGET /groups/{groupId} | Group詳細照会API");
            var getGroupResponse = await client.Groups[groupId].GetAsync();

            Console.WriteLine("指定したグループの詳細情報の取得が完了しました。");
            WriteGroupDetails(getGroupResponse.Apigw.Group, 2);

            // PUT 指定したグループの情報を更新する
            Console.WriteLine("\nPUT /groups/{groupId} | Group更新API");
            await client.Groups[groupId].PutAsync(new Group
            {
                Name = "updated-sample-group",
                Tags = ["Sample", "UpdatedByAPI"]
            });

            Console.WriteLine("指定したグループの更新が完了しました。(レスポンスボディ無し)");
        }
        finally
        {
            // DELETE 指定したグループを削除する
            if (groupId != Guid.Empty)
            {
                Console.WriteLine("\nDELETE /groups/{groupId} | Group削除API");
                await client.Groups[groupId].DeleteAsync();
                Console.WriteLine("指定したグループの削除が完了しました。(レスポンスボディ無し)");
            }

            Console.WriteLine("\nグループのサンプルの実行が完了しました");
        }
    }

    private void WriteGroupDetails(Group group, int indent = 2)
    {
        Console.WriteIndentedLine($"- Id: {group.Id.FormatNullable()}", indent);
        Console.WriteIndentedLine($"- Name: {group.Name.FormatNullable()}", indent + 2);
        Console.WriteIndentedLine($"- CreatedAt: {group.CreatedAt.FormatNullable()}", indent + 2);
        Console.WriteIndentedLine($"- UpdatedAt: {group.UpdatedAt.FormatNullable()}", indent + 2);
        Console.WriteIndentedLine("- Tags:", indent + 2);
        if (group.Tags is null || group.Tags.Count <= 0)
        {
            Console.WriteIndentedLine("- (null)", indent + 4);
        }
        else
        {
            foreach (var tag in group.Tags)
            {
                Console.WriteIndentedLine($"- {tag.FormatNullable()}", indent + 4);
            }
        }
    }
};
