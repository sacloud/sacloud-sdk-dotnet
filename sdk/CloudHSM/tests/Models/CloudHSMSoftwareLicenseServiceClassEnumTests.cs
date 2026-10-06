using System;
using System.Reflection;
using System.Runtime.Serialization;
using Sacloud.Sdk.CloudHSM.Models;

namespace Sacloud.Sdk.CloudHSM.Tests.Models;

/// <summary>
/// CloudHSMSoftwareLicenseServiceClassEnum Enumのテスト
/// </summary>
public class CloudHSMSoftwareLicenseServiceClassEnumTests
{

    /// <summary>
    /// すべてのEnum値が定義されていることを確認
    /// </summary>
    [Fact]
    public void CloudHSMSoftwareLicenseServiceClassEnum_HasExpectedValues()
    {
        var values = Enum.GetValues<global::Sacloud.Sdk.CloudHSM.Models.CloudHSMSoftwareLicenseServiceClassEnum>();
        Assert.Contains(global::Sacloud.Sdk.CloudHSM.Models.CloudHSMSoftwareLicenseServiceClassEnum.CloudCloudhsmLicenseL7, values);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: CloudCloudhsmLicenseL7
    /// </summary>
    [Fact]
    public void CloudHSMSoftwareLicenseServiceClassEnum_CloudCloudhsmLicenseL7_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(global::Sacloud.Sdk.CloudHSM.Models.CloudHSMSoftwareLicenseServiceClassEnum).GetField(nameof(global::Sacloud.Sdk.CloudHSM.Models.CloudHSMSoftwareLicenseServiceClassEnum.CloudCloudhsmLicenseL7));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("cloud/cloudhsm/license/l7", attribute!.Value);
    }
}
