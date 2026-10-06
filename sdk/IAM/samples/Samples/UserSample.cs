using Sacloud.Sdk.IAM.Compat.Users;
using Sacloud.Sdk.IAM.Compat.Users.Item;
using Sacloud.Sdk.IAM.Compat.Users.Item.RegisterEmail;
using Sacloud.Sdk.IAM.Samples.Helpers;

namespace Sacloud.Sdk.IAM.Samples.Samples;

class UserSample(IAMClient client)
{
    private readonly IAMClient client = client
        ?? throw new ArgumentNullException(nameof(client));

    public async Task RunAsync()
    {
        Console.WriteLine("ユーザのサンプルを実行します");

        // -------------------------------------------------------
        // POST ユーザーを作成する
        Console.WriteLine("POST /compat/users | ユーザーを作成する");
        var createUserResponse = await client.Compat.Users.PostAsync(new UsersPostRequestBody
        {
            Name = "iam-sample-user",
            Password = PasswordHelper.GeneratePassword(),
            Code = "iam-sample-user",
            Description = "SDKからサンプルユーザ",
        });
        var userId = createUserResponse.Id
            ?? throw new InvalidOperationException("ユーザーIDがレスポンスに含まれていませんでした");
        Console.WriteLine($"  - 作成したユーザーID: {createUserResponse.Id}");
        Console.WriteLine($"    - Name: {createUserResponse.Name}");
        Console.WriteLine($"    - Code: {createUserResponse.Code}");
        Console.WriteLine($"    - Description: {createUserResponse.Description}");

        // -------------------------------------------------------
        // GET ユーザー一覧を取得する
        Console.WriteLine("GET /compat/users | ユーザー一覧を取得する");
        var listUsersResponse = await client.Compat.Users.GetAsync();
        Console.WriteLine($"  - 取得したユーザー数: {listUsersResponse.Items?.Count ?? 0}");

        // -------------------------------------------------------
        // GET 指定したユーザーを取得する
        Console.WriteLine($"GET /compat/users/{userId} | 指定したユーザーを取得する");
        var getUserResponse = await client.Compat.Users[userId].GetAsync();
        Console.WriteLine($"  - 取得したユーザーID: {getUserResponse.Id}");
        Console.WriteLine($"    - Name: {getUserResponse.Name}");
        Console.WriteLine($"    - Code: {getUserResponse.Code}");
        Console.WriteLine($"    - Description: {getUserResponse.Description}");

        // -------------------------------------------------------
        // PUT 指定したユーザーを更新する
        Console.WriteLine($"PUT /compat/users/{userId} | 指定したユーザーを更新する");
        var updateUserResponse = await client.Compat.Users[userId].PutAsync(new WithUser_PutRequestBody
        {
            Name = "updated-iam-sample-user",
            Description = "SDKから更新したサンプルユーザ",
        });
        Console.WriteLine($"  - 更新したユーザーID: {updateUserResponse.Id}");
        Console.WriteLine($"    - Name: {updateUserResponse.Name}");
        Console.WriteLine($"    - Description: {updateUserResponse.Description}");

        // -------------------------------------------------------
        // POST ユーザにメールアドレスを登録する
        Console.WriteLine($"POST /compat/users/{userId}/register-email | ユーザにメールアドレスを登録する");
        await client.Compat.Users[userId].RegisterEmail.PostAsync(new RegisterEmailPostRequestBody()
        {
            Email = "sample@example.com",
        });
        Console.WriteLine("  - メールアドレスの登録が完了しました (レスポンスボディ無し)");

        // -------------------------------------------------------
        // POST ユーザのメールアドレスを削除する
        Console.WriteLine($"DELETE /compat/users/{userId}/unregister-email | ユーザのメールアドレスを削除する");
        await client.Compat.Users[userId].UnregisterEmail.PostAsync();
        Console.WriteLine("  - メールアドレスの削除が完了しました (レスポンスボディ無し)");

        // -------------------------------------------------------
        // DELETE 指定したユーザーを削除する
        Console.WriteLine($"DELETE /compat/users/{userId} | 指定したユーザーを削除する");
        await client.Compat.Users[userId].DeleteAsync();
        Console.WriteLine("  - ユーザーの削除が完了しました (レスポンスボディ無し)");
        
        Console.WriteLine("ユーザのサンプルの実行が完了しました");
    }
}
