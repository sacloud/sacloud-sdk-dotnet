using System.Runtime.Serialization;
using Sacloud.Sdk.KMS.Models;

namespace Sacloud.Sdk.KMS.Tests.Models;

/// <summary>
/// CreateKeyResponse_ServiceClass enumのテスト
/// </summary>
public class CreateKeyResponse_ServiceClassTests
{
    [Theory]
    [InlineData(CreateKeyResponse_ServiceClass.CloudKmsKey, "cloud/kms/key")]
    [InlineData(CreateKeyResponse_ServiceClass.CloudKmsKeyLegacy, "cloud/kms/key/legacy")]
    public void EnumMemberValue_MatchesApiValue(CreateKeyResponse_ServiceClass value, string expected)
    {
        var member = typeof(CreateKeyResponse_ServiceClass).GetMember(value.ToString())[0];
        var attribute = member.GetCustomAttributes(typeof(EnumMemberAttribute), false)
            .Cast<EnumMemberAttribute>()
            .FirstOrDefault();

        Assert.NotNull(attribute);
        Assert.Equal(expected, attribute.Value);
    }
}
