using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.CloudHSM.Models;

namespace Sacloud.Sdk.CloudHSM.Tests.Models;

/// <summary>
/// CloudHSMPeerモデルのテスト
/// </summary>
public class CloudHSMPeerTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void CloudHSMPeer_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new global::Sacloud.Sdk.CloudHSM.Models.CloudHSMPeer();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("ID"));
        Assert.True(deserializers.ContainsKey("SecretKey"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CloudHSMPeer_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new global::Sacloud.Sdk.CloudHSM.Models.CloudHSMPeer();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void CloudHSMPeer_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new global::Sacloud.Sdk.CloudHSM.Models.CloudHSMPeer();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("ID"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["ID"](parseNode);
        Assert.NotNull(model.ID);

        Assert.True(deserializers.ContainsKey("SecretKey"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["SecretKey"](parseNode);
        Assert.NotNull(model.SecretKey);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CloudHSMPeer_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new global::Sacloud.Sdk.CloudHSM.Models.CloudHSMPeer();

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue<global::Sacloud.Sdk.CloudHSM.Models.CloudHSMPeer>(global::Sacloud.Sdk.CloudHSM.Models.CloudHSMPeer.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task CloudHSMPeer_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new global::Sacloud.Sdk.CloudHSM.Models.CloudHSMPeer
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
        var deserializedModel = parseNode.GetObjectValue<global::Sacloud.Sdk.CloudHSM.Models.CloudHSMPeer>(global::Sacloud.Sdk.CloudHSM.Models.CloudHSMPeer.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel!.AdditionalData.ContainsKey("extra_key"));
    }
}
