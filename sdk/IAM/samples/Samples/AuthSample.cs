using Sacloud.Sdk.IAM.Models;

namespace Sacloud.Sdk.IAM.Samples.Samples;

class AuthSample(IAMClient client)
{
    private readonly IAMClient client = client
        ?? throw new ArgumentNullException(nameof(client));

    public async Task RunAsync()
    {
        Console.WriteLine("認証のサンプルを実行します");

        // -------------------------------------------------------
        // GET 組織のパスワードポリシーを取得する
        var getPasswordPolicyResponse = await client.OrganizationPasswordPolicy.GetAsync();
        Console.WriteLine("GET /organization-password-policy | 組織のパスワードポリシーを取得する");
        Console.WriteLine($"  - RequireUppercase: {getPasswordPolicyResponse.RequireUppercase}");
        Console.WriteLine($"  - RequireLowercase: {getPasswordPolicyResponse.RequireLowercase}");

        // -------------------------------------------------------
        // PUT 組織のパスワードポリシーを更新する
        var updatePasswordPolicyResponse = await client.OrganizationPasswordPolicy.PutAsync(new PasswordPolicy
        {
            MinLength = getPasswordPolicyResponse.MinLength,
            RequireUppercase = getPasswordPolicyResponse.RequireUppercase,
            RequireLowercase = getPasswordPolicyResponse.RequireLowercase,
            RequireSymbols = getPasswordPolicyResponse.RequireSymbols,
        });
        Console.WriteLine("PUT /organization-password-policy | 組織のパスワードポリシーを更新する");
        Console.WriteLine($"  - RequireUppercase: {updatePasswordPolicyResponse.RequireUppercase}");
        Console.WriteLine($"  - RequireLowercase: {updatePasswordPolicyResponse.RequireLowercase}");

        // -------------------------------------------------------
        // GET 組織の認証条件を取得する
        var getAuthConditionResponse = await client.OrganizationAuthConditions.GetAsync();
        Console.WriteLine("GET /organization-auth-conditions | 組織の認証条件を取得する");
        Console.WriteLine($"  - IpRestriction:");
        if (getAuthConditionResponse.IpRestriction.AuthConditionsIpRestrictionMember1 is not null)
        {
            Console.WriteLine($"    - Mode: {getAuthConditionResponse.IpRestriction.AuthConditionsIpRestrictionMember1.Mode}");
        }
        if (getAuthConditionResponse.IpRestriction.AuthConditionsIpRestrictionMember2 is not null)
        {
            Console.WriteLine($"    - Mode: {getAuthConditionResponse.IpRestriction.AuthConditionsIpRestrictionMember2.Mode}");
            Console.WriteLine($"    - SourceNetwork: {getAuthConditionResponse.IpRestriction.AuthConditionsIpRestrictionMember2.SourceNetwork}");
        }
        Console.WriteLine($"  - RequireTwoFactorAuth:");
        Console.WriteLine($"    - Enabled: {getAuthConditionResponse.RequireTwoFactorAuth.Enabled}");
        Console.WriteLine($"  - DatetimeRestriction:");
        Console.WriteLine($"    - After: {getAuthConditionResponse.DatetimeRestriction.After}");
        Console.WriteLine($"    - Before: {getAuthConditionResponse.DatetimeRestriction.Before}");

        // -------------------------------------------------------
        // PUT 組織の認証条件を更新する
        var updateAuthConditionResponse = await client.OrganizationAuthConditions.PutAsync(new AuthConditions
        {
            IpRestriction = getAuthConditionResponse.IpRestriction,
            RequireTwoFactorAuth = new AuthConditions_require_two_factor_auth
            {
                Enabled = getAuthConditionResponse.RequireTwoFactorAuth.Enabled,
            },
            DatetimeRestriction = new AuthConditions_datetime_restriction
            {
                After = getAuthConditionResponse.DatetimeRestriction.After,
                Before = getAuthConditionResponse.DatetimeRestriction.Before,
            }
        });
        Console.WriteLine("PUT /organization-auth-conditions | 組織の認証条件を更新する");
        Console.WriteLine($"  - IpRestriction:");
        if (updateAuthConditionResponse.IpRestriction.AuthConditionsIpRestrictionMember1 is not null)
        {
            Console.WriteLine($"    - Mode: {updateAuthConditionResponse.IpRestriction.AuthConditionsIpRestrictionMember1.Mode}");
        }
        if (updateAuthConditionResponse.IpRestriction.AuthConditionsIpRestrictionMember2 is not null)
        {
            Console.WriteLine($"    - Mode: {updateAuthConditionResponse.IpRestriction.AuthConditionsIpRestrictionMember2.Mode}");
            Console.WriteLine($"    - SourceNetwork: {updateAuthConditionResponse.IpRestriction.AuthConditionsIpRestrictionMember2.SourceNetwork}");
        }
        Console.WriteLine($"  - RequireTwoFactorAuth:");
        Console.WriteLine($"    - Enabled: {updateAuthConditionResponse.RequireTwoFactorAuth.Enabled}");
        Console.WriteLine($"  - DatetimeRestriction:");
        Console.WriteLine($"    - After: {updateAuthConditionResponse.DatetimeRestriction.After}");
        Console.WriteLine($"    - Before: {updateAuthConditionResponse.DatetimeRestriction.Before}");

        // -------------------------------------------------------
        // GET 認証情報のコンテキストを取得する
        Console.WriteLine("GET /auth/context | 認証情報のコンテキストを取得する");
        var getAuthContextResponse = await client.Auth.Context.GetAsync();
        Console.WriteLine($"  - ResourceId: {getAuthContextResponse.ResourceId}");
        Console.WriteLine($"  - AuthType: {getAuthContextResponse.AuthType}");
        Console.WriteLine($"  - LimitedToProjectId: {getAuthContextResponse.LimitedToProjectId}");
        Console.WriteLine($"  - MemberCode: {getAuthContextResponse.MemberCode}");

        Console.WriteLine("認証のサンプルの実行が完了しました");
    }
}
