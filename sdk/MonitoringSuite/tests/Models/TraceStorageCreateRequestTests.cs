using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// TraceStorageCreateRequestモデルのテスト
/// </summary>
public class TraceStorageCreateRequestTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void TraceStorageCreateRequest_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = TraceStorageCreateRequest.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<TraceStorageCreateRequest>(result);
    }

    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void TraceStorageCreateRequest_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new TraceStorageCreateRequest().Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void TraceStorageCreateRequest_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new TraceStorageCreateRequest();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        deserializers["classification"](parseNode);

        parseNode.GetStringValue().Returns("test_description");
        deserializers["description"](parseNode);
        Assert.Equal("test_description", model.Description);

        parseNode.GetLongValue().Returns(1L);
        deserializers["kms_key_id"](parseNode);
        Assert.Equal(1L, model.KmsKeyId);

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
    public void TraceStorageCreateRequest_Serialize_WritesExpectedFields()
    {
        var model = new TraceStorageCreateRequest { Description = "test_description", KmsKeyId = 1L, Name = "test_name", ServicePrincipalId = 1L };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteStringValue("description", "test_description");
        writer.Received(1).WriteLongValue("kms_key_id", 1L);
        writer.Received(1).WriteStringValue("name", "test_name");
        writer.Received(1).WriteLongValue("service_principal_id", 1L);
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
