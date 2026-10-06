using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;

namespace Sacloud.Sdk.NetworkingSuite.Tests.Models;

/// <summary>
/// SubnetResponseモデルのテスト
/// </summary>
public class SubnetResponseTests
{

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Sdk.NetworkingSuite.Models.SubnetResponse.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void SubnetResponse_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new Sdk.NetworkingSuite.Models.SubnetResponse();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("Subnet"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void SubnetResponse_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new Sdk.NetworkingSuite.Models.SubnetResponse();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void SubnetResponse_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new Sdk.NetworkingSuite.Models.SubnetResponse();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("Subnet"));

        parseNode.GetObjectValue(Arg.Any<ParsableFactory<Sdk.NetworkingSuite.Models.ReadSubnet>>()).Returns(Substitute.For<Sdk.NetworkingSuite.Models.ReadSubnet>());

        deserializers["Subnet"](parseNode);
        Assert.NotNull(model.Subnet);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task SubnetResponse_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new Sdk.NetworkingSuite.Models.SubnetResponse
        {
            Subnet = new Sdk.NetworkingSuite.Models.ReadSubnet()
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Sdk.NetworkingSuite.Models.SubnetResponse.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Subnet);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task SubnetResponse_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new Sdk.NetworkingSuite.Models.SubnetResponse
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
        var deserializedModel = parseNode.GetObjectValue(Sdk.NetworkingSuite.Models.SubnetResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
