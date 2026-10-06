using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// DashboardProjectモデルのテスト
/// </summary>
public class DashboardProjectTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void DashboardProject_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<DashboardProject>(DashboardProject.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));
    /// <summary>
    /// nullパースノードでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void DashboardProject_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => DashboardProject.CreateFromDiscriminatorValue(null!));
    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void DashboardProject_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new DashboardProject().GetFieldDeserializers();
        Assert.Equal(8, d.Count);
        Assert.True(d.ContainsKey("account_id"));
        Assert.True(d.ContainsKey("created_at"));
        Assert.True(d.ContainsKey("description"));
        Assert.True(d.ContainsKey("icon"));
        Assert.True(d.ContainsKey("id"));
        Assert.True(d.ContainsKey("name"));
        Assert.True(d.ContainsKey("resource_id"));
        Assert.True(d.ContainsKey("tags"));
    }
    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void DashboardProject_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new DashboardProject().Serialize(null!));
    /// <summary>
    /// コンストラクタがAdditionalDataを初期化することを確認
    /// </summary>
    [Fact]
    public void DashboardProject_Constructor_InitializesAdditionalData()
        => Assert.NotNull(new DashboardProject().AdditionalData);

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void DashboardProject_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new DashboardProject();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        deserializers["account_id"](parseNode);

        deserializers["created_at"](parseNode);

        parseNode.GetStringValue().Returns("test_description");
        deserializers["description"](parseNode);
        Assert.Equal("test_description", model.Description);

        deserializers["icon"](parseNode);

        deserializers["id"](parseNode);

        parseNode.GetStringValue().Returns("test_name");
        deserializers["name"](parseNode);
        Assert.Equal("test_name", model.Name);

        deserializers["resource_id"](parseNode);

        deserializers["tags"](parseNode);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void DashboardProject_Serialize_WritesExpectedFields()
    {
        var model = new DashboardProject { Description = "test_description", Name = "test_name" };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteStringValue("description", "test_description");
        writer.Received(1).WriteStringValue("name", "test_name");
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
