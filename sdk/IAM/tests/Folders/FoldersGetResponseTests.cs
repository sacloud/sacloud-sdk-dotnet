using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.IAM.Folders;
using Sacloud.Sdk.IAM.Models;

namespace Sacloud.Sdk.IAM.Tests.Folders;

/// <summary>
/// FoldersGetResponseのテスト
/// </summary>
public class FoldersGetResponseTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = FoldersGetResponse.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<FoldersGetResponse>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => FoldersGetResponse.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var response = new FoldersGetResponse();

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
        var response = new FoldersGetResponse();

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
        var response = new FoldersGetResponse();

        Assert.Throws<ArgumentNullException>(() => response.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのitemsアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ItemsAction_WorksCorrectly()
    {
        var response = new FoldersGetResponse();
        var deserializers = response.GetFieldDeserializers();

        var parseNode = Substitute.For<IParseNode>();
        var folders = new List<Folder>();
        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<Folder>>())
            .Returns(folders);

        deserializers["items"](parseNode);

        Assert.NotNull(response.Items);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAllProperties()
    {
        var original = new FoldersGetResponse
        {
            Items =
            [
                new Folder { Name = "Folder1" },
                new Folder { Name = "Folder2" }
            ],
            Count = 2,
            Next = "http://next-page",
            Previous = "http://previous-page"
        };

        using var stream = new MemoryStream();
        using var writer = new Microsoft.Kiota.Serialization.Json.JsonSerializationWriter();
        writer.WriteObjectValue(string.Empty, original);
        using var content = writer.GetSerializedContent();
        content.CopyTo(stream);

        var json = System.Text.Encoding.UTF8.GetString(stream.ToArray());

        using var jsonStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));
        var parseNode = await new Microsoft.Kiota.Serialization.Json.JsonParseNodeFactory().GetRootParseNodeAsync("application/json", jsonStream);
        var deserialized = parseNode.GetObjectValue(FoldersGetResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(deserialized);
        Assert.Equal(2, deserialized.Count);
        Assert.Equal("http://next-page", deserialized.Next);
        Assert.Equal("http://previous-page", deserialized.Previous);
    }

    /// <summary>
    /// AdditionalDataプロパティが正しく動作することを検証
    /// </summary>
    [Fact]
    public void AdditionalData_CanSetAndGet()
    {
        var response = new FoldersGetResponse();
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
        var response = new FoldersGetResponse();
        var folders = new List<Folder>();

        response.Items = folders;

        Assert.NotNull(response.Items);
        Assert.Empty(response.Items);
    }

    /// <summary>
    /// 基底クラスPaginationのプロパティが正しく継承されていることを検証
    /// </summary>
    [Fact]
    public void BaseClassProperties_AreInherited()
    {
        var response = new FoldersGetResponse
        {
            Count = 100,
            Next = "http://example.com/next",
            Previous = "http://example.com/prev"
        };

        Assert.Equal(100, response.Count);
        Assert.Equal("http://example.com/next", response.Next);
        Assert.Equal("http://example.com/prev", response.Previous);
    }
}
