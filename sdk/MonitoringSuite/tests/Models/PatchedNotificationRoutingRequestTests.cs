using System.ComponentModel.DataAnnotations;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// PatchedNotificationRoutingRequestモデルのテスト
/// </summary>
public class PatchedNotificationRoutingRequestTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void PatchedNotificationRoutingRequest_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<PatchedNotificationRoutingRequest>(PatchedNotificationRoutingRequest.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));

    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void PatchedNotificationRoutingRequest_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new PatchedNotificationRoutingRequest().Serialize(null!));

    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void PatchedNotificationRoutingRequest_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new PatchedNotificationRoutingRequest().GetFieldDeserializers();
        Assert.Equal(3, d.Count);
        Assert.True(d.ContainsKey("match_labels"));
        Assert.True(d.ContainsKey("notification_target_uid"));
        Assert.True(d.ContainsKey("resend_interval_minutes"));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void PatchedNotificationRoutingRequest_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new PatchedNotificationRoutingRequest();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        deserializers["match_labels"](parseNode);

        deserializers["notification_target_uid"](parseNode);

        parseNode.GetIntValue().Returns(1);
        deserializers["resend_interval_minutes"](parseNode);
        Assert.Equal(1, model.ResendIntervalMinutes);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void PatchedNotificationRoutingRequest_Serialize_WritesExpectedFields()
    {
        var model = new PatchedNotificationRoutingRequest { NotificationTargetUid = null, ResendIntervalMinutes = 1 };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteGuidValue("notification_target_uid", null);
        writer.Received(1).WriteIntValue("resend_interval_minutes", 1);
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }

    /// <summary>
    /// ResendIntervalMinutesが最大値を超えた場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void PatchedNotificationRoutingRequest_Validation_ResendIntervalMinutesExceedsMax_ReturnsValidationError()
    {
        var model = new PatchedNotificationRoutingRequest { ResendIntervalMinutes = 721 };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(PatchedNotificationRoutingRequest.ResendIntervalMinutes)));
    }

    /// <summary>
    /// ResendIntervalMinutesが負の値の場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void PatchedNotificationRoutingRequest_Validation_ResendIntervalMinutesBelowMin_ReturnsValidationError()
    {
        var model = new PatchedNotificationRoutingRequest { ResendIntervalMinutes = -1 };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(PatchedNotificationRoutingRequest.ResendIntervalMinutes)));
    }

    /// <summary>
    /// 全フィールドがnullの場合にバリデーションが成功することを確認（PATCH は全フィールド任意）
    /// </summary>
    [Fact]
    public void PatchedNotificationRoutingRequest_Validation_AllNullModel_PassesValidation()
    {
        var model = new PatchedNotificationRoutingRequest();
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.True(isValid);
        Assert.Empty(results);
    }

    /// <summary>
    /// 有効な値を設定した場合にバリデーションが成功することを確認
    /// </summary>
    [Fact]
    public void PatchedNotificationRoutingRequest_Validation_ValidModel_PassesValidation()
    {
        var model = new PatchedNotificationRoutingRequest
        {
            MatchLabels = [],
            NotificationTargetUid = Guid.NewGuid(),
            ResendIntervalMinutes = 60
        };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.True(isValid);
        Assert.Empty(results);
    }
}
