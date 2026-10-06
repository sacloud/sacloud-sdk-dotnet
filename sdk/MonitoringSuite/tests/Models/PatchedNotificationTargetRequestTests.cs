using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// PatchedNotificationTargetRequestモデルのテスト
/// </summary>
public class PatchedNotificationTargetRequestTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void PatchedNotificationTargetRequest_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<PatchedNotificationTargetRequest>(PatchedNotificationTargetRequest.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));

    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void PatchedNotificationTargetRequest_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new PatchedNotificationTargetRequest().Serialize(null!));

    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void PatchedNotificationTargetRequest_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new PatchedNotificationTargetRequest().GetFieldDeserializers();
        Assert.Equal(3, d.Count);
        Assert.True(d.ContainsKey("description"));
        Assert.True(d.ContainsKey("service_type"));
        Assert.True(d.ContainsKey("url"));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void PatchedNotificationTargetRequest_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new PatchedNotificationTargetRequest();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetStringValue().Returns("test_description");
        deserializers["description"](parseNode);
        Assert.Equal("test_description", model.Description);

        deserializers["service_type"](parseNode);

        parseNode.GetStringValue().Returns("test_url");
        deserializers["url"](parseNode);
        Assert.Equal("test_url", model.Url);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void PatchedNotificationTargetRequest_Serialize_WritesExpectedFields()
    {
        var model = new PatchedNotificationTargetRequest { Description = "test_description", Url = "test_url" };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteStringValue("description", "test_description");
        writer.Received(1).WriteStringValue("url", "test_url");
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }

    /// <summary>
    /// すべてのプロパティがnullでも検証が成功することを確認（PATCHセマンティクス）
    /// </summary>
    [Fact]
    public void PatchedNotificationTargetRequest_Validate_AllNullProperties_PassesValidation()
    {
        var model = new PatchedNotificationTargetRequest();
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);
        Assert.True(isValid);
        Assert.Empty(results);
    }

    /// <summary>
    /// Descriptionが最大長(100)以内の場合に検証が成功することを確認
    /// </summary>
    [Fact]
    public void PatchedNotificationTargetRequest_Validate_DescriptionAtMaxLength_PassesValidation()
    {
        var model = new PatchedNotificationTargetRequest { Description = new string('a', 100) };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);
        Assert.True(isValid);
        Assert.Empty(results);
    }

    /// <summary>
    /// Descriptionが最大長(100)を超える場合に検証が失敗することを確認
    /// </summary>
    [Fact]
    public void PatchedNotificationTargetRequest_Validate_DescriptionExceedsMaxLength_FailsValidation()
    {
        var model = new PatchedNotificationTargetRequest { Description = new string('a', 101) };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);
        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(PatchedNotificationTargetRequest.Description)));
    }

    /// <summary>
    /// Urlが最大長(1024)以内の場合に検証が成功することを確認
    /// </summary>
    [Fact]
    public void PatchedNotificationTargetRequest_Validate_UrlAtMaxLength_PassesValidation()
    {
        var model = new PatchedNotificationTargetRequest { Url = new string('a', 1024) };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);
        Assert.True(isValid);
        Assert.Empty(results);
    }

    /// <summary>
    /// Urlが最大長(1024)を超える場合に検証が失敗することを確認
    /// </summary>
    [Fact]
    public void PatchedNotificationTargetRequest_Validate_UrlExceedsMaxLength_FailsValidation()
    {
        var model = new PatchedNotificationTargetRequest { Url = new string('a', 1025) };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);
        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(PatchedNotificationTargetRequest.Url)));
    }
}
