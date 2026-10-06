using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// ServiceDetailのテスト
/// </summary>
public class ServiceDetailTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = ServiceDetail.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<ServiceDetail>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ServiceDetail.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（親Service16+独自2=18キー）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new ServiceDetail();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Equal(18, deserializers.Count);
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
        Assert.True(deserializers.ContainsKey("corsConfig"));
        Assert.True(deserializers.ContainsKey("objectStorageConfig"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ServiceDetail();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証（独自プロパティのみ）
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new ServiceDetail();
        var deserializers = model.GetFieldDeserializers();
        var corsConfig = new CorsConfig();
        var objectStorageConfig = new ObjectStorageConfig();

        var corsConfigNode = Substitute.For<IParseNode>();
        corsConfigNode.GetObjectValue(Arg.Any<ParsableFactory<CorsConfig>>()).Returns(corsConfig);
        deserializers["corsConfig"](corsConfigNode);

        var objectStorageConfigNode = Substitute.For<IParseNode>();
        objectStorageConfigNode.GetObjectValue(Arg.Any<ParsableFactory<ObjectStorageConfig>>()).Returns(objectStorageConfig);
        deserializers["objectStorageConfig"](objectStorageConfigNode);

        var nameNode = Substitute.For<IParseNode>();
        nameNode.GetStringValue().Returns("service-detail");
        deserializers["name"](nameNode);

        Assert.Same(corsConfig, model.CorsConfig);
        Assert.Same(objectStorageConfig, model.ObjectStorageConfig);
        Assert.Equal("service-detail", model.Name);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティを保持できることを検証
    /// </summary>
    [Fact]
    public async Task ServiceDetail_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ServiceDetail
        {
            Authentication = Service_authentication.Oidc,
            ConnectTimeout = 10,
            CorsConfig = new CorsConfig(),
            Host = "backend.example.com",
            Name = "service-detail",
            ObjectStorageConfig = new ObjectStorageConfig(),
            Path = "/backend",
            Port = 443,
            Protocol = Service_protocol.Https,
            ReadTimeout = 20,
            Retries = 2,
            Tags = ["private"],
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
        var deserializedModel = parseNode.GetObjectValue(ServiceDetail.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Authentication, deserializedModel.Authentication);
        Assert.Equal(originalModel.ConnectTimeout, deserializedModel.ConnectTimeout);
        Assert.NotNull(deserializedModel.CorsConfig);
        Assert.Equal(originalModel.Host, deserializedModel.Host);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.NotNull(deserializedModel.ObjectStorageConfig);
        Assert.Equal(originalModel.Path, deserializedModel.Path);
        Assert.Equal(originalModel.Port, deserializedModel.Port);
        Assert.Equal(originalModel.Protocol, deserializedModel.Protocol);
        Assert.NotNull(deserializedModel.Tags);
        Assert.Single(deserializedModel.Tags);
        Assert.Equal("private", deserializedModel.Tags[0]);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティとAdditionalDataを保持できることを検証
    /// </summary>
    [Fact]
    public async Task ServiceDetail_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new ServiceDetail
        {
            Host = "backend.example.com",
            Name = "service-detail",
            CorsConfig = new CorsConfig(),
            ObjectStorageConfig = new ObjectStorageConfig(),
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
        var deserializedModel = parseNode.GetObjectValue(ServiceDetail.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Host, deserializedModel.Host);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.NotNull(deserializedModel.CorsConfig);
        Assert.NotNull(deserializedModel.ObjectStorageConfig);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
