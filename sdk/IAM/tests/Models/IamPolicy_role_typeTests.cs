using System.Reflection;
using System.Runtime.Serialization;
using Sacloud.Sdk.IAM.Models;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// IamPolicy_role_typeのテスト
/// </summary>
public class IamPolicy_role_typeTests
{
    /// <summary>
    /// Enum値が正しく定義されていることを確認
    /// </summary>
    [Fact]
    public void EnumValues_AreDefinedCorrectly()
    {
        var preset = IamPolicy_role_type.Preset;

        Assert.Equal(IamPolicy_role_type.Preset, preset);
    }

    /// <summary>
    /// Enum値の数が正しいことを確認
    /// </summary>
    [Fact]
    public void EnumValues_HasCorrectCount()
    {
        var values = Enum.GetValues(typeof(IamPolicy_role_type));

        Assert.Single(values);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Preset
    /// </summary>
    [Fact]
    public void IamPolicy_role_type_Preset_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(IamPolicy_role_type).GetField(nameof(IamPolicy_role_type.Preset));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("preset", attribute.Value);
    }
}
