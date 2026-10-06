using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// Serviceモデルのテスト
/// </summary>
public class ServiceTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = Service.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<Service>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Service.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（親3+独自13=16キー）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new Service();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Equal(16, deserializers.Count);
        Assert.True(deserializers.ContainsKey("createdAt"));
        Assert.True(deserializers.ContainsKey("id"));
        Assert.True(deserializers.ContainsKey("updatedAt"));
        Assert.True(deserializers.ContainsKey("authentication"));
        Assert.True(deserializers.ContainsKey("connectTimeout"));
        Assert.True(deserializers.ContainsKey("host"));
        Assert.True(deserializers.ContainsKey("name"));
        Assert.True(deserializers.ContainsKey("oidc"));
        Assert.True(deserializers.ContainsKey("path"));
        Assert.True(deserializers.ContainsKey("port"));
        Assert.True(deserializers.ContainsKey("protocol"));
        Assert.True(deserializers.ContainsKey("readTimeout"));
        Assert.True(deserializers.ContainsKey("retries"));
        Assert.True(deserializers.ContainsKey("routeHost"));
        Assert.True(deserializers.ContainsKey("tags"));
        Assert.True(deserializers.ContainsKey("writeTimeout"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new Service();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new Service();
        var deserializers = model.GetFieldDeserializers();
        var testId = new Guid("12345678-1234-1234-1234-123456789012");
        var oidc = new OidcSummary();
        var tags = new[] { "tag-1", "tag-2" };

        var parseNodeCreatedAt = Substitute.For<IParseNode>();
        parseNodeCreatedAt.GetDateTimeOffsetValue().Returns(new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero));
        deserializers["createdAt"](parseNodeCreatedAt);

        var parseNodeId = Substitute.For<IParseNode>();
        parseNodeId.GetGuidValue().Returns(testId);
        deserializers["id"](parseNodeId);

        var parseNodeUpdatedAt = Substitute.For<IParseNode>();
        parseNodeUpdatedAt.GetDateTimeOffsetValue().Returns(new DateTimeOffset(2024, 1, 2, 0, 0, 0, TimeSpan.Zero));
        deserializers["updatedAt"](parseNodeUpdatedAt);

        var authenticationNode = Substitute.For<IParseNode>();
        authenticationNode.GetEnumValue<Service_authentication>().Returns(Service_authentication.Oidc);
        deserializers["authentication"](authenticationNode);

        var connectTimeoutNode = Substitute.For<IParseNode>();
        connectTimeoutNode.GetIntValue().Returns(10);
        deserializers["connectTimeout"](connectTimeoutNode);

        var hostNode = Substitute.For<IParseNode>();
        hostNode.GetStringValue().Returns("backend.example.com");
        deserializers["host"](hostNode);

        var nameNode = Substitute.For<IParseNode>();
        nameNode.GetStringValue().Returns("service-1");
        deserializers["name"](nameNode);

        var oidcNode = Substitute.For<IParseNode>();
        oidcNode.GetObjectValue(Arg.Any<ParsableFactory<OidcSummary>>()).Returns(oidc);
        deserializers["oidc"](oidcNode);

        var pathNode = Substitute.For<IParseNode>();
        pathNode.GetStringValue().Returns("/backend");
        deserializers["path"](pathNode);

        var portNode = Substitute.For<IParseNode>();
        portNode.GetIntValue().Returns(443);
        deserializers["port"](portNode);

        var protocolNode = Substitute.For<IParseNode>();
        protocolNode.GetEnumValue<Service_protocol>().Returns(Service_protocol.Https);
        deserializers["protocol"](protocolNode);

        var readTimeoutNode = Substitute.For<IParseNode>();
        readTimeoutNode.GetIntValue().Returns(20);
        deserializers["readTimeout"](readTimeoutNode);

        var retriesNode = Substitute.For<IParseNode>();
        retriesNode.GetIntValue().Returns(2);
        deserializers["retries"](retriesNode);

        var routeHostNode = Substitute.For<IParseNode>();
        routeHostNode.GetStringValue().Returns("service.example.com");
        deserializers["routeHost"](routeHostNode);

        var tagsNode = Substitute.For<IParseNode>();
        tagsNode.GetCollectionOfPrimitiveValues<string>().Returns(tags);
        deserializers["tags"](tagsNode);

        var writeTimeoutNode = Substitute.For<IParseNode>();
        writeTimeoutNode.GetIntValue().Returns(30);
        deserializers["writeTimeout"](writeTimeoutNode);

        Assert.Equal(new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), model.CreatedAt);
        Assert.Equal(testId, model.Id);
        Assert.Equal(new DateTimeOffset(2024, 1, 2, 0, 0, 0, TimeSpan.Zero), model.UpdatedAt);
        Assert.Equal(Service_authentication.Oidc, model.Authentication);
        Assert.Equal(10, model.ConnectTimeout);
        Assert.Equal("backend.example.com", model.Host);
        Assert.Equal("service-1", model.Name);
        Assert.Same(oidc, model.Oidc);
        Assert.Equal("/backend", model.Path);
        Assert.Equal(443, model.Port);
        Assert.Equal(Service_protocol.Https, model.Protocol);
        Assert.Equal(20, model.ReadTimeout);
        Assert.Equal(2, model.Retries);
        Assert.Equal("service.example.com", model.RouteHost);
        Assert.NotNull(model.Tags);
        Assert.Equal(2, model.Tags.Count);
        Assert.Equal("tag-1", model.Tags[0]);
        Assert.Equal("tag-2", model.Tags[1]);
        Assert.Equal(30, model.WriteTimeout);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティを保持できることを検証
    /// </summary>
    [Fact]
    public async Task Service_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new Service
        {
            Authentication = Service_authentication.Basic,
            ConnectTimeout = 10,
            Host = "backend.example.com",
            Name = "service-1",
            Path = "/backend",
            Port = 443,
            Protocol = Service_protocol.Https,
            ReadTimeout = 20,
            Retries = 2,
            Tags = ["tag-1", "tag-2"],
            WriteTimeout = 30,
        };
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Service.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Authentication, deserializedModel.Authentication);
        Assert.Equal(originalModel.ConnectTimeout, deserializedModel.ConnectTimeout);
        Assert.Equal(originalModel.Host, deserializedModel.Host);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.Equal(originalModel.Path, deserializedModel.Path);
        Assert.Equal(originalModel.Port, deserializedModel.Port);
        Assert.Equal(originalModel.Protocol, deserializedModel.Protocol);
        Assert.Equal(originalModel.ReadTimeout, deserializedModel.ReadTimeout);
        Assert.Equal(originalModel.Retries, deserializedModel.Retries);
        Assert.Equal(originalModel.WriteTimeout, deserializedModel.WriteTimeout);
        Assert.NotNull(deserializedModel.Tags);
        Assert.Equal(2, deserializedModel.Tags.Count);
        Assert.Equal("tag-1", deserializedModel.Tags[0]);
        Assert.Equal("tag-2", deserializedModel.Tags[1]);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティとAdditionalDataを保持できることを検証
    /// </summary>
    [Fact]
    public async Task Service_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new Service
        {
            Authentication = Service_authentication.Basic,
            ConnectTimeout = 10,
            Host = "backend.example.com",
            Name = "service-1",
            Path = "/backend",
            Port = 443,
            Protocol = Service_protocol.Https,
            ReadTimeout = 20,
            Retries = 2,
            Tags = ["tag-1", "tag-2"],
            WriteTimeout = 30,
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
        var deserializedModel = parseNode.GetObjectValue(Service.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.Equal(originalModel.Host, deserializedModel.Host);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
