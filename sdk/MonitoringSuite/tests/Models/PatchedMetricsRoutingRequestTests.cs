using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// PatchedMetricsRoutingRequestモデルのテスト
/// </summary>
public class PatchedMetricsRoutingRequestTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void PatchedMetricsRoutingRequest_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<PatchedMetricsRoutingRequest>(PatchedMetricsRoutingRequest.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));

    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void PatchedMetricsRoutingRequest_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new PatchedMetricsRoutingRequest().Serialize(null!));

    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void PatchedMetricsRoutingRequest_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new PatchedMetricsRoutingRequest().GetFieldDeserializers();
        Assert.Equal(4, d.Count);
        Assert.True(d.ContainsKey("metrics_storage_id"));
        Assert.True(d.ContainsKey("publisher_code"));
        Assert.True(d.ContainsKey("resource_id"));
        Assert.True(d.ContainsKey("variant"));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void PatchedMetricsRoutingRequest_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new PatchedMetricsRoutingRequest();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetLongValue().Returns(1L);
        deserializers["metrics_storage_id"](parseNode);
        Assert.Equal(1L, model.MetricsStorageId);

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
    public void PatchedMetricsRoutingRequest_Serialize_WritesExpectedFields()
    {
        var model = new PatchedMetricsRoutingRequest { MetricsStorageId = 1L, PublisherCode = "test_publisher_code", ResourceId = 1L, Variant = "test_variant" };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteLongValue("metrics_storage_id", 1L);
        writer.Received(1).WriteStringValue("publisher_code", "test_publisher_code");
        writer.Received(1).WriteLongValue("resource_id", 1L);
        writer.Received(1).WriteStringValue("variant", "test_variant");
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }

    /// <summary>
    /// すべてのプロパティがnullでも検証が成功することを確認（PATCHセマンティクス）
    /// </summary>
    [Fact]
    public void PatchedMetricsRoutingRequest_Validate_AllNullProperties_PassesValidation()
    {
        var model = new PatchedMetricsRoutingRequest();
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);
        Assert.True(isValid);
        Assert.Empty(results);
    }

    /// <summary>
    /// PublisherCodeが1文字以上の場合に検証が成功することを確認
    /// </summary>
    [Fact]
    public void PatchedMetricsRoutingRequest_Validate_PublisherCodeWithMinLength_PassesValidation()
    {
        var model = new PatchedMetricsRoutingRequest { PublisherCode = "a" };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);
        Assert.True(isValid);
        Assert.Empty(results);
    }

    /// <summary>
    /// PublisherCodeが空文字の場合に検証が失敗することを確認
    /// </summary>
    [Fact]
    public void PatchedMetricsRoutingRequest_Validate_PublisherCodeEmpty_FailsValidation()
    {
        var model = new PatchedMetricsRoutingRequest { PublisherCode = "" };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);
        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(PatchedMetricsRoutingRequest.PublisherCode)));
    }

    /// <summary>
    /// Variantが1文字以上の場合に検証が成功することを確認
    /// </summary>
    [Fact]
    public void PatchedMetricsRoutingRequest_Validate_VariantWithMinLength_PassesValidation()
    {
        var model = new PatchedMetricsRoutingRequest { Variant = "a" };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);
        Assert.True(isValid);
        Assert.Empty(results);
    }

    /// <summary>
    /// Variantが空文字の場合に検証が失敗することを確認
    /// </summary>
    [Fact]
    public void PatchedMetricsRoutingRequest_Validate_VariantEmpty_FailsValidation()
    {
        var model = new PatchedMetricsRoutingRequest { Variant = "" };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);
        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(PatchedMetricsRoutingRequest.Variant)));
    }
}
