using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Models;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models;

/// <summary>
/// BucketBillingItemモデルのテスト
/// </summary>
public class BucketBillingItemTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void BucketBillingItem_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new BucketBillingItem();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(6, deserializers.Count);
        Assert.True(deserializers.ContainsKey("basic"));
        Assert.True(deserializers.ContainsKey("num_objects"));
        Assert.True(deserializers.ContainsKey("num_transactions"));
        Assert.True(deserializers.ContainsKey("storage_usage"));
        Assert.True(deserializers.ContainsKey("transfer_amount"));
        Assert.True(deserializers.ContainsKey("year_month"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void BucketBillingItem_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new BucketBillingItem();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void BucketBillingItem_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new BucketBillingItem();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("basic"));

        parseNode.GetObjectValue(Arg.Any<ParsableFactory<BucketBillingItemDetail>>()).Returns(Substitute.For<BucketBillingItemDetail>());

        deserializers["basic"](parseNode);
        Assert.NotNull(model.Basic);

        Assert.True(deserializers.ContainsKey("num_objects"));

        parseNode.GetObjectValue(Arg.Any<ParsableFactory<BucketBillingItemDetail>>()).Returns(Substitute.For<BucketBillingItemDetail>());

        deserializers["num_objects"](parseNode);
        Assert.NotNull(model.NumObjects);

        Assert.True(deserializers.ContainsKey("num_transactions"));

        parseNode.GetObjectValue(Arg.Any<ParsableFactory<BucketBillingItemDetail>>()).Returns(Substitute.For<BucketBillingItemDetail>());

        deserializers["num_transactions"](parseNode);
        Assert.NotNull(model.NumTransactions);

        Assert.True(deserializers.ContainsKey("storage_usage"));

        parseNode.GetObjectValue(Arg.Any<ParsableFactory<BucketBillingItemDetail>>()).Returns(Substitute.For<BucketBillingItemDetail>());

        deserializers["storage_usage"](parseNode);
        Assert.NotNull(model.StorageUsage);

        Assert.True(deserializers.ContainsKey("transfer_amount"));

        parseNode.GetObjectValue(Arg.Any<ParsableFactory<BucketBillingItemDetail>>()).Returns(Substitute.For<BucketBillingItemDetail>());

        deserializers["transfer_amount"](parseNode);
        Assert.NotNull(model.TransferAmount);

        Assert.True(deserializers.ContainsKey("year_month"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["year_month"](parseNode);
        Assert.NotNull(model.YearMonth);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task BucketBillingItem_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new BucketBillingItem
        {
            Basic = new BucketBillingItemDetail(),
            NumObjects = new BucketBillingItemDetail(),
            NumTransactions = new BucketBillingItemDetail(),
            StorageUsage = new BucketBillingItemDetail(),
            TransferAmount = new BucketBillingItemDetail(),
            YearMonth = "sample"
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(BucketBillingItem.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Basic);
        Assert.NotNull(deserializedModel.NumObjects);
        Assert.NotNull(deserializedModel.NumTransactions);
        Assert.NotNull(deserializedModel.StorageUsage);
        Assert.NotNull(deserializedModel.TransferAmount);
        Assert.Equal(originalModel.YearMonth, deserializedModel.YearMonth);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task BucketBillingItem_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new BucketBillingItem
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
        var deserializedModel = parseNode.GetObjectValue(BucketBillingItem.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
