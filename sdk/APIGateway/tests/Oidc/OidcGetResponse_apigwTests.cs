using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Oidc;

namespace Sacloud.Sdk.APIGateway.Tests.Oidc;

/// <summary>
/// OidcGetResponse_apigwのテスト
/// </summary>
public class OidcGetResponse_apigwTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = OidcGetResponse_apigw.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<OidcGetResponse_apigw>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => OidcGetResponse_apigw.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var response = new OidcGetResponse_apigw();

        var deserializers = response.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.True(deserializers.ContainsKey("oidcs"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var response = new OidcGetResponse_apigw();

        Assert.Throws<ArgumentNullException>(() => response.Serialize(null));
    }

    /// <summary>
    /// Serializeがoidcsプロパティを書き込むことを検証
    /// </summary>
    [Fact]
    public void Serialize_WritesOidcsProperty()
    {
        var oidcs = new List<APIGateway.Models.Oidc>
        {
            new() { Name = "OIDC 1" },
            new() { Name = "OIDC 2" }
        };
        var response = new OidcGetResponse_apigw
        {
            Oidcs = oidcs
        };
        var writer = Substitute.For<ISerializationWriter>();

        response.Serialize(writer);

        writer.Received(1).WriteCollectionOfObjectValues("oidcs", oidcs);
        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }

    /// <summary>
    /// Serializeがoidcsがnullの場合でも正しく動作することを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullOidcs_Works()
    {
        var response = new OidcGetResponse_apigw();
        var writer = Substitute.For<ISerializationWriter>();

        response.Serialize(writer);

        writer.Received(1).WriteCollectionOfObjectValues<APIGateway.Models.Oidc>("oidcs", null);
        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }

    /// <summary>
    /// GetFieldDeserializersのOidcsアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_OidcsAction_WorksCorrectly()
    {
        var response = new OidcGetResponse_apigw();
        var deserializers = response.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();
        var oidcsValue = new List<APIGateway.Models.Oidc>
        {
            new() { Name = "OIDC 1" }
        };
        parseNode.GetCollectionOfObjectValues(
            Arg.Any<ParsableFactory<APIGateway.Models.Oidc>>())
            .Returns(oidcsValue);

        deserializers["oidcs"](parseNode);

        Assert.NotNull(response.Oidcs);
        Assert.Equal(oidcsValue.Count, response.Oidcs.Count);
    }

    /// <summary>
    /// AdditionalDataプロパティが初期化されることを検証
    /// </summary>
    [Fact]
    public void Constructor_InitializesAdditionalData()
    {
        var response = new OidcGetResponse_apigw();

        Assert.NotNull(response.AdditionalData);
        Assert.Empty(response.AdditionalData);
    }

    /// <summary>
    /// AdditionalDataに値を追加できることを検証
    /// </summary>
    [Fact]
    public void AdditionalData_CanStoreCustomData()
    {
        var response = new OidcGetResponse_apigw();
        var customKey = "customKey";
        var customValue = "customValue";

        response.AdditionalData[customKey] = customValue;

        Assert.True(response.AdditionalData.ContainsKey(customKey));
        Assert.Equal(customValue, response.AdditionalData[customKey]);
    }

    /// <summary>
    /// Oidcsプロパティにnullを設定できることを検証
    /// </summary>
    [Fact]
    public void Oidcs_CanBeSetToNull()
    {
        var response = new OidcGetResponse_apigw();
        var oidcs = new List<APIGateway.Models.Oidc> { new() { Name = "OIDC" } };
        response.Oidcs = oidcs;

        response.Oidcs = null;

        Assert.Null(response.Oidcs);
    }

    /// <summary>
    /// Oidcsプロパティに複数のOidcオブジェクトを追加できることを検証
    /// </summary>
    [Fact]
    public void Oidcs_CanStoreMultipleOidcObjects()
    {
        var response = new OidcGetResponse_apigw();
        var oidc1 = new APIGateway.Models.Oidc { Name = "OIDC 1" };
        var oidc2 = new APIGateway.Models.Oidc { Name = "OIDC 2" };
        response.Oidcs = [oidc1, oidc2];

        Assert.NotNull(response.Oidcs);
        Assert.Equal(2, response.Oidcs.Count);
        Assert.Contains(oidc1, response.Oidcs);
        Assert.Contains(oidc2, response.Oidcs);
    }
}
