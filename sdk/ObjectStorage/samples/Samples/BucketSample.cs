using Sacloud.Sdk.ObjectStorage.Models;
using Sacloud.Sdk.ObjectStorage.Models.Handler;
using Sacloud.Sdk.ObjectStorage.Models.Model;

namespace Sacloud.Sdk.ObjectStorage.Samples.Samples;

sealed class BucketSample(ObjectStorageClient client) : AbstractSample(client)
{
    // GET バケット一覧の取得
    public async Task GetBucketsAsync()
    {
        Console.WriteLine("\nGET /buckets | バケット一覧の取得\n");

        var response = await client.Buckets.GetAsync();

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機


        var buckets = response?.Data
            ?? throw new InvalidOperationException("バケット情報(data)が null です。");
        foreach (var bucket in buckets)
        {
            Console.WriteLine($"- ResourceID: {bucket.ResourceId}");
            Console.WriteLine($"  Name: {bucket.Name}");
            if (bucket.Plan is not null)
            {
                Console.WriteLine("  Plan:");
                Console.WriteLine($"    Type: {bucket.Plan.Type}");
                Console.WriteLine($"    ServiceClassPath: {bucket.Plan.ServiceClassPath}");
            }
        }
    }

    // PUT バケットの作成
    public async Task PutBucketAsync()
    {
        Console.WriteLine("\nPUT /buckets/{bucketName} | バケットの作成\n");

        var clusterId = ReadRequiredValue("ClusterId");
        if (clusterId is null) return;

        var bucketName = ReadRequiredValue("BucketName");
        if (bucketName is null) return;

        var serviceClassPath = ReadRequiredValue("ServiceClassPath");
        if (serviceClassPath is null) return;

        var planType = ReadPlanType();
        if (planType is null) return;

        var requestBody = new CreateBucketRequestBody
        {
            ClusterId = clusterId,
            Plan = new CreateBucketRequestBody_plan
            {
                Type = planType,
                ServiceClassPath = serviceClassPath
            }
        };
        var response = await client.Buckets[bucketName].PutAsync(requestBody);

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機

        var bucket = response?.Data
            ?? throw new InvalidOperationException("バケット作成レスポンス(data)が null です。");
        Console.WriteLine($"ClusterID: {bucket.ClusterId}");
        Console.WriteLine($"Name: {bucket.Name}");
        if (bucket.Plan is not null)
        {
            Console.WriteLine("Plan:");
            Console.WriteLine($"  Type: {bucket.Plan.Type}");
            Console.WriteLine($"  ServiceClassPath: {bucket.Plan.ServiceClassPath}");
        }
    }

    // DELETE バケットの削除
    public async Task DeleteBucketAsync()
    {
        Console.WriteLine("\nDELETE /buckets/{bucketName} | バケットの削除\n");

        var bucketName = ReadRequiredValue("BucketName");
        if (bucketName is null) return;

        await client.Buckets[bucketName].DeleteAsync();

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機

        Console.WriteLine($"バケットを削除しました。（レスポンスボディ無し）");
    }

    // GET バケットのレプリケーション設定の取得
    public async Task GetReplicationAsync()
    {
        Console.WriteLine("\nGET /buckets/{bucketName}/replication | バケットのレプリケーション設定の取得\n");

        var bucketName = ReadRequiredValue("BucketName");
        if (bucketName is null) return;

        var response = await client.Buckets[bucketName].Replication.GetAsync();

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機

        var replication = response?.Data
            ?? throw new InvalidOperationException("レプリケーション対象情報(data)が null です。");
        Console.WriteLine("SourceBucket:");
        if (replication.SourceBucket is not null)
        {
            Console.WriteLine($"  Name: {replication.SourceBucket.Name}");
            Console.WriteLine($"  ClusterID: {replication.SourceBucket.ClusterId}");

            if (replication.SourceBucket.Plan is not null)
            {
                Console.WriteLine($"  Plan:");
                Console.WriteLine($"    Type: {replication.SourceBucket.Plan.Type}");
                Console.WriteLine($"    ServiceClassPath: {replication.SourceBucket.Plan.ServiceClassPath}");
            }
        }
        Console.WriteLine("DestBucket:");
        if (replication.DestBucket is not null)
        {
            Console.WriteLine($"  Name: {replication.DestBucket.Name}");
            Console.WriteLine($"  ClusterID: {replication.DestBucket.ClusterId}");

            if (replication.DestBucket.Plan is not null)
            {
                Console.WriteLine($"  Plan:");
                Console.WriteLine($"    Type: {replication.DestBucket.Plan.Type}");
                Console.WriteLine($"    ServiceClassPath: {replication.DestBucket.Plan.ServiceClassPath}");
            }
        }
        Console.WriteLine($"ConfigStatus: {replication.ConfigStatus}");
        Console.WriteLine($"CreatedAt: {replication.CreatedAt}");
    }

    // POST バケットのレプリケーション設定の作成
    public async Task PostReplicationAsync()
    {
        Console.WriteLine("\nPOST /buckets/{bucketName}/replication | バケットのレプリケーション設定の作成\n");

        var bucketName = ReadRequiredValue("SourceBucketName");
        if (bucketName is null) return;

        var destBucket = ReadRequiredValue("DestBucketName");
        if (destBucket is null) return;

        var replication = new CreateBucketReplication
        {
            DestBucket = destBucket
        };
        var response = await client.Buckets[bucketName].Replication.PostAsync(replication);

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機

        var result = response?.Data
            ?? throw new InvalidOperationException("レプリケーション対象情報(data)が null です。");
        Console.WriteLine("SourceBucket:");
        if (result.SourceBucket is not null)
        {
            Console.WriteLine($"  Name: {result.SourceBucket.Name}");
            Console.WriteLine($"  ClusterID: {result.SourceBucket.ClusterId}");

            if (result.SourceBucket.Plan is not null)
            {
                Console.WriteLine($"  Plan:");
                Console.WriteLine($"    Type: {result.SourceBucket.Plan.Type}");
                Console.WriteLine($"    ServiceClassPath: {result.SourceBucket.Plan.ServiceClassPath}");
            }
        }
        Console.WriteLine("DestBucket:");
        if (result.DestBucket is not null)
        {
            Console.WriteLine($"  Name: {result.DestBucket.Name}");
            Console.WriteLine($"  ClusterID: {result.DestBucket.ClusterId}");

            if (result.DestBucket.Plan is not null)
            {
                Console.WriteLine($"  Plan:");
                Console.WriteLine($"    Type: {result.DestBucket.Plan.Type}");
                Console.WriteLine($"    ServiceClassPath: {result.DestBucket.Plan.ServiceClassPath}");
            }
        }
        Console.WriteLine($"ConfigStatus: {result.ConfigStatus}");
        Console.WriteLine($"CreatedAt: {result.CreatedAt}");
    }

    // DELETE レプリケーション設定の削除
    public async Task DeleteReplicationAsync()
    {
        Console.WriteLine("\nDELETE /buckets/{bucketName}/replication | バケットのレプリケーション設定の削除\n");

        var bucketName = ReadRequiredValue("BucketName");
        if (bucketName is null) return;

        await client.Buckets[bucketName].Replication.DeleteAsync();

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機

        Console.WriteLine("レプリケーション対象情報を削除しました。（レスポンスボディ無し）");
    }

    // GET バケットのレプリケーション設定可能なディスティネーションバケットを取得
    public async Task GetBucketReplicableTargetsAsync()
    {
        Console.WriteLine("\nGET /buckets/{bucketName}/replicable-targets | バケットのレプリケーション設定可能なディスティネーションバケットを取得\n");

        var bucketName = ReadRequiredValue("BucketName");
        if (bucketName is null) return;

        var response = await client.Buckets[bucketName].ReplicableTargets.GetAsync();

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機

        var replicableTargets = response?.Data
            ?? throw new InvalidOperationException("バケット複製対象情報(data)が null です。");
        foreach (var target in replicableTargets)
        {
            Console.WriteLine($"- ResourceID: {target.ResourceId}");
            Console.WriteLine($"  Name: {target.Name}");

            if (target.Plan is not null)
            {
                Console.WriteLine($"  Plan:");
                Console.WriteLine($"    Type: {target.Plan.Type}");
                Console.WriteLine($"    ServiceClassPath: {target.Plan.ServiceClassPath}");
            }
        }
    }

    // GET バケット暗号化設定の取得
    public async Task GetBucketEncryptionAsync()
    {
        Console.WriteLine("\nGET /buckets/{bucketName}/encryption | バケット暗号化設定の取得\n");

        var bucketName = ReadRequiredValue("BucketName");
        if (bucketName is null) return;

        var response = await client.Buckets[bucketName].Encryption.GetAsync();

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機

        var encryption = response?.Data
            ?? throw new InvalidOperationException("バケット暗号化情報(data)が null です。");
        Console.WriteLine($"KmsKeyId: {encryption.KmsKeyId}");
        Console.WriteLine($"ConfiguredAt: {encryption.ConfiguredAt}");
    }

    // PUT バケット暗号化設定の作成（KMSキー指定）
    public async Task PutBucketEncryptionAsync()
    {
        Console.WriteLine("\nPUT /buckets/{bucketName}/encryption | バケット暗号化設定の作成（KMSキー指定）\n");

        var bucketName = ReadRequiredValue("BucketName");
        if (bucketName is null) return;

        var kmsKeyId = ReadRequiredValue("KmsKeyId");
        if (kmsKeyId is null) return;

        var requestBody = new UpdateBucketEncryptionRequestBody
        {
            KmsKeyId = kmsKeyId
        };
        var response = await client.Buckets[bucketName].Encryption.PutAsync(requestBody);

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機

        Console.WriteLine($"KmsKeyId: {response?.Data?.KmsKeyId}");
        Console.WriteLine($"ConfiguredAt: {response?.Data?.ConfiguredAt}");
    }

    // DELETE バケット暗号化設定の削除（無効化）
    public async Task DeleteBucketEncryptionAsync()
    {
        Console.WriteLine("\nDELETE /buckets/{bucketName}/encryption | バケット暗号化設定の削除（無効化）\n");

        var bucketName = ReadRequiredValue("BucketName");
        if (bucketName is null) return;

        await client.Buckets[bucketName].Encryption.DeleteAsync();

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機

        Console.WriteLine("バケット暗号化設定を削除しました。（レスポンスボディ無し）");
    }

    // GET 現在のプランを取得
    public async Task GetBucketPlanAsync()
    {
        Console.WriteLine("\nGET /buckets/{bucketName}/plan | 現在のプランを取得\n");

        var bucketName = ReadRequiredValue("BucketName");
        if (bucketName is null) return;

        var response = await client.Buckets[bucketName].Plan.GetAsync();

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機

        var plan = response?.Data
            ?? throw new InvalidOperationException("バケットプラン情報(data)が null です。");
        if (plan.Plan is not null)
        {
            Console.WriteLine("Plan:");
            Console.WriteLine($"  Type: {plan.Plan.Type}");
            Console.WriteLine($"  ServiceClassPath: {plan.Plan.ServiceClassPath}");
            Console.WriteLine($"  ClusterID: {plan.Plan.ClusterId}");
            Console.WriteLine($"  CapacityGib: {plan.Plan.CapacityGib}");
        }
        Console.WriteLine("Contract:");
        if (plan.Contract is not null)
        {
            Console.WriteLine($"  ResourceID: {plan.Contract.ResourceId}");
            Console.WriteLine($"  Status: {plan.Contract.Status}");
            Console.WriteLine($"  CreatedAt: {plan.Contract.CreatedAt}");
            Console.WriteLine($"  DeletedAt: {plan.Contract.DeletedAt}");
        }
    }

    // PUT プラン変更を実行
    public async Task PutBucketPlanAsync()
    {
        Console.WriteLine("\nPUT /buckets/{bucketName}/plan | プラン変更を実行\n");

        var bucketName = ReadRequiredValue("BucketName");
        if (bucketName is null) return;

        var previousContractResourceId = ReadRequiredValue("PreviousContractResourceID");
        if (previousContractResourceId is null) return;

        var serviceClassPath = ReadRequiredValue("ServiceClassPath");
        if (serviceClassPath is null) return;

        var planType = ReadPlanType();
        if (planType is null) return;

        var requestBody = new PlanChangeReqBody
        {
            PreviousContract = new PlanChangeReqBody_previous_contract
            {
                ResourceId = previousContractResourceId
            },
            NewPlan = new PlanChangeReqBody_new_plan
            {
                Type = planType,
                ServiceClassPath = serviceClassPath
            }
        };
        var response = await client.Buckets[bucketName].Plan.PutAsync(requestBody);

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機

        var planChangeResponse = response?.Data
            ?? throw new InvalidOperationException("バケットプラン変更情報(data)が null です。");
        Console.WriteLine("PreviousContract:");
        if (planChangeResponse.PreviousContract is not null)
        {
            Console.WriteLine($"  ResourceID: {planChangeResponse.PreviousContract.ResourceId}");
            Console.WriteLine($"  Status: {planChangeResponse.PreviousContract.Status}");
            Console.WriteLine($"  CreatedAt: {planChangeResponse.PreviousContract.CreatedAt}");
            Console.WriteLine($"  DeletedAt: {planChangeResponse.PreviousContract.DeletedAt}");
        }
        Console.WriteLine("NewContract:");
        if (planChangeResponse.NewContract is not null)
        {
            Console.WriteLine($"  ResourceID: {planChangeResponse.NewContract.ResourceId}");
            Console.WriteLine($"  Status: {planChangeResponse.NewContract.Status}");
            Console.WriteLine($"  CreatedAt: {planChangeResponse.NewContract.CreatedAt}");
            Console.WriteLine($"  DeletedAt: {planChangeResponse.NewContract.DeletedAt}");
        }
        Console.WriteLine("Plan:");
        if (planChangeResponse.Plan is not null)
        {
            Console.WriteLine($"  Type: {planChangeResponse.Plan.Type}");
            Console.WriteLine($"  ServiceClassPath: {planChangeResponse.Plan.ServiceClassPath}");
            Console.WriteLine($"  ClusterID: {planChangeResponse.Plan.ClusterId}");
            Console.WriteLine($"  CapacityGib: {planChangeResponse.Plan.CapacityGib}");
        }
    }

    // GET バケットのペナルティ状況を取得
    public async Task GetBucketPenaltyAsync()
    {
        Console.WriteLine("\nGET /buckets/{bucketName}/penalty | バケットのペナルティ状況を取得\n");

        var bucketName = ReadRequiredValue("BucketName");
        if (bucketName is null) return;

        var response = await client.Buckets[bucketName].Penalty.GetAsync();

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機

        var penalty = response?.Data
            ?? throw new InvalidOperationException("バケットペナルティ情報(data)が null です。");
        Console.WriteLine("NumObjectsPerBucket:");
        if (penalty.NumObjectsPerBucket is not null)
        {
            Console.WriteLine($"  Val: {penalty.NumObjectsPerBucket.Val}");
            Console.WriteLine($"  Quota: {penalty.NumObjectsPerBucket.Quota}");
            Console.WriteLine($"  IsApplied: {penalty.NumObjectsPerBucket.IsApplied}");
        }
        Console.WriteLine("AmountGibPerBucket:");
        if (penalty.AmountGibPerBucket is not null)
        {
            Console.WriteLine($"  Val: {penalty.AmountGibPerBucket.Val}");
            Console.WriteLine($"  Quota: {penalty.AmountGibPerBucket.Quota}");
            Console.WriteLine($"  IsApplied: {penalty.AmountGibPerBucket.IsApplied}");
        }
    }

    // GET バケットの使用量を取得
    public async Task GetBucketUsageAsync()
    {
        Console.WriteLine("\nGET /buckets/{bucketName}/usage | バケットの使用量を取得\n");

        var bucketName = ReadRequiredValue("BucketName");
        if (bucketName is null) return;

        var response = await client.Buckets[bucketName].Usage.GetAsync();

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機

        var usage = response?.Data
            ?? throw new InvalidOperationException("バケット使用量情報(data)が null です。");
        Console.WriteLine($"NumObjectsPerBucket: {usage.NumObjectsPerBucket}");
        Console.WriteLine($"AmountGibPerBucket: {usage.AmountGibPerBucket}");
    }

    // GET バケットの制限値を取得
    public async Task GetBucketQuotaAsync()
    {
        Console.WriteLine("\nGET /buckets/{bucketName}/quota | バケットの制限値を取得\n");

        var bucketName = ReadRequiredValue("BucketName");
        if (bucketName is null) return;

        var response = await client.Buckets[bucketName].Quota.GetAsync();

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機

        var quota = response?.Data
            ?? throw new InvalidOperationException("バケットクォータ情報(data)が null です。");
        Console.WriteLine($"NumObjectsPerBucket: {quota.NumObjectsPerBucket}");
        Console.WriteLine($"AmountGibPerBucket: {quota.AmountGibPerBucket}");
    }

    private static PlanType? ReadPlanType()
    {
        Console.Write("PlanTypeを入力してください (Standard/Archive): ");
        var planTypeInput = Console.ReadLine()?.Trim();
        if (!Enum.TryParse<PlanType>(planTypeInput, true, out var planType)
            || !Enum.IsDefined(planType))
        {
            Console.WriteLine("PlanTypeは Standard または Archive を指定してください。");
            return null;
        }
        return planType;
    }
}
