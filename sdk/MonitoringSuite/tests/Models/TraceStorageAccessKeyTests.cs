using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// TraceStorageAccessKeyモデルのテスト
/// </summary>
public class TraceStorageAccessKeyTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void TraceStorageAccessKey_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<TraceStorageAccessKey>(TraceStorageAccessKey.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));

    /// <summary>
    /// nullパースノードでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void TraceStorageAccessKey_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => TraceStorageAccessKey.CreateFromDiscriminatorValue(null!));

    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void TraceStorageAccessKey_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new TraceStorageAccessKey().GetFieldDeserializers();
        Assert.Equal(4, d.Count);
        Assert.True(d.ContainsKey("description"));
        Assert.True(d.ContainsKey("secret"));
        Assert.True(d.ContainsKey("token"));
        Assert.True(d.ContainsKey("uid"));
    }

    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void TraceStorageAccessKey_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new TraceStorageAccessKey().Serialize(null!));

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void TraceStorageAccessKey_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new TraceStorageAccessKey();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetStringValue().Returns("test_description");
        deserializers["description"](parseNode);
        Assert.Equal("test_description", model.Description);

        deserializers["secret"](parseNode);

        deserializers["token"](parseNode);

        deserializers["uid"](parseNode);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void TraceStorageAccessKey_Serialize_WritesExpectedFields()
    {
        var model = new TraceStorageAccessKey { Description = "test_description" };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteStringValue("description", "test_description");
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
