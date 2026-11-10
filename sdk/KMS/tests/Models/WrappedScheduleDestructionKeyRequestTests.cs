using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.KMS.Models;
using System.Text.Json;

namespace Sacloud.Sdk.KMS.Tests.Models;

/// <summary>
/// WrappedScheduleDestructionKeyRequestモデルのテスト
/// </summary>
public class WrappedScheduleDestructionKeyRequestTests
{
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var result = WrappedScheduleDestructionKeyRequest.CreateFromDiscriminatorValue(Substitute.For<IParseNode>());

        Assert.IsType<WrappedScheduleDestructionKeyRequest>(result);
    }

    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() => WrappedScheduleDestructionKeyRequest.CreateFromDiscriminatorValue(parseNode!));
    }

    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var deserializers = new WrappedScheduleDestructionKeyRequest().GetFieldDeserializers();

        Assert.Single(deserializers);
        Assert.Contains("Key", deserializers.Keys);
    }

    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new WrappedScheduleDestructionKeyRequest().Serialize(null!));
    }

    [Fact]
    public void GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new WrappedScheduleDestructionKeyRequest();
        var parseNode = Substitute.For<IParseNode>();
        var request = new ScheduleDestructionKeyRequest { PendingDays = 10 };
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<ScheduleDestructionKeyRequest>>()).Returns(request);

        model.GetFieldDeserializers()["Key"](parseNode);

        Assert.NotNull(model.Key);
        Assert.Equal(10, model.Key.PendingDays);
    }

    [Fact]
    public async Task CanSerializeAndDeserializeKey()
    {
        var originalModel = new WrappedScheduleDestructionKeyRequest
        {
            Key = new ScheduleDestructionKeyRequest { PendingDays = 14 }
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
        Assert.Equal(new[] { "PendingDays" }, key.EnumerateObject().Select(property => property.Name));
        Assert.Equal(14, key.GetProperty("PendingDays").GetInt32());

        stream.Position = 0;
        var parseNode = await new JsonParseNodeFactory().GetRootParseNodeAsync("application/json", stream);
        var result = parseNode.GetObjectValue(WrappedScheduleDestructionKeyRequest.CreateFromDiscriminatorValue);

        Assert.NotNull(result);
        Assert.NotNull(result.Key);
        Assert.Equal(14, result.Key.PendingDays);
    }

    [Fact]
    public async Task CanSerializeAndDeserializeAdditionalData()
    {
        var originalModel = new WrappedScheduleDestructionKeyRequest
        {
            Key = new ScheduleDestructionKeyRequest { PendingDays = 7 }
        };
        originalModel.AdditionalData.Add("custom_field", "value");
        using var stream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        await writer.GetSerializedContent().CopyToAsync(stream);

        stream.Position = 0;
        var parseNode = await new JsonParseNodeFactory().GetRootParseNodeAsync("application/json", stream);
        var result = parseNode.GetObjectValue(WrappedScheduleDestructionKeyRequest.CreateFromDiscriminatorValue);

        Assert.NotNull(result);
        Assert.Contains("custom_field", result.AdditionalData.Keys);
    }
}
