using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Models.Model;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models.Model;

/// <summary>
/// Replicationモデルのテスト
/// </summary>
public class ReplicationTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = Replication.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<Replication>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() =>
            Replication.CreateFromDiscriminatorValue(parseNode));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいJSONフィールド名キーを返すことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectKeys()
    {
        var model = new Replication();

        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.True(deserializers.ContainsKey("config_status"));
        Assert.True(deserializers.ContainsKey("created_at"));
        Assert.True(deserializers.ContainsKey("dest_bucket"));
        Assert.True(deserializers.ContainsKey("source_bucket"));
    }

    /// <summary>
    /// SerializeがnullのwriterでArgumentNullExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new Replication();

        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersの各アクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new Replication();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();
        var createdAt = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var destination = new Replication_dest_bucket
        {
            ClusterId = "dest-cluster-1",
            Name = "dest-bucket-1"
        };
        var source = new Replication_source_bucket
        {
            ClusterId = "src-cluster-1",
            Name = "src-bucket-1"
        };
        parseNode.GetEnumValue<Replication_config_status>().Returns(Replication_config_status.Created);
        parseNode.GetStringValue().Returns("bucket-dest");
        parseNode.GetDateTimeOffsetValue().Returns(createdAt);
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<Replication_dest_bucket>>()).Returns(destination);
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<Replication_source_bucket>>()).Returns(source);

        deserializers["config_status"](parseNode);
        deserializers["created_at"](parseNode);
        deserializers["dest_bucket"](parseNode);
        deserializers["source_bucket"](parseNode);

        Assert.Equal(Replication_config_status.Created, model.ConfigStatus);
        Assert.Equal(createdAt, model.CreatedAt);
        Assert.Same(destination, model.DestBucket);
        Assert.Same(source, model.SourceBucket);
    }

    /// <summary>
    /// AdditionalDataが初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_AdditionalDataInitialized()
    {
        var model = new Replication();

        Assert.NotNull(model.AdditionalData);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task Replication_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new Replication
        {
            ConfigStatus = Replication_config_status.Created,
            CreatedAt = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero),
            DestBucket = new Replication_dest_bucket
            {
                ClusterId = "dest-cluster-1",
                Name = "dest-bucket-1",
                Plan = new Replication_dest_bucket_plan
                {
                    ServiceClassPath = "dest-plan-1",
                    Type = PlanType.Standard
                }
            },
            SourceBucket = new Replication_source_bucket
            {
                ClusterId = "src-cluster-1",
                Name = "src-bucket-1",
                Plan = new Replication_source_bucket_plan
                {
                    ServiceClassPath = "src-plan-1",
                    Type = PlanType.Standard
                }
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
        var deserializedModel = parseNode.GetObjectValue(Replication.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.ConfigStatus, deserializedModel.ConfigStatus);
        Assert.Equal(originalModel.CreatedAt, deserializedModel.CreatedAt);
        Assert.NotNull(deserializedModel.DestBucket);
        Assert.Equal(originalModel.DestBucket.ClusterId, deserializedModel.DestBucket.ClusterId);
        Assert.Equal(originalModel.DestBucket.Name, deserializedModel.DestBucket.Name);
        Assert.NotNull(deserializedModel.DestBucket.Plan);
        Assert.Equal(originalModel.DestBucket.Plan.ServiceClassPath, deserializedModel.DestBucket.Plan.ServiceClassPath);
        Assert.Equal(originalModel.DestBucket.Plan.Type, deserializedModel.DestBucket.Plan.Type);
        Assert.NotNull(deserializedModel.SourceBucket);
        Assert.Equal(originalModel.SourceBucket.ClusterId, deserializedModel.SourceBucket.ClusterId);
        Assert.Equal(originalModel.SourceBucket.Name, deserializedModel.SourceBucket.Name);
        Assert.NotNull(deserializedModel.SourceBucket.Plan);
        Assert.Equal(originalModel.SourceBucket.Plan.ServiceClassPath, deserializedModel.SourceBucket.Plan.ServiceClassPath);
        Assert.Equal(originalModel.SourceBucket.Plan.Type, deserializedModel.SourceBucket.Plan.Type);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task Replication_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new Replication
        {
            AdditionalData = new Dictionary<string, object>
            {
                { "extra_key", "extra-value" }
            }
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, model);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Replication.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
