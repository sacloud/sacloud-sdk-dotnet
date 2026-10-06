using System.Reflection;
using System.Runtime.Serialization;
using Sacloud.Sdk.IAM.Models;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// IdPolicy_role_typeのテスト
/// </summary>
public class IdPolicy_role_typeTests
{
    /// <summary>
    /// Enum値が正しく定義されていることを確認
    /// </summary>
    [Fact]
    public void EnumValues_AreDefinedCorrectly()
    {
        var preset = IdPolicy_role_type.Preset;

        Assert.Equal(IdPolicy_role_type.Preset, preset);
    }

    /// <summary>
    /// Enum値の数が正しいことを確認
    /// </summary>
    [Fact]
    public void EnumValues_HasCorrectCount()
    {
        var values = Enum.GetValues(typeof(IdPolicy_role_type));

        Assert.Single(values);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Preset
    /// </summary>
    [Fact]
    public void IdPolicy_role_type_Preset_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(IdPolicy_role_type).GetField(nameof(IdPolicy_role_type.Preset));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("preset", attribute.Value);
    }
}
