using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// Routeモデルのテスト
/// </summary>
public class RouteTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = Route.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<Route>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Route.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（親3+独自14=17キー）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new Route();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Equal(17, deserializers.Count);
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
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new Route();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new Route();
        var deserializers = model.GetFieldDeserializers();
        var testId = new Guid("12345678-1234-1234-1234-123456789012");
        var serviceId = Guid.NewGuid();
        var hosts = new[] { "example.com", "api.example.com" };
        var methods = new List<HTTPMethod?> { HTTPMethod.GET, HTTPMethod.POST };
        var tags = new[] { "public", "v1" };

        var parseNodeCreatedAt = Substitute.For<IParseNode>();
        parseNodeCreatedAt.GetDateTimeOffsetValue().Returns(new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero));
        deserializers["createdAt"](parseNodeCreatedAt);

        var parseNodeId = Substitute.For<IParseNode>();
        parseNodeId.GetGuidValue().Returns(testId);
        deserializers["id"](parseNodeId);

        var parseNodeUpdatedAt = Substitute.For<IParseNode>();
        parseNodeUpdatedAt.GetDateTimeOffsetValue().Returns(new DateTimeOffset(2024, 1, 2, 0, 0, 0, TimeSpan.Zero));
        deserializers["updatedAt"](parseNodeUpdatedAt);

        var hostNode = Substitute.For<IParseNode>();
        hostNode.GetStringValue().Returns("generated.example.com");
        deserializers["host"](hostNode);

        var hostsNode = Substitute.For<IParseNode>();
        hostsNode.GetCollectionOfPrimitiveValues<string>().Returns(hosts);
        deserializers["hosts"](hostsNode);

        var httpsRedirectStatusCodeNode = Substitute.For<IParseNode>();
        httpsRedirectStatusCodeNode.GetIntValue().Returns(301);
        deserializers["httpsRedirectStatusCode"](httpsRedirectStatusCodeNode);

        var methodsNode = Substitute.For<IParseNode>();
        methodsNode.GetCollectionOfEnumValues<HTTPMethod>().Returns(methods);
        deserializers["methods"](methodsNode);

        var nameNode = Substitute.For<IParseNode>();
        nameNode.GetStringValue().Returns("route-1");
        deserializers["name"](nameNode);

        var pathNode = Substitute.For<IParseNode>();
        pathNode.GetStringValue().Returns("/api/v1");
        deserializers["path"](pathNode);

        var preserveHostNode = Substitute.For<IParseNode>();
        preserveHostNode.GetBoolValue().Returns(true);
        deserializers["preserveHost"](preserveHostNode);

        var protocolsNode = Substitute.For<IParseNode>();
        protocolsNode.GetEnumValue<Route_protocols>().Returns(Route_protocols.Https);
        deserializers["protocols"](protocolsNode);

        var regexPriorityNode = Substitute.For<IParseNode>();
        regexPriorityNode.GetIntValue().Returns(1);
        deserializers["regexPriority"](regexPriorityNode);

        var requestBufferingNode = Substitute.For<IParseNode>();
        requestBufferingNode.GetBoolValue().Returns(true);
        deserializers["requestBuffering"](requestBufferingNode);

        var responseBufferingNode = Substitute.For<IParseNode>();
        responseBufferingNode.GetBoolValue().Returns(false);
        deserializers["responseBuffering"](responseBufferingNode);

        var serviceIdNode = Substitute.For<IParseNode>();
        serviceIdNode.GetGuidValue().Returns(serviceId);
        deserializers["serviceId"](serviceIdNode);

        var stripPathNode = Substitute.For<IParseNode>();
        stripPathNode.GetBoolValue().Returns(true);
        deserializers["stripPath"](stripPathNode);

        var tagsNode = Substitute.For<IParseNode>();
        tagsNode.GetCollectionOfPrimitiveValues<string>().Returns(tags);
        deserializers["tags"](tagsNode);

        Assert.Equal(new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), model.CreatedAt);
        Assert.Equal(testId, model.Id);
        Assert.Equal(new DateTimeOffset(2024, 1, 2, 0, 0, 0, TimeSpan.Zero), model.UpdatedAt);
        Assert.Equal("generated.example.com", model.Host);
        Assert.NotNull(model.Hosts);
        Assert.Equal(2, model.Hosts.Count);
        Assert.Equal(301, model.HttpsRedirectStatusCode);
        Assert.NotNull(model.Methods);
        Assert.Equal(2, model.Methods.Count);
        Assert.Equal("route-1", model.Name);
        Assert.Equal("/api/v1", model.Path);
        Assert.True(model.PreserveHost);
        Assert.Equal(Route_protocols.Https, model.Protocols);
        Assert.Equal(1, model.RegexPriority);
        Assert.True(model.RequestBuffering);
        Assert.False(model.ResponseBuffering);
        Assert.Equal(serviceId, model.ServiceId);
        Assert.True(model.StripPath);
        Assert.NotNull(model.Tags);
        Assert.Equal(2, model.Tags.Count);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティを保持できることを検証
    /// </summary>
    [Fact]
    public async Task Route_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new Route
        {
            Hosts = ["example.com"],
            HttpsRedirectStatusCode = 301,
            Methods = [HTTPMethod.GET, HTTPMethod.POST],
            Name = "route-1",
            Path = "/api/v1",
            PreserveHost = true,
            Protocols = Route_protocols.Https,
            RegexPriority = 1,
            RequestBuffering = true,
            ResponseBuffering = false,
            StripPath = true,
            Tags = ["public", "v1"],
        };
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Route.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Hosts);
        Assert.Single(deserializedModel.Hosts);
        Assert.Equal("example.com", deserializedModel.Hosts[0]);
        Assert.Equal(301, deserializedModel.HttpsRedirectStatusCode);
        Assert.NotNull(deserializedModel.Methods);
        Assert.Equal(2, deserializedModel.Methods.Count);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.Equal(originalModel.Path, deserializedModel.Path);
        Assert.True(deserializedModel.PreserveHost);
        Assert.Equal(originalModel.Protocols, deserializedModel.Protocols);
        Assert.Equal(1, deserializedModel.RegexPriority);
        Assert.True(deserializedModel.RequestBuffering);
        Assert.False(deserializedModel.ResponseBuffering);
        Assert.True(deserializedModel.StripPath);
        Assert.NotNull(deserializedModel.Tags);
        Assert.Equal(2, deserializedModel.Tags.Count);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティとAdditionalDataを保持できることを検証
    /// </summary>
    [Fact]
    public async Task Route_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new Route
        {
            Hosts = ["example.com"],
            Name = "route-1",
            Path = "/api/v1",
            Protocols = Route_protocols.Https,
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
        var deserializedModel = parseNode.GetObjectValue(Route.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.Equal(originalModel.Path, deserializedModel.Path);
        Assert.Equal(originalModel.Protocols, deserializedModel.Protocols);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
