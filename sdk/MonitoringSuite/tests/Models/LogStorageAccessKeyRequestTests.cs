using System.ComponentModel.DataAnnotations;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// LogStorageAccessKeyRequestモデルのテスト
/// </summary>
public class LogStorageAccessKeyRequestTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void LogStorageAccessKeyRequest_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<LogStorageAccessKeyRequest>(LogStorageAccessKeyRequest.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));
    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void LogStorageAccessKeyRequest_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new LogStorageAccessKeyRequest().Serialize(null!));
    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void LogStorageAccessKeyRequest_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new LogStorageAccessKeyRequest().GetFieldDeserializers();
        Assert.Single(d);
        Assert.True(d.ContainsKey("description"));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void LogStorageAccessKeyRequest_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new LogStorageAccessKeyRequest();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetStringValue().Returns("test_description");
        deserializers["description"](parseNode);
        Assert.Equal("test_description", model.Description);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void LogStorageAccessKeyRequest_Serialize_WritesExpectedFields()
    {
        var model = new LogStorageAccessKeyRequest { Description = "test_description" };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteStringValue("description", "test_description");
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }

    [Fact]
    public void LogStorageAccessKeyRequest_Validation_DescriptionExceedsMaxLength_ReturnsValidationError()
    {
        var model = new LogStorageAccessKeyRequest { Description = new string('a', 257) };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(LogStorageAccessKeyRequest.Description)));
    }
}
