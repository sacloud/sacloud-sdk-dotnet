using System.Reflection;
using System.Runtime.Serialization;
using Sacloud.Sdk.ObjectStorage.Models.Model;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models.Model;

/// <summary>
/// Cluster_plan_family Enumのテスト
/// </summary>
public class Cluster_plan_familyTests
{

    /// <summary>
    /// すべてのEnum値が定義されていることを確認
    /// </summary>
    [Fact]
    public void Cluster_plan_family_HasExpectedValues()
    {
        var values = Enum.GetValues<Cluster_plan_family>();
        Assert.Contains(Cluster_plan_family.Standard, values);
        Assert.Contains(Cluster_plan_family.Archive, values);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Standard
    /// </summary>
    [Fact]
    public void Cluster_plan_family_Standard_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(Cluster_plan_family).GetField(nameof(Cluster_plan_family.Standard));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("standard", attribute?.Value);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Archive
    /// </summary>
    [Fact]
    public void Cluster_plan_family_Archive_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(Cluster_plan_family).GetField(nameof(Cluster_plan_family.Archive));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("archive", attribute?.Value);
    }
}
