using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// MetricsStorageMonthlyUsageモデルのテスト
/// </summary>
public class MetricsStorageMonthlyUsageTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void MetricsStorageMonthlyUsage_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<MetricsStorageMonthlyUsage>(MetricsStorageMonthlyUsage.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));
    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void MetricsStorageMonthlyUsage_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new MetricsStorageMonthlyUsage().Serialize(null!));
    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void MetricsStorageMonthlyUsage_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new MetricsStorageMonthlyUsage().GetFieldDeserializers();
        Assert.Equal(4, d.Count);
        Assert.True(d.ContainsKey("ingested_rows"));
        Assert.True(d.ContainsKey("month"));
        Assert.True(d.ContainsKey("timestamp"));
        Assert.True(d.ContainsKey("year"));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void MetricsStorageMonthlyUsage_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new MetricsStorageMonthlyUsage();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        deserializers["ingested_rows"](parseNode);

        deserializers["month"](parseNode);

        deserializers["timestamp"](parseNode);

        deserializers["year"](parseNode);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void MetricsStorageMonthlyUsage_Serialize_WritesExpectedFields()
    {
        var model = new MetricsStorageMonthlyUsage();
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
