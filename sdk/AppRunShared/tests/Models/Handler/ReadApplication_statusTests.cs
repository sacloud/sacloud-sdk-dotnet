using System.Reflection;
using System.Runtime.Serialization;
using Sacloud.Sdk.AppRunShared.Models.Handler;

namespace Sacloud.Sdk.AppRunShared.Tests.Models.Handler;

/// <summary>
/// ReadApplication_statusモデルのテスト
/// </summary>
public class ReadApplication_statusTests
{
    /// <summary>
    /// Enum ReadApplication_statusが期待される値を持つことを確認
    /// </summary>
    [Fact]
    public void ReadApplication_status_HasExpectedValues()
    {
        var values = Enum.GetValues<ReadApplication_status>();
        Assert.Contains(ReadApplication_status.Healthy, values);
        Assert.Contains(ReadApplication_status.UnHealthy, values);
        Assert.Contains(ReadApplication_status.Deploying, values);
    }

    /// <summary>
    /// Healthyが正しいEnumMember属性を持つことを確認
    /// </summary>
    [Fact]
    public void ReadApplication_status_Healthy_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(ReadApplication_status).GetField(nameof(ReadApplication_status.Healthy));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("Healthy", attribute.Value);
    }

    /// <summary>
    /// UnHealthyが正しいEnumMember属性を持つことを確認
    /// </summary>
    [Fact]
    public void ReadApplication_status_UnHealthy_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(ReadApplication_status).GetField(nameof(ReadApplication_status.UnHealthy));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("UnHealthy", attribute.Value);
    }

    /// <summary>
    /// Deployingが正しいEnumMember属性を持つことを確認
    /// </summary>
    [Fact]
    public void ReadApplication_status_Deploying_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(ReadApplication_status).GetField(nameof(ReadApplication_status.Deploying));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("Deploying", attribute.Value);
    }
}
