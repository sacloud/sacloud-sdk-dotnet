using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.CloudHSM.Models;

namespace Sacloud.Sdk.CloudHSM.Tests.Models;

/// <summary>
/// CreateCloudHSMSoftwareLicenseモデルのテスト
/// </summary>
public class CreateCloudHSMSoftwareLicenseTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void CreateCloudHSMSoftwareLicense_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new global::Sacloud.Sdk.CloudHSM.Models.CreateCloudHSMSoftwareLicense();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(7, deserializers.Count);
        Assert.True(deserializers.ContainsKey("CreatedAt"));
        Assert.True(deserializers.ContainsKey("Description"));
        Assert.True(deserializers.ContainsKey("ID"));
        Assert.True(deserializers.ContainsKey("ModifiedAt"));
        Assert.True(deserializers.ContainsKey("Name"));
        Assert.True(deserializers.ContainsKey("ServiceClass"));
        Assert.True(deserializers.ContainsKey("Tags"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateCloudHSMSoftwareLicense_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new global::Sacloud.Sdk.CloudHSM.Models.CreateCloudHSMSoftwareLicense();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void CreateCloudHSMSoftwareLicense_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new global::Sacloud.Sdk.CloudHSM.Models.CreateCloudHSMSoftwareLicense();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("CreatedAt"));

        parseNode.GetDateTimeOffsetValue().Returns(DateTimeOffset.Parse("2026-01-01T00:00:00+00:00"));

        deserializers["CreatedAt"](parseNode);
        Assert.NotNull(model.CreatedAt);

        Assert.True(deserializers.ContainsKey("Description"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["Description"](parseNode);
        Assert.NotNull(model.Description);

        Assert.True(deserializers.ContainsKey("ID"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["ID"](parseNode);
        Assert.NotNull(model.ID);

        Assert.True(deserializers.ContainsKey("ModifiedAt"));

        parseNode.GetDateTimeOffsetValue().Returns(DateTimeOffset.Parse("2026-01-01T00:00:00+00:00"));

        deserializers["ModifiedAt"](parseNode);
        Assert.NotNull(model.ModifiedAt);

        Assert.True(deserializers.ContainsKey("Name"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["Name"](parseNode);
        Assert.NotNull(model.Name);

        Assert.True(deserializers.ContainsKey("ServiceClass"));

        parseNode.GetEnumValue<Sacloud.Sdk.CloudHSM.Models.CloudHSMSoftwareLicenseServiceClassEnum>().Returns((Sacloud.Sdk.CloudHSM.Models.CloudHSMSoftwareLicenseServiceClassEnum)0);

        deserializers["ServiceClass"](parseNode);
        Assert.NotNull(model.ServiceClass);

        Assert.True(deserializers.ContainsKey("Tags"));

        Assert.NotNull(parseNode);

        deserializers["Tags"](parseNode);
        Assert.NotNull(model.Tags);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CreateCloudHSMSoftwareLicense_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new global::Sacloud.Sdk.CloudHSM.Models.CreateCloudHSMSoftwareLicense();
        originalModel.Description = "sample";
        originalModel.Name = "sample";
        originalModel.Tags = [];

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue<global::Sacloud.Sdk.CloudHSM.Models.CreateCloudHSMSoftwareLicense>(global::Sacloud.Sdk.CloudHSM.Models.CreateCloudHSMSoftwareLicense.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Description, deserializedModel!.Description);
        Assert.Equal(originalModel.Name, deserializedModel!.Name);
        Assert.Equal(originalModel.ServiceClass, deserializedModel!.ServiceClass);
        Assert.NotNull(deserializedModel!.Tags);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task CreateCloudHSMSoftwareLicense_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new global::Sacloud.Sdk.CloudHSM.Models.CreateCloudHSMSoftwareLicense
        {
            AdditionalData = new Dictionary<string, object>
            {
                { "extra_key", "extra-value" },
            },
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, model);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue<global::Sacloud.Sdk.CloudHSM.Models.CreateCloudHSMSoftwareLicense>(global::Sacloud.Sdk.CloudHSM.Models.CreateCloudHSMSoftwareLicense.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel!.AdditionalData.ContainsKey("extra_key"));
    }

    /// <summary>
    /// 必須プロパティが不足している場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void CreateCloudHSMSoftwareLicense_Validation_WithMissingRequiredProperties_ReturnsValidationErrors()
    {
        var model = new CreateCloudHSMSoftwareLicense();
        var validationContext = new ValidationContext(model);
        var validationResults = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.MemberNames.Contains(nameof(CreateCloudHSMSoftwareLicense.Name)));
        Assert.Contains(validationResults, r => r.MemberNames.Contains(nameof(CreateCloudHSMSoftwareLicense.ServiceClass)));
        Assert.Contains(validationResults, r => r.MemberNames.Contains(nameof(CreateCloudHSMSoftwareLicense.Tags)));
    }

    /// <summary>
    /// Nameが最大長を超える場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void CreateCloudHSMSoftwareLicense_Validation_WithTooLongName_ReturnsValidationError()
    {
        var model = new CreateCloudHSMSoftwareLicense
        {
            Name = new string('a', 256),
            ServiceClass = CloudHSMSoftwareLicenseServiceClassEnum.CloudCloudhsmLicenseL7,
            Tags = new List<string> { "tag1" }
        };
        var validationContext = new ValidationContext(model);
        var validationResults = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.MemberNames.Contains(nameof(CreateCloudHSMSoftwareLicense.Name)));
    }

    /// <summary>
    /// Tagsがnullの場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void CreateCloudHSMSoftwareLicense_Validation_WithNullTags_ReturnsValidationError()
    {
        var model = new CreateCloudHSMSoftwareLicense
        {
            Name = "Test name",
            ServiceClass = CloudHSMSoftwareLicenseServiceClassEnum.CloudCloudhsmLicenseL7
        };

        var validationContext = new ValidationContext(model);
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.MemberNames.Contains(nameof(CreateCloudHSMSoftwareLicense.Tags)));
    }

    /// <summary>
    /// 空のTagsリストでバリデーションが成功することを確認
    /// </summary>
    [Fact]
    public void CreateCloudHSMSoftwareLicense_Validation_WithEmptyTagsList_ReturnsNoValidationError()
    {
        var model = new CreateCloudHSMSoftwareLicense
        {
            Name = "Test name",
            ServiceClass = CloudHSMSoftwareLicenseServiceClassEnum.CloudCloudhsmLicenseL7,
            Tags = new List<string>()
        };

        var validationContext = new ValidationContext(model);
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

        Assert.True(isValid);
        Assert.DoesNotContain(validationResults, r => r.MemberNames.Contains(nameof(CreateCloudHSMSoftwareLicense.Tags)));
    }

    [Fact]
    public void CreateCloudHSMSoftwareLicense_Validation_WithUndefinedServiceClass_ReturnsValidationError()
    {
        var model = new CreateCloudHSMSoftwareLicense
        {
            Name = "Test name",
            ServiceClass = (CloudHSMSoftwareLicenseServiceClassEnum)int.MaxValue,
            Tags = new List<string> { "tag1" }
        };
        var validationContext = new ValidationContext(model);
        var validationResults = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.MemberNames.Contains(nameof(CreateCloudHSMSoftwareLicense.ServiceClass)));
    }
}
