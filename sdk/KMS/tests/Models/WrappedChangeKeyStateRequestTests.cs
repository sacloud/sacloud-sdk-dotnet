using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.KMS.Models;
using System.Text.Json;

namespace Sacloud.Sdk.KMS.Tests.Models;

/// <summary>
/// WrappedChangeKeyStateRequestモデルのテスト
/// </summary>
public class WrappedChangeKeyStateRequestTests
{
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var result = WrappedChangeKeyStateRequest.CreateFromDiscriminatorValue(Substitute.For<IParseNode>());

        Assert.IsType<WrappedChangeKeyStateRequest>(result);
    }

    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() => WrappedChangeKeyStateRequest.CreateFromDiscriminatorValue(parseNode!));
    }

    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var deserializers = new WrappedChangeKeyStateRequest().GetFieldDeserializers();

        Assert.Single(deserializers);
        Assert.Contains("Key", deserializers.Keys);
    }

    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new WrappedChangeKeyStateRequest().Serialize(null!));
    }

    [Fact]
    public void GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new WrappedChangeKeyStateRequest();
        var parseNode = Substitute.For<IParseNode>();
        var request = new ChangeKeyStateRequest { Status = ChangeKeyStateRequest_Status.Active };
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<ChangeKeyStateRequest>>()).Returns(request);

        model.GetFieldDeserializers()["Key"](parseNode);

        Assert.NotNull(model.Key);
        Assert.Equal(ChangeKeyStateRequest_Status.Active, model.Key.Status);
    }

    [Fact]
    public async Task CanSerializeAndDeserializeKey()
    {
        var originalModel = new WrappedChangeKeyStateRequest
        {
            Key = new ChangeKeyStateRequest { Status = ChangeKeyStateRequest_Status.Suspended }
        };
        using var stream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        await writer.GetSerializedContent().CopyToAsync(stream);

        stream.Position = 0;
        using var jsonDocument = await JsonDocument.ParseAsync(stream);
        var root = jsonDocument.RootElement;
        Assert.Equal(new[] { "Key" }, root.EnumerateObject().Select(property => property.Name));
        var key = root.GetProperty("Key");
        Assert.Equal(new[] { "Status" }, key.EnumerateObject().Select(property => property.Name));
        Assert.Equal("suspended", key.GetProperty("Status").GetString());

        stream.Position = 0;
        var parseNode = await new JsonParseNodeFactory().GetRootParseNodeAsync("application/json", stream);
        var result = parseNode.GetObjectValue(WrappedChangeKeyStateRequest.CreateFromDiscriminatorValue);

        Assert.NotNull(result);
        Assert.NotNull(result.Key);
        Assert.Equal(originalModel.Key.Status, result.Key.Status);
    }

    [Fact]
    public async Task CanSerializeAndDeserializeAdditionalData()
    {
        var originalModel = new WrappedChangeKeyStateRequest
        {
            Key = new ChangeKeyStateRequest { Status = ChangeKeyStateRequest_Status.Restricted }
        };
        originalModel.AdditionalData.Add("custom_field", "value");
        using var stream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        await writer.GetSerializedContent().CopyToAsync(stream);

        stream.Position = 0;
        var parseNode = await new JsonParseNodeFactory().GetRootParseNodeAsync("application/json", stream);
        var result = parseNode.GetObjectValue(WrappedChangeKeyStateRequest.CreateFromDiscriminatorValue);

        Assert.NotNull(result);
        Assert.Contains("custom_field", result.AdditionalData.Keys);
    }
}
