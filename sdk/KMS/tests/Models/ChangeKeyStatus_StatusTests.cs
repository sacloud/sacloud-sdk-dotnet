using System.Runtime.Serialization;
using Sacloud.Sdk.KMS.Models;

namespace Sacloud.Sdk.KMS.Tests.Models;

/// <summary>
/// ChangeKeyStatus_Statusのテスト
/// </summary>
public class ChangeKeyStatus_StatusTests
{
    /// <summary>
    /// ChangeKeyStatus_Statusが期待される値を持つことを確認
    /// </summary>
    [Fact]
    public void ChangeKeyStatus_Status_HasExpectedValues()
    {
        var values = Enum.GetValues<ChangeKeyStatus_Status>();

        Assert.Equal(3, values.Length);
        Assert.Contains(ChangeKeyStatus_Status.Active, values);
        Assert.Contains(ChangeKeyStatus_Status.Restricted, values);
        Assert.Contains(ChangeKeyStatus_Status.Suspended, values);
    }

    /// <summary>
    /// ChangeKeyStatus_Status.Activeが正しいEnumMember値を持つことを確認
    /// </summary>
    [Fact]
    public void ChangeKeyStatus_Status_Active_HasCorrectEnumMemberValue()
    {
        var memberInfo = typeof(ChangeKeyStatus_Status).GetMember(nameof(ChangeKeyStatus_Status.Active))[0];
        var attribute = memberInfo.GetCustomAttributes(typeof(EnumMemberAttribute), false)
            .Cast<EnumMemberAttribute>()
            .FirstOrDefault();

        Assert.NotNull(attribute);
        Assert.Equal("active", attribute.Value);
    }

    /// <summary>
    /// ChangeKeyStatus_Status.Restrictedが正しいEnumMember値を持つことを確認
    /// </summary>
    [Fact]
    public void ChangeKeyStatus_Status_Restricted_HasCorrectEnumMemberValue()
    {
        var memberInfo = typeof(ChangeKeyStatus_Status).GetMember(nameof(ChangeKeyStatus_Status.Restricted))[0];
        var attribute = memberInfo.GetCustomAttributes(typeof(EnumMemberAttribute), false)
            .Cast<EnumMemberAttribute>()
            .FirstOrDefault();

        Assert.NotNull(attribute);
        Assert.Equal("restricted", attribute.Value);
    }

    /// <summary>
    /// ChangeKeyStatus_Status.Suspendedが正しいEnumMember値を持つことを確認
    /// </summary>
    [Fact]
    public void ChangeKeyStatus_Status_Suspended_HasCorrectEnumMemberValue()
    {
        var memberInfo = typeof(ChangeKeyStatus_Status).GetMember(nameof(ChangeKeyStatus_Status.Suspended))[0];
        var attribute = memberInfo.GetCustomAttributes(typeof(EnumMemberAttribute), false)
            .Cast<EnumMemberAttribute>()
            .FirstOrDefault();

        Assert.NotNull(attribute);
        Assert.Equal("suspended", attribute.Value);
    }
}
