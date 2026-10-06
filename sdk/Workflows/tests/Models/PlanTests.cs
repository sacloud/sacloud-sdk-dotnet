using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.Workflows.Models;

namespace Sacloud.Sdk.Workflows.Tests.Models;

/// <summary>
/// Planモデルのテスト
/// </summary>
public class PlanTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void Plan_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = Plan.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<Plan>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Plan_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() => Plan.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void Plan_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new Plan();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(8, deserializers.Count);
        Assert.True(deserializers.ContainsKey("basePrice"));
        Assert.True(deserializers.ContainsKey("grade"));
        Assert.True(deserializers.ContainsKey("id"));
        Assert.True(deserializers.ContainsKey("includedSteps"));
        Assert.True(deserializers.ContainsKey("name"));
        Assert.True(deserializers.ContainsKey("overagePricePerUnit"));
        Assert.True(deserializers.ContainsKey("overageStepUnit"));
        Assert.True(deserializers.ContainsKey("serviceClassPath"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Plan_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new Plan();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void Plan_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new Plan();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetIntValue().Returns(1000);
        deserializers["basePrice"](parseNode);
        Assert.Equal(1000, model.BasePrice);

        parseNode.GetIntValue().Returns(1);
        deserializers["grade"](parseNode);
        Assert.Equal(1, model.Grade);

        parseNode.GetIntValue().Returns(123);
        deserializers["id"](parseNode);
        Assert.Equal(123, model.Id);

        parseNode.GetIntValue().Returns(10000);
        deserializers["includedSteps"](parseNode);
        Assert.Equal(10000, model.IncludedSteps);

        parseNode.GetStringValue().Returns("Test Plan");
        deserializers["name"](parseNode);
        Assert.Equal("Test Plan", model.Name);

        parseNode.GetIntValue().Returns(1);
        deserializers["overagePricePerUnit"](parseNode);
        Assert.Equal(1, model.OveragePricePerUnit);

        parseNode.GetIntValue().Returns(100);
        deserializers["overageStepUnit"](parseNode);
        Assert.Equal(100, model.OverageStepUnit);

        parseNode.GetStringValue().Returns("/workflow/standard");
        deserializers["serviceClassPath"](parseNode);
        Assert.Equal("/workflow/standard", model.ServiceClassPath);
    }

    /// <summary>
    /// AdditionalDataプロパティが正しく動作することを確認
    /// </summary>
    [Fact]
    public void Plan_AdditionalData_CanBeSetAndRetrieved()
    {
        var model = new Plan();
        var testData = new Dictionary<string, object> { { "custom", "value" } };
        model.AdditionalData = testData;
        Assert.Equal(testData, model.AdditionalData);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task Plan_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new Plan
        {
            BasePrice = 1000,
            Grade = 1,
            Id = 123,
            IncludedSteps = 10000,
            Name = "Test Plan",
            OveragePricePerUnit = 1,
            OverageStepUnit = 100,
            ServiceClassPath = "/workflow/standard"
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Plan.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.BasePrice, deserializedModel.BasePrice);
        Assert.Equal(originalModel.Grade, deserializedModel.Grade);
        Assert.Equal(originalModel.Id, deserializedModel.Id);
        Assert.Equal(originalModel.IncludedSteps, deserializedModel.IncludedSteps);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.Equal(originalModel.OveragePricePerUnit, deserializedModel.OveragePricePerUnit);
        Assert.Equal(originalModel.OverageStepUnit, deserializedModel.OverageStepUnit);
        Assert.Equal(originalModel.ServiceClassPath, deserializedModel.ServiceClassPath);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task Plan_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new Plan
        {
            Name = "Test Plan"
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
        var deserializedModel = parseNode.GetObjectValue(Plan.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}

