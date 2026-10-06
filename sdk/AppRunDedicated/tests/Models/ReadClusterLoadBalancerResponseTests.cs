using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunDedicated.Models;

namespace Sacloud.Sdk.AppRunDedicated.Tests.Models;

/// <summary>
/// ReadClusterLoadBalancerResponseモデルのテスト
/// </summary>
public class ReadClusterLoadBalancerResponseTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void ReadClusterLoadBalancerResponse_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new global::Sacloud.Sdk.AppRunDedicated.Models.ReadClusterLoadBalancerResponse();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("loadBalancer"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ReadClusterLoadBalancerResponse_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new global::Sacloud.Sdk.AppRunDedicated.Models.ReadClusterLoadBalancerResponse();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void ReadClusterLoadBalancerResponse_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new global::Sacloud.Sdk.AppRunDedicated.Models.ReadClusterLoadBalancerResponse();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("loadBalancer"));

        parseNode.GetObjectValue<Sacloud.Sdk.AppRunDedicated.Models.ReadClusterLoadBalancer>(Arg.Any<ParsableFactory<Sacloud.Sdk.AppRunDedicated.Models.ReadClusterLoadBalancer>>()).Returns(Substitute.For<Sacloud.Sdk.AppRunDedicated.Models.ReadClusterLoadBalancer>());

        deserializers["loadBalancer"](parseNode);
        Assert.NotNull(model.LoadBalancer);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ReadClusterLoadBalancerResponse_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new global::Sacloud.Sdk.AppRunDedicated.Models.ReadClusterLoadBalancerResponse();
        originalModel.LoadBalancer = new Sacloud.Sdk.AppRunDedicated.Models.ReadClusterLoadBalancer();

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue<global::Sacloud.Sdk.AppRunDedicated.Models.ReadClusterLoadBalancerResponse>(global::Sacloud.Sdk.AppRunDedicated.Models.ReadClusterLoadBalancerResponse.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel!.LoadBalancer);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task ReadClusterLoadBalancerResponse_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new global::Sacloud.Sdk.AppRunDedicated.Models.ReadClusterLoadBalancerResponse
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
        var deserializedModel = parseNode.GetObjectValue<global::Sacloud.Sdk.AppRunDedicated.Models.ReadClusterLoadBalancerResponse>(global::Sacloud.Sdk.AppRunDedicated.Models.ReadClusterLoadBalancerResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel!.AdditionalData.ContainsKey("extra_key"));
    }
}
