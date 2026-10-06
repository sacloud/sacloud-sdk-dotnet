using System.Reflection;
using System.Runtime.Serialization;
using Sacloud.Sdk.AppRunDedicated.Models;

namespace Sacloud.Sdk.AppRunDedicated.Tests.Models;

/// <summary>
/// ScalingMode Enumのテスト
/// </summary>
public class ScalingModeTests
{
    /// <summary>
    /// すべてのEnum値が定義されていることを確認
    /// </summary>
    [Fact]
    public void ScalingMode_HasExpectedValues()
    {
        var values = Enum.GetValues<ScalingMode>();
        Assert.Contains(ScalingMode.Manual, values);
        Assert.Contains(ScalingMode.Cpu, values);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Manual
    /// </summary>
    [Fact]
    public void ScalingMode_Manual_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(ScalingMode).GetField(nameof(ScalingMode.Manual));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("manual", attribute.Value);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Cpu
    /// </summary>
    [Fact]
    public void ScalingMode_Cpu_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(ScalingMode).GetField(nameof(ScalingMode.Cpu));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("cpu", attribute.Value);
    }
}
