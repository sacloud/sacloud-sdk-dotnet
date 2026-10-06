using System.ComponentModel.DataAnnotations;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// PatchedMetricsStorageRequestモデルのテスト
/// </summary>
public class PatchedMetricsStorageRequestTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void PatchedMetricsStorageRequest_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<PatchedMetricsStorageRequest>(PatchedMetricsStorageRequest.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));

    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void PatchedMetricsStorageRequest_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new PatchedMetricsStorageRequest().Serialize(null!));

    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void PatchedMetricsStorageRequest_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new PatchedMetricsStorageRequest().GetFieldDeserializers();
        Assert.Equal(2, d.Count);
        Assert.True(d.ContainsKey("description"));
        Assert.True(d.ContainsKey("name"));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void PatchedMetricsStorageRequest_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new PatchedMetricsStorageRequest();
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
    public void PatchedMetricsStorageRequest_Serialize_WritesExpectedFields()
    {
        var model = new PatchedMetricsStorageRequest { Description = "test_description", Name = "test_name" };
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
    public void PatchedMetricsStorageRequest_Validate_AllNullProperties_PassesValidation()
    {
        var model = new PatchedMetricsStorageRequest();
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
    public void PatchedMetricsStorageRequest_Validate_DescriptionAtMaxLength_PassesValidation()
    {
        var model = new PatchedMetricsStorageRequest { Description = new string('a', 512) };
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
    public void PatchedMetricsStorageRequest_Validate_DescriptionExceedsMaxLength_FailsValidation()
    {
        var model = new PatchedMetricsStorageRequest { Description = new string('a', 513) };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);
        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(PatchedMetricsStorageRequest.Description)));
    }

    /// <summary>
    /// Nameが最大長(64)以内の場合に検証が成功することを確認
    /// </summary>
    [Fact]
    public void PatchedMetricsStorageRequest_Validate_NameAtMaxLength_PassesValidation()
    {
        var model = new PatchedMetricsStorageRequest { Name = new string('a', 64) };
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
    public void PatchedMetricsStorageRequest_Validate_NameExceedsMaxLength_FailsValidation()
    {
        var model = new PatchedMetricsStorageRequest { Name = new string('a', 65) };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);
        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(PatchedMetricsStorageRequest.Name)));
    }
}
