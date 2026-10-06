using System.Reflection;
using System.Runtime.Serialization;
using Sacloud.Sdk.AppRunDedicated.Models;

namespace Sacloud.Sdk.AppRunDedicated.Tests.Models;

/// <summary>
/// RegistryPasswordAction Enumのテスト
/// </summary>
public class RegistryPasswordActionTests
{
    /// <summary>
    /// すべてのEnum値が定義されていることを確認
    /// </summary>
    [Fact]
    public void RegistryPasswordAction_HasExpectedValues()
    {
        var values = Enum.GetValues<RegistryPasswordAction>();
        Assert.Contains(RegistryPasswordAction.Keep, values);
        Assert.Contains(RegistryPasswordAction.Remove, values);
        Assert.Contains(RegistryPasswordAction.New, values);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Keep
    /// </summary>
    [Fact]
    public void RegistryPasswordAction_Keep_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(RegistryPasswordAction).GetField(nameof(RegistryPasswordAction.Keep));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("keep", attribute.Value);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Remove
    /// </summary>
    [Fact]
    public void RegistryPasswordAction_Remove_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(RegistryPasswordAction).GetField(nameof(RegistryPasswordAction.Remove));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("remove", attribute.Value);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: New
    /// </summary>
    [Fact]
    public void RegistryPasswordAction_New_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(RegistryPasswordAction).GetField(nameof(RegistryPasswordAction.New));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("new", attribute.Value);
    }
}
