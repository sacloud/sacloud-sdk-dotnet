using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// NotificationTargetRequestモデルのテスト
/// </summary>
public class NotificationTargetRequestTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void NotificationTargetRequest_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<NotificationTargetRequest>(NotificationTargetRequest.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));
    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void NotificationTargetRequest_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new NotificationTargetRequest().Serialize(null!));
    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void NotificationTargetRequest_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new NotificationTargetRequest().GetFieldDeserializers();
        Assert.Equal(3, d.Count);
        Assert.True(d.ContainsKey("description"));
        Assert.True(d.ContainsKey("service_type"));
        Assert.True(d.ContainsKey("url"));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void NotificationTargetRequest_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new NotificationTargetRequest();
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
    public void NotificationTargetRequest_Serialize_WritesExpectedFields()
    {
        var model = new NotificationTargetRequest { Description = "test_description", Url = "test_url" };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteStringValue("description", "test_description");
        writer.Received(1).WriteStringValue("url", "test_url");
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
