using System.Runtime.Serialization;
using Sacloud.Sdk.KMS.Models;

namespace Sacloud.Sdk.KMS.Tests.Models;

/// <summary>
/// KeyServiceClassEnumのテスト
/// </summary>
public class KeyServiceClassEnumTests
{
    /// <summary>
    /// KeyServiceClassEnumが期待される値を持つことを確認
    /// </summary>
    [Fact]
    public void KeyServiceClassEnum_HasExpectedValues()
    {
        var values = Enum.GetValues<KeyServiceClassEnum>();

        Assert.Equal(2, values.Length);
        Assert.Contains(KeyServiceClassEnum.CloudKmsKey, values);
        Assert.Contains(KeyServiceClassEnum.CloudKmsKeyLegacy, values);
    }

    /// <summary>
    /// KeyServiceClassEnum.CloudKmsKeyが正しいEnumMember値を持つことを確認
    /// </summary>
    [Fact]
    public void KeyServiceClassEnum_CloudKmsKey_HasCorrectEnumMemberValue()
    {
        var memberInfo = typeof(KeyServiceClassEnum).GetMember(nameof(KeyServiceClassEnum.CloudKmsKey))[0];
        var attribute = memberInfo.GetCustomAttributes(typeof(EnumMemberAttribute), false)
            .Cast<EnumMemberAttribute>()
            .FirstOrDefault();

        Assert.NotNull(attribute);
        Assert.Equal("cloud/kms/key", attribute.Value);
    }

    /// <summary>
    /// KeyServiceClassEnum.CloudKmsKeyLegacyが正しいEnumMember値を持つことを確認
    /// </summary>
    [Fact]
    public void KeyServiceClassEnum_CloudKmsKeyLegacy_HasCorrectEnumMemberValue()
    {
        var memberInfo = typeof(KeyServiceClassEnum).GetMember(nameof(KeyServiceClassEnum.CloudKmsKeyLegacy))[0];
        var attribute = memberInfo.GetCustomAttributes(typeof(EnumMemberAttribute), false)
            .Cast<EnumMemberAttribute>()
            .FirstOrDefault();

        Assert.NotNull(attribute);
        Assert.Equal("cloud/kms/key/legacy", attribute.Value);
    }
}
