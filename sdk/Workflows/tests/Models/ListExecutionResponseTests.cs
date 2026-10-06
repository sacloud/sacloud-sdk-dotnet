using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.Workflows.Models;

namespace Sacloud.Sdk.Workflows.Tests.Models;

/// <summary>
/// ListExecutionResponseモデルのテスト
/// </summary>
public class ListExecutionResponseTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void ListExecutionResponse_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = ListExecutionResponse.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<ListExecutionResponse>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ListExecutionResponse_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() => ListExecutionResponse.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void ListExecutionResponse_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new ListExecutionResponse();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(5, deserializers.Count);
        Assert.True(deserializers.ContainsKey("Count"));
        Assert.True(deserializers.ContainsKey("Executions"));
        Assert.True(deserializers.ContainsKey("From"));
        Assert.True(deserializers.ContainsKey("is_ok"));
        Assert.True(deserializers.ContainsKey("Total"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ListExecutionResponse_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ListExecutionResponse();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void ListExecutionResponse_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new ListExecutionResponse();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetIntValue().Returns(10);
        deserializers["Count"](parseNode);
        Assert.Equal(10, model.Count);

        parseNode.GetIntValue().Returns(0);
        deserializers["From"](parseNode);
        Assert.Equal(0, model.From);

        parseNode.GetBoolValue().Returns(true);
        deserializers["is_ok"](parseNode);
        Assert.True(model.IsOk);

        parseNode.GetIntValue().Returns(100);
        deserializers["Total"](parseNode);
        Assert.Equal(100, model.Total);
    }

    /// <summary>
    /// AdditionalDataプロパティが正しく動作することを確認
    /// </summary>
    [Fact]
    public void ListExecutionResponse_AdditionalData_CanBeSetAndRetrieved()
    {
        var model = new ListExecutionResponse();
        var testData = new Dictionary<string, object>
        {
            { "customField", "customValue" }
        };

        model.AdditionalData = testData;

        Assert.Equal(testData, model.AdditionalData);
        Assert.Equal("customValue", model.AdditionalData["customField"]);
    }
}
