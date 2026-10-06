using System.ComponentModel.DataAnnotations;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// NotificationRoutingRequestモデルのテスト
/// </summary>
public class NotificationRoutingRequestTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void NotificationRoutingRequest_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<NotificationRoutingRequest>(NotificationRoutingRequest.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));
    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void NotificationRoutingRequest_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new NotificationRoutingRequest().Serialize(null!));
    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void NotificationRoutingRequest_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new NotificationRoutingRequest().GetFieldDeserializers();
        Assert.Equal(3, d.Count);
        Assert.True(d.ContainsKey("match_labels"));
        Assert.True(d.ContainsKey("notification_target_uid"));
        Assert.True(d.ContainsKey("resend_interval_minutes"));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void NotificationRoutingRequest_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new NotificationRoutingRequest();
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
    public void NotificationRoutingRequest_Serialize_WritesExpectedFields()
    {
        var model = new NotificationRoutingRequest { NotificationTargetUid = null, ResendIntervalMinutes = 1 };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteGuidValue("notification_target_uid", null);
        writer.Received(1).WriteIntValue("resend_interval_minutes", 1);
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }

    /// <summary>
    /// MatchLabelsがnullの場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void NotificationRoutingRequest_Validation_MatchLabelsIsNull_ReturnsValidationError()
    {
        var model = new NotificationRoutingRequest
        {
            MatchLabels = null,
            NotificationTargetUid = Guid.NewGuid()
        };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(NotificationRoutingRequest.MatchLabels)));
    }

    /// <summary>
    /// NotificationTargetUidがnullの場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void NotificationRoutingRequest_Validation_NotificationTargetUidIsNull_ReturnsValidationError()
    {
        var model = new NotificationRoutingRequest
        {
            MatchLabels = [],
            NotificationTargetUid = null
        };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(NotificationRoutingRequest.NotificationTargetUid)));
    }

    /// <summary>
    /// ResendIntervalMinutesが最大値を超えた場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void NotificationRoutingRequest_Validation_ResendIntervalMinutesExceedsMax_ReturnsValidationError()
    {
        var model = new NotificationRoutingRequest
        {
            MatchLabels = [],
            NotificationTargetUid = Guid.NewGuid(),
            ResendIntervalMinutes = 721
        };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(NotificationRoutingRequest.ResendIntervalMinutes)));
    }

    /// <summary>
    /// ResendIntervalMinutesが負の値の場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void NotificationRoutingRequest_Validation_ResendIntervalMinutesBelowMin_ReturnsValidationError()
    {
        var model = new NotificationRoutingRequest
        {
            MatchLabels = [],
            NotificationTargetUid = Guid.NewGuid(),
            ResendIntervalMinutes = -1
        };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(NotificationRoutingRequest.ResendIntervalMinutes)));
    }

    /// <summary>
    /// すべてのフィールドが有効な場合にバリデーションが成功することを確認
    /// </summary>
    [Fact]
    public void NotificationRoutingRequest_Validation_ValidModel_PassesValidation()
    {
        var model = new NotificationRoutingRequest
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
