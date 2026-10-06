namespace Sacloud.Sdk.AppRunDedicated.Samples;

static class LimitSample
{
    public static async Task RunSampleAsync(AppRunDedicatedClient client)
    {
        // GET プロジェクトのリソース制限値の取得
        var getLimitResponse = await client.Limits.GetAsync();
        var limit = getLimitResponse.Limit
            ?? throw new InvalidOperationException("LimitがNullです");
        Console.WriteLine($"プロジェクトのリソース制限値を取得しました: ApplicationCount={limit.ApplicationCount}, ClusterCount={limit.ClusterCount}");
    }
}
