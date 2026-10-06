using Sacloud.Sdk.NetworkingSuite.Models;

namespace Sacloud.Sdk.NetworkingSuite.Samples.Samples;

public class SubnetsSample(NetworkingSuiteClient client) : AbstractSample(client)
{
    public async Task ListSubnetsAsync()
    {
        Console.WriteLine("GET /networking-suite/subnets?subnetGroupSRN={subnetGroupSRN} | サブネットの一覧取得\n");

        Console.WriteLine("サブネット一覧を取得するサブネットグループの情報を入力してください");
        string? subnetGroupSrn = ReadRequiredValue("サブネットグループのSRN");
        Console.WriteLine();

        SubnetListResponse response = await client.NetworkingSuite.Subnets.GetAsync(requestConfiguration =>
        {
            requestConfiguration.QueryParameters.SubnetGroupSRN = subnetGroupSrn;
        }) ?? throw new InvalidOperationException("レスポンスが空です");

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機
        Console.WriteLine();

        Console.WriteLine($"Total: {response.Total}");
        Console.WriteLine($"From: {response.From}");
        Console.WriteLine($"Count: {response.Count}");
        Console.WriteLine($"Subnets:");
        foreach (ReadSubnet subnet in response.Subnets)
        {
            Console.WriteLine($"- SRN: {subnet.SRN}");
            Console.WriteLine($"  Name: {subnet.Name}");
            Console.WriteLine($"  Description: {subnet.Description}");
            Console.WriteLine($"  IPv4AddressRangeCIDR: {subnet.IPv4AddressRangeCIDR}");
            Console.WriteLine($"  SubnetGroup:");
            Console.WriteLine($"    SRN: {subnet.SubnetGroup.SRN}");
            Console.WriteLine($"  Zone:");
            Console.WriteLine($"    Code: {subnet.Zone.Code}");
        }
    }

    public async Task GetSubnetAsync()
    {
        Console.WriteLine("GET /networking-suite/subnets/{id} | サブネットの取得\n");

        Console.WriteLine("取得するサブネットの情報を入力してください");
        string? id = ReadRequiredValue("サブネットID");
        Console.WriteLine();

        SubnetResponse response = await client.NetworkingSuite.Subnets[id].GetAsync()
            ?? throw new InvalidOperationException("レスポンスが空です");

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機
        Console.WriteLine();

        Console.WriteLine($"Subnet:");
        Console.WriteLine($"  SRN: {response.Subnet.SRN}");
        Console.WriteLine($"  Name: {response.Subnet.Name}");
        Console.WriteLine($"  Description: {response.Subnet.Description}");
        Console.WriteLine($"  IPv4AddressRangeCIDR: {response.Subnet.IPv4AddressRangeCIDR}");
        Console.WriteLine($"  SubnetGroup:");
        Console.WriteLine($"    SRN: {response.Subnet.SubnetGroup.SRN}");
        Console.WriteLine($"  Zone:");
        Console.WriteLine($"    Code: {response.Subnet.Zone.Code}");
    }

    public async Task CreateSubnetAsync()
    {
        Console.WriteLine("POST /networking-suite/subnets | サブネットの作成\n");

        Console.WriteLine("作成するサブネットの情報を入力してください");
        string? name = ReadRequiredValue("サブネット名");
        string? description = ReadRequiredValue("サブネットの説明");
        string? ipv4AddressRangeCidr = ReadRequiredValue("IPv4アドレス範囲(CIDR)");
        string? subnetGroupSrn = ReadRequiredValue("サブネットグループSRN");
        string? zoneCode = ReadRequiredValue("ゾーンコード");
        Console.WriteLine();

        CreateSubnet body = new()
        {
            Name = name,
            Description = description,
            IPv4AddressRangeCIDR = ipv4AddressRangeCidr,
            SubnetGroup = new SakuraResourceNameRef
            {
                SRN = subnetGroupSrn
            },
            Zone = new Zone
            {
                Code = zoneCode
            }
        };

        CreatedSubnetResponse response = await client.NetworkingSuite.Subnets.PostAsync(body)
            ?? throw new InvalidOperationException("レスポンスが空です");

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機
        Console.WriteLine();

        Console.WriteLine($"Subnet:");
        Console.WriteLine($"  SRN: {response.Subnet.SRN}");
        Console.WriteLine($"  Name: {response.Subnet.Name}");
        Console.WriteLine($"  Description: {response.Subnet.Description}");
        Console.WriteLine($"  IPv4AddressRangeCIDR: {response.Subnet.IPv4AddressRangeCIDR}");
        Console.WriteLine($"  SubnetGroup:");
        Console.WriteLine($"    SRN: {response.Subnet.SubnetGroup.SRN}");
        Console.WriteLine($"  Zone:");
        Console.WriteLine($"    Code: {response.Subnet.Zone.Code}");
    }

    public async Task UpdateSubnetAsync()
    {
        Console.WriteLine("PUT /networking-suite/subnets/{id} | サブネットの更新\n");

        Console.WriteLine("更新するサブネットの情報を入力してください");
        string? id = ReadRequiredValue("サブネットID");
        string? name = ReadRequiredValue("サブネット名");
        string? description = ReadRequiredValue("サブネットの説明");
        Console.WriteLine();

        UpdateSubnet body = new()
        {
            Name = name,
            Description = description
        };

        SubnetResponse response = await client.NetworkingSuite.Subnets[id].PutAsync(body)
            ?? throw new InvalidOperationException("レスポンスが空です");

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機
        Console.WriteLine();

        Console.WriteLine($"Subnet:");
        Console.WriteLine($"  SRN: {response.Subnet.SRN}");
        Console.WriteLine($"  Name: {response.Subnet.Name}");
        Console.WriteLine($"  Description: {response.Subnet.Description}");
        Console.WriteLine($"  IPv4AddressRangeCIDR: {response.Subnet.IPv4AddressRangeCIDR}");
        Console.WriteLine($"  SubnetGroup:");
        Console.WriteLine($"    SRN: {response.Subnet.SubnetGroup.SRN}");
        Console.WriteLine($"  Zone:");
        Console.WriteLine($"    Code: {response.Subnet.Zone.Code}");
    }

    public async Task DeleteSubnetAsync()
    {
        Console.WriteLine("DELETE /networking-suite/subnets/{id} | サブネットの削除\n");

        Console.WriteLine("削除するサブネットの情報を入力してください");
        string? id = ReadRequiredValue("サブネットID");
        Console.WriteLine();

        await client.NetworkingSuite.Subnets[id].DeleteAsync();

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機
        Console.WriteLine();

        Console.WriteLine("サブネットを削除しました。(レスポンスボディ無し)");
    }
}
