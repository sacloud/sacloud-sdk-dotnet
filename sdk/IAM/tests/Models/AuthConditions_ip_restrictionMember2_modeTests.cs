using Sacloud.Sdk.IAM.Models;
using System.Reflection;
using System.Runtime.Serialization;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// AuthConditions_ip_restrictionMember2_modeのテスト
/// </summary>
public class AuthConditions_ip_restrictionMember2_modeTests
{
    /// <summary>
    /// Enum値が正しく定義されていることを確認
    /// </summary>
    [Fact]
    public void EnumValues_AreDefinedCorrectly()
    {
        var allowList = AuthConditions_ip_restrictionMember2_mode.Allow_list;

        Assert.Equal(AuthConditions_ip_restrictionMember2_mode.Allow_list, allowList);
    }

    /// <summary>
    /// Enum値の数が正しいことを確認
    /// </summary>
    [Fact]
    public void EnumValues_HasCorrectCount()
    {
        var values = Enum.GetValues(typeof(AuthConditions_ip_restrictionMember2_mode));

        Assert.Single(values);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Allow_list
    /// </summary>
    [Fact]
    public void Allow_list_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(AuthConditions_ip_restrictionMember2_mode).GetField(nameof(AuthConditions_ip_restrictionMember2_mode.Allow_list));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("allow_list", attribute.Value);
    }
}
