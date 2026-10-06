using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// WrappedMetricsStorage_iconモデルのテスト
/// </summary>
public class WrappedMetricsStorage_iconTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WrappedMetricsStorage_icon_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<WrappedMetricsStorage_icon>(WrappedMetricsStorage_icon.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));

    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void WrappedMetricsStorage_icon_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new WrappedMetricsStorage_icon().Serialize(null!));

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void WrappedMetricsStorage_icon_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new WrappedMetricsStorage_icon();
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
    public void WrappedMetricsStorage_icon_Serialize_WritesExpectedFields()
    {
        var model = new WrappedMetricsStorage_icon { Id = "test_id" };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteStringValue("id", "test_id");
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
