using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// NotificationRoutingOrderRequestモデルのテスト
/// </summary>
public class NotificationRoutingOrderRequestTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void NotificationRoutingOrderRequest_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = NotificationRoutingOrderRequest.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<NotificationRoutingOrderRequest>(result);
    }

    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void NotificationRoutingOrderRequest_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new NotificationRoutingOrderRequest().Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void NotificationRoutingOrderRequest_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new NotificationRoutingOrderRequest();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        deserializers["notification_routing_uid"](parseNode);

        parseNode.GetIntValue().Returns(1);
        deserializers["order"](parseNode);
        Assert.Equal(1, model.Order);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void NotificationRoutingOrderRequest_Serialize_WritesExpectedFields()
    {
        var model = new NotificationRoutingOrderRequest { NotificationRoutingUid = null, Order = 1 };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteGuidValue("notification_routing_uid", null);
        writer.Received(1).WriteIntValue("order", 1);
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }

    /// <summary>
    /// NotificationRoutingUidがnullの場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void NotificationRoutingOrderRequest_Validation_NotificationRoutingUidIsNull_ReturnsValidationError()
    {
        var model = new NotificationRoutingOrderRequest { NotificationRoutingUid = null, Order = 1 };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(NotificationRoutingOrderRequest.NotificationRoutingUid)));
    }

    /// <summary>
    /// Orderがnullの場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void NotificationRoutingOrderRequest_Validation_OrderIsNull_ReturnsValidationError()
    {
        var model = new NotificationRoutingOrderRequest { NotificationRoutingUid = Guid.NewGuid(), Order = null };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(NotificationRoutingOrderRequest.Order)));
    }

    /// <summary>
    /// Orderが最小値未満（0）の場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void NotificationRoutingOrderRequest_Validation_OrderBelowMinimum_ReturnsValidationError()
    {
        var model = new NotificationRoutingOrderRequest { NotificationRoutingUid = Guid.NewGuid(), Order = 0 };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(NotificationRoutingOrderRequest.Order)));
    }

    /// <summary>
    /// Orderが最小値（1）の場合にバリデーションエラーにならないことを確認
    /// </summary>
    [Fact]
    public void NotificationRoutingOrderRequest_Validation_OrderAtMinimum_IsValid()
    {
        var model = new NotificationRoutingOrderRequest { NotificationRoutingUid = Guid.NewGuid(), Order = 1 };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.True(isValid);
        Assert.Empty(results);
    }
}
