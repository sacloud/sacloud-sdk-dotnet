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
/// CreateCloudHSMPeerモデルのテスト
/// </summary>
public class CreateCloudHSMPeerTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void CreateCloudHSMPeer_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new global::Sacloud.Sdk.CloudHSM.Models.CreateCloudHSMPeer();
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
    public void CreateCloudHSMPeer_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new global::Sacloud.Sdk.CloudHSM.Models.CreateCloudHSMPeer();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void CreateCloudHSMPeer_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new global::Sacloud.Sdk.CloudHSM.Models.CreateCloudHSMPeer();
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
    public async Task CreateCloudHSMPeer_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new global::Sacloud.Sdk.CloudHSM.Models.CreateCloudHSMPeer();
        originalModel.ID = "sample";
        originalModel.SecretKey = "sample";

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue<global::Sacloud.Sdk.CloudHSM.Models.CreateCloudHSMPeer>(global::Sacloud.Sdk.CloudHSM.Models.CreateCloudHSMPeer.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.ID, deserializedModel!.ID);
        Assert.Equal(originalModel.SecretKey, deserializedModel!.SecretKey);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task CreateCloudHSMPeer_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new global::Sacloud.Sdk.CloudHSM.Models.CreateCloudHSMPeer
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
        var deserializedModel = parseNode.GetObjectValue<global::Sacloud.Sdk.CloudHSM.Models.CreateCloudHSMPeer>(global::Sacloud.Sdk.CloudHSM.Models.CreateCloudHSMPeer.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel!.AdditionalData.ContainsKey("extra_key"));
    }

    /// <summary>
    /// 必須プロパティが不足している場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void CreateCloudHSMPeer_Validation_WithMissingRequiredProperties_ReturnsValidationErrors()
    {
        var model = new CreateCloudHSMPeer();
        var validationContext = new ValidationContext(model);
        var validationResults = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.MemberNames.Contains(nameof(CreateCloudHSMPeer.ID)));
        Assert.Contains(validationResults, r => r.MemberNames.Contains(nameof(CreateCloudHSMPeer.SecretKey)));
    }
}
