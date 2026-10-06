using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// Jwtのテスト
/// </summary>
public class JwtTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = Jwt.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<Jwt>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Jwt.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"createdAt", "id", "updatedAt", "algorithm", "key", "secret"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new Jwt();

        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(6, deserializers.Count);
        Assert.True(deserializers.ContainsKey("createdAt"));
        Assert.True(deserializers.ContainsKey("id"));
        Assert.True(deserializers.ContainsKey("updatedAt"));
        Assert.True(deserializers.ContainsKey("algorithm"));
        Assert.True(deserializers.ContainsKey("key"));
        Assert.True(deserializers.ContainsKey("secret"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new Jwt();

        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new Jwt();
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

        var parseNodeAlgorithm = Substitute.For<IParseNode>();
        parseNodeAlgorithm.GetEnumValue<Jwt_algorithm>().Returns(Jwt_algorithm.HS256);
        deserializers["algorithm"](parseNodeAlgorithm);

        var parseNodeKey = Substitute.For<IParseNode>();
        parseNodeKey.GetStringValue().Returns("jwt-key");
        deserializers["key"](parseNodeKey);

        var parseNodeSecret = Substitute.For<IParseNode>();
        parseNodeSecret.GetStringValue().Returns("jwt-secret");
        deserializers["secret"](parseNodeSecret);

        Assert.Equal(new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), model.CreatedAt);
        Assert.Equal(testId, model.Id);
        Assert.Equal(new DateTimeOffset(2024, 1, 2, 0, 0, 0, TimeSpan.Zero), model.UpdatedAt);
        Assert.Equal(Jwt_algorithm.HS256, model.Algorithm);
        Assert.Equal("jwt-key", model.Key);
        Assert.Equal("jwt-secret", model.Secret);
    }

    /// <summary>
    /// Serializeが正しく値を書き込むことを検証
    /// </summary>
    [Fact]
    public void Serialize_WritesValues()
    {
        var model = new Jwt
        {
            Algorithm = Jwt_algorithm.HS256,
            Key = "jwt-key",
            Secret = "jwt-secret",
        };
        var writer = Substitute.For<ISerializationWriter>();

        model.Serialize(writer);

        writer.Received(1).WriteEnumValue<Jwt_algorithm>("algorithm", Jwt_algorithm.HS256);
        writer.Received(1).WriteStringValue("key", "jwt-key");
        writer.Received(1).WriteStringValue("secret", "jwt-secret");
        writer.Received(1).WriteAdditionalData(Arg.Any<IDictionary<string, object>>());
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task Jwt_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new Jwt
        {
            Algorithm = Jwt_algorithm.HS256,
            Key = "jwt-key",
            Secret = "jwt-secret",
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Jwt.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(Jwt_algorithm.HS256, deserializedModel.Algorithm);
        Assert.Equal("jwt-key", deserializedModel.Key);
        Assert.Equal("jwt-secret", deserializedModel.Secret);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task Jwt_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new Jwt
        {
            Algorithm = Jwt_algorithm.HS256,
            Key = "jwt-key",
            Secret = "jwt-secret",
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
        var deserializedModel = parseNode.GetObjectValue(Jwt.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
