using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.KMS.Models;

namespace Sacloud.Sdk.KMS.Tests.Models;

/// <summary>
/// EncryptionRequestモデルのテスト
/// </summary>
public class EncryptionRequestTests
{
    /// <summary>
    /// AlgoとPlainのデシリアライズ処理を確認
    /// </summary>
    [Fact]
    public void EncryptionRequest_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new EncryptionRequest();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.Equal(2, deserializers.Count);
        Assert.Contains("Algo", deserializers.Keys);
        Assert.Contains("Plain", deserializers.Keys);

        parseNode.GetEnumValue<EncryptionRequest_Algo>().Returns(EncryptionRequest_Algo.Aes256Cbc);
        deserializers["Algo"](parseNode);
        Assert.Equal(EncryptionRequest_Algo.Aes256Cbc, model.Algo);

        parseNode.GetStringValue().Returns("base64-encoded-plain-data");
        deserializers["Plain"](parseNode);
        Assert.Equal("base64-encoded-plain-data", model.Plain);
    }

    /// <summary>
    /// AlgoとPlainをJSONへシリアライズしてデシリアライズできることを確認
    /// </summary>
    [Fact]
    public async Task EncryptionRequest_CanSerializeAndDeserializeAllProperties()
    {
        var originalModel = new EncryptionRequest
        {
            Algo = EncryptionRequest_Algo.Aes256Cbc,
            Plain = "base64-encoded-plain-data"
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        await content.CopyToAsync(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(EncryptionRequest.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Algo, deserializedModel.Algo);
        Assert.Equal(originalModel.Plain, deserializedModel.Plain);
    }
}