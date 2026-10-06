using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// WrappedLogStorageAccessKeyモデルのテスト
/// </summary>
public class WrappedLogStorageAccessKeyTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WrappedLogStorageAccessKey_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<WrappedLogStorageAccessKey>(WrappedLogStorageAccessKey.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));

    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void WrappedLogStorageAccessKey_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new WrappedLogStorageAccessKey().Serialize(null!));

    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void WrappedLogStorageAccessKey_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new WrappedLogStorageAccessKey().GetFieldDeserializers();
        Assert.Equal(6, d.Count);
        Assert.True(d.ContainsKey("description"));
        Assert.True(d.ContainsKey("id"));
        Assert.True(d.ContainsKey("is_ok"));
        Assert.True(d.ContainsKey("secret"));
        Assert.True(d.ContainsKey("token"));
        Assert.True(d.ContainsKey("uid"));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void WrappedLogStorageAccessKey_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new WrappedLogStorageAccessKey();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetStringValue().Returns("test_description");
        deserializers["description"](parseNode);
        Assert.Equal("test_description", model.Description);

        deserializers["id"](parseNode);

        parseNode.GetBoolValue().Returns(true);
        deserializers["is_ok"](parseNode);
        Assert.Equal(true, model.IsOk);

        deserializers["secret"](parseNode);

        deserializers["token"](parseNode);

        deserializers["uid"](parseNode);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void WrappedLogStorageAccessKey_Serialize_WritesExpectedFields()
    {
        var model = new WrappedLogStorageAccessKey { Description = "test_description", IsOk = true };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteStringValue("description", "test_description");
        writer.Received(1).WriteBoolValue("is_ok", true);
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
