using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using Sacloud.Sdk.KMS.Models;

namespace Sacloud.Sdk.KMS.Tests.Models;

/// <summary>
/// KeyRequestモデルのテスト
/// </summary>
public class KeyRequestTests
{
    [Fact]
    public async Task CanSerializeAndDeserializeProperties()
    {
        var originalModel = new KeyRequest
        {
            Name = "key-name",
            Description = "key-description",
            Tags = ["production", "encrypted"]
        };
        using var stream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        await writer.GetSerializedContent().CopyToAsync(stream);

        stream.Position = 0;
        var parseNode = await new JsonParseNodeFactory().GetRootParseNodeAsync("application/json", stream);
        var result = parseNode.GetObjectValue(KeyRequest.CreateFromDiscriminatorValue);

        Assert.NotNull(result);
        Assert.Equal(originalModel.Name, result.Name);
        Assert.Equal(originalModel.Description, result.Description);
        Assert.Equal(originalModel.Tags, result.Tags);
    }
}
