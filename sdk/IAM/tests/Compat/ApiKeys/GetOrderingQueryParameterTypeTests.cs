using System.Reflection;
using System.Runtime.Serialization;
using Sacloud.Sdk.IAM.Compat.ApiKeys;

namespace Sacloud.Sdk.IAM.Tests.Compat.ApiKeys;

/// <summary>
/// GetOrderingQueryParameterType Enumの単体テスト
/// </summary>
public class GetOrderingQueryParameterTypeTests
{
    /// <summary>
    /// すべてのEnum値が定義されていることを確認
    /// </summary>
    [Fact]
    public void GetOrderingQueryParameterType_HasExpectedValues()
    {
        var values = Enum.GetValues<GetOrderingQueryParameterType>();
        Assert.Contains(GetOrderingQueryParameterType.Name, values);
        Assert.Contains(GetOrderingQueryParameterType.Minus_name, values);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Name
    /// </summary>
    [Fact]
    public void GetOrderingQueryParameterType_Name_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(GetOrderingQueryParameterType).GetField(nameof(GetOrderingQueryParameterType.Name));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("name", attribute.Value);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Minus_name
    /// </summary>
    [Fact]
    public void GetOrderingQueryParameterType_Minus_name_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(GetOrderingQueryParameterType).GetField(nameof(GetOrderingQueryParameterType.Minus_name));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("-name", attribute.Value);
    }
}
