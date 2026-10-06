using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.IAM.Compat.Users.Item.SecurityKeys;
using Sacloud.Sdk.IAM.Models;
using System.Text;

namespace Sacloud.Sdk.IAM.Tests.Compat.Users.Item.SecurityKeys;

/// <summary>
/// SecurityKeysGetResponseモデルの単体テスト
/// </summary>
public class SecurityKeysGetResponseTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = SecurityKeysGetResponse.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<SecurityKeysGetResponse>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            SecurityKeysGetResponse.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new SecurityKeysGetResponse();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.True(deserializers.ContainsKey("items"));
    }

    /// <summary>
    /// Serializeがnullのwriterで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new SecurityKeysGetResponse();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new SecurityKeysGetResponse();
        var parseNode = Substitute.For<IParseNode>();
        
        var testItems = new List<UserSecurityKey> { new UserSecurityKey() };
        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<UserSecurityKey>>())
            .Returns(testItems);

        var deserializers = model.GetFieldDeserializers();
        deserializers["items"](parseNode);
        Assert.NotNull(model.Items);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズが可能なことを確認
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAllProperties()
    {
        var original = new SecurityKeysGetResponse
        {
            Items = [new UserSecurityKey()],
            Count = 1,
            Next = "next-url",
            Previous = "previous-url"
        };
        using var stream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(string.Empty, original);
        using var content = writer.GetSerializedContent();
        content.CopyTo(stream);
        
        var json = Encoding.UTF8.GetString(stream.ToArray());
        using var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        var parseNode = await new JsonParseNodeFactory().GetRootParseNodeAsync("application/json", jsonStream);
        var deserialized = parseNode.GetObjectValue(SecurityKeysGetResponse.CreateFromDiscriminatorValue);
        Assert.NotNull(deserialized);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズが可能なことを確認
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAdditionalData()
    {
        var original = new SecurityKeysGetResponse
        {
            AdditionalData = new Dictionary<string, object>
            {
                { "customField", "customValue" },
                { "numericField", 123 }
            }
        };
        using var stream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(string.Empty, original);
        using var content = writer.GetSerializedContent();
        content.CopyTo(stream);
        
        var json = Encoding.UTF8.GetString(stream.ToArray());
        using var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        var parseNode = await new JsonParseNodeFactory().GetRootParseNodeAsync("application/json", jsonStream);
        var deserialized = parseNode.GetObjectValue(SecurityKeysGetResponse.CreateFromDiscriminatorValue);
        Assert.NotNull(deserialized);
        Assert.NotNull(deserialized.AdditionalData);
        Assert.True(deserialized.AdditionalData.Count >= 1);
    }
}
