using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using GroupMembershipsModel = Sacloud.Sdk.IAM.Models.GroupMemberships;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// GroupMembershipsのテスト
/// </summary>
public class GroupMembershipsTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = GroupMembershipsModel.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<GroupMembershipsModel>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Sacloud.Sdk.IAM.Models.GroupMemberships.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"compat_users"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var groupMemberships = new GroupMembershipsModel();

        var deserializers = groupMemberships.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("compat_users"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var groupMemberships = new GroupMembershipsModel();

        Assert.Throws<ArgumentNullException>(() => groupMemberships.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクション（"compat_users"）が正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var groupMemberships = new GroupMembershipsModel();
        var deserializers = groupMemberships.GetFieldDeserializers();
        
        var compatUser1 = new IAM.Models.GroupMemberships_compat_users { Id = 123L };
        var compatUser2 = new IAM.Models.GroupMemberships_compat_users { Id = 456L };
        var compatUsers = new List<IAM.Models.GroupMemberships_compat_users> { compatUser1, compatUser2 };
        
        var parseNodeCompatUsers = Substitute.For<IParseNode>();
        parseNodeCompatUsers.GetCollectionOfObjectValues(
            Arg.Any<ParsableFactory<IAM.Models.GroupMemberships_compat_users>>())
            .Returns(compatUsers);
        deserializers["compat_users"](parseNodeCompatUsers);

        Assert.NotNull(groupMemberships.CompatUsers);
        Assert.Equal(2, groupMemberships.CompatUsers.Count);
        Assert.Equal(123L, groupMemberships.CompatUsers[0].Id);
        Assert.Equal(456L, groupMemberships.CompatUsers[1].Id);
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task GroupMemberships_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new GroupMembershipsModel
        {
            CompatUsers =
            [
                new IAM.Models.GroupMemberships_compat_users { Id = 123L },
                new IAM.Models.GroupMemberships_compat_users { Id = 456L }
            ]
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Sacloud.Sdk.IAM.Models.GroupMemberships.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.CompatUsers);
        Assert.Equal(2, deserializedModel.CompatUsers.Count);
        Assert.Equal(123L, deserializedModel.CompatUsers[0].Id);
        Assert.Equal(456L, deserializedModel.CompatUsers[1].Id);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task GroupMemberships_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new GroupMembershipsModel
        {
            CompatUsers =
            [
                new IAM.Models.GroupMemberships_compat_users { Id = 123L }
            ]
        };
        originalModel.AdditionalData.Add("customField", "customValue");

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Sacloud.Sdk.IAM.Models.GroupMemberships.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.CompatUsers);
        Assert.Single(deserializedModel.CompatUsers);
        Assert.Equal(123L, deserializedModel.CompatUsers[0].Id);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
