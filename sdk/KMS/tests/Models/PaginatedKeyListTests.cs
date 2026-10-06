using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.KMS.Models;

namespace Sacloud.Sdk.KMS.Tests.Models;

/// <summary>
/// PaginatedKeyListモデルのテスト
/// </summary>
public class PaginatedKeyListTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void PaginatedKeyList_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = PaginatedKeyList.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<PaginatedKeyList>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void PaginatedKeyList_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() => PaginatedKeyList.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void PaginatedKeyList_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new PaginatedKeyList();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(4, deserializers.Count);
        Assert.True(deserializers.ContainsKey("Count"));
        Assert.True(deserializers.ContainsKey("From"));
        Assert.True(deserializers.ContainsKey("Keys"));
        Assert.True(deserializers.ContainsKey("Total"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void PaginatedKeyList_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new PaginatedKeyList();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void PaginatedKeyList_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new PaginatedKeyList();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetIntValue().Returns(10);
        deserializers["Count"](parseNode);
        Assert.Equal(10, model.Count);

        parseNode.GetIntValue().Returns(0);
        deserializers["From"](parseNode);
        Assert.Equal(0, model.From);

        var keysList = new List<Key> { new Key { Name = "key-1" }, new Key { Name = "key-2" } };
        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<Key>>()).Returns(keysList);
        deserializers["Keys"](parseNode);
        Assert.NotNull(model.Keys);
        Assert.Equal(2, model.Keys.Count);

        parseNode.GetIntValue().Returns(100);
        deserializers["Total"](parseNode);
        Assert.Equal(100, model.Total);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task PaginatedKeyList_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new PaginatedKeyList
        {
            Count = 2,
            From = 0,
            Total = 10,
            Keys = new List<Key>
            {
                new Key { Name = "test-key-1", Description = "Description 1" },
                new Key { Name = "test-key-2", Description = "Description 2" }
            }
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        await content.CopyToAsync(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(PaginatedKeyList.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Count, deserializedModel.Count);
        Assert.Equal(originalModel.From, deserializedModel.From);
        Assert.Equal(originalModel.Total, deserializedModel.Total);
        Assert.NotNull(deserializedModel.Keys);
        Assert.Equal(2, deserializedModel.Keys.Count);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task PaginatedKeyList_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new PaginatedKeyList
        {
            Count = 1,
            From = 0,
            Total = 1
        };
        originalModel.AdditionalData.Add("custom_field_1", "custom_value_1");
        originalModel.AdditionalData.Add("custom_field_2", 42);

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        await content.CopyToAsync(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(PaginatedKeyList.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }

    /// <summary>
    /// 空のKeysリストでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task PaginatedKeyList_CanSerializeAndDeserialize_WithEmptyKeysList()
    {
        var originalModel = new PaginatedKeyList
        {
            Count = 0,
            From = 0,
            Total = 0,
            Keys = new List<Key>()
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        await content.CopyToAsync(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(PaginatedKeyList.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(0, deserializedModel.Count);
        Assert.NotNull(deserializedModel.Keys);
        Assert.Empty(deserializedModel.Keys);
    }
}
