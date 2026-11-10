using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.KMS.Models;

namespace Sacloud.Sdk.KMS.Tests.Models;

/// <summary>
/// ChangeKeyStateRequestモデルのテスト
/// </summary>
public class ChangeKeyStateRequestTests
{
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var result = ChangeKeyStateRequest.CreateFromDiscriminatorValue(Substitute.For<IParseNode>());

        Assert.IsType<ChangeKeyStateRequest>(result);
    }

    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() => ChangeKeyStateRequest.CreateFromDiscriminatorValue(parseNode!));
    }

    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var deserializers = new ChangeKeyStateRequest().GetFieldDeserializers();

        Assert.Single(deserializers);
        Assert.Contains("Status", deserializers.Keys);
    }

    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new ChangeKeyStateRequest().Serialize(null!));
    }

    [Fact]
    public void GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new ChangeKeyStateRequest();
        var parseNode = Substitute.For<IParseNode>();
        parseNode.GetEnumValue<ChangeKeyStateRequest_Status>().Returns(ChangeKeyStateRequest_Status.Active);

        model.GetFieldDeserializers()["Status"](parseNode);

        Assert.Equal(ChangeKeyStateRequest_Status.Active, model.Status);
    }

    [Fact]
    public async Task CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ChangeKeyStateRequest { Status = ChangeKeyStateRequest_Status.Suspended };
        using var stream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        await writer.GetSerializedContent().CopyToAsync(stream);

        stream.Position = 0;
        var parseNode = await new JsonParseNodeFactory().GetRootParseNodeAsync("application/json", stream);
        var result = parseNode.GetObjectValue(ChangeKeyStateRequest.CreateFromDiscriminatorValue);

        Assert.NotNull(result);
        Assert.Equal(originalModel.Status, result.Status);
    }

    [Fact]
    public async Task CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new ChangeKeyStateRequest { Status = ChangeKeyStateRequest_Status.Restricted };
        originalModel.AdditionalData.Add("custom_field", "value");
        using var stream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        await writer.GetSerializedContent().CopyToAsync(stream);

        stream.Position = 0;
        var parseNode = await new JsonParseNodeFactory().GetRootParseNodeAsync("application/json", stream);
        var result = parseNode.GetObjectValue(ChangeKeyStateRequest.CreateFromDiscriminatorValue);

        Assert.NotNull(result);
        Assert.Contains("custom_field", result.AdditionalData.Keys);
    }
}
