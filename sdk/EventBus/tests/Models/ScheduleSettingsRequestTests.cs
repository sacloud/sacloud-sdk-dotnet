using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.EventBus.Models;

namespace Sacloud.Sdk.EventBus.Tests.Models;

/// <summary>
/// ScheduleSettingsRequestモデルのテスト
/// </summary>
public class ScheduleSettingsRequestTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void ScheduleSettingsRequest_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = ScheduleSettingsRequest.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<ScheduleSettingsRequest>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ScheduleSettingsRequest_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() =>
            ScheduleSettingsRequest.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void ScheduleSettingsRequest_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new ScheduleSettingsRequest();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(5, deserializers.Count);
        Assert.True(deserializers.ContainsKey("Crontab"));
        Assert.True(deserializers.ContainsKey("ProcessConfigurationID"));
        Assert.True(deserializers.ContainsKey("RecurringStep"));
        Assert.True(deserializers.ContainsKey("RecurringUnit"));
        Assert.True(deserializers.ContainsKey("StartsAt"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ScheduleSettingsRequest_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ScheduleSettingsRequest();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void ScheduleSettingsRequest_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new ScheduleSettingsRequest();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetStringValue().Returns("0 0 * * *");
        deserializers["Crontab"](parseNode);
        Assert.Equal("0 0 * * *", model.Crontab);

        parseNode.GetStringValue().Returns("proc-123");
        deserializers["ProcessConfigurationID"](parseNode);
        Assert.Equal("proc-123", model.ProcessConfigurationID);

        parseNode.GetIntValue().Returns(5);
        deserializers["RecurringStep"](parseNode);
        Assert.Equal(5, model.RecurringStep);

        parseNode.GetLongValue().Returns(1704067200000L);
        deserializers["StartsAt"](parseNode);
        Assert.Equal(1704067200000L, model.StartsAt);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ScheduleSettingsRequest_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ScheduleSettingsRequest
        {
            Crontab = "0 0 * * *",
            ProcessConfigurationID = "proc-123",
            RecurringStep = 5,
            StartsAt = 1704067200000L
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        await content.CopyToAsync(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(ScheduleSettingsRequest.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Crontab, deserializedModel.Crontab);
        Assert.Equal(originalModel.ProcessConfigurationID, deserializedModel.ProcessConfigurationID);
        Assert.Equal(originalModel.RecurringStep, deserializedModel.RecurringStep);
        Assert.Equal(originalModel.StartsAt, deserializedModel.StartsAt);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ScheduleSettingsRequest_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new ScheduleSettingsRequest { StartsAt = 1704067200000L };
        originalModel.AdditionalData.Add("custom_field_1", "custom_value_1");
        originalModel.AdditionalData.Add("custom_field_2", 42);

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        await content.CopyToAsync(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(ScheduleSettingsRequest.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}
