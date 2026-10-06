using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Models;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models;

/// <summary>
/// Status_data_status_codeモデルのテスト
/// </summary>
public class Status_data_status_codeTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void Status_data_status_code_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new Status_data_status_code();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("id"));
        Assert.True(deserializers.ContainsKey("status"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Status_data_status_code_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new Status_data_status_code();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void Status_data_status_code_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new Status_data_status_code();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("id"));

        parseNode.GetIntValue().Returns(123);

        deserializers["id"](parseNode);
        Assert.NotNull(model.Id);

        Assert.True(deserializers.ContainsKey("status"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["status"](parseNode);
        Assert.NotNull(model.Status);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task Status_data_status_code_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new Status_data_status_code
        {
            Id = 1,
            Status = "sample"
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Status_data_status_code.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Id, deserializedModel.Id);
        Assert.Equal(originalModel.Status, deserializedModel.Status);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task Status_data_status_code_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new Status_data_status_code
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
        var deserializedModel = parseNode.GetObjectValue(Status_data_status_code.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
