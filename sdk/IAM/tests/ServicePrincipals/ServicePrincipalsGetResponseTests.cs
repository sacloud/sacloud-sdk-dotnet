using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.IAM.Models;
using Sacloud.Sdk.IAM.ServicePrincipals;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// ServicePrincipalsGetResponseのテスト
/// </summary>
public class ServicePrincipalsGetResponseTests
{
    /// <summary>
    /// パラメータなしコンストラクタでインスタンスが作成されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithoutParameters_CreatesInstance()
    {
        var response = new ServicePrincipalsGetResponse();

        Assert.NotNull(response);
    }

    /// <summary>
    /// ServicePrincipalsプロパティがnullで初期化されることを確認
    /// </summary>
    [Fact]
    public void ServicePrincipals_InitializedAsNull()
    {
        var response = new ServicePrincipalsGetResponse();

        Assert.Null(response.Items);
    }

    /// <summary>
    /// ServicePrincipalsプロパティが正しく設定されることを確認
    /// </summary>
    [Fact]
    public void ServicePrincipals_SetProperty_WorksCorrectly()
    {
        var response = new ServicePrincipalsGetResponse();
        var servicePrincipals = new List<ServicePrincipal>
        {
            new ServicePrincipal { Name = "SP1" },
            new ServicePrincipal { Name = "SP2" }
        };

        response.Items = servicePrincipals;

        Assert.NotNull(response.Items);
        Assert.Equal(2, response.Items.Count);
        Assert.Equal("SP1", response.Items[0].Name);
        Assert.Equal("SP2", response.Items[1].Name);
    }

    /// <summary>
    /// AdditionalDataプロパティが正しく初期化されることを確認
    /// </summary>
    [Fact]
    public void AdditionalData_InitializedCorrectly()
    {
        var response = new ServicePrincipalsGetResponse();

        Assert.NotNull(response.AdditionalData);
        Assert.Empty(response.AdditionalData);
    }

    /// <summary>
    /// AdditionalDataプロパティが正しく設定されることを確認
    /// </summary>
    [Fact]
    public void AdditionalData_SetProperty_WorksCorrectly()
    {
        var response = new ServicePrincipalsGetResponse();
        var additionalData = new Dictionary<string, object>
        {
            { "key1", "value1" },
            { "key2", 123 }
        };

        response.AdditionalData = additionalData;

        Assert.NotNull(response.AdditionalData);
        Assert.Equal(2, response.AdditionalData.Count);
        Assert.Equal("value1", response.AdditionalData["key1"]);
        Assert.Equal(123, response.AdditionalData["key2"]);
    }

    /// <summary>
    /// GetFieldDeserializersが正しいデシリアライザを返すことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectDeserializers()
    {
        var response = new ServicePrincipalsGetResponse();

        var deserializers = response.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.True(deserializers.Count > 0);
        Assert.True(deserializers.ContainsKey("items"));
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = NSubstitute.Substitute.For<IParseNode>();

        var result = ServicePrincipalsGetResponse.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<ServicePrincipalsGetResponse>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            ServicePrincipalsGetResponse.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// Serializeがnullのwriterで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var response = new ServicePrincipalsGetResponse();

        Assert.Throws<ArgumentNullException>(() => response.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクション(items, count, next, previous)が正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var response = new ServicePrincipalsGetResponse();
        var deserializers = response.GetFieldDeserializers();

        // itemsのテスト
        var parseNodeItems = NSubstitute.Substitute.For<IParseNode>();
        var testItems = new List<ServicePrincipal>
        {
            new ServicePrincipal { Name = "SP1" },
            new ServicePrincipal { Name = "SP2" }
        };
        parseNodeItems.GetCollectionOfObjectValues(NSubstitute.Arg.Any<ParsableFactory<ServicePrincipal>>())
            .Returns(testItems);
        deserializers["items"](parseNodeItems);
        Assert.NotNull(response.Items);
        Assert.Equal(2, response.Items.Count);

        // countのテスト(基底クラスPaginationから)
        var parseNodeCount = NSubstitute.Substitute.For<IParseNode>();
        parseNodeCount.GetIntValue().Returns(10);
        deserializers["count"](parseNodeCount);
        Assert.Equal(10, response.Count);

        // nextのテスト(基底クラスPaginationから)
        var parseNodeNext = NSubstitute.Substitute.For<IParseNode>();
        parseNodeNext.GetStringValue().Returns("http://next-page");
        deserializers["next"](parseNodeNext);
        Assert.Equal("http://next-page", response.Next);

        // previousのテスト(基底クラスPaginationから)
        var parseNodePrevious = NSubstitute.Substitute.For<IParseNode>();
        parseNodePrevious.GetStringValue().Returns("http://previous-page");
        deserializers["previous"](parseNodePrevious);
        Assert.Equal("http://previous-page", response.Previous);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAllProperties()
    {
        var original = new ServicePrincipalsGetResponse
        {
            Items =
            [
                new ServicePrincipal { Name = "SP1" },
                new ServicePrincipal { Name = "SP2" }
            ],
            Count = 2,
            Next = "http://next-page",
            Previous = "http://previous-page"
        };

        using var stream = new MemoryStream();
        using var writer = new Microsoft.Kiota.Serialization.Json.JsonSerializationWriter();
        writer.WriteObjectValue(string.Empty, original);
        using var content = writer.GetSerializedContent();
        content.CopyTo(stream);

        var json = System.Text.Encoding.UTF8.GetString(stream.ToArray());

        using var jsonStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));
        var parseNode = await new Microsoft.Kiota.Serialization.Json.JsonParseNodeFactory().GetRootParseNodeAsync("application/json", jsonStream);
        var deserialized = parseNode.GetObjectValue(ServicePrincipalsGetResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(deserialized);
        Assert.Equal(2, deserialized.Count);
        Assert.Equal("http://next-page", deserialized.Next);
        Assert.Equal("http://previous-page", deserialized.Previous);
    }
}

