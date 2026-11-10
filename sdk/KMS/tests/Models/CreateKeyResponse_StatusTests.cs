using System.Runtime.Serialization;
using Sacloud.Sdk.KMS.Models;

namespace Sacloud.Sdk.KMS.Tests.Models;

/// <summary>
/// CreateKeyResponse_Status enumのテスト
/// </summary>
public class CreateKeyResponse_StatusTests
{
    [Theory]
    [InlineData(CreateKeyResponse_Status.Active, "active")]
    [InlineData(CreateKeyResponse_Status.Restricted, "restricted")]
    [InlineData(CreateKeyResponse_Status.Suspended, "suspended")]
    [InlineData(CreateKeyResponse_Status.Pending_destruction, "pending_destruction")]
    [InlineData(CreateKeyResponse_Status.Destroyed, "destroyed")]
    public void EnumMemberValue_MatchesApiValue(CreateKeyResponse_Status value, string expected)
    {
        var member = typeof(CreateKeyResponse_Status).GetMember(value.ToString())[0];
        var attribute = member.GetCustomAttributes(typeof(EnumMemberAttribute), false)
            .Cast<EnumMemberAttribute>()
            .FirstOrDefault();

        Assert.NotNull(attribute);
        Assert.Equal(expected, attribute.Value);
    }
}
