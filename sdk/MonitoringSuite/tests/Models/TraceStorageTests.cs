using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// TraceStorageモデルのテスト
/// </summary>
public class TraceStorageTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void TraceStorage_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<TraceStorage>(TraceStorage.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));

    /// <summary>
    /// nullパースノードでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void TraceStorage_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => TraceStorage.CreateFromDiscriminatorValue(null!));

    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void TraceStorage_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new TraceStorage().GetFieldDeserializers();
        Assert.Equal(13, d.Count);
        Assert.True(d.ContainsKey("account_id"));
        Assert.True(d.ContainsKey("classification"));
        Assert.True(d.ContainsKey("created_at"));
        Assert.True(d.ContainsKey("description"));
        Assert.True(d.ContainsKey("endpoints"));
        Assert.True(d.ContainsKey("icon"));
        Assert.True(d.ContainsKey("id"));
        Assert.True(d.ContainsKey("kms_key_id"));
        Assert.True(d.ContainsKey("name"));
        Assert.True(d.ContainsKey("resource_id"));
        Assert.True(d.ContainsKey("retention_period_days"));
        Assert.True(d.ContainsKey("service_principal_id"));
        Assert.True(d.ContainsKey("tags"));
    }

    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void TraceStorage_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new TraceStorage().Serialize(null!));

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void TraceStorage_Serialize_WritesFields()
    {
        var m = new TraceStorage { Name = "trace", Description = "d" };
        var w = Substitute.For<ISerializationWriter>();
        m.Serialize(w);
        w.Received(1).WriteStringValue("name", "trace");
        w.Received(1).WriteStringValue("description", "d");
        w.Received(1).WriteAdditionalData(m.AdditionalData);
    }

    /// <summary>
    /// コンストラクタがAdditionalDataを初期化することを確認
    /// </summary>
    [Fact]
    public void TraceStorage_Constructor_InitializesAdditionalData()
        => Assert.NotNull(new TraceStorage().AdditionalData);

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void TraceStorage_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new TraceStorage();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        deserializers["account_id"](parseNode);

        deserializers["classification"](parseNode);

        deserializers["created_at"](parseNode);

        parseNode.GetStringValue().Returns("test_description");
        deserializers["description"](parseNode);
        Assert.Equal("test_description", model.Description);

        deserializers["endpoints"](parseNode);

        deserializers["icon"](parseNode);

        deserializers["id"](parseNode);

        deserializers["kms_key_id"](parseNode);

        parseNode.GetStringValue().Returns("test_name");
        deserializers["name"](parseNode);
        Assert.Equal("test_name", model.Name);

        deserializers["resource_id"](parseNode);

        deserializers["retention_period_days"](parseNode);

        parseNode.GetLongValue().Returns(1L);
        deserializers["service_principal_id"](parseNode);
        Assert.Equal(1L, model.ServicePrincipalId);

        deserializers["tags"](parseNode);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void TraceStorage_Serialize_WritesExpectedFields()
    {
        var model = new TraceStorage { Description = "test_description", Name = "test_name", ServicePrincipalId = 1L };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteStringValue("description", "test_description");
        writer.Received(1).WriteStringValue("name", "test_name");
        writer.Received(1).WriteLongValue("service_principal_id", 1L);
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
