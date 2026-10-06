using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.EventBus.Models;

namespace Sacloud.Sdk.EventBus.Tests.Models;

/// <summary>
/// SettingsRequestモデルのテスト
/// </summary>
public class SettingsRequestTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void SettingsRequest_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = SettingsRequest.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<SettingsRequest>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void SettingsRequest_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() =>
            SettingsRequest.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// ProcessConfigurationSettingsプロパティが正しく設定できることを確認
    /// </summary>
    [Fact]
    public void SettingsRequest_ProcessConfigurationSettings_CanBeSet()
    {
        var model = new SettingsRequest
        {
            ProcessConfigurationSettings = new ProcessConfigurationSettings
            {
                Parameters = "{\"test\":\"value\"}"
            }
        };

        Assert.NotNull(model.ProcessConfigurationSettings);
        Assert.Equal("{\"test\":\"value\"}", model.ProcessConfigurationSettings.Parameters);
    }

    /// <summary>
    /// ScheduleSettingsRequestプロパティが正しく設定できることを確認
    /// </summary>
    [Fact]
    public void SettingsRequest_ScheduleSettingsRequest_CanBeSet()
    {
        var model = new SettingsRequest
        {
            ScheduleSettingsRequest = new ScheduleSettingsRequest
            {
                StartsAt = 1704067200000L
            }
        };

        Assert.NotNull(model.ScheduleSettingsRequest);
        Assert.Equal(1704067200000L, model.ScheduleSettingsRequest.StartsAt);
    }

    /// <summary>
    /// TriggerSettingsプロパティが正しく設定できることを確認
    /// </summary>
    [Fact]
    public void SettingsRequest_TriggerSettings_CanBeSet()
    {
        var model = new SettingsRequest
        {
            TriggerSettings = new TriggerSettings
            {
                Source = "simplemq"
            }
        };

        Assert.NotNull(model.TriggerSettings);
        Assert.Equal("simplemq", model.TriggerSettings.Source);
    }

    /// <summary>
    /// GetFieldDeserializersがProcessConfigurationSettingsの場合に正しく動作することを確認
    /// </summary>
    [Fact]
    public void SettingsRequest_GetFieldDeserializers_WithProcessConfigurationSettings_ReturnsCorrectMappings()
    {
        var model = new SettingsRequest
        {
            ProcessConfigurationSettings = new ProcessConfigurationSettings()
        };
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.NotEmpty(deserializers);
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void SettingsRequest_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new SettingsRequest
        {
            ProcessConfigurationSettings = new ProcessConfigurationSettings()
        };
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// 空のSettingsRequestインスタンスが作成できることを確認
    /// </summary>
    [Fact]
    public void SettingsRequest_EmptyInstance_CanBeCreated()
    {
        var model = new SettingsRequest();

        Assert.NotNull(model);
        Assert.Null(model.ProcessConfigurationSettings);
        Assert.Null(model.ScheduleSettingsRequest);
        Assert.Null(model.TriggerSettings);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがDestinationフィールド存在時にProcessConfigurationSettingsを設定することを確認
    /// </summary>
    [Fact]
    public void SettingsRequest_CreateFromDiscriminatorValue_WithDestination_SetsProcessConfigurationSettings()
    {
        var parseNode = Substitute.For<IParseNode>();
        var destinationNode = Substitute.For<IParseNode>();
        var processConfig = new ProcessConfigurationSettings { Parameters = "{\"test\":\"value\"}" };

        parseNode.GetChildNode("Destination").Returns(destinationNode);
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<ProcessConfigurationSettings>>()).Returns(processConfig);

        var result = SettingsRequest.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.NotNull(result.ProcessConfigurationSettings);
        Assert.Null(result.ScheduleSettingsRequest);
        Assert.Null(result.TriggerSettings);
        Assert.Equal("{\"test\":\"value\"}", result.ProcessConfigurationSettings.Parameters);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがStartsAtフィールド存在時にScheduleSettingsRequestを設定することを確認
    /// </summary>
    [Fact]
    public void SettingsRequest_CreateFromDiscriminatorValue_WithStartsAt_SetsScheduleSettingsRequest()
    {
        var parseNode = Substitute.For<IParseNode>();
        var startsAtNode = Substitute.For<IParseNode>();
        var scheduleSettings = new ScheduleSettingsRequest { StartsAt = 1704067200000L };

        parseNode.GetChildNode("Destination").Returns((IParseNode?)null);
        parseNode.GetChildNode("StartsAt").Returns(startsAtNode);
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<ScheduleSettingsRequest>>()).Returns(scheduleSettings);

        var result = SettingsRequest.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.Null(result.ProcessConfigurationSettings);
        Assert.NotNull(result.ScheduleSettingsRequest);
        Assert.Null(result.TriggerSettings);
        Assert.Equal(1704067200000L, result.ScheduleSettingsRequest.StartsAt);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがSourceフィールド存在時にTriggerSettingsを設定することを確認
    /// </summary>
    [Fact]
    public void SettingsRequest_CreateFromDiscriminatorValue_WithSource_SetsTriggerSettings()
    {
        var parseNode = Substitute.For<IParseNode>();
        var sourceNode = Substitute.For<IParseNode>();
        var triggerSettings = new TriggerSettings { Source = "simplemq" };

        parseNode.GetChildNode("Destination").Returns((IParseNode?)null);
        parseNode.GetChildNode("StartsAt").Returns((IParseNode?)null);
        parseNode.GetChildNode("Source").Returns(sourceNode);
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<TriggerSettings>>()).Returns(triggerSettings);

        var result = SettingsRequest.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.Null(result.ProcessConfigurationSettings);
        Assert.Null(result.ScheduleSettingsRequest);
        Assert.NotNull(result.TriggerSettings);
        Assert.Equal("simplemq", result.TriggerSettings.Source);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueが判別できない場合に空のインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void SettingsRequest_CreateFromDiscriminatorValue_WithNoMatchingFields_ReturnsEmptyInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetChildNode("Destination").Returns((IParseNode?)null);
        parseNode.GetChildNode("StartsAt").Returns((IParseNode?)null);
        parseNode.GetChildNode("Source").Returns((IParseNode?)null);

        var result = SettingsRequest.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.Null(result.ProcessConfigurationSettings);
        Assert.Null(result.ScheduleSettingsRequest);
        Assert.Null(result.TriggerSettings);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがDestinationフィールド優先で判別することを確認（複数フィールド存在時）
    /// </summary>
    [Fact]
    public void SettingsRequest_CreateFromDiscriminatorValue_WithMultipleFields_PrioritizesDestination()
    {
        var parseNode = Substitute.For<IParseNode>();
        var destinationNode = Substitute.For<IParseNode>();
        var startsAtNode = Substitute.For<IParseNode>();
        var processConfig = new ProcessConfigurationSettings { Parameters = "{\"test\":\"value\"}" };

        parseNode.GetChildNode("Destination").Returns(destinationNode);
        parseNode.GetChildNode("StartsAt").Returns(startsAtNode);
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<ProcessConfigurationSettings>>()).Returns(processConfig);

        var result = SettingsRequest.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.NotNull(result.ProcessConfigurationSettings);
        Assert.Null(result.ScheduleSettingsRequest);
        Assert.Null(result.TriggerSettings);
    }

    /// <summary>
    /// GetFieldDeserializersがScheduleSettingsRequestの場合に正しく動作することを確認
    /// </summary>
    [Fact]
    public void SettingsRequest_GetFieldDeserializers_WithScheduleSettingsRequest_ReturnsCorrectMappings()
    {
        var model = new SettingsRequest
        {
            ScheduleSettingsRequest = new ScheduleSettingsRequest()
        };
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.NotEmpty(deserializers);
    }

    /// <summary>
    /// GetFieldDeserializersがTriggerSettingsの場合に正しく動作することを確認
    /// </summary>
    [Fact]
    public void SettingsRequest_GetFieldDeserializers_WithTriggerSettings_ReturnsCorrectMappings()
    {
        var model = new SettingsRequest
        {
            TriggerSettings = new TriggerSettings()
        };
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.NotEmpty(deserializers);
    }

    /// <summary>
    /// GetFieldDeserializersが空の場合に空のディクショナリを返すことを確認
    /// </summary>
    [Fact]
    public void SettingsRequest_GetFieldDeserializers_WithNoSettings_ReturnsEmptyDictionary()
    {
        var model = new SettingsRequest();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Empty(deserializers);
    }

    /// <summary>
    /// SerializeがProcessConfigurationSettings設定時に正しくシリアライズすることを確認
    /// </summary>
    [Fact]
    public void SettingsRequest_Serialize_WithProcessConfigurationSettings_SerializesCorrectly()
    {
        var model = new SettingsRequest
        {
            ProcessConfigurationSettings = new ProcessConfigurationSettings { Parameters = "{}" }
        };
        var writer = Substitute.For<ISerializationWriter>();

        model.Serialize(writer);

        writer.Received(1).WriteObjectValue<ProcessConfigurationSettings>(null, Arg.Any<ProcessConfigurationSettings>());
    }

    /// <summary>
    /// SerializeがScheduleSettingsRequest設定時に正しくシリアライズすることを確認
    /// </summary>
    [Fact]
    public void SettingsRequest_Serialize_WithScheduleSettingsRequest_SerializesCorrectly()
    {
        var model = new SettingsRequest
        {
            ScheduleSettingsRequest = new ScheduleSettingsRequest { StartsAt = 1704067200000L }
        };
        var writer = Substitute.For<ISerializationWriter>();

        model.Serialize(writer);

        writer.Received(1).WriteObjectValue<ScheduleSettingsRequest>(null, Arg.Any<ScheduleSettingsRequest>());
    }

    /// <summary>
    /// SerializeがTriggerSettings設定時に正しくシリアライズすることを確認
    /// </summary>
    [Fact]
    public void SettingsRequest_Serialize_WithTriggerSettings_SerializesCorrectly()
    {
        var model = new SettingsRequest
        {
            TriggerSettings = new TriggerSettings { Source = "simplemq" }
        };
        var writer = Substitute.For<ISerializationWriter>();

        model.Serialize(writer);

        writer.Received(1).WriteObjectValue<TriggerSettings>(null, Arg.Any<TriggerSettings>());
    }
}
