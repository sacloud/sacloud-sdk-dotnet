using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.CloudHSM.Models;

namespace Sacloud.Sdk.CloudHSM.Tests.Models;

/// <summary>
/// WrappedCreateCloudHSMPeerモデルのテスト
/// </summary>
public class WrappedCreateCloudHSMPeerTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void WrappedCreateCloudHSMPeer_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new global::Sacloud.Sdk.CloudHSM.Models.WrappedCreateCloudHSMPeer();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("Peer"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void WrappedCreateCloudHSMPeer_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new global::Sacloud.Sdk.CloudHSM.Models.WrappedCreateCloudHSMPeer();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void WrappedCreateCloudHSMPeer_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new global::Sacloud.Sdk.CloudHSM.Models.WrappedCreateCloudHSMPeer();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("Peer"));

        parseNode.GetObjectValue<Sacloud.Sdk.CloudHSM.Models.CreateCloudHSMPeer>(Arg.Any<ParsableFactory<Sacloud.Sdk.CloudHSM.Models.CreateCloudHSMPeer>>()).Returns(Substitute.For<Sacloud.Sdk.CloudHSM.Models.CreateCloudHSMPeer>());

        deserializers["Peer"](parseNode);
        Assert.NotNull(model.Peer);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task WrappedCreateCloudHSMPeer_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new global::Sacloud.Sdk.CloudHSM.Models.WrappedCreateCloudHSMPeer();
        originalModel.Peer = new Sacloud.Sdk.CloudHSM.Models.CreateCloudHSMPeer();

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue<global::Sacloud.Sdk.CloudHSM.Models.WrappedCreateCloudHSMPeer>(global::Sacloud.Sdk.CloudHSM.Models.WrappedCreateCloudHSMPeer.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel!.Peer);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task WrappedCreateCloudHSMPeer_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new global::Sacloud.Sdk.CloudHSM.Models.WrappedCreateCloudHSMPeer
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
        var deserializedModel = parseNode.GetObjectValue<global::Sacloud.Sdk.CloudHSM.Models.WrappedCreateCloudHSMPeer>(global::Sacloud.Sdk.CloudHSM.Models.WrappedCreateCloudHSMPeer.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel!.AdditionalData.ContainsKey("extra_key"));
    }

    /// <summary>
    /// 必須プロパティが不足している場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void WrappedCreateCloudHSMPeer_Validation_WithMissingRequiredProperty_ReturnsValidationError()
    {
        var model = new WrappedCreateCloudHSMPeer();
        var validationContext = new ValidationContext(model);
        var validationResults = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.MemberNames.Contains(nameof(WrappedCreateCloudHSMPeer.Peer)));
    }
}
