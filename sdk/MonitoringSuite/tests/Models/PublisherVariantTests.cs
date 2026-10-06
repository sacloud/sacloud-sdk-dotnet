using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// PublisherVariantモデルのテスト
/// </summary>
public class PublisherVariantTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void PublisherVariant_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<PublisherVariant>(PublisherVariant.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));

    /// <summary>
    /// nullパースノードでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void PublisherVariant_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => PublisherVariant.CreateFromDiscriminatorValue(null!));

    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void PublisherVariant_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new PublisherVariant().GetFieldDeserializers();
        Assert.Equal(5, d.Count);
        Assert.True(d.ContainsKey("label"));
        Assert.True(d.ContainsKey("metrics_prefix"));
        Assert.True(d.ContainsKey("name"));
        Assert.True(d.ContainsKey("storage"));
        Assert.True(d.ContainsKey("system"));
    }

    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void PublisherVariant_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new PublisherVariant().Serialize(null!));

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void PublisherVariant_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new PublisherVariant();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetStringValue().Returns("test_label");
        deserializers["label"](parseNode);
        Assert.Equal("test_label", model.Label);

        parseNode.GetStringValue().Returns("test_metrics_prefix");
        deserializers["metrics_prefix"](parseNode);
        Assert.Equal("test_metrics_prefix", model.MetricsPrefix);

        parseNode.GetStringValue().Returns("test_name");
        deserializers["name"](parseNode);
        Assert.Equal("test_name", model.Name);

        deserializers["storage"](parseNode);

        deserializers["system"](parseNode);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void PublisherVariant_Serialize_WritesExpectedFields()
    {
        var model = new PublisherVariant { Label = "test_label", MetricsPrefix = "test_metrics_prefix", Name = "test_name" };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteStringValue("label", "test_label");
        writer.Received(1).WriteStringValue("metrics_prefix", "test_metrics_prefix");
        writer.Received(1).WriteStringValue("name", "test_name");
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
