using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// WrappedLogStorageモデルのテスト
/// </summary>
public class WrappedLogStorageTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WrappedLogStorage_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<WrappedLogStorage>(WrappedLogStorage.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));

    /// <summary>
    /// nullパースノードでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void WrappedLogStorage_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => WrappedLogStorage.CreateFromDiscriminatorValue(null!));

    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void WrappedLogStorage_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new WrappedLogStorage().GetFieldDeserializers();
        Assert.Equal(16, d.Count);
        Assert.True(d.ContainsKey("account_id"));
        Assert.True(d.ContainsKey("classification"));
        Assert.True(d.ContainsKey("created_at"));
        Assert.True(d.ContainsKey("description"));
        Assert.True(d.ContainsKey("endpoints"));
        Assert.True(d.ContainsKey("expire_day"));
        Assert.True(d.ContainsKey("icon"));
        Assert.True(d.ContainsKey("id"));
        Assert.True(d.ContainsKey("is_ok"));
        Assert.True(d.ContainsKey("is_system"));
        Assert.True(d.ContainsKey("kms_key_id"));
        Assert.True(d.ContainsKey("name"));
        Assert.True(d.ContainsKey("resource_id"));
        Assert.True(d.ContainsKey("service_principal_id"));
        Assert.True(d.ContainsKey("tags"));
        Assert.True(d.ContainsKey("usage"));
    }

    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void WrappedLogStorage_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new WrappedLogStorage().Serialize(null!));

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void WrappedLogStorage_Serialize_WritesFields()
    {
        var m = new WrappedLogStorage { Name = "logs", Description = "d", IsOk = true };
        var w = Substitute.For<ISerializationWriter>();
        m.Serialize(w);
        w.Received(1).WriteStringValue("name", "logs");
        w.Received(1).WriteBoolValue("is_ok", true);
        w.Received(1).WriteAdditionalData(m.AdditionalData);
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void WrappedLogStorage_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new WrappedLogStorage();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        deserializers["account_id"](parseNode);

        deserializers["classification"](parseNode);

        deserializers["created_at"](parseNode);

        parseNode.GetStringValue().Returns("test_description");
        deserializers["description"](parseNode);
        Assert.Equal("test_description", model.Description);

        deserializers["endpoints"](parseNode);

        deserializers["expire_day"](parseNode);

        deserializers["icon"](parseNode);

        deserializers["id"](parseNode);

        parseNode.GetBoolValue().Returns(true);
        deserializers["is_ok"](parseNode);
        Assert.Equal(true, model.IsOk);

        deserializers["is_system"](parseNode);

        deserializers["kms_key_id"](parseNode);

        parseNode.GetStringValue().Returns("test_name");
        deserializers["name"](parseNode);
        Assert.Equal("test_name", model.Name);

        deserializers["resource_id"](parseNode);

        parseNode.GetLongValue().Returns(1L);
        deserializers["service_principal_id"](parseNode);
        Assert.Equal(1L, model.ServicePrincipalId);

        deserializers["tags"](parseNode);

        deserializers["usage"](parseNode);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void WrappedLogStorage_Serialize_WritesExpectedFields()
    {
        var model = new WrappedLogStorage { Description = "test_description", IsOk = true, Name = "test_name", ServicePrincipalId = 1L };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteStringValue("description", "test_description");
        writer.Received(1).WriteBoolValue("is_ok", true);
        writer.Received(1).WriteStringValue("name", "test_name");
        writer.Received(1).WriteLongValue("service_principal_id", 1L);
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
