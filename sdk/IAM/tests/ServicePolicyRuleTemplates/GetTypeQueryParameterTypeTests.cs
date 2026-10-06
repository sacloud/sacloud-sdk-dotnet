using Sacloud.Sdk.IAM.ServicePolicyRuleTemplates;

namespace Sacloud.Sdk.IAM.Tests.ServicePolicyRuleTemplates;

/// <summary>
/// GetTypeQueryParameterTypeのテスト
/// </summary>
public class GetTypeQueryParameterTypeTests
{
    /// <summary>
    /// Boolean値が定義されていることを確認
    /// </summary>
    [Fact]
    public void Boolean_IsDefined()
    {
        var value = GetTypeQueryParameterType.Boolean;

        Assert.True(Enum.IsDefined(typeof(GetTypeQueryParameterType), value));
    }

    /// <summary>
    /// List値が定義されていることを確認
    /// </summary>
    [Fact]
    public void List_IsDefined()
    {
        var value = GetTypeQueryParameterType.List;

        Assert.True(Enum.IsDefined(typeof(GetTypeQueryParameterType), value));
    }

    /// <summary>
    /// BooleanとListが異なる値であることを確認
    /// </summary>
    [Fact]
    public void BooleanAndList_AreDifferentValues()
    {
        var booleanValue = GetTypeQueryParameterType.Boolean;
        var listValue = GetTypeQueryParameterType.List;

        Assert.NotEqual(booleanValue, listValue);
    }

    /// <summary>
    /// Enumの値が変数に代入できることを確認
    /// </summary>
    [Fact]
    public void EnumValue_CanBeAssigned()
    {
        GetTypeQueryParameterType typeValue = GetTypeQueryParameterType.Boolean;

        Assert.Equal(GetTypeQueryParameterType.Boolean, typeValue);
    }

    /// <summary>
    /// Enum値が比較可能であることを確認
    /// </summary>
    [Fact]
    public void EnumValues_AreComparable()
    {
        var value1 = GetTypeQueryParameterType.Boolean;
        var value2 = GetTypeQueryParameterType.Boolean;
        var value3 = GetTypeQueryParameterType.List;

        Assert.Equal(value1, value2);
        Assert.NotEqual(value1, value3);
    }
}
