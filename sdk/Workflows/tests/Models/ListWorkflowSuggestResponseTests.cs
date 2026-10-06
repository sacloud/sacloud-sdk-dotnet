using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.Workflows.Models;

namespace Sacloud.Sdk.Workflows.Tests.Models;

/// <summary>
/// ListWorkflowSuggestResponseモデルのテスト
/// </summary>
public class ListWorkflowSuggestResponseTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void ListWorkflowSuggestResponse_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = ListWorkflowSuggestResponse.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<ListWorkflowSuggestResponse>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ListWorkflowSuggestResponse_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;
        Assert.Throws<ArgumentNullException>(() => ListWorkflowSuggestResponse.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void ListWorkflowSuggestResponse_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new ListWorkflowSuggestResponse();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Equal(5, deserializers.Count);
        Assert.True(deserializers.ContainsKey("Count"));
        Assert.True(deserializers.ContainsKey("From"));
        Assert.True(deserializers.ContainsKey("is_ok"));
        Assert.True(deserializers.ContainsKey("Suggests"));
        Assert.True(deserializers.ContainsKey("Total"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ListWorkflowSuggestResponse_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ListWorkflowSuggestResponse();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// AdditionalDataプロパティが正しく動作することを確認
    /// </summary>
    [Fact]
    public void ListWorkflowSuggestResponse_AdditionalData_CanBeSetAndRetrieved()
    {
        var model = new ListWorkflowSuggestResponse();
        var testData = new Dictionary<string, object> { { "custom", "value" } };
        model.AdditionalData = testData;
        Assert.Equal(testData, model.AdditionalData);
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確誋
    /// </summary>
    [Fact]
    public void ListWorkflowSuggestResponse_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new ListWorkflowSuggestResponse();
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

        var suggestsList = new List<Suggest> { new Suggest { Name = "test-suggest" } };
        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<Suggest>>()).Returns(suggestsList);
        deserializers["Suggests"](parseNode);
        Assert.NotNull(model.Suggests);
        Assert.Single(model.Suggests);

        parseNode.GetIntValue().Returns(100);
        deserializers["Total"](parseNode);
        Assert.Equal(100, model.Total);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ListWorkflowSuggestResponse_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ListWorkflowSuggestResponse
        {
            Count = 10,
            From = 0,
            IsOk = true,
            Suggests = new List<Suggest> { new Suggest { Name = "test-suggest" } },
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
        var deserializedModel = parseNode.GetObjectValue(ListWorkflowSuggestResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Count, deserializedModel.Count);
        Assert.Equal(originalModel.From, deserializedModel.From);
        Assert.Equal(originalModel.IsOk, deserializedModel.IsOk);
        Assert.NotNull(deserializedModel.Suggests);
        Assert.Single(deserializedModel.Suggests);
        Assert.Equal(originalModel.Total, deserializedModel.Total);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ListWorkflowSuggestResponse_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new ListWorkflowSuggestResponse
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
        var deserializedModel = parseNode.GetObjectValue(ListWorkflowSuggestResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}
