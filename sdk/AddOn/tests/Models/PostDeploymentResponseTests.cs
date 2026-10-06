using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;

namespace Sacloud.Sdk.AddOn.Tests.Models;

/// <summary>
/// PostDeploymentResponseモデルのテスト
/// </summary>
public class PostDeploymentResponseTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void PostDeploymentResponse_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new global::Sacloud.Sdk.AddOn.Models.PostDeploymentResponse();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("deploymentName"));
        Assert.True(deserializers.ContainsKey("resourceGroupName"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void PostDeploymentResponse_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new global::Sacloud.Sdk.AddOn.Models.PostDeploymentResponse();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void PostDeploymentResponse_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new global::Sacloud.Sdk.AddOn.Models.PostDeploymentResponse();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("deploymentName"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["deploymentName"](parseNode);
        Assert.NotNull(model.DeploymentName);

        Assert.True(deserializers.ContainsKey("resourceGroupName"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["resourceGroupName"](parseNode);
        Assert.NotNull(model.ResourceGroupName);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task PostDeploymentResponse_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new global::Sacloud.Sdk.AddOn.Models.PostDeploymentResponse();
        originalModel.DeploymentName = "sample";
        originalModel.ResourceGroupName = "sample";

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue<global::Sacloud.Sdk.AddOn.Models.PostDeploymentResponse>(global::Sacloud.Sdk.AddOn.Models.PostDeploymentResponse.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.DeploymentName, deserializedModel!.DeploymentName);
        Assert.Equal(originalModel.ResourceGroupName, deserializedModel!.ResourceGroupName);
    }
}
