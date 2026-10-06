using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.Workflows.Models;

namespace Sacloud.Sdk.Workflows.Tests.Models;

/// <summary>
/// GetExecutionResponseモデルのテスト
/// </summary>
public class GetExecutionResponseTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void GetExecutionResponse_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = GetExecutionResponse.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<GetExecutionResponse>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void GetExecutionResponse_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() => GetExecutionResponse.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void GetExecutionResponse_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new GetExecutionResponse();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("Execution"));
        Assert.True(deserializers.ContainsKey("is_ok"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void GetExecutionResponse_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new GetExecutionResponse();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void GetExecutionResponse_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new GetExecutionResponse();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        var testExecution = new Execution();
        parseNode.GetObjectValue<Execution>(Arg.Any<ParsableFactory<Execution>>()).Returns(testExecution);
        deserializers["Execution"](parseNode);
        Assert.NotNull(model.Execution);

        parseNode.GetBoolValue().Returns(true);
        deserializers["is_ok"](parseNode);
        Assert.True(model.IsOk);
    }

    /// <summary>
    /// AdditionalDataプロパティが正しく動作することを確認
    /// </summary>
    [Fact]
    public void GetExecutionResponse_AdditionalData_CanBeSetAndRetrieved()
    {
        var model = new GetExecutionResponse();
        var testData = new Dictionary<string, object>
        {
            { "customField", "customValue" }
        };

        model.AdditionalData = testData;

        Assert.Equal(testData, model.AdditionalData);
        Assert.Equal("customValue", model.AdditionalData["customField"]);
    }
}
