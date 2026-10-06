using System.Reflection;
using System.Runtime.Serialization;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// Oidc_authenticationMethodsのテスト
/// </summary>
public class Oidc_authenticationMethodsTests
{
    /// <summary>
    /// Enum値の数が正しいことを検証
    /// </summary>
    [Fact]
    public void EnumValues_HasCorrectCount()
    {
        var values = Enum.GetValues<Oidc_authenticationMethods>();
        Assert.Equal(2, values.Length);
    }

    /// <summary>
    /// AuthorizationCodeFlowメンバーのEnumMember属性が "authorizationCodeFlow" であることを検証
    /// </summary>
    [Fact]
    public void Oidc_authenticationMethods_AuthorizationCodeFlow_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(Oidc_authenticationMethods).GetField(nameof(Oidc_authenticationMethods.AuthorizationCodeFlow));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("authorizationCodeFlow", attribute.Value);
    }

    /// <summary>
    /// AccessTokenメンバーのEnumMember属性が "accessToken" であることを検証
    /// </summary>
    [Fact]
    public void Oidc_authenticationMethods_AccessToken_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(Oidc_authenticationMethods).GetField(nameof(Oidc_authenticationMethods.AccessToken));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("accessToken", attribute.Value);
    }
}
