using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using UserSecurityKeyModel = Sacloud.Sdk.IAM.Models.UserSecurityKey;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// UserSecurityKeyのテスト
/// </summary>
public class UserSecurityKeyTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = UserSecurityKeyModel.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<UserSecurityKeyModel>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => UserSecurityKeyModel.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new UserSecurityKeyModel();

        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(6, deserializers.Count);
        Assert.True(deserializers.ContainsKey("aaguid"));
        Assert.True(deserializers.ContainsKey("id"));
        Assert.True(deserializers.ContainsKey("last_used_at"));
        Assert.True(deserializers.ContainsKey("name"));
        Assert.True(deserializers.ContainsKey("registered_at"));
        Assert.True(deserializers.ContainsKey("sign_count"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new UserSecurityKeyModel();

        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// Serializeが正しく値を書き込むことを検証
    /// </summary>
    [Fact]
    public void Serialize_WritesValues()
    {
        var testLastUsedAt = DateTimeOffset.Parse("2024-01-01T12:00:00Z");
        var testRegisteredAt = DateTimeOffset.Parse("2024-01-01T00:00:00Z");
        var model = new UserSecurityKeyModel
        {
            Aaguid = "aaguid-123",
            Id = 123456L,
            LastUsedAt = testLastUsedAt,
            Name = "My Security Key",
            RegisteredAt = testRegisteredAt,
            SignCount = 42
        };
        var writer = Substitute.For<ISerializationWriter>();

        model.Serialize(writer);

        writer.Received(1).WriteStringValue("aaguid", "aaguid-123");
        writer.Received(1).WriteLongValue("id", 123456L);
        writer.Received(1).WriteDateTimeOffsetValue("last_used_at", testLastUsedAt);
        writer.Received(1).WriteStringValue("name", "My Security Key");
        writer.Received(1).WriteDateTimeOffsetValue("registered_at", testRegisteredAt);
        writer.Received(1).WriteIntValue("sign_count", 42);
        writer.Received(1).WriteAdditionalData(Arg.Any<IDictionary<string, object>>());
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new UserSecurityKeyModel();
        var deserializers = model.GetFieldDeserializers();
        
        var parseNodeAaguid = Substitute.For<IParseNode>();
        parseNodeAaguid.GetStringValue().Returns("aaguid-123");
        deserializers["aaguid"](parseNodeAaguid);
        
        var parseNodeId = Substitute.For<IParseNode>();
        parseNodeId.GetLongValue().Returns(123456L);
        deserializers["id"](parseNodeId);
        
        var testLastUsedAt = DateTimeOffset.Parse("2024-01-01T12:00:00Z");
        var parseNodeLastUsedAt = Substitute.For<IParseNode>();
        parseNodeLastUsedAt.GetDateTimeOffsetValue().Returns(testLastUsedAt);
        deserializers["last_used_at"](parseNodeLastUsedAt);
        
        var parseNodeName = Substitute.For<IParseNode>();
        parseNodeName.GetStringValue().Returns("My Security Key");
        deserializers["name"](parseNodeName);
        
        var testRegisteredAt = DateTimeOffset.Parse("2024-01-01T00:00:00Z");
        var parseNodeRegisteredAt = Substitute.For<IParseNode>();
        parseNodeRegisteredAt.GetDateTimeOffsetValue().Returns(testRegisteredAt);
        deserializers["registered_at"](parseNodeRegisteredAt);
        
        var parseNodeSignCount = Substitute.For<IParseNode>();
        parseNodeSignCount.GetIntValue().Returns(42);
        deserializers["sign_count"](parseNodeSignCount);

        Assert.Equal("aaguid-123", model.Aaguid);
        Assert.Equal(123456L, model.Id);
        Assert.Equal(testLastUsedAt, model.LastUsedAt);
        Assert.Equal("My Security Key", model.Name);
        Assert.Equal(testRegisteredAt, model.RegisteredAt);
        Assert.Equal(42, model.SignCount);
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task UserSecurityKey_CanSerializeAndDeserializeWithAllProperties()
    {
        var testLastUsedAt = DateTimeOffset.Parse("2024-01-01T12:00:00Z");
        var testRegisteredAt = DateTimeOffset.Parse("2024-01-01T00:00:00Z");
        var originalModel = new UserSecurityKeyModel
        {
            Aaguid = "aaguid-123",
            Id = 123456L,
            LastUsedAt = testLastUsedAt,
            Name = "My Security Key",
            RegisteredAt = testRegisteredAt,
            SignCount = 42
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(UserSecurityKeyModel.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Aaguid, deserializedModel.Aaguid);
        Assert.Equal(originalModel.Id, deserializedModel.Id);
        Assert.Equal(originalModel.LastUsedAt, deserializedModel.LastUsedAt);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.Equal(originalModel.RegisteredAt, deserializedModel.RegisteredAt);
        Assert.Equal(originalModel.SignCount, deserializedModel.SignCount);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task UserSecurityKey_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new UserSecurityKeyModel
        {
            Name = "My Security Key"
        };
        originalModel.AdditionalData.Add("customField", "customValue");

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(UserSecurityKeyModel.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
