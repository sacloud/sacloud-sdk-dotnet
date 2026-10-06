using System.Reflection;
using System.Runtime.Serialization;
using Sacloud.Sdk.AppRunShared.Models.Handler;

namespace Sacloud.Sdk.AppRunShared.Tests.Models.Handler;

/// <summary>
/// ReadVersionStatus_statusモデルのテスト
/// </summary>
public class ReadVersionStatus_statusTests
{
    /// <summary>
    /// Enum ReadVersionStatus_statusが期待される値を持つことを確認
    /// </summary>
    [Fact]
    public void ReadVersionStatus_status_HasExpectedValues()
    {
        var values = Enum.GetValues<ReadVersionStatus_status>();
        Assert.Contains(ReadVersionStatus_status.Healthy, values);
        Assert.Contains(ReadVersionStatus_status.UnHealthy, values);
        Assert.Contains(ReadVersionStatus_status.Deploying, values);
    }

    /// <summary>
    /// Healthyが正しいEnumMember属性を持つことを確認
    /// </summary>
    [Fact]
    public void ReadVersionStatus_status_Healthy_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(ReadVersionStatus_status).GetField(nameof(ReadVersionStatus_status.Healthy));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("Healthy", attribute.Value);
    }

    /// <summary>
    /// UnHealthyが正しいEnumMember属性を持つことを確認
    /// </summary>
    [Fact]
    public void ReadVersionStatus_status_UnHealthy_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(ReadVersionStatus_status).GetField(nameof(ReadVersionStatus_status.UnHealthy));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("UnHealthy", attribute.Value);
    }

    /// <summary>
    /// Deployingが正しいEnumMember属性を持つことを確認
    /// </summary>
    [Fact]
    public void ReadVersionStatus_status_Deploying_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(ReadVersionStatus_status).GetField(nameof(ReadVersionStatus_status.Deploying));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("Deploying", attribute.Value);
    }
}
