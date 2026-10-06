using Sacloud.Sdk.IAM.ServicePrincipals.Item.Keys;

namespace Sacloud.Sdk.IAM.Tests.ServicePrincipals.Item.Keys;

/// <summary>
/// GetOrderingQueryParameterTypeのテスト
/// </summary>
public class GetOrderingQueryParameterTypeTests
{
    /// <summary>
    /// Created_at値が定義されていることを確認
    /// </summary>
    [Fact]
    public void CreatedAt_IsDefined()
    {
        var value = GetOrderingQueryParameterType.Created_at;

        Assert.True(Enum.IsDefined(typeof(GetOrderingQueryParameterType), value));
    }

    /// <summary>
    /// Key_expires_at値が定義されていることを確認
    /// </summary>
    [Fact]
    public void KeyExpiresAt_IsDefined()
    {
        var value = GetOrderingQueryParameterType.Key_expires_at;

        Assert.True(Enum.IsDefined(typeof(GetOrderingQueryParameterType), value));
    }

    /// <summary>
    /// Created_atとKey_expires_atが異なる値であることを確認
    /// </summary>
    [Fact]
    public void CreatedAtAndKeyExpiresAt_AreDifferentValues()
    {
        var createdAt = GetOrderingQueryParameterType.Created_at;
        var keyExpiresAt = GetOrderingQueryParameterType.Key_expires_at;

        Assert.NotEqual(createdAt, keyExpiresAt);
    }

    /// <summary>
    /// Enumの値が変数に代入できることを確認
    /// </summary>
    [Fact]
    public void EnumValue_CanBeAssigned()
    {
        GetOrderingQueryParameterType orderingType = GetOrderingQueryParameterType.Created_at;

        Assert.Equal(GetOrderingQueryParameterType.Created_at, orderingType);
    }

    /// <summary>
    /// Enum値が比較可能であることを確認
    /// </summary>
    [Fact]
    public void EnumValues_AreComparable()
    {
        var value1 = GetOrderingQueryParameterType.Created_at;
        var value2 = GetOrderingQueryParameterType.Created_at;
        var value3 = GetOrderingQueryParameterType.Key_expires_at;

        Assert.Equal(value1, value2);
        Assert.NotEqual(value1, value3);
    }
}
