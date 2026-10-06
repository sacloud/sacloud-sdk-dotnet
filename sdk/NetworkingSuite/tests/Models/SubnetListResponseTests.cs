using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;

namespace Sacloud.Sdk.NetworkingSuite.Tests.Models;

/// <summary>
/// SubnetListResponseモデルのテスト
/// </summary>
public class SubnetListResponseTests
{

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Sdk.NetworkingSuite.Models.SubnetListResponse.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void SubnetListResponse_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new Sdk.NetworkingSuite.Models.SubnetListResponse();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(4, deserializers.Count);
        Assert.True(deserializers.ContainsKey("Count"));
        Assert.True(deserializers.ContainsKey("From"));
        Assert.True(deserializers.ContainsKey("Subnets"));
        Assert.True(deserializers.ContainsKey("Total"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void SubnetListResponse_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new Sdk.NetworkingSuite.Models.SubnetListResponse();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void SubnetListResponse_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new Sdk.NetworkingSuite.Models.SubnetListResponse();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("Count"));

        parseNode.GetIntValue().Returns(123);

        deserializers["Count"](parseNode);
        Assert.NotNull(model.Count);

        Assert.True(deserializers.ContainsKey("From"));

        parseNode.GetIntValue().Returns(123);

        deserializers["From"](parseNode);
        Assert.NotNull(model.From);

        Assert.True(deserializers.ContainsKey("Subnets"));

        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<Sdk.NetworkingSuite.Models.ReadSubnet>>()).Returns([]);

        deserializers["Subnets"](parseNode);
        Assert.NotNull(model.Subnets);

        Assert.True(deserializers.ContainsKey("Total"));

        parseNode.GetIntValue().Returns(123);

        deserializers["Total"](parseNode);
        Assert.NotNull(model.Total);
    }

    /// <summary>
    /// Subnetsのデシリアライズ結果がnullの場合にnullが設定されることを確認
    /// </summary>
    [Fact]
    public void SubnetListResponse_GetFieldDeserializers_SubnetsNull_SetsNull()
    {
        var model = new Sdk.NetworkingSuite.Models.SubnetListResponse
        {
            Subnets = []
        };
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<Sdk.NetworkingSuite.Models.ReadSubnet>>())
            .Returns((IEnumerable<Sdk.NetworkingSuite.Models.ReadSubnet>?)null);

        deserializers["Subnets"](parseNode);

        Assert.Null(model.Subnets);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task SubnetListResponse_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new Sdk.NetworkingSuite.Models.SubnetListResponse
        {
            Count = 1,
            From = 1,
            Subnets = [],
            Total = 1
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Sdk.NetworkingSuite.Models.SubnetListResponse.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Count, deserializedModel.Count);
        Assert.Equal(originalModel.From, deserializedModel.From);
        Assert.NotNull(deserializedModel.Subnets);
        Assert.Equal(originalModel.Total, deserializedModel.Total);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task SubnetListResponse_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new Sdk.NetworkingSuite.Models.SubnetListResponse
        {
            AdditionalData = new Dictionary<string, object>
            {
                { "extra_key", "extra-value" },
            },
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, model);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Sdk.NetworkingSuite.Models.SubnetListResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
