using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.IAM.ScimConfigurations.Item.RegenerateToken;

namespace Sacloud.Sdk.IAM.Tests.ScimConfigurations.Item.RegenerateToken;

/// <summary>
/// RegenerateTokenPostResponseのテスト
/// </summary>
public class RegenerateTokenPostResponseTests
{
    /// <summary>
    /// コンストラクタ実行後にAdditionalDataが空のDictionaryで初期化されることを検証
    /// </summary>
    [Fact]
    public void コンストラクタ_AdditionalDataが空のDictionaryで初期化される()
    {
        var response = new RegenerateTokenPostResponse();

        Assert.NotNull(response.AdditionalData);
        Assert.Empty(response.AdditionalData);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_インスタンスを返す()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = RegenerateTokenPostResponse.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<RegenerateTokenPostResponse>(result);
    }

    /// <summary>
    /// GetFieldDeserializersが"secret_token"キーを含むことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_secret_tokenキーを含む()
    {
        var response = new RegenerateTokenPostResponse();

        var deserializers = response.GetFieldDeserializers();

        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("secret_token"));
    }

    /// <summary>
    /// GetFieldDeserializersの"secret_token"アクションがSecretTokenプロパティをセットすることを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_secretTokenアクションがSecretTokenプロパティをセットする()
    {
        const string expected = "test-secret-token";
        var response = new RegenerateTokenPostResponse();
        var deserializers = response.GetFieldDeserializers();

        var parseNode = Substitute.For<IParseNode>();
        parseNode.GetStringValue().Returns(expected);

        deserializers["secret_token"](parseNode);

        Assert.Equal(expected, response.SecretToken);
    }

    /// <summary>
    /// SerializeがWriteStringValue("secret_token", ...)を1回呼び出すことを検証
    /// </summary>
    [Fact]
    public void Serialize_secretTokenプロパティを書き込む()
    {
        const string secretToken = "my-token";
        var response = new RegenerateTokenPostResponse { SecretToken = secretToken };
        var writer = Substitute.For<ISerializationWriter>();

        response.Serialize(writer);

        writer.Received(1).WriteStringValue("secret_token", secretToken);
    }

    /// <summary>
    /// SerializeがWriteAdditionalDataを1回呼び出すことを検証
    /// </summary>
    [Fact]
    public void Serialize_AdditionalDataを書き込む()
    {
        var response = new RegenerateTokenPostResponse();
        response.AdditionalData["key"] = "value";
        var writer = Substitute.For<ISerializationWriter>();

        response.Serialize(writer);

        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_nullのParseNodeでArgumentNullExceptionをスロー()
    {
        Assert.Throws<ArgumentNullException>(
            () => RegenerateTokenPostResponse.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_nullのwriterでArgumentNullExceptionをスロー()
    {
        var response = new RegenerateTokenPostResponse();

        Assert.Throws<ArgumentNullException>(() => response.Serialize(null));
    }
}
