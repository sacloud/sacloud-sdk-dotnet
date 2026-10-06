using System.Reflection;
using System.Runtime.Serialization;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// Service_protocolのテスト
/// </summary>
public class Service_protocolTests
{
    /// <summary>
    /// Enum値の数が正しいことを検証
    /// </summary>
    [Fact]
    public void EnumValues_HasCorrectCount()
    {
        var values = Enum.GetValues<Service_protocol>();
        Assert.Equal(2, values.Length);
    }

    /// <summary>
    /// HttpメンバーのEnumMember属性が "http" であることを検証
    /// </summary>
    [Fact]
    public void Service_protocol_Http_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(Service_protocol).GetField(nameof(Service_protocol.Http));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("http", attribute.Value);
    }

    /// <summary>
    /// HttpsメンバーのEnumMember属性が "https" であることを検証
    /// </summary>
    [Fact]
    public void Service_protocol_Https_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(Service_protocol).GetField(nameof(Service_protocol.Https));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("https", attribute.Value);
    }
}
