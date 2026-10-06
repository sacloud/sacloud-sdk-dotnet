using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.SimpleNotification.Models;
using Sacloud.Sdk.Shared.Validation;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.SimpleNotification.Tests.Models.Model;

/// <summary>
/// RoutingSettingsモデルのテスト
/// </summary>
public class RoutingSettingsTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = RoutingSettings.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<RoutingSettings>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => RoutingSettings.CreateFromDiscriminatorValue(null!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new RoutingSettings();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(4, deserializers.Count);
        Assert.True(deserializers.ContainsKey("MatchLabels"));
        Assert.True(deserializers.ContainsKey("PriorityRank"));
        Assert.True(deserializers.ContainsKey("SourceID"));
        Assert.True(deserializers.ContainsKey("TargetGroupID"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new RoutingSettings();

        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new RoutingSettings();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetStringValue().Returns("100");
        deserializers["SourceID"](parseNode);
        Assert.Equal("100", model.SourceID);

        parseNode.GetStringValue().Returns("200");
        deserializers["TargetGroupID"](parseNode);
        Assert.Equal("200", model.TargetGroupID);

        parseNode.GetIntValue().Returns(5);
        deserializers["PriorityRank"](parseNode);
        Assert.Equal(5, model.PriorityRank);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new RoutingSettings
        {
            SourceID = "1",
            TargetGroupID = "2",
            MatchLabels =
            [
                new RoutingSettings_MatchLabels { Name = "env", Value = "prod" },
            ],
        };

        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        using var serializeStream = new MemoryStream();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(RoutingSettings.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.SourceID, deserializedModel.SourceID);
        Assert.Equal(originalModel.TargetGroupID, deserializedModel.TargetGroupID);
        Assert.NotNull(deserializedModel.MatchLabels);
        Assert.Single(deserializedModel.MatchLabels);
        Assert.Equal("env", deserializedModel.MatchLabels[0].Name);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new RoutingSettings();
        originalModel.AdditionalData.Add("custom_field", "custom_value");

        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        using var serializeStream = new MemoryStream();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(RoutingSettings.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field"));
    }

    /// <summary>
    /// TargetGroupIDが数値12桁以内でない場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void Validation_WithInvalidTargetGroupId_ReturnsValidationError()
    {
        var model = new RoutingSettings
        {
            SourceID = "1",
            TargetGroupID = "abc",
        };

        var validationContext = new ValidationContext(model);
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.MemberNames.Contains(nameof(RoutingSettings.TargetGroupID)));
    }

    /// <summary>
    /// MatchLabels.Nameが仕様外の場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void MatchLabelsValidation_WithInvalidName_ReturnsValidationError()
    {
        var model = new RoutingSettings_MatchLabels
        {
            Name = new string('a', 65),
            Value = "prod",
        };

        var validationContext = new ValidationContext(model);
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.MemberNames.Contains(nameof(RoutingSettings_MatchLabels.Name)));
    }
}
