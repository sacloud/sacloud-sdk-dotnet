using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.SecretManager.Models;

namespace Sacloud.Sdk.SecretManager.Tests.Models;

/// <summary>
/// WrappedDeleteSecretモデルのテスト
/// </summary>
public class WrappedDeleteSecretTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void WrappedDeleteSecret_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = WrappedDeleteSecret.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<WrappedDeleteSecret>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void WrappedDeleteSecret_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() => WrappedDeleteSecret.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void WrappedDeleteSecret_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new WrappedDeleteSecret();

        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("Secret"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void WrappedDeleteSecret_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new WrappedDeleteSecret();

        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void WrappedDeleteSecret_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new WrappedDeleteSecret();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        var secret = new DeleteSecret { Name = "test-secret" };
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<DeleteSecret>>()).Returns(secret);
        deserializers["Secret"](parseNode);
        Assert.NotNull(model.Secret);
        Assert.Equal("test-secret", model.Secret.Name);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task WrappedDeleteSecret_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new WrappedDeleteSecret
        {
            Secret = new DeleteSecret
            {
                Name = "test-secret"
            }
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(WrappedDeleteSecret.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Secret);
        Assert.Equal(originalModel.Secret.Name, deserializedModel.Secret.Name);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task WrappedDeleteSecret_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new WrappedDeleteSecret
        {
            Secret = new DeleteSecret { Name = "test-secret" }
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
        var deserializedModel = parseNode.GetObjectValue(WrappedDeleteSecret.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}
