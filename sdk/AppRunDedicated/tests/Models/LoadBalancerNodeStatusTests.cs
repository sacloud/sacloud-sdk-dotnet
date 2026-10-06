using System.Reflection;
using System.Runtime.Serialization;
using Sacloud.Sdk.AppRunDedicated.Models;

namespace Sacloud.Sdk.AppRunDedicated.Tests.Models;

/// <summary>
/// LoadBalancerNodeStatus Enumのテスト
/// </summary>
public class LoadBalancerNodeStatusTests
{
    /// <summary>
    /// すべてのEnum値が定義されていることを確認
    /// </summary>
    [Fact]
    public void LoadBalancerNodeStatus_HasExpectedValues()
    {
        var values = Enum.GetValues<LoadBalancerNodeStatus>();
        Assert.Contains(LoadBalancerNodeStatus.Healthy, values);
        Assert.Contains(LoadBalancerNodeStatus.Unhealthy, values);
        Assert.Contains(LoadBalancerNodeStatus.Creating, values);
        Assert.Contains(LoadBalancerNodeStatus.Starting, values);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Healthy
    /// </summary>
    [Fact]
    public void LoadBalancerNodeStatus_Healthy_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(LoadBalancerNodeStatus).GetField(nameof(LoadBalancerNodeStatus.Healthy));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("healthy", attribute.Value);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Unhealthy
    /// </summary>
    [Fact]
    public void LoadBalancerNodeStatus_Unhealthy_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(LoadBalancerNodeStatus).GetField(nameof(LoadBalancerNodeStatus.Unhealthy));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("unhealthy", attribute.Value);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Creating
    /// </summary>
    [Fact]
    public void LoadBalancerNodeStatus_Creating_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(LoadBalancerNodeStatus).GetField(nameof(LoadBalancerNodeStatus.Creating));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("creating", attribute.Value);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Starting
    /// </summary>
    [Fact]
    public void LoadBalancerNodeStatus_Starting_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(LoadBalancerNodeStatus).GetField(nameof(LoadBalancerNodeStatus.Starting));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("starting", attribute.Value);
    }
}
