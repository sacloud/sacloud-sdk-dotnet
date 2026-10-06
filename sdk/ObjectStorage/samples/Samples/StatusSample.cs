namespace Sacloud.Sdk.ObjectStorage.Samples.Samples;

sealed class StatusSample(ObjectStorageClient client) : AbstractSample(client)
{
    // GET ステータス情報の取得
    public async Task GetStatusAsync()
    {
        Console.WriteLine("\nGET /status | ステータス情報の取得\n");

        var response = await client.Status.GetAsync();

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機

        var status = response?.Data
            ?? throw new InvalidOperationException("ステータス情報(data)が null です。");
        Console.WriteLine($"AcceptNew: {status.AcceptNew}");
        Console.WriteLine($"Message: {status.Message}");
        Console.WriteLine($"StartedAt: {status.StartedAt}");
        Console.WriteLine($"StatusCode:");
        if (status.StatusCode is not null)
        {
            Console.WriteLine($"  Id: {status.StatusCode.Id}");
            Console.WriteLine($"  Status: {status.StatusCode.Status}");
        }
    }
}
