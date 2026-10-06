using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.SimpleNotification.Models;
using Sacloud.Sdk.Shared.Validation;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.SimpleNotification.Tests.Models.Root;

/// <summary>
/// PutCommonServiceItemRoutingReorderRequest_Ordersモデルのテスト
/// </summary>
public class PutCommonServiceItemRoutingReorderRequest_OrdersTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = PutCommonServiceItemRoutingReorderRequest_Orders.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<PutCommonServiceItemRoutingReorderRequest_Orders>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => PutCommonServiceItemRoutingReorderRequest_Orders.CreateFromDiscriminatorValue(null!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new PutCommonServiceItemRoutingReorderRequest_Orders();
        var deserializers = model.GetFieldDeserializers();
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("PriorityRank"));
        Assert.True(deserializers.ContainsKey("RoutingID"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new PutCommonServiceItemRoutingReorderRequest_Orders();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new PutCommonServiceItemRoutingReorderRequest_Orders
        {
            PriorityRank = 1,
            RoutingID = "123456789012",
        };

        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        using var stream = new MemoryStream();
        content.CopyTo(stream);
        stream.Position = 0;

        var parseNode = await new JsonParseNodeFactory().GetRootParseNodeAsync("application/json", stream);
        var deserialized = parseNode.GetObjectValue(PutCommonServiceItemRoutingReorderRequest_Orders.CreateFromDiscriminatorValue);

        Assert.NotNull(deserialized);
        Assert.Equal(1, deserialized.PriorityRank);
        Assert.Equal("123456789012", deserialized.RoutingID);
    }

    /// <summary>
    /// バリデーションでPriorityRankの範囲を検証できることを確認
    /// </summary>
    [Fact]
    public void ValidateRequestBody_WithOutOfRangePriorityRank_ThrowsValidationException()
    {
        var model = new PutCommonServiceItemRoutingReorderRequest_Orders
        {
            PriorityRank = 101,
            RoutingID = "123456789012"
        };

        var exception = Assert.Throws<ValidationException>(() => RequestParameterValidator.ValidateRequestBody(model, NullLogger.Instance));

        Assert.Contains("PriorityRank", exception.Message);
    }

    /// <summary>
    /// バリデーションでRoutingIDの正規表現を検証できることを確認
    /// </summary>
    [Fact]
    public void ValidateRequestBody_WithInvalidRoutingId_ThrowsValidationException()
    {
        var model = new PutCommonServiceItemRoutingReorderRequest_Orders
        {
            PriorityRank = 1,
            RoutingID = "routing-123"
        };

        var exception = Assert.Throws<ValidationException>(() => RequestParameterValidator.ValidateRequestBody(model, NullLogger.Instance));

        Assert.Contains("RoutingID", exception.Message);
    }
}
