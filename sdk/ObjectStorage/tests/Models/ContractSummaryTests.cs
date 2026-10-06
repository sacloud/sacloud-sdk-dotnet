using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Models;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models;

/// <summary>
/// ContractSummaryモデルのテスト
/// </summary>
public class ContractSummaryTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void ContractSummary_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new ContractSummary();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(4, deserializers.Count);
        Assert.True(deserializers.ContainsKey("created_at"));
        Assert.True(deserializers.ContainsKey("deleted_at"));
        Assert.True(deserializers.ContainsKey("resource_id"));
        Assert.True(deserializers.ContainsKey("status"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ContractSummary_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ContractSummary();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void ContractSummary_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new ContractSummary();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("created_at"));

        parseNode.GetDateTimeOffsetValue().Returns(DateTimeOffset.Parse("2026-01-01T00:00:00+00:00"));

        deserializers["created_at"](parseNode);
        Assert.NotNull(model.CreatedAt);

        Assert.True(deserializers.ContainsKey("deleted_at"));

        parseNode.GetDateTimeOffsetValue().Returns(DateTimeOffset.Parse("2026-01-01T00:00:00+00:00"));

        deserializers["deleted_at"](parseNode);
        Assert.NotNull(model.DeletedAt);

        Assert.True(deserializers.ContainsKey("resource_id"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["resource_id"](parseNode);
        Assert.NotNull(model.ResourceId);

        Assert.True(deserializers.ContainsKey("status"));

        parseNode.GetEnumValue<ContractSummary_status>().Returns(ContractSummary_status.Active);

        deserializers["status"](parseNode);
        Assert.NotNull(model.Status);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ContractSummary_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ContractSummary
        {
            CreatedAt = DateTimeOffset.Parse("2026-01-01T00:00:00+00:00"),
            DeletedAt = DateTimeOffset.Parse("2026-01-01T00:00:00+00:00"),
            ResourceId = "sample",
            Status = ContractSummary_status.Active
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(ContractSummary.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.CreatedAt, deserializedModel.CreatedAt);
        Assert.Equal(originalModel.DeletedAt, deserializedModel.DeletedAt);
        Assert.Equal(originalModel.ResourceId, deserializedModel.ResourceId);
        Assert.Equal(originalModel.Status, deserializedModel.Status);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task ContractSummary_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new ContractSummary
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
        var deserializedModel = parseNode.GetObjectValue(ContractSummary.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
