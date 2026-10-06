using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// TraceStorageDailyUsageBodyモデルのテスト
/// </summary>
public class TraceStorageDailyUsageBodyTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void TraceStorageDailyUsageBody_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<TraceStorageDailyUsageBody>(TraceStorageDailyUsageBody.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));

    /// <summary>
    /// nullパースノードでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void TraceStorageDailyUsageBody_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => TraceStorageDailyUsageBody.CreateFromDiscriminatorValue(null!));

    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void TraceStorageDailyUsageBody_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new TraceStorageDailyUsageBody().GetFieldDeserializers();
        Assert.Single(d);
        Assert.True(d.ContainsKey("usages"));
    }

    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void TraceStorageDailyUsageBody_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new TraceStorageDailyUsageBody().Serialize(null!));

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void TraceStorageDailyUsageBody_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new TraceStorageDailyUsageBody();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        deserializers["usages"](parseNode);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void TraceStorageDailyUsageBody_Serialize_WritesExpectedFields()
    {
        var model = new TraceStorageDailyUsageBody();
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
