using System.Reflection;
using System.Runtime.Serialization;

namespace Sacloud.Sdk.AddOn.Tests.Models;

/// <summary>
/// StreamingRequestBody_location Enumのテスト
/// </summary>
public class StreamingRequestBody_locationTests
{

    /// <summary>
    /// すべてのEnum値が定義されていることを確認
    /// </summary>
    [Fact]
    public void StreamingRequestBody_location_HasExpectedValues()
    {
        var values = Enum.GetValues<global::Sacloud.Sdk.AddOn.Models.StreamingRequestBody_location>();
        Assert.Contains(global::Sacloud.Sdk.AddOn.Models.StreamingRequestBody_location.Japaneast, values);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Japaneast
    /// </summary>
    [Fact]
    public void StreamingRequestBody_location_Japaneast_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(global::Sacloud.Sdk.AddOn.Models.StreamingRequestBody_location).GetField(nameof(global::Sacloud.Sdk.AddOn.Models.StreamingRequestBody_location.Japaneast));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("japaneast", attribute!.Value);
    }
}
