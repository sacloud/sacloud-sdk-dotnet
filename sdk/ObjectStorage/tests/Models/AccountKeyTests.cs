using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Models;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models;

/// <summary>
/// AccountKeyモデルのテスト
/// </summary>
public class AccountKeyTests
{
    /// <summary>
    /// GetFieldDeserializersがdataキーを含む正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void AccountKey_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new AccountKey();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.Contains("data", deserializers.Keys);
    }

    /// <summary>
    /// SerializeがnullのwriterでArgumentNullExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void AccountKey_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new AccountKey();

        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションがdataを正しく設定することを確認
    /// </summary>
    [Fact]
    public void AccountKey_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new AccountKey();
        var parseNode = Substitute.For<IParseNode>();
        var data = new AccountKey_data();
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<AccountKey_data>>()).Returns(data);

        foreach (var action in model.GetFieldDeserializers().Values)
        {
            action(parseNode);
        }

        Assert.Same(data, model.Data);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task AccountKey_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new AccountKey
        {
            Data = new AccountKey_data
            {
                CreatedAt = DateTimeOffset.Parse("2026-01-01T00:00:00+00:00"),
                Id = "access-key-id",
                Secret = "secret-access-key",
            },
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(AccountKey.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Data);
        Assert.Equal(originalModel.Data.CreatedAt, deserializedModel.Data.CreatedAt);
        Assert.Equal(originalModel.Data.Id, deserializedModel.Data.Id);
        Assert.Equal(originalModel.Data.Secret, deserializedModel.Data.Secret);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task AccountKey_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new AccountKey
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
        var deserializedModel = parseNode.GetObjectValue(AccountKey.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
