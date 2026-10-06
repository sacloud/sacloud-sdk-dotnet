using System.Reflection;
using System.Runtime.Serialization;
using Sacloud.Sdk.IAM.Models;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// ServicePrincipalJWTGrantRequest_grant_typeのテスト
/// </summary>
public class ServicePrincipalJWTGrantRequest_grant_typeTests
{
    /// <summary>
    /// Enum値が正しく定義されていることを確認
    /// </summary>
    [Fact]
    public void EnumValues_AreDefinedCorrectly()
    {
        var urnIetfParamsOauthGrantTypeJwtBearer = ServicePrincipalJWTGrantRequest_grant_type.UrnIetfParamsOauthGrantTypeJwtBearer;

        Assert.Equal(ServicePrincipalJWTGrantRequest_grant_type.UrnIetfParamsOauthGrantTypeJwtBearer, urnIetfParamsOauthGrantTypeJwtBearer);
    }

    /// <summary>
    /// Enum値の数が正しいことを確認
    /// </summary>
    [Fact]
    public void EnumValues_HasCorrectCount()
    {
        var values = Enum.GetValues(typeof(ServicePrincipalJWTGrantRequest_grant_type));

        Assert.Single(values);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: UrnIetfParamsOauthGrantTypeJwtBearer
    /// </summary>
    [Fact]
    public void UrnIetfParamsOauthGrantTypeJwtBearer_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(ServicePrincipalJWTGrantRequest_grant_type).GetField(nameof(ServicePrincipalJWTGrantRequest_grant_type.UrnIetfParamsOauthGrantTypeJwtBearer));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("urn:ietf:params:oauth:grant-type:jwt-bearer", attribute.Value);
    }
}
