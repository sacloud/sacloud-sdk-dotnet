using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using RuleResponseModel = Sacloud.Sdk.IAM.Models.RuleResponse;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// RuleResponseのテスト
/// </summary>
public class RuleResponseTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = RuleResponseModel.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<RuleResponseModel>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Sacloud.Sdk.IAM.Models.RuleResponse.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"is_active", "is_dry_run", "code", "dry_run_spec", "name", "spec"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var ruleResponse = new RuleResponseModel();

        var deserializers = ruleResponse.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(6, deserializers.Count);
        Assert.True(deserializers.ContainsKey("is_active"));
        Assert.True(deserializers.ContainsKey("is_dry_run"));
        Assert.True(deserializers.ContainsKey("code"));
        Assert.True(deserializers.ContainsKey("dry_run_spec"));
        Assert.True(deserializers.ContainsKey("name"));
        Assert.True(deserializers.ContainsKey("spec"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var ruleResponse = new RuleResponseModel();

        Assert.Throws<ArgumentNullException>(() => ruleResponse.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクション（"is_active", "is_dry_run", "code", "dry_run_spec", "name", "spec"）が正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var ruleResponse = new RuleResponseModel();
        var deserializers = ruleResponse.GetFieldDeserializers();
        
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
        
        var parseNodeName = Substitute.For<IParseNode>();
        parseNodeName.GetStringValue().Returns("Test Rule");
        deserializers["name"](parseNodeName);
        
        var parseNodeSpec = Substitute.For<IParseNode>();
        var spec = new IAM.Models.RuleSpec();
        parseNodeSpec.GetObjectValue(Arg.Any<ParsableFactory<IAM.Models.RuleSpec>>()).Returns(spec);
        deserializers["spec"](parseNodeSpec);

        Assert.True(ruleResponse.IsActive);
        Assert.False(ruleResponse.IsDryRun);
        Assert.Equal("TEST_CODE", ruleResponse.Code);
        Assert.NotNull(ruleResponse.DryRunSpec);
        Assert.Equal("Test Rule", ruleResponse.Name);
        Assert.NotNull(ruleResponse.Spec);
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task RuleResponse_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new RuleResponseModel
        {
            IsActive = true,
            IsDryRun = false,
            Code = "TEST_CODE",
            Name = "Test Rule"
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Sacloud.Sdk.IAM.Models.RuleResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.IsActive);
        Assert.False(deserializedModel.IsDryRun);
        Assert.Equal("TEST_CODE", deserializedModel.Code);
        Assert.Equal("Test Rule", deserializedModel.Name);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task RuleResponse_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new RuleResponseModel
        {
            IsActive = true,
            IsDryRun = false,
            Code = "TEST_CODE",
            Name = "Test Rule"
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
        var deserializedModel = parseNode.GetObjectValue(Sacloud.Sdk.IAM.Models.RuleResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.IsActive);
        Assert.False(deserializedModel.IsDryRun);
        Assert.Equal("TEST_CODE", deserializedModel.Code);
        Assert.Equal("Test Rule", deserializedModel.Name);
        Assert.NotNull(deserializedModel.AdditionalData);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
