using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// WrappedTraceStorage_endpointsモデルのテスト
/// </summary>
public class WrappedTraceStorage_endpointsTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WrappedTraceStorage_endpoints_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<WrappedTraceStorage_endpoints>(WrappedTraceStorage_endpoints.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));

    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void WrappedTraceStorage_endpoints_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new WrappedTraceStorage_endpoints().Serialize(null!));

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void WrappedTraceStorage_endpoints_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new WrappedTraceStorage_endpoints();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        deserializers["ingester"](parseNode);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void WrappedTraceStorage_endpoints_Serialize_WritesExpectedFields()
    {
        var model = new WrappedTraceStorage_endpoints();
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
