using System.Runtime.Serialization;
using Sacloud.Sdk.KMS.Models;

namespace Sacloud.Sdk.KMS.Tests.Models;

/// <summary>
/// EncryptionRequest_Algo enumのテスト
/// </summary>
public class EncryptionRequest_AlgoTests
{
    [Fact]
    public void HasExpectedValues()
    {
        var values = Enum.GetValues<EncryptionRequest_Algo>();

        Assert.Equal(3, values.Length);
        Assert.Contains(EncryptionRequest_Algo.Aes256Gcm, values);
        Assert.Contains(EncryptionRequest_Algo.Aes256Cbc, values);
        Assert.Contains(EncryptionRequest_Algo.Aes256Kw, values);
    }

    [Theory]
    [InlineData(EncryptionRequest_Algo.Aes256Gcm, "aes-256-gcm")]
    [InlineData(EncryptionRequest_Algo.Aes256Cbc, "aes-256-cbc")]
    [InlineData(EncryptionRequest_Algo.Aes256Kw, "aes-256-kw")]
    public void EnumMemberValue_MatchesApiValue(EncryptionRequest_Algo value, string expected)
    {
        var member = typeof(EncryptionRequest_Algo).GetMember(value.ToString())[0];
        var attribute = member.GetCustomAttributes(typeof(EnumMemberAttribute), false)
            .Cast<EnumMemberAttribute>()
            .FirstOrDefault();

        Assert.NotNull(attribute);
        Assert.Equal(expected, attribute.Value);
    }
}
