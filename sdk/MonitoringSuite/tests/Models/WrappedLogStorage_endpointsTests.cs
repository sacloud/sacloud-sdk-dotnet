using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// WrappedLogStorage_endpointsモデルのテスト
/// </summary>
public class WrappedLogStorage_endpointsTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WrappedLogStorage_endpoints_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<WrappedLogStorage_endpoints>(WrappedLogStorage_endpoints.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));

    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void WrappedLogStorage_endpoints_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new WrappedLogStorage_endpoints().Serialize(null!));

    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void WrappedLogStorage_endpoints_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new WrappedLogStorage_endpoints().GetFieldDeserializers();
        Assert.Single(d);
        Assert.True(d.ContainsKey("ingester"));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void WrappedLogStorage_endpoints_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new WrappedLogStorage_endpoints();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        deserializers["ingester"](parseNode);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void WrappedLogStorage_endpoints_Serialize_WritesExpectedFields()
    {
        var model = new WrappedLogStorage_endpoints();
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
