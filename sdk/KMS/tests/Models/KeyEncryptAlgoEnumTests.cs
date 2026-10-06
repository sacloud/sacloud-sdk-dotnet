using System.Runtime.Serialization;
using Sacloud.Sdk.KMS.Models;

namespace Sacloud.Sdk.KMS.Tests.Models;

/// <summary>
/// KeyEncryptAlgoEnumのテスト
/// </summary>
public class KeyEncryptAlgoEnumTests
{
    /// <summary>
    /// KeyEncryptAlgoEnumが期待される値を持つことを確認
    /// </summary>
    [Fact]
    public void KeyEncryptAlgoEnum_HasExpectedValues()
    {
        var values = Enum.GetValues<KeyEncryptAlgoEnum>();

        Assert.Equal(3, values.Length);
        Assert.Contains(KeyEncryptAlgoEnum.Aes256Gcm, values);
        Assert.Contains(KeyEncryptAlgoEnum.Aes256Cbc, values);
        Assert.Contains(KeyEncryptAlgoEnum.Aes256Kw, values);
    }

    /// <summary>
    /// KeyEncryptAlgoEnum.Aes256Gcmが正しいEnumMember値を持つことを確認
    /// </summary>
    [Fact]
    public void KeyEncryptAlgoEnum_Aes256Gcm_HasCorrectEnumMemberValue()
    {
        var memberInfo = typeof(KeyEncryptAlgoEnum).GetMember(nameof(KeyEncryptAlgoEnum.Aes256Gcm))[0];
        var attribute = memberInfo.GetCustomAttributes(typeof(EnumMemberAttribute), false)
            .Cast<EnumMemberAttribute>()
            .FirstOrDefault();

        Assert.NotNull(attribute);
        Assert.Equal("aes-256-gcm", attribute.Value);
    }

    /// <summary>
    /// KeyEncryptAlgoEnum.Aes256Cbcが正しいEnumMember値を持つことを確認
    /// </summary>
    [Fact]
    public void KeyEncryptAlgoEnum_Aes256Cbc_HasCorrectEnumMemberValue()
    {
        var memberInfo = typeof(KeyEncryptAlgoEnum).GetMember(nameof(KeyEncryptAlgoEnum.Aes256Cbc))[0];
        var attribute = memberInfo.GetCustomAttributes(typeof(EnumMemberAttribute), false)
            .Cast<EnumMemberAttribute>()
            .FirstOrDefault();

        Assert.NotNull(attribute);
        Assert.Equal("aes-256-cbc", attribute.Value);
    }

    /// <summary>
    /// KeyEncryptAlgoEnum.Aes256Kwが正しいEnumMember値を持つことを確認
    /// </summary>
    [Fact]
    public void KeyEncryptAlgoEnum_Aes256Kw_HasCorrectEnumMemberValue()
    {
        var memberInfo = typeof(KeyEncryptAlgoEnum).GetMember(nameof(KeyEncryptAlgoEnum.Aes256Kw))[0];
        var attribute = memberInfo.GetCustomAttributes(typeof(EnumMemberAttribute), false)
            .Cast<EnumMemberAttribute>()
            .FirstOrDefault();

        Assert.NotNull(attribute);
        Assert.Equal("aes-256-kw", attribute.Value);
    }
}
