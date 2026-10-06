using Sacloud.Sdk.IAM.OrganizationServicePolicy;

namespace Sacloud.Sdk.IAM.Tests.OrganizationServicePolicy;

/// <summary>
/// GetTypeQueryParameterTypeのテスト
/// </summary>
public class GetTypeQueryParameterTypeTests
{
    /// <summary>
    /// Bool値が定義されていることを確認
    /// </summary>
    [Fact]
    public void Bool_IsDefined()
    {
        var value = GetTypeQueryParameterType.Bool;

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
    /// BoolとListが異なる値であることを確認
    /// </summary>
    [Fact]
    public void BoolAndList_AreDifferentValues()
    {
        var boolValue = GetTypeQueryParameterType.Bool;
        var listValue = GetTypeQueryParameterType.List;

        Assert.NotEqual(boolValue, listValue);
    }

    /// <summary>
    /// Enumの値が変数に代入できることを確認
    /// </summary>
    [Fact]
    public void EnumValue_CanBeAssigned()
    {
        GetTypeQueryParameterType typeValue = GetTypeQueryParameterType.Bool;

        Assert.Equal(GetTypeQueryParameterType.Bool, typeValue);
    }

    /// <summary>
    /// Enum値が比較可能であることを確認
    /// </summary>
    [Fact]
    public void EnumValues_AreComparable()
    {
        var value1 = GetTypeQueryParameterType.Bool;
        var value2 = GetTypeQueryParameterType.Bool;
        var value3 = GetTypeQueryParameterType.List;

        Assert.Equal(value1, value2);
        Assert.NotEqual(value1, value3);
    }
}
