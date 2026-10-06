using Sacloud.Sdk.NetworkingSuite.Models;

namespace Sacloud.Sdk.NetworkingSuite.Samples.Samples;

public class SubnetGroupsSample(NetworkingSuiteClient client) : AbstractSample(client)
{
    public async Task ListSubnetGroupsAsync()
    {
        Console.WriteLine("GET /networking-suite/subnet-groups | サブネットグループの一覧取得\n");

        SubnetGroupListResponse response = await client.NetworkingSuite.SubnetGroups.GetAsync()
            ?? throw new InvalidOperationException("レスポンスが空です");

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機
        Console.WriteLine();

        Console.WriteLine($"Total: {response.Total}");
        Console.WriteLine($"From: {response.From}");
        Console.WriteLine($"Count: {response.Count}");
        Console.WriteLine($"SubnetGroups:");
        foreach (ReadSubnetGroup subnetGroup in response.SubnetGroups)
        {
            Console.WriteLine($"- SRN: {subnetGroup.SRN}");
            Console.WriteLine($"  Name: {subnetGroup.Name}");
            Console.WriteLine($"  Description: {subnetGroup.Description}");
            Console.WriteLine($"  IPv4AddressRangeCIDR: {subnetGroup.IPv4AddressRangeCIDR}");
            Console.WriteLine($"  Region:");
            Console.WriteLine($"    Code: {subnetGroup.Region.Code}");
        }
    }

    public async Task GetSubnetGroupAsync()
    {
        Console.WriteLine("GET /networking-suite/subnet-groups/{id} | サブネットグループの取得\n");

        Console.WriteLine("取得するサブネットグループの情報を入力してください");
        string? id = ReadRequiredValue("サブネットグループID");
        Console.WriteLine();

        SubnetGroupResponse response = await client.NetworkingSuite.SubnetGroups[id].GetAsync()
            ?? throw new InvalidOperationException("レスポンスが空です");

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機
        Console.WriteLine();

        Console.WriteLine($"SubnetGroup:");
        Console.WriteLine($"  SRN: {response.SubnetGroup.SRN}");
        Console.WriteLine($"  Name: {response.SubnetGroup.Name}");
        Console.WriteLine($"  Description: {response.SubnetGroup.Description}");
        Console.WriteLine($"  IPv4AddressRangeCIDR: {response.SubnetGroup.IPv4AddressRangeCIDR}");
        Console.WriteLine($"  Region:");
        Console.WriteLine($"    Code: {response.SubnetGroup.Region.Code}");
    }

    public async Task CreateSubnetGroupAsync()
    {
        Console.WriteLine("POST /networking-suite/subnet-groups | サブネットグループの作成\n");

        Console.WriteLine("作成するサブネットグループの情報を入力してください");
        string? name = ReadRequiredValue("サブネットグループ名");
        string? description = ReadRequiredValue("サブネットグループの説明");
        string? ipv4AddressRangeCidr = ReadRequiredValue("IPv4アドレス範囲(CIDR)");
        string? regionCode = ReadRequiredValue("リージョンコード");
        Console.WriteLine();

        CreateSubnetGroup body = new()
        {
            Name = name,
            Description = description,
            IPv4AddressRangeCIDR = ipv4AddressRangeCidr,
            Region = new Region
            {
                Code = regionCode
            }
        };

        CreatedSubnetGroupResponse response = await client.NetworkingSuite.SubnetGroups.PostAsync(body)
            ?? throw new InvalidOperationException("レスポンスが空です");

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機
        Console.WriteLine();

        Console.WriteLine($"SubnetGroup:");
        Console.WriteLine($"  SRN: {response.SubnetGroup.SRN}");
        Console.WriteLine($"  Name: {response.SubnetGroup.Name}");
        Console.WriteLine($"  Description: {response.SubnetGroup.Description}");
        Console.WriteLine($"  IPv4AddressRangeCIDR: {response.SubnetGroup.IPv4AddressRangeCIDR}");
        Console.WriteLine($"  Region:");
        Console.WriteLine($"    Code: {response.SubnetGroup.Region.Code}");
    }

    public async Task UpdateSubnetGroupAsync()
    {
        Console.WriteLine("PUT /networking-suite/subnet-groups/{id} | サブネットグループの更新\n");

        Console.WriteLine("更新するサブネットグループの情報を入力してください");
        string? id = ReadRequiredValue("サブネットグループID");
        string? name = ReadRequiredValue("サブネットグループ名");
        string? description = ReadRequiredValue("サブネットグループの説明");
        Console.WriteLine();

        UpdateSubnetGroup body = new()
        {
            Name = name,
            Description = description
        };

        SubnetGroupResponse response = await client.NetworkingSuite.SubnetGroups[id].PutAsync(body)
            ?? throw new InvalidOperationException("レスポンスが空です");

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機
        Console.WriteLine();

        Console.WriteLine($"SubnetGroup:");
        Console.WriteLine($"  SRN: {response.SubnetGroup.SRN}");
        Console.WriteLine($"  Name: {response.SubnetGroup.Name}");
        Console.WriteLine($"  Description: {response.SubnetGroup.Description}");
        Console.WriteLine($"  IPv4AddressRangeCIDR: {response.SubnetGroup.IPv4AddressRangeCIDR}");
        Console.WriteLine($"  Region:");
        Console.WriteLine($"    Code: {response.SubnetGroup.Region.Code}");
    }

    public async Task DeleteSubnetGroupAsync()
    {
        Console.WriteLine("DELETE /networking-suite/subnet-groups/{id} | サブネットグループの削除\n");

        Console.WriteLine("削除するサブネットグループの情報を入力してください");
        string? id = ReadRequiredValue("サブネットグループID");
        Console.WriteLine();

        await client.NetworkingSuite.SubnetGroups[id].DeleteAsync();

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機
        Console.WriteLine();

        Console.WriteLine("サブネットグループを削除しました。(レスポンスボディ無し)");
    }
}
