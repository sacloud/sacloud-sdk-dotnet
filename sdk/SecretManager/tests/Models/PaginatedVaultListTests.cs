using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.SecretManager.Models;

namespace Sacloud.Sdk.SecretManager.Tests.Models;

/// <summary>
/// PaginatedVaultListモデルのテスト
/// </summary>
public class PaginatedVaultListTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void PaginatedVaultList_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = PaginatedVaultList.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<PaginatedVaultList>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void PaginatedVaultList_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() => PaginatedVaultList.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void PaginatedVaultList_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new PaginatedVaultList();

        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(4, deserializers.Count);
        Assert.True(deserializers.ContainsKey("Count"));
        Assert.True(deserializers.ContainsKey("From"));
        Assert.True(deserializers.ContainsKey("Total"));
        Assert.True(deserializers.ContainsKey("Vaults"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void PaginatedVaultList_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new PaginatedVaultList();

        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void PaginatedVaultList_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new PaginatedVaultList();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetIntValue().Returns(10);
        deserializers["Count"](parseNode);
        Assert.Equal(10, model.Count);

        parseNode.GetIntValue().Returns(0);
        deserializers["From"](parseNode);
        Assert.Equal(0, model.From);

        parseNode.GetIntValue().Returns(100);
        deserializers["Total"](parseNode);
        Assert.Equal(100, model.Total);

        var vaultsList = new List<Vault> { new Vault { Name = "test-vault" } };
        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<Vault>>()).Returns(vaultsList);
        deserializers["Vaults"](parseNode);
        Assert.NotNull(model.Vaults);
        Assert.Single(model.Vaults);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task PaginatedVaultList_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new PaginatedVaultList
        {
            Count = 10,
            From = 0,
            Total = 100,
            Vaults = new List<Vault>
            {
                new Vault { Name = "test-vault-1" },
                new Vault { Name = "test-vault-2" }
            }
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(PaginatedVaultList.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Count, deserializedModel.Count);
        Assert.Equal(originalModel.From, deserializedModel.From);
        Assert.Equal(originalModel.Total, deserializedModel.Total);
        Assert.NotNull(deserializedModel.Vaults);
        Assert.Equal(2, deserializedModel.Vaults.Count);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task PaginatedVaultList_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new PaginatedVaultList
        {
            Count = 10
        };
        originalModel.AdditionalData.Add("custom_field_1", "custom_value_1");
        originalModel.AdditionalData.Add("custom_field_2", 42);

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(PaginatedVaultList.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}
