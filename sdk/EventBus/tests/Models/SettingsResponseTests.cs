using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.EventBus.Models;

namespace Sacloud.Sdk.EventBus.Tests.Models;

/// <summary>
/// SettingsResponseモデルのテスト
/// </summary>
public class SettingsResponseTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void SettingsResponse_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = SettingsResponse.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<SettingsResponse>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void SettingsResponse_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() =>
            SettingsResponse.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// ProcessConfigurationSettingsプロパティが正しく設定できることを確認
    /// </summary>
    [Fact]
    public void SettingsResponse_ProcessConfigurationSettings_CanBeSet()
    {
        var model = new SettingsResponse
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
    /// ScheduleSettingsResponseプロパティが正しく設定できることを確認
    /// </summary>
    [Fact]
    public void SettingsResponse_ScheduleSettingsResponse_CanBeSet()
    {
        var model = new SettingsResponse
        {
            ScheduleSettingsResponse = new ScheduleSettingsResponse
            {
                StartsAt = "1704067200000"
            }
        };

        Assert.NotNull(model.ScheduleSettingsResponse);
        Assert.Equal("1704067200000", model.ScheduleSettingsResponse.StartsAt);
    }

    /// <summary>
    /// TriggerSettingsプロパティが正しく設定できることを確認
    /// </summary>
    [Fact]
    public void SettingsResponse_TriggerSettings_CanBeSet()
    {
        var model = new SettingsResponse
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
    public void SettingsResponse_GetFieldDeserializers_WithProcessConfigurationSettings_ReturnsCorrectMappings()
    {
        var model = new SettingsResponse
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
    public void SettingsResponse_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new SettingsResponse
        {
            ProcessConfigurationSettings = new ProcessConfigurationSettings()
        };
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// 空のSettingsResponseインスタンスが作成できることを確認
    /// </summary>
    [Fact]
    public void SettingsResponse_EmptyInstance_CanBeCreated()
    {
        var model = new SettingsResponse();

        Assert.NotNull(model);
        Assert.Null(model.ProcessConfigurationSettings);
        Assert.Null(model.ScheduleSettingsResponse);
        Assert.Null(model.TriggerSettings);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがDestinationフィールド存在時にProcessConfigurationSettingsを設定することを確認
    /// </summary>
    [Fact]
    public void SettingsResponse_CreateFromDiscriminatorValue_WithDestination_SetsProcessConfigurationSettings()
    {
        var parseNode = Substitute.For<IParseNode>();
        var destinationNode = Substitute.For<IParseNode>();
        var processConfig = new ProcessConfigurationSettings { Parameters = "{\"test\":\"value\"}" };

        parseNode.GetChildNode("Destination").Returns(destinationNode);
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<ProcessConfigurationSettings>>()).Returns(processConfig);

        var result = SettingsResponse.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.NotNull(result.ProcessConfigurationSettings);
        Assert.Null(result.ScheduleSettingsResponse);
        Assert.Null(result.TriggerSettings);
        Assert.Equal("{\"test\":\"value\"}", result.ProcessConfigurationSettings.Parameters);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがStartsAtフィールド存在時にScheduleSettingsResponseを設定することを確認
    /// </summary>
    [Fact]
    public void SettingsResponse_CreateFromDiscriminatorValue_WithStartsAt_SetsScheduleSettingsResponse()
    {
        var parseNode = Substitute.For<IParseNode>();
        var startsAtNode = Substitute.For<IParseNode>();
        var scheduleSettings = new ScheduleSettingsResponse { StartsAt = "1704067200000" };

        parseNode.GetChildNode("Destination").Returns((IParseNode?)null);
        parseNode.GetChildNode("StartsAt").Returns(startsAtNode);
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<ScheduleSettingsResponse>>()).Returns(scheduleSettings);

        var result = SettingsResponse.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.Null(result.ProcessConfigurationSettings);
        Assert.NotNull(result.ScheduleSettingsResponse);
        Assert.Null(result.TriggerSettings);
        Assert.Equal("1704067200000", result.ScheduleSettingsResponse.StartsAt);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがSourceフィールド存在時にTriggerSettingsを設定することを確認
    /// </summary>
    [Fact]
    public void SettingsResponse_CreateFromDiscriminatorValue_WithSource_SetsTriggerSettings()
    {
        var parseNode = Substitute.For<IParseNode>();
        var sourceNode = Substitute.For<IParseNode>();
        var triggerSettings = new TriggerSettings { Source = "simplemq" };

        parseNode.GetChildNode("Destination").Returns((IParseNode?)null);
        parseNode.GetChildNode("StartsAt").Returns((IParseNode?)null);
        parseNode.GetChildNode("Source").Returns(sourceNode);
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<TriggerSettings>>()).Returns(triggerSettings);

        var result = SettingsResponse.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.Null(result.ProcessConfigurationSettings);
        Assert.Null(result.ScheduleSettingsResponse);
        Assert.NotNull(result.TriggerSettings);
        Assert.Equal("simplemq", result.TriggerSettings.Source);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueが判別できない場合に空のインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void SettingsResponse_CreateFromDiscriminatorValue_WithNoMatchingFields_ReturnsEmptyInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetChildNode("Destination").Returns((IParseNode?)null);
        parseNode.GetChildNode("StartsAt").Returns((IParseNode?)null);
        parseNode.GetChildNode("Source").Returns((IParseNode?)null);

        var result = SettingsResponse.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.Null(result.ProcessConfigurationSettings);
        Assert.Null(result.ScheduleSettingsResponse);
        Assert.Null(result.TriggerSettings);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがDestinationフィールド優先で判別することを確認（複数フィールド存在時）
    /// </summary>
    [Fact]
    public void SettingsResponse_CreateFromDiscriminatorValue_WithMultipleFields_PrioritizesDestination()
    {
        var parseNode = Substitute.For<IParseNode>();
        var destinationNode = Substitute.For<IParseNode>();
        var startsAtNode = Substitute.For<IParseNode>();
        var processConfig = new ProcessConfigurationSettings { Parameters = "{\"test\":\"value\"}" };

        parseNode.GetChildNode("Destination").Returns(destinationNode);
        parseNode.GetChildNode("StartsAt").Returns(startsAtNode);
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<ProcessConfigurationSettings>>()).Returns(processConfig);

        var result = SettingsResponse.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.NotNull(result.ProcessConfigurationSettings);
        Assert.Null(result.ScheduleSettingsResponse);
        Assert.Null(result.TriggerSettings);
    }

    /// <summary>
    /// GetFieldDeserializersがScheduleSettingsResponseの場合に正しく動作することを確認
    /// </summary>
    [Fact]
    public void SettingsResponse_GetFieldDeserializers_WithScheduleSettingsResponse_ReturnsCorrectMappings()
    {
        var model = new SettingsResponse
        {
            ScheduleSettingsResponse = new ScheduleSettingsResponse()
        };
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.NotEmpty(deserializers);
    }

    /// <summary>
    /// GetFieldDeserializersがTriggerSettingsの場合に正しく動作することを確認
    /// </summary>
    [Fact]
    public void SettingsResponse_GetFieldDeserializers_WithTriggerSettings_ReturnsCorrectMappings()
    {
        var model = new SettingsResponse
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
    public void SettingsResponse_GetFieldDeserializers_WithNoSettings_ReturnsEmptyDictionary()
    {
        var model = new SettingsResponse();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Empty(deserializers);
    }

    /// <summary>
    /// SerializeがProcessConfigurationSettings設定時に正しくシリアライズすることを確認
    /// </summary>
    [Fact]
    public void SettingsResponse_Serialize_WithProcessConfigurationSettings_SerializesCorrectly()
    {
        var model = new SettingsResponse
        {
            ProcessConfigurationSettings = new ProcessConfigurationSettings { Parameters = "{}" }
        };
        var writer = Substitute.For<ISerializationWriter>();

        model.Serialize(writer);

        writer.Received(1).WriteObjectValue<ProcessConfigurationSettings>(null, Arg.Any<ProcessConfigurationSettings>());
    }

    /// <summary>
    /// SerializeがScheduleSettingsResponse設定時に正しくシリアライズすることを確認
    /// </summary>
    [Fact]
    public void SettingsResponse_Serialize_WithScheduleSettingsResponse_SerializesCorrectly()
    {
        var model = new SettingsResponse
        {
            ScheduleSettingsResponse = new ScheduleSettingsResponse { StartsAt = "1704067200000" }
        };
        var writer = Substitute.For<ISerializationWriter>();

        model.Serialize(writer);

        writer.Received(1).WriteObjectValue<ScheduleSettingsResponse>(null, Arg.Any<ScheduleSettingsResponse>());
    }

    /// <summary>
    /// SerializeがTriggerSettings設定時に正しくシリアライズすることを確認
    /// </summary>
    [Fact]
    public void SettingsResponse_Serialize_WithTriggerSettings_SerializesCorrectly()
    {
        var model = new SettingsResponse
        {
            TriggerSettings = new TriggerSettings { Source = "simplemq" }
        };
        var writer = Substitute.For<ISerializationWriter>();

        model.Serialize(writer);

        writer.Received(1).WriteObjectValue<TriggerSettings>(null, Arg.Any<TriggerSettings>());
    }
}
