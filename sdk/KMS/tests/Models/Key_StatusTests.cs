using System.Runtime.Serialization;
using Sacloud.Sdk.KMS.Models;

namespace Sacloud.Sdk.KMS.Tests.Models;

/// <summary>
/// Key_Status enumのテスト
/// </summary>
public class Key_StatusTests
{
    [Fact]
    public void HasExpectedValues()
    {
        var values = Enum.GetValues<Key_Status>();

        Assert.Equal(5, values.Length);
        Assert.Contains(Key_Status.Active, values);
        Assert.Contains(Key_Status.Restricted, values);
        Assert.Contains(Key_Status.Suspended, values);
        Assert.Contains(Key_Status.Pending_destruction, values);
        Assert.Contains(Key_Status.Destroyed, values);
    }

    [Theory]
    [InlineData(Key_Status.Active, "active")]
    [InlineData(Key_Status.Restricted, "restricted")]
    [InlineData(Key_Status.Suspended, "suspended")]
    [InlineData(Key_Status.Pending_destruction, "pending_destruction")]
    [InlineData(Key_Status.Destroyed, "destroyed")]
    public void EnumMemberValue_MatchesApiValue(Key_Status value, string expected)
    {
        var member = typeof(Key_Status).GetMember(value.ToString())[0];
        var attribute = member.GetCustomAttributes(typeof(EnumMemberAttribute), false)
            .Cast<EnumMemberAttribute>()
            .FirstOrDefault();

        Assert.NotNull(attribute);
        Assert.Equal(expected, attribute.Value);
    }
}
