using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// LogStorage_endpoints_ingesterモデルのテスト
/// </summary>
public class LogStorage_endpoints_ingesterTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void LogStorage_endpoints_ingester_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<LogStorage_endpoints_ingester>(LogStorage_endpoints_ingester.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));
    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void LogStorage_endpoints_ingester_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new LogStorage_endpoints_ingester().Serialize(null!));
    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void LogStorage_endpoints_ingester_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new LogStorage_endpoints_ingester().GetFieldDeserializers();
        Assert.Equal(2, d.Count);
        Assert.True(d.ContainsKey("address"));
        Assert.True(d.ContainsKey("insecure"));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void LogStorage_endpoints_ingester_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new LogStorage_endpoints_ingester();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetStringValue().Returns("test_address");
        deserializers["address"](parseNode);
        Assert.Equal("test_address", model.Address);

        parseNode.GetBoolValue().Returns(true);
        deserializers["insecure"](parseNode);
        Assert.Equal(true, model.Insecure);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void LogStorage_endpoints_ingester_Serialize_WritesExpectedFields()
    {
        var model = new LogStorage_endpoints_ingester { Address = "test_address", Insecure = true };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteStringValue("address", "test_address");
        writer.Received(1).WriteBoolValue("insecure", true);
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
