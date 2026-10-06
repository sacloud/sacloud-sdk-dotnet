using System.Reflection;
using System.Runtime.Serialization;
using Sacloud.Sdk.ObjectStorage.Models;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models;

/// <summary>
/// ContractSummary_status Enumのテスト
/// </summary>
public class ContractSummary_statusTests
{

    /// <summary>
    /// すべてのEnum値が定義されていることを確認
    /// </summary>
    [Fact]
    public void ContractSummary_status_HasExpectedValues()
    {
        var values = Enum.GetValues<ContractSummary_status>();
        Assert.Contains(ContractSummary_status.Active, values);
        Assert.Contains(ContractSummary_status.Terminated, values);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Active
    /// </summary>
    [Fact]
    public void ContractSummary_status_Active_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(ContractSummary_status).GetField(nameof(ContractSummary_status.Active));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("active", attribute?.Value);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Terminated
    /// </summary>
    [Fact]
    public void ContractSummary_status_Terminated_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(ContractSummary_status).GetField(nameof(ContractSummary_status.Terminated));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("terminated", attribute?.Value);
    }
}
