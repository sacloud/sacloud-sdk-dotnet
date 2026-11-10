using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using Sacloud.Sdk.KMS.Models;

namespace Sacloud.Sdk.KMS.Tests.Models;

/// <summary>
/// DecryptionRequestモデルのテスト
/// </summary>
public class DecryptionRequestTests
{
    [Fact]
    public async Task CanSerializeAndDeserializeCipher()
    {
        var originalModel = new DecryptionRequest { Cipher = "encoded-cipher" };
        using var stream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        await writer.GetSerializedContent().CopyToAsync(stream);

        stream.Position = 0;
        var parseNode = await new JsonParseNodeFactory().GetRootParseNodeAsync("application/json", stream);
        var result = parseNode.GetObjectValue(DecryptionRequest.CreateFromDiscriminatorValue);

        Assert.NotNull(result);
        Assert.Equal("encoded-cipher", result.Cipher);
    }
}
