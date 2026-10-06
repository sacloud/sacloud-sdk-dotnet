using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// MetricsStorageモデルのテスト
/// </summary>
public class MetricsStorageTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void MetricsStorage_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<MetricsStorage>(MetricsStorage.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));
    /// <summary>
    /// nullパースノードでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void MetricsStorage_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => MetricsStorage.CreateFromDiscriminatorValue(null!));
    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void MetricsStorage_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new MetricsStorage().GetFieldDeserializers();
        Assert.Equal(12, d.Count);
        Assert.True(d.ContainsKey("account_id"));
        Assert.True(d.ContainsKey("created_at"));
        Assert.True(d.ContainsKey("description"));
        Assert.True(d.ContainsKey("endpoints"));
        Assert.True(d.ContainsKey("icon"));
        Assert.True(d.ContainsKey("id"));
        Assert.True(d.ContainsKey("is_system"));
        Assert.True(d.ContainsKey("name"));
        Assert.True(d.ContainsKey("resource_id"));
        Assert.True(d.ContainsKey("tags"));
        Assert.True(d.ContainsKey("updated_at"));
        Assert.True(d.ContainsKey("usage"));
    }
    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void MetricsStorage_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new MetricsStorage().Serialize(null!));
    /// <summary>
    /// コンストラクタがAdditionalDataを初期化することを確認
    /// </summary>
    [Fact]
    public void MetricsStorage_Constructor_InitializesAdditionalData()
        => Assert.NotNull(new MetricsStorage().AdditionalData);

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void MetricsStorage_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new MetricsStorage();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        deserializers["account_id"](parseNode);

        deserializers["created_at"](parseNode);

        parseNode.GetStringValue().Returns("test_description");
        deserializers["description"](parseNode);
        Assert.Equal("test_description", model.Description);

        deserializers["endpoints"](parseNode);

        deserializers["icon"](parseNode);

        deserializers["id"](parseNode);

        deserializers["is_system"](parseNode);

        parseNode.GetStringValue().Returns("test_name");
        deserializers["name"](parseNode);
        Assert.Equal("test_name", model.Name);

        deserializers["resource_id"](parseNode);

        deserializers["tags"](parseNode);

        deserializers["updated_at"](parseNode);

        deserializers["usage"](parseNode);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void MetricsStorage_Serialize_WritesExpectedFields()
    {
        var model = new MetricsStorage { Description = "test_description", Name = "test_name" };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteStringValue("description", "test_description");
        writer.Received(1).WriteStringValue("name", "test_name");
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
