using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.SimpleNotification.Models;
using Sacloud.Sdk.Shared.Validation;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.SimpleNotification.Tests.Models.Root;

/// <summary>
/// PutCommonServiceItemRoutingReorderRequestモデルのテスト
/// </summary>
public class PutCommonServiceItemRoutingReorderRequestTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = PutCommonServiceItemRoutingReorderRequest.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<PutCommonServiceItemRoutingReorderRequest>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => PutCommonServiceItemRoutingReorderRequest.CreateFromDiscriminatorValue(null!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new PutCommonServiceItemRoutingReorderRequest();
        var deserializers = model.GetFieldDeserializers();
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("Orders"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new PutCommonServiceItemRoutingReorderRequest();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// バリデーションでOrders必須を検証できることを確認
    /// </summary>
    [Fact]
    public void ValidateRequestBody_WithMissingOrders_ThrowsValidationException()
    {
        var model = new PutCommonServiceItemRoutingReorderRequest();

        var exception = Assert.Throws<ValidationException>(() => RequestParameterValidator.ValidateRequestBody(model, NullLogger.Instance));

        Assert.Contains("Orders", exception.Message);
    }

    /// <summary>
    /// バリデーションで空配列のOrdersを許容することを確認
    /// </summary>
    [Fact]
    public void ValidateRequestBody_WithEmptyOrders_DoesNotThrow()
    {
        var model = new PutCommonServiceItemRoutingReorderRequest
        {
            Orders = new List<PutCommonServiceItemRoutingReorderRequest_Orders>()
        };

        var exception = Record.Exception(() => RequestParameterValidator.ValidateRequestBody(model, NullLogger.Instance));

        Assert.Null(exception);
    }
}
