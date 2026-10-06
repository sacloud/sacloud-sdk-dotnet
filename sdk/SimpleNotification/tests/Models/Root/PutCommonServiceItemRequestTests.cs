using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sacloud.Sdk.SimpleNotification.Models;
using Sacloud.Sdk.Shared.Validation;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.SimpleNotification.Tests.Models.Root;

/// <summary>
/// PutCommonServiceItemRequestモデルのテスト
/// </summary>
public class PutCommonServiceItemRequestTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = PutCommonServiceItemRequest.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<PutCommonServiceItemRequest>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => PutCommonServiceItemRequest.CreateFromDiscriminatorValue(null!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new PutCommonServiceItemRequest();
        var deserializers = model.GetFieldDeserializers();
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("CommonServiceItem"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new PutCommonServiceItemRequest();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new PutCommonServiceItemRequest();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        var item = new PutCommonServiceItemRequest_CommonServiceItem();
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<PutCommonServiceItemRequest_CommonServiceItem>>()).Returns(item);
        deserializers["CommonServiceItem"](parseNode);
        Assert.Same(item, model.CommonServiceItem);
    }

    /// <summary>
    /// CommonServiceItem必須をバリデーションできることを確認
    /// </summary>
    [Fact]
    public void ValidateRequestBody_WithMissingCommonServiceItem_ThrowsValidationException()
    {
        var model = new PutCommonServiceItemRequest();

        var exception = Assert.Throws<ValidationException>(() => RequestParameterValidator.ValidateRequestBody(model, NullLogger.Instance));

        Assert.Contains("CommonServiceItem", exception.Message);
    }
}
