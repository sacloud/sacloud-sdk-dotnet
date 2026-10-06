using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// MetricsRoutingモデルのテスト
/// </summary>
public class MetricsRoutingTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void MetricsRouting_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<MetricsRouting>(MetricsRouting.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));
    /// <summary>
    /// nullパースノードでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void MetricsRouting_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => MetricsRouting.CreateFromDiscriminatorValue(null!));
    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void MetricsRouting_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new MetricsRouting().GetFieldDeserializers();
        Assert.Equal(8, d.Count);
        Assert.True(d.ContainsKey("created_at"));
        Assert.True(d.ContainsKey("id"));
        Assert.True(d.ContainsKey("metrics_storage"));
        Assert.True(d.ContainsKey("publisher"));
        Assert.True(d.ContainsKey("resource_id"));
        Assert.True(d.ContainsKey("uid"));
        Assert.True(d.ContainsKey("updated_at"));
        Assert.True(d.ContainsKey("variant"));
    }
    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void MetricsRouting_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new MetricsRouting().Serialize(null!));

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void MetricsRouting_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new MetricsRouting();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        deserializers["created_at"](parseNode);

        deserializers["id"](parseNode);

        deserializers["metrics_storage"](parseNode);

        deserializers["publisher"](parseNode);

        parseNode.GetLongValue().Returns(1L);
        deserializers["resource_id"](parseNode);
        Assert.Equal(1L, model.ResourceId);

        deserializers["uid"](parseNode);

        deserializers["updated_at"](parseNode);

        parseNode.GetStringValue().Returns("test_variant");
        deserializers["variant"](parseNode);
        Assert.Equal("test_variant", model.Variant);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void MetricsRouting_Serialize_WritesExpectedFields()
    {
        var model = new MetricsRouting { ResourceId = 1L, Variant = "test_variant" };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteLongValue("resource_id", 1L);
        writer.Received(1).WriteStringValue("variant", "test_variant");
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
