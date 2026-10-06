using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// HmacAuthのテスト
/// </summary>
public class HmacAuthTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = HmacAuth.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<HmacAuth>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => HmacAuth.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"createdAt", "id", "updatedAt", "secret", "userName"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new HmacAuth();

        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(5, deserializers.Count);
        Assert.True(deserializers.ContainsKey("createdAt"));
        Assert.True(deserializers.ContainsKey("id"));
        Assert.True(deserializers.ContainsKey("updatedAt"));
        Assert.True(deserializers.ContainsKey("secret"));
        Assert.True(deserializers.ContainsKey("userName"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new HmacAuth();

        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new HmacAuth();
        var deserializers = model.GetFieldDeserializers();

        var testId = new Guid("12345678-1234-1234-1234-123456789012");
        var parseNodeCreatedAt = Substitute.For<IParseNode>();
        parseNodeCreatedAt.GetDateTimeOffsetValue().Returns(new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero));
        deserializers["createdAt"](parseNodeCreatedAt);

        var parseNodeId = Substitute.For<IParseNode>();
        parseNodeId.GetGuidValue().Returns(testId);
        deserializers["id"](parseNodeId);

        var parseNodeUpdatedAt = Substitute.For<IParseNode>();
        parseNodeUpdatedAt.GetDateTimeOffsetValue().Returns(new DateTimeOffset(2024, 1, 2, 0, 0, 0, TimeSpan.Zero));
        deserializers["updatedAt"](parseNodeUpdatedAt);

        var parseNodeSecret = Substitute.For<IParseNode>();
        parseNodeSecret.GetStringValue().Returns("hmac-secret");
        deserializers["secret"](parseNodeSecret);

        var parseNodeUserName = Substitute.For<IParseNode>();
        parseNodeUserName.GetStringValue().Returns("test-user");
        deserializers["userName"](parseNodeUserName);

        Assert.Equal(new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), model.CreatedAt);
        Assert.Equal(testId, model.Id);
        Assert.Equal(new DateTimeOffset(2024, 1, 2, 0, 0, 0, TimeSpan.Zero), model.UpdatedAt);
        Assert.Equal("hmac-secret", model.Secret);
        Assert.Equal("test-user", model.UserName);
    }

    /// <summary>
    /// Serializeが正しく値を書き込むことを検証
    /// </summary>
    [Fact]
    public void Serialize_WritesValues()
    {
        var model = new HmacAuth
        {
            Secret = "hmac-secret",
            UserName = "user-1",
        };
        var writer = Substitute.For<ISerializationWriter>();

        model.Serialize(writer);

        writer.Received(1).WriteStringValue("secret", "hmac-secret");
        writer.Received(1).WriteStringValue("userName", "user-1");
        writer.Received(1).WriteAdditionalData(Arg.Any<IDictionary<string, object>>());
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task HmacAuth_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new HmacAuth
        {
            Secret = "hmac-secret",
            UserName = "test-user",
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(HmacAuth.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal("hmac-secret", deserializedModel.Secret);
        Assert.Equal("test-user", deserializedModel.UserName);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task HmacAuth_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new HmacAuth
        {
            Secret = "hmac-secret",
            UserName = "test-user",
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
        var deserializedModel = parseNode.GetObjectValue(HmacAuth.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
