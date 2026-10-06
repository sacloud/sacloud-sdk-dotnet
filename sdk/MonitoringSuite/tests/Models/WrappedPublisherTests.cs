using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// WrappedPublisherモデルのテスト
/// </summary>
public class WrappedPublisherTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WrappedPublisher_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<WrappedPublisher>(WrappedPublisher.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));

    /// <summary>
    /// nullパースノードでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void WrappedPublisher_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => WrappedPublisher.CreateFromDiscriminatorValue(null!));

    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void WrappedPublisher_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new WrappedPublisher().GetFieldDeserializers();
        Assert.Equal(4, d.Count);
        Assert.True(d.ContainsKey("code"));
        Assert.True(d.ContainsKey("description"));
        Assert.True(d.ContainsKey("is_ok"));
        Assert.True(d.ContainsKey("variants"));
    }

    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void WrappedPublisher_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new WrappedPublisher().Serialize(null!));

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void WrappedPublisher_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new WrappedPublisher();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetStringValue().Returns("test_code");
        deserializers["code"](parseNode);
        Assert.Equal("test_code", model.Code);

        parseNode.GetStringValue().Returns("test_description");
        deserializers["description"](parseNode);
        Assert.Equal("test_description", model.Description);

        parseNode.GetBoolValue().Returns(true);
        deserializers["is_ok"](parseNode);
        Assert.Equal(true, model.IsOk);

        deserializers["variants"](parseNode);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void WrappedPublisher_Serialize_WritesExpectedFields()
    {
        var model = new WrappedPublisher { Code = "test_code", Description = "test_description", IsOk = true };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteStringValue("code", "test_code");
        writer.Received(1).WriteStringValue("description", "test_description");
        writer.Received(1).WriteBoolValue("is_ok", true);
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
