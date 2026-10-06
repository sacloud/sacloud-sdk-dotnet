using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.IAM.Models;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// ScimConfigurationのテスト
/// </summary>
public class ScimConfigurationTests
{
    /// <summary>
    /// コンストラクタ実行後にAdditionalDataが空のDictionaryで初期化されることを検証
    /// </summary>
    [Fact]
    public void コンストラクタ_AdditionalDataが空のDictionaryで初期化される()
    {
        var model = new ScimConfiguration();

        Assert.NotNull(model.AdditionalData);
        Assert.Empty(model.AdditionalData);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_インスタンスを返す()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = ScimConfiguration.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<ScimConfiguration>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_nullのParseNodeでArgumentNullExceptionをスロー()
    {
        Assert.Throws<ArgumentNullException>(() => ScimConfiguration.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが基底クラスのキーを含む全6キーを含むことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_全6キーを含む()
    {
        var model = new ScimConfiguration();

        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(6, deserializers.Count);
        Assert.True(deserializers.ContainsKey("base_url"));
        Assert.True(deserializers.ContainsKey("created_at"));
        Assert.True(deserializers.ContainsKey("id"));
        Assert.True(deserializers.ContainsKey("name"));
        Assert.True(deserializers.ContainsKey("updated_at"));
        Assert.True(deserializers.ContainsKey("secret_token"));
    }

    /// <summary>
    /// GetFieldDeserializersのsecret_tokenアクションがSecretTokenプロパティをセットすることを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_secret_tokenアクションがSecretTokenプロパティをセットする()
    {
        var model = new ScimConfiguration();
        var deserializers = model.GetFieldDeserializers();

        var parseNode = Substitute.For<IParseNode>();
        parseNode.GetStringValue().Returns("test-secret-token");

        deserializers["secret_token"](parseNode);

        Assert.Equal("test-secret-token", model.SecretToken);
    }

    /// <summary>
    /// SerializeがWriteStringValueを通じてnameプロパティを書き込むことを検証
    /// </summary>
    [Fact]
    public void Serialize_nameプロパティを書き込む()
    {
        var model = new ScimConfiguration
        {
            Name = "Test SCIM Configuration",
        };
        var writer = Substitute.For<ISerializationWriter>();

        model.Serialize(writer);

        writer.Received(1).WriteStringValue("name", "Test SCIM Configuration");
    }

    /// <summary>
    /// SerializeがWriteAdditionalDataを呼び出すことを検証
    /// </summary>
    [Fact]
    public void Serialize_AdditionalDataを書き込む()
    {
        var model = new ScimConfiguration();
        var writer = Substitute.For<ISerializationWriter>();

        model.Serialize(writer);

        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_nullのwriterでArgumentNullExceptionをスロー()
    {
        var model = new ScimConfiguration();

        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }
}
