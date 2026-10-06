namespace Sacloud.Sdk.ObjectStorage.Samples.Samples;

sealed class QuotaSample(ObjectStorageClient client) : AbstractSample(client)
{
    // GET クォータ情報の取得
    public async Task GetQuotaAsync()
    {
        Console.WriteLine("\nGET /quota | クォータ情報の取得\n");

        var response = await client.Quota.GetAsync();

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機

        var quota = response?.Data
            ?? throw new InvalidOperationException("クォータ情報(data)が null です。");
        Console.WriteLine($"NumRootKeys: {quota.NumRootKeys}");
        Console.WriteLine($"NumBuckets: {quota.NumBuckets}");
        Console.WriteLine($"NumPermissions: {quota.NumPermissions}");
        Console.WriteLine($"NumKeysPerPermission: {quota.NumKeysPerPermission}");
        Console.WriteLine($"NumBucketsPerPermission: {quota.NumBucketsPerPermission}");
        Console.WriteLine($"NumObjectsPerBucket: {quota.NumObjectsPerBucket}");
        Console.WriteLine($"AmountGibPerBucket: {quota.AmountGibPerBucket}");
    }
}
