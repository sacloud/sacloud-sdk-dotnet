using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.IAM.ServicePrincipals.Item.UploadKey;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// UploadKeyPostRequestBodyのテスト
/// </summary>
public class UploadKeyPostRequestBodyTests
{
    /// <summary>
    /// パラメータなしコンストラクタでインスタンスが作成されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithoutParameters_CreatesInstance()
    {
        var requestBody = new UploadKeyPostRequestBody();

        Assert.NotNull(requestBody);
    }

    /// <summary>
    /// PublicKeyプロパティがnullで初期化されることを確認
    /// </summary>
    [Fact]
    public void PublicKey_InitializedAsNull()
    {
        var requestBody = new UploadKeyPostRequestBody();

        Assert.Null(requestBody.PublicKey);
    }

    /// <summary>
    /// PublicKeyプロパティが正しく設定されることを確認
    /// </summary>
    [Fact]
    public void PublicKey_SetProperty_WorksCorrectly()
    {
        var requestBody = new UploadKeyPostRequestBody
        {
            PublicKey = "ssh-rsa AAAAB3NzaC1yc2EAAAADAQABAAABAQC..."
        };

        Assert.Equal("ssh-rsa AAAAB3NzaC1yc2EAAAADAQABAAABAQC...", requestBody.PublicKey);
    }

    /// <summary>
    /// AdditionalDataプロパティが正しく初期化されることを確認
    /// </summary>
    [Fact]
    public void AdditionalData_InitializedCorrectly()
    {
        var requestBody = new UploadKeyPostRequestBody();

        Assert.NotNull(requestBody.AdditionalData);
        Assert.Empty(requestBody.AdditionalData);
    }

    /// <summary>
    /// AdditionalDataプロパティが正しく設定されることを確認
    /// </summary>
    [Fact]
    public void AdditionalData_SetProperty_WorksCorrectly()
    {
        var requestBody = new UploadKeyPostRequestBody();
        var additionalData = new Dictionary<string, object>
        {
            { "key1", "value1" },
            { "key2", 123 }
        };

        requestBody.AdditionalData = additionalData;

        Assert.NotNull(requestBody.AdditionalData);
        Assert.Equal(2, requestBody.AdditionalData.Count);
        Assert.Equal("value1", requestBody.AdditionalData["key1"]);
        Assert.Equal(123, requestBody.AdditionalData["key2"]);
    }

    /// <summary>
    /// GetFieldDeserializersが正しいデシリアライザを返すことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectDeserializers()
    {
        var requestBody = new UploadKeyPostRequestBody();

        var deserializers = requestBody.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.True(deserializers.Count > 0);
        Assert.True(deserializers.ContainsKey("public_key"));
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = UploadKeyPostRequestBody.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<UploadKeyPostRequestBody>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            UploadKeyPostRequestBody.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var requestBody = new UploadKeyPostRequestBody();
        var deserializers = requestBody.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetStringValue().Returns("ssh-rsa AAAAB3NzaC1yc2EAAAADAQABAAABAQC...");
        deserializers["public_key"](parseNode);
        Assert.Equal("ssh-rsa AAAAB3NzaC1yc2EAAAADAQABAAABAQC...", requestBody.PublicKey);
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var requestBody = new UploadKeyPostRequestBody();

        Assert.Throws<ArgumentNullException>(() => requestBody.Serialize(null));
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new UploadKeyPostRequestBody
        {
            PublicKey = "ssh-rsa AAAAB3NzaC1yc2EAAAADAQABAAABAQC..."
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(UploadKeyPostRequestBody.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.PublicKey, deserializedModel.PublicKey);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new UploadKeyPostRequestBody
        {
            PublicKey = "ssh-rsa AAAAB3NzaC1yc2EAAAADAQABAAABAQC..."
        };
        originalModel.AdditionalData.Add("custom_field", "custom_value");

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(UploadKeyPostRequestBody.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field"));
    }
}
