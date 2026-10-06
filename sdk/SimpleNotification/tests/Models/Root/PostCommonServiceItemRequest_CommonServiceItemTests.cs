using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.SimpleNotification.Models;
using Sacloud.Sdk.Shared.Validation;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.SimpleNotification.Tests.Models.Root;

/// <summary>
/// PostCommonServiceItemRequest_CommonServiceItemモデルのテスト
/// </summary>
public class PostCommonServiceItemRequest_CommonServiceItemTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = PostCommonServiceItemRequest_CommonServiceItem.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<PostCommonServiceItemRequest_CommonServiceItem>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => PostCommonServiceItemRequest_CommonServiceItem.CreateFromDiscriminatorValue(null!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new PostCommonServiceItemRequest_CommonServiceItem();
        var deserializers = model.GetFieldDeserializers();
        Assert.Equal(7, deserializers.Count);
        Assert.True(deserializers.ContainsKey("Description"));
        Assert.True(deserializers.ContainsKey("Icon"));
        Assert.True(deserializers.ContainsKey("Name"));
        Assert.True(deserializers.ContainsKey("Provider"));
        Assert.True(deserializers.ContainsKey("ServiceClass"));
        Assert.True(deserializers.ContainsKey("Settings"));
        Assert.True(deserializers.ContainsKey("Tags"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new PostCommonServiceItemRequest_CommonServiceItem();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new PostCommonServiceItemRequest_CommonServiceItem();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetStringValue().Returns("test-name");
        deserializers["Name"](parseNode);
        Assert.Equal("test-name", model.Name);

        parseNode.GetStringValue().Returns("test-desc");
        deserializers["Description"](parseNode);
        Assert.Equal("test-desc", model.Description);

        var settings = new CommonServiceItemSettings();
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<CommonServiceItemSettings>>()).Returns(settings);
        deserializers["Settings"](parseNode);
        Assert.Same(settings, model.Settings);
    }
}
