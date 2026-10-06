using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// LogStorageDailyUsageモデルのテスト
/// </summary>
public class LogStorageDailyUsageTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void LogStorageDailyUsage_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<LogStorageDailyUsage>(LogStorageDailyUsage.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));
    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void LogStorageDailyUsage_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new LogStorageDailyUsage().Serialize(null!));
    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void LogStorageDailyUsage_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new LogStorageDailyUsage().GetFieldDeserializers();
        Assert.Equal(5, d.Count);
        Assert.True(d.ContainsKey("date"));
        Assert.True(d.ContainsKey("ingested_bytes"));
        Assert.True(d.ContainsKey("ingested_rows"));
        Assert.True(d.ContainsKey("stored_bytes"));
        Assert.True(d.ContainsKey("timestamp"));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void LogStorageDailyUsage_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new LogStorageDailyUsage();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        deserializers["date"](parseNode);

        deserializers["ingested_bytes"](parseNode);

        deserializers["ingested_rows"](parseNode);

        deserializers["stored_bytes"](parseNode);

        deserializers["timestamp"](parseNode);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void LogStorageDailyUsage_Serialize_WritesExpectedFields()
    {
        var model = new LogStorageDailyUsage();
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
