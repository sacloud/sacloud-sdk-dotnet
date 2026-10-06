using System.Reflection;
using System.Runtime.Serialization;

namespace Sacloud.Sdk.AddOn.Tests.Models;

/// <summary>
/// DatalakePostRequestBody_location Enumのテスト
/// </summary>
public class DatalakePostRequestBody_locationTests
{

    /// <summary>
    /// すべてのEnum値が定義されていることを確認
    /// </summary>
    [Fact]
    public void DatalakePostRequestBody_location_HasExpectedValues()
    {
        var values = Enum.GetValues<global::Sacloud.Sdk.AddOn.Models.DatalakePostRequestBody_location>();
        Assert.Contains(global::Sacloud.Sdk.AddOn.Models.DatalakePostRequestBody_location.Japaneast, values);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Japaneast
    /// </summary>
    [Fact]
    public void DatalakePostRequestBody_location_Japaneast_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(global::Sacloud.Sdk.AddOn.Models.DatalakePostRequestBody_location).GetField(nameof(global::Sacloud.Sdk.AddOn.Models.DatalakePostRequestBody_location.Japaneast));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("japaneast", attribute!.Value);
    }
}
