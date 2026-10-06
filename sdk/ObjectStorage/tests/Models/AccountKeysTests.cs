using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Models;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models;

/// <summary>
/// AccountKeysモデルのテスト
/// </summary>
public class AccountKeysTests
{
    /// <summary>
    /// GetFieldDeserializersがdataキーを含む正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void AccountKeys_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new AccountKeys();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.Contains("data", deserializers.Keys);
    }

    /// <summary>
    /// SerializeがnullのwriterでArgumentNullExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void AccountKeys_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new AccountKeys();

        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションがdataを正しく設定することを確認
    /// </summary>
    [Fact]
    public void AccountKeys_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new AccountKeys();
        var data = new AccountKeys_data();
        var parseNode = Substitute.For<IParseNode>();
        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<AccountKeys_data>>()).Returns([data]);

        foreach (var action in model.GetFieldDeserializers().Values)
        {
            action(parseNode);
        }

        Assert.NotNull(model.Data);
        Assert.Single(model.Data);
        Assert.Same(data, model.Data[0]);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task AccountKeys_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new AccountKeys
        {
            Data =
            [
                new AccountKeys_data
                {
                    CreatedAt = DateTimeOffset.Parse("2026-01-01T00:00:00+00:00"),
                    Id = "access-key-id",
                },
            ],
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        writer.GetSerializedContent().CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNode = await new JsonParseNodeFactory().GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(AccountKeys.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Data);
        Assert.Single(deserializedModel.Data);
        Assert.Equal(originalModel.Data[0].CreatedAt, deserializedModel.Data[0].CreatedAt);
        Assert.Equal(originalModel.Data[0].Id, deserializedModel.Data[0].Id);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task AccountKeys_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new AccountKeys
        {
            AdditionalData = new Dictionary<string, object>
            {
                { "extra_key", "extra-value" },
            },
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, model);
        writer.GetSerializedContent().CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNode = await new JsonParseNodeFactory().GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(AccountKeys.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}