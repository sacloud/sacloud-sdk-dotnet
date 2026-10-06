namespace Sacloud.Sdk.MonitoringSuite.Samples;

class PublishersSample(MonitoringSuiteClient client)
{
    private readonly MonitoringSuiteClient client = client
        ?? throw new ArgumentNullException(nameof(client));

    public async Task RunAsync()
    {
        Console.WriteLine("連携サービス（パブリッシャー）のサンプルを実行します");

        // -------------------------------------------------------
        // GET 連携サービス一覧を取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine("GET /publishers | 連携サービス一覧を取得する");
        var listResponse = await client.Publishers.GetAsync();
        Console.WriteLine($"  - 取得した連携サービス数: {listResponse?.Results?.Count}");

        // -------------------------------------------------------
        // GET 連携サービス一覧をクエリパラメータ付きで取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine("GET /publishers?count=10&from=0 | 連携サービス一覧をクエリパラメータ付きで取得する");
        var listWithQueryResponse = await client.Publishers.GetAsync(config =>
        {
            config.QueryParameters.Count = 10;
            config.QueryParameters.From = 0;
        });
        Console.WriteLine($"  - 取得した連携サービス数 (count=10, from=0): {listWithQueryResponse?.Results?.Count}");

        if (listResponse?.Results != null && listResponse.Results.Count > 0)
        {
            var firstCode = listResponse.Results[0].Code
                ?? throw new InvalidOperationException("連携サービスコードがNullです");
            Console.WriteLine($"  - 最初の連携サービスコード: {firstCode}");

            // -------------------------------------------------------
            // GET 特定の連携サービス情報を取得する
            Console.WriteLine("-----------------------");
            Console.WriteLine($"GET /publishers/{firstCode} | 特定の連携サービス情報を取得する");
            var getResponse = await client.Publishers[firstCode].GetAsync();
            Console.WriteLine($"  - 取得した連携サービスコード: {getResponse?.Code}");
            Console.WriteLine($"    - Description: {getResponse?.Description}");
            Console.WriteLine($"    - Variants数: {getResponse?.Variants?.Count}");
        }

        Console.WriteLine("連携サービス（パブリッシャー）のサンプルの実行が完了しました");
    }
}
