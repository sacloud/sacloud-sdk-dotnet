using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Models;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models;

/// <summary>
/// BucketBillingItemDetailモデルのテスト
/// </summary>
public class BucketBillingItemDetailTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void BucketBillingItemDetail_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new BucketBillingItemDetail();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(6, deserializers.Count);
        Assert.True(deserializers.ContainsKey("charge"));
        Assert.True(deserializers.ContainsKey("found"));
        Assert.True(deserializers.ContainsKey("from"));
        Assert.True(deserializers.ContainsKey("is_closing"));
        Assert.True(deserializers.ContainsKey("to"));
        Assert.True(deserializers.ContainsKey("usage"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void BucketBillingItemDetail_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new BucketBillingItemDetail();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void BucketBillingItemDetail_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new BucketBillingItemDetail();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("charge"));

        parseNode.GetIntValue().Returns(123);

        deserializers["charge"](parseNode);
        Assert.NotNull(model.Charge);

        Assert.True(deserializers.ContainsKey("found"));

        parseNode.GetBoolValue().Returns(true);

        deserializers["found"](parseNode);
        Assert.NotNull(model.Found);

        Assert.True(deserializers.ContainsKey("from"));

        parseNode.GetDateValue().Returns(new Date(2026, 1, 1));

        deserializers["from"](parseNode);
        Assert.NotNull(model.From);

        Assert.True(deserializers.ContainsKey("is_closing"));

        parseNode.GetBoolValue().Returns(true);

        deserializers["is_closing"](parseNode);
        Assert.NotNull(model.IsClosing);

        Assert.True(deserializers.ContainsKey("to"));

        parseNode.GetDateValue().Returns(new Date(2026, 1, 2));

        deserializers["to"](parseNode);
        Assert.NotNull(model.To);

        Assert.True(deserializers.ContainsKey("usage"));

        parseNode.GetDoubleValue().Returns(1.5d);

        deserializers["usage"](parseNode);
        Assert.NotNull(model.Usage);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task BucketBillingItemDetail_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new BucketBillingItemDetail
        {
            Charge = 1,
            Found = true,
            From = new Date(2026, 1, 1),
            IsClosing = true,
            To = new Date(2026, 1, 2),
            Usage = 1.5d
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(BucketBillingItemDetail.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Charge, deserializedModel.Charge);
        Assert.Equal(originalModel.Found, deserializedModel.Found);
        Assert.Equal(originalModel.From, deserializedModel.From);
        Assert.Equal(originalModel.IsClosing, deserializedModel.IsClosing);
        Assert.Equal(originalModel.To, deserializedModel.To);
        Assert.Equal(originalModel.Usage, deserializedModel.Usage);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task BucketBillingItemDetail_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new BucketBillingItemDetail
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
        var deserializedModel = parseNode.GetObjectValue(BucketBillingItemDetail.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
