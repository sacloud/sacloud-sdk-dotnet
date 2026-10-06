using System.Reflection;
using System.Runtime.Serialization;
using Sacloud.Sdk.ObjectStorage.Models.Model;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models.Model;

/// <summary>
/// PlanType Enumのテスト
/// </summary>
public class PlanTypeTests
{

    /// <summary>
    /// すべてのEnum値が定義されていることを確認
    /// </summary>
    [Fact]
    public void PlanType_HasExpectedValues()
    {
        var values = Enum.GetValues<PlanType>();
        Assert.Contains(PlanType.Standard, values);
        Assert.Contains(PlanType.Archive, values);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Standard
    /// </summary>
    [Fact]
    public void PlanType_Standard_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(PlanType).GetField(nameof(PlanType.Standard));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("standard", attribute?.Value);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Archive
    /// </summary>
    [Fact]
    public void PlanType_Archive_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(PlanType).GetField(nameof(PlanType.Archive));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("archive", attribute?.Value);
    }
}
