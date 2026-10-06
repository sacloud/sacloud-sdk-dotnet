using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.Workflows.Models;

namespace Sacloud.Sdk.Workflows.Tests.Models;

/// <summary>
/// CreateExecutionモデルのテスト
/// </summary>
public class CreateExecutionTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void CreateExecution_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = CreateExecution.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<CreateExecution>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateExecution_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() => CreateExecution.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void CreateExecution_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new CreateExecution();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(4, deserializers.Count);
        Assert.True(deserializers.ContainsKey("Args"));
        Assert.True(deserializers.ContainsKey("Name"));
        Assert.True(deserializers.ContainsKey("RevisionAlias"));
        Assert.True(deserializers.ContainsKey("RevisionId"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateExecution_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new CreateExecution();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void CreateExecution_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new CreateExecution();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetStringValue().Returns("test-args");
        deserializers["Args"](parseNode);
        Assert.Equal("test-args", model.Args);

        parseNode.GetStringValue().Returns("Test execution");
        deserializers["Name"](parseNode);
        Assert.Equal("Test execution", model.Name);

        parseNode.GetStringValue().Returns("v1");
        deserializers["RevisionAlias"](parseNode);
        Assert.Equal("v1", model.RevisionAlias);

        parseNode.GetIntValue().Returns(1);
        deserializers["RevisionId"](parseNode);
        Assert.Equal(1, model.RevisionId);
    }

    /// <summary>
    /// AdditionalDataプロパティが正しく動作することを確認
    /// </summary>
    [Fact]
    public void CreateExecution_AdditionalData_CanBeSetAndRetrieved()
    {
        var model = new CreateExecution();
        var testData = new Dictionary<string, object>
        {
            { "customField", "customValue" }
        };

        model.AdditionalData = testData;

        Assert.Equal(testData, model.AdditionalData);
        Assert.Equal("customValue", model.AdditionalData["customField"]);
    }
}
