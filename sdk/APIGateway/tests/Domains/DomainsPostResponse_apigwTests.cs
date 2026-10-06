using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Domains;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Domains;

/// <summary>
/// DomainsPostResponse_apigwモデルのテスト
/// </summary>
public class DomainsPostResponse_apigwTests
{
    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void DomainsPostResponse_apigw_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new DomainsPostResponse_apigw();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("domain"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void DomainsPostResponse_apigw_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new DomainsPostResponse_apigw();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void DomainsPostResponse_apigw_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = DomainsPostResponse_apigw.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<DomainsPostResponse_apigw>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void DomainsPostResponse_apigw_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;
        Assert.Throws<ArgumentNullException>(() =>
            DomainsPostResponse_apigw.CreateFromDiscriminatorValue(parseNode));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void DomainsPostResponse_apigw_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new DomainsPostResponse_apigw();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();
        var domain = new Domain { DomainName = "test-domain.example.com" };
        parseNode.GetObjectValue(
            Arg.Any<ParsableFactory<Domain>>()).Returns(domain);
        deserializers["domain"](parseNode);
        Assert.Equal(domain, model.Domain);
    }

    /// <summary>
    /// すべてのプロパティとAdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task DomainsPostResponse_apigw_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new DomainsPostResponse_apigw
        {
            Domain = new Domain
            {
                DomainName = "test-domain.example.com"
            },
            AdditionalData = new Dictionary<string, object>
            {
                { "extraKey", "extraValue" }
            }
        };
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(DomainsPostResponse_apigw.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Domain);
        Assert.Equal(originalModel.Domain.DomainName, deserializedModel.Domain.DomainName);
        Assert.NotNull(deserializedModel.AdditionalData);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extraKey"));
    }

    /// <summary>
    /// AdditionalDataが初期化されることを確認
    /// </summary>
    [Fact]
    public void DomainsPostResponse_apigw_AdditionalData_IsInitialized()
    {
        var model = new DomainsPostResponse_apigw();
        Assert.NotNull(model.AdditionalData);
        Assert.Empty(model.AdditionalData);
    }
}
