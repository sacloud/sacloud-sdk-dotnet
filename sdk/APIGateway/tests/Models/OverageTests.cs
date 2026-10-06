using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// Overageのテスト
/// </summary>
public class OverageTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = Overage.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<Overage>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Overage.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"unitPrice", "unitRequests"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new Overage();

        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("unitPrice"));
        Assert.True(deserializers.ContainsKey("unitRequests"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new Overage();

        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new Overage();
        var deserializers = model.GetFieldDeserializers();

        var parseNodeUnitPrice = Substitute.For<IParseNode>();
        parseNodeUnitPrice.GetStringValue().Returns("0.01");
        deserializers["unitPrice"](parseNodeUnitPrice);

        var parseNodeUnitRequests = Substitute.For<IParseNode>();
        parseNodeUnitRequests.GetIntValue().Returns(1000);
        deserializers["unitRequests"](parseNodeUnitRequests);

        Assert.Equal("0.01", model.UnitPrice);
        Assert.Equal(1000, model.UnitRequests);
    }

    /// <summary>
    /// Serializeが正しく値を書き込むことを検証
    /// </summary>
    [Fact]
    public void Serialize_WritesValues()
    {
        var model = new Overage
        {
            UnitPrice = "0.01",
            UnitRequests = 1000,
        };
        var writer = Substitute.For<ISerializationWriter>();

        model.Serialize(writer);

        writer.Received(1).WriteStringValue("unitPrice", "0.01");
        writer.Received(1).WriteIntValue("unitRequests", 1000);
        writer.Received(1).WriteAdditionalData(Arg.Any<IDictionary<string, object>>());
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task Overage_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new Overage
        {
            UnitPrice = "0.01",
            UnitRequests = 1000,
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Overage.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal("0.01", deserializedModel.UnitPrice);
        Assert.Equal(1000, deserializedModel.UnitRequests);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task Overage_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new Overage
        {
            UnitPrice = "0.01",
            UnitRequests = 1000,
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
        var deserializedModel = parseNode.GetObjectValue(Overage.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
