using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using User_memberModel = Sacloud.Sdk.IAM.Models.User_member;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// User_memberのテスト
/// </summary>
public class User_memberTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = User_memberModel.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<User_memberModel>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Sacloud.Sdk.IAM.Models.User_member.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"code", "id"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var member = new User_memberModel();

        var deserializers = member.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("code"));
        Assert.True(deserializers.ContainsKey("id"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var member = new User_memberModel();

        Assert.Throws<ArgumentNullException>(() => member.Serialize(null));
    }

    /// <summary>
    /// Serializeが正しく値を書き込むことを検証
    /// </summary>
    [Fact]
    public void Serialize_WritesValues()
    {
        var member = new User_memberModel
        {
            Code = "mem00001",
            Id = 123456L
        };
        var writer = Substitute.For<ISerializationWriter>();

        member.Serialize(writer);

        writer.Received(1).WriteStringValue("code", "mem00001");
        writer.Received(1).WriteLongValue("id", 123456L);
        writer.Received(1).WriteAdditionalData(Arg.Any<IDictionary<string, object>>());
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクション（"code", "id"）が正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var member = new User_memberModel();
        var deserializers = member.GetFieldDeserializers();
        
        var parseNodeCode = Substitute.For<IParseNode>();
        parseNodeCode.GetStringValue().Returns("mem00001");
        deserializers["code"](parseNodeCode);
        
        var parseNodeId = Substitute.For<IParseNode>();
        parseNodeId.GetLongValue().Returns(123456L);
        deserializers["id"](parseNodeId);

        Assert.Equal("mem00001", member.Code);
        Assert.Equal(123456L, member.Id);
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task User_member_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new User_memberModel
        {
            Code = "mem00001",
            Id = 123456L
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Sacloud.Sdk.IAM.Models.User_member.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Code, deserializedModel.Code);
        Assert.Equal(originalModel.Id, deserializedModel.Id);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task User_member_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new User_memberModel
        {
            Code = "mem00001"
        };
        originalModel.AdditionalData.Add("customField", "customValue");

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Sacloud.Sdk.IAM.Models.User_member.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Code, deserializedModel.Code);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
