using Sacloud.Sdk.IAM.Folders;

namespace Sacloud.Sdk.IAM.Samples.Samples;

class FolderSample(IAMClient client)
{
    private readonly IAMClient client = client
        ?? throw new ArgumentNullException(nameof(client));

    public async Task RunAsync()
    {
        Console.WriteLine("フォルダのサンプルを実行します");

        // -------------------------------------------------------
        // POST フォルダを作成する
        Console.WriteLine("-----------------------");
        Console.WriteLine("POST /folders | フォルダを作成する");

        var createFolderResponse1 = await client.Folders.PostAsync(new FoldersPostRequestBody
        {
            Name = "sdk-sample-folder-1",
            Description = "SDKから作成したサンプルフォルダ",
        });
        var folderId1 = createFolderResponse1.Id
            ?? throw new InvalidOperationException("フォルダの作成に失敗しました");
        Console.WriteLine($"  - 作成したフォルダID: {createFolderResponse1.Id}");
        Console.WriteLine($"    - Name: {createFolderResponse1.Name}");
        Console.WriteLine($"    - Description: {createFolderResponse1.Description}");

        var createFolderResponse2 = await client.Folders.PostAsync(new FoldersPostRequestBody
        {
            Name = "sdk-sample-folder-2",
            Description = "SDKから作成したサンプルフォルダ",
        });
        var folderId2 = createFolderResponse2.Id
            ?? throw new InvalidOperationException("フォルダの作成に失敗しました");
        Console.WriteLine($"  - 作成したフォルダID: {createFolderResponse2.Id}");
        Console.WriteLine($"    - Name: {createFolderResponse2.Name}");
        Console.WriteLine($"    - Description: {createFolderResponse2.Description}");

        // -------------------------------------------------------
        // GET 特定のフォルダの情報を取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"GET /folders/{folderId1} | 特定のフォルダの情報を取得する");
        var getFolderResponse = await client.Folders[folderId1].GetAsync();
        Console.WriteLine($"  - 取得したフォルダID: {getFolderResponse.Id}");
        Console.WriteLine($"    - Name: {getFolderResponse.Name}");
        Console.WriteLine($"    - Description: {getFolderResponse.Description}");

        // -------------------------------------------------------
        // PUT 特定のフォルダの情報を更新する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"PUT /folders/{folderId1} | 特定のフォルダの情報を更新する");
        var updateFolderResponse = await client.Folders[folderId1].PutAsync(new Folders.Item.WithFolder_PutRequestBody
        {
            Name = "sdk-updated-sample-folder",
            Description = "SDKから更新したサンプルフォルダ",
        });
        Console.WriteLine($"  - 更新したフォルダID: {updateFolderResponse.Id}");
        Console.WriteLine($"    - Name: {updateFolderResponse.Name}");
        Console.WriteLine($"    - Description: {updateFolderResponse.Description}");

        // -------------------------------------------------------
        // GET フォルダ一覧を取得する
        Console.WriteLine("-----------------------");
        Console.WriteLine("GET /folders | フォルダ一覧を取得する");
        var listFoldersResponse = await client.Folders.GetAsync();
        Console.WriteLine($"  - 取得したフォルダ数: {listFoldersResponse.Items.Count}");

        // -------------------------------------------------------
        // POST フォルダを移動する
        Console.WriteLine("-----------------------");
        Console.WriteLine("POST /move-folders | フォルダを移動する");
        await client.MoveFolders.PostAsync(new Models.MoveFolders
        {
            FolderIds = [folderId1],
            ParentId = folderId2,
        });
        Console.WriteLine("  - フォルダの移動が完了しました (レスポンスボディ無し)");

        // -------------------------------------------------------
        // DELETE フォルダを削除する
        Console.WriteLine("-----------------------");
        Console.WriteLine($"DELETE /folders/{folderId1} | フォルダを削除する");
        await client.Folders[folderId1].DeleteAsync();
        Console.WriteLine("  - フォルダ1の削除が完了しました (レスポンスボディ無し)");

        await client.Folders[folderId2].DeleteAsync();
        Console.WriteLine("  - フォルダ2の削除が完了しました (レスポンスボディ無し)");

        Console.WriteLine("フォルダのサンプルの実行が完了しました");
    }
}
