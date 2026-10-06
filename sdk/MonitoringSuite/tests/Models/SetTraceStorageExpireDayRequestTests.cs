using System.ComponentModel.DataAnnotations;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// SetTraceStorageExpireDayRequestモデルのテスト
/// </summary>
public class SetTraceStorageExpireDayRequestTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void SetTraceStorageExpireDayRequest_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<SetTraceStorageExpireDayRequest>(SetTraceStorageExpireDayRequest.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));

    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void SetTraceStorageExpireDayRequest_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new SetTraceStorageExpireDayRequest().Serialize(null!));

    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void SetTraceStorageExpireDayRequest_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new SetTraceStorageExpireDayRequest().GetFieldDeserializers();
        Assert.Single(d);
        Assert.True(d.ContainsKey("days"));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void SetTraceStorageExpireDayRequest_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new SetTraceStorageExpireDayRequest();
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
    public void SetTraceStorageExpireDayRequest_Serialize_WritesExpectedFields()
    {
        var model = new SetTraceStorageExpireDayRequest { Days = 1 };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteIntValue("days", 1);
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }

    /// <summary>
    /// Daysがnullの場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void SetTraceStorageExpireDayRequest_Validation_DaysIsNull_ReturnsValidationError()
    {
        var model = new SetTraceStorageExpireDayRequest { Days = null };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(SetTraceStorageExpireDayRequest.Days)));
    }

    /// <summary>
    /// Daysが最小値未満の場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void SetTraceStorageExpireDayRequest_Validation_DaysBelowMin_ReturnsValidationError()
    {
        var model = new SetTraceStorageExpireDayRequest { Days = 0 };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(SetTraceStorageExpireDayRequest.Days)));
    }

    /// <summary>
    /// Daysが最大値を超えた場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void SetTraceStorageExpireDayRequest_Validation_DaysExceedsMax_ReturnsValidationError()
    {
        var model = new SetTraceStorageExpireDayRequest { Days = 731 };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(SetTraceStorageExpireDayRequest.Days)));
    }

    /// <summary>
    /// Daysが最小値ちょうどの場合にバリデーションが成功することを確認
    /// </summary>
    [Fact]
    public void SetTraceStorageExpireDayRequest_Validation_DaysAtMinValue_PassesValidation()
    {
        var model = new SetTraceStorageExpireDayRequest { Days = 1 };
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
    public void SetTraceStorageExpireDayRequest_Validation_DaysAtMaxValue_PassesValidation()
    {
        var model = new SetTraceStorageExpireDayRequest { Days = 730 };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.True(isValid);
        Assert.Empty(results);
    }
}
