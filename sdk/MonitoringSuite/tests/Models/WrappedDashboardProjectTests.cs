using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// WrappedDashboardProjectモデルのテスト
/// </summary>
public class WrappedDashboardProjectTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WrappedDashboardProject_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<WrappedDashboardProject>(WrappedDashboardProject.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));

    /// <summary>
    /// nullパースノードでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void WrappedDashboardProject_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => WrappedDashboardProject.CreateFromDiscriminatorValue(null!));

    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void WrappedDashboardProject_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new WrappedDashboardProject().GetFieldDeserializers();
        Assert.Equal(9, d.Count);
        Assert.True(d.ContainsKey("account_id"));
        Assert.True(d.ContainsKey("created_at"));
        Assert.True(d.ContainsKey("description"));
        Assert.True(d.ContainsKey("icon"));
        Assert.True(d.ContainsKey("id"));
        Assert.True(d.ContainsKey("is_ok"));
        Assert.True(d.ContainsKey("name"));
        Assert.True(d.ContainsKey("resource_id"));
        Assert.True(d.ContainsKey("tags"));
    }

    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void WrappedDashboardProject_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new WrappedDashboardProject().Serialize(null!));

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void WrappedDashboardProject_Serialize_WritesFields()
    {
        var m = new WrappedDashboardProject { Name = "dash", IsOk = false };
        var w = Substitute.For<ISerializationWriter>();
        m.Serialize(w);
        w.Received(1).WriteStringValue("name", "dash");
        w.Received(1).WriteBoolValue("is_ok", false);
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void WrappedDashboardProject_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new WrappedDashboardProject();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        deserializers["account_id"](parseNode);

        deserializers["created_at"](parseNode);

        parseNode.GetStringValue().Returns("test_description");
        deserializers["description"](parseNode);
        Assert.Equal("test_description", model.Description);

        deserializers["icon"](parseNode);

        deserializers["id"](parseNode);

        parseNode.GetBoolValue().Returns(true);
        deserializers["is_ok"](parseNode);
        Assert.Equal(true, model.IsOk);

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
    public void WrappedDashboardProject_Serialize_WritesExpectedFields()
    {
        var model = new WrappedDashboardProject { Description = "test_description", IsOk = true, Name = "test_name" };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteStringValue("description", "test_description");
        writer.Received(1).WriteBoolValue("is_ok", true);
        writer.Received(1).WriteStringValue("name", "test_name");
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
