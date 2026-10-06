using System.Reflection;
using System.Runtime.Serialization;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// IpRestrictionConfig_protocolsのテスト
/// </summary>
public class IpRestrictionConfig_protocolsTests
{
    /// <summary>
    /// Enum値の数が正しいことを検証
    /// </summary>
    [Fact]
    public void EnumValues_HasCorrectCount()
    {
        var values = Enum.GetValues<IpRestrictionConfig_protocols>();
        Assert.Equal(3, values.Length);
    }

    /// <summary>
    /// HttpHttpsメンバーのEnumMember属性が "http,https" であることを検証
    /// </summary>
    [Fact]
    public void IpRestrictionConfig_protocols_HttpHttps_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(IpRestrictionConfig_protocols).GetField(nameof(IpRestrictionConfig_protocols.HttpHttps));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("http,https", attribute.Value);
    }

    /// <summary>
    /// HttpメンバーのEnumMember属性が "http" であることを検証
    /// </summary>
    [Fact]
    public void IpRestrictionConfig_protocols_Http_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(IpRestrictionConfig_protocols).GetField(nameof(IpRestrictionConfig_protocols.Http));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("http", attribute.Value);
    }

    /// <summary>
    /// HttpsメンバーのEnumMember属性が "https" であることを検証
    /// </summary>
    [Fact]
    public void IpRestrictionConfig_protocols_Https_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(IpRestrictionConfig_protocols).GetField(nameof(IpRestrictionConfig_protocols.Https));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("https", attribute.Value);
    }
}
