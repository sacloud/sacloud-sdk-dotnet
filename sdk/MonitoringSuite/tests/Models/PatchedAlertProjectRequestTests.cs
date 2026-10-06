using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// PatchedAlertProjectRequestモデルのテスト
/// </summary>
public class PatchedAlertProjectRequestTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void PatchedAlertProjectRequest_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = PatchedAlertProjectRequest.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<PatchedAlertProjectRequest>(result);
    }

    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void PatchedAlertProjectRequest_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new PatchedAlertProjectRequest().Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void PatchedAlertProjectRequest_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new PatchedAlertProjectRequest();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetStringValue().Returns("test_description");
        deserializers["description"](parseNode);
        Assert.Equal("test_description", model.Description);

        parseNode.GetStringValue().Returns("test_name");
        deserializers["name"](parseNode);
        Assert.Equal("test_name", model.Name);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void PatchedAlertProjectRequest_Serialize_WritesExpectedFields()
    {
        var model = new PatchedAlertProjectRequest { Description = "test_description", Name = "test_name" };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteStringValue("description", "test_description");
        writer.Received(1).WriteStringValue("name", "test_name");
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }

    /// <summary>
    /// すべてのプロパティがnullでも検証が成功することを確認（PATCHセマンティクス）
    /// </summary>
    [Fact]
    public void PatchedAlertProjectRequest_Validate_AllNullProperties_PassesValidation()
    {
        var model = new PatchedAlertProjectRequest();
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);
        Assert.True(isValid);
        Assert.Empty(results);
    }

    /// <summary>
    /// Descriptionが最大長(512)以内の場合に検証が成功することを確認
    /// </summary>
    [Fact]
    public void PatchedAlertProjectRequest_Validate_DescriptionAtMaxLength_PassesValidation()
    {
        var model = new PatchedAlertProjectRequest { Description = new string('a', 512) };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);
        Assert.True(isValid);
        Assert.Empty(results);
    }

    /// <summary>
    /// Descriptionが最大長(512)を超える場合に検証が失敗することを確認
    /// </summary>
    [Fact]
    public void PatchedAlertProjectRequest_Validate_DescriptionExceedsMaxLength_FailsValidation()
    {
        var model = new PatchedAlertProjectRequest { Description = new string('a', 513) };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);
        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(PatchedAlertProjectRequest.Description)));
    }

    /// <summary>
    /// Nameが最大長(64)以内の場合に検証が成功することを確認
    /// </summary>
    [Fact]
    public void PatchedAlertProjectRequest_Validate_NameAtMaxLength_PassesValidation()
    {
        var model = new PatchedAlertProjectRequest { Name = new string('a', 64) };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);
        Assert.True(isValid);
        Assert.Empty(results);
    }

    /// <summary>
    /// Nameが最大長(64)を超える場合に検証が失敗することを確認
    /// </summary>
    [Fact]
    public void PatchedAlertProjectRequest_Validate_NameExceedsMaxLength_FailsValidation()
    {
        var model = new PatchedAlertProjectRequest { Name = new string('a', 65) };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);
        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(PatchedAlertProjectRequest.Name)));
    }
}
