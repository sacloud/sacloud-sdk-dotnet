using Sacloud.Sdk.IAM.ServicePrincipals;

namespace Sacloud.Sdk.IAM.Tests.ServicePrincipals;

/// <summary>
/// GetOrderingQueryParameterTypeのテスト
/// </summary>
public class GetOrderingQueryParameterTypeTests
{
    /// <summary>
    /// Name値が定義されていることを確認
    /// </summary>
    [Fact]
    public void Name_IsDefined()
    {
        var value = GetOrderingQueryParameterType.Name;

        Assert.True(Enum.IsDefined(typeof(GetOrderingQueryParameterType), value));
    }

    /// <summary>
    /// Minus_name値が定義されていることを確認
    /// </summary>
    [Fact]
    public void MinusName_IsDefined()
    {
        var value = GetOrderingQueryParameterType.Minus_name;

        Assert.True(Enum.IsDefined(typeof(GetOrderingQueryParameterType), value));
    }

    /// <summary>
    /// NameとMinus_nameが異なる値であることを確認
    /// </summary>
    [Fact]
    public void NameAndMinusName_AreDifferentValues()
    {
        var name = GetOrderingQueryParameterType.Name;
        var minusName = GetOrderingQueryParameterType.Minus_name;

        Assert.NotEqual(name, minusName);
    }

    /// <summary>
    /// Enumの値が変数に代入できることを確認
    /// </summary>
    [Fact]
    public void EnumValue_CanBeAssigned()
    {
        GetOrderingQueryParameterType orderingType = GetOrderingQueryParameterType.Name;

        Assert.Equal(GetOrderingQueryParameterType.Name, orderingType);
    }

    /// <summary>
    /// Enum値が比較可能であることを確認
    /// </summary>
    [Fact]
    public void EnumValues_AreComparable()
    {
        var value1 = GetOrderingQueryParameterType.Name;
        var value2 = GetOrderingQueryParameterType.Name;
        var value3 = GetOrderingQueryParameterType.Minus_name;

        Assert.Equal(value1, value2);
        Assert.NotEqual(value1, value3);
    }
}
