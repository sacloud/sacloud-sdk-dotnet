using System.ComponentModel.DataAnnotations;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// PatchedLogRoutingRequestモデルのテスト
/// </summary>
public class PatchedLogRoutingRequestTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void PatchedLogRoutingRequest_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<PatchedLogRoutingRequest>(PatchedLogRoutingRequest.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));

    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void PatchedLogRoutingRequest_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new PatchedLogRoutingRequest().Serialize(null!));

    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void PatchedLogRoutingRequest_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new PatchedLogRoutingRequest().GetFieldDeserializers();
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
    public void PatchedLogRoutingRequest_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new PatchedLogRoutingRequest();
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
    public void PatchedLogRoutingRequest_Serialize_WritesExpectedFields()
    {
        var model = new PatchedLogRoutingRequest { LogStorageId = 1L, PublisherCode = "test_publisher_code", ResourceId = 1L, Variant = "test_variant" };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteLongValue("log_storage_id", 1L);
        writer.Received(1).WriteStringValue("publisher_code", "test_publisher_code");
        writer.Received(1).WriteLongValue("resource_id", 1L);
        writer.Received(1).WriteStringValue("variant", "test_variant");
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }

    /// <summary>
    /// PublisherCodeが空文字の場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void PatchedLogRoutingRequest_Validation_PublisherCodeIsEmpty_ReturnsValidationError()
    {
        var model = new PatchedLogRoutingRequest { PublisherCode = "" };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(PatchedLogRoutingRequest.PublisherCode)));
    }

    /// <summary>
    /// Variantが空文字の場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void PatchedLogRoutingRequest_Validation_VariantIsEmpty_ReturnsValidationError()
    {
        var model = new PatchedLogRoutingRequest { Variant = "" };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(PatchedLogRoutingRequest.Variant)));
    }

    /// <summary>
    /// 全フィールドがnullの場合にバリデーションが成功することを確認（PATCH は全フィールド任意）
    /// </summary>
    [Fact]
    public void PatchedLogRoutingRequest_Validation_AllNullModel_PassesValidation()
    {
        var model = new PatchedLogRoutingRequest();
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.True(isValid);
        Assert.Empty(results);
    }

    /// <summary>
    /// 有効な値を設定した場合にバリデーションが成功することを確認
    /// </summary>
    [Fact]
    public void PatchedLogRoutingRequest_Validation_ValidModel_PassesValidation()
    {
        var model = new PatchedLogRoutingRequest { PublisherCode = "sakura-cloud", Variant = "logs" };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.True(isValid);
        Assert.Empty(results);
    }
}
