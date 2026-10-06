using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.SimpleNotification.Models;

namespace Sacloud.Sdk.SimpleNotification.Tests.Models.Root;

/// <summary>
/// ListCommonServiceItemsResponseモデルのテスト
/// </summary>
public class ListCommonServiceItemsResponseTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = ListCommonServiceItemsResponse.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<ListCommonServiceItemsResponse>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ListCommonServiceItemsResponse.CreateFromDiscriminatorValue(null!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new ListCommonServiceItemsResponse();
        var deserializers = model.GetFieldDeserializers();
        Assert.Equal(4, deserializers.Count);
        Assert.True(deserializers.ContainsKey("CommonServiceItems"));
        Assert.True(deserializers.ContainsKey("Count"));
        Assert.True(deserializers.ContainsKey("From"));
        Assert.True(deserializers.ContainsKey("Total"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ListCommonServiceItemsResponse();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new ListCommonServiceItemsResponse();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetIntValue().Returns(10);
        deserializers["Count"](parseNode);
        Assert.Equal(10, model.Count);

        parseNode.GetIntValue().Returns(0);
        deserializers["From"](parseNode);
        Assert.Equal(0, model.From);

        parseNode.GetIntValue().Returns(100);
        deserializers["Total"](parseNode);
        Assert.Equal(100, model.Total);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new ListCommonServiceItemsResponse();
        originalModel.AdditionalData.Add("custom", "value");

        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        using var stream = new MemoryStream();
        content.CopyTo(stream);
        stream.Position = 0;

        var parseNode = await new JsonParseNodeFactory().GetRootParseNodeAsync("application/json", stream);
        var deserialized = parseNode.GetObjectValue(ListCommonServiceItemsResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(deserialized);
        Assert.True(deserialized.AdditionalData.ContainsKey("custom"));
    }
}
