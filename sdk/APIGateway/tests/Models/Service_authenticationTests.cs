using System.Reflection;
using System.Runtime.Serialization;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// Service_authenticationのテスト
/// </summary>
public class Service_authenticationTests
{
    /// <summary>
    /// Enum値の数が正しいことを検証
    /// </summary>
    [Fact]
    public void EnumValues_HasCorrectCount()
    {
        var values = Enum.GetValues<Service_authentication>();
        Assert.Equal(5, values.Length);
    }

    /// <summary>
    /// NoneメンバーのEnumMember属性が "none" であることを検証
    /// </summary>
    [Fact]
    public void Service_authentication_None_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(Service_authentication).GetField(nameof(Service_authentication.None));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("none", attribute.Value);
    }

    /// <summary>
    /// BasicメンバーのEnumMember属性が "basic" であることを検証
    /// </summary>
    [Fact]
    public void Service_authentication_Basic_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(Service_authentication).GetField(nameof(Service_authentication.Basic));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("basic", attribute.Value);
    }

    /// <summary>
    /// HmacメンバーのEnumMember属性が "hmac" であることを検証
    /// </summary>
    [Fact]
    public void Service_authentication_Hmac_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(Service_authentication).GetField(nameof(Service_authentication.Hmac));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("hmac", attribute.Value);
    }

    /// <summary>
    /// JwtメンバーのEnumMember属性が "jwt" であることを検証
    /// </summary>
    [Fact]
    public void Service_authentication_Jwt_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(Service_authentication).GetField(nameof(Service_authentication.Jwt));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("jwt", attribute.Value);
    }

    /// <summary>
    /// OidcメンバーのEnumMember属性が "oidc" であることを検証
    /// </summary>
    [Fact]
    public void Service_authentication_Oidc_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(Service_authentication).GetField(nameof(Service_authentication.Oidc));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("oidc", attribute.Value);
    }
}
