using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.EventBus.Models;

namespace Sacloud.Sdk.EventBus.Tests.Models;

/// <summary>
/// TriggerSettingsモデルのテスト
/// </summary>
public class TriggerSettingsTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void TriggerSettings_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = TriggerSettings.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<TriggerSettings>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void TriggerSettings_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() =>
            TriggerSettings.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void TriggerSettings_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new TriggerSettings();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(4, deserializers.Count);
        Assert.True(deserializers.ContainsKey("Conditions"));
        Assert.True(deserializers.ContainsKey("ProcessConfigurationID"));
        Assert.True(deserializers.ContainsKey("Source"));
        Assert.True(deserializers.ContainsKey("Types"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void TriggerSettings_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new TriggerSettings();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void TriggerSettings_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new TriggerSettings();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetStringValue().Returns("proc-123");
        deserializers["ProcessConfigurationID"](parseNode);
        Assert.Equal("proc-123", model.ProcessConfigurationID);

        parseNode.GetStringValue().Returns("simplemq");
        deserializers["Source"](parseNode);
        Assert.Equal("simplemq", model.Source);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task TriggerSettings_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new TriggerSettings
        {
            ProcessConfigurationID = "proc-123",
            Source = "simplemq",
            Types = new List<string> { "message.received" }
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        await content.CopyToAsync(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(TriggerSettings.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.ProcessConfigurationID, deserializedModel.ProcessConfigurationID);
        Assert.Equal(originalModel.Source, deserializedModel.Source);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task TriggerSettings_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new TriggerSettings { Source = "simplemq" };
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
        var deserializedModel = parseNode.GetObjectValue(TriggerSettings.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }

    /// <summary>
    /// TriggerSettings_Conditions.CreateFromDiscriminatorValueがOp="eq"の時にTriggerConditionEqを設定することを確認
    /// </summary>
    [Fact]
    public void TriggerSettings_Conditions_CreateFromDiscriminatorValue_WithOpEq_SetsTriggerConditionEq()
    {
        var parseNode = Substitute.For<IParseNode>();
        var opNode = Substitute.For<IParseNode>();

        parseNode.GetChildNode("Op").Returns(opNode);
        opNode.GetStringValue().Returns("eq");

        var result = TriggerSettings.TriggerSettings_Conditions.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.NotNull(result.TriggerConditionEq);
        Assert.Null(result.TriggerConditionIn);
    }

    /// <summary>
    /// TriggerSettings_Conditions.CreateFromDiscriminatorValueがOp="in"の時にTriggerConditionInを設定することを確認
    /// </summary>
    [Fact]
    public void TriggerSettings_Conditions_CreateFromDiscriminatorValue_WithOpIn_SetsTriggerConditionIn()
    {
        var parseNode = Substitute.For<IParseNode>();
        var opNode = Substitute.For<IParseNode>();

        parseNode.GetChildNode("Op").Returns(opNode);
        opNode.GetStringValue().Returns("in");

        var result = TriggerSettings.TriggerSettings_Conditions.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.Null(result.TriggerConditionEq);
        Assert.NotNull(result.TriggerConditionIn);
    }

    /// <summary>
    /// TriggerSettings_Conditions.CreateFromDiscriminatorValueが大文字小文字を区別しないことを確認（Op="EQ"）
    /// </summary>
    [Fact]
    public void TriggerSettings_Conditions_CreateFromDiscriminatorValue_WithOpEqUpperCase_SetsTriggerConditionEq()
    {
        var parseNode = Substitute.For<IParseNode>();
        var opNode = Substitute.For<IParseNode>();

        parseNode.GetChildNode("Op").Returns(opNode);
        opNode.GetStringValue().Returns("EQ");

        var result = TriggerSettings.TriggerSettings_Conditions.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.NotNull(result.TriggerConditionEq);
        Assert.Null(result.TriggerConditionIn);
    }

    /// <summary>
    /// TriggerSettings_Conditions.CreateFromDiscriminatorValueが大文字小文字を区別しないことを確認（Op="IN"）
    /// </summary>
    [Fact]
    public void TriggerSettings_Conditions_CreateFromDiscriminatorValue_WithOpInUpperCase_SetsTriggerConditionIn()
    {
        var parseNode = Substitute.For<IParseNode>();
        var opNode = Substitute.For<IParseNode>();

        parseNode.GetChildNode("Op").Returns(opNode);
        opNode.GetStringValue().Returns("IN");

        var result = TriggerSettings.TriggerSettings_Conditions.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.Null(result.TriggerConditionEq);
        Assert.NotNull(result.TriggerConditionIn);
    }

    /// <summary>
    /// TriggerSettings_Conditions.CreateFromDiscriminatorValueが判別できない場合に空のインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void TriggerSettings_Conditions_CreateFromDiscriminatorValue_WithUnknownOp_ReturnsEmptyInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var opNode = Substitute.For<IParseNode>();

        parseNode.GetChildNode("Op").Returns(opNode);
        opNode.GetStringValue().Returns("unknown");

        var result = TriggerSettings.TriggerSettings_Conditions.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.Null(result.TriggerConditionEq);
        Assert.Null(result.TriggerConditionIn);
    }

    /// <summary>
    /// TriggerSettings_Conditions.CreateFromDiscriminatorValueがOpノードがnullの場合に空のインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void TriggerSettings_Conditions_CreateFromDiscriminatorValue_WithNoOpNode_ReturnsEmptyInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetChildNode("Op").Returns((IParseNode?)null);

        var result = TriggerSettings.TriggerSettings_Conditions.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.Null(result.TriggerConditionEq);
        Assert.Null(result.TriggerConditionIn);
    }

    /// <summary>
    /// TriggerSettings_Conditions.CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void TriggerSettings_Conditions_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() =>
            TriggerSettings.TriggerSettings_Conditions.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// TriggerSettings_Conditions.GetFieldDeserializersがTriggerConditionEq設定時に正しいデシリアライザーを返すことを確認
    /// </summary>
    [Fact]
    public void TriggerSettings_Conditions_GetFieldDeserializers_WithTriggerConditionEq_ReturnsCorrectDeserializers()
    {
        var condition = new TriggerSettings.TriggerSettings_Conditions
        {
            TriggerConditionEq = new TriggerConditionEq()
        };

        var deserializers = condition.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.NotEmpty(deserializers);
        Assert.True(deserializers.ContainsKey("Key"));
        Assert.True(deserializers.ContainsKey("Op"));
        Assert.True(deserializers.ContainsKey("Values"));
    }

    /// <summary>
    /// TriggerSettings_Conditions.GetFieldDeserializersがTriggerConditionIn設定時に正しいデシリアライザーを返すことを確認
    /// </summary>
    [Fact]
    public void TriggerSettings_Conditions_GetFieldDeserializers_WithTriggerConditionIn_ReturnsCorrectDeserializers()
    {
        var condition = new TriggerSettings.TriggerSettings_Conditions
        {
            TriggerConditionIn = new TriggerConditionIn()
        };

        var deserializers = condition.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.NotEmpty(deserializers);
        Assert.True(deserializers.ContainsKey("Key"));
        Assert.True(deserializers.ContainsKey("Op"));
        Assert.True(deserializers.ContainsKey("Values"));
    }

    /// <summary>
    /// TriggerSettings_Conditions.GetFieldDeserializersが空の場合に空のディクショナリを返すことを確認
    /// </summary>
    [Fact]
    public void TriggerSettings_Conditions_GetFieldDeserializers_WithNoCondition_ReturnsEmptyDictionary()
    {
        var condition = new TriggerSettings.TriggerSettings_Conditions();

        var deserializers = condition.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Empty(deserializers);
    }

    /// <summary>
    /// TriggerSettings_Conditions.SerializeがTriggerConditionEq設定時に正しくシリアライズすることを確認
    /// </summary>
    [Fact]
    public void TriggerSettings_Conditions_Serialize_WithTriggerConditionEq_SerializesCorrectly()
    {
        var condition = new TriggerSettings.TriggerSettings_Conditions
        {
            TriggerConditionEq = new TriggerConditionEq
            {
                Key = "status",
                Values = new List<string> { "active" }
            }
        };
        var writer = Substitute.For<ISerializationWriter>();

        condition.Serialize(writer);

        writer.Received(1).WriteObjectValue<TriggerConditionEq>(null, Arg.Is<TriggerConditionEq>(c => c.Key == "status"));
    }

    /// <summary>
    /// TriggerSettings_Conditions.SerializeがTriggerConditionIn設定時に正しくシリアライズすることを確認
    /// </summary>
    [Fact]
    public void TriggerSettings_Conditions_Serialize_WithTriggerConditionIn_SerializesCorrectly()
    {
        var condition = new TriggerSettings.TriggerSettings_Conditions
        {
            TriggerConditionIn = new TriggerConditionIn
            {
                Key = "type",
                Values = new List<string> { "message.received", "message.sent" }
            }
        };
        var writer = Substitute.For<ISerializationWriter>();

        condition.Serialize(writer);

        writer.Received(1).WriteObjectValue<TriggerConditionIn>(null, Arg.Is<TriggerConditionIn>(c => c.Key == "type"));
    }

    /// <summary>
    /// TriggerSettings_Conditions.Serializeがnullのライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void TriggerSettings_Conditions_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var condition = new TriggerSettings.TriggerSettings_Conditions
        {
            TriggerConditionEq = new TriggerConditionEq()
        };

        Assert.Throws<ArgumentNullException>(() => condition.Serialize(null!));
    }
}
