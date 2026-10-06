using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using RuleModel = Sacloud.Sdk.IAM.Models.Rule;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// Ruleのテスト
/// </summary>
public class RuleTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = RuleModel.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<RuleModel>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Sacloud.Sdk.IAM.Models.Rule.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"is_active", "is_dry_run", "code", "dry_run_spec", "spec"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var rule = new RuleModel();

        var deserializers = rule.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(5, deserializers.Count);
        Assert.True(deserializers.ContainsKey("is_active"));
        Assert.True(deserializers.ContainsKey("is_dry_run"));
        Assert.True(deserializers.ContainsKey("code"));
        Assert.True(deserializers.ContainsKey("dry_run_spec"));
        Assert.True(deserializers.ContainsKey("spec"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var rule = new RuleModel();

        Assert.Throws<ArgumentNullException>(() => rule.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクション（"is_active", "is_dry_run", "code", "dry_run_spec", "spec"）が正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var rule = new RuleModel();
        var deserializers = rule.GetFieldDeserializers();
        
        var parseNodeIsActive = Substitute.For<IParseNode>();
        parseNodeIsActive.GetBoolValue().Returns(true);
        deserializers["is_active"](parseNodeIsActive);
        
        var parseNodeIsDryRun = Substitute.For<IParseNode>();
        parseNodeIsDryRun.GetBoolValue().Returns(false);
        deserializers["is_dry_run"](parseNodeIsDryRun);
        
        var parseNodeCode = Substitute.For<IParseNode>();
        parseNodeCode.GetStringValue().Returns("TEST_CODE");
        deserializers["code"](parseNodeCode);
        
        var parseNodeDryRunSpec = Substitute.For<IParseNode>();
        var dryRunSpec = new IAM.Models.RuleSpec();
        parseNodeDryRunSpec.GetObjectValue(Arg.Any<ParsableFactory<IAM.Models.RuleSpec>>()).Returns(dryRunSpec);
        deserializers["dry_run_spec"](parseNodeDryRunSpec);
        
        var parseNodeSpec = Substitute.For<IParseNode>();
        var spec = new IAM.Models.RuleSpec();
        parseNodeSpec.GetObjectValue(Arg.Any<ParsableFactory<IAM.Models.RuleSpec>>()).Returns(spec);
        deserializers["spec"](parseNodeSpec);

        Assert.True(rule.IsActive);
        Assert.False(rule.IsDryRun);
        Assert.Equal("TEST_CODE", rule.Code);
        Assert.NotNull(rule.DryRunSpec);
        Assert.NotNull(rule.Spec);
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task Rule_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new RuleModel
        {
            IsActive = true,
            IsDryRun = false,
            Code = "TEST_CODE"
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Sacloud.Sdk.IAM.Models.Rule.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.IsActive);
        Assert.False(deserializedModel.IsDryRun);
        Assert.Equal("TEST_CODE", deserializedModel.Code);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task Rule_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new RuleModel
        {
            IsActive = true,
            IsDryRun = false,
            Code = "TEST_CODE"
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
        var deserializedModel = parseNode.GetObjectValue(Sacloud.Sdk.IAM.Models.Rule.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.IsActive);
        Assert.False(deserializedModel.IsDryRun);
        Assert.Equal("TEST_CODE", deserializedModel.Code);
        Assert.NotNull(deserializedModel.AdditionalData);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
