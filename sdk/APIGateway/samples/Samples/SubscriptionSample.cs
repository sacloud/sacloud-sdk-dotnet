using Sacloud.Sdk.APIGateway.Models;
using Sacloud.Sdk.APIGateway.Samples.Extensions;

namespace Sacloud.Sdk.APIGateway.Samples.Samples;

class SubscriptionSample(APIGatewayClient client) : ISample
{
    private readonly APIGatewayClient client = client
        ?? throw new ArgumentNullException(nameof(client));

    private Guid subscriptionId = Guid.Empty;
    private Guid planId = Guid.Empty;

    public string Description { get; } = "サブスクリプションカテゴリのサンプルを実行";

    public async Task RunAsync()
    {
        Console.WriteLine("\nサブスクリプションのサンプルを実行します");
        try
        {
            // サンプルで利用する依存リソースの作成
            Console.WriteLine("\n---------[事前準備]--------");

            // サンプル用プランの取得
            Console.WriteLine("\nサンプル用プランの取得中...");
            var listPlansResponse = await client.Plans.GetAsync();
            var firstPlan = listPlansResponse.Apigw.Plans?.FirstOrDefault()
                ?? throw new NullReferenceException("契約APIで利用可能なプランが見つかりませんでした。");
            planId = firstPlan.Id
                ?? throw new NullReferenceException("プラン一覧照会APIのレスポンスにプランIDが含まれていません。");

            Console.WriteLine("サンプル用プランの取得が完了しました。");

            // サンプルの実行
            Console.WriteLine("\n---------[サブスクリプションサンプル]--------");

            var subscriptionName = "sample-subscription";

            // POST 契約を作成する
            Console.WriteLine("\nPOST /subscriptions | 契約API");
            await client.Subscriptions.PostAsync(new SubscriptionCreate
            {
                Name = subscriptionName,
                PlanId = planId
            });

            Console.WriteLine("契約の作成が完了しました。(レスポンスボディ無し)");

            // GET 契約一覧を取得する
            Console.WriteLine("\nGET /subscriptions | 契約一覧照会API");
            var listSubscriptionsResponse = await client.Subscriptions.GetAsync();
            var subscriptions = listSubscriptionsResponse.Apigw.Subscriptions
                ?? throw new NullReferenceException("契約一覧照会APIのレスポンスに契約情報が含まれていません。");
            var targetSubscription = subscriptions.FirstOrDefault(x => x.Name == subscriptionName && x.PlanId == planId)
                ?? throw new NullReferenceException("作成した契約が契約一覧に見つかりませんでした。");
            subscriptionId = targetSubscription.Id
                ?? throw new NullReferenceException("契約一覧照会APIのレスポンスに契約IDが含まれていません。");

            Console.WriteLine("契約一覧の取得が完了しました。");
            Console.WriteLine("  - Subscriptions:");
            if (subscriptions is null || subscriptions.Count <= 0)
            {
                throw new NullReferenceException("    - (null)");
            }
            else
            {
                foreach (var subscription in subscriptions)
                {
                    Console.WriteLine($"    - Id: {subscription.Id.FormatNullable()}");
                    Console.WriteLine($"      - Name: {subscription.Name.FormatNullable()}");
                    Console.WriteLine($"      - PlanId: {subscription.PlanId.FormatNullable()}");
                    Console.WriteLine($"      - MonthlyRequest: {subscription.MonthlyRequest.FormatNullable()}");
                    Console.WriteLine($"      - ResourceId: {subscription.ResourceId.FormatNullable()}");
                    Console.WriteLine($"      - CreatedAt: {subscription.CreatedAt.FormatNullable()}");
                    Console.WriteLine($"      - UpdatedAt: {subscription.UpdatedAt.FormatNullable()}");
                    WriteSubscriptionService(subscription.Service, 4);
                }
            }

            // GET 指定した契約の詳細情報を取得する
            Console.WriteLine("\nGET /subscriptions/{subscriptionId} | 契約詳細照会API");
            var getSubscriptionResponse = await client.Subscriptions[subscriptionId].GetAsync();
            var subscriptionDetail = getSubscriptionResponse.Apigw.Subscription;

            Console.WriteLine("指定した契約の詳細情報の取得が完了しました。");
            Console.WriteLine($"  - Id: {subscriptionDetail.Id.FormatNullable()}");
            Console.WriteLine($"    - Name: {subscriptionDetail.Name.FormatNullable()}");
            Console.WriteLine($"    - MonthlyRequest: {subscriptionDetail.MonthlyRequest.FormatNullable()}");
            Console.WriteLine($"    - ResourceId: {subscriptionDetail.ResourceId.FormatNullable()}");
            Console.WriteLine($"    - CreatedAt: {subscriptionDetail.CreatedAt.FormatNullable()}");
            Console.WriteLine($"    - UpdatedAt: {subscriptionDetail.UpdatedAt.FormatNullable()}");
            WriteSubscriptionService(subscriptionDetail.Service, 4);
            Console.WriteLine("    - Plan:");
            if (subscriptionDetail.Plan is null)
            {
                Console.WriteLine("      - (null)");
            }
            else
            {
                Console.WriteLine($"      - PlanID: {subscriptionDetail.Plan.PlanID.FormatNullable()}");
                Console.WriteLine($"      - PlanName: {subscriptionDetail.Plan.PlanName.FormatNullable()}");
                Console.WriteLine($"      - Price: {subscriptionDetail.Plan.Price.FormatNullable()}");
                Console.WriteLine($"      - MaxServices: {subscriptionDetail.Plan.MaxServices.FormatNullable()}");
                Console.WriteLine($"      - MaxRequests: {subscriptionDetail.Plan.MaxRequests.FormatNullable()}");
                Console.WriteLine($"      - MaxRequestsUnit: {subscriptionDetail.Plan.MaxRequestsUnit.FormatNullable()}");
            }

            // PUT 指定した契約を更新する
            Console.WriteLine("\nPUT /subscriptions/{subscriptionId} | 契約更新API");
            await client.Subscriptions[subscriptionId].PutAsync(new SubscriptionUpdate
            {
                Name = "updated-sample-subscription"
            });

            Console.WriteLine("指定した契約の更新が完了しました。(レスポンスボディ無し)");
        }
        finally
        {
            // DELETE 指定した契約を解約する
            if (subscriptionId != Guid.Empty)
            {
                Console.WriteLine("\nDELETE /subscriptions/{subscriptionId} | 契約解約API");
                await client.Subscriptions[subscriptionId].DeleteAsync();

                Console.WriteLine("指定した契約の解約が完了しました。(レスポンスボディ無し)");
            }

            Console.WriteLine("\nサブスクリプションのサンプルの実行が完了しました");
        }
    }

    private void WriteSubscriptionService(SubscriptionService? service, int indent = 2)
    {
        Console.WriteIndentedLine("- Service:", indent);
        if (service is null)
        {
            Console.WriteIndentedLine("- (null)", indent + 2);
        }
        else
        {
            Console.WriteIndentedLine($"- Id: {service.Id.FormatNullable()}", indent + 2);
            Console.WriteIndentedLine($"- Name: {service.Name.FormatNullable()}", indent + 2);
        }
    }
};
