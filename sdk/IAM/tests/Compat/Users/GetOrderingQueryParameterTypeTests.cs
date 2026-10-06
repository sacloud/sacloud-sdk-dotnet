using Sacloud.Sdk.IAM.Compat.Users;

namespace Sacloud.Sdk.IAM.Tests.Compat.Users;

/// <summary>
/// GetOrderingQueryParameterTypeのテスト
/// </summary>
public class GetOrderingQueryParameterTypeTests
{
    /// <summary>
    /// Enum値がすべて存在することを確認
    /// </summary>
    [Fact]
    public void Enum_HasAllExpectedValues()
    {
        var expectedValues = new[]
        {
            GetOrderingQueryParameterType.Code,
            GetOrderingQueryParameterType.Minus_code
        };

        var enumValues = Enum.GetValues(typeof(GetOrderingQueryParameterType))
            .Cast<GetOrderingQueryParameterType>()
            .ToArray();

        foreach (var expected in expectedValues)
        {
            Assert.Contains(expected, enumValues);
        }
    }

    /// <summary>
    /// Enum値の型が正しいことを確認
    /// </summary>
    [Fact]
    public void Enum_IsCorrectType()
    {
        var enumType = typeof(GetOrderingQueryParameterType);

        Assert.True(enumType.IsEnum);
        Assert.Equal(typeof(int), Enum.GetUnderlyingType(enumType));
    }
}
