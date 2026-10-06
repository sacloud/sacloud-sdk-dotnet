using System.Reflection;
using System.Runtime.Serialization;
using Sacloud.Sdk.AppRunShared.Models;

namespace Sacloud.Sdk.AppRunShared.Tests.Models;

/// <summary>
/// ContainerRegistryAction Enumのテスト
/// </summary>
public class ContainerRegistryActionTests
{
    /// <summary>
    /// すべてのEnum値が定義されていることを確認
    /// </summary>
    [Fact]
    public void ContainerRegistryAction_HasExpectedValues()
    {
        var values = Enum.GetValues<ContainerRegistryAction>();
        Assert.Contains(ContainerRegistryAction.New, values);
        Assert.Contains(ContainerRegistryAction.Keep, values);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: New
    /// </summary>
    [Fact]
    public void ContainerRegistryAction_New_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(ContainerRegistryAction).GetField(nameof(ContainerRegistryAction.New));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("new", attribute.Value);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Keep
    /// </summary>
    [Fact]
    public void ContainerRegistryAction_Keep_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(ContainerRegistryAction).GetField(nameof(ContainerRegistryAction.Keep));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("keep", attribute.Value);
    }
}
