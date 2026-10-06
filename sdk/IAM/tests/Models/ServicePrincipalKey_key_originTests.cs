using System.Reflection;
using System.Runtime.Serialization;
using Sacloud.Sdk.IAM.Models;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// ServicePrincipalKey_key_originのテスト
/// </summary>
public class ServicePrincipalKey_key_originTests
{
    /// <summary>
    /// Enum値が正しく定義されていることを確認
    /// </summary>
    [Fact]
    public void EnumValues_AreDefinedCorrectly()
    {
        var user = ServicePrincipalKey_key_origin.User;

        Assert.Equal(ServicePrincipalKey_key_origin.User, user);
    }

    /// <summary>
    /// Enum値の数が正しいことを確認
    /// </summary>
    [Fact]
    public void EnumValues_HasCorrectCount()
    {
        var values = Enum.GetValues(typeof(ServicePrincipalKey_key_origin));

        Assert.Single(values);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: User
    /// </summary>
    [Fact]
    public void User_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(ServicePrincipalKey_key_origin).GetField(nameof(ServicePrincipalKey_key_origin.User));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("user", attribute.Value);
    }
}
