using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using Sacloud.Sdk.KMS.Models;
using System.Text.Json;

namespace Sacloud.Sdk.KMS.Tests.Models;

/// <summary>
/// WrappedEncryptionRequestモデルのテスト
/// </summary>
public class WrappedEncryptionRequestTests
{
    [Fact]
    public async Task CanSerializeAndDeserializeEncryptionProperties()
    {
        var originalModel = new WrappedEncryptionRequest
        {
            Key = new EncryptionRequest
            {
                Plain = "encoded-plain",
                Algo = EncryptionRequest_Algo.Aes256Cbc
            }
        };
        using var stream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        await writer.GetSerializedContent().CopyToAsync(stream);

        stream.Position = 0;
        using var jsonDocument = await JsonDocument.ParseAsync(stream);
        var root = jsonDocument.RootElement;
        Assert.Equal(new[] { "Key" }, root.EnumerateObject().Select(property => property.Name));
        var key = root.GetProperty("Key");
        Assert.Equal(new[] { "Algo", "Plain" }, key.EnumerateObject().Select(property => property.Name).OrderBy(name => name));
        Assert.Equal("aes-256-cbc", key.GetProperty("Algo").GetString());
        Assert.Equal("encoded-plain", key.GetProperty("Plain").GetString());

        stream.Position = 0;
        var parseNode = await new JsonParseNodeFactory().GetRootParseNodeAsync("application/json", stream);
        var result = parseNode.GetObjectValue(WrappedEncryptionRequest.CreateFromDiscriminatorValue);

        Assert.NotNull(result);
        Assert.NotNull(result.Key);
        Assert.Equal(originalModel.Key.Plain, result.Key.Plain);
        Assert.Equal(originalModel.Key.Algo, result.Key.Algo);
    }
}
