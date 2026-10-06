using Sacloud.Sdk.IAM.Models;
using System.Reflection;
using System.Runtime.Serialization;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// User_statusのテスト
/// </summary>
public class User_statusTests
{
    /// <summary>
    /// Enum値が正しく定義されていることを確認
    /// </summary>
    [Fact]
    public void EnumValues_AreDefinedCorrectly()
    {
        var available = User_status.Available;

        Assert.Equal(User_status.Available, available);
    }

    /// <summary>
    /// Enum値の数が正しいことを確認
    /// </summary>
    [Fact]
    public void EnumValues_HasCorrectCount()
    {
        var values = Enum.GetValues(typeof(User_status));

        Assert.Single(values);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Available
    /// </summary>
    [Fact]
    public void User_status_Available_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(User_status).GetField(nameof(User_status.Available));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("available", attribute.Value);
    }
}
