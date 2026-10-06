using System.ComponentModel.DataAnnotations;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// TraceStorageAccessKeyRequestモデルのテスト
/// </summary>
public class TraceStorageAccessKeyRequestTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void TraceStorageAccessKeyRequest_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<TraceStorageAccessKeyRequest>(TraceStorageAccessKeyRequest.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));

    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void TraceStorageAccessKeyRequest_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new TraceStorageAccessKeyRequest().Serialize(null!));

    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void TraceStorageAccessKeyRequest_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new TraceStorageAccessKeyRequest().GetFieldDeserializers();
        Assert.Single(d);
        Assert.True(d.ContainsKey("description"));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void TraceStorageAccessKeyRequest_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new TraceStorageAccessKeyRequest();
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
    public void TraceStorageAccessKeyRequest_Serialize_WritesExpectedFields()
    {
        var model = new TraceStorageAccessKeyRequest { Description = "test_description" };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteStringValue("description", "test_description");
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }

    [Fact]
    public void TraceStorageAccessKeyRequest_Validation_DescriptionExceedsMaxLength_ReturnsValidationError()
    {
        var model = new TraceStorageAccessKeyRequest { Description = new string('a', 257) };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(TraceStorageAccessKeyRequest.Description)));
    }
}
