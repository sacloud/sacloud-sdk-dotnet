using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.IAM.Groups;
using Sacloud.Sdk.IAM.Models;

namespace Sacloud.Sdk.IAM.Tests.Groups;

/// <summary>
/// GroupsGetResponseのテスト
/// </summary>
public class GroupsGetResponseTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = GroupsGetResponse.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<GroupsGetResponse>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => GroupsGetResponse.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var response = new GroupsGetResponse();

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
        var response = new GroupsGetResponse();

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
        var response = new GroupsGetResponse();

        Assert.Throws<ArgumentNullException>(() => response.Serialize(null));
    }

    /// <summary>
    /// Itemsプロパティが正しく設定できることを検証
    /// </summary>
    [Fact]
    public void Items_CanSetAndGet()
    {
        var response = new GroupsGetResponse();
        var groups = new List<Group>
        {
            new Group { Name = "Test Group" }
        };

        response.Items = groups;

        Assert.NotNull(response.Items);
        Assert.Single(response.Items);
        Assert.Equal("Test Group", response.Items[0].Name);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAllProperties()
    {
        var original = new GroupsGetResponse
        {
            Items =
            [
                new Group { Name = "Group1" },
                new Group { Name = "Group2" }
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
        var deserialized = parseNode.GetObjectValue(GroupsGetResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(deserialized);
        Assert.Equal(2, deserialized.Count);
        Assert.Equal("http://next-page", deserialized.Next);
        Assert.Equal("http://previous-page", deserialized.Previous);
    }
}
