using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.CloudHSM.Models;

namespace Sacloud.Sdk.CloudHSM.Tests.Models;

/// <summary>
/// CloudHSMモデルのテスト
/// </summary>
public class CloudHSMTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void CloudHSM_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new global::Sacloud.Sdk.CloudHSM.Models.CloudHSM();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(12, deserializers.Count);
        Assert.True(deserializers.ContainsKey("Availability"));
        Assert.True(deserializers.ContainsKey("CreatedAt"));
        Assert.True(deserializers.ContainsKey("Description"));
        Assert.True(deserializers.ContainsKey("ID"));
        Assert.True(deserializers.ContainsKey("IPv4Address"));
        Assert.True(deserializers.ContainsKey("IPv4NetworkAddress"));
        Assert.True(deserializers.ContainsKey("IPv4PrefixLength"));
        Assert.True(deserializers.ContainsKey("LocalRouter"));
        Assert.True(deserializers.ContainsKey("ModifiedAt"));
        Assert.True(deserializers.ContainsKey("Name"));
        Assert.True(deserializers.ContainsKey("ServiceClass"));
        Assert.True(deserializers.ContainsKey("Tags"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CloudHSM_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new global::Sacloud.Sdk.CloudHSM.Models.CloudHSM();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void CloudHSM_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new global::Sacloud.Sdk.CloudHSM.Models.CloudHSM();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("Availability"));

        parseNode.GetEnumValue<Sacloud.Sdk.CloudHSM.Models.AvailabilityEnum>().Returns((Sacloud.Sdk.CloudHSM.Models.AvailabilityEnum)0);

        deserializers["Availability"](parseNode);
        Assert.NotNull(model.Availability);

        Assert.True(deserializers.ContainsKey("CreatedAt"));

        parseNode.GetDateTimeOffsetValue().Returns(DateTimeOffset.Parse("2026-01-01T00:00:00+00:00"));

        deserializers["CreatedAt"](parseNode);
        Assert.NotNull(model.CreatedAt);

        Assert.True(deserializers.ContainsKey("Description"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["Description"](parseNode);
        Assert.NotNull(model.Description);

        Assert.True(deserializers.ContainsKey("ID"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["ID"](parseNode);
        Assert.NotNull(model.ID);

        Assert.True(deserializers.ContainsKey("IPv4Address"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["IPv4Address"](parseNode);
        Assert.NotNull(model.IPv4Address);

        Assert.True(deserializers.ContainsKey("IPv4NetworkAddress"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["IPv4NetworkAddress"](parseNode);
        Assert.NotNull(model.IPv4NetworkAddress);

        Assert.True(deserializers.ContainsKey("IPv4PrefixLength"));

        parseNode.GetIntValue().Returns(123);

        deserializers["IPv4PrefixLength"](parseNode);
        Assert.NotNull(model.IPv4PrefixLength);

        Assert.True(deserializers.ContainsKey("LocalRouter"));

        parseNode.GetObjectValue<Sacloud.Sdk.CloudHSM.Models.CloudHSMLocalRouter>(Arg.Any<ParsableFactory<Sacloud.Sdk.CloudHSM.Models.CloudHSMLocalRouter>>()).Returns(Substitute.For<Sacloud.Sdk.CloudHSM.Models.CloudHSMLocalRouter>());

        deserializers["LocalRouter"](parseNode);
        Assert.NotNull(model.LocalRouter);

        Assert.True(deserializers.ContainsKey("ModifiedAt"));

        parseNode.GetDateTimeOffsetValue().Returns(DateTimeOffset.Parse("2026-01-01T00:00:00+00:00"));

        deserializers["ModifiedAt"](parseNode);
        Assert.NotNull(model.ModifiedAt);

        Assert.True(deserializers.ContainsKey("Name"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["Name"](parseNode);
        Assert.NotNull(model.Name);

        Assert.True(deserializers.ContainsKey("ServiceClass"));

        parseNode.GetEnumValue<Sacloud.Sdk.CloudHSM.Models.ServiceClassEnum>().Returns((Sacloud.Sdk.CloudHSM.Models.ServiceClassEnum)0);

        deserializers["ServiceClass"](parseNode);
        Assert.NotNull(model.ServiceClass);

        Assert.True(deserializers.ContainsKey("Tags"));

        Assert.NotNull(parseNode);

        deserializers["Tags"](parseNode);
        Assert.NotNull(model.Tags);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CloudHSM_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new global::Sacloud.Sdk.CloudHSM.Models.CloudHSM();
        originalModel.Description = "sample";
        originalModel.IPv4NetworkAddress = "sample";
        originalModel.IPv4PrefixLength = 1;
        originalModel.Name = "sample";
        originalModel.Tags = [];

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue<global::Sacloud.Sdk.CloudHSM.Models.CloudHSM>(global::Sacloud.Sdk.CloudHSM.Models.CloudHSM.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Description, deserializedModel!.Description);
        Assert.Equal(originalModel.IPv4NetworkAddress, deserializedModel!.IPv4NetworkAddress);
        Assert.Equal(originalModel.IPv4PrefixLength, deserializedModel!.IPv4PrefixLength);
        Assert.Equal(originalModel.Name, deserializedModel!.Name);
        Assert.NotNull(deserializedModel!.Tags);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task CloudHSM_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new global::Sacloud.Sdk.CloudHSM.Models.CloudHSM
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
        var deserializedModel = parseNode.GetObjectValue<global::Sacloud.Sdk.CloudHSM.Models.CloudHSM>(global::Sacloud.Sdk.CloudHSM.Models.CloudHSM.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel!.AdditionalData.ContainsKey("extra_key"));
    }
}
