using System.Globalization;

namespace Sacloud.Sdk.ObjectStorage.Samples.Samples;

sealed class MeteringBucketSample(ObjectStorageClient client) : AbstractSample(client)
{
    // GET バケットのメータリング情報の取得
    public async Task GetBucketMeteringBucketAsync()
    {
        Console.WriteLine("\nGET /metering/buckets/{bucketName} | バケットのメータリング情報の取得\n");

        var bucketName = ReadRequiredValue("BucketName");
        if (bucketName is null) return;

        var fromInput = ReadRequiredValue("From");
        if (fromInput is null) return;
        if (!DateTime.TryParseExact(fromInput, "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out var from))
        {
            Console.WriteLine("Fromは yyyy-MM 形式で指定してください。");
            return;
        }

        var toInput = ReadRequiredValue("To");
        if (toInput is null) return;
        if (!DateTime.TryParseExact(toInput, "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out var to))
        {
            Console.WriteLine("Toは yyyy-MM 形式で指定してください。");
            return;
        }

        if (from > to)
        {
            Console.WriteLine($"FromはTo以下で指定してください。 From: {from:yyyy-MM}, To: {to:yyyy-MM}");
            return;
        }

        var response = await client.Metering.Buckets[bucketName].GetAsync(config =>
        {
            config.QueryParameters.From = from.ToString("yyyy-MM");
            config.QueryParameters.To = to.ToString("yyyy-MM");
        });

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機

        var billingItems = response?.Data
            ?? throw new InvalidOperationException("バケットメータリング情報(data)が null です。");
        foreach (var item in billingItems)
        {
            Console.WriteLine($"- YearMonth: {item.YearMonth}");
            Console.WriteLine("  Basic:");
            if (item.Basic is not null)
            {
                Console.WriteLine($"    Found: {item.Basic.Found}");
                Console.WriteLine($"    Usage: {item.Basic.Usage}");
                Console.WriteLine($"    Charge: {item.Basic.Charge}");
                Console.WriteLine($"    IsClosing: {item.Basic.IsClosing}");
                Console.WriteLine($"    From: {item.Basic.From}");
                Console.WriteLine($"    To: {item.Basic.To}");
            }
            Console.WriteLine("  StorageUsage:");
            if (item.StorageUsage is not null)
            {
                Console.WriteLine($"    Found: {item.StorageUsage.Found}");
                Console.WriteLine($"    Usage: {item.StorageUsage.Usage}");
                Console.WriteLine($"    Charge: {item.StorageUsage.Charge}");
                Console.WriteLine($"    IsClosing: {item.StorageUsage.IsClosing}");
                Console.WriteLine($"    From: {item.StorageUsage.From}");
                Console.WriteLine($"    To: {item.StorageUsage.To}");
            }
            Console.WriteLine("  NumObjects:");
            if (item.NumObjects is not null)
            {
                Console.WriteLine($"    Found: {item.NumObjects.Found}");
                Console.WriteLine($"    Usage: {item.NumObjects.Usage}");
                Console.WriteLine($"    Charge: {item.NumObjects.Charge}");
                Console.WriteLine($"    IsClosing: {item.NumObjects.IsClosing}");
                Console.WriteLine($"    From: {item.NumObjects.From}");
                Console.WriteLine($"    To: {item.NumObjects.To}");
            }
            Console.WriteLine("  NumTransactions:");
            if (item.NumTransactions is not null)
            {
                Console.WriteLine($"    Found: {item.NumTransactions.Found}");
                Console.WriteLine($"    Usage: {item.NumTransactions.Usage}");
                Console.WriteLine($"    Charge: {item.NumTransactions.Charge}");
                Console.WriteLine($"    IsClosing: {item.NumTransactions.IsClosing}");
                Console.WriteLine($"    From: {item.NumTransactions.From}");
                Console.WriteLine($"    To: {item.NumTransactions.To}");
            }
            Console.WriteLine("  TransferAmount:");
            if (item.TransferAmount is not null)
            {
                Console.WriteLine($"    Found: {item.TransferAmount.Found}");
                Console.WriteLine($"    Usage: {item.TransferAmount.Usage}");
                Console.WriteLine($"    Charge: {item.TransferAmount.Charge}");
                Console.WriteLine($"    IsClosing: {item.TransferAmount.IsClosing}");
                Console.WriteLine($"    From: {item.TransferAmount.From}");
                Console.WriteLine($"    To: {item.TransferAmount.To}");
            }
        }
    }
}
