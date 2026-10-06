using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.IAM.OrganizationServicePolicy;
using RuleModel = Sacloud.Sdk.IAM.Models.Rule;

namespace Sacloud.Sdk.IAM.Tests.OrganizationServicePolicy;

/// <summary>
/// OrganizationServicePolicyPutRequestBodyのテスト
/// </summary>
public class OrganizationServicePolicyPutRequestBodyTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = OrganizationServicePolicyPutRequestBody.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<OrganizationServicePolicyPutRequestBody>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => OrganizationServicePolicyPutRequestBody.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"rules"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var body = new OrganizationServicePolicyPutRequestBody();

        var deserializers = body.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("rules"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var body = new OrganizationServicePolicyPutRequestBody();

        Assert.Throws<ArgumentNullException>(() => body.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクション（"rules"）が正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var body = new OrganizationServicePolicyPutRequestBody();
        var deserializers = body.GetFieldDeserializers();

        var parseNodeRules = Substitute.For<IParseNode>();
        var rules = new List<RuleModel>();
        parseNodeRules.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<RuleModel>>()).Returns(rules);
        deserializers["rules"](parseNodeRules);

        Assert.NotNull(body.Rules);
    }

    /// <summary>
    /// シリアライズとデシリアライズのラウンドトリップが正しく動作することを確認
    /// </summary>
    [Fact]
    public async Task OrganizationServicePolicyPutRequestBody_SerializeDeserialize_RoundTrip()
    {
        var originalModel = new OrganizationServicePolicyPutRequestBody
        {
            Rules =
            [
                new RuleModel { Code = "rule-code-1" },
                new RuleModel { Code = "rule-code-2" }
            ]
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        await content.CopyToAsync(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(OrganizationServicePolicyPutRequestBody.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Rules);
        Assert.Equal(2, deserializedModel.Rules.Count);
        Assert.Equal("rule-code-1", deserializedModel.Rules[0].Code);
        Assert.Equal("rule-code-2", deserializedModel.Rules[1].Code);
    }

    /// <summary>
    /// AdditionalDataプロパティが正しく初期化されることを確認
    /// </summary>
    [Fact]
    public void AdditionalData_InitializedCorrectly()
    {
        var body = new OrganizationServicePolicyPutRequestBody();

        Assert.NotNull(body.AdditionalData);
        Assert.Empty(body.AdditionalData);
    }
}
