using Sacloud.Sdk.IAM.Projects;

namespace Sacloud.Sdk.IAM.Tests.Projects;

/// <summary>
/// GetOrderingQueryParameterTypeのテスト
/// </summary>
public class GetOrderingQueryParameterTypeTests
{
    /// <summary>
    /// Code値が定義されていることを確認
    /// </summary>
    [Fact]
    public void Code_IsDefined()
    {
        var value = GetOrderingQueryParameterType.Code;

        Assert.True(Enum.IsDefined(typeof(GetOrderingQueryParameterType), value));
    }

    /// <summary>
    /// Minus_code値が定義されていることを確認
    /// </summary>
    [Fact]
    public void MinusCode_IsDefined()
    {
        var value = GetOrderingQueryParameterType.Minus_code;

        Assert.True(Enum.IsDefined(typeof(GetOrderingQueryParameterType), value));
    }

    /// <summary>
    /// CodeとMinus_codeが異なる値であることを確認
    /// </summary>
    [Fact]
    public void CodeAndMinusCode_AreDifferentValues()
    {
        var code = GetOrderingQueryParameterType.Code;
        var minusCode = GetOrderingQueryParameterType.Minus_code;

        Assert.NotEqual(code, minusCode);
    }

    /// <summary>
    /// Enumの値が変数に代入できることを確認
    /// </summary>
    [Fact]
    public void EnumValue_CanBeAssigned()
    {
        GetOrderingQueryParameterType orderingType = GetOrderingQueryParameterType.Code;

        Assert.Equal(GetOrderingQueryParameterType.Code, orderingType);
    }

    /// <summary>
    /// Enum値が比較可能であることを確認
    /// </summary>
    [Fact]
    public void EnumValues_AreComparable()
    {
        var value1 = GetOrderingQueryParameterType.Code;
        var value2 = GetOrderingQueryParameterType.Code;
        var value3 = GetOrderingQueryParameterType.Minus_code;

        Assert.Equal(value1, value2);
        Assert.NotEqual(value1, value3);
    }
}
