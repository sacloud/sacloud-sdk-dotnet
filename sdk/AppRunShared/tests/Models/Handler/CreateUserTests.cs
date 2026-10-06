using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunShared.Models.Handler;

namespace Sacloud.Sdk.AppRunShared.Tests.Models.Handler;

/// <summary>
/// CreateUserモデルのテスト
/// </summary>
public class CreateUserTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void CreateUser_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = CreateUser.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<CreateUser>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateUser_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() =>
            CreateUser.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void CreateUser_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new CreateUser();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("limit"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateUser_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new CreateUser();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void CreateUser_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new CreateUser();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        // Object型フィールド: limit
        var limitModel = new CreateUser_limit { ApplicationCount = 5 };
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<CreateUser_limit>>()).Returns(limitModel);
        deserializers["limit"](parseNode);
        Assert.Equal(limitModel, model.Limit);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CreateUser_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new CreateUser
        {
            Limit = new CreateUser_limit { ApplicationCount = 10 }
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(CreateUser.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Limit);
        Assert.Equal(originalModel.Limit.ApplicationCount, deserializedModel.Limit.ApplicationCount);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CreateUser_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new CreateUser();
        originalModel.AdditionalData.Add("custom_field_1", "custom_value_1");
        originalModel.AdditionalData.Add("custom_field_2", 42);

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(CreateUser.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}
