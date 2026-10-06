using System.Reflection;
using System.Runtime.Serialization;
using Sacloud.Sdk.AppRunDedicated.Models;

namespace Sacloud.Sdk.AppRunDedicated.Tests.Models;

/// <summary>
/// WorkerNodeStatus Enumのテスト
/// </summary>
public class WorkerNodeStatusTests
{
    /// <summary>
    /// すべてのEnum値が定義されていることを確認
    /// </summary>
    [Fact]
    public void WorkerNodeStatus_HasExpectedValues()
    {
        var values = Enum.GetValues<WorkerNodeStatus>();
        Assert.Contains(WorkerNodeStatus.Healthy, values);
        Assert.Contains(WorkerNodeStatus.Unhealthy, values);
        Assert.Contains(WorkerNodeStatus.Creating, values);
        Assert.Contains(WorkerNodeStatus.Starting, values);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Healthy
    /// </summary>
    [Fact]
    public void WorkerNodeStatus_Healthy_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(WorkerNodeStatus).GetField(nameof(WorkerNodeStatus.Healthy));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("healthy", attribute.Value);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Unhealthy
    /// </summary>
    [Fact]
    public void WorkerNodeStatus_Unhealthy_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(WorkerNodeStatus).GetField(nameof(WorkerNodeStatus.Unhealthy));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("unhealthy", attribute.Value);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Creating
    /// </summary>
    [Fact]
    public void WorkerNodeStatus_Creating_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(WorkerNodeStatus).GetField(nameof(WorkerNodeStatus.Creating));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("creating", attribute.Value);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Starting
    /// </summary>
    [Fact]
    public void WorkerNodeStatus_Starting_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(WorkerNodeStatus).GetField(nameof(WorkerNodeStatus.Starting));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("starting", attribute.Value);
    }
}
