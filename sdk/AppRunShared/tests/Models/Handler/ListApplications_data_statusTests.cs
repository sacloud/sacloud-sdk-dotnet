using System.Reflection;
using System.Runtime.Serialization;
using Sacloud.Sdk.AppRunShared.Models.Handler;

namespace Sacloud.Sdk.AppRunShared.Tests.Models.Handler;

/// <summary>
/// ListApplications_data_statusモデルのテスト
/// </summary>
public class ListApplications_data_statusTests
{
    /// <summary>
    /// Enum ListApplications_data_statusが期待される値を持つことを確認
    /// </summary>
    [Fact]
    public void ListApplications_data_status_HasExpectedValues()
    {
        var values = Enum.GetValues<ListApplications_data_status>();
        Assert.Contains(ListApplications_data_status.Healthy, values);
        Assert.Contains(ListApplications_data_status.UnHealthy, values);
        Assert.Contains(ListApplications_data_status.Deploying, values);
    }

    /// <summary>
    /// Healthyが正しいEnumMember属性を持つことを確認
    /// </summary>
    [Fact]
    public void ListApplications_data_status_Healthy_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(ListApplications_data_status).GetField(nameof(ListApplications_data_status.Healthy));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("Healthy", attribute.Value);
    }

    /// <summary>
    /// UnHealthyが正しいEnumMember属性を持つことを確認
    /// </summary>
    [Fact]
    public void ListApplications_data_status_UnHealthy_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(ListApplications_data_status).GetField(nameof(ListApplications_data_status.UnHealthy));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("UnHealthy", attribute.Value);
    }

    /// <summary>
    /// Deployingが正しいEnumMember属性を持つことを確認
    /// </summary>
    [Fact]
    public void ListApplications_data_status_Deploying_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(ListApplications_data_status).GetField(nameof(ListApplications_data_status.Deploying));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("Deploying", attribute.Value);
    }
}
