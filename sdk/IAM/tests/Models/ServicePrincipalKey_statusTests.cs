using System.Reflection;
using System.Runtime.Serialization;
using Sacloud.Sdk.IAM.Models;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// ServicePrincipalKey_statusのテスト
/// </summary>
public class ServicePrincipalKey_statusTests
{
    /// <summary>
    /// Enum値が正しく定義されていることを確認
    /// </summary>
    [Fact]
    public void EnumValues_AreDefinedCorrectly()
    {
        var enabled = ServicePrincipalKey_status.Enabled;
        var disabled = ServicePrincipalKey_status.Disabled;

        Assert.Equal(ServicePrincipalKey_status.Enabled, enabled);
        Assert.Equal(ServicePrincipalKey_status.Disabled, disabled);
    }

    /// <summary>
    /// Enum値の数が正しいことを確認
    /// </summary>
    [Fact]
    public void EnumValues_HasCorrectCount()
    {
        var values = Enum.GetValues(typeof(ServicePrincipalKey_status));

        Assert.Equal(2, values.Length);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Enabled
    /// </summary>
    [Fact]
    public void Enabled_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(ServicePrincipalKey_status).GetField(nameof(ServicePrincipalKey_status.Enabled));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("enabled", attribute.Value);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Disabled
    /// </summary>
    [Fact]
    public void Disabled_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(ServicePrincipalKey_status).GetField(nameof(ServicePrincipalKey_status.Disabled));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("disabled", attribute.Value);
    }
}
