using Sacloud.Sdk.AppRunShared.Samples.Operations;

namespace Sacloud.Sdk.AppRunShared.Samples;

/// <summary>
/// AppRun共用型の各種操作を実装するクラス
/// </summary>
public class AppRunSharedOperations
{
    private readonly AppRunSharedClient _client;
    private readonly ApplicationsOperations _applicationsOperations;
    private readonly UserOperations _userOperations;
    private readonly TrafficsOperations _trafficsOperations;
    private readonly VersionsOperations _versionsOperations;
    private readonly PacketFilterOperations _packetFilterOperations;

    public AppRunSharedOperations(AppRunSharedClient client)
    {
        _client = client;
        _applicationsOperations = new ApplicationsOperations(_client);
        _userOperations = new UserOperations(_client);
        _trafficsOperations = new TrafficsOperations(_client);
        _versionsOperations = new VersionsOperations(_client);
        _packetFilterOperations = new PacketFilterOperations(_client);
    }

    /// <summary>
    /// ユーザーに操作メニューを表示し、選択された操作を実行する
    /// </summary>
    public async Task RunInteractiveMenuAsync()
    {
        Console.WriteLine("\n実行する操作を選択してください:");
        Console.WriteLine("1. アプリケーション一覧を取得");
        Console.WriteLine("2. アプリケーションを作成");
        Console.WriteLine("3. アプリケーション詳細を取得");
        Console.WriteLine("4. アプリケーションを部分的に変更");
        Console.WriteLine("5. アプリケーションを削除");
        Console.WriteLine("6. アプリケーションステータスを取得");
        Console.WriteLine("7. ユーザー情報を取得");
        Console.WriteLine("8. ユーザーを作成");
        Console.WriteLine("9. アプリケーショントラフィック分散を取得");
        Console.WriteLine("10. アプリケーショントラフィック分散を変更");
        Console.WriteLine("11. アプリケーションバージョン一覧を取得");
        Console.WriteLine("12. アプリケーションバージョン詳細を取得");
        Console.WriteLine("13. アプリケーションバージョンを削除");
        Console.WriteLine("14. アプリケーションバージョンステータスを取得");
        Console.WriteLine("15. パケットフィルタを取得");
        Console.WriteLine("16. パケットフィルタを部分的に変更");
        Console.Write("選択 (1 ~ 16 から入力): ");
        var choice = Console.ReadLine();

        await ExecuteOperationAsync(choice);
    }

    /// <summary>
    /// 選択された操作を実行する
    /// </summary>
    private async Task ExecuteOperationAsync(string? choice)
    {
        switch (choice)
        {
            case "1":
                await _applicationsOperations.ListApplicationsAsync();
                break;
            case "2":
                await _applicationsOperations.CreateApplicationAsync();
                break;
            case "3":
                await _applicationsOperations.GetApplicationDetailsAsync();
                break;
            case "4":
                await _applicationsOperations.UpdateApplicationAsync();
                break;
            case "5":
                await _applicationsOperations.DeleteApplicationAsync();
                break;
            case "6":
                await _applicationsOperations.GetApplicationStatusAsync();
                break;
            case "7":
                await _userOperations.GetUserInfoAsync();
                break;
            case "8":
                await _userOperations.CreateUserAsync();
                break;
            case "9":
                await _trafficsOperations.GetTrafficDistributionAsync();
                break;
            case "10":
                await _trafficsOperations.UpdateTrafficDistributionAsync();
                break;
            case "11":
                await _versionsOperations.ListVersionsAsync();
                break;
            case "12":
                await _versionsOperations.GetVersionDetailsAsync();
                break;
            case "13":
                await _versionsOperations.DeleteVersionAsync();
                break;
            case "14":
                await _versionsOperations.GetVersionStatusAsync();
                break;
            case "15":
                await _packetFilterOperations.GetPacketFilterAsync();
                break;
            case "16":
                await _packetFilterOperations.UpdatePacketFilterAsync();
                break;
            default:
                Console.WriteLine("無効な番号が選択されました。");
                break;
        }
    }
}
