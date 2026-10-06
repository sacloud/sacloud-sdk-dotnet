using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// PatchedLogStorageAccessKeyRequestモデルのテスト
/// </summary>
public class PatchedLogStorageAccessKeyRequestTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void PatchedLogStorageAccessKeyRequest_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<PatchedLogStorageAccessKeyRequest>(PatchedLogStorageAccessKeyRequest.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));

    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void PatchedLogStorageAccessKeyRequest_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new PatchedLogStorageAccessKeyRequest().Serialize(null!));

    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void PatchedLogStorageAccessKeyRequest_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new PatchedLogStorageAccessKeyRequest().GetFieldDeserializers();
        Assert.Single(d);
        Assert.True(d.ContainsKey("description"));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void PatchedLogStorageAccessKeyRequest_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new PatchedLogStorageAccessKeyRequest();
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
    public void PatchedLogStorageAccessKeyRequest_Serialize_WritesExpectedFields()
    {
        var model = new PatchedLogStorageAccessKeyRequest { Description = "test_description" };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteStringValue("description", "test_description");
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
