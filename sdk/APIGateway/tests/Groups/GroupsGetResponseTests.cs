using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Groups;

namespace Sacloud.Sdk.APIGateway.Tests.Groups;

/// <summary>
/// GroupsGetResponseのテスト
/// </summary>
public class GroupsGetResponseTests
{
    /// <summary>
    /// GetFieldDeserializersが"apigw"キーを返すことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ContainsApigwKey()
    {
        var response = new GroupsGetResponse();
        var deserializers = response.GetFieldDeserializers();
        Assert.Contains("apigw", deserializers.Keys);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがGroupsGetResponseインスタンスを作成することを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsGroupsGetResponse()
    {
        var parseNode = Substitute.For<IParseNode>();
        var response = GroupsGetResponse.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(response);
        Assert.IsType<GroupsGetResponse>(response);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullパラメータで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => GroupsGetResponse.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var response = new GroupsGetResponse();
        Assert.Throws<ArgumentNullException>(() => response.Serialize(null));
    }

    /// <summary>
    /// SerializeとCreateFromDiscriminatorValueを通じたラウンドトリップが成功することを確認
    /// </summary>
    [Fact]
    public async Task Serialize_Roundtrip_Success()
    {
        var response = new GroupsGetResponse
        {
            AdditionalData = new Dictionary<string, object>
            {
                { "test-key", "test-value" }
            }
        };
        var json = """
        {
            "apigw": {
                "groups": []
            }
        }
        """;
        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));
        var factory = new JsonParseNodeFactory();
        var parseNode = await factory.GetRootParseNodeAsync("application/json", stream);
        var deserialized = GroupsGetResponse.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(deserialized);
        Assert.NotNull(deserialized.AdditionalData);
    }

    /// <summary>
    /// AdditionalDataが初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_InitializesAdditionalData()
    {
        var response = new GroupsGetResponse();
        Assert.NotNull(response.AdditionalData);
        Assert.Empty(response.AdditionalData);
    }

    /// <summary>
    /// Apigwプロパティが正しく設定されることを確認
    /// </summary>
    [Fact]
    public void ApigwProperty_CanBeSet()
    {
        var response = new GroupsGetResponse();
        var apigw = new GroupsGetResponse_apigw();
        response.Apigw = apigw;
        Assert.Equal(apigw, response.Apigw);
    }

    /// <summary>
    /// Serializeがapigwプロパティを正しく書き込むことを確認
    /// </summary>
    [Fact]
    public void Serialize_WritesApigwProperty()
    {
        var apigw = new GroupsGetResponse_apigw();
        var response = new GroupsGetResponse { Apigw = apigw };
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
        var response = new GroupsGetResponse();
        var writer = Substitute.For<ISerializationWriter>();
        response.Serialize(writer);
        writer.Received(1).WriteObjectValue<GroupsGetResponse_apigw>("apigw", null);
        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }

    /// <summary>
    /// GetFieldDeserializersのapigwアクションが機能することを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ApigwAction_WorksCorrectly()
    {
        var response = new GroupsGetResponse();
        var deserializers = response.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();
        var apigw = new GroupsGetResponse_apigw();
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<GroupsGetResponse_apigw>>())
            .Returns(apigw);
        deserializers["apigw"](parseNode);
        Assert.Equal(apigw, response.Apigw);
    }
}
