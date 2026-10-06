namespace Sacloud.Sdk.AppRunShared.Samples.Operations;

public sealed class UserOperations(AppRunSharedClient client)
{
    private readonly AppRunSharedClient _client = client;

    public async Task GetUserInfoAsync()
    {
        var userInfo = await _client.User.GetAsync();
        Console.WriteLine("ユーザー情報:");

        if (userInfo?.Limit is not null)
        {
            Console.WriteLine($"プロジェクトで作成できるアプリケーションの上限: {userInfo.Limit.ApplicationCount}");

        }
        else
        {
            Console.WriteLine("利用上限情報が見つかりませんでした。");
        }
    }

    public async Task CreateUserAsync()
    {
        var userInfo = await _client.User.PostAsync();
        Console.WriteLine("ユーザー情報:");

        if (userInfo?.Limit is not null)
        {
            Console.WriteLine($"プロジェクトで作成できるアプリケーションの上限: {userInfo.Limit.ApplicationCount}");

        }
        else
        {
            Console.WriteLine("利用上限情報が見つかりませんでした。");
        }
    }
}
