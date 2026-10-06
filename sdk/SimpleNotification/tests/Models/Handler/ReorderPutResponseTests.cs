using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.SimpleNotification.Commonserviceitem.Simplenotification.Routing.Reorder;

namespace Sacloud.Sdk.SimpleNotification.Tests.Models.Handler;

/// <summary>
/// ReorderPutResponseモデルのテスト
/// </summary>
public class ReorderPutResponseTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = ReorderPutResponse.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<ReorderPutResponse>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ReorderPutResponse.CreateFromDiscriminatorValue(null!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new ReorderPutResponse();
        var deserializers = model.GetFieldDeserializers();
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("is_ok"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ReorderPutResponse();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ReorderPutResponse { IsOk = true };

        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        using var stream = new MemoryStream();
        content.CopyTo(stream);
        stream.Position = 0;

        var parseNode = await new JsonParseNodeFactory().GetRootParseNodeAsync("application/json", stream);
        var deserialized = parseNode.GetObjectValue(ReorderPutResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(deserialized);
        Assert.Equal(originalModel.IsOk, deserialized.IsOk);
    }
}
