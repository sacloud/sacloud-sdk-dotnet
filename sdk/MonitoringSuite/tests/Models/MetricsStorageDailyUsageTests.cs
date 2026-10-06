using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// MetricsStorageDailyUsageモデルのテスト
/// </summary>
public class MetricsStorageDailyUsageTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void MetricsStorageDailyUsage_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<MetricsStorageDailyUsage>(MetricsStorageDailyUsage.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));
    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void MetricsStorageDailyUsage_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new MetricsStorageDailyUsage().Serialize(null!));
    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void MetricsStorageDailyUsage_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new MetricsStorageDailyUsage().GetFieldDeserializers();
        Assert.Equal(3, d.Count);
        Assert.True(d.ContainsKey("date"));
        Assert.True(d.ContainsKey("ingested_rows"));
        Assert.True(d.ContainsKey("timestamp"));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void MetricsStorageDailyUsage_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new MetricsStorageDailyUsage();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        deserializers["date"](parseNode);

        deserializers["ingested_rows"](parseNode);

        deserializers["timestamp"](parseNode);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void MetricsStorageDailyUsage_Serialize_WritesExpectedFields()
    {
        var model = new MetricsStorageDailyUsage();
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
