using System.Reflection;
using System.Runtime.Serialization;
using Sacloud.Sdk.IAM.Models;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// Project_statusのテスト
/// </summary>
public class Project_statusTests
{
    /// <summary>
    /// すべてのEnum値が定義されていることを確認
    /// </summary>
    [Fact]
    public void Project_status_HasExpectedValues()
    {
        var values = Enum.GetValues<Project_status>();
        Assert.Contains(Project_status.Available, values);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Available
    /// </summary>
    [Fact]
    public void Project_status_Available_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(Project_status).GetField(nameof(Project_status.Available));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("available", attribute.Value);
    }
}
