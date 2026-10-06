using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;

namespace Sacloud.Sdk.AddOn.Tests.Models;

/// <summary>
/// SystemDataモデルのテスト
/// </summary>
public class SystemDataTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void SystemData_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new global::Sacloud.Sdk.AddOn.Models.SystemData();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(6, deserializers.Count);
        Assert.True(deserializers.ContainsKey("createdBy"));
        Assert.True(deserializers.ContainsKey("createdByType"));
        Assert.True(deserializers.ContainsKey("createdOn"));
        Assert.True(deserializers.ContainsKey("lastModifiedBy"));
        Assert.True(deserializers.ContainsKey("lastModifiedByType"));
        Assert.True(deserializers.ContainsKey("lastModifiedOn"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void SystemData_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new global::Sacloud.Sdk.AddOn.Models.SystemData();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void SystemData_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new global::Sacloud.Sdk.AddOn.Models.SystemData();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("createdBy"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["createdBy"](parseNode);
        Assert.NotNull(model.CreatedBy);

        Assert.True(deserializers.ContainsKey("createdByType"));

        parseNode.GetObjectValue<Sacloud.Sdk.AddOn.Models.CreatedByType>(Arg.Any<ParsableFactory<Sacloud.Sdk.AddOn.Models.CreatedByType>>()).Returns(Substitute.For<Sacloud.Sdk.AddOn.Models.CreatedByType>());

        deserializers["createdByType"](parseNode);
        Assert.NotNull(model.CreatedByType);

        Assert.True(deserializers.ContainsKey("createdOn"));

        parseNode.GetDateTimeOffsetValue().Returns(DateTimeOffset.Parse("2026-01-01T00:00:00+00:00"));

        deserializers["createdOn"](parseNode);
        Assert.NotNull(model.CreatedOn);

        Assert.True(deserializers.ContainsKey("lastModifiedBy"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["lastModifiedBy"](parseNode);
        Assert.NotNull(model.LastModifiedBy);

        Assert.True(deserializers.ContainsKey("lastModifiedByType"));

        parseNode.GetObjectValue<Sacloud.Sdk.AddOn.Models.CreatedByType>(Arg.Any<ParsableFactory<Sacloud.Sdk.AddOn.Models.CreatedByType>>()).Returns(Substitute.For<Sacloud.Sdk.AddOn.Models.CreatedByType>());

        deserializers["lastModifiedByType"](parseNode);
        Assert.NotNull(model.LastModifiedByType);

        Assert.True(deserializers.ContainsKey("lastModifiedOn"));

        parseNode.GetDateTimeOffsetValue().Returns(DateTimeOffset.Parse("2026-01-01T00:00:00+00:00"));

        deserializers["lastModifiedOn"](parseNode);
        Assert.NotNull(model.LastModifiedOn);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task SystemData_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new global::Sacloud.Sdk.AddOn.Models.SystemData();
        originalModel.CreatedByType = new Sacloud.Sdk.AddOn.Models.CreatedByType();
        originalModel.LastModifiedByType = new Sacloud.Sdk.AddOn.Models.CreatedByType();

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue<global::Sacloud.Sdk.AddOn.Models.SystemData>(global::Sacloud.Sdk.AddOn.Models.SystemData.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel!.CreatedByType);
        Assert.NotNull(deserializedModel!.LastModifiedByType);
    }
}
