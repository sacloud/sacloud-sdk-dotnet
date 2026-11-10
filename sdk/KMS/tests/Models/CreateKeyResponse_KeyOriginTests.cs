using System.Runtime.Serialization;
using Sacloud.Sdk.KMS.Models;

namespace Sacloud.Sdk.KMS.Tests.Models;

/// <summary>
/// CreateKeyResponse_KeyOrigin enumのテスト
/// </summary>
public class CreateKeyResponse_KeyOriginTests
{
    [Theory]
    [InlineData(CreateKeyResponse_KeyOrigin.Generated, "generated")]
    [InlineData(CreateKeyResponse_KeyOrigin.Imported, "imported")]
    public void EnumMemberValue_MatchesApiValue(CreateKeyResponse_KeyOrigin value, string expected)
    {
        var member = typeof(CreateKeyResponse_KeyOrigin).GetMember(value.ToString())[0];
        var attribute = member.GetCustomAttributes(typeof(EnumMemberAttribute), false)
            .Cast<EnumMemberAttribute>()
            .FirstOrDefault();

        Assert.NotNull(attribute);
        Assert.Equal(expected, attribute.Value);
    }
}
