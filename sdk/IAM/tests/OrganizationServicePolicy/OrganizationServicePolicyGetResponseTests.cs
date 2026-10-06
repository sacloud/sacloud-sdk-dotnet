using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.IAM.OrganizationServicePolicy;
using RuleResponseModel = Sacloud.Sdk.IAM.Models.RuleResponse;

namespace Sacloud.Sdk.IAM.Tests.OrganizationServicePolicy;

/// <summary>
/// OrganizationServicePolicyGetResponseのテスト
/// </summary>
public class OrganizationServicePolicyGetResponseTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = OrganizationServicePolicyGetResponse.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<OrganizationServicePolicyGetResponse>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => OrganizationServicePolicyGetResponse.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"rules"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var response = new OrganizationServicePolicyGetResponse();

        var deserializers = response.GetFieldDeserializers();

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
        var response = new OrganizationServicePolicyGetResponse();

        Assert.Throws<ArgumentNullException>(() => response.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクション（"rules"）が正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var response = new OrganizationServicePolicyGetResponse();
        var deserializers = response.GetFieldDeserializers();

        var parseNodeRules = Substitute.For<IParseNode>();
        var rules = new List<RuleResponseModel>();
        parseNodeRules.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<RuleResponseModel>>()).Returns(rules);
        deserializers["rules"](parseNodeRules);

        Assert.NotNull(response.Rules);
    }

    /// <summary>
    /// シリアライズとデシリアライズのラウンドトリップが正しく動作することを確認
    /// </summary>
    [Fact]
    public async Task OrganizationServicePolicyGetResponse_SerializeDeserialize_RoundTrip()
    {
        var originalModel = new OrganizationServicePolicyGetResponse
        {
            Rules =
            [
                new RuleResponseModel { Code = "rule-code-1", Name = "Rule 1" },
                new RuleResponseModel { Code = "rule-code-2", Name = "Rule 2" }
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
        var deserializedModel = parseNode.GetObjectValue(OrganizationServicePolicyGetResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Rules);
        Assert.Equal(2, deserializedModel.Rules.Count);
        Assert.Equal("rule-code-1", deserializedModel.Rules[0].Code);
        Assert.Equal("Rule 1", deserializedModel.Rules[0].Name);
        Assert.Equal("rule-code-2", deserializedModel.Rules[1].Code);
        Assert.Equal("Rule 2", deserializedModel.Rules[1].Name);
    }

    /// <summary>
    /// AdditionalDataプロパティが正しく初期化されることを確認
    /// </summary>
    [Fact]
    public void AdditionalData_InitializedCorrectly()
    {
        var response = new OrganizationServicePolicyGetResponse();

        Assert.NotNull(response.AdditionalData);
        Assert.Empty(response.AdditionalData);
    }
}
