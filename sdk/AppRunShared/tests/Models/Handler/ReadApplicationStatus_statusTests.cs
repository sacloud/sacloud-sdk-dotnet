using System.Reflection;
using System.Runtime.Serialization;
using Sacloud.Sdk.AppRunShared.Models.Handler;

namespace Sacloud.Sdk.AppRunShared.Tests.Models.Handler;

/// <summary>
/// ReadApplicationStatus_statusモデルのテスト
/// </summary>
public class ReadApplicationStatus_statusTests
{
    /// <summary>
    /// Enum ReadApplicationStatus_statusが期待される値を持つことを確認
    /// </summary>
    [Fact]
    public void ReadApplicationStatus_status_HasExpectedValues()
    {
        var values = Enum.GetValues<ReadApplicationStatus_status>();
        Assert.Contains(ReadApplicationStatus_status.Healthy, values);
        Assert.Contains(ReadApplicationStatus_status.UnHealthy, values);
        Assert.Contains(ReadApplicationStatus_status.Deploying, values);
    }

    /// <summary>
    /// Healthyが正しいEnumMember属性を持つことを確認
    /// </summary>
    [Fact]
    public void ReadApplicationStatus_status_Healthy_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(ReadApplicationStatus_status).GetField(nameof(ReadApplicationStatus_status.Healthy));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("Healthy", attribute.Value);
    }

    /// <summary>
    /// UnHealthyが正しいEnumMember属性を持つことを確認
    /// </summary>
    [Fact]
    public void ReadApplicationStatus_status_UnHealthy_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(ReadApplicationStatus_status).GetField(nameof(ReadApplicationStatus_status.UnHealthy));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("UnHealthy", attribute.Value);
    }

    /// <summary>
    /// Deployingが正しいEnumMember属性を持つことを確認
    /// </summary>
    [Fact]
    public void ReadApplicationStatus_status_Deploying_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(ReadApplicationStatus_status).GetField(nameof(ReadApplicationStatus_status.Deploying));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("Deploying", attribute.Value);
    }
}
