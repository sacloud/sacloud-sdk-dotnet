using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using RuleContent_valuesModel = Sacloud.Sdk.IAM.Models.RuleContent_values;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// RuleContent_valuesのテスト
/// </summary>
public class RuleContent_valuesTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = RuleContent_valuesModel.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<RuleContent_valuesModel>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Sacloud.Sdk.IAM.Models.RuleContent_values.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"allowed_values", "denied_values"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var ruleContentValues = new RuleContent_valuesModel();

        var deserializers = ruleContentValues.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("allowed_values"));
        Assert.True(deserializers.ContainsKey("denied_values"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var ruleContentValues = new RuleContent_valuesModel();

        Assert.Throws<ArgumentNullException>(() => ruleContentValues.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクション（"allowed_values", "denied_values"）が正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var ruleContentValues = new RuleContent_valuesModel();
        var deserializers = ruleContentValues.GetFieldDeserializers();
        
        var parseNodeAllowedValues = Substitute.For<IParseNode>();
        var allowedValues = new List<string> { "value1", "value2" };
        parseNodeAllowedValues.GetCollectionOfPrimitiveValues<string>().Returns(allowedValues);
        deserializers["allowed_values"](parseNodeAllowedValues);
        
        var parseNodeDeniedValues = Substitute.For<IParseNode>();
        var deniedValues = new List<string> { "value3", "value4" };
        parseNodeDeniedValues.GetCollectionOfPrimitiveValues<string>().Returns(deniedValues);
        deserializers["denied_values"](parseNodeDeniedValues);

        Assert.NotNull(ruleContentValues.AllowedValues);
        Assert.Equal(2, ruleContentValues.AllowedValues.Count);
        Assert.Equal("value1", ruleContentValues.AllowedValues[0]);
        Assert.Equal("value2", ruleContentValues.AllowedValues[1]);
        Assert.NotNull(ruleContentValues.DeniedValues);
        Assert.Equal(2, ruleContentValues.DeniedValues.Count);
        Assert.Equal("value3", ruleContentValues.DeniedValues[0]);
        Assert.Equal("value4", ruleContentValues.DeniedValues[1]);
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task RuleContent_values_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new RuleContent_valuesModel
        {
            AllowedValues = ["value1", "value2"],
            DeniedValues = ["value3", "value4"]
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Sacloud.Sdk.IAM.Models.RuleContent_values.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.AllowedValues);
        Assert.Equal(2, deserializedModel.AllowedValues.Count);
        Assert.Contains("value1", deserializedModel.AllowedValues);
        Assert.Contains("value2", deserializedModel.AllowedValues);
        Assert.NotNull(deserializedModel.DeniedValues);
        Assert.Equal(2, deserializedModel.DeniedValues.Count);
        Assert.Contains("value3", deserializedModel.DeniedValues);
        Assert.Contains("value4", deserializedModel.DeniedValues);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task RuleContent_values_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new RuleContent_valuesModel
        {
            AllowedValues = ["value1", "value2"],
            DeniedValues = ["value3", "value4"]
        };
        originalModel.AdditionalData.Add("customField", "customValue");

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Sacloud.Sdk.IAM.Models.RuleContent_values.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.AllowedValues);
        Assert.Equal(2, deserializedModel.AllowedValues.Count);
        Assert.NotNull(deserializedModel.DeniedValues);
        Assert.Equal(2, deserializedModel.DeniedValues.Count);
        Assert.NotNull(deserializedModel.AdditionalData);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
