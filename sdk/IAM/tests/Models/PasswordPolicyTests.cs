using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.IAM.Models;
using PasswordPolicyModel = Sacloud.Sdk.IAM.Models.PasswordPolicy;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// PasswordPolicyのテスト
/// </summary>
public class PasswordPolicyTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = PasswordPolicyModel.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<PasswordPolicyModel>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Sacloud.Sdk.IAM.Models.PasswordPolicy.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"min_length", "require_lowercase", "require_symbols", "require_uppercase"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var passwordPolicy = new PasswordPolicyModel();

        var deserializers = passwordPolicy.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(4, deserializers.Count);
        Assert.True(deserializers.ContainsKey("min_length"));
        Assert.True(deserializers.ContainsKey("require_lowercase"));
        Assert.True(deserializers.ContainsKey("require_symbols"));
        Assert.True(deserializers.ContainsKey("require_uppercase"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var passwordPolicy = new PasswordPolicyModel();

        Assert.Throws<ArgumentNullException>(() => passwordPolicy.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクション（"min_length", "require_lowercase", "require_symbols", "require_uppercase"）が正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var passwordPolicy = new PasswordPolicyModel();
        var deserializers = passwordPolicy.GetFieldDeserializers();
        
        var parseNodeMinLength = Substitute.For<IParseNode>();
        parseNodeMinLength.GetIntValue().Returns(12);
        deserializers["min_length"](parseNodeMinLength);
        
        var parseNodeRequireLowercase = Substitute.For<IParseNode>();
        parseNodeRequireLowercase.GetBoolValue().Returns(true);
        deserializers["require_lowercase"](parseNodeRequireLowercase);

        var parseNodeRequireSymbols = Substitute.For<IParseNode>();
        parseNodeRequireSymbols.GetBoolValue().Returns(true);
        deserializers["require_symbols"](parseNodeRequireSymbols);

        var parseNodeRequireUppercase = Substitute.For<IParseNode>();
        parseNodeRequireUppercase.GetBoolValue().Returns(false);
        deserializers["require_uppercase"](parseNodeRequireUppercase);

        Assert.Equal(12, passwordPolicy.MinLength);
        Assert.True(passwordPolicy.RequireLowercase);
        Assert.True(passwordPolicy.RequireSymbols);
        Assert.False(passwordPolicy.RequireUppercase);
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task PasswordPolicy_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new PasswordPolicyModel
        {
            MinLength = 16,
            RequireLowercase = true,
            RequireSymbols = true,
            RequireUppercase = true
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(PasswordPolicyModel.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(16, deserializedModel.MinLength);
        Assert.True(deserializedModel.RequireLowercase);
        Assert.True(deserializedModel.RequireSymbols);
        Assert.True(deserializedModel.RequireUppercase);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task PasswordPolicy_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new PasswordPolicyModel
        {
            MinLength = 10,
            RequireLowercase = false
        };
        originalModel.AdditionalData.Add("customField", "customValue");

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(PasswordPolicyModel.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(10, deserializedModel.MinLength);
        Assert.False(deserializedModel.RequireLowercase);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }

}
