using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// RouteDetailモデルのテスト
/// </summary>
public class RouteDetailTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = RouteDetail.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<RouteDetail>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => RouteDetail.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（親Route17キー+独自1=18キー）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new RouteDetail();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Equal(18, deserializers.Count);
        Assert.True(deserializers.ContainsKey("createdAt"));
        Assert.True(deserializers.ContainsKey("id"));
        Assert.True(deserializers.ContainsKey("updatedAt"));
        Assert.True(deserializers.ContainsKey("host"));
        Assert.True(deserializers.ContainsKey("hosts"));
        Assert.True(deserializers.ContainsKey("httpsRedirectStatusCode"));
        Assert.True(deserializers.ContainsKey("methods"));
        Assert.True(deserializers.ContainsKey("name"));
        Assert.True(deserializers.ContainsKey("path"));
        Assert.True(deserializers.ContainsKey("preserveHost"));
        Assert.True(deserializers.ContainsKey("protocols"));
        Assert.True(deserializers.ContainsKey("regexPriority"));
        Assert.True(deserializers.ContainsKey("requestBuffering"));
        Assert.True(deserializers.ContainsKey("responseBuffering"));
        Assert.True(deserializers.ContainsKey("serviceId"));
        Assert.True(deserializers.ContainsKey("stripPath"));
        Assert.True(deserializers.ContainsKey("tags"));
        Assert.True(deserializers.ContainsKey("ipRestrictionConfig"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new RouteDetail();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証（独自プロパティのみ）
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new RouteDetail();
        var deserializers = model.GetFieldDeserializers();
        var ipRestrictionConfig = new IpRestrictionConfig();

        var ipRestrictionConfigNode = Substitute.For<IParseNode>();
        ipRestrictionConfigNode.GetObjectValue(Arg.Any<ParsableFactory<IpRestrictionConfig>>()).Returns(ipRestrictionConfig);
        deserializers["ipRestrictionConfig"](ipRestrictionConfigNode);

        var nameNode = Substitute.For<IParseNode>();
        nameNode.GetStringValue().Returns("route-detail");
        deserializers["name"](nameNode);

        Assert.Same(ipRestrictionConfig, model.IpRestrictionConfig);
        Assert.Equal("route-detail", model.Name);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティを保持できることを検証
    /// </summary>
    [Fact]
    public async Task RouteDetail_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new RouteDetail
        {
            Hosts = ["api.example.com"],
            HttpsRedirectStatusCode = 308,
            IpRestrictionConfig = new IpRestrictionConfig(),
            Methods = [HTTPMethod.GET, HTTPMethod.POST],
            Name = "route-1",
            Path = "/v1",
            PreserveHost = true,
            Protocols = Route_protocols.Https,
            RegexPriority = 1,
            RequestBuffering = true,
            ResponseBuffering = true,
            StripPath = false,
            Tags = ["public"],
        };
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(RouteDetail.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Hosts);
        Assert.Single(deserializedModel.Hosts);
        Assert.Equal("api.example.com", deserializedModel.Hosts[0]);
        Assert.Equal(308, deserializedModel.HttpsRedirectStatusCode);
        Assert.NotNull(deserializedModel.IpRestrictionConfig);
        Assert.NotNull(deserializedModel.Methods);
        Assert.Equal(2, deserializedModel.Methods.Count);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.Equal(originalModel.Path, deserializedModel.Path);
        Assert.True(deserializedModel.PreserveHost);
        Assert.Equal(originalModel.Protocols, deserializedModel.Protocols);
        Assert.Equal(1, deserializedModel.RegexPriority);
        Assert.True(deserializedModel.RequestBuffering);
        Assert.True(deserializedModel.ResponseBuffering);
        Assert.False(deserializedModel.StripPath);
        Assert.NotNull(deserializedModel.Tags);
        Assert.Single(deserializedModel.Tags);
        Assert.Equal("public", deserializedModel.Tags[0]);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティとAdditionalDataを保持できることを検証
    /// </summary>
    [Fact]
    public async Task RouteDetail_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new RouteDetail
        {
            Name = "route-1",
            Path = "/v1",
            IpRestrictionConfig = new IpRestrictionConfig(),
        };
        originalModel.AdditionalData["customField"] = "customValue";
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(RouteDetail.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.Equal(originalModel.Path, deserializedModel.Path);
        Assert.NotNull(deserializedModel.IpRestrictionConfig);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
