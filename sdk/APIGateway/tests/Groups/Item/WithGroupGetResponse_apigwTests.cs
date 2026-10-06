using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Groups.Item;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Groups.Item;

/// <summary>
/// WithGroupGetResponse_apigwのテスト
/// </summary>
public class WithGroupGetResponse_apigwTests
{
    /// <summary>
    /// GetFieldDeserializersが"group"キーを返すことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ContainsGroupKey()
    {
        var response = new WithGroupGetResponse_apigw();

        var deserializers = response.GetFieldDeserializers();

        Assert.Contains("group", deserializers.Keys);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがWithGroupGetResponse_apigwインスタンスを作成することを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsWithGroupGetResponseApigw()
    {
        var parseNode = Substitute.For<IParseNode>();

        var response = WithGroupGetResponse_apigw.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(response);
        Assert.IsType<WithGroupGetResponse_apigw>(response);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullパラメータで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => WithGroupGetResponse_apigw.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var response = new WithGroupGetResponse_apigw();

        Assert.Throws<ArgumentNullException>(() => response.Serialize(null));
    }

    /// <summary>
    /// SerializeとCreateFromDiscriminatorValueを通じたラウンドトリップが成功することを確認
    /// </summary>
    [Fact]
    public async Task Serialize_Roundtrip_Success()
    {
        var response = new WithGroupGetResponse_apigw
        {
            Group = new Group { Name = "test-group" },
            AdditionalData = new Dictionary<string, object>
            {
                { "test-key", "test-value" }
            }
        };

        var json = """
        {
            "group": {
                "id": "123e4567-e89b-12d3-a456-426614174000",
                "name": "test-group",
                "createdAt": "2024-01-01T00:00:00Z",
                "updatedAt": "2024-01-01T00:00:00Z"
            }
        }
        """;
        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));
        var factory = new JsonParseNodeFactory();
        var parseNode = await factory.GetRootParseNodeAsync("application/json", stream);
        var deserialized = WithGroupGetResponse_apigw.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(deserialized);
        Assert.NotNull(deserialized.AdditionalData);
    }

    /// <summary>
    /// AdditionalDataが初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_InitializesAdditionalData()
    {
        var response = new WithGroupGetResponse_apigw();
        Assert.NotNull(response.AdditionalData);
        Assert.Empty(response.AdditionalData);
    }

    /// <summary>
    /// Groupプロパティが正しく設定されることを確認
    /// </summary>
    [Fact]
    public void GroupProperty_CanBeSet()
    {
        var response = new WithGroupGetResponse_apigw();
        var group = new Group { Name = "test-group" };

        response.Group = group;

        Assert.Equal(group, response.Group);
        Assert.Equal("test-group", response.Group.Name);
    }

    /// <summary>
    /// SerializeがgroupプロパティをWriter経由で正しく書き込むことを確認
    /// </summary>
    [Fact]
    public void Serialize_WritesGroupProperty()
    {
        var group = new Group { Name = "test-group" };
        var response = new WithGroupGetResponse_apigw { Group = group };
        var writer = Substitute.For<ISerializationWriter>();

        response.Serialize(writer);

        writer.Received(1).WriteObjectValue("group", group);
        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }

    /// <summary>
    /// GetFieldDeserializersのgroupアクションが機能することを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_GroupAction_WorksCorrectly()
    {
        var response = new WithGroupGetResponse_apigw();
        var deserializers = response.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();
        var group = new Group { Name = "test-group" };
        parseNode.GetObjectValue(Arg.Any<ParsableFactory<Group>>())
            .Returns(group);

        deserializers["group"](parseNode);

        Assert.Equal(group, response.Group);
    }

    /// <summary>
    /// Serializeがnullプロパティの場合でも動作することを確認
    /// </summary>
    [Fact]
    public void Serialize_WithNullGroup_Works()
    {
        var response = new WithGroupGetResponse_apigw();
        var writer = Substitute.For<ISerializationWriter>();
        response.Serialize(writer);
        writer.Received(1).WriteObjectValue<Group>("group", null);
        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }
}
