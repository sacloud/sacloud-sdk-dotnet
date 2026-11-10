using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.KMS.Models;

namespace Sacloud.Sdk.KMS.Tests.Models;

/// <summary>
/// WrappedCreateKeyResponseモデルのテスト
/// </summary>
public class WrappedCreateKeyResponseTests
{
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var result = WrappedCreateKeyResponse.CreateFromDiscriminatorValue(Substitute.For<IParseNode>());

        Assert.IsType<WrappedCreateKeyResponse>(result);
    }

    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() => WrappedCreateKeyResponse.CreateFromDiscriminatorValue(parseNode!));
    }

    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var deserializers = new WrappedCreateKeyResponse().GetFieldDeserializers();

        Assert.Equal(2, deserializers.Count);
        Assert.Contains("is_ok", deserializers.Keys);
        Assert.Contains("Key", deserializers.Keys);
    }

    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new WrappedCreateKeyResponse().Serialize(null!));
    }

    [Fact]
    public void GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new WrappedCreateKeyResponse();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetBoolValue().Returns(true);
        deserializers["is_ok"](parseNode);
        Assert.True(model.IsOk);

        var response = new CreateKeyResponse { Name = "test-create-key" };
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<CreateKeyResponse>>()).Returns(response);
        deserializers["Key"](parseNode);
        Assert.NotNull(model.Key);
        Assert.Equal("test-create-key", model.Key.Name);
    }

    [Fact]
    public async Task CanSerializeAndDeserializeResponseProperties()
    {
        var originalModel = new WrappedCreateKeyResponse
        {
            IsOk = true,
            Key = new CreateKeyResponse { Name = "key-name", Description = "key-description" }
        };
        using var stream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        await writer.GetSerializedContent().CopyToAsync(stream);

        stream.Position = 0;
        var parseNode = await new JsonParseNodeFactory().GetRootParseNodeAsync("application/json", stream);
        var result = parseNode.GetObjectValue(WrappedCreateKeyResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(result);
        Assert.True(result.IsOk);
        Assert.NotNull(result.Key);
        Assert.Equal(originalModel.Key.Name, result.Key.Name);
        Assert.Equal(originalModel.Key.Description, result.Key.Description);
    }

    [Fact]
    public async Task CanSerializeAndDeserializeAdditionalData()
    {
        var originalModel = new WrappedCreateKeyResponse
        {
            Key = new CreateKeyResponse { Name = "key-name" }
        };
        originalModel.AdditionalData.Add("custom_field", "value");
        using var stream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        await writer.GetSerializedContent().CopyToAsync(stream);

        stream.Position = 0;
        var parseNode = await new JsonParseNodeFactory().GetRootParseNodeAsync("application/json", stream);
        var result = parseNode.GetObjectValue(WrappedCreateKeyResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(result);
        Assert.Contains("custom_field", result.AdditionalData.Keys);
    }
}
