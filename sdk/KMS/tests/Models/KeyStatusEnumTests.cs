using System.Runtime.Serialization;
using Sacloud.Sdk.KMS.Models;

namespace Sacloud.Sdk.KMS.Tests.Models;

/// <summary>
/// KeyStatusEnumのテスト
/// </summary>
public class KeyStatusEnumTests
{
    /// <summary>
    /// KeyStatusEnumが期待される値を持つことを確認
    /// </summary>
    [Fact]
    public void KeyStatusEnum_HasExpectedValues()
    {
        var values = Enum.GetValues<KeyStatusEnum>();

        Assert.Equal(4, values.Length);
        Assert.Contains(KeyStatusEnum.Active, values);
        Assert.Contains(KeyStatusEnum.Restricted, values);
        Assert.Contains(KeyStatusEnum.Suspended, values);
        Assert.Contains(KeyStatusEnum.Pending_destruction, values);
    }

    /// <summary>
    /// KeyStatusEnum.Activeが正しいEnumMember値を持つことを確認
    /// </summary>
    [Fact]
    public void KeyStatusEnum_Active_HasCorrectEnumMemberValue()
    {
        var memberInfo = typeof(KeyStatusEnum).GetMember(nameof(KeyStatusEnum.Active))[0];
        var attribute = memberInfo.GetCustomAttributes(typeof(EnumMemberAttribute), false)
            .Cast<EnumMemberAttribute>()
            .FirstOrDefault();

        Assert.NotNull(attribute);
        Assert.Equal("active", attribute.Value);
    }

    /// <summary>
    /// KeyStatusEnum.Restrictedが正しいEnumMember値を持つことを確認
    /// </summary>
    [Fact]
    public void KeyStatusEnum_Restricted_HasCorrectEnumMemberValue()
    {
        var memberInfo = typeof(KeyStatusEnum).GetMember(nameof(KeyStatusEnum.Restricted))[0];
        var attribute = memberInfo.GetCustomAttributes(typeof(EnumMemberAttribute), false)
            .Cast<EnumMemberAttribute>()
            .FirstOrDefault();

        Assert.NotNull(attribute);
        Assert.Equal("restricted", attribute.Value);
    }

    /// <summary>
    /// KeyStatusEnum.Suspendedが正しいEnumMember値を持つことを確認
    /// </summary>
    [Fact]
    public void KeyStatusEnum_Suspended_HasCorrectEnumMemberValue()
    {
        var memberInfo = typeof(KeyStatusEnum).GetMember(nameof(KeyStatusEnum.Suspended))[0];
        var attribute = memberInfo.GetCustomAttributes(typeof(EnumMemberAttribute), false)
            .Cast<EnumMemberAttribute>()
            .FirstOrDefault();

        Assert.NotNull(attribute);
        Assert.Equal("suspended", attribute.Value);
    }

    /// <summary>
    /// KeyStatusEnum.Pending_destructionが正しいEnumMember値を持つことを確認
    /// </summary>
    [Fact]
    public void KeyStatusEnum_PendingDestruction_HasCorrectEnumMemberValue()
    {
        var memberInfo = typeof(KeyStatusEnum).GetMember(nameof(KeyStatusEnum.Pending_destruction))[0];
        var attribute = memberInfo.GetCustomAttributes(typeof(EnumMemberAttribute), false)
            .Cast<EnumMemberAttribute>()
            .FirstOrDefault();

        Assert.NotNull(attribute);
        Assert.Equal("pending_destruction", attribute.Value);
    }
}
