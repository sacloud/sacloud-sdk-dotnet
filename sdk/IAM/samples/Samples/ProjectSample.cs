using Sacloud.Sdk.IAM.Projects;

namespace Sacloud.Sdk.IAM.Samples.Samples;

class ProjectSample(IAMClient client)
{
    private readonly IAMClient client = client
        ?? throw new ArgumentNullException(nameof(client));

    public async Task RunAsync(long folderId)
    {
        Console.WriteLine("プロジェクトのサンプルを実行します");

        // -------------------------------------------------------
        // POST プロジェクトを作成する
        Console.WriteLine("POST /projects | プロジェクトを作成する");
        var createProjectResponse = await client.Projects.PostAsync(new ProjectsPostRequestBody
        {
            Name = "sdk-sample-project",
            Code = "sdk-sample-project",
            Description = "SDKから作成したサンプルプロジェクト",
        });
        var projectId = createProjectResponse.Id
            ?? throw new InvalidOperationException("プロジェクトの作成に失敗しました");
        Console.WriteLine($"  - 作成したプロジェクトID: {createProjectResponse.Id}");
        Console.WriteLine($"    - Name: {createProjectResponse.Name}");
        Console.WriteLine($"    - Code: {createProjectResponse.Code}");

        // -------------------------------------------------------
        // GET 指定したプロジェクトを取得する
        Console.WriteLine($"GET /projects/{projectId} | 指定したプロジェクトを取得する");
        var getProjectResponse = await client.Projects[projectId].GetAsync();
        Console.WriteLine($"  - 取得したプロジェクトID: {getProjectResponse.Id}");
        Console.WriteLine($"    - Name: {getProjectResponse.Name}");

        // -------------------------------------------------------
        // PUT 指定したプロジェクトを更新する
        Console.WriteLine($"PUT /projects/{projectId} | 指定したプロジェクトを更新する");
        var updateProjectResponse = await client.Projects[projectId].PutAsync(new Projects.Item.WithProject_PutRequestBody
        {
            Name = getProjectResponse.Name,
            Description = getProjectResponse.Description,
        });
        Console.WriteLine($"  - 更新したプロジェクトID: {updateProjectResponse.Id}");
        Console.WriteLine($"    - Name: {updateProjectResponse.Name}");

        // -------------------------------------------------------
        // GET プロジェクト一覧を取得する
        Console.WriteLine("GET /projects | プロジェクト一覧を取得する");
        var listProjectsResponse = await client.Projects.GetAsync();
        Console.WriteLine($"  - 取得したプロジェクト数: {listProjectsResponse.Items.Count}");

        // -------------------------------------------------------
        // POST プロジェクトを一括で移動する
        Console.WriteLine("POST /move-projects | プロジェクトを一括で移動する");
        await client.MoveProjects.PostAsync(new Models.MoveProjects
        {
            ProjectIds = [projectId],
            ParentFolderId = folderId,
        });
        Console.WriteLine("  - プロジェクトの移動が完了しました (レスポンスボディ無し)");

        // -------------------------------------------------------
        // DELETE 指定したプロジェクトを削除する
        Console.WriteLine($"DELETE /projects/{projectId} | 指定したプロジェクトを削除する");
        await client.Projects[projectId].DeleteAsync();
        Console.WriteLine("  - プロジェクトの削除が完了しました (レスポンスボディ無し)");

        Console.WriteLine("プロジェクトのサンプルの実行が完了しました");
    }
}
