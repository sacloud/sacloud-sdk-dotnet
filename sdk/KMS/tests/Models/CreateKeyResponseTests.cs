using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.KMS.Models;
using System.Text;

namespace Sacloud.Sdk.KMS.Tests.Models;

/// <summary>
/// CreateKeyResponseモデルのテスト
/// </summary>
public class CreateKeyResponseTests
{
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var result = CreateKeyResponse.CreateFromDiscriminatorValue(Substitute.For<IParseNode>());

        Assert.IsType<CreateKeyResponse>(result);
    }

    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() => CreateKeyResponse.CreateFromDiscriminatorValue(parseNode!));
    }

    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var deserializers = new CreateKeyResponse().GetFieldDeserializers();

        Assert.Equal(11, deserializers.Count);
        Assert.Contains("CreatedAt", deserializers.Keys);
        Assert.Contains("Description", deserializers.Keys);
        Assert.Contains("ID", deserializers.Keys);
        Assert.Contains("KeyOrigin", deserializers.Keys);
        Assert.Contains("ModifiedAt", deserializers.Keys);
        Assert.Contains("Name", deserializers.Keys);
        Assert.Contains("DeletionScheduledAfter", deserializers.Keys);
        Assert.Contains("LatestVersion", deserializers.Keys);
        Assert.Contains("ServiceClass", deserializers.Keys);
        Assert.Contains("Status", deserializers.Keys);
        Assert.Contains("Tags", deserializers.Keys);
    }

    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new CreateKeyResponse().Serialize(null!));
    }

    [Fact]
    public void GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new CreateKeyResponse();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        var createdAt = DateTimeOffset.Parse("2024-01-15T10:30:00Z");
        parseNode.GetDateTimeOffsetValue().Returns(createdAt);
        deserializers["CreatedAt"](parseNode);
        Assert.Equal(createdAt, model.CreatedAt);

        var deletionScheduledAfter = new DateTimeOffset(2024, 3, 1, 0, 0, 0, TimeSpan.Zero);
        parseNode.GetDateTimeOffsetValue().Returns(deletionScheduledAfter);
        deserializers["DeletionScheduledAfter"](parseNode);
        Assert.Equal(deletionScheduledAfter, model.DeletionScheduledAfter);

        parseNode.GetStringValue().Returns("Test description");
        deserializers["Description"](parseNode);
        Assert.Equal("Test description", model.Description);

        parseNode.GetStringValue().Returns("key-123");
        deserializers["ID"](parseNode);
        Assert.Equal("key-123", model.ID);

        parseNode.GetEnumValue<CreateKeyResponse_KeyOrigin>().Returns(CreateKeyResponse_KeyOrigin.Imported);
        deserializers["KeyOrigin"](parseNode);
        Assert.Equal(CreateKeyResponse_KeyOrigin.Imported, model.KeyOrigin);

        parseNode.GetIntValue().Returns(3);
        deserializers["LatestVersion"](parseNode);
        Assert.Equal(3, model.LatestVersion);

        var modifiedAt = DateTimeOffset.Parse("2024-02-20T15:45:00Z");
        parseNode.GetDateTimeOffsetValue().Returns(modifiedAt);
        deserializers["ModifiedAt"](parseNode);
        Assert.Equal(modifiedAt, model.ModifiedAt);

        parseNode.GetStringValue().Returns("test-key-name");
        deserializers["Name"](parseNode);
        Assert.Equal("test-key-name", model.Name);

        parseNode.GetEnumValue<CreateKeyResponse_ServiceClass>().Returns(CreateKeyResponse_ServiceClass.CloudKmsKey);
        deserializers["ServiceClass"](parseNode);
        Assert.Equal(CreateKeyResponse_ServiceClass.CloudKmsKey, model.ServiceClass);

        parseNode.GetEnumValue<CreateKeyResponse_Status>().Returns(CreateKeyResponse_Status.Active);
        deserializers["Status"](parseNode);
        Assert.Equal(CreateKeyResponse_Status.Active, model.Status);

        var tags = new List<string> { "tag1", "tag2", "tag3" };
        parseNode.GetCollectionOfPrimitiveValues<string>().Returns(tags);
        deserializers["Tags"](parseNode);
        Assert.Equal(tags, model.Tags);
    }

    [Fact]
    public async Task CanDeserializeResponseJsonWithAllProperties()
    {
        const string json = """
            {
              "CreatedAt": "2024-01-15T10:30:00Z",
              "DeletionScheduledAfter": "2024-03-01T00:00:00Z",
              "Description": "key-description",
              "ID": "key-123",
              "KeyOrigin": "imported",
              "LatestVersion": 3,
              "ModifiedAt": "2024-02-20T15:45:00Z",
              "Name": "key-name",
              "ServiceClass": "cloud/kms/key",
              "Status": "active",
              "Tags": ["tag1", "tag2"]
            }
            """;
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        var parseNode = await new JsonParseNodeFactory().GetRootParseNodeAsync("application/json", stream);
        var result = parseNode.GetObjectValue(CreateKeyResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(result);
        Assert.Equal(new DateTimeOffset(2024, 1, 15, 10, 30, 0, TimeSpan.Zero), result.CreatedAt);
        Assert.Equal(new DateTimeOffset(2024, 3, 1, 0, 0, 0, TimeSpan.Zero), result.DeletionScheduledAfter);
        Assert.Equal("key-description", result.Description);
        Assert.Equal("key-123", result.ID);
        Assert.Equal(CreateKeyResponse_KeyOrigin.Imported, result.KeyOrigin);
        Assert.Equal(3, result.LatestVersion);
        Assert.Equal(new DateTimeOffset(2024, 2, 20, 15, 45, 0, TimeSpan.Zero), result.ModifiedAt);
        Assert.Equal("key-name", result.Name);
        Assert.Equal(CreateKeyResponse_ServiceClass.CloudKmsKey, result.ServiceClass);
        Assert.Equal(CreateKeyResponse_Status.Active, result.Status);
        Assert.Equal(new[] { "tag1", "tag2" }, result.Tags);
    }

    [Fact]
    public async Task CanSerializeAndDeserializeProperties()
    {
        var originalModel = new CreateKeyResponse
        {
            Description = "key-description",
            Name = "key-name",
            Tags = ["tag1", "tag2"]
        };
        using var stream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        await writer.GetSerializedContent().CopyToAsync(stream);

        stream.Position = 0;
        var parseNode = await new JsonParseNodeFactory().GetRootParseNodeAsync("application/json", stream);
        var result = parseNode.GetObjectValue(CreateKeyResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(result);
        Assert.Equal(originalModel.Description, result.Description);
        Assert.Equal(originalModel.Name, result.Name);
        Assert.Equal(originalModel.Tags, result.Tags);
    }

    [Fact]
    public async Task CanSerializeAndDeserializeAdditionalData()
    {
        var originalModel = new CreateKeyResponse { Name = "key-name" };
        originalModel.AdditionalData.Add("custom_field", "value");
        using var stream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        await writer.GetSerializedContent().CopyToAsync(stream);

        stream.Position = 0;
        var parseNode = await new JsonParseNodeFactory().GetRootParseNodeAsync("application/json", stream);
        var result = parseNode.GetObjectValue(CreateKeyResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(result);
        Assert.Contains("custom_field", result.AdditionalData.Keys);
    }
}
