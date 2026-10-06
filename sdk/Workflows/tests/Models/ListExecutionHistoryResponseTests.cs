using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.Workflows.Models;

namespace Sacloud.Sdk.Workflows.Tests.Models;

/// <summary>
/// ListExecutionHistoryResponseモデルのテスト
/// </summary>
public class ListExecutionHistoryResponseTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void ListExecutionHistoryResponse_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = ListExecutionHistoryResponse.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<ListExecutionHistoryResponse>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ListExecutionHistoryResponse_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;
        Assert.Throws<ArgumentNullException>(() => ListExecutionHistoryResponse.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void ListExecutionHistoryResponse_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new ListExecutionHistoryResponse();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Equal(5, deserializers.Count);
        Assert.True(deserializers.ContainsKey("Count"));
        Assert.True(deserializers.ContainsKey("From"));
        Assert.True(deserializers.ContainsKey("Histories"));
        Assert.True(deserializers.ContainsKey("is_ok"));
        Assert.True(deserializers.ContainsKey("Total"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ListExecutionHistoryResponse_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ListExecutionHistoryResponse();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// AdditionalDataプロパティが正しく動作することを確認
    /// </summary>
    [Fact]
    public void ListExecutionHistoryResponse_AdditionalData_CanBeSetAndRetrieved()
    {
        var model = new ListExecutionHistoryResponse();
        var testData = new Dictionary<string, object> { { "custom", "value" } };
        model.AdditionalData = testData;
        Assert.Equal(testData, model.AdditionalData);
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void ListExecutionHistoryResponse_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new ListExecutionHistoryResponse();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetIntValue().Returns(10);
        deserializers["Count"](parseNode);
        Assert.Equal(10, model.Count);

        parseNode.GetIntValue().Returns(0);
        deserializers["From"](parseNode);
        Assert.Equal(0, model.From);

        var historiesList = new List<Historie> { new Historie { Type = Historie_Type.WorkflowWillStart } };
        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<Historie>>()).Returns(historiesList);
        deserializers["Histories"](parseNode);
        Assert.NotNull(model.Histories);
        Assert.Single(model.Histories);

        parseNode.GetBoolValue().Returns(true);
        deserializers["is_ok"](parseNode);
        Assert.True(model.IsOk);

        parseNode.GetIntValue().Returns(100);
        deserializers["Total"](parseNode);
        Assert.Equal(100, model.Total);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ListExecutionHistoryResponse_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ListExecutionHistoryResponse
        {
            Count = 10,
            From = 0,
            Histories = new List<Historie> { new Historie { Type = Historie_Type.WorkflowWillStart } },
            IsOk = true,
            Total = 100
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(ListExecutionHistoryResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Count, deserializedModel.Count);
        Assert.Equal(originalModel.From, deserializedModel.From);
        Assert.NotNull(deserializedModel.Histories);
        Assert.Single(deserializedModel.Histories);
        Assert.Equal(originalModel.IsOk, deserializedModel.IsOk);
        Assert.Equal(originalModel.Total, deserializedModel.Total);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ListExecutionHistoryResponse_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new ListExecutionHistoryResponse
        {
            IsOk = true,
            Total = 100
        };
        originalModel.AdditionalData.Add("custom_field_1", "custom_value_1");
        originalModel.AdditionalData.Add("custom_field_2", 42);

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(ListExecutionHistoryResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}
