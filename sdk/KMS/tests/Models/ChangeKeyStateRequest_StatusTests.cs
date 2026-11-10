using System.Runtime.Serialization;
using Sacloud.Sdk.KMS.Models;

namespace Sacloud.Sdk.KMS.Tests.Models;

/// <summary>
/// ChangeKeyStateRequest_Status enumのテスト
/// </summary>
public class ChangeKeyStateRequest_StatusTests
{
    [Fact]
    public void HasExpectedValues()
    {
        var values = Enum.GetValues<ChangeKeyStateRequest_Status>();

        Assert.Equal(3, values.Length);
        Assert.Contains(ChangeKeyStateRequest_Status.Active, values);
        Assert.Contains(ChangeKeyStateRequest_Status.Restricted, values);
        Assert.Contains(ChangeKeyStateRequest_Status.Suspended, values);
    }

    [Theory]
    [InlineData(ChangeKeyStateRequest_Status.Active, "active")]
    [InlineData(ChangeKeyStateRequest_Status.Restricted, "restricted")]
    [InlineData(ChangeKeyStateRequest_Status.Suspended, "suspended")]
    public void EnumMemberValue_MatchesApiValue(ChangeKeyStateRequest_Status value, string expected)
    {
        var member = typeof(ChangeKeyStateRequest_Status).GetMember(value.ToString())[0];
        var attribute = member.GetCustomAttributes(typeof(EnumMemberAttribute), false)
            .Cast<EnumMemberAttribute>()
            .FirstOrDefault();

        Assert.NotNull(attribute);
        Assert.Equal(expected, attribute.Value);
    }
}
