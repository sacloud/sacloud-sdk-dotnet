using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.CloudHSM.Models;

namespace Sacloud.Sdk.CloudHSM.Tests.Models;

/// <summary>
/// PaginatedCloudHSMSoftwareLicenseListモデルのテスト
/// </summary>
public class PaginatedCloudHSMSoftwareLicenseListTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void PaginatedCloudHSMSoftwareLicenseList_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new global::Sacloud.Sdk.CloudHSM.Models.PaginatedCloudHSMSoftwareLicenseList();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(4, deserializers.Count);
        Assert.True(deserializers.ContainsKey("CloudHSMs"));
        Assert.True(deserializers.ContainsKey("Count"));
        Assert.True(deserializers.ContainsKey("From"));
        Assert.True(deserializers.ContainsKey("Total"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void PaginatedCloudHSMSoftwareLicenseList_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new global::Sacloud.Sdk.CloudHSM.Models.PaginatedCloudHSMSoftwareLicenseList();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void PaginatedCloudHSMSoftwareLicenseList_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new global::Sacloud.Sdk.CloudHSM.Models.PaginatedCloudHSMSoftwareLicenseList();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("CloudHSMs"));

        parseNode.GetCollectionOfObjectValues<Sacloud.Sdk.CloudHSM.Models.CloudHSMSoftwareLicense>(Arg.Any<ParsableFactory<Sacloud.Sdk.CloudHSM.Models.CloudHSMSoftwareLicense>>()).Returns(new List<Sacloud.Sdk.CloudHSM.Models.CloudHSMSoftwareLicense>());

        deserializers["CloudHSMs"](parseNode);
        Assert.NotNull(model.CloudHSMs);

        Assert.True(deserializers.ContainsKey("Count"));

        parseNode.GetIntValue().Returns(123);

        deserializers["Count"](parseNode);
        Assert.NotNull(model.Count);

        Assert.True(deserializers.ContainsKey("From"));

        parseNode.GetIntValue().Returns(123);

        deserializers["From"](parseNode);
        Assert.NotNull(model.From);

        Assert.True(deserializers.ContainsKey("Total"));

        parseNode.GetIntValue().Returns(123);

        deserializers["Total"](parseNode);
        Assert.NotNull(model.Total);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task PaginatedCloudHSMSoftwareLicenseList_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new global::Sacloud.Sdk.CloudHSM.Models.PaginatedCloudHSMSoftwareLicenseList();
        originalModel.CloudHSMs = [];
        originalModel.Count = 1;
        originalModel.From = 1;
        originalModel.Total = 1;

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue<global::Sacloud.Sdk.CloudHSM.Models.PaginatedCloudHSMSoftwareLicenseList>(global::Sacloud.Sdk.CloudHSM.Models.PaginatedCloudHSMSoftwareLicenseList.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel!.CloudHSMs);
        Assert.Equal(originalModel.Count, deserializedModel!.Count);
        Assert.Equal(originalModel.From, deserializedModel!.From);
        Assert.Equal(originalModel.Total, deserializedModel!.Total);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task PaginatedCloudHSMSoftwareLicenseList_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new global::Sacloud.Sdk.CloudHSM.Models.PaginatedCloudHSMSoftwareLicenseList
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
        var deserializedModel = parseNode.GetObjectValue<global::Sacloud.Sdk.CloudHSM.Models.PaginatedCloudHSMSoftwareLicenseList>(global::Sacloud.Sdk.CloudHSM.Models.PaginatedCloudHSMSoftwareLicenseList.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel!.AdditionalData.ContainsKey("extra_key"));
    }
}
