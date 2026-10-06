using Sacloud.Sdk.IAM.Models;
using System.Reflection;
using System.Runtime.Serialization;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// User_otp_statusのテスト
/// </summary>
public class User_otp_statusTests
{
    /// <summary>
    /// Enum値が正しく定義されていることを確認
    /// </summary>
    [Fact]
    public void EnumValues_AreDefinedCorrectly()
    {
        var deactivated = User_otp_status.Deactivated;
        var activated = User_otp_status.Activated;
        var activating = User_otp_status.Activating;

        Assert.Equal(User_otp_status.Deactivated, deactivated);
        Assert.Equal(User_otp_status.Activated, activated);
        Assert.Equal(User_otp_status.Activating, activating);
    }

    /// <summary>
    /// Enum値の数が正しいことを確認
    /// </summary>
    [Fact]
    public void EnumValues_HasCorrectCount()
    {
        var values = Enum.GetValues(typeof(User_otp_status));

        Assert.Equal(3, values.Length);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Deactivated
    /// </summary>
    [Fact]
    public void User_otp_status_Deactivated_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(User_otp_status).GetField(nameof(User_otp_status.Deactivated));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("deactivated", attribute.Value);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Activated
    /// </summary>
    [Fact]
    public void User_otp_status_Activated_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(User_otp_status).GetField(nameof(User_otp_status.Activated));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("activated", attribute.Value);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Activating
    /// </summary>
    [Fact]
    public void User_otp_status_Activating_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(User_otp_status).GetField(nameof(User_otp_status.Activating));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("activating", attribute.Value);
    }
}
