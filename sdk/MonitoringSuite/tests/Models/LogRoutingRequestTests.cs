using System.ComponentModel.DataAnnotations;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;
using Sacloud.Sdk.Shared.Extensions;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// LogRoutingRequestモデルのテスト
/// </summary>
public class LogRoutingRequestTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void LogRoutingRequest_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<LogRoutingRequest>(LogRoutingRequest.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));
    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void LogRoutingRequest_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new LogRoutingRequest().Serialize(null!));
    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void LogRoutingRequest_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new LogRoutingRequest().GetFieldDeserializers();
        Assert.Equal(4, d.Count);
        Assert.True(d.ContainsKey("log_storage_id"));
        Assert.True(d.ContainsKey("publisher_code"));
        Assert.True(d.ContainsKey("resource_id"));
        Assert.True(d.ContainsKey("variant"));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void LogRoutingRequest_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new LogRoutingRequest();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetLongValue().Returns(1L);
        deserializers["log_storage_id"](parseNode);
        Assert.Equal(1L, model.LogStorageId);

        parseNode.GetStringValue().Returns("test_publisher_code");
        deserializers["publisher_code"](parseNode);
        Assert.Equal("test_publisher_code", model.PublisherCode);

        parseNode.GetLongValue().Returns(1L);
        deserializers["resource_id"](parseNode);
        Assert.Equal(1L, model.ResourceId);

        parseNode.GetStringValue().Returns("test_variant");
        deserializers["variant"](parseNode);
        Assert.Equal("test_variant", model.Variant);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void LogRoutingRequest_Serialize_WritesExpectedFields()
    {
        var model = new LogRoutingRequest { LogStorageId = 1L, PublisherCode = "test_publisher_code", ResourceId = 1L, Variant = "test_variant" };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteNullableLongValue("log_storage_id", 1L);
        writer.Received(1).WriteStringValue("publisher_code", "test_publisher_code");
        writer.Received(1).WriteLongValue("resource_id", 1L);
        writer.Received(1).WriteStringValue("variant", "test_variant");
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }

    /// <summary>
    /// LogStorageIdがnullの場合にSerializeがnull値のフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void LogRoutingRequest_Serialize_WithNullLogStorageId_WritesNullValue()
    {
        var model = new LogRoutingRequest { LogStorageId = null };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteNullableLongValue("log_storage_id", null);
    }

    /// <summary>
    /// LogStorageIdが未定義の場合にSerializeがnull値のフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void LogRoutingRequest_Serialize_WithUndefinedLogStorageId_WritesNullValue()
    {
        var model = new LogRoutingRequest();
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteNullableLongValue("log_storage_id", null);
    }

    /// <summary>
    /// PublisherCodeがnullの場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void LogRoutingRequest_Validation_PublisherCodeIsNull_ReturnsValidationError()
    {
        var model = new LogRoutingRequest { LogStorageId = 1L, PublisherCode = null, Variant = "logs" };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(LogRoutingRequest.PublisherCode)));
    }

    /// <summary>
    /// PublisherCodeが空文字の場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void LogRoutingRequest_Validation_PublisherCodeIsEmpty_ReturnsValidationError()
    {
        var model = new LogRoutingRequest { LogStorageId = 1L, PublisherCode = "", Variant = "logs" };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(LogRoutingRequest.PublisherCode)));
    }

    /// <summary>
    /// Variantがnullの場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void LogRoutingRequest_Validation_VariantIsNull_ReturnsValidationError()
    {
        var model = new LogRoutingRequest { LogStorageId = 1L, PublisherCode = "code", Variant = null };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(LogRoutingRequest.Variant)));
    }

    /// <summary>
    /// Variantが空文字の場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void LogRoutingRequest_Validation_VariantIsEmpty_ReturnsValidationError()
    {
        var model = new LogRoutingRequest { LogStorageId = 1L, PublisherCode = "code", Variant = "" };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(LogRoutingRequest.Variant)));
    }

    /// <summary>
    /// すべてのフィールドが有効な場合にバリデーションが成功することを確認
    /// </summary>
    [Fact]
    public void LogRoutingRequest_Validation_ValidModel_PassesValidation()
    {
        var model = new LogRoutingRequest { LogStorageId = 1L, PublisherCode = "sakura-cloud", Variant = "logs" };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.True(isValid);
        Assert.Empty(results);
    }
}
