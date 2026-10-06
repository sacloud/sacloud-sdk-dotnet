using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.IAM.ServicePrincipals.Item.Keys;
using Sacloud.Sdk.IAM.Models;
using System.Text;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// KeysGetResponseのテスト
/// </summary>
public class KeysGetResponseTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = KeysGetResponse.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<KeysGetResponse>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => KeysGetResponse.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var response = new KeysGetResponse();

        var deserializers = response.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.True(deserializers.ContainsKey("items"));
    }

    /// <summary>
    /// GetFieldDeserializersが基底クラスのフィールドも含むことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_IncludesBaseClassFields()
    {
        var response = new KeysGetResponse();

        var deserializers = response.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.True(deserializers.ContainsKey("count"));
        Assert.True(deserializers.ContainsKey("next"));
        Assert.True(deserializers.ContainsKey("previous"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var response = new KeysGetResponse();

        Assert.Throws<ArgumentNullException>(() => response.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのitemsアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ItemsAction_WorksCorrectly()
    {
        var response = new KeysGetResponse();
        var deserializers = response.GetFieldDeserializers();

        var parseNode = Substitute.For<IParseNode>();
        var keys = new List<ServicePrincipalKey>();
        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<ServicePrincipalKey>>())
            .Returns(keys);

        deserializers["items"](parseNode);

        Assert.NotNull(response.Items);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// AdditionalDataプロパティが正しく動作することを検証
    /// </summary>
    [Fact]
    public void AdditionalData_CanSetAndGet()
    {
        var response = new KeysGetResponse();
        var additionalData = new Dictionary<string, object>
        {
            { "custom_field", "custom_value" }
        };

        response.AdditionalData = additionalData;

        Assert.NotNull(response.AdditionalData);
        Assert.True(response.AdditionalData.ContainsKey("custom_field"));
        Assert.Equal("custom_value", response.AdditionalData["custom_field"]);
    }

    /// <summary>
    /// Itemsプロパティが正しく設定できることを検証
    /// </summary>
    [Fact]
    public void Items_CanSetAndGet()
    {
        var response = new KeysGetResponse();
        var keys = new List<ServicePrincipalKey>();

        response.Items = keys;

        Assert.NotNull(response.Items);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// 基底クラスPaginationのプロパティが正しく継承されていることを検証
    /// </summary>
    [Fact]
    public void BaseClassProperties_AreInherited()
    {
        var response = new KeysGetResponse
        {
            Count = 100,
            Next = "http://example.com/next",
            Previous = "http://example.com/prev"
        };

        Assert.Equal(100, response.Count);
        Assert.Equal("http://example.com/next", response.Next);
        Assert.Equal("http://example.com/prev", response.Previous);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAllProperties()
    {
        var original = new KeysGetResponse
        {
            Items = [],
            Count = 5,
            Next = "http://example.com/next",
            Previous = "http://example.com/prev"
        };

        using var stream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(string.Empty, original);
        using var content = writer.GetSerializedContent();
        content.CopyTo(stream);
        
        var json = Encoding.UTF8.GetString(stream.ToArray());

        using var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        var parseNode = await new JsonParseNodeFactory().GetRootParseNodeAsync("application/json", jsonStream);
        var deserialized = parseNode.GetObjectValue(KeysGetResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(deserialized);
        Assert.NotNull(deserialized.Items);
        Assert.Equal(original.Count, deserialized.Count);
        Assert.Equal(original.Next, deserialized.Next);
        Assert.Equal(original.Previous, deserialized.Previous);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAdditionalData()
    {
        var original = new KeysGetResponse
        {
            Items = [],
            Count = 3,
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
        var deserialized = parseNode.GetObjectValue(KeysGetResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(deserialized);
        Assert.NotNull(deserialized.AdditionalData);
        Assert.Equal(2, deserialized.AdditionalData.Count);
    }
}
