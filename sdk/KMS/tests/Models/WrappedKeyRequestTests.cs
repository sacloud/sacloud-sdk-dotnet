using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using Sacloud.Sdk.KMS.Models;
using System.Text.Json;

namespace Sacloud.Sdk.KMS.Tests.Models;

/// <summary>
/// WrappedKeyRequestモデルのテスト
/// </summary>
public class WrappedKeyRequestTests
{
    [Fact]
    public async Task CanSerializeAndDeserializeKeyProperties()
    {
        var originalModel = new WrappedKeyRequest
        {
            Key = new KeyRequest
            {
                Name = "key-name",
                Description = "key-description",
                Tags = ["production", "encrypted"]
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
        Assert.Equal(new[] { "Description", "Name", "Tags" }, key.EnumerateObject().Select(property => property.Name).OrderBy(name => name));
        Assert.Equal("key-name", key.GetProperty("Name").GetString());
        Assert.Equal("key-description", key.GetProperty("Description").GetString());
        Assert.Equal(new[] { "production", "encrypted" }, key.GetProperty("Tags").EnumerateArray().Select(tag => tag.GetString()!));

        stream.Position = 0;
        var parseNode = await new JsonParseNodeFactory().GetRootParseNodeAsync("application/json", stream);
        var result = parseNode.GetObjectValue(WrappedKeyRequest.CreateFromDiscriminatorValue);

        Assert.NotNull(result);
        Assert.NotNull(result.Key);
        Assert.Equal(originalModel.Key.Name, result.Key.Name);
        Assert.Equal(originalModel.Key.Description, result.Key.Description);
        Assert.Equal(originalModel.Key.Tags, result.Key.Tags);
    }
}
