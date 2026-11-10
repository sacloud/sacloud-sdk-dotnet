using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using Sacloud.Sdk.KMS.Models;
using System.Text.Json;

namespace Sacloud.Sdk.KMS.Tests.Models;

/// <summary>
/// WrappedCreateKeyRequestモデルのテスト
/// </summary>
public class WrappedCreateKeyRequestTests
{
    [Fact]
    public async Task CanSerializeAndDeserializeKey()
    {
        var originalModel = new WrappedCreateKeyRequest
        {
            Key = new CreateKeyRequest { Name = "key-name", PlainKey = "plain-key" }
        };
        originalModel.Key.Description = "key-description";
        originalModel.Key.Tags = ["production", "encrypted"];
        using var stream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        await writer.GetSerializedContent().CopyToAsync(stream);

        stream.Position = 0;
        using var jsonDocument = await JsonDocument.ParseAsync(stream);
        var root = jsonDocument.RootElement;
        Assert.Equal(new[] { "Key" }, root.EnumerateObject().Select(property => property.Name));
        var key = root.GetProperty("Key");
        Assert.Equal(new[] { "Description", "Name", "PlainKey", "Tags" }, key.EnumerateObject().Select(property => property.Name).OrderBy(name => name));
        Assert.Equal("key-description", key.GetProperty("Description").GetString());
        Assert.Equal("key-name", key.GetProperty("Name").GetString());
        Assert.Equal("plain-key", key.GetProperty("PlainKey").GetString());
        Assert.Equal(new[] { "production", "encrypted" }, key.GetProperty("Tags").EnumerateArray().Select(tag => tag.GetString()!));

        stream.Position = 0;
        var parseNode = await new JsonParseNodeFactory().GetRootParseNodeAsync("application/json", stream);
        var result = parseNode.GetObjectValue(WrappedCreateKeyRequest.CreateFromDiscriminatorValue);

        Assert.NotNull(result);
        Assert.NotNull(result.Key);
        Assert.Equal(originalModel.Key.Name, result.Key.Name);
        Assert.Equal(originalModel.Key.PlainKey, result.Key.PlainKey);
    }
}
