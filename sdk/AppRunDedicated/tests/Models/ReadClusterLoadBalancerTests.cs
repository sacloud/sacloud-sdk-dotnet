using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunDedicated.Models;

namespace Sacloud.Sdk.AppRunDedicated.Tests.Models;

/// <summary>
/// ReadClusterLoadBalancerモデルのテスト
/// </summary>
public class ReadClusterLoadBalancerTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void ReadClusterLoadBalancer_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new global::Sacloud.Sdk.AppRunDedicated.Models.ReadClusterLoadBalancer();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("hasLetsEncryptEmail"));
        Assert.True(deserializers.ContainsKey("ports"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ReadClusterLoadBalancer_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new global::Sacloud.Sdk.AppRunDedicated.Models.ReadClusterLoadBalancer();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void ReadClusterLoadBalancer_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new global::Sacloud.Sdk.AppRunDedicated.Models.ReadClusterLoadBalancer();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("hasLetsEncryptEmail"));

        parseNode.GetBoolValue().Returns(true);

        deserializers["hasLetsEncryptEmail"](parseNode);
        Assert.NotNull(model.HasLetsEncryptEmail);

        Assert.True(deserializers.ContainsKey("ports"));

        parseNode.GetCollectionOfObjectValues<Sacloud.Sdk.AppRunDedicated.Models.ClusterLoadBalancerPort>(Arg.Any<ParsableFactory<Sacloud.Sdk.AppRunDedicated.Models.ClusterLoadBalancerPort>>()).Returns(new List<Sacloud.Sdk.AppRunDedicated.Models.ClusterLoadBalancerPort>());

        deserializers["ports"](parseNode);
        Assert.NotNull(model.Ports);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ReadClusterLoadBalancer_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new global::Sacloud.Sdk.AppRunDedicated.Models.ReadClusterLoadBalancer();
        originalModel.HasLetsEncryptEmail = true;
        originalModel.Ports = [];

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue<global::Sacloud.Sdk.AppRunDedicated.Models.ReadClusterLoadBalancer>(global::Sacloud.Sdk.AppRunDedicated.Models.ReadClusterLoadBalancer.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.HasLetsEncryptEmail, deserializedModel!.HasLetsEncryptEmail);
        Assert.NotNull(deserializedModel!.Ports);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task ReadClusterLoadBalancer_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new global::Sacloud.Sdk.AppRunDedicated.Models.ReadClusterLoadBalancer
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
        var deserializedModel = parseNode.GetObjectValue<global::Sacloud.Sdk.AppRunDedicated.Models.ReadClusterLoadBalancer>(global::Sacloud.Sdk.AppRunDedicated.Models.ReadClusterLoadBalancer.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel!.AdditionalData.ContainsKey("extra_key"));
    }
}
