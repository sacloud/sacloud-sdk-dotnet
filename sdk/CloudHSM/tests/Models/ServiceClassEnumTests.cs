using System;
using System.Reflection;
using System.Runtime.Serialization;
using Sacloud.Sdk.CloudHSM.Models;

namespace Sacloud.Sdk.CloudHSM.Tests.Models;

/// <summary>
/// ServiceClassEnum Enumのテスト
/// </summary>
public class ServiceClassEnumTests
{

    /// <summary>
    /// すべてのEnum値が定義されていることを確認
    /// </summary>
    [Fact]
    public void ServiceClassEnum_HasExpectedValues()
    {
        var values = Enum.GetValues<global::Sacloud.Sdk.CloudHSM.Models.ServiceClassEnum>();
        Assert.Contains(global::Sacloud.Sdk.CloudHSM.Models.ServiceClassEnum.CloudCloudhsmPartition, values);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: CloudCloudhsmPartition
    /// </summary>
    [Fact]
    public void ServiceClassEnum_CloudCloudhsmPartition_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(global::Sacloud.Sdk.CloudHSM.Models.ServiceClassEnum).GetField(nameof(global::Sacloud.Sdk.CloudHSM.Models.ServiceClassEnum.CloudCloudhsmPartition));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("cloud/cloudhsm/partition", attribute!.Value);
    }
}
