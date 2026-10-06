using System.Reflection;
using System.Runtime.Serialization;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// Plan_maxRequestsUnitのテスト
/// </summary>
public class Plan_maxRequestsUnitTests
{
    /// <summary>
    /// Enum値が正しく定義されていることを確認
    /// </summary>
    [Fact]
    public void EnumValues_AreDefinedCorrectly()
    {
        Assert.Equal(Plan_maxRequestsUnit.Second, Plan_maxRequestsUnit.Second);
        Assert.Equal(Plan_maxRequestsUnit.Minute, Plan_maxRequestsUnit.Minute);
        Assert.Equal(Plan_maxRequestsUnit.Hour, Plan_maxRequestsUnit.Hour);
        Assert.Equal(Plan_maxRequestsUnit.Day, Plan_maxRequestsUnit.Day);
        Assert.Equal(Plan_maxRequestsUnit.Month, Plan_maxRequestsUnit.Month);
        Assert.Equal(Plan_maxRequestsUnit.Year, Plan_maxRequestsUnit.Year);
    }

    /// <summary>
    /// Enum値の数が正しいことを確認
    /// </summary>
    [Fact]
    public void EnumValues_HasCorrectCount()
    {
        var values = Enum.GetValues<Plan_maxRequestsUnit>();

        Assert.Equal(6, values.Length);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Second
    /// </summary>
    [Fact]
    public void Plan_maxRequestsUnit_Second_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(Plan_maxRequestsUnit).GetField(nameof(Plan_maxRequestsUnit.Second));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("second", attribute.Value);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Minute
    /// </summary>
    [Fact]
    public void Plan_maxRequestsUnit_Minute_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(Plan_maxRequestsUnit).GetField(nameof(Plan_maxRequestsUnit.Minute));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("minute", attribute.Value);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Hour
    /// </summary>
    [Fact]
    public void Plan_maxRequestsUnit_Hour_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(Plan_maxRequestsUnit).GetField(nameof(Plan_maxRequestsUnit.Hour));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("hour", attribute.Value);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Day
    /// </summary>
    [Fact]
    public void Plan_maxRequestsUnit_Day_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(Plan_maxRequestsUnit).GetField(nameof(Plan_maxRequestsUnit.Day));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("day", attribute.Value);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Month
    /// </summary>
    [Fact]
    public void Plan_maxRequestsUnit_Month_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(Plan_maxRequestsUnit).GetField(nameof(Plan_maxRequestsUnit.Month));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("month", attribute.Value);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Year
    /// </summary>
    [Fact]
    public void Plan_maxRequestsUnit_Year_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(Plan_maxRequestsUnit).GetField(nameof(Plan_maxRequestsUnit.Year));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("year", attribute.Value);
    }
}
