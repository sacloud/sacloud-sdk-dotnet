using System.Runtime.Serialization;
using Sacloud.Sdk.KMS.Models;

namespace Sacloud.Sdk.KMS.Tests.Models;

/// <summary>
/// ChangeKeyState_Status enumのテスト
/// </summary>
public class ChangeKeyState_StatusTests
{
    [Theory]
    [InlineData(ChangeKeyState_Status.Active, "active")]
    [InlineData(ChangeKeyState_Status.Restricted, "restricted")]
    [InlineData(ChangeKeyState_Status.Suspended, "suspended")]
    public void EnumMemberValue_MatchesApiValue(ChangeKeyState_Status value, string expected)
    {
        var member = typeof(ChangeKeyState_Status).GetMember(value.ToString())[0];
        var attribute = member.GetCustomAttributes(typeof(EnumMemberAttribute), false)
            .Cast<EnumMemberAttribute>()
            .FirstOrDefault();

        Assert.NotNull(attribute);
        Assert.Equal(expected, attribute.Value);
    }
}
