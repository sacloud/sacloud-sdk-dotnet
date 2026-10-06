using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using UserTrustedDeviceModel = Sacloud.Sdk.IAM.Models.UserTrustedDevice;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// UserTrustedDeviceのテスト
/// </summary>
public class UserTrustedDeviceTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = UserTrustedDeviceModel.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<UserTrustedDeviceModel>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Sacloud.Sdk.IAM.Models.UserTrustedDevice.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"created_at", "id", "name"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var device = new UserTrustedDeviceModel();

        var deserializers = device.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(3, deserializers.Count);
        Assert.True(deserializers.ContainsKey("created_at"));
        Assert.True(deserializers.ContainsKey("id"));
        Assert.True(deserializers.ContainsKey("name"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var device = new UserTrustedDeviceModel();

        Assert.Throws<ArgumentNullException>(() => device.Serialize(null));
    }

    /// <summary>
    /// Serializeが正しく値を書き込むことを検証
    /// </summary>
    [Fact]
    public void Serialize_WritesValues()
    {
        var testCreatedAt = DateTimeOffset.Parse("2024-01-01T00:00:00Z");
        var device = new UserTrustedDeviceModel
        {
            CreatedAt = testCreatedAt,
            Id = 123456L,
            Name = "Test Device"
        };
        var writer = Substitute.For<ISerializationWriter>();

        device.Serialize(writer);

        writer.Received(1).WriteDateTimeOffsetValue("created_at", testCreatedAt);
        writer.Received(1).WriteLongValue("id", 123456L);
        writer.Received(1).WriteStringValue("name", "Test Device");
        writer.Received(1).WriteAdditionalData(Arg.Any<IDictionary<string, object>>());
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクション（"created_at", "id", "name"）が正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var device = new UserTrustedDeviceModel();
        var deserializers = device.GetFieldDeserializers();
        
        var parseNodeCreatedAt = Substitute.For<IParseNode>();
        parseNodeCreatedAt.GetDateTimeOffsetValue().Returns(DateTimeOffset.Parse("2024-01-01T00:00:00Z"));
        deserializers["created_at"](parseNodeCreatedAt);
        
        var parseNodeId = Substitute.For<IParseNode>();
        parseNodeId.GetLongValue().Returns(123456L);
        deserializers["id"](parseNodeId);
        
        var parseNodeName = Substitute.For<IParseNode>();
        parseNodeName.GetStringValue().Returns("Test Device");
        deserializers["name"](parseNodeName);

        Assert.Equal(DateTimeOffset.Parse("2024-01-01T00:00:00Z"), device.CreatedAt);
        Assert.Equal(123456L, device.Id);
        Assert.Equal("Test Device", device.Name);
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task UserTrustedDevice_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new UserTrustedDeviceModel
        {
            CreatedAt = DateTimeOffset.Parse("2024-01-01T00:00:00Z"),
            Id = 123456L,
            Name = "Test Device"
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Sacloud.Sdk.IAM.Models.UserTrustedDevice.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.CreatedAt, deserializedModel.CreatedAt);
        Assert.Equal(originalModel.Id, deserializedModel.Id);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task UserTrustedDevice_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new UserTrustedDeviceModel
        {
            Name = "Test Device"
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
        var deserializedModel = parseNode.GetObjectValue(Sacloud.Sdk.IAM.Models.UserTrustedDevice.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
