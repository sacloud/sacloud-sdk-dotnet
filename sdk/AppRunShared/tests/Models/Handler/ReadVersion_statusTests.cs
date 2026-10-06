using System.Reflection;
using System.Runtime.Serialization;
using Sacloud.Sdk.AppRunShared.Models.Handler;

namespace Sacloud.Sdk.AppRunShared.Tests.Models.Handler;

/// <summary>
/// ReadVersion_statusモデルのテスト
/// </summary>
public class ReadVersion_statusTests
{
    /// <summary>
    /// Enum ReadVersion_statusが期待される値を持つことを確認
    /// </summary>
    [Fact]
    public void ReadVersion_status_HasExpectedValues()
    {
        var values = Enum.GetValues<ReadVersion_status>();
        Assert.Contains(ReadVersion_status.Healthy, values);
        Assert.Contains(ReadVersion_status.UnHealthy, values);
        Assert.Contains(ReadVersion_status.Deploying, values);
    }

    /// <summary>
    /// Healthyが正しいEnumMember属性を持つことを確認
    /// </summary>
    [Fact]
    public void ReadVersion_status_Healthy_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(ReadVersion_status).GetField(nameof(ReadVersion_status.Healthy));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("Healthy", attribute.Value);
    }

    /// <summary>
    /// UnHealthyが正しいEnumMember属性を持つことを確認
    /// </summary>
    [Fact]
    public void ReadVersion_status_UnHealthy_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(ReadVersion_status).GetField(nameof(ReadVersion_status.UnHealthy));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("UnHealthy", attribute.Value);
    }

    /// <summary>
    /// Deployingが正しいEnumMember属性を持つことを確認
    /// </summary>
    [Fact]
    public void ReadVersion_status_Deploying_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(ReadVersion_status).GetField(nameof(ReadVersion_status.Deploying));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("Deploying", attribute.Value);
    }
}
