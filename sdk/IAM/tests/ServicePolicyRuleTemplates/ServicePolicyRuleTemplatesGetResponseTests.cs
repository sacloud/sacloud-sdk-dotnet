using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.IAM.Models;
using Sacloud.Sdk.IAM.ServicePolicyRuleTemplates;

namespace Sacloud.Sdk.IAM.Tests.ServicePolicyRuleTemplates;

/// <summary>
/// ServicePolicyRuleTemplatesGetResponseモデルのテスト
/// </summary>
public class ServicePolicyRuleTemplatesGetResponseTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = ServicePolicyRuleTemplatesGetResponse.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<ServicePolicyRuleTemplatesGetResponse>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() =>
            ServicePolicyRuleTemplatesGetResponse.CreateFromDiscriminatorValue(parseNode));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new ServicePolicyRuleTemplatesGetResponse();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.True(deserializers.ContainsKey("items"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ServicePolicyRuleTemplatesGetResponse();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new ServicePolicyRuleTemplatesGetResponse();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();
        var ruleTemplate = new RuleTemplate();

        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<RuleTemplate>>())
            .Returns(new List<RuleTemplate> { ruleTemplate });

        deserializers["items"](parseNode);

        Assert.NotNull(model.Items);
        Assert.Single(model.Items);
        Assert.Same(ruleTemplate, model.Items[0]);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAllProperties()
    {
        var jsonContent = @"{
            ""items"": [
                {
                    ""code"": ""abc"",
                    ""name"": ""Test Template"",
                    ""type"": ""boolean"",
                    ""description"": ""Test Description"",
                    ""default_value"": ""true"",
                    ""values"": []
                }
            ],
            ""page"": 1,
            ""per_page"": 10,
            ""total_pages"": 1,
            ""total_items"": 1
        }";

        using var serializeStream = new MemoryStream();
        using var streamWriter = new StreamWriter(serializeStream);
        await streamWriter.WriteAsync(jsonContent);
        await streamWriter.FlushAsync();

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(ServicePolicyRuleTemplatesGetResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Items);
        Assert.Single(deserializedModel.Items);
    }
}
