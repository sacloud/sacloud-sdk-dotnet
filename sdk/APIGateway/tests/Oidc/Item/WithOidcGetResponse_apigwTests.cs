using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;
using Sacloud.Sdk.APIGateway.Oidc.Item;

namespace Sacloud.Sdk.APIGateway.Tests.Oidc.Item;

/// <summary>
/// WithOidcGetResponse_apigwのテスト
/// </summary>
public class WithOidcGetResponse_apigwTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = WithOidcGetResponse_apigw.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<WithOidcGetResponse_apigw>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => WithOidcGetResponse_apigw.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var response = new WithOidcGetResponse_apigw();

        var deserializers = response.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.True(deserializers.ContainsKey("oidc"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var response = new WithOidcGetResponse_apigw();

        Assert.Throws<ArgumentNullException>(() => response.Serialize(null));
    }

    /// <summary>
    /// Serializeがoidcプロパティを書き込むことを検証
    /// </summary>
    [Fact]
    public void Serialize_WritesOidcProperty()
    {
        var oidc = new OidcDetail();
        var response = new WithOidcGetResponse_apigw
        {
            Oidc = oidc
        };
        var writer = Substitute.For<ISerializationWriter>();

        response.Serialize(writer);

        writer.Received(1).WriteObjectValue("oidc", oidc);
        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }

    /// <summary>
    /// Serializeがoidcがnullの場合でも正しく動作することを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullOidc_Works()
    {
        var response = new WithOidcGetResponse_apigw();
        var writer = Substitute.For<ISerializationWriter>();

        response.Serialize(writer);

        writer.Received(1).WriteObjectValue<OidcDetail>("oidc", null);
        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }

    /// <summary>
    /// GetFieldDeserializersのOidcアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_OidcAction_WorksCorrectly()
    {
        var response = new WithOidcGetResponse_apigw();
        var deserializers = response.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();
        var oidcValue = new OidcDetail();
        parseNode.GetObjectValue(
            Arg.Any<ParsableFactory<OidcDetail>>())
            .Returns(oidcValue);

        deserializers["oidc"](parseNode);

        Assert.Equal(oidcValue, response.Oidc);
    }

    /// <summary>
    /// AdditionalDataプロパティが初期化されることを検証
    /// </summary>
    [Fact]
    public void Constructor_InitializesAdditionalData()
    {
        var response = new WithOidcGetResponse_apigw();

        Assert.NotNull(response.AdditionalData);
        Assert.Empty(response.AdditionalData);
    }

    /// <summary>
    /// AdditionalDataに値を追加できることを検証
    /// </summary>
    [Fact]
    public void AdditionalData_CanStoreCustomData()
    {
        var response = new WithOidcGetResponse_apigw();
        var customKey = "customKey";
        var customValue = "customValue";

        response.AdditionalData[customKey] = customValue;

        Assert.True(response.AdditionalData.ContainsKey(customKey));
        Assert.Equal(customValue, response.AdditionalData[customKey]);
    }

    /// <summary>
    /// Oidcプロパティにnullを設定できることを検証
    /// </summary>
    [Fact]
    public void Oidc_CanBeSetToNull()
    {
        var response = new WithOidcGetResponse_apigw();
        var oidc = new OidcDetail();
        response.Oidc = oidc;

        response.Oidc = null;

        Assert.Null(response.Oidc);
    }

    /// <summary>
    /// Oidcプロパティに異なるOidcDetailオブジェクトを設定できることを検証
    /// </summary>
    [Fact]
    public void Oidc_CanBeSetToDifferentObjects()
    {
        var response = new WithOidcGetResponse_apigw();
        var oidc1 = new OidcDetail();
        var oidc2 = new OidcDetail();

        response.Oidc = oidc1;
        Assert.Equal(oidc1, response.Oidc);

        response.Oidc = oidc2;
        Assert.Equal(oidc2, response.Oidc);
    }

    /// <summary>
    /// Oidcプロパティのプロパティを設定できることを検証
    /// </summary>
    [Fact]
    public void Oidc_PropertiesCanBeSet()
    {
        var response = new WithOidcGetResponse_apigw();
        var oidc = new OidcDetail();

        response.Oidc = oidc;
        Assert.NotNull(response.Oidc);
        Assert.Same(oidc, response.Oidc);
    }
}
