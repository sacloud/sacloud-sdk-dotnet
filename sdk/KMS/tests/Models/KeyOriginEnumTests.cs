using System.Runtime.Serialization;
using Sacloud.Sdk.KMS.Models;

namespace Sacloud.Sdk.KMS.Tests.Models;

/// <summary>
/// KeyOriginEnumのテスト
/// </summary>
public class KeyOriginEnumTests
{
    /// <summary>
    /// KeyOriginEnumが期待される値を持つことを確認
    /// </summary>
    [Fact]
    public void KeyOriginEnum_HasExpectedValues()
    {
        var values = Enum.GetValues<KeyOriginEnum>();

        Assert.Equal(2, values.Length);
        Assert.Contains(KeyOriginEnum.Generated, values);
        Assert.Contains(KeyOriginEnum.Imported, values);
    }

    /// <summary>
    /// KeyOriginEnum.Generatedが正しいEnumMember値を持つことを確認
    /// </summary>
    [Fact]
    public void KeyOriginEnum_Generated_HasCorrectEnumMemberValue()
    {
        var memberInfo = typeof(KeyOriginEnum).GetMember(nameof(KeyOriginEnum.Generated))[0];
        var attribute = memberInfo.GetCustomAttributes(typeof(EnumMemberAttribute), false)
            .Cast<EnumMemberAttribute>()
            .FirstOrDefault();

        Assert.NotNull(attribute);
        Assert.Equal("generated", attribute.Value);
    }

    /// <summary>
    /// KeyOriginEnum.Importedが正しいEnumMember値を持つことを確認
    /// </summary>
    [Fact]
    public void KeyOriginEnum_Imported_HasCorrectEnumMemberValue()
    {
        var memberInfo = typeof(KeyOriginEnum).GetMember(nameof(KeyOriginEnum.Imported))[0];
        var attribute = memberInfo.GetCustomAttributes(typeof(EnumMemberAttribute), false)
            .Cast<EnumMemberAttribute>()
            .FirstOrDefault();

        Assert.NotNull(attribute);
        Assert.Equal("imported", attribute.Value);
    }
}
