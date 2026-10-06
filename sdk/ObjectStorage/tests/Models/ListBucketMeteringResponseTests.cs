using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Models;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models;

/// <summary>
/// ListBucketMeteringResponseモデルのテスト
/// </summary>
public class ListBucketMeteringResponseTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void ListBucketMeteringResponse_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new ListBucketMeteringResponse();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("data"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ListBucketMeteringResponse_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ListBucketMeteringResponse();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void ListBucketMeteringResponse_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new ListBucketMeteringResponse();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("data"));

        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<BucketBillingItem>>()).Returns(new List<BucketBillingItem> { new BucketBillingItem() });

        deserializers["data"](parseNode);
        Assert.NotNull(model.Data);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ListBucketMeteringResponse_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ListBucketMeteringResponse
        {
            Data =
            [
                new BucketBillingItem
                {
                    Basic = new BucketBillingItemDetail
                    {
                        Charge = 100,
                        Found = true,
                        From = new Microsoft.Kiota.Abstractions.Date(2026, 1, 1),
                        IsClosing = true,
                        To = new Microsoft.Kiota.Abstractions.Date(2026, 1, 31),
                        Usage = 1.25,
                    },
                    NumObjects = new BucketBillingItemDetail { Charge = 10, Found = true, Usage = 2.5 },
                    NumTransactions = new BucketBillingItemDetail { Charge = 20, Found = true, Usage = 3.5 },
                    StorageUsage = new BucketBillingItemDetail { Charge = 30, Found = true, Usage = 4.5 },
                    TransferAmount = new BucketBillingItemDetail { Charge = 40, Found = true, Usage = 5.5 },
                    YearMonth = "2026-01",
                },
            ]
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(ListBucketMeteringResponse.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Data);
        Assert.Single(deserializedModel.Data);
        Assert.Equal(originalModel.Data[0].YearMonth, deserializedModel.Data[0].YearMonth);
        Assert.NotNull(deserializedModel.Data[0].Basic);
        Assert.Equal(originalModel.Data[0].Basic.Charge, deserializedModel.Data[0].Basic.Charge);
        Assert.Equal(originalModel.Data[0].Basic.From, deserializedModel.Data[0].Basic.From);
        Assert.Equal(originalModel.Data[0].Basic.Usage, deserializedModel.Data[0].Basic.Usage);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task ListBucketMeteringResponse_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new ListBucketMeteringResponse
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
        var deserializedModel = parseNode.GetObjectValue(ListBucketMeteringResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
