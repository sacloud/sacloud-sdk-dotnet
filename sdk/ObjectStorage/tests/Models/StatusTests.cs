using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Models;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models;

/// <summary>
/// Statusモデルのテスト
/// </summary>
public class StatusTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void Status_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new ObjectStorage.Models.Status();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("data"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Status_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ObjectStorage.Models.Status();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void Status_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new ObjectStorage.Models.Status();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("data"));

        parseNode.GetObjectValue(Arg.Any<ParsableFactory<Status_data>>()).Returns(Substitute.For<Status_data>());

        deserializers["data"](parseNode);
        Assert.NotNull(model.Data);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task Status_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ObjectStorage.Models.Status
        {
            Data = new Status_data
            {
                AcceptNew = true,
                Message = "service is ready",
                StartedAt = DateTimeOffset.Parse("2026-01-01T00:00:00+00:00"),
                StatusCode = new Status_data_status_code
                {
                    Id = 200,
                    Status = "ok",
                },
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
        var deserializedModel = parseNode.GetObjectValue(ObjectStorage.Models.Status.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Data);
        Assert.Equal(originalModel.Data.AcceptNew, deserializedModel.Data.AcceptNew);
        Assert.Equal(originalModel.Data.Message, deserializedModel.Data.Message);
        Assert.Equal(originalModel.Data.StartedAt, deserializedModel.Data.StartedAt);
        Assert.NotNull(deserializedModel.Data.StatusCode);
        Assert.Equal(originalModel.Data.StatusCode.Id, deserializedModel.Data.StatusCode.Id);
        Assert.Equal(originalModel.Data.StatusCode.Status, deserializedModel.Data.StatusCode.Status);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task Status_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new ObjectStorage.Models.Status
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
        var deserializedModel = parseNode.GetObjectValue(ObjectStorage.Models.Status.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
