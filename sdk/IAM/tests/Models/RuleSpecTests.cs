using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using RuleSpecModel = Sacloud.Sdk.IAM.Models.RuleSpec;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// RuleSpecのテスト
/// </summary>
public class RuleSpecTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = RuleSpecModel.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<RuleSpecModel>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Sacloud.Sdk.IAM.Models.RuleSpec.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"contents"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var ruleSpec = new RuleSpecModel();

        var deserializers = ruleSpec.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("contents"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var ruleSpec = new RuleSpecModel();

        Assert.Throws<ArgumentNullException>(() => ruleSpec.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクション（"contents"）が正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var ruleSpec = new RuleSpecModel();
        var deserializers = ruleSpec.GetFieldDeserializers();
        
        var parseNodeContents = Substitute.For<IParseNode>();
        var contents = new List<IAM.Models.RuleContent>
        {
            new IAM.Models.RuleContent(),
            new IAM.Models.RuleContent()
        };
        parseNodeContents.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<IAM.Models.RuleContent>>()).Returns(contents);
        deserializers["contents"](parseNodeContents);

        Assert.NotNull(ruleSpec.Contents);
        Assert.Equal(2, ruleSpec.Contents.Count);
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task RuleSpec_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new RuleSpecModel
        {
            Contents =
            [
                new IAM.Models.RuleContent { AllowAll = true },
                new IAM.Models.RuleContent { DenyAll = true }
            ]
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Sacloud.Sdk.IAM.Models.RuleSpec.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Contents);
        Assert.Equal(2, deserializedModel.Contents.Count);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task RuleSpec_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new RuleSpecModel
        {
            Contents =
            [
                new IAM.Models.RuleContent { AllowAll = true }
            ]
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
        var deserializedModel = parseNode.GetObjectValue(Sacloud.Sdk.IAM.Models.RuleSpec.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Contents);
        Assert.Single(deserializedModel.Contents);
        Assert.NotNull(deserializedModel.AdditionalData);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
