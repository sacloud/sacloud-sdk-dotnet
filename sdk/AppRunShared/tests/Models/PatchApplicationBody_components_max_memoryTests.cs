using System.Reflection;
using System.Runtime.Serialization;
using Sacloud.Sdk.AppRunShared.Models;

namespace Sacloud.Sdk.AppRunShared.Tests.Models;

/// <summary>
/// PatchApplicationBody_components_max_memory Enumのテスト
/// </summary>
public class PatchApplicationBody_components_max_memoryTests
{
    /// <summary>
    /// すべてのEnum値が定義されていることを確認
    /// </summary>
    [Fact]
    public void PatchApplicationBody_components_max_memory_HasExpectedValues()
    {
        var values = Enum.GetValues<PatchApplicationBody_components_max_memory>();
        Assert.Contains(PatchApplicationBody_components_max_memory.OneGi, values);
        Assert.Contains(PatchApplicationBody_components_max_memory.TwoGi, values);
        Assert.Contains(PatchApplicationBody_components_max_memory.FourGi, values);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: OneGi
    /// </summary>
    [Fact]
    public void PatchApplicationBody_components_max_memory_OneGi_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(PatchApplicationBody_components_max_memory).GetField(nameof(PatchApplicationBody_components_max_memory.OneGi));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("1Gi", attribute.Value);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: TwoGi
    /// </summary>
    [Fact]
    public void PatchApplicationBody_components_max_memory_TwoGi_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(PatchApplicationBody_components_max_memory).GetField(nameof(PatchApplicationBody_components_max_memory.TwoGi));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("2Gi", attribute.Value);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: FourGi
    /// </summary>
    [Fact]
    public void PatchApplicationBody_components_max_memory_FourGi_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(PatchApplicationBody_components_max_memory).GetField(nameof(PatchApplicationBody_components_max_memory.FourGi));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("4Gi", attribute.Value);
    }
}
