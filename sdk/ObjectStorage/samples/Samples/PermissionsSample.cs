using Sacloud.Sdk.ObjectStorage.Models;

namespace Sacloud.Sdk.ObjectStorage.Samples.Samples;

sealed class PermissionsSample(ObjectStorageClient client) : AbstractSample(client)
{
    // GET パーミッション一覧の取得
    public async Task GetPermissionsAsync()
    {
        Console.WriteLine("\nGET /permissions | パーミッション一覧の取得\n");

        var response = await client.Permissions.GetAsync();

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機

        var permissions = response?.Data
            ?? throw new InvalidOperationException("パーミッション一覧(data)が null です。");
        foreach (var permission in permissions)
        {
            Console.WriteLine($"- ID: {permission.Id}");
            Console.WriteLine($"  DisplayName: {permission.DisplayName}");
            Console.WriteLine($"  CreatedAt: {permission.CreatedAt}");
            Console.WriteLine($"  BucketControls:");
            if (permission.BucketControls is not null)
            {
                foreach (var bucketControl in permission.BucketControls)
                {
                    Console.WriteLine($"    - BucketName: {bucketControl.BucketName}");
                    Console.WriteLine($"      CanRead: {bucketControl.CanRead}");
                    Console.WriteLine($"      CanWrite: {bucketControl.CanWrite}");
                    Console.WriteLine($"      CreatedAt: {bucketControl.CreatedAt}");
                }
            }
        }
    }

    // POST パーミッションの作成
    public async Task CreatePermissionAsync()
    {
        Console.WriteLine("\nPOST /permissions | パーミッションの作成\n");

        var displayName = ReadRequiredValue("DisplayName");
        if (displayName is null) return;

        var bucketName = ReadRequiredValue("BucketName");
        if (bucketName is null) return;

        var canReadInput = ReadRequiredValue("CanRead");
        if (canReadInput is null) return;
        if (!bool.TryParse(canReadInput, out var canRead))
        {
            Console.WriteLine("CanReadは true または false を指定してください。");
            return;
        }

        var canWriteInput = ReadRequiredValue("CanWrite");
        if (canWriteInput is null) return;
        if (!bool.TryParse(canWriteInput, out var canWrite))
        {
            Console.WriteLine("CanWriteは true または false を指定してください。");
            return;
        }

        var requestBody = new PermissionBucketControlsBody
        {
            DisplayName = displayName,
            BucketControls =
            [
                new BucketControlsBodyItem
                {
                    BucketName = bucketName,
                    CanRead = canRead,
                    CanWrite = canWrite,
                }
            ]
        };
        var response = await client.Permissions.PostAsync(requestBody);

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機

        var permission = response?.Data
            ?? throw new InvalidOperationException("作成したパーミッション情報(data)が null です。");
        Console.WriteLine($"ID: {permission.Id}");
        Console.WriteLine($"DisplayName: {permission.DisplayName}");
        Console.WriteLine($"CreatedAt: {permission.CreatedAt}");
        Console.WriteLine($"BucketControls:");
        if (permission.BucketControls is not null)
        {
            foreach (var bucketControl in permission.BucketControls)
            {
                Console.WriteLine($"  - BucketName: {bucketControl.BucketName}");
                Console.WriteLine($"    CanRead: {bucketControl.CanRead}");
                Console.WriteLine($"    CanWrite: {bucketControl.CanWrite}");
                Console.WriteLine($"    CreatedAt: {bucketControl.CreatedAt}");
            }
        }
    }

    // GET パーミッションの取得
    public async Task GetPermissionAsync()
    {
        Console.WriteLine("\nGET /permissions/{permissionId} | パーミッションの取得\n");

        var permissionIdInput = ReadRequiredValue("PermissionId");
        if (permissionIdInput is null) return;
        if (!int.TryParse(permissionIdInput, out var permissionId))
        {
            Console.WriteLine("PermissionIdは整数で指定してください。");
            return;
        }

        var response = await client.Permissions[permissionId].GetAsync();

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機

        var permission = response?.Data
            ?? throw new InvalidOperationException("パーミッション情報(data)が null です。");
        Console.WriteLine($"ID: {permission.Id}");
        Console.WriteLine($"DisplayName: {permission.DisplayName}");
        Console.WriteLine($"CreatedAt: {permission.CreatedAt}");
        Console.WriteLine("BucketControls:");
        if (permission.BucketControls is not null)
        {
            foreach (var bucketControl in permission.BucketControls)
            {
                Console.WriteLine($"  - BucketName: {bucketControl.BucketName}");
                Console.WriteLine($"    CanRead: {bucketControl.CanRead}");
                Console.WriteLine($"    CanWrite: {bucketControl.CanWrite}");
                Console.WriteLine($"    CreatedAt: {bucketControl.CreatedAt}");
            }
        }
    }

    // PUT パーミッションの更新
    public async Task UpdatePermissionAsync()
    {
        Console.WriteLine("\nPUT /permissions/{permissionId} | パーミッションの更新\n");

        var permissionIdInput = ReadRequiredValue("PermissionId");
        if (permissionIdInput is null) return;
        if (!int.TryParse(permissionIdInput, out var permissionId))
        {
            Console.WriteLine("PermissionIdは整数で指定してください。");
            return;
        }

        var displayName = ReadRequiredValue("DisplayName");
        if (displayName is null) return;

        var bucketName = ReadRequiredValue("BucketName");
        if (bucketName is null) return;

        var canReadInput = ReadRequiredValue("CanRead");
        if (canReadInput is null) return;
        if (!bool.TryParse(canReadInput, out var canRead))
        {
            Console.WriteLine("CanReadは true または false を指定してください。");
            return;
        }

        var canWriteInput = ReadRequiredValue("CanWrite");
        if (canWriteInput is null) return;
        if (!bool.TryParse(canWriteInput, out var canWrite))
        {
            Console.WriteLine("CanWriteは true または false を指定してください。");
            return;
        }

        var requestBody = new PermissionBucketControlsBody
        {
            DisplayName = displayName,
            BucketControls =
            [
                new BucketControlsBodyItem
                {
                    BucketName = bucketName,
                    CanRead = canRead,
                    CanWrite = canWrite,
                }
            ]
        };
        var response = await client.Permissions[permissionId].PutAsync(requestBody);

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機

        var permission = response?.Data
            ?? throw new InvalidOperationException("更新したパーミッション情報(data)が null です。");
        Console.WriteLine($"ID: {permission.Id}");
        Console.WriteLine($"DisplayName: {permission.DisplayName}");
        Console.WriteLine($"CreatedAt: {permission.CreatedAt}");
        Console.WriteLine($"BucketControls:");
        if (permission.BucketControls is not null)
        {
            foreach (var bucketControl in permission.BucketControls)
            {
                Console.WriteLine($"  - BucketName: {bucketControl.BucketName}");
                Console.WriteLine($"    CanRead: {bucketControl.CanRead}");
                Console.WriteLine($"    CanWrite: {bucketControl.CanWrite}");
                Console.WriteLine($"    CreatedAt: {bucketControl.CreatedAt}");
            }
        }
    }

    // DELETE パーミッションの削除
    public async Task DeletePermissionAsync()
    {
        Console.WriteLine("\nDELETE /permissions/{permissionId} | パーミッションの削除\n");

        var permissionIdInput = ReadRequiredValue("PermissionId");
        if (permissionIdInput is null) return;
        if (!int.TryParse(permissionIdInput, out var permissionId))
        {
            Console.WriteLine("PermissionIdは整数で指定してください。");
            return;
        }

        await client.Permissions[permissionId].DeleteAsync();

    await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機

        Console.WriteLine("パーミッションを削除しました。（レスポンスボディ無し）");
    }

    // GET パーミッションが保有するアクセスキー一覧の取得
    public async Task GetPermissionKeysAsync()
    {
        Console.WriteLine("\nGET /permissions/{permissionId}/keys | パーミッションが保有するアクセスキー一覧の取得\n");

        var permissionIdInput = ReadRequiredValue("PermissionId");
        if (permissionIdInput is null) return;
        if (!int.TryParse(permissionIdInput, out var permissionId))
        {
            Console.WriteLine("PermissionIdは整数で指定してください。");
            return;
        }

        var response = await client.Permissions[permissionId].Keys.GetAsync();

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機

        var keys = response?.Data
            ?? throw new InvalidOperationException("パーミッションが保有するアクセスキー一覧(data)が null です。");
        foreach (var key in keys)
        {
            Console.WriteLine($"- ID: {key.Id}");
            Console.WriteLine($"  CreatedAt: {key.CreatedAt}");
        }
    }

    // POST パーミッションのアクセスキーの発行
    public async Task CreatePermissionKeyAsync()
    {
        Console.WriteLine("\nPOST /permissions/{permissionId}/keys | パーミッションのアクセスキーの発行\n");

        var permissionIdInput = ReadRequiredValue("PermissionId");
        if (permissionIdInput is null) return;
        if (!int.TryParse(permissionIdInput, out var permissionId))
        {
            Console.WriteLine("PermissionIdは整数で指定してください。");
            return;
        }

        var response = await client.Permissions[permissionId].Keys.PostAsync();

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機

        var key = response?.Data
            ?? throw new InvalidOperationException("パーミッションのアクセスキー情報(data)が null です。");
        Console.WriteLine($"ID: {key.Id}");
        Console.WriteLine($"Secret: {key.Secret}");
        Console.WriteLine($"CreatedAt: {key.CreatedAt}");
    }

    // GET パーミッションが保有するアクセスキーの取得
    public async Task GetPermissionKeyAsync()
    {
        Console.WriteLine("\nGET /permissions/{permissionId}/keys/{keyId} | パーミッションが保有するアクセスキーの取得\n");

        var permissionIdInput = ReadRequiredValue("PermissionId");
        if (permissionIdInput is null) return;
        if (!int.TryParse(permissionIdInput, out var permissionId))
        {
            Console.WriteLine("PermissionIdは整数で指定してください。");
            return;
        }

        var keyId = ReadRequiredValue("KeyId");
        if (keyId is null) return;

        var response = await client.Permissions[permissionId].Keys[keyId].GetAsync();

        await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機

        var key = response?.Data
            ?? throw new InvalidOperationException("パーミッションのアクセスキー情報(data)が null です。");
        Console.WriteLine($"ID: {key.Id}");
        Console.WriteLine($"CreatedAt: {key.CreatedAt}");
    }

    // DELETE パーミッションが保有するアクセスキーの削除
    public async Task DeletePermissionKeyAsync()
    {
        Console.WriteLine("\nDELETE /permissions/{permissionId}/keys/{keyId} | パーミッションが保有するアクセスキーの削除\n");

        var permissionIdInput = ReadRequiredValue("PermissionId");
        if (permissionIdInput is null) return;
        if (!int.TryParse(permissionIdInput, out var permissionId))
        {
            Console.WriteLine("PermissionIdは整数で指定してください。");
            return;
        }

        var keyId = ReadRequiredValue("KeyId");
        if (keyId is null) return;

        await client.Permissions[permissionId].Keys[keyId].DeleteAsync();

    await Task.Delay(TimeSpan.FromSeconds(DELAY_SECONDS)); // ログの可読性のため少し待機

        Console.WriteLine("パーミッションのアクセスキーを削除しました。（レスポンスボディ無し）");
    }
}
