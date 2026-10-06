using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.KMS.Models;

namespace Sacloud.Sdk.KMS.Tests.Models;

/// <summary>
/// CreateKeyモデルのテスト
/// </summary>
public class CreateKeyTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void CreateKey_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = CreateKey.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<CreateKey>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateKey_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() => CreateKey.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void CreateKey_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new CreateKey();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(8, deserializers.Count);
        Assert.True(deserializers.ContainsKey("CreatedAt"));
        Assert.True(deserializers.ContainsKey("Description"));
        Assert.True(deserializers.ContainsKey("ID"));
        Assert.True(deserializers.ContainsKey("KeyOrigin"));
        Assert.True(deserializers.ContainsKey("ModifiedAt"));
        Assert.True(deserializers.ContainsKey("Name"));
        Assert.True(deserializers.ContainsKey("PlainKey"));
        Assert.True(deserializers.ContainsKey("Tags"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateKey_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new CreateKey();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void CreateKey_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new CreateKey();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        var testDate = DateTimeOffset.Parse("2024-01-15T10:30:00Z");
        parseNode.GetDateTimeOffsetValue().Returns(testDate);
        deserializers["CreatedAt"](parseNode);
        Assert.Equal(testDate, model.CreatedAt);

        parseNode.GetStringValue().Returns("Test description");
        deserializers["Description"](parseNode);
        Assert.Equal("Test description", model.Description);

        parseNode.GetStringValue().Returns("key-123");
        deserializers["ID"](parseNode);
        Assert.Equal("key-123", model.ID);

        parseNode.GetEnumValue<KeyOriginEnum>().Returns(KeyOriginEnum.Imported);
        deserializers["KeyOrigin"](parseNode);
        Assert.Equal(KeyOriginEnum.Imported, model.KeyOrigin);

        var modifiedDate = DateTimeOffset.Parse("2024-02-20T15:45:00Z");
        parseNode.GetDateTimeOffsetValue().Returns(modifiedDate);
        deserializers["ModifiedAt"](parseNode);
        Assert.Equal(modifiedDate, model.ModifiedAt);

        parseNode.GetStringValue().Returns("test-key-name");
        deserializers["Name"](parseNode);
        Assert.Equal("test-key-name", model.Name);

        parseNode.GetStringValue().Returns("plain-key-data");
        deserializers["PlainKey"](parseNode);
        Assert.Equal("plain-key-data", model.PlainKey);

        var tagsList = new List<string> { "tag1", "tag2", "tag3" };
        parseNode.GetCollectionOfPrimitiveValues<string>().Returns(tagsList);
        deserializers["Tags"](parseNode);
        Assert.NotNull(model.Tags);
        Assert.Equal(3, model.Tags.Count);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CreateKey_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new CreateKey
        {
            Description = "Test create key description",
            Name = "test-create-key-name",
            PlainKey = "plain-key-content",
            Tags = new List<string> { "tag1", "tag2" }
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        await content.CopyToAsync(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(CreateKey.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Description, deserializedModel.Description);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.Equal(originalModel.PlainKey, deserializedModel.PlainKey);
        Assert.NotNull(deserializedModel.Tags);
        Assert.Equal(2, deserializedModel.Tags.Count);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CreateKey_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new CreateKey
        {
            Name = "test-key"
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
        var deserializedModel = parseNode.GetObjectValue(CreateKey.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}
