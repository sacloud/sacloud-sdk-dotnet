using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Domains;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Domains;

/// <summary>
/// DomainsPostResponseモデルのテスト
/// </summary>
public class DomainsPostResponseTests
{
    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void DomainsPostResponse_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new DomainsPostResponse();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("apigw"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void DomainsPostResponse_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new DomainsPostResponse();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void DomainsPostResponse_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = DomainsPostResponse.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<DomainsPostResponse>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void DomainsPostResponse_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;
        Assert.Throws<ArgumentNullException>(() =>
            DomainsPostResponse.CreateFromDiscriminatorValue(parseNode));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void DomainsPostResponse_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new DomainsPostResponse();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();
        var apigw = new DomainsPostResponse_apigw
        {
            Domain = new Domain { DomainName = "test-domain.example.com" }
        };
        parseNode.GetObjectValue(
            Arg.Any<ParsableFactory<DomainsPostResponse_apigw>>()).Returns(apigw);
        deserializers["apigw"](parseNode);
        Assert.Equal(apigw, model.Apigw);
    }

    /// <summary>
    /// すべてのプロパティとAdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task DomainsPostResponse_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new DomainsPostResponse
        {
            Apigw = new DomainsPostResponse_apigw
            {
                Domain = new Domain
                {
                    DomainName = "test-domain.example.com"
                }
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
        var deserializedModel = parseNode.GetObjectValue(DomainsPostResponse.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Apigw);
        Assert.NotNull(deserializedModel.Apigw.Domain);
        Assert.Equal(originalModel.Apigw.Domain.DomainName, deserializedModel.Apigw.Domain.DomainName);
        Assert.NotNull(deserializedModel.AdditionalData);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extraKey"));
    }

    /// <summary>
    /// AdditionalDataが初期化されることを確認
    /// </summary>
    [Fact]
    public void DomainsPostResponse_AdditionalData_IsInitialized()
    {
        var model = new DomainsPostResponse();
        Assert.NotNull(model.AdditionalData);
        Assert.Empty(model.AdditionalData);
    }
}
