using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.CloudHSM.Models;

namespace Sacloud.Sdk.CloudHSM.Tests.Models;

/// <summary>
/// WrappedCloudHSMSoftwareLicenseモデルのテスト
/// </summary>
public class WrappedCloudHSMSoftwareLicenseTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void WrappedCloudHSMSoftwareLicense_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new global::Sacloud.Sdk.CloudHSM.Models.WrappedCloudHSMSoftwareLicense();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("CloudHSM"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void WrappedCloudHSMSoftwareLicense_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new global::Sacloud.Sdk.CloudHSM.Models.WrappedCloudHSMSoftwareLicense();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void WrappedCloudHSMSoftwareLicense_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new global::Sacloud.Sdk.CloudHSM.Models.WrappedCloudHSMSoftwareLicense();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("CloudHSM"));

        parseNode.GetObjectValue<Sacloud.Sdk.CloudHSM.Models.CloudHSMSoftwareLicense>(Arg.Any<ParsableFactory<Sacloud.Sdk.CloudHSM.Models.CloudHSMSoftwareLicense>>()).Returns(Substitute.For<Sacloud.Sdk.CloudHSM.Models.CloudHSMSoftwareLicense>());

        deserializers["CloudHSM"](parseNode);
        Assert.NotNull(model.CloudHSM);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task WrappedCloudHSMSoftwareLicense_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new global::Sacloud.Sdk.CloudHSM.Models.WrappedCloudHSMSoftwareLicense();
        originalModel.CloudHSM = new Sacloud.Sdk.CloudHSM.Models.CloudHSMSoftwareLicense();

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue<global::Sacloud.Sdk.CloudHSM.Models.WrappedCloudHSMSoftwareLicense>(global::Sacloud.Sdk.CloudHSM.Models.WrappedCloudHSMSoftwareLicense.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel!.CloudHSM);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task WrappedCloudHSMSoftwareLicense_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new global::Sacloud.Sdk.CloudHSM.Models.WrappedCloudHSMSoftwareLicense
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
        var deserializedModel = parseNode.GetObjectValue<global::Sacloud.Sdk.CloudHSM.Models.WrappedCloudHSMSoftwareLicense>(global::Sacloud.Sdk.CloudHSM.Models.WrappedCloudHSMSoftwareLicense.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel!.AdditionalData.ContainsKey("extra_key"));
    }
}
