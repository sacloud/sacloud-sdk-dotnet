using System.Runtime.Serialization;
using Sacloud.Sdk.KMS.Models;

namespace Sacloud.Sdk.KMS.Tests.Models;

/// <summary>
/// Key_KeyOrigin enumのテスト
/// </summary>
public class Key_KeyOriginTests
{
    [Fact]
    public void HasExpectedValues()
    {
        var values = Enum.GetValues<Key_KeyOrigin>();

        Assert.Equal(2, values.Length);
        Assert.Contains(Key_KeyOrigin.Generated, values);
        Assert.Contains(Key_KeyOrigin.Imported, values);
    }

    [Theory]
    [InlineData(Key_KeyOrigin.Generated, "generated")]
    [InlineData(Key_KeyOrigin.Imported, "imported")]
    public void EnumMemberValue_MatchesApiValue(Key_KeyOrigin value, string expected)
    {
        var member = typeof(Key_KeyOrigin).GetMember(value.ToString())[0];
        var attribute = member.GetCustomAttributes(typeof(EnumMemberAttribute), false)
            .Cast<EnumMemberAttribute>()
            .FirstOrDefault();

        Assert.NotNull(attribute);
        Assert.Equal(expected, attribute.Value);
    }
}
