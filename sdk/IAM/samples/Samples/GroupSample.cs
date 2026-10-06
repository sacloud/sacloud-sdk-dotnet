
using Sacloud.Sdk.IAM.Groups;
using Sacloud.Sdk.IAM.Groups.Item.Memberships;

namespace Sacloud.Sdk.IAM.Samples.Samples;

class GroupSample(IAMClient client)
{
    private readonly IAMClient client = client
        ?? throw new ArgumentNullException(nameof(client));

    public async Task RunAsync(long userId)
    {
        Console.WriteLine("グループのサンプルを実行します");

        // -------------------------------------------------------
        // POST グループを作成する
        Console.WriteLine("POST /groups | グループを作成する");
        var createGroupResponse = await client.Groups.PostAsync(new GroupsPostRequestBody
        {
            Name = "sdk-sample-group",
            Description = "SDKから作成したサンプルグループ",
        });
        var groupId = createGroupResponse.Id
            ?? throw new InvalidOperationException("グループの作成に失敗しました");
        Console.WriteLine($"  - 作成したグループID: {createGroupResponse.Id}");
        Console.WriteLine($"    - Name: {createGroupResponse.Name}");

        // -------------------------------------------------------
        // GET 指定したグループを取得する
        Console.WriteLine($"GET /groups/{groupId} | 指定したグループを取得する");
        var getGroupResponse = await client.Groups[groupId].GetAsync();
        Console.WriteLine($"  - 取得したグループID: {getGroupResponse.Id}");
        Console.WriteLine($"    - Name: {getGroupResponse.Name}");

        // -------------------------------------------------------
        // PUT 指定したグループを更新する
        Console.WriteLine($"PUT /groups/{groupId} | 指定したグループを更新する");
        var updateGroupResponse = await client.Groups[groupId].PutAsync(new Groups.Item.WithGroup_PutRequestBody
        {
            Name = getGroupResponse.Name,
            Description = getGroupResponse.Description,
        });
        Console.WriteLine($"  - 更新したグループID: {updateGroupResponse.Id}");
        Console.WriteLine($"    - Name: {updateGroupResponse.Name}");

        // -------------------------------------------------------
        // GET グループ一覧を取得する
        Console.WriteLine("GET /groups | グループ一覧を取得する");
        var listGroupsResponse = await client.Groups.GetAsync();
        Console.WriteLine($"  - 取得したグループ数: {listGroupsResponse.Items.Count}");

        // -------------------------------------------------------
        // PUT グループの所属情報を更新する
        Console.WriteLine($"PUT /groups/{groupId}/memberships | グループの所属情報を更新する");
        var updateGroupMembersResponse = await client.Groups[groupId].Memberships.PutAsync(new MembershipsPutRequestBody
        {
            CompatUsers =
            [
                new MembershipsPutRequestBody_compat_users
                {
                    Id = userId,
                }
            ]
        });
        Console.WriteLine("  - 更新後のグループ所属ユーザー一覧:");
        if (updateGroupMembersResponse.CompatUsers is not null)
        {
            foreach (var member in updateGroupMembersResponse.CompatUsers)
            {
                Console.WriteLine($"    - ユーザーID: {member.Id}");
            }
        }
        else
        {
            Console.WriteLine("    - 所属ユーザーが存在しません");
        }

        // -------------------------------------------------------
        // GET グループの所属情報を取得する
        Console.WriteLine($"GET /groups/{groupId}/memberships | グループの所属情報を取得する");
        var getGroupMembersResponse = await client.Groups[groupId].Memberships.GetAsync();
        Console.WriteLine("  - グループ所属ユーザー一覧:");
        if (getGroupMembersResponse.CompatUsers is not null)
        {
            foreach (var member in getGroupMembersResponse.CompatUsers)
            {
                Console.WriteLine($"    - ユーザーID: {member.Id}");
            }
        }
        else
        {
            Console.WriteLine("    - 所属ユーザーが存在しません");
        }

        // -------------------------------------------------------
        // DELETE 指定したグループを削除する
        Console.WriteLine($"DELETE /groups/{groupId} | 指定したグループを削除する");
        await client.Groups[groupId].DeleteAsync();
        Console.WriteLine("  - グループの削除が完了しました (レスポンスボディ無し)");

        Console.WriteLine("グループのサンプルの実行が完了しました");
    }
}
