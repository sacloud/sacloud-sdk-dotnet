using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.SimpleNotification.Commonserviceitem.Item;
using Sacloud.Sdk.SimpleNotification.Models;

namespace Sacloud.Sdk.SimpleNotification.Tests.Models.Handler;

/// <summary>
/// CommonserviceitemDeleteResponseモデルのテスト
/// </summary>
public class CommonserviceitemDeleteResponseTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = CommonserviceitemDeleteResponse.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<CommonserviceitemDeleteResponse>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => CommonserviceitemDeleteResponse.CreateFromDiscriminatorValue(null!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new CommonserviceitemDeleteResponse();
        var deserializers = model.GetFieldDeserializers();
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("CommonServiceItem"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new CommonserviceitemDeleteResponse();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new CommonserviceitemDeleteResponse();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        var item = new CommonServiceItem();
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<CommonServiceItem>>()).Returns(item);
        deserializers["CommonServiceItem"](parseNode);
        Assert.Same(item, model.CommonServiceItem);
    }
}
