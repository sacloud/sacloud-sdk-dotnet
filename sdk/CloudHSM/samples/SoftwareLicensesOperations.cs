using Sacloud.Sdk.CloudHSM.Models;

namespace Sacloud.Sdk.CloudHSM.Samples;

/// <summary>
/// ソフトウェアライセンスの操作を実装するクラス
/// </summary>
public class SoftwareLicensesOperations
{
    private readonly CloudHSMClient _client;

    public SoftwareLicensesOperations(CloudHSMClient client)
    {
        _client = client;
    }

    public async Task ListAsync()
    {
        var response = await _client.Cloudhsm.Licenses.GetAsync();
        Console.WriteLine("ソフトウェアライセンス一覧:");
        if (response?.CloudHSMs is null || response.CloudHSMs.Count == 0)
        {
            Console.WriteLine("ソフトウェアライセンスがありません。");
            return;
        }

        foreach (var license in response.CloudHSMs)
        {
            Console.WriteLine($"- {license.ID}: {license.Name}");
        }
    }

    public async Task CreateAsync()
    {
        var body = new WrappedCreateCloudHSMSoftwareLicense
        {
            CloudHSM = new CreateCloudHSMSoftwareLicense
            {
                Name = ReadRequiredInput("ソフトウェアライセンス名を入力してください"),
                Description = ReadOptionalInput("説明を入力してください"),
                ServiceClass = CloudHSMSoftwareLicenseServiceClassEnum.CloudCloudhsmLicenseL7,
                Tags = new List<string> { "sample" }
            }
        };

        var response = await _client.Cloudhsm.Licenses.PostAsync(body);
        Console.WriteLine("作成したソフトウェアライセンス:");
        Console.WriteLine($"- {response?.CloudHSM?.ID}: {response?.CloudHSM?.Name}");
    }

    public async Task GetDetailsAsync()
    {
        var licenseId = ReadRequiredInput("詳細を取得するソフトウェアライセンス IDを入力してください");
        var response = await _client.Cloudhsm.Licenses[licenseId].GetAsync();
        Console.WriteLine("ソフトウェアライセンス詳細:");
        Console.WriteLine($"ID: {response?.CloudHSM?.ID}");
        Console.WriteLine($"Name: {response?.CloudHSM?.Name}");
        Console.WriteLine($"Description: {response?.CloudHSM?.Description}");
        Console.WriteLine($"ServiceClass: {response?.CloudHSM?.ServiceClass}");
    }

    public async Task UpdateAsync()
    {
        var licenseId = ReadRequiredInput("更新するソフトウェアライセンス IDを入力してください");
        var current = await _client.Cloudhsm.Licenses[licenseId].GetAsync();
        var license = current?.CloudHSM
            ?? throw new InvalidOperationException("更新対象のソフトウェアライセンスが見つかりませんでした。");

        license.Name = ReadRequiredInput("更新後のソフトウェアライセンス名を入力してください");
        license.Description = ReadOptionalInput("更新後の説明を入力してください");
        license.Tags ??= new List<string> { "sample" };

        var response = await _client.Cloudhsm.Licenses[licenseId].PutAsync(new WrappedCloudHSMSoftwareLicense
        {
            CloudHSM = license
        });
        Console.WriteLine("更新したソフトウェアライセンス:");
        Console.WriteLine($"- {response?.CloudHSM?.ID}: {response?.CloudHSM?.Name}");
    }

    public async Task DeleteAsync()
    {
        var licenseId = ReadRequiredInput("削除するソフトウェアライセンス IDを入力してください");
        await _client.Cloudhsm.Licenses[licenseId].DeleteAsync();
        Console.WriteLine("ソフトウェアライセンスを削除しました。");
    }

    private static string ReadRequiredInput(string prompt)
    {
        Console.Write($"{prompt}: ");
        var value = Console.ReadLine();
        return value ?? string.Empty;
    }

    private static string? ReadOptionalInput(string prompt)
    {
        Console.Write($"{prompt}（省略可）: ");
        return Console.ReadLine();
    }
}
