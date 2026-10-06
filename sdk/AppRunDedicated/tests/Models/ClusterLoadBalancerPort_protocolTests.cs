using System.Reflection;
using System.Runtime.Serialization;
using Sacloud.Sdk.AppRunDedicated.Models;

namespace Sacloud.Sdk.AppRunDedicated.Tests.Models;

/// <summary>
/// ClusterLoadBalancerPort_protocol Enumのテスト
/// </summary>
public class ClusterLoadBalancerPort_protocolTests
{
    /// <summary>
    /// すべてのEnum値が定義されていることを確認
    /// </summary>
    [Fact]
    public void CreateLoadBalancerPort_protocol_HasExpectedValues()
    {
        var values = Enum.GetValues<ClusterLoadBalancerPort_protocol>();
        Assert.Contains(ClusterLoadBalancerPort_protocol.Http, values);
        Assert.Contains(ClusterLoadBalancerPort_protocol.Https, values);
        Assert.Contains(ClusterLoadBalancerPort_protocol.Tcp, values);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Http
    /// </summary>
    [Fact]
    public void CreateLoadBalancerPort_protocol_Http_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(ClusterLoadBalancerPort_protocol).GetField(nameof(ClusterLoadBalancerPort_protocol.Http));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("http", attribute.Value);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Https
    /// </summary>
    [Fact]
    public void CreateLoadBalancerPort_protocol_Https_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(ClusterLoadBalancerPort_protocol).GetField(nameof(ClusterLoadBalancerPort_protocol.Https));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("https", attribute.Value);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Tcp
    /// </summary>
    [Fact]
    public void CreateLoadBalancerPort_protocol_Tcp_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(ClusterLoadBalancerPort_protocol).GetField(nameof(ClusterLoadBalancerPort_protocol.Tcp));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("tcp", attribute.Value);
    }
}
