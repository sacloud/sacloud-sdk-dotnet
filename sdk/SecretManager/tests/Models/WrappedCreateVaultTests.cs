using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.SecretManager.Models;

namespace Sacloud.Sdk.SecretManager.Tests.Models;

/// <summary>
/// WrappedCreateVaultモデルのテスト
/// </summary>
public class WrappedCreateVaultTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void WrappedCreateVault_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = WrappedCreateVault.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<WrappedCreateVault>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void WrappedCreateVault_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() => WrappedCreateVault.CreateFromDiscriminatorValue(parseNode!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void WrappedCreateVault_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new WrappedCreateVault();

        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("Vault"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void WrappedCreateVault_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new WrappedCreateVault();

        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void WrappedCreateVault_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new WrappedCreateVault();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        var vault = new CreateVault { Name = "test-vault" };
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<CreateVault>>()).Returns(vault);
        deserializers["Vault"](parseNode);
        Assert.NotNull(model.Vault);
        Assert.Equal("test-vault", model.Vault.Name);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task WrappedCreateVault_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new WrappedCreateVault
        {
            Vault = new CreateVault
            {
                Name = "test-vault",
                Description = "test-description"
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
        var deserializedModel = parseNode.GetObjectValue(WrappedCreateVault.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Vault);
        Assert.Equal(originalModel.Vault.Name, deserializedModel.Vault.Name);
        Assert.Equal(originalModel.Vault.Description, deserializedModel.Vault.Description);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task WrappedCreateVault_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new WrappedCreateVault
        {
            Vault = new CreateVault { Name = "test-vault" }
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
        var deserializedModel = parseNode.GetObjectValue(WrappedCreateVault.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_1"));
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom_field_2"));
    }
}
