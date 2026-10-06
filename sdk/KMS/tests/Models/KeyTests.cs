using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.KMS.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.KMS.Tests.Models;

/// <summary>
/// Keyモデルのテスト
/// </summary>
public class KeyTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void Key_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = Key.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<Key>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Key_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() => Key.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void Key_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new Key();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(10, deserializers.Count);
        Assert.True(deserializers.ContainsKey("CreatedAt"));
        Assert.True(deserializers.ContainsKey("Description"));
        Assert.True(deserializers.ContainsKey("ID"));
        Assert.True(deserializers.ContainsKey("KeyOrigin"));
        Assert.True(deserializers.ContainsKey("LatestVersion"));
        Assert.True(deserializers.ContainsKey("ModifiedAt"));
        Assert.True(deserializers.ContainsKey("Name"));
        Assert.True(deserializers.ContainsKey("ServiceClass"));
        Assert.True(deserializers.ContainsKey("Status"));
        Assert.True(deserializers.ContainsKey("Tags"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Key_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new Key();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void Key_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new Key();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        var testDate = DateTimeOffset.Parse("2024-01-15T10:30:00Z");
        parseNode.GetDateTimeOffsetValue().Returns(testDate);
        deserializers["CreatedAt"](parseNode);
        Assert.Equal(testDate, model.CreatedAt);

        parseNode.GetStringValue().Returns("Test description");
        deserializers["Description"](parseNode);
        Assert.Equal("Test description", model.Description);

        parseNode.GetStringValue().Returns("key-123");
        deserializers["ID"](parseNode);
        Assert.Equal("key-123", model.ID);

        parseNode.GetEnumValue<KeyOriginEnum>().Returns(KeyOriginEnum.Generated);
        deserializers["KeyOrigin"](parseNode);
        Assert.Equal(KeyOriginEnum.Generated, model.KeyOrigin);

        parseNode.GetIntValue().Returns(5);
        deserializers["LatestVersion"](parseNode);
        Assert.Equal(5, model.LatestVersion);

        var modifiedDate = DateTimeOffset.Parse("2024-02-20T15:45:00Z");
        parseNode.GetDateTimeOffsetValue().Returns(modifiedDate);
        deserializers["ModifiedAt"](parseNode);
        Assert.Equal(modifiedDate, model.ModifiedAt);

        parseNode.GetStringValue().Returns("test-key-name");
        deserializers["Name"](parseNode);
        Assert.Equal("test-key-name", model.Name);

        parseNode.GetEnumValue<KeyServiceClassEnum>().Returns(KeyServiceClassEnum.CloudKmsKey);
        deserializers["ServiceClass"](parseNode);
        Assert.Equal(KeyServiceClassEnum.CloudKmsKey, model.ServiceClass);

        parseNode.GetEnumValue<KeyStatusEnum>().Returns(KeyStatusEnum.Active);
        deserializers["Status"](parseNode);
        Assert.Equal(KeyStatusEnum.Active, model.Status);

        var tagsList = new List<string> { "tag1", "tag2" };
        parseNode.GetCollectionOfPrimitiveValues<string>().Returns(tagsList);
        deserializers["Tags"](parseNode);
        Assert.NotNull(model.Tags);
        Assert.Equal(2, model.Tags.Count);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task Key_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new Key
        {
            Description = "Test key description",
            Name = "test-key-name",
            Tags = new List<string> { "tag1", "tag2" }
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        await content.CopyToAsync(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Key.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Description, deserializedModel.Description);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.NotNull(deserializedModel.Tags);
        Assert.Equal(2, deserializedModel.Tags.Count);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task Key_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new Key
        {
            Name = "test-key"
        };
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
        var deserializedModel = parseNode.GetObjectValue(Key.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }

    /// <summary>
    /// ISO 8601形式の日時文字列が正しくパースされることを確認
    /// </summary>
    [Theory]
    [InlineData("{\"CreatedAt\":\"2024-01-15T10:30:00Z\"}", 2024, 1, 15, 10, 30, 0)]
    [InlineData("{\"CreatedAt\":\"2024-12-31T23:59:59Z\"}", 2024, 12, 31, 23, 59, 59)]
    [InlineData("{\"CreatedAt\":\"2024-01-01T00:00:00+09:00\"}", 2024, 1, 1, 0, 0, 0)]
    [InlineData("{\"CreatedAt\":\"2024-06-15T12:30:45.123Z\"}", 2024, 6, 15, 12, 30, 45)]
    public async Task Key_CanParseIso8601DateTimeFormats(string json, int year, int month, int day, int hour, int minute, int second)
    {
        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", stream);
        var model = parseNode.GetObjectValue(Key.CreateFromDiscriminatorValue);

        Assert.NotNull(model);
        Assert.NotNull(model.CreatedAt);
        Assert.Equal(year, model.CreatedAt.Value.Year);
        Assert.Equal(month, model.CreatedAt.Value.Month);
        Assert.Equal(day, model.CreatedAt.Value.Day);
        Assert.Equal(hour, model.CreatedAt.Value.Hour);
        Assert.Equal(minute, model.CreatedAt.Value.Minute);
        Assert.Equal(second, model.CreatedAt.Value.Second);
    }

    /// <summary>
    /// 日時情報がシリアライズ後も欠損しないことを確認
    /// </summary>
    [Fact]
    public async Task Key_DateTimeInfoIsPreservedAfterSerialization()
    {
        var createdAtJson = "{\"CreatedAt\":\"2024-03-15T14:30:45.123+09:00\",\"ModifiedAt\":\"2024-03-20T09:15:30.456Z\"}";
        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(createdAtJson));
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", stream);
        var model = parseNode.GetObjectValue(Key.CreateFromDiscriminatorValue);

        Assert.NotNull(model);
        Assert.NotNull(model.CreatedAt);
        Assert.NotNull(model.ModifiedAt);

        Assert.Equal(2024, model.CreatedAt.Value.Year);
        Assert.Equal(3, model.CreatedAt.Value.Month);
        Assert.Equal(15, model.CreatedAt.Value.Day);

        Assert.Equal(2024, model.ModifiedAt.Value.Year);
        Assert.Equal(3, model.ModifiedAt.Value.Month);
        Assert.Equal(20, model.ModifiedAt.Value.Day);
        Assert.Equal(9, model.ModifiedAt.Value.Hour);
        Assert.Equal(15, model.ModifiedAt.Value.Minute);
        Assert.Equal(30, model.ModifiedAt.Value.Second);
    }

    /// <summary>
    /// Nameがnullの場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void Key_Validation_WithNullName_ReturnsValidationError()
    {
        var model = new Key
        {
            Description = "Test description",
            Tags = new List<string> { "tag1" }
        };

        var validationContext = new ValidationContext(model);
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.MemberNames.Contains("Name"));
    }

    /// <summary>
    /// Descriptionがnullの場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void Key_Validation_WithNullDescription_ReturnsValidationError()
    {
        var model = new Key
        {
            Name = "Test name",
            Tags = new List<string> { "tag1" }
        };

        var validationContext = new ValidationContext(model);
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.MemberNames.Contains("Description"));
    }

    /// <summary>
    /// Tagsがnullの場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void Key_Validation_WithNullTags_ReturnsValidationError()
    {
        var model = new Key
        {
            Name = "Test name",
            Description = "Test description"
        };

        var validationContext = new ValidationContext(model);
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.MemberNames.Contains("Tags"));
    }

    /// <summary>
    /// Nameが255文字を超える場合にバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void Key_Validation_WithNameExceedingMaxLength_ReturnsValidationError()
    {
        var model = new Key
        {
            Name = new string('a', 256),
            Description = "Test description",
            Tags = new List<string> { "tag1" }
        };

        var validationContext = new ValidationContext(model);
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.MemberNames.Contains("Name"));
    }

    /// <summary>
    /// すべての必須フィールドが設定されている場合にバリデーションが成功することを確認
    /// </summary>
    [Fact]
    public void Key_Validation_WithAllRequiredFields_PassesValidation()
    {
        var model = new Key
        {
            Name = "Test name",
            Description = "Test description",
            Tags = new List<string> { "tag1", "tag2" }
        };

        var validationContext = new ValidationContext(model);
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

        Assert.True(isValid);
        Assert.Empty(validationResults);
    }

    /// <summary>
    /// 空のTagsリストでバリデーションエラーになることを確認
    /// </summary>
    [Fact]
    public void Key_Validation_WithEmptyTagsList_ReturnsValidationError()
    {
        var model = new Key
        {
            Name = "Test name",
            Description = "Test description",
            Tags = new List<string>()
        };

        var validationContext = new ValidationContext(model);
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.MemberNames.Contains("Tags"));
    }
}
