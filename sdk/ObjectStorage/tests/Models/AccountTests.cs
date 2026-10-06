using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Models;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models;

/// <summary>
/// Accountモデルのテスト
/// </summary>
public class AccountTests
{
    /// <summary>
    /// GetFieldDeserializersがdataキーを含む正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void Account_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new ObjectStorage.Models.Account();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.Contains("data", deserializers.Keys);
    }

    /// <summary>
    /// SerializeがnullのwriterでArgumentNullExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void Account_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ObjectStorage.Models.Account();

        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションがdataを正しく設定することを確認
    /// </summary>
    [Fact]
    public void Account_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new ObjectStorage.Models.Account();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();
        var data = new Account_data();
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<Account_data>>()).Returns(data);

        foreach (var action in deserializers.Values)
        {
            action(parseNode);
        }

        Assert.Same(data, model.Data);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task Account_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ObjectStorage.Models.Account
        {
            Data = new Account_data
            {
                Code = "code",
                CreatedAt = DateTimeOffset.Parse("2026-01-01T00:00:00+00:00"),
                ResourceId = "resource-id",
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
        var deserializedModel = parseNode.GetObjectValue(ObjectStorage.Models.Account.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Data);
        Assert.Equal(originalModel.Data.Code, deserializedModel.Data.Code);
        Assert.Equal(originalModel.Data.CreatedAt, deserializedModel.Data.CreatedAt);
        Assert.Equal(originalModel.Data.ResourceId, deserializedModel.Data.ResourceId);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task Account_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new ObjectStorage.Models.Account
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
        var deserializedModel = parseNode.GetObjectValue(ObjectStorage.Models.Account.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
