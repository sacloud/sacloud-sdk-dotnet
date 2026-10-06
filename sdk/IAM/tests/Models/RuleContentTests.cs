using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using RuleContentModel = Sacloud.Sdk.IAM.Models.RuleContent;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// RuleContentのテスト
/// </summary>
public class RuleContentTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = RuleContentModel.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<RuleContentModel>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Sacloud.Sdk.IAM.Models.RuleContent.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"allow_all", "deny_all", "enforce", "values"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var ruleContent = new RuleContentModel();

        var deserializers = ruleContent.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(4, deserializers.Count);
        Assert.True(deserializers.ContainsKey("allow_all"));
        Assert.True(deserializers.ContainsKey("deny_all"));
        Assert.True(deserializers.ContainsKey("enforce"));
        Assert.True(deserializers.ContainsKey("values"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var ruleContent = new RuleContentModel();

        Assert.Throws<ArgumentNullException>(() => ruleContent.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクション（"allow_all", "deny_all", "enforce", "values"）が正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var ruleContent = new RuleContentModel();
        var deserializers = ruleContent.GetFieldDeserializers();
        
        var parseNodeAllowAll = Substitute.For<IParseNode>();
        parseNodeAllowAll.GetBoolValue().Returns(true);
        deserializers["allow_all"](parseNodeAllowAll);
        
        var parseNodeDenyAll = Substitute.For<IParseNode>();
        parseNodeDenyAll.GetBoolValue().Returns(false);
        deserializers["deny_all"](parseNodeDenyAll);
        
        var parseNodeEnforce = Substitute.For<IParseNode>();
        parseNodeEnforce.GetBoolValue().Returns(true);
        deserializers["enforce"](parseNodeEnforce);
        
        var parseNodeValues = Substitute.For<IParseNode>();
        var values = new IAM.Models.RuleContent_values();
        parseNodeValues.GetObjectValue(Arg.Any<ParsableFactory<IAM.Models.RuleContent_values>>()).Returns(values);
        deserializers["values"](parseNodeValues);

        Assert.True(ruleContent.AllowAll);
        Assert.False(ruleContent.DenyAll);
        Assert.True(ruleContent.Enforce);
        Assert.NotNull(ruleContent.Values);
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task RuleContent_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new RuleContentModel
        {
            AllowAll = true,
            DenyAll = false,
            Enforce = true
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Sacloud.Sdk.IAM.Models.RuleContent.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AllowAll);
        Assert.False(deserializedModel.DenyAll);
        Assert.True(deserializedModel.Enforce);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task RuleContent_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new RuleContentModel
        {
            AllowAll = true,
            DenyAll = false,
            Enforce = true
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
        var deserializedModel = parseNode.GetObjectValue(Sacloud.Sdk.IAM.Models.RuleContent.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AllowAll);
        Assert.False(deserializedModel.DenyAll);
        Assert.True(deserializedModel.Enforce);
        Assert.NotNull(deserializedModel.AdditionalData);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
