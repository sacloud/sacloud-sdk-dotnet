using Sacloud.Sdk.NetworkingSuite.Models;

namespace Sacloud.Sdk.NetworkingSuite.Samples.Samples;

public class AddressesSample(NetworkingSuiteClient client) : AbstractSample(client)
{
    public async Task ListAddressesAsync()
    {
        Console.WriteLine("GET /networking-suite/addresses?subnetSRN={subnetSRN} | アドレス一覧の取得\n");

        Console.WriteLine("アドレス一覧を取得するサブネットの情報を入力してください");
        string? subnetSrn = ReadRequiredValue("サブネットのSRN");
        string? interfaceConnectionSrn = ReadRequiredValue("インターフェースコネクションのSRN（指定しない場合は空欄）");
        Console.WriteLine();

        AddressListResponse response = await client.NetworkingSuite.Addresses.GetAsync(requestConfiguration =>
        {
            requestConfiguration.QueryParameters.SubnetSRN = subnetSrn;
            if (!string.IsNullOrWhiteSpace(interfaceConnectionSrn))
            {
                requestConfiguration.QueryParameters.InterfaceConnectionSRN = interfaceConnectionSrn;
            }
        }) ?? throw new InvalidOperationException("レスポンスが空です");

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機
        Console.WriteLine();

        Console.WriteLine($"Total: {response.Total}");
        Console.WriteLine($"From: {response.From}");
        Console.WriteLine($"Count: {response.Count}");
        Console.WriteLine($"Addresses:");
        foreach (ReadAddress address in response.Addresses)
        {
            Console.WriteLine($"- SRN: {address.SRN}");
            Console.WriteLine($"  AddressType: {address.AddressType}");
            Console.WriteLine($"  IPVersion: {address.IPVersion}");
            Console.WriteLine($"  IPAddress: {address.IPAddress}");
            Console.WriteLine($"  Subnet:");
            Console.WriteLine($"    SRN: {address.Subnet.SRN}");
        }
    }

    public async Task GetAddressAsync()
    {
        Console.WriteLine("GET /networking-suite/addresses/{id} | アドレス詳細の取得\n");

        Console.WriteLine("取得するアドレスの情報を入力してください");
        string? id = ReadRequiredValue("アドレスID");
        Console.WriteLine();

        AddressResponse response = await client.NetworkingSuite.Addresses[id].GetAsync()
            ?? throw new InvalidOperationException("レスポンスが空です");

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機
        Console.WriteLine();

        Console.WriteLine($"Address:");
        Console.WriteLine($"  SRN: {response.Address.SRN}");
        Console.WriteLine($"  AddressType: {response.Address.AddressType}");
        Console.WriteLine($"  IPVersion: {response.Address.IPVersion}");
        Console.WriteLine($"  IPAddress: {response.Address.IPAddress}");
        Console.WriteLine($"  Subnet:");
        Console.WriteLine($"    SRN: {response.Address.Subnet.SRN}");
    }
}
