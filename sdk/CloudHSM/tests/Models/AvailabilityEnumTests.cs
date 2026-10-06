using System;
using System.Reflection;
using System.Runtime.Serialization;
using Sacloud.Sdk.CloudHSM.Models;

namespace Sacloud.Sdk.CloudHSM.Tests.Models;

/// <summary>
/// AvailabilityEnum Enumのテスト
/// </summary>
public class AvailabilityEnumTests
{

    /// <summary>
    /// すべてのEnum値が定義されていることを確認
    /// </summary>
    [Fact]
    public void AvailabilityEnum_HasExpectedValues()
    {
        var values = Enum.GetValues<global::Sacloud.Sdk.CloudHSM.Models.AvailabilityEnum>();
        Assert.Contains(global::Sacloud.Sdk.CloudHSM.Models.AvailabilityEnum.Precreate, values);
        Assert.Contains(global::Sacloud.Sdk.CloudHSM.Models.AvailabilityEnum.Available, values);
        Assert.Contains(global::Sacloud.Sdk.CloudHSM.Models.AvailabilityEnum.Discontinued, values);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Precreate
    /// </summary>
    [Fact]
    public void AvailabilityEnum_Precreate_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(global::Sacloud.Sdk.CloudHSM.Models.AvailabilityEnum).GetField(nameof(global::Sacloud.Sdk.CloudHSM.Models.AvailabilityEnum.Precreate));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("precreate", attribute!.Value);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Available
    /// </summary>
    [Fact]
    public void AvailabilityEnum_Available_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(global::Sacloud.Sdk.CloudHSM.Models.AvailabilityEnum).GetField(nameof(global::Sacloud.Sdk.CloudHSM.Models.AvailabilityEnum.Available));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("available", attribute!.Value);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Discontinued
    /// </summary>
    [Fact]
    public void AvailabilityEnum_Discontinued_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(global::Sacloud.Sdk.CloudHSM.Models.AvailabilityEnum).GetField(nameof(global::Sacloud.Sdk.CloudHSM.Models.AvailabilityEnum.Discontinued));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("discontinued", attribute!.Value);
    }
}
