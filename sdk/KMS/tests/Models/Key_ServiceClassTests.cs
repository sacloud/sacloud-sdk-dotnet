using System.Runtime.Serialization;
using Sacloud.Sdk.KMS.Models;

namespace Sacloud.Sdk.KMS.Tests.Models;

/// <summary>
/// Key_ServiceClass enumのテスト
/// </summary>
public class Key_ServiceClassTests
{
    [Fact]
    public void HasExpectedValues()
    {
        var values = Enum.GetValues<Key_ServiceClass>();

        Assert.Equal(2, values.Length);
        Assert.Contains(Key_ServiceClass.CloudKmsKey, values);
        Assert.Contains(Key_ServiceClass.CloudKmsKeyLegacy, values);
    }

    [Theory]
    [InlineData(Key_ServiceClass.CloudKmsKey, "cloud/kms/key")]
    [InlineData(Key_ServiceClass.CloudKmsKeyLegacy, "cloud/kms/key/legacy")]
    public void EnumMemberValue_MatchesApiValue(Key_ServiceClass value, string expected)
    {
        var member = typeof(Key_ServiceClass).GetMember(value.ToString())[0];
        var attribute = member.GetCustomAttributes(typeof(EnumMemberAttribute), false)
            .Cast<EnumMemberAttribute>()
            .FirstOrDefault();

        Assert.NotNull(attribute);
        Assert.Equal(expected, attribute.Value);
    }
}
