using System.Reflection;
using System.Runtime.Serialization;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// SubscriptionPlanResponse_maxRequestsUnitのテスト
/// </summary>
public class SubscriptionPlanResponse_maxRequestsUnitTests
{
    /// <summary>
    /// Enum値の数が正しいことを検証
    /// </summary>
    [Fact]
    public void EnumValues_HasCorrectCount()
    {
        var values = Enum.GetValues<SubscriptionPlanResponse_maxRequestsUnit>();
        Assert.Equal(6, values.Length);
    }

    /// <summary>
    /// SecondメンバーのEnumMember属性が "second" であることを検証
    /// </summary>
    [Fact]
    public void SubscriptionPlanResponse_maxRequestsUnit_Second_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(SubscriptionPlanResponse_maxRequestsUnit).GetField(nameof(SubscriptionPlanResponse_maxRequestsUnit.Second));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("second", attribute.Value);
    }

    /// <summary>
    /// MinuteメンバーのEnumMember属性が "minute" であることを検証
    /// </summary>
    [Fact]
    public void SubscriptionPlanResponse_maxRequestsUnit_Minute_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(SubscriptionPlanResponse_maxRequestsUnit).GetField(nameof(SubscriptionPlanResponse_maxRequestsUnit.Minute));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("minute", attribute.Value);
    }

    /// <summary>
    /// HourメンバーのEnumMember属性が "hour" であることを検証
    /// </summary>
    [Fact]
    public void SubscriptionPlanResponse_maxRequestsUnit_Hour_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(SubscriptionPlanResponse_maxRequestsUnit).GetField(nameof(SubscriptionPlanResponse_maxRequestsUnit.Hour));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("hour", attribute.Value);
    }

    /// <summary>
    /// DayメンバーのEnumMember属性が "day" であることを検証
    /// </summary>
    [Fact]
    public void SubscriptionPlanResponse_maxRequestsUnit_Day_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(SubscriptionPlanResponse_maxRequestsUnit).GetField(nameof(SubscriptionPlanResponse_maxRequestsUnit.Day));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("day", attribute.Value);
    }

    /// <summary>
    /// MonthメンバーのEnumMember属性が "month" であることを検証
    /// </summary>
    [Fact]
    public void SubscriptionPlanResponse_maxRequestsUnit_Month_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(SubscriptionPlanResponse_maxRequestsUnit).GetField(nameof(SubscriptionPlanResponse_maxRequestsUnit.Month));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("month", attribute.Value);
    }

    /// <summary>
    /// YearメンバーのEnumMember属性が "year" であることを検証
    /// </summary>
    [Fact]
    public void SubscriptionPlanResponse_maxRequestsUnit_Year_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(SubscriptionPlanResponse_maxRequestsUnit).GetField(nameof(SubscriptionPlanResponse_maxRequestsUnit.Year));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("year", attribute.Value);
    }
}
