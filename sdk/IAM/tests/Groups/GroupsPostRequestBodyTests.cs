using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.IAM.Groups;

namespace Sacloud.Sdk.IAM.Tests.Groups;

/// <summary>
/// GroupsPostRequestBodyのテスト
/// </summary>
public class GroupsPostRequestBodyTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = GroupsPostRequestBody.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<GroupsPostRequestBody>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => GroupsPostRequestBody.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var body = new GroupsPostRequestBody();

        var deserializers = body.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("description"));
        Assert.True(deserializers.ContainsKey("name"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var body = new GroupsPostRequestBody();

        Assert.Throws<ArgumentNullException>(() => body.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var body = new GroupsPostRequestBody();
        var deserializers = body.GetFieldDeserializers();

        var parseNodeDescription = Substitute.For<IParseNode>();
        parseNodeDescription.GetStringValue().Returns("Test Description");
        deserializers["description"](parseNodeDescription);

        var parseNodeName = Substitute.For<IParseNode>();
        parseNodeName.GetStringValue().Returns("Test Group");
        deserializers["name"](parseNodeName);

        Assert.Equal("Test Description", body.Description);
        Assert.Equal("Test Group", body.Name);
    }

    /// <summary>
    /// AdditionalDataプロパティが正しく動作することを検証
    /// </summary>
    [Fact]
    public void AdditionalData_CanSetAndGet()
    {
        var body = new GroupsPostRequestBody();
        var additionalData = new Dictionary<string, object>
        {
            { "custom_field", "custom_value" }
        };

        body.AdditionalData = additionalData;

        Assert.NotNull(body.AdditionalData);
        Assert.True(body.AdditionalData.ContainsKey("custom_field"));
        Assert.Equal("custom_value", body.AdditionalData["custom_field"]);
    }

    /// <summary>
    /// Descriptionプロパティが正しく設定できることを検証
    /// </summary>
    [Fact]
    public void Description_CanSetAndGet()
    {
        var body = new GroupsPostRequestBody
        {
            Description = "Test Description"
        };

        Assert.Equal("Test Description", body.Description);
    }

    /// <summary>
    /// Nameプロパティが正しく設定できることを検証
    /// </summary>
    [Fact]
    public void Name_CanSetAndGet()
    {
        var body = new GroupsPostRequestBody
        {
            Name = "Test Group"
        };

        Assert.Equal("Test Group", body.Name);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAllProperties()
    {
        var original = new GroupsPostRequestBody
        {
            Name = "Test Group",
            Description = "Test Description"
        };

        using var stream = new MemoryStream();
        using var writer = new Microsoft.Kiota.Serialization.Json.JsonSerializationWriter();
        writer.WriteObjectValue(string.Empty, original);
        using var content = writer.GetSerializedContent();
        content.CopyTo(stream);

        var json = System.Text.Encoding.UTF8.GetString(stream.ToArray());

        using var jsonStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));
        var parseNode = await new Microsoft.Kiota.Serialization.Json.JsonParseNodeFactory().GetRootParseNodeAsync("application/json", jsonStream);
        var deserialized = parseNode.GetObjectValue(GroupsPostRequestBody.CreateFromDiscriminatorValue);

        Assert.NotNull(deserialized);
        Assert.Equal("Test Group", deserialized.Name);
        Assert.Equal("Test Description", deserialized.Description);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAdditionalData()
    {
        var original = new GroupsPostRequestBody
        {
            Name = "Test Group",
            AdditionalData = new Dictionary<string, object>
            {
                { "customField", "customValue" }
            }
        };

        using var stream = new MemoryStream();
        using var writer = new Microsoft.Kiota.Serialization.Json.JsonSerializationWriter();
        writer.WriteObjectValue(string.Empty, original);
        using var content = writer.GetSerializedContent();
        content.CopyTo(stream);

        var json = System.Text.Encoding.UTF8.GetString(stream.ToArray());

        using var jsonStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));
        var parseNode = await new Microsoft.Kiota.Serialization.Json.JsonParseNodeFactory().GetRootParseNodeAsync("application/json", jsonStream);
        var deserialized = parseNode.GetObjectValue(GroupsPostRequestBody.CreateFromDiscriminatorValue);

        Assert.NotNull(deserialized);
        Assert.Equal("Test Group", deserialized.Name);
        Assert.NotNull(deserialized.AdditionalData);
    }
}
