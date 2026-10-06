using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// WrappedMetricsStorage_endpointsモデルのテスト
/// </summary>
public class WrappedMetricsStorage_endpointsTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WrappedMetricsStorage_endpoints_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<WrappedMetricsStorage_endpoints>(WrappedMetricsStorage_endpoints.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));

    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void WrappedMetricsStorage_endpoints_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new WrappedMetricsStorage_endpoints().Serialize(null!));

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void WrappedMetricsStorage_endpoints_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new WrappedMetricsStorage_endpoints();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetStringValue().Returns("test_address");
        deserializers["address"](parseNode);
        Assert.Equal("test_address", model.Address);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void WrappedMetricsStorage_endpoints_Serialize_WritesExpectedFields()
    {
        var model = new WrappedMetricsStorage_endpoints { Address = "test_address" };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteStringValue("address", "test_address");
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
