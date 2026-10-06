using Sacloud.Sdk.IAM.Models;
using System.Reflection;
using System.Runtime.Serialization;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// AuthConditions_ip_restrictionMember1_modeのテスト
/// </summary>
public class AuthConditions_ip_restrictionMember1_modeTests
{
    /// <summary>
    /// Enum値が正しく定義されていることを確認
    /// </summary>
    [Fact]
    public void EnumValues_AreDefinedCorrectly()
    {
        var allowAll = AuthConditions_ip_restrictionMember1_mode.Allow_all;

        Assert.Equal(AuthConditions_ip_restrictionMember1_mode.Allow_all, allowAll);
    }

    /// <summary>
    /// Enum値の数が正しいことを確認
    /// </summary>
    [Fact]
    public void EnumValues_HasCorrectCount()
    {
        var values = Enum.GetValues(typeof(AuthConditions_ip_restrictionMember1_mode));

        Assert.Single(values);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Allow_all
    /// </summary>
    [Fact]
    public void Allow_all_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(AuthConditions_ip_restrictionMember1_mode).GetField(nameof(AuthConditions_ip_restrictionMember1_mode.Allow_all));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("allow_all", attribute.Value);
    }
}
