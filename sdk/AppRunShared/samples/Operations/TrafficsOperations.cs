using Sacloud.Sdk.AppRunShared.Applications.Item.Traffics;
using Sacloud.Sdk.AppRunShared.Models;

namespace Sacloud.Sdk.AppRunShared.Samples.Operations;

public sealed class TrafficsOperations(AppRunSharedClient client)
{
    private readonly AppRunSharedClient _client = client;

    public async Task GetTrafficDistributionAsync()
    {
        Console.Write("トラフィック分散を取得するアプリケーションIDを入力してください: ");
        var id = Console.ReadLine();
        var traffics = await _client.Applications[id].Traffics.GetAsync();

        Console.WriteLine("トラフィック分散詳細:");
        if (traffics.Data is not null)
        {
            Console.WriteLine("Data:");
            foreach (var traffic in traffics.Data)
            {
                Console.WriteLine($"- IsLatestVersion: {traffic.IsLatestVersion}");
                Console.WriteLine($"  VersionName: {traffic.VersionName}");
                Console.WriteLine($"  Percent: {traffic.Percent}");
            }
        }
    }

    public async Task UpdateTrafficDistributionAsync()
    {
        Console.Write("トラフィック分散を変更するアプリケーションIDを入力してください: ");
        var id = Console.ReadLine();
        var newLatestVersionTrafficPercent = 50;
        var body = new List<TrafficsRequestBuilder.UpdateTrafficBody>
        {
            new TrafficsRequestBuilder.UpdateTrafficBody
            {
                UpdateTrafficBodyMember1 = new UpdateTrafficBodyMember1
                {
                    IsLatestVersion = true,
                    Percent = newLatestVersionTrafficPercent
                }
            },
            new TrafficsRequestBuilder.UpdateTrafficBody
            {
                UpdateTrafficBodyMember2 = new UpdateTrafficBodyMember2
                {
                    VersionName = "<YOUR_VERSION_NAME>", // トラフィック分散を変更するバージョン名を指定
                    Percent = 100 - newLatestVersionTrafficPercent
                }
            }
        };
        var traffics = await _client.Applications[id].Traffics.PutAsync(body);
        Console.WriteLine("変更後のトラフィック分散詳細:");

        if (traffics.Data is not null)
        {
            Console.WriteLine("Data:");
            foreach (var traffic in traffics.Data)
            {
                Console.WriteLine($"- IsLatestVersion: {traffic.IsLatestVersion}");
                Console.WriteLine($"  VersionName: {traffic.VersionName}");
                Console.WriteLine($"  Percent: {traffic.Percent}");

            }
        }
    }
}
