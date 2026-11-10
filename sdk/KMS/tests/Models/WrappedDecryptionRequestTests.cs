using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using Sacloud.Sdk.KMS.Models;
using System.Text.Json;

namespace Sacloud.Sdk.KMS.Tests.Models;

/// <summary>
/// WrappedDecryptionRequestモデルのテスト
/// </summary>
public class WrappedDecryptionRequestTests
{
    [Fact]
    public async Task CanSerializeAndDeserializeCipher()
    {
        var originalModel = new WrappedDecryptionRequest
        {
            Key = new DecryptionRequest { Cipher = "encoded-cipher" }
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
        Assert.Equal(new[] { "Cipher" }, key.EnumerateObject().Select(property => property.Name));
        Assert.Equal("encoded-cipher", key.GetProperty("Cipher").GetString());

        stream.Position = 0;
        var parseNode = await new JsonParseNodeFactory().GetRootParseNodeAsync("application/json", stream);
        var result = parseNode.GetObjectValue(WrappedDecryptionRequest.CreateFromDiscriminatorValue);

        Assert.NotNull(result);
        Assert.NotNull(result.Key);
        Assert.Equal("encoded-cipher", result.Key.Cipher);
    }
}
