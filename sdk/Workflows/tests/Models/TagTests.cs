using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.Workflows.Models;

namespace Sacloud.Sdk.Workflows.Tests.Models;

/// <summary>
/// Tagモデルのテスト
/// </summary>
public class TagTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void Tag_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = Tag.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<Tag>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Tag_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() => Tag.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void Tag_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new Tag();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("Name"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Tag_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new Tag();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void Tag_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new Tag();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetStringValue().Returns("test-tag");
        deserializers["Name"](parseNode);
        Assert.Equal("test-tag", model.Name);
    }

    /// <summary>
    /// AdditionalDataプロパティが正しく動作することを確認
    /// </summary>
    [Fact]
    public void Tag_AdditionalData_CanBeSetAndRetrieved()
    {
        var model = new Tag();
        var testData = new Dictionary<string, object>
        {
            { "customField", "customValue" }
        };

        model.AdditionalData = testData;

        Assert.Equal(testData, model.AdditionalData);
        Assert.Equal("customValue", model.AdditionalData["customField"]);
    }
}
