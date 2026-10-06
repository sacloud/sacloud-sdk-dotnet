using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// UserDetailのテスト
/// </summary>
public class UserDetailTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = UserDetail.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<UserDetail>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => UserDetail.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（親User7+独自1=8キー）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new UserDetail();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Equal(8, deserializers.Count);
        Assert.True(deserializers.ContainsKey("createdAt"));
        Assert.True(deserializers.ContainsKey("id"));
        Assert.True(deserializers.ContainsKey("updatedAt"));
        Assert.True(deserializers.ContainsKey("customID"));
        Assert.True(deserializers.ContainsKey("groups"));
        Assert.True(deserializers.ContainsKey("name"));
        Assert.True(deserializers.ContainsKey("tags"));
        Assert.True(deserializers.ContainsKey("ipRestrictionConfig"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new UserDetail();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証（独自プロパティのみ）
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new UserDetail();
        var deserializers = model.GetFieldDeserializers();
        var ipRestrictionConfig = new IpRestrictionConfig();

        var ipRestrictionConfigNode = Substitute.For<IParseNode>();
        ipRestrictionConfigNode.GetObjectValue(Arg.Any<ParsableFactory<IpRestrictionConfig>>()).Returns(ipRestrictionConfig);
        deserializers["ipRestrictionConfig"](ipRestrictionConfigNode);

        var nameNode = Substitute.For<IParseNode>();
        nameNode.GetStringValue().Returns("user-detail");
        deserializers["name"](nameNode);

        Assert.Same(ipRestrictionConfig, model.IpRestrictionConfig);
        Assert.Equal("user-detail", model.Name);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティを保持できることを検証
    /// </summary>
    [Fact]
    public async Task UserDetail_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new UserDetail
        {
            CustomID = "custom-user-id",
            IpRestrictionConfig = new IpRestrictionConfig(),
            Name = "user-detail",
            Tags = ["team-a", "dev"],
        };
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(UserDetail.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.CustomID, deserializedModel.CustomID);
        Assert.NotNull(deserializedModel.IpRestrictionConfig);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.NotNull(deserializedModel.Tags);
        Assert.Equal(2, deserializedModel.Tags.Count);
        Assert.Equal("team-a", deserializedModel.Tags[0]);
        Assert.Equal("dev", deserializedModel.Tags[1]);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティとAdditionalDataを保持できることを検証
    /// </summary>
    [Fact]
    public async Task UserDetail_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new UserDetail
        {
            CustomID = "custom-user-id",
            IpRestrictionConfig = new IpRestrictionConfig(),
            Name = "user-detail",
            Tags = ["team-a", "dev"],
        };
        originalModel.AdditionalData["customField"] = "customValue";
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(UserDetail.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.CustomID, deserializedModel.CustomID);
        Assert.NotNull(deserializedModel.IpRestrictionConfig);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.NotNull(deserializedModel.Tags);
        Assert.Equal(2, deserializedModel.Tags.Count);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
