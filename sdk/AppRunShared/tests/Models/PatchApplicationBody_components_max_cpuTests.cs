using System.Reflection;
using System.Runtime.Serialization;
using Sacloud.Sdk.AppRunShared.Models;

namespace Sacloud.Sdk.AppRunShared.Tests.Models;

/// <summary>
/// PatchApplicationBody_components_max_cpu Enumのテスト
/// </summary>
public class PatchApplicationBody_components_max_cpuTests
{
    /// <summary>
    /// すべてのEnum値が定義されていることを確認
    /// </summary>
    [Fact]
    public void PatchApplicationBody_components_max_cpu_HasExpectedValues()
    {
        var values = Enum.GetValues<PatchApplicationBody_components_max_cpu>();
        Assert.Contains(PatchApplicationBody_components_max_cpu.ZeroFive, values);
        Assert.Contains(PatchApplicationBody_components_max_cpu.One, values);
        Assert.Contains(PatchApplicationBody_components_max_cpu.Two, values);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: ZeroFive
    /// </summary>
    [Fact]
    public void PatchApplicationBody_components_max_cpu_ZeroFive_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(PatchApplicationBody_components_max_cpu).GetField(nameof(PatchApplicationBody_components_max_cpu.ZeroFive));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("0.5", attribute.Value);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: One
    /// </summary>
    [Fact]
    public void PatchApplicationBody_components_max_cpu_One_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(PatchApplicationBody_components_max_cpu).GetField(nameof(PatchApplicationBody_components_max_cpu.One));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("1", attribute.Value);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Two
    /// </summary>
    [Fact]
    public void PatchApplicationBody_components_max_cpu_Two_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(PatchApplicationBody_components_max_cpu).GetField(nameof(PatchApplicationBody_components_max_cpu.Two));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("2", attribute.Value);
    }
}
