using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// MetricsStorageAccessKeyモデルのテスト
/// </summary>
public class MetricsStorageAccessKeyTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void MetricsStorageAccessKey_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<MetricsStorageAccessKey>(MetricsStorageAccessKey.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));
    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void MetricsStorageAccessKey_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new MetricsStorageAccessKey().Serialize(null!));
    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void MetricsStorageAccessKey_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new MetricsStorageAccessKey().GetFieldDeserializers();
        Assert.Equal(5, d.Count);
        Assert.True(d.ContainsKey("description"));
        Assert.True(d.ContainsKey("id"));
        Assert.True(d.ContainsKey("secret"));
        Assert.True(d.ContainsKey("token"));
        Assert.True(d.ContainsKey("uid"));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void MetricsStorageAccessKey_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new MetricsStorageAccessKey();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetStringValue().Returns("test_description");
        deserializers["description"](parseNode);
        Assert.Equal("test_description", model.Description);

        deserializers["id"](parseNode);

        deserializers["secret"](parseNode);

        deserializers["token"](parseNode);

        deserializers["uid"](parseNode);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void MetricsStorageAccessKey_Serialize_WritesExpectedFields()
    {
        var model = new MetricsStorageAccessKey { Description = "test_description" };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteStringValue("description", "test_description");
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
