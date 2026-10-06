using System.ComponentModel.DataAnnotations;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// PatchedLogStorageRequestモデルのテスト
/// </summary>
public class PatchedLogStorageRequestTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void PatchedLogStorageRequest_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<PatchedLogStorageRequest>(PatchedLogStorageRequest.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));

    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void PatchedLogStorageRequest_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new PatchedLogStorageRequest().Serialize(null!));

    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void PatchedLogStorageRequest_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new PatchedLogStorageRequest().GetFieldDeserializers();
        Assert.Equal(3, d.Count);
        Assert.True(d.ContainsKey("description"));
        Assert.True(d.ContainsKey("name"));
        Assert.True(d.ContainsKey("service_principal_id"));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void PatchedLogStorageRequest_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new PatchedLogStorageRequest();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetStringValue().Returns("test_description");
        deserializers["description"](parseNode);
        Assert.Equal("test_description", model.Description);

        parseNode.GetStringValue().Returns("test_name");
        deserializers["name"](parseNode);
        Assert.Equal("test_name", model.Name);

        parseNode.GetLongValue().Returns(1L);
        deserializers["service_principal_id"](parseNode);
        Assert.Equal(1L, model.ServicePrincipalId);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void PatchedLogStorageRequest_Serialize_WritesExpectedFields()
    {
        var model = new PatchedLogStorageRequest { Description = "test_description", Name = "test_name", ServicePrincipalId = 1L };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteStringValue("description", "test_description");
        writer.Received(1).WriteStringValue("name", "test_name");
        writer.Received(1).WriteLongValue("service_principal_id", 1L);
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }

    /// <summary>
    /// すべてのプロパティがnullでも検証が成功することを確認（PATCHセマンティクス）
    /// </summary>
    [Fact]
    public void PatchedLogStorageRequest_Validate_AllNullProperties_PassesValidation()
    {
        var model = new PatchedLogStorageRequest();
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
    public void PatchedLogStorageRequest_Validate_DescriptionAtMaxLength_PassesValidation()
    {
        var model = new PatchedLogStorageRequest { Description = new string('a', 512) };
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
    public void PatchedLogStorageRequest_Validate_DescriptionExceedsMaxLength_FailsValidation()
    {
        var model = new PatchedLogStorageRequest { Description = new string('a', 513) };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);
        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(PatchedLogStorageRequest.Description)));
    }

    /// <summary>
    /// Nameが最大長(64)以内の場合に検証が成功することを確認
    /// </summary>
    [Fact]
    public void PatchedLogStorageRequest_Validate_NameAtMaxLength_PassesValidation()
    {
        var model = new PatchedLogStorageRequest { Name = new string('a', 64) };
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
    public void PatchedLogStorageRequest_Validate_NameExceedsMaxLength_FailsValidation()
    {
        var model = new PatchedLogStorageRequest { Name = new string('a', 65) };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);
        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(PatchedLogStorageRequest.Name)));
    }
}
