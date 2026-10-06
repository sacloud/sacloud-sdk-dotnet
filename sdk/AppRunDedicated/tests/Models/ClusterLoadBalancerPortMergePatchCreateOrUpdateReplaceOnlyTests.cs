using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunDedicated.Models;

namespace Sacloud.Sdk.AppRunDedicated.Tests.Models;

/// <summary>
/// ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnlyモデルのテスト
/// </summary>
public class ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnlyTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new global::Sacloud.Sdk.AppRunDedicated.Models.ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("port"));
        Assert.True(deserializers.ContainsKey("protocol"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new global::Sacloud.Sdk.AppRunDedicated.Models.ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new global::Sacloud.Sdk.AppRunDedicated.Models.ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("port"));

        parseNode.GetIntValue().Returns(123);

        deserializers["port"](parseNode);
        Assert.NotNull(model.Port);

        Assert.True(deserializers.ContainsKey("protocol"));

        parseNode.GetEnumValue<Sacloud.Sdk.AppRunDedicated.Models.ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly_protocol>().Returns((Sacloud.Sdk.AppRunDedicated.Models.ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly_protocol)0);

        deserializers["protocol"](parseNode);
        Assert.NotNull(model.Protocol);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new global::Sacloud.Sdk.AppRunDedicated.Models.ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly();
        originalModel.Port = 1;
        originalModel.Protocol = global::Sacloud.Sdk.AppRunDedicated.Models.ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly_protocol.Https;

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue<global::Sacloud.Sdk.AppRunDedicated.Models.ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly>(global::Sacloud.Sdk.AppRunDedicated.Models.ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Port, deserializedModel!.Port);
        Assert.Equal(originalModel.Protocol, deserializedModel!.Protocol);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new global::Sacloud.Sdk.AppRunDedicated.Models.ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly
        {
            AdditionalData = new Dictionary<string, object>
            {
                { "extra_key", "extra-value" },
            },
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, model);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue<global::Sacloud.Sdk.AppRunDedicated.Models.ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly>(global::Sacloud.Sdk.AppRunDedicated.Models.ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel!.AdditionalData.ContainsKey("extra_key"));
    }
}
