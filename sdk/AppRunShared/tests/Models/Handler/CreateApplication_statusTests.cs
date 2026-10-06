using System.Reflection;
using System.Runtime.Serialization;
using Sacloud.Sdk.AppRunShared.Models.Handler;

namespace Sacloud.Sdk.AppRunShared.Tests.Models.Handler;

/// <summary>
/// CreateApplication_statusモデルのテスト
/// </summary>
public class CreateApplication_statusTests
{
    /// <summary>
    /// CreateApplication_statusが期待される値を持つことを確認
    /// </summary>
    [Fact]
    public void CreateApplication_status_HasExpectedValues()
    {
        var values = Enum.GetValues<CreateApplication_status>();
        Assert.Contains(CreateApplication_status.Healthy, values);
        Assert.Contains(CreateApplication_status.UnHealthy, values);
        Assert.Contains(CreateApplication_status.Deploying, values);
    }

    /// <summary>
    /// CreateApplication_status.Healthyが正しいEnumMember属性を持つことを確認
    /// </summary>
    [Fact]
    public void CreateApplication_status_Healthy_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(CreateApplication_status).GetField(nameof(CreateApplication_status.Healthy));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("Healthy", attribute.Value);
    }

    /// <summary>
    /// CreateApplication_status.UnHealthyが正しいEnumMember属性を持つことを確認
    /// </summary>
    [Fact]
    public void CreateApplication_status_UnHealthy_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(CreateApplication_status).GetField(nameof(CreateApplication_status.UnHealthy));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("UnHealthy", attribute.Value);
    }

    /// <summary>
    /// CreateApplication_status.Deployingが正しいEnumMember属性を持つことを確認
    /// </summary>
    [Fact]
    public void CreateApplication_status_Deploying_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(CreateApplication_status).GetField(nameof(CreateApplication_status.Deploying));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("Deploying", attribute.Value);
    }
}
