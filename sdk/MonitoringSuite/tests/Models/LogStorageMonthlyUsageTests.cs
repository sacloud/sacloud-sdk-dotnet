using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// LogStorageMonthlyUsageモデルのテスト
/// </summary>
public class LogStorageMonthlyUsageTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void LogStorageMonthlyUsage_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<LogStorageMonthlyUsage>(LogStorageMonthlyUsage.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));
    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void LogStorageMonthlyUsage_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new LogStorageMonthlyUsage().Serialize(null!));
    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void LogStorageMonthlyUsage_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new LogStorageMonthlyUsage().GetFieldDeserializers();
        Assert.Equal(6, d.Count);
        Assert.True(d.ContainsKey("ingested_bytes"));
        Assert.True(d.ContainsKey("ingested_rows"));
        Assert.True(d.ContainsKey("max_stored_bytes"));
        Assert.True(d.ContainsKey("month"));
        Assert.True(d.ContainsKey("timestamp"));
        Assert.True(d.ContainsKey("year"));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void LogStorageMonthlyUsage_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new LogStorageMonthlyUsage();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        deserializers["ingested_bytes"](parseNode);

        deserializers["ingested_rows"](parseNode);

        deserializers["max_stored_bytes"](parseNode);

        deserializers["month"](parseNode);

        deserializers["timestamp"](parseNode);

        deserializers["year"](parseNode);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void LogStorageMonthlyUsage_Serialize_WritesExpectedFields()
    {
        var model = new LogStorageMonthlyUsage();
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
