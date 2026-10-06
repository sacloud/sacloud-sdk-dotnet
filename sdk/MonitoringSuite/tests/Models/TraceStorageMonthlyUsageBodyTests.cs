using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// TraceStorageMonthlyUsageBodyモデルのテスト
/// </summary>
public class TraceStorageMonthlyUsageBodyTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void TraceStorageMonthlyUsageBody_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<TraceStorageMonthlyUsageBody>(TraceStorageMonthlyUsageBody.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));

    /// <summary>
    /// nullパースノードでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void TraceStorageMonthlyUsageBody_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => TraceStorageMonthlyUsageBody.CreateFromDiscriminatorValue(null!));

    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void TraceStorageMonthlyUsageBody_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new TraceStorageMonthlyUsageBody().GetFieldDeserializers();
        Assert.Single(d);
        Assert.True(d.ContainsKey("usages"));
    }

    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void TraceStorageMonthlyUsageBody_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new TraceStorageMonthlyUsageBody().Serialize(null!));

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void TraceStorageMonthlyUsageBody_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new TraceStorageMonthlyUsageBody();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        deserializers["usages"](parseNode);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void TraceStorageMonthlyUsageBody_Serialize_WritesExpectedFields()
    {
        var model = new TraceStorageMonthlyUsageBody();
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
