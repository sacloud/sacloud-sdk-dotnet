using System.Runtime.Serialization;
using Sacloud.Sdk.KMS.Models;

namespace Sacloud.Sdk.KMS.Tests.Models;

/// <summary>
/// KeyScheduledDestruction_Status enumのテスト
/// </summary>
public class KeyScheduledDestruction_StatusTests
{
    [Theory]
    [InlineData(KeyScheduledDestruction_Status.Active, "active")]
    [InlineData(KeyScheduledDestruction_Status.Restricted, "restricted")]
    [InlineData(KeyScheduledDestruction_Status.Suspended, "suspended")]
    [InlineData(KeyScheduledDestruction_Status.Pending_destruction, "pending_destruction")]
    [InlineData(KeyScheduledDestruction_Status.Destroyed, "destroyed")]
    public void EnumMemberValue_MatchesApiValue(KeyScheduledDestruction_Status value, string expected)
    {
        var member = typeof(KeyScheduledDestruction_Status).GetMember(value.ToString())[0];
        var attribute = member.GetCustomAttributes(typeof(EnumMemberAttribute), false)
            .Cast<EnumMemberAttribute>()
            .FirstOrDefault();

        Assert.NotNull(attribute);
        Assert.Equal(expected, attribute.Value);
    }
}
