using Sacloud.Sdk.CloudHSM.Models;

namespace Sacloud.Sdk.CloudHSM.Samples;

/// <summary>
/// CloudHSMピアの操作を実装するクラス
/// </summary>
public class CloudHSMPeersOperations
{
    private readonly CloudHSMClient _client;

    public CloudHSMPeersOperations(CloudHSMClient client)
    {
        _client = client;
    }

    public async Task ListAsync()
    {
        var cloudHsmId = ReadRequiredInput("CloudHSM IDを入力してください");
        var response = await _client.Cloudhsm.Cloudhsms[cloudHsmId].Peers.GetAsync();
        Console.WriteLine("CloudHSMピア一覧:");
        if (response?.Peers is null || response.Peers.Count == 0)
        {
            Console.WriteLine("CloudHSMピアがありません。");
            return;
        }

        foreach (var peer in response.Peers)
        {
            Console.WriteLine($"- {peer.ID}");
        }
    }

    public async Task CreateAsync()
    {
        var cloudHsmId = ReadRequiredInput("CloudHSM IDを入力してください");
        var body = new WrappedCreateCloudHSMPeer
        {
            Peer = new CreateCloudHSMPeer
            {
                ID = ReadRequiredInput("ピア IDを入力してください"),
                SecretKey = ReadRequiredInput("ピアシークレットキーを入力してください")
            }
        };

        var response = await _client.Cloudhsm.Cloudhsms[cloudHsmId].Peers.PostAsync(body);
        Console.WriteLine("作成したCloudHSMピア:");
        Console.WriteLine($"- {response?.Peer?.ID}");
    }

    public async Task DeleteAsync()
    {
        var cloudHsmId = ReadRequiredInput("CloudHSM IDを入力してください");
        var peerId = ReadRequiredInput("削除するCloudHSMピア IDを入力してください");
        await _client.Cloudhsm.Cloudhsms[cloudHsmId].Peers[peerId].DeleteAsync();
        Console.WriteLine("CloudHSMピアを削除しました。");
    }

    private static string ReadRequiredInput(string prompt)
    {
        Console.Write($"{prompt}: ");
        var value = Console.ReadLine();
        return value ?? string.Empty;
    }
}
