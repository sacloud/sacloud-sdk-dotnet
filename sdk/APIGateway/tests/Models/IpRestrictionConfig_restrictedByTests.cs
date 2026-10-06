using System.Reflection;
using System.Runtime.Serialization;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// IpRestrictionConfig_restrictedByのテスト
/// </summary>
public class IpRestrictionConfig_restrictedByTests
{
    /// <summary>
    /// Enum値の数が正しいことを検証
    /// </summary>
    [Fact]
    public void EnumValues_HasCorrectCount()
    {
        var values = Enum.GetValues<IpRestrictionConfig_restrictedBy>();
        Assert.Equal(2, values.Length);
    }

    /// <summary>
    /// AllowIpsメンバーのEnumMember属性が "allowIps" であることを検証
    /// </summary>
    [Fact]
    public void IpRestrictionConfig_restrictedBy_AllowIps_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(IpRestrictionConfig_restrictedBy).GetField(nameof(IpRestrictionConfig_restrictedBy.AllowIps));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("allowIps", attribute.Value);
    }

    /// <summary>
    /// DenyIpsメンバーのEnumMember属性が "denyIps" であることを検証
    /// </summary>
    [Fact]
    public void IpRestrictionConfig_restrictedBy_DenyIps_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(IpRestrictionConfig_restrictedBy).GetField(nameof(IpRestrictionConfig_restrictedBy.DenyIps));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("denyIps", attribute.Value);
    }
}
