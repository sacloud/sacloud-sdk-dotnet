using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.IAM.Models;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// AuthConditions_require_two_factor_authのテスト
/// </summary>
public class AuthConditions_require_two_factor_authTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = AuthConditions_require_two_factor_auth.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<AuthConditions_require_two_factor_auth>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() =>
            AuthConditions_require_two_factor_auth.CreateFromDiscriminatorValue(parseNode));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var twoFactorAuth = new AuthConditions_require_two_factor_auth();

        var deserializers = twoFactorAuth.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("enabled"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var twoFactorAuth = new AuthConditions_require_two_factor_auth();

        Assert.Throws<ArgumentNullException>(() => twoFactorAuth.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var twoFactorAuth = new AuthConditions_require_two_factor_auth();
        var deserializers = twoFactorAuth.GetFieldDeserializers();

        var parseNodeEnabled = Substitute.For<IParseNode>();
        parseNodeEnabled.GetBoolValue().Returns(true);
        deserializers["enabled"](parseNodeEnabled);

        Assert.True(twoFactorAuth.Enabled);
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを確認
    /// </summary>
    [Fact]
    public async Task AuthConditions_require_two_factor_auth_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new AuthConditions_require_two_factor_auth
        {
            Enabled = true
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(AuthConditions_require_two_factor_auth.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.Enabled);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを確認
    /// </summary>
    [Fact]
    public async Task AuthConditions_require_two_factor_auth_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new AuthConditions_require_two_factor_auth();
        originalModel.AdditionalData.Add("customField", "customValue");

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(AuthConditions_require_two_factor_auth.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
