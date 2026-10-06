using System;
using System.Reflection;
using System.Runtime.Serialization;
using Sacloud.Sdk.AppRunDedicated.Models;

namespace Sacloud.Sdk.AppRunDedicated.Tests.Models;

/// <summary>
/// ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly_protocol Enumのテスト
/// </summary>
public class ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly_protocolTests
{

    /// <summary>
    /// すべてのEnum値が定義されていることを確認
    /// </summary>
    [Fact]
    public void ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly_protocol_HasExpectedValues()
    {
        var values = Enum.GetValues<global::Sacloud.Sdk.AppRunDedicated.Models.ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly_protocol>();
        Assert.Contains(global::Sacloud.Sdk.AppRunDedicated.Models.ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly_protocol.Http, values);
        Assert.Contains(global::Sacloud.Sdk.AppRunDedicated.Models.ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly_protocol.Https, values);
        Assert.Contains(global::Sacloud.Sdk.AppRunDedicated.Models.ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly_protocol.Tcp, values);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Http
    /// </summary>
    [Fact]
    public void ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly_protocol_Http_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(global::Sacloud.Sdk.AppRunDedicated.Models.ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly_protocol).GetField(nameof(global::Sacloud.Sdk.AppRunDedicated.Models.ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly_protocol.Http));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("http", attribute!.Value);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Https
    /// </summary>
    [Fact]
    public void ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly_protocol_Https_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(global::Sacloud.Sdk.AppRunDedicated.Models.ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly_protocol).GetField(nameof(global::Sacloud.Sdk.AppRunDedicated.Models.ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly_protocol.Https));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("https", attribute!.Value);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Tcp
    /// </summary>
    [Fact]
    public void ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly_protocol_Tcp_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(global::Sacloud.Sdk.AppRunDedicated.Models.ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly_protocol).GetField(nameof(global::Sacloud.Sdk.AppRunDedicated.Models.ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly_protocol.Tcp));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("tcp", attribute!.Value);
    }
}
