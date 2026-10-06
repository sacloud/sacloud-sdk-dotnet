using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// ObjectStorageConfigのテスト
/// </summary>
public class ObjectStorageConfigTests
{
    /// <summary>
    /// コンストラクタがAdditionalDataを空辞書で初期化することを検証
    /// </summary>
    [Fact]
    public void Constructor_InitializesAdditionalData()
    {
        var model = new ObjectStorageConfig();
        Assert.NotNull(model.AdditionalData);
        Assert.Empty(model.AdditionalData);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = ObjectStorageConfig.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<ObjectStorageConfig>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ObjectStorageConfig.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"accessKeyID", "bucketName", "endpoint", "folderName", "region", "secretAccessKey", "useDocumentIndex"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new ObjectStorageConfig();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.Equal(7, deserializers.Count);
        Assert.True(deserializers.ContainsKey("accessKeyID"));
        Assert.True(deserializers.ContainsKey("bucketName"));
        Assert.True(deserializers.ContainsKey("endpoint"));
        Assert.True(deserializers.ContainsKey("folderName"));
        Assert.True(deserializers.ContainsKey("region"));
        Assert.True(deserializers.ContainsKey("secretAccessKey"));
        Assert.True(deserializers.ContainsKey("useDocumentIndex"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ObjectStorageConfig();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new ObjectStorageConfig();
        var deserializers = model.GetFieldDeserializers();

        var accessKeyIdNode = Substitute.For<IParseNode>();
        accessKeyIdNode.GetStringValue().Returns("example_access_key_id");
        deserializers["accessKeyID"](accessKeyIdNode);

        var bucketNameNode = Substitute.For<IParseNode>();
        bucketNameNode.GetStringValue().Returns("example_bucket_name");
        deserializers["bucketName"](bucketNameNode);

        var endpointNode = Substitute.For<IParseNode>();
        endpointNode.GetStringValue().Returns("https://example.com");
        deserializers["endpoint"](endpointNode);

        var folderNameNode = Substitute.For<IParseNode>();
        folderNameNode.GetStringValue().Returns("example_folder_name");
        deserializers["folderName"](folderNameNode);

        var regionNode = Substitute.For<IParseNode>();
        regionNode.GetStringValue().Returns("example_region");
        deserializers["region"](regionNode);

        var secretAccessKeyNode = Substitute.For<IParseNode>();
        secretAccessKeyNode.GetStringValue().Returns("example_password");
        deserializers["secretAccessKey"](secretAccessKeyNode);

        var useDocumentIndexNode = Substitute.For<IParseNode>();
        useDocumentIndexNode.GetBoolValue().Returns(true);
        deserializers["useDocumentIndex"](useDocumentIndexNode);

        Assert.Equal("example_access_key_id", model.AccessKeyID);
        Assert.Equal("example_bucket_name", model.BucketName);
        Assert.Equal("https://example.com", model.Endpoint);
        Assert.Equal("example_folder_name", model.FolderName);
        Assert.Equal("example_region", model.Region);
        Assert.Equal("example_password", model.SecretAccessKey);
        Assert.True(model.UseDocumentIndex);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティを保持できることを検証
    /// </summary>
    [Fact]
    public async Task ObjectStorageConfig_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ObjectStorageConfig
        {
            AccessKeyID = "example_access_key_id",
            BucketName = "example_bucket_name",
            Endpoint = "https://example.com",
            FolderName = "example_folder_name",
            Region = "example_region",
            SecretAccessKey = "example_password",
            UseDocumentIndex = true,
        };
        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(ObjectStorageConfig.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.AccessKeyID, deserializedModel.AccessKeyID);
        Assert.Equal(originalModel.BucketName, deserializedModel.BucketName);
        Assert.Equal(originalModel.Endpoint, deserializedModel.Endpoint);
        Assert.Equal(originalModel.FolderName, deserializedModel.FolderName);
        Assert.Equal(originalModel.Region, deserializedModel.Region);
        Assert.Equal(originalModel.SecretAccessKey, deserializedModel.SecretAccessKey);
        Assert.Equal(originalModel.UseDocumentIndex, deserializedModel.UseDocumentIndex);
    }

    /// <summary>
    /// シリアライズとデシリアライズで主要プロパティとAdditionalDataを保持できることを検証
    /// </summary>
    [Fact]
    public async Task ObjectStorageConfig_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new ObjectStorageConfig
        {
            AccessKeyID = "example_access_key_id",
            BucketName = "example_bucket_name",
            Endpoint = "https://example.com",
            FolderName = "example_folder_name",
            Region = "example_region",
            SecretAccessKey = "example_password",
            UseDocumentIndex = true,
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
        var deserializedModel = parseNode.GetObjectValue(ObjectStorageConfig.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.AccessKeyID, deserializedModel.AccessKeyID);
        Assert.Equal(originalModel.BucketName, deserializedModel.BucketName);
        Assert.Equal(originalModel.Endpoint, deserializedModel.Endpoint);
        Assert.Equal(originalModel.FolderName, deserializedModel.FolderName);
        Assert.Equal(originalModel.Region, deserializedModel.Region);
        Assert.Equal(originalModel.SecretAccessKey, deserializedModel.SecretAccessKey);
        Assert.Equal(originalModel.UseDocumentIndex, deserializedModel.UseDocumentIndex);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
