using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.IAM.Models;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// ScimConfigurationPublicのテスト
/// </summary>
public class ScimConfigurationPublicTests
{
    /// <summary>
    /// コンストラクタ実行後にAdditionalDataが空のDictionaryで初期化されることを検証
    /// </summary>
    [Fact]
    public void コンストラクタ_AdditionalDataが空のDictionaryで初期化される()
    {
        var model = new ScimConfigurationPublic();

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

        var result = ScimConfigurationPublic.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<ScimConfigurationPublic>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_nullのParseNodeでArgumentNullExceptionをスロー()
    {
        Assert.Throws<ArgumentNullException>(() => ScimConfigurationPublic.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが全5キーを含むことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_全5キーを含む()
    {
        var model = new ScimConfigurationPublic();

        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(5, deserializers.Count);
        Assert.True(deserializers.ContainsKey("base_url"));
        Assert.True(deserializers.ContainsKey("created_at"));
        Assert.True(deserializers.ContainsKey("id"));
        Assert.True(deserializers.ContainsKey("name"));
        Assert.True(deserializers.ContainsKey("updated_at"));
    }

    /// <summary>
    /// GetFieldDeserializersのbase_urlアクションがBaseUrlプロパティをセットすることを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_base_urlアクションがBaseUrlプロパティをセットする()
    {
        var model = new ScimConfigurationPublic();
        var deserializers = model.GetFieldDeserializers();

        var parseNode = Substitute.For<IParseNode>();
        parseNode.GetStringValue().Returns("https://api.example.com/scim/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");

        deserializers["base_url"](parseNode);

        Assert.Equal("https://api.example.com/scim/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", model.BaseUrl);
    }

    /// <summary>
    /// GetFieldDeserializersのcreated_atアクションがCreatedAtプロパティをセットすることを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_created_atアクションがCreatedAtプロパティをセットする()
    {
        var model = new ScimConfigurationPublic();
        var deserializers = model.GetFieldDeserializers();

        var parseNode = Substitute.For<IParseNode>();
        parseNode.GetStringValue().Returns("2024-01-01T00:00:00Z");

        deserializers["created_at"](parseNode);

        Assert.Equal("2024-01-01T00:00:00Z", model.CreatedAt);
    }

    /// <summary>
    /// GetFieldDeserializersのidアクションがIdプロパティをセットすることを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_idアクションがIdプロパティをセットする()
    {
        var model = new ScimConfigurationPublic();
        var deserializers = model.GetFieldDeserializers();

        var expectedId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var parseNode = Substitute.For<IParseNode>();
        parseNode.GetGuidValue().Returns(expectedId);

        deserializers["id"](parseNode);

        Assert.Equal(expectedId, model.Id);
    }

    /// <summary>
    /// GetFieldDeserializersのnameアクションがNameプロパティをセットすることを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_nameアクションがNameプロパティをセットする()
    {
        var model = new ScimConfigurationPublic();
        var deserializers = model.GetFieldDeserializers();

        var parseNode = Substitute.For<IParseNode>();
        parseNode.GetStringValue().Returns("Test SCIM Configuration");

        deserializers["name"](parseNode);

        Assert.Equal("Test SCIM Configuration", model.Name);
    }

    /// <summary>
    /// GetFieldDeserializersのupdated_atアクションがUpdatedAtプロパティをセットすることを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_updated_atアクションがUpdatedAtプロパティをセットする()
    {
        var model = new ScimConfigurationPublic();
        var deserializers = model.GetFieldDeserializers();

        var parseNode = Substitute.For<IParseNode>();
        parseNode.GetStringValue().Returns("2024-06-01T00:00:00Z");

        deserializers["updated_at"](parseNode);

        Assert.Equal("2024-06-01T00:00:00Z", model.UpdatedAt);
    }

    /// <summary>
    /// SerializeがWriteStringValueを通じてnameプロパティを書き込むことを検証
    /// </summary>
    [Fact]
    public void Serialize_nameプロパティを書き込む()
    {
        var model = new ScimConfigurationPublic
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
        var model = new ScimConfigurationPublic();
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
        var model = new ScimConfigurationPublic();

        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }
}
