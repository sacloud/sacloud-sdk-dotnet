using Sacloud.Sdk.AppRunShared.Models;

namespace Sacloud.Sdk.AppRunShared.Samples.Operations;

public sealed class PacketFilterOperations(AppRunSharedClient client)
{
    private readonly AppRunSharedClient _client = client;

    public async Task GetPacketFilterAsync()
    {
        Console.Write("パケットフィルタを取得するアプリケーションIDを入力してください: ");
        var applicationId = Console.ReadLine();
        var packetFilter = await _client.Applications[applicationId].Packet_filter.GetAsync();
        Console.WriteLine("パケットフィルタ詳細:");
        Console.WriteLine($"IsEnabled: {packetFilter.IsEnabled}");

        if (packetFilter.Settings is not null)
        {
            Console.WriteLine("Settings:");
            foreach (var setting in packetFilter.Settings)
            {
                Console.WriteLine($"- FromIp: {setting.FromIp}");
                Console.WriteLine($"  FromIpPrefixLength: {setting.FromIpPrefixLength}");
            }
        }
    }

    public async Task UpdatePacketFilterAsync()
    {
        Console.Write("パケットフィルタを変更するアプリケーションIDを入力してください: ");
        var applicationId = Console.ReadLine();
        var body = new PatchPacketFilterBody
        {
            IsEnabled = true,
            Settings = new List<PatchPacketFilterBody_settings>
            {
                new PatchPacketFilterBody_settings
                {
                    FromIp = "<YOUR_SOURCE_IPv4_ADDRESS>", // 許可する送信元IPv4アドレスを指定
                    FromIpPrefixLength = 24
                }
            }
        };
        var packetFilter = await _client.Applications[applicationId].Packet_filter.PatchAsync(body);
        Console.WriteLine("変更後のパケットフィルタ詳細:");
        Console.WriteLine($"IsEnabled: {packetFilter.IsEnabled}");

        if (packetFilter.Settings is not null)
        {
            Console.WriteLine("Settings:");
            foreach (var setting in packetFilter.Settings)
            {
                Console.WriteLine($"- FromIp: {setting.FromIp}");
                Console.WriteLine($"  FromIpPrefixLength: {setting.FromIpPrefixLength}");
            }
        }
    }
}
