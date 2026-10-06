using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Models;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models;

/// <summary>
/// Status_dataモデルのテスト
/// </summary>
public class Status_dataTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void Status_data_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new Status_data();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(4, deserializers.Count);
        Assert.True(deserializers.ContainsKey("accept_new"));
        Assert.True(deserializers.ContainsKey("message"));
        Assert.True(deserializers.ContainsKey("started_at"));
        Assert.True(deserializers.ContainsKey("status_code"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Status_data_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new Status_data();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void Status_data_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new Status_data();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("accept_new"));

        parseNode.GetBoolValue().Returns(true);

        deserializers["accept_new"](parseNode);
        Assert.NotNull(model.AcceptNew);

        Assert.True(deserializers.ContainsKey("message"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["message"](parseNode);
        Assert.NotNull(model.Message);

        Assert.True(deserializers.ContainsKey("started_at"));

        parseNode.GetDateTimeOffsetValue().Returns(DateTimeOffset.Parse("2026-01-01T00:00:00+00:00"));

        deserializers["started_at"](parseNode);
        Assert.NotNull(model.StartedAt);

        Assert.True(deserializers.ContainsKey("status_code"));

        parseNode.GetObjectValue(Arg.Any<ParsableFactory<Status_data_status_code>>()).Returns(Substitute.For<Status_data_status_code>());

        deserializers["status_code"](parseNode);
        Assert.NotNull(model.StatusCode);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task Status_data_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new Status_data
        {
            AcceptNew = true,
            Message = "sample",
            StartedAt = DateTimeOffset.Parse("2026-01-01T00:00:00+00:00"),
            StatusCode = new Status_data_status_code
            {
                Id = 200,
                Status = "ok",
            }
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Status_data.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.AcceptNew, deserializedModel.AcceptNew);
        Assert.Equal(originalModel.Message, deserializedModel.Message);
        Assert.Equal(originalModel.StartedAt, deserializedModel.StartedAt);
        Assert.NotNull(deserializedModel.StatusCode);
        Assert.Equal(originalModel.StatusCode.Id, deserializedModel.StatusCode.Id);
        Assert.Equal(originalModel.StatusCode.Status, deserializedModel.StatusCode.Status);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task Status_data_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new Status_data
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
        var deserializedModel = parseNode.GetObjectValue(Status_data.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
