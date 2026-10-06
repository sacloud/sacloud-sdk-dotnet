using System.Reflection;
using System.Runtime.Serialization;

namespace Sacloud.Sdk.AddOn.Tests.Models;

/// <summary>
/// EtlPostRequestBody_location Enumのテスト
/// </summary>
public class EtlPostRequestBody_locationTests
{

    /// <summary>
    /// すべてのEnum値が定義されていることを確認
    /// </summary>
    [Fact]
    public void EtlPostRequestBody_location_HasExpectedValues()
    {
        var values = Enum.GetValues<global::Sacloud.Sdk.AddOn.Models.EtlPostRequestBody_location>();
        Assert.Contains(global::Sacloud.Sdk.AddOn.Models.EtlPostRequestBody_location.Japaneast, values);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Japaneast
    /// </summary>
    [Fact]
    public void EtlPostRequestBody_location_Japaneast_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(global::Sacloud.Sdk.AddOn.Models.EtlPostRequestBody_location).GetField(nameof(global::Sacloud.Sdk.AddOn.Models.EtlPostRequestBody_location.Japaneast));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("japaneast", attribute!.Value);
    }
}
