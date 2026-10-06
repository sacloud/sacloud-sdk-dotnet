using System.ComponentModel.DataAnnotations;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// SetLogStorageExpireDayRequestモデルのテスト
/// </summary>
public class SetLogStorageExpireDayRequestTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void SetLogStorageExpireDayRequest_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = SetLogStorageExpireDayRequest.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<SetLogStorageExpireDayRequest>(result);
    }

    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void SetLogStorageExpireDayRequest_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new SetLogStorageExpireDayRequest().Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void SetLogStorageExpireDayRequest_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new SetLogStorageExpireDayRequest();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetIntValue().Returns(1);
        deserializers["days"](parseNode);
        Assert.Equal(1, model.Days);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void SetLogStorageExpireDayRequest_Serialize_WritesExpectedFields()
    {
        var model = new SetLogStorageExpireDayRequest { Days = 1 };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteIntValue("days", 1);
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }

    /// <summary>
    /// Daysがnullの場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void SetLogStorageExpireDayRequest_Validation_DaysIsNull_ReturnsValidationError()
    {
        var model = new SetLogStorageExpireDayRequest { Days = null };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(SetLogStorageExpireDayRequest.Days)));
    }

    /// <summary>
    /// Daysが最小値未満の場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void SetLogStorageExpireDayRequest_Validation_DaysBelowMin_ReturnsValidationError()
    {
        var model = new SetLogStorageExpireDayRequest { Days = 0 };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(SetLogStorageExpireDayRequest.Days)));
    }

    /// <summary>
    /// Daysが最大値を超えた場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void SetLogStorageExpireDayRequest_Validation_DaysExceedsMax_ReturnsValidationError()
    {
        var model = new SetLogStorageExpireDayRequest { Days = 731 };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(SetLogStorageExpireDayRequest.Days)));
    }

    /// <summary>
    /// Daysが最小値ちょうどの場合にバリデーションが成功することを確認
    /// </summary>
    [Fact]
    public void SetLogStorageExpireDayRequest_Validation_DaysAtMinValue_PassesValidation()
    {
        var model = new SetLogStorageExpireDayRequest { Days = 1 };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.True(isValid);
        Assert.Empty(results);
    }

    /// <summary>
    /// Daysが最大値ちょうどの場合にバリデーションが成功することを確認
    /// </summary>
    [Fact]
    public void SetLogStorageExpireDayRequest_Validation_DaysAtMaxValue_PassesValidation()
    {
        var model = new SetLogStorageExpireDayRequest { Days = 730 };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.True(isValid);
        Assert.Empty(results);
    }
}
