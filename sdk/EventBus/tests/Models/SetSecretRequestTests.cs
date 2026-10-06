using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.EventBus.Models;

namespace Sacloud.Sdk.EventBus.Tests.Models;

/// <summary>
/// SetSecretRequestモデルのテスト
/// </summary>
public class SetSecretRequestTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void SetSecretRequest_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = SetSecretRequest.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<SetSecretRequest>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void SetSecretRequest_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() =>
            SetSecretRequest.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void SetSecretRequest_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new SetSecretRequest();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("Secret"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void SetSecretRequest_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new SetSecretRequest();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void SetSecretRequest_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new SetSecretRequest();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();
        var secret = new SetSecretRequest.SetSecretRequest_Secret();

        parseNode.GetObjectValue(Arg.Any<ParsableFactory<SetSecretRequest.SetSecretRequest_Secret>>()).Returns(secret);
        deserializers["Secret"](parseNode);

        Assert.NotNull(model.Secret);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task SetSecretRequest_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new SetSecretRequest
        {
            Secret = new SetSecretRequest.SetSecretRequest_Secret
            {
                SimpleMQSecret = new SimpleMQSecret { APIKey = "test-key" }
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
        var deserializedModel = parseNode.GetObjectValue(SetSecretRequest.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Secret);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task SetSecretRequest_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new SetSecretRequest();
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
        var deserializedModel = parseNode.GetObjectValue(SetSecretRequest.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }

    /// <summary>
    /// SetSecretRequest_Secret.CreateFromDiscriminatorValueがAccessTokenフィールド存在時にSacloudAPISecretを設定することを確認
    /// </summary>
    [Fact]
    public void SetSecretRequest_Secret_CreateFromDiscriminatorValue_WithAccessToken_SetsSacloudAPISecret()
    {
        var parseNode = Substitute.For<IParseNode>();
        var accessTokenNode = Substitute.For<IParseNode>();
        var sacloudSecret = new SacloudAPISecret { AccessToken = "test-token", AccessTokenSecret = "test-secret" };

        parseNode.GetChildNode("AccessToken").Returns(accessTokenNode);
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<SacloudAPISecret>>()).Returns(sacloudSecret);

        var result = SetSecretRequest.SetSecretRequest_Secret.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.NotNull(result.SacloudAPISecret);
        Assert.Null(result.SimpleMQSecret);
        Assert.Equal("test-token", result.SacloudAPISecret.AccessToken);
        Assert.Equal("test-secret", result.SacloudAPISecret.AccessTokenSecret);
    }

    /// <summary>
    /// SetSecretRequest_Secret.CreateFromDiscriminatorValueがAPIKeyフィールド存在時にSimpleMQSecretを設定することを確認
    /// </summary>
    [Fact]
    public void SetSecretRequest_Secret_CreateFromDiscriminatorValue_WithAPIKey_SetsSimpleMQSecret()
    {
        var parseNode = Substitute.For<IParseNode>();
        var apiKeyNode = Substitute.For<IParseNode>();
        var simpleMQSecret = new SimpleMQSecret { APIKey = "test-api-key" };

        parseNode.GetChildNode("AccessToken").Returns((IParseNode?)null);
        parseNode.GetChildNode("APIKey").Returns(apiKeyNode);
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<SimpleMQSecret>>()).Returns(simpleMQSecret);

        var result = SetSecretRequest.SetSecretRequest_Secret.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.Null(result.SacloudAPISecret);
        Assert.NotNull(result.SimpleMQSecret);
        Assert.Equal("test-api-key", result.SimpleMQSecret.APIKey);
    }

    /// <summary>
    /// SetSecretRequest_Secret.CreateFromDiscriminatorValueが判別できない場合に空のインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void SetSecretRequest_Secret_CreateFromDiscriminatorValue_WithNoMatchingFields_ReturnsEmptyInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetChildNode("AccessToken").Returns((IParseNode?)null);
        parseNode.GetChildNode("APIKey").Returns((IParseNode?)null);

        var result = SetSecretRequest.SetSecretRequest_Secret.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.Null(result.SacloudAPISecret);
        Assert.Null(result.SimpleMQSecret);
    }

    /// <summary>
    /// SetSecretRequest_Secret.CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void SetSecretRequest_Secret_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() =>
            SetSecretRequest.SetSecretRequest_Secret.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// SetSecretRequest_Secret.GetFieldDeserializersがSacloudAPISecret設定時に正しいデシリアライザーを返すことを確認
    /// </summary>
    [Fact]
    public void SetSecretRequest_Secret_GetFieldDeserializers_WithSacloudAPISecret_ReturnsCorrectDeserializers()
    {
        var secret = new SetSecretRequest.SetSecretRequest_Secret
        {
            SacloudAPISecret = new SacloudAPISecret()
        };

        var deserializers = secret.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.NotEmpty(deserializers);
        Assert.True(deserializers.ContainsKey("AccessToken"));
        Assert.True(deserializers.ContainsKey("AccessTokenSecret"));
    }

    /// <summary>
    /// SetSecretRequest_Secret.GetFieldDeserializersがSimpleMQSecret設定時に正しいデシリアライザーを返すことを確認
    /// </summary>
    [Fact]
    public void SetSecretRequest_Secret_GetFieldDeserializers_WithSimpleMQSecret_ReturnsCorrectDeserializers()
    {
        var secret = new SetSecretRequest.SetSecretRequest_Secret
        {
            SimpleMQSecret = new SimpleMQSecret()
        };

        var deserializers = secret.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.NotEmpty(deserializers);
        Assert.True(deserializers.ContainsKey("APIKey"));
    }

    /// <summary>
    /// SetSecretRequest_Secret.GetFieldDeserializersが空の場合に空のディクショナリを返すことを確認
    /// </summary>
    [Fact]
    public void SetSecretRequest_Secret_GetFieldDeserializers_WithNoSecret_ReturnsEmptyDictionary()
    {
        var secret = new SetSecretRequest.SetSecretRequest_Secret();

        var deserializers = secret.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Empty(deserializers);
    }

    /// <summary>
    /// SetSecretRequest_Secret.SerializeがSacloudAPISecret設定時に正しくシリアライズすることを確認
    /// </summary>
    [Fact]
    public void SetSecretRequest_Secret_Serialize_WithSacloudAPISecret_SerializesCorrectly()
    {
        var secret = new SetSecretRequest.SetSecretRequest_Secret
        {
            SacloudAPISecret = new SacloudAPISecret
            {
                AccessToken = "test-token",
                AccessTokenSecret = "test-secret"
            }
        };
        var writer = Substitute.For<ISerializationWriter>();

        secret.Serialize(writer);

        writer.Received(1).WriteObjectValue<SacloudAPISecret>(null, Arg.Is<SacloudAPISecret>(s => s.AccessToken == "test-token"));
    }

    /// <summary>
    /// SetSecretRequest_Secret.SerializeがSimpleMQSecret設定時に正しくシリアライズすることを確認
    /// </summary>
    [Fact]
    public void SetSecretRequest_Secret_Serialize_WithSimpleMQSecret_SerializesCorrectly()
    {
        var secret = new SetSecretRequest.SetSecretRequest_Secret
        {
            SimpleMQSecret = new SimpleMQSecret
            {
                APIKey = "test-api-key"
            }
        };
        var writer = Substitute.For<ISerializationWriter>();

        secret.Serialize(writer);

        writer.Received(1).WriteObjectValue<SimpleMQSecret>(null, Arg.Is<SimpleMQSecret>(s => s.APIKey == "test-api-key"));
    }

    /// <summary>
    /// SetSecretRequest_Secret.Serializeがnullのライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void SetSecretRequest_Secret_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var secret = new SetSecretRequest.SetSecretRequest_Secret
        {
            SacloudAPISecret = new SacloudAPISecret()
        };

        Assert.Throws<ArgumentNullException>(() => secret.Serialize(null!));
    }
}
