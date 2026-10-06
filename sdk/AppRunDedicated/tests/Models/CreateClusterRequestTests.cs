using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunDedicated.Models;

namespace Sacloud.Sdk.AppRunDedicated.Tests.Models;

/// <summary>
/// CreateClusterRequestモデルのテスト
/// </summary>
public class CreateClusterRequestTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void CreateCluster_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = CreateClusterRequest.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<CreateClusterRequest>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateCluster_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() =>
            CreateClusterRequest.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void CreateCluster_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new CreateClusterRequest();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(4, deserializers.Count);
        Assert.True(deserializers.ContainsKey("letsEncryptEmail"));
        Assert.True(deserializers.ContainsKey("name"));
        Assert.True(deserializers.ContainsKey("ports"));
        Assert.True(deserializers.ContainsKey("servicePrincipalID"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateCluster_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new CreateClusterRequest();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void CreateCluster_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new CreateClusterRequest();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetStringValue().Returns("test@example.com");
        deserializers["letsEncryptEmail"](parseNode);
        Assert.Equal("test@example.com", model.LetsEncryptEmail);

        parseNode.GetStringValue().Returns("test-cluster");
        deserializers["name"](parseNode);
        Assert.Equal("test-cluster", model.Name);

        var portsList = new List<ClusterLoadBalancerPort>
        {
            new ClusterLoadBalancerPort { Port = 80 }
        };
        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<ClusterLoadBalancerPort>>())
            .Returns(portsList);
        deserializers["ports"](parseNode);
        Assert.NotNull(model.Ports);

        parseNode.GetStringValue().Returns("sp-123");
        deserializers["servicePrincipalID"](parseNode);
        Assert.Equal("sp-123", model.ServicePrincipalID);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CreateCluster_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new CreateClusterRequest
        {
            LetsEncryptEmail = "test@example.com",
            Name = "test-cluster",
            Ports = new List<ClusterLoadBalancerPort>
            {
                new ClusterLoadBalancerPort { Port = 80 }
            },
            ServicePrincipalID = "sp-123"
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(CreateClusterRequest.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.LetsEncryptEmail, deserializedModel.LetsEncryptEmail);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.NotNull(deserializedModel.Ports);
        Assert.Equal(originalModel.ServicePrincipalID, deserializedModel.ServicePrincipalID);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CreateCluster_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new CreateClusterRequest
        {
            Name = "test-cluster",
            ServicePrincipalID = "sp-123"
        };
        originalModel.AdditionalData.Add("custom_field", "custom_value");

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(CreateClusterRequest.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field"));
    }
}
