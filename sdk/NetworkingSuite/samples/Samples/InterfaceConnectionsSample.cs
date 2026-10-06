using Sacloud.Sdk.NetworkingSuite.Models;

namespace Sacloud.Sdk.NetworkingSuite.Samples.Samples;

public class InterfaceConnectionsSample(NetworkingSuiteClient client) : AbstractSample(client)
{
    public async Task CreateInterfaceConnectionAsync()
    {
        Console.WriteLine("POST /networking-suite/interface-connections | NICをサブネットに接続\n");

        Console.WriteLine("接続するNICとサブネットの情報を入力してください");
        string? interfaceSrn = ReadRequiredValue("NICのSRN");
        string? subnetSrn = ReadRequiredValue("サブネットのSRN");
        string? ephemeralIPv4Address = ReadRequiredValue("エフェメラルIPv4アドレス（自動割り当ての場合は空欄）");
        Console.WriteLine();

        CreateInterfaceConnection body = new()
        {
            Interface = new SakuraResourceNameRef
            {
                SRN = interfaceSrn
            },
            Subnet = new SakuraResourceNameRef
            {
                SRN = subnetSrn
            },
            EphemeralIPv4Address = string.IsNullOrWhiteSpace(ephemeralIPv4Address) ? null : ephemeralIPv4Address
        };

        CreatedInterfaceConnectionResponse response = await client.NetworkingSuite.InterfaceConnections.PostAsync(body)
            ?? throw new InvalidOperationException("レスポンスが空です");

        ReadInterfaceConnection interfaceConnection = response.InterfaceConnection
            ?? throw new InvalidOperationException("インターフェースコネクションの情報が空です");

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機
        Console.WriteLine();

        Console.WriteLine("InterfaceConnection:");
        Console.WriteLine($"  SRN: {interfaceConnection.SRN}");
        Console.WriteLine($"  EphemeralIPv4Address: {interfaceConnection.EphemeralIPv4Address}");
    }

    public async Task DeleteInterfaceConnectionAsync()
    {
        Console.WriteLine("DELETE /networking-suite/interface-connections/{id} | NICをサブネットから切断\n");

        Console.WriteLine("削除するインターフェースコネクションの情報を入力してください");
        string? id = ReadRequiredValue("インターフェースコネクションID");
        Console.WriteLine();

        await client.NetworkingSuite.InterfaceConnections[id].DeleteAsync();

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機
        Console.WriteLine();

        Console.WriteLine("NICをサブネットから切断しました。(レスポンスボディ無し)");
    }
}
