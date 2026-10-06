using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// ResourcesLimitsモデルのテスト
/// </summary>
public class ResourcesLimitsTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void ResourcesLimits_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<ResourcesLimits>(ResourcesLimits.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));

    /// <summary>
    /// nullパースノードでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void ResourcesLimits_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => ResourcesLimits.CreateFromDiscriminatorValue(null!));

    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void ResourcesLimits_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new ResourcesLimits().GetFieldDeserializers();
        Assert.Equal(5, d.Count);
        Assert.True(d.ContainsKey("alerts"));
        Assert.True(d.ContainsKey("dashboards"));
        Assert.True(d.ContainsKey("logs"));
        Assert.True(d.ContainsKey("metrics"));
        Assert.True(d.ContainsKey("traces"));
    }

    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void ResourcesLimits_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new ResourcesLimits().Serialize(null!));

    /// <summary>
    /// コンストラクタがAdditionalDataを初期化することを確認
    /// </summary>
    [Fact]
    public void ResourcesLimits_Constructor_InitializesAdditionalData()
        => Assert.NotNull(new ResourcesLimits().AdditionalData);

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void ResourcesLimits_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new ResourcesLimits();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        deserializers["alerts"](parseNode);

        deserializers["dashboards"](parseNode);

        deserializers["logs"](parseNode);

        deserializers["metrics"](parseNode);

        deserializers["traces"](parseNode);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void ResourcesLimits_Serialize_WritesExpectedFields()
    {
        var model = new ResourcesLimits();
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
