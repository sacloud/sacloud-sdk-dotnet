using System.Text;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.SecretManager.Models;

namespace Sacloud.Sdk.SecretManager.Tests.Models;

/// <summary>
/// Vaultモデルのテスト
/// </summary>
public class VaultTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void Vault_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = Vault.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<Vault>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Vault_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() => Vault.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void Vault_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new Vault();

        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(7, deserializers.Count);
        Assert.True(deserializers.ContainsKey("CreatedAt"));
        Assert.True(deserializers.ContainsKey("Description"));
        Assert.True(deserializers.ContainsKey("ID"));
        Assert.True(deserializers.ContainsKey("KmsKeyID"));
        Assert.True(deserializers.ContainsKey("ModifiedAt"));
        Assert.True(deserializers.ContainsKey("Name"));
        Assert.True(deserializers.ContainsKey("Tags"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Vault_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new Vault();

        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void Vault_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new Vault();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetDateTimeOffsetValue().Returns(DateTimeOffset.Now);
        deserializers["CreatedAt"](parseNode);

        parseNode.GetStringValue().Returns("test-description");
        deserializers["Description"](parseNode);
        Assert.Equal("test-description", model.Description);

        parseNode.GetStringValue().Returns("test-id");
        deserializers["ID"](parseNode);

        parseNode.GetStringValue().Returns("test-kms-key-id");
        deserializers["KmsKeyID"](parseNode);

        parseNode.GetDateTimeOffsetValue().Returns(DateTimeOffset.Now);
        deserializers["ModifiedAt"](parseNode);

        parseNode.GetStringValue().Returns("test-name");
        deserializers["Name"](parseNode);
        Assert.Equal("test-name", model.Name);

        var tagsList = new List<string> { "tag1", "tag2" };
        parseNode.GetCollectionOfPrimitiveValues<string>().Returns(tagsList);
        deserializers["Tags"](parseNode);
        Assert.NotNull(model.Tags);
        Assert.Equal(2, model.Tags.Count);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task Vault_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new Vault
        {
            Description = "test-description",
            Name = "test-name",
            Tags = new List<string> { "tag1", "tag2" }
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Vault.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Description, deserializedModel.Description);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.NotNull(deserializedModel.Tags);
        Assert.Equal(2, deserializedModel.Tags.Count);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task Vault_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new Vault
        {
            Name = "test-name"
        };
        originalModel.AdditionalData.Add("custom_field_1", "custom_value_1");
        originalModel.AdditionalData.Add("custom_field_2", 42);

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Vault.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }

    /// <summary>
    /// CreatedAtとModifiedAtがデシリアライズで欠損しないことを確認
    /// </summary>
    [Fact]
    public async Task Vault_CanDeserializeDateTimeFields()
    {
        const string json = "{\"CreatedAt\":\"2024-01-02T03:04:05Z\",\"ModifiedAt\":\"2024-01-03T04:05:06+09:00\",\"Name\":\"test-vault\"}";
        await using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", stream);

        var model = parseNode.GetObjectValue(Vault.CreateFromDiscriminatorValue);

        Assert.NotNull(model);
        Assert.Equal(DateTimeOffset.Parse("2024-01-02T03:04:05Z"), model.CreatedAt);
        Assert.Equal(DateTimeOffset.Parse("2024-01-03T04:05:06+09:00"), model.ModifiedAt);
        Assert.Equal("test-vault", model.Name);
    }
}
