using Sacloud.Sdk.APIGateway.Samples.Extensions;

namespace Sacloud.Sdk.APIGateway.Samples.Samples;

class UserSample(APIGatewayClient client) : ISample
{
    private readonly APIGatewayClient client = client
        ?? throw new ArgumentNullException(nameof(client));

    private Guid groupId = Guid.Empty;
    private Guid userId = Guid.Empty;

    public string Description { get; } = "ユーザーカテゴリのサンプルを実行";

    public async Task RunAsync()
    {
        Console.WriteLine("\nユーザーのサンプルを実行します");
        try
        {
            // サンプルで利用する依存リソースの作成
            Console.WriteLine("\n---------[事前準備]--------");

            // サンプル用グループの作成
            Console.WriteLine("\nサンプル用グループの作成中...");
            var createGroupResponse = await client.Groups.PostAsync(new Models.Group
            {
                Name = "sample-group",
                Tags = ["Sample", "CreatedByAPI"]
            });
            groupId = createGroupResponse.Apigw.Group.Id
                ?? throw new NullReferenceException("グループ登録APIのレスポンスにグループIDが含まれていません。");

            Console.WriteLine("サンプル用グループの作成が完了しました。");

            // サンプルの実行
            Console.WriteLine("\n---------[ユーザーサンプル]--------");

            // POST ユーザーを登録する
            Console.WriteLine("\nPOST /users | User登録API");
            var createUserResponse = await client.Users.PostAsync(new Models.UserDetail
            {
                Name = "sample-user",
                CustomID = "sample-user-custom-id",
                Tags = ["Sample", "CreatedByAPI"],
                IpRestrictionConfig = new Models.IpRestrictionConfig
                {
                    Protocols = Models.IpRestrictionConfig_protocols.Https,
                    RestrictedBy = Models.IpRestrictionConfig_restrictedBy.DenyIps,
                    Ips = ["0.0.0.0"]
                }
            });
            userId = createUserResponse.Apigw.User.Id
                ?? throw new NullReferenceException("ユーザー登録APIのレスポンスにユーザーIDが含まれていません。");

            Console.WriteLine("ユーザーの登録が完了しました。");
            WriteUserDetails(createUserResponse.Apigw.User, 2);

            // GET ユーザー一覧を取得する
            Console.WriteLine("\nGET /users | User一覧照会API");
            var listUsersResponse = await client.Users.GetAsync();

            Console.WriteLine("ユーザー一覧の取得が完了しました。");
            Console.WriteLine("  - Users:");
            if (listUsersResponse.Apigw.Users is null || listUsersResponse.Apigw.Users.Count <= 0)
            {
                Console.WriteLine("    - (null)");
            }
            else
            {
                foreach (var user in listUsersResponse.Apigw.Users)
                {
                    Console.WriteLine($"    - Id: {user.Id.FormatNullable()}");
                    Console.WriteLine($"      - Name: {user.Name.FormatNullable()}");
                    Console.WriteLine($"      - CreatedAt: {user.CreatedAt.FormatNullable()}");
                    Console.WriteLine($"      - UpdatedAt: {user.UpdatedAt.FormatNullable()}");
                    Console.WriteLine($"      - CustomId: {user.CustomID.FormatNullable()}");
                    WriteTags(user.Tags, 6);
                    WriteGroups(user.Groups, 6);
                }
            }

            // GET 指定したユーザーの詳細情報を取得する
            Console.WriteLine("\nGET /users/{userId} | User詳細照会API");
            var getUserResponse = await client.Users[userId].GetAsync();

            Console.WriteLine("指定したユーザーの詳細情報の取得が完了しました。");
            Console.WriteLine("  - User:");
            WriteUserDetails(getUserResponse.Apigw.User, 4);

            // PUT 指定したユーザーの情報を更新する
            Console.WriteLine($"\nPUT /users/{userId} | User更新API");
            await client.Users[userId].PutAsync(new Models.UserDetail
            {
                Name = "updated-sample-user",
                CustomID = "updated-sample-user-custom-id",
                Tags = ["Sample", "UpdatedByAPI"],
                IpRestrictionConfig = new Models.IpRestrictionConfig
                {
                    Protocols = Models.IpRestrictionConfig_protocols.Https,
                    RestrictedBy = Models.IpRestrictionConfig_restrictedBy.AllowIps,
                    Ips = ["0.0.0.0"]
                }
            });

            Console.WriteLine("ユーザーの更新が完了しました。 (レスポンスボディ無し)");

            // PUT 指定したユーザーの所属グループを更新する
            Console.WriteLine($"\nPUT /users/{userId}/groups | User所属Group更新API");
            await client.Users[userId].Groups.PutAsync(
            [
                new() {
                    UserGroupDetailUpdateMember1 = new Models.UserGroupDetailUpdateMember1
                    {
                        IsAssigned = true,
                        Id = groupId
                    }
                }
            ]);

            Console.WriteLine("ユーザーの所属グループの更新が完了しました。 (レスポンスボディ無し)");

            // GET 指定したユーザーの所属グループ一覧を取得する
            Console.WriteLine($"\nGET /users/{userId}/groups | User所属Group照会API");
            var listUserGroupsResponse = await client.Users[userId].Groups.GetAsync();

            Console.WriteLine("指定したユーザーの所属グループ一覧の取得が完了しました。");
            Console.WriteLine("  - Groups:");
            foreach (var group in listUserGroupsResponse.Apigw.Groups)
            {
                Console.WriteLine($"    - Id: {group.Id.FormatNullable()}");
                Console.WriteLine($"      - Name: {group.Name.FormatNullable()}");
                Console.WriteLine($"      - IsAssigned: {group.IsAssigned.FormatNullable()}");
            }

            // PUT 指定したユーザーの認証情報を更新する
            Console.WriteLine($"\nPUT /users/{userId}/credentials | User認証情報更新API");
            await client.Users[userId].Authentication.PutAsync(new Models.UserAuthentication
            {
                BasicAuth = new Models.BasicAuth
                {
                    UserName = "sample-basic-username",
                    Password = "sample-basic-password"
                },
                Jwt = new Models.Jwt
                {
                    Key = "sample-jwt-key",
                    Secret = "sample-jwt-secret",
                    Algorithm = Models.Jwt_algorithm.HS256
                },
                HmacAuth = new Models.HmacAuth
                {
                    UserName = "sample-hmac-username",
                    Secret = "sample-hmac-secret"
                }
            });

            Console.WriteLine("ユーザーの認証情報の更新が完了しました。 (レスポンスボディ無し)");

            // GET 指定したユーザーの認証情報を取得する
            Console.WriteLine($"\nGET /users/{userId}/credentials | User認証情報照会API");
            var getUserAuthenticationResponse = await client.Users[userId].Authentication.GetAsync();
            var userAuthentication = getUserAuthenticationResponse.Apigw.UserAuthentication
                ?? throw new NullReferenceException("ユーザー認証情報照会APIのレスポンスにユーザー認証情報が含まれていません。");

            Console.WriteLine("指定したユーザーの認証情報の取得が完了しました。");
            Console.WriteLine("  - UserAuthentication:");
            if (userAuthentication.BasicAuth is null)
            {
                Console.WriteLine("    - BasicAuth: (null)");
            }
            else
            {
                Console.WriteLine("    - BasicAuth:");
                WriteModels(userAuthentication.BasicAuth, 6);
                Console.WriteLine($"      - UserName: {userAuthentication.BasicAuth.UserName.FormatNullable()}");
            }
            if (userAuthentication.Jwt is null)
            {
                Console.WriteLine("    - Jwt: (null)");
            }
            else
            {
                Console.WriteLine("    - Jwt:");
                WriteModels(userAuthentication.Jwt, 6);
                Console.WriteLine($"      - Key: {userAuthentication.Jwt.Key.FormatNullable()}");
                Console.WriteLine($"      - Algorithm: {userAuthentication.Jwt.Algorithm.FormatNullable()}");
            }
            if (userAuthentication.HmacAuth is null)
            {
                Console.WriteLine("    - HmacAuth: (null)");
            }
            else
            {
                Console.WriteLine("    - HmacAuth:");
                WriteModels(userAuthentication.HmacAuth, 6);
                Console.WriteLine($"      - UserName: {userAuthentication.HmacAuth.UserName.FormatNullable()}");
            }
        }
        finally
        {
            // DELETE 指定したユーザーを削除する
            if (userId != Guid.Empty)
            {
                Console.WriteLine($"\nDELETE /users/{userId} | 指定したユーザーを削除する");
                await client.Users[userId].DeleteAsync();

                Console.WriteLine("指定したユーザーの削除が完了しました。 (レスポンスボディ無し)");
            }

            Console.WriteLine("\n---------[クリーンアップ]---------");

            // サンプル用グループの削除
            if (groupId != Guid.Empty)
            {
                Console.WriteLine($"\nサンプル用グループの削除中...");
                await client.Groups[groupId].DeleteAsync();

                Console.WriteLine("サンプル用グループの削除が完了しました。");
            }

            Console.WriteLine("\nユーザーのサンプルの実行が完了しました");
        }
    }

    private void WriteModels(Models.Model model, int indent = 2)
    {
        Console.WriteIndentedLine($"- Id: {model.Id.FormatNullable()}", indent);
        Console.WriteIndentedLine($"- CreatedAt: {model.CreatedAt.FormatNullable()}", indent);
        Console.WriteIndentedLine($"- UpdatedAt: {model.UpdatedAt.FormatNullable()}", indent);
    }

    private void WriteUserDetails(Models.UserDetail user, int indent = 2)
    {
        Console.WriteIndentedLine($"- Id: {user.Id.FormatNullable()}", indent);
        Console.WriteIndentedLine($"- Name: {user.Name.FormatNullable()}", indent + 2);
        Console.WriteIndentedLine($"- CustomId: {user.CustomID.FormatNullable()}", indent + 2);
        Console.WriteIndentedLine($"- CreatedAt: {user.CreatedAt.FormatNullable()}", indent + 2);
        Console.WriteIndentedLine($"- UpdatedAt: {user.UpdatedAt.FormatNullable()}", indent + 2);
        WriteTags(user.Tags, indent + 2);
        WriteGroups(user.Groups, indent + 2);
        Console.WriteIndentedLine("- IpRestrictionConfig:", indent + 2);
        if (user.IpRestrictionConfig is null)
        {
            Console.WriteIndentedLine("- (null)", indent + 4);
        }
        else
        {
            Console.WriteIndentedLine($"- Protocols: {user.IpRestrictionConfig.Protocols.FormatNullable()}", indent + 4);
            Console.WriteIndentedLine($"- RestrictedBy: {user.IpRestrictionConfig.RestrictedBy.FormatNullable()}", indent + 4);
            Console.WriteIndentedLine("- Ips:", indent + 4);
            if (user.IpRestrictionConfig.Ips is null || user.IpRestrictionConfig.Ips.Count <= 0)
            {
                Console.WriteIndentedLine("- (null)", indent + 6);
            }
            else
            {
                foreach (var ip in user.IpRestrictionConfig.Ips)
                {
                    Console.WriteIndentedLine($"- {ip.FormatNullable()}", indent + 6);
                }
            }
        }
    }

    private void WriteTags(List<string>? tags, int indent = 2)
    {
        Console.WriteIndentedLine("- Tags:", indent);
        if (tags is null || tags.Count <= 0)
        {
            Console.WriteIndentedLine("- (null)", indent + 2);
        }
        else
        {
            foreach (var tag in tags)
            {
                Console.WriteIndentedLine($"- {tag.FormatNullable()}", indent + 2);
            }
        }
    }

    private void WriteGroups(List<Models.Group>? groups, int indent = 2)
    {
        Console.WriteIndentedLine("- Groups:", indent);
        if (groups is null || groups.Count <= 0)
        {
            Console.WriteIndentedLine("- (null)", indent + 2);
        }
        else
        {
            foreach (var group in groups)
            {
                Console.WriteIndentedLine($"- Id: {group.Id.FormatNullable()}", indent + 2);
                Console.WriteIndentedLine($"  - Name: {group.Name.FormatNullable()}", indent + 4);
                Console.WriteIndentedLine($"  - CreatedAt: {group.CreatedAt.FormatNullable()}", indent + 4);
                Console.WriteIndentedLine($"  - UpdatedAt: {group.UpdatedAt.FormatNullable()}", indent + 4);
                WriteTags(group.Tags, indent + 4);
            }
        }
    }
}
