using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// CorsConfigのテスト
/// </summary>
public class CorsConfigTests
{
    /// <summary>
    /// コンストラクタがAdditionalDataを初期化することを検証
    /// </summary>
    [Fact]
    public void Constructor_InitializesAdditionalData()
    {
        var model = new CorsConfig();
        Assert.NotNull(model.AdditionalData);
        Assert.Empty(model.AdditionalData);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = CorsConfig.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<CorsConfig>(result);
    }

    /// <summary>
    /// null ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => CorsConfig.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new CorsConfig();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(8, deserializers.Count);
        Assert.True(deserializers.ContainsKey("accessControlAllowHeaders"));
        Assert.True(deserializers.ContainsKey("accessControlAllowMethods"));
        Assert.True(deserializers.ContainsKey("accessControlAllowOrigins"));
        Assert.True(deserializers.ContainsKey("accessControlExposedHeaders"));
        Assert.True(deserializers.ContainsKey("credentials"));
        Assert.True(deserializers.ContainsKey("maxAge"));
        Assert.True(deserializers.ContainsKey("preflightContinue"));
        Assert.True(deserializers.ContainsKey("privateNetwork"));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new CorsConfig();
        var deserializers = model.GetFieldDeserializers();

        var allowHeadersNode = Substitute.For<IParseNode>();
        allowHeadersNode.GetStringValue().Returns("x-request-id, x-trace-id");
        deserializers["accessControlAllowHeaders"](allowHeadersNode);

        var allowMethodsNode = Substitute.For<IParseNode>();
        allowMethodsNode.GetCollectionOfEnumValues<HTTPMethod>().Returns([HTTPMethod.GET, HTTPMethod.POST]);
        deserializers["accessControlAllowMethods"](allowMethodsNode);

        var allowOriginsNode = Substitute.For<IParseNode>();
        allowOriginsNode.GetStringValue().Returns("https://example.com");
        deserializers["accessControlAllowOrigins"](allowOriginsNode);

        var exposedHeadersNode = Substitute.For<IParseNode>();
        exposedHeadersNode.GetStringValue().Returns("x-response-id");
        deserializers["accessControlExposedHeaders"](exposedHeadersNode);

        var credentialsNode = Substitute.For<IParseNode>();
        credentialsNode.GetBoolValue().Returns(true);
        deserializers["credentials"](credentialsNode);

        var maxAgeNode = Substitute.For<IParseNode>();
        maxAgeNode.GetIntValue().Returns(600);
        deserializers["maxAge"](maxAgeNode);

        var preflightContinueNode = Substitute.For<IParseNode>();
        preflightContinueNode.GetBoolValue().Returns(false);
        deserializers["preflightContinue"](preflightContinueNode);

        var privateNetworkNode = Substitute.For<IParseNode>();
        privateNetworkNode.GetBoolValue().Returns(true);
        deserializers["privateNetwork"](privateNetworkNode);

        Assert.Equal("x-request-id, x-trace-id", model.AccessControlAllowHeaders);
        Assert.Equal([HTTPMethod.GET, HTTPMethod.POST], model.AccessControlAllowMethods);
        Assert.Equal("https://example.com", model.AccessControlAllowOrigins);
        Assert.Equal("x-response-id", model.AccessControlExposedHeaders);
        Assert.True(model.Credentials);
        Assert.Equal(600, model.MaxAge);
        Assert.False(model.PreflightContinue);
        Assert.True(model.PrivateNetwork);
    }

    /// <summary>
    /// null writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new CorsConfig();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティを保持できることを検証
    /// </summary>
    [Fact]
    public async Task CorsConfig_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new CorsConfig
        {
            AccessControlAllowHeaders = "x-request-id, x-trace-id",
            AccessControlAllowMethods = [HTTPMethod.GET, HTTPMethod.POST],
            AccessControlAllowOrigins = "https://example.com",
            AccessControlExposedHeaders = "x-response-id",
            Credentials = true,
            MaxAge = 600,
            PreflightContinue = false,
            PrivateNetwork = true,
        };
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(CorsConfig.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal("x-request-id, x-trace-id", deserializedModel.AccessControlAllowHeaders);
        Assert.Equal([HTTPMethod.GET, HTTPMethod.POST], deserializedModel.AccessControlAllowMethods);
        Assert.Equal("https://example.com", deserializedModel.AccessControlAllowOrigins);
        Assert.Equal("x-response-id", deserializedModel.AccessControlExposedHeaders);
        Assert.True(deserializedModel.Credentials);
        Assert.Equal(600, deserializedModel.MaxAge);
        Assert.False(deserializedModel.PreflightContinue);
        Assert.True(deserializedModel.PrivateNetwork);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task CorsConfig_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new CorsConfig
        {
            AccessControlAllowHeaders = "x-request-id, x-trace-id",
            AccessControlAllowMethods = [HTTPMethod.GET, HTTPMethod.POST],
            AccessControlAllowOrigins = "https://example.com",
            AccessControlExposedHeaders = "x-response-id",
            Credentials = true,
            MaxAge = 600,
            PreflightContinue = false,
            PrivateNetwork = true,
        };
        originalModel.AdditionalData["customField"] = "customValue";
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(CorsConfig.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal("x-request-id, x-trace-id", deserializedModel.AccessControlAllowHeaders);
        Assert.Equal([HTTPMethod.GET, HTTPMethod.POST], deserializedModel.AccessControlAllowMethods);
        Assert.Equal("https://example.com", deserializedModel.AccessControlAllowOrigins);
        Assert.Equal("x-response-id", deserializedModel.AccessControlExposedHeaders);
        Assert.True(deserializedModel.Credentials);
        Assert.Equal(600, deserializedModel.MaxAge);
        Assert.False(deserializedModel.PreflightContinue);
        Assert.True(deserializedModel.PrivateNetwork);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
