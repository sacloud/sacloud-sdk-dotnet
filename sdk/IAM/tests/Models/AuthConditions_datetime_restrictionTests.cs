using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.IAM.Models;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// AuthConditions_datetime_restrictionのテスト
/// </summary>
public class AuthConditions_datetime_restrictionTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = AuthConditions_datetime_restriction.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<AuthConditions_datetime_restriction>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() =>
            AuthConditions_datetime_restriction.CreateFromDiscriminatorValue(parseNode));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var datetimeRestriction = new AuthConditions_datetime_restriction();

        var deserializers = datetimeRestriction.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("after"));
        Assert.True(deserializers.ContainsKey("before"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var datetimeRestriction = new AuthConditions_datetime_restriction();

        Assert.Throws<ArgumentNullException>(() => datetimeRestriction.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var datetimeRestriction = new AuthConditions_datetime_restriction();
        var deserializers = datetimeRestriction.GetFieldDeserializers();

        var afterValue = DateTimeOffset.UtcNow;
        var parseNodeAfter = Substitute.For<IParseNode>();
        parseNodeAfter.GetDateTimeOffsetValue().Returns(afterValue);
        deserializers["after"](parseNodeAfter);

        var beforeValue = DateTimeOffset.UtcNow.AddDays(1);
        var parseNodeBefore = Substitute.For<IParseNode>();
        parseNodeBefore.GetDateTimeOffsetValue().Returns(beforeValue);
        deserializers["before"](parseNodeBefore);

        Assert.Equal(afterValue, datetimeRestriction.After);
        Assert.Equal(beforeValue, datetimeRestriction.Before);
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを確認
    /// </summary>
    [Fact]
    public async Task AuthConditions_datetime_restriction_CanSerializeAndDeserializeWithAllProperties()
    {
        var after = DateTimeOffset.UtcNow;
        var before = DateTimeOffset.UtcNow.AddDays(1);
        var originalModel = new AuthConditions_datetime_restriction
        {
            After = after,
            Before = before
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(AuthConditions_datetime_restriction.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.After);
        Assert.NotNull(deserializedModel.Before);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを確認
    /// </summary>
    [Fact]
    public async Task AuthConditions_datetime_restriction_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new AuthConditions_datetime_restriction();
        originalModel.AdditionalData.Add("customField", "customValue");

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(AuthConditions_datetime_restriction.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
