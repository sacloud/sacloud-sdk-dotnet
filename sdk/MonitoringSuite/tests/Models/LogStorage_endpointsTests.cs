using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// LogStorage_endpointsモデルのテスト
/// </summary>
public class LogStorage_endpointsTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void LogStorage_endpoints_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<LogStorage_endpoints>(LogStorage_endpoints.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));
    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void LogStorage_endpoints_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new LogStorage_endpoints().Serialize(null!));
    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void LogStorage_endpoints_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new LogStorage_endpoints().GetFieldDeserializers();
        Assert.Single(d);
        Assert.True(d.ContainsKey("ingester"));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void LogStorage_endpoints_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new LogStorage_endpoints();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        deserializers["ingester"](parseNode);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void LogStorage_endpoints_Serialize_WritesExpectedFields()
    {
        var model = new LogStorage_endpoints();
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
