using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// MetricsStorageDailyUsageBodyモデルのテスト
/// </summary>
public class MetricsStorageDailyUsageBodyTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void MetricsStorageDailyUsageBody_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<MetricsStorageDailyUsageBody>(MetricsStorageDailyUsageBody.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));
    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void MetricsStorageDailyUsageBody_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new MetricsStorageDailyUsageBody().Serialize(null!));
    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void MetricsStorageDailyUsageBody_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new MetricsStorageDailyUsageBody().GetFieldDeserializers();
        Assert.Single(d);
        Assert.True(d.ContainsKey("usages"));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void MetricsStorageDailyUsageBody_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new MetricsStorageDailyUsageBody();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        deserializers["usages"](parseNode);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void MetricsStorageDailyUsageBody_Serialize_WritesExpectedFields()
    {
        var model = new MetricsStorageDailyUsageBody();
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
