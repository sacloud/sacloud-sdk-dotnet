using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.KMS.Models;

namespace Sacloud.Sdk.KMS.Tests.Models;

/// <summary>
/// ScheduleDestructionKeyRequestモデルのテスト
/// </summary>
public class ScheduleDestructionKeyRequestTests
{
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var result = ScheduleDestructionKeyRequest.CreateFromDiscriminatorValue(Substitute.For<IParseNode>());

        Assert.IsType<ScheduleDestructionKeyRequest>(result);
    }

    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() => ScheduleDestructionKeyRequest.CreateFromDiscriminatorValue(parseNode!));
    }

    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var deserializers = new ScheduleDestructionKeyRequest().GetFieldDeserializers();

        Assert.Single(deserializers);
        Assert.Contains("PendingDays", deserializers.Keys);
    }

    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new ScheduleDestructionKeyRequest().Serialize(null!));
    }

    [Fact]
    public void GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new ScheduleDestructionKeyRequest();
        var parseNode = Substitute.For<IParseNode>();
        parseNode.GetIntValue().Returns(7);

        model.GetFieldDeserializers()["PendingDays"](parseNode);

        Assert.Equal(7, model.PendingDays);
    }

    [Theory]
    [InlineData(7)]
    [InlineData(14)]
    public async Task CanSerializeAndDeserializePendingDays(int pendingDays)
    {
        var originalModel = new ScheduleDestructionKeyRequest { PendingDays = pendingDays };
        using var stream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        await writer.GetSerializedContent().CopyToAsync(stream);

        stream.Position = 0;
        var parseNode = await new JsonParseNodeFactory().GetRootParseNodeAsync("application/json", stream);
        var result = parseNode.GetObjectValue(ScheduleDestructionKeyRequest.CreateFromDiscriminatorValue);

        Assert.NotNull(result);
        Assert.Equal(pendingDays, result.PendingDays);
    }

    [Fact]
    public async Task CanSerializeAndDeserializeAdditionalData()
    {
        var originalModel = new ScheduleDestructionKeyRequest { PendingDays = 10 };
        originalModel.AdditionalData.Add("custom_field", "value");
        using var stream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        await writer.GetSerializedContent().CopyToAsync(stream);

        stream.Position = 0;
        var parseNode = await new JsonParseNodeFactory().GetRootParseNodeAsync("application/json", stream);
        var result = parseNode.GetObjectValue(ScheduleDestructionKeyRequest.CreateFromDiscriminatorValue);

        Assert.NotNull(result);
        Assert.Contains("custom_field", result.AdditionalData.Keys);
    }
}
