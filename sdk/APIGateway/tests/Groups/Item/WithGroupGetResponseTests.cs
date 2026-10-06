using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Groups.Item;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Groups.Item;

/// <summary>
/// WithGroupGetResponseのテスト
/// </summary>
public class WithGroupGetResponseTests
{
    /// <summary>
    /// GetFieldDeserializersが"apigw"キーを返すことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ContainsApigwKey()
    {
        var response = new WithGroupGetResponse();

        var deserializers = response.GetFieldDeserializers();

        Assert.Contains("apigw", deserializers.Keys);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがWithGroupGetResponseインスタンスを作成することを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsWithGroupGetResponse()
    {
        var parseNode = Substitute.For<IParseNode>();

        var response = WithGroupGetResponse.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(response);
        Assert.IsType<WithGroupGetResponse>(response);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullパラメータで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => WithGroupGetResponse.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var response = new WithGroupGetResponse();

        Assert.Throws<ArgumentNullException>(() => response.Serialize(null));
    }

    /// <summary>
    /// SerializeとCreateFromDiscriminatorValueを通じたラウンドトリップが成功することを確認
    /// </summary>
    [Fact]
    public async Task Serialize_Roundtrip_Success()
    {
        var response = new WithGroupGetResponse
        {
            AdditionalData = new Dictionary<string, object>
            {
                { "test-key", "test-value" }
            }
        };

        var json = """
        {
            "apigw": {
                "id": "123e4567-e89b-12d3-a456-426614174000",
                "name": "test-group"
            }
        }
        """;
        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));
        var factory = new JsonParseNodeFactory();
        var parseNode = await factory.GetRootParseNodeAsync("application/json", stream);
        var deserialized = WithGroupGetResponse.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(deserialized);
        Assert.NotNull(deserialized.AdditionalData);
    }

    /// <summary>
    /// AdditionalDataが初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_InitializesAdditionalData()
    {
        var response = new WithGroupGetResponse();

        Assert.NotNull(response.AdditionalData);
        Assert.IsType<Dictionary<string, object>>(response.AdditionalData);
    }

    /// <summary>
    /// Apigwプロパティが正しく設定されることを確認
    /// </summary>
    [Fact]
    public void ApigwProperty_CanBeSet()
    {
        var response = new WithGroupGetResponse();
        var apigw = new WithGroupGetResponse_apigw { Group = new Group { Name = "test" } };

        response.Apigw = apigw;

        Assert.Equal(apigw, response.Apigw);
    }

    /// <summary>
    /// Serializeがapigwプロパティを正しく書き込むことを確認
    /// </summary>
    [Fact]
    public void Serialize_WritesApigwProperty()
    {
        var apigw = new WithGroupGetResponse_apigw { Group = new Group { Name = "test-group" } };
        var response = new WithGroupGetResponse { Apigw = apigw };
        var writer = Substitute.For<ISerializationWriter>();

        response.Serialize(writer);

        writer.Received(1).WriteObjectValue("apigw", apigw);
        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }

    /// <summary>
    /// Serializeがnullプロパティの場合でも動作することを確認
    /// </summary>
    [Fact]
    public void Serialize_WithNullApigw_Works()
    {
        var response = new WithGroupGetResponse();
        var writer = Substitute.For<ISerializationWriter>();

        response.Serialize(writer);

        writer.Received(1).WriteObjectValue<WithGroupGetResponse_apigw>("apigw", null);
        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }

    /// <summary>
    /// GetFieldDeserializersのapigwアクションが機能することを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ApigwAction_WorksCorrectly()
    {
        var response = new WithGroupGetResponse();
        var deserializers = response.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();
        var apigw = new WithGroupGetResponse_apigw { Group = new Group { Name = "test-group" } };
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<WithGroupGetResponse_apigw>>())
            .Returns(apigw);

        deserializers["apigw"](parseNode);

        Assert.Equal(apigw, response.Apigw);
    }
}
