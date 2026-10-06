using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// ResourceItemLimitsモデルのテスト
/// </summary>
public class ResourceItemLimitsTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void ResourceItemLimits_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<ResourceItemLimits>(ResourceItemLimits.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));

    /// <summary>
    /// nullパースノードでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void ResourceItemLimits_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => ResourceItemLimits.CreateFromDiscriminatorValue(null!));

    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void ResourceItemLimits_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new ResourceItemLimits().GetFieldDeserializers();
        Assert.Equal(2, d.Count);
        Assert.True(d.ContainsKey("max_user_count"));
        Assert.True(d.ContainsKey("max_user_dedicated_count"));
    }

    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void ResourceItemLimits_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new ResourceItemLimits().Serialize(null!));

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void ResourceItemLimits_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new ResourceItemLimits();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetIntValue().Returns(1);
        deserializers["max_user_count"](parseNode);
        Assert.Equal(1, model.MaxUserCount);

        parseNode.GetIntValue().Returns(1);
        deserializers["max_user_dedicated_count"](parseNode);
        Assert.Equal(1, model.MaxUserDedicatedCount);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void ResourceItemLimits_Serialize_WritesExpectedFields()
    {
        var model = new ResourceItemLimits { MaxUserCount = 1, MaxUserDedicatedCount = 1 };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteIntValue("max_user_count", 1);
        writer.Received(1).WriteIntValue("max_user_dedicated_count", 1);
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
