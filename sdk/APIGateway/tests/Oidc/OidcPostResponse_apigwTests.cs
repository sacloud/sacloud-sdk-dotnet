using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Oidc;

namespace Sacloud.Sdk.APIGateway.Tests.Oidc;

/// <summary>
/// OidcPostResponse_apigwのテスト
/// </summary>
public class OidcPostResponse_apigwTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = OidcPostResponse_apigw.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<OidcPostResponse_apigw>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => OidcPostResponse_apigw.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var response = new OidcPostResponse_apigw();

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
        var response = new OidcPostResponse_apigw();

        Assert.Throws<ArgumentNullException>(() => response.Serialize(null));
    }

    /// <summary>
    /// Serializeがoidcプロパティを書き込むことを検証
    /// </summary>
    [Fact]
    public void Serialize_WritesOidcProperty()
    {
        var oidc = new APIGateway.Models.Oidc { Name = "Test OIDC" };
        var response = new OidcPostResponse_apigw
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
        var response = new OidcPostResponse_apigw();
        var writer = Substitute.For<ISerializationWriter>();

        response.Serialize(writer);

        writer.Received(1).WriteObjectValue<APIGateway.Models.Oidc>("oidc", null);
        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }

    /// <summary>
    /// GetFieldDeserializersのOidcアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_OidcAction_WorksCorrectly()
    {
        var response = new OidcPostResponse_apigw();
        var deserializers = response.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();
        var oidcValue = new APIGateway.Models.Oidc { Name = "Test OIDC" };
        parseNode.GetObjectValue(
            Arg.Any<ParsableFactory<APIGateway.Models.Oidc>>())
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
        var response = new OidcPostResponse_apigw();

        Assert.NotNull(response.AdditionalData);
        Assert.Empty(response.AdditionalData);
    }

    /// <summary>
    /// AdditionalDataに値を追加できることを検証
    /// </summary>
    [Fact]
    public void AdditionalData_CanStoreCustomData()
    {
        var response = new OidcPostResponse_apigw();
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
        var response = new OidcPostResponse_apigw();
        var oidc = new APIGateway.Models.Oidc { Name = "Test OIDC" };
        response.Oidc = oidc;

        response.Oidc = null;

        Assert.Null(response.Oidc);
    }

    /// <summary>
    /// Oidcプロパティに異なるOidcオブジェクトを設定できることを検証
    /// </summary>
    [Fact]
    public void Oidc_CanBeSetToDifferentObjects()
    {
        var response = new OidcPostResponse_apigw();
        var oidc1 = new APIGateway.Models.Oidc { Name = "OIDC 1" };
        var oidc2 = new APIGateway.Models.Oidc { Name = "OIDC 2" };

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
        var response = new OidcPostResponse_apigw();
        var oidc = new APIGateway.Models.Oidc();

        response.Oidc = oidc;
        Assert.NotNull(response.Oidc);
        Assert.Same(oidc, response.Oidc);
    }
}
