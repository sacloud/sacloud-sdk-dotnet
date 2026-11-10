using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.KMS.Models;
using System.Text.Json;

namespace Sacloud.Sdk.KMS.Tests.Models;

/// <summary>
/// CreateKeyRequestモデルのテスト
/// </summary>
public class CreateKeyRequestTests
{
    /// <summary>
    /// PlainKeyがデシリアライズ対象に含まれることを確認
    /// </summary>
    [Fact]
    public void CreateKeyRequest_GetFieldDeserializers_ContainsPlainKey()
    {
        var model = new CreateKeyRequest();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();
        parseNode.GetStringValue().Returns("plain-key-data");

        Assert.Contains("PlainKey", deserializers.Keys);
        deserializers["PlainKey"](parseNode);
        Assert.Equal("plain-key-data", model.PlainKey);
    }

    /// <summary>
    /// PlainKeyをJSONへシリアライズしてデシリアライズできることを確認
    /// </summary>
    [Fact]
    public async Task CreateKeyRequest_CanSerializeAndDeserializePlainKey()
    {
        var originalModel = new CreateKeyRequest
        {
            Name = "test-key-name",
            PlainKey = "plain-key-data"
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        await content.CopyToAsync(serializeStream);

        serializeStream.Position = 0;
        using var jsonDocument = await JsonDocument.ParseAsync(serializeStream);
        var root = jsonDocument.RootElement;
        Assert.Equal(new[] { "Name", "PlainKey" }, root.EnumerateObject().Select(property => property.Name).OrderBy(name => name));
        Assert.Equal("test-key-name", root.GetProperty("Name").GetString());
        Assert.Equal("plain-key-data", root.GetProperty("PlainKey").GetString());

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(CreateKeyRequest.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.Equal(originalModel.PlainKey, deserializedModel.PlainKey);
    }
}