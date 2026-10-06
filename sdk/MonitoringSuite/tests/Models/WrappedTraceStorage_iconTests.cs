using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// WrappedTraceStorage_iconモデルのテスト
/// </summary>
public class WrappedTraceStorage_iconTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WrappedTraceStorage_icon_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<WrappedTraceStorage_icon>(WrappedTraceStorage_icon.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));

    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void WrappedTraceStorage_icon_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new WrappedTraceStorage_icon().Serialize(null!));

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void WrappedTraceStorage_icon_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new WrappedTraceStorage_icon();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetStringValue().Returns("test_id");
        deserializers["id"](parseNode);
        Assert.Equal("test_id", model.Id);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void WrappedTraceStorage_icon_Serialize_WritesExpectedFields()
    {
        var model = new WrappedTraceStorage_icon { Id = "test_id" };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteStringValue("id", "test_id");
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
