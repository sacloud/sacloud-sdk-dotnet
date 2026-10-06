using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Models;
using Sacloud.Sdk.APIGateway.Subscriptions.Item;

namespace Sacloud.Sdk.APIGateway.Tests.Subscriptions.Item;

/// <summary>
/// WithSubscriptionGetResponse_apigwのテスト
/// </summary>
public class WithSubscriptionGetResponse_apigwTests
{
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = WithSubscriptionGetResponse_apigw.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<WithSubscriptionGetResponse_apigw>(result);
    }

    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => WithSubscriptionGetResponse_apigw.CreateFromDiscriminatorValue(null));
    }

    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var response = new WithSubscriptionGetResponse_apigw();
        var deserializers = response.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.True(deserializers.ContainsKey("subscription"));
    }

    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var response = new WithSubscriptionGetResponse_apigw();
        Assert.Throws<ArgumentNullException>(() => response.Serialize(null));
    }

    [Fact]
    public void Serialize_WritesSubscriptionProperty()
    {
        var subscription = new SubscriptionDetailResponse();
        var response = new WithSubscriptionGetResponse_apigw { Subscription = subscription };
        var writer = Substitute.For<ISerializationWriter>();

        response.Serialize(writer);

        writer.Received(1).WriteObjectValue("subscription", subscription);
        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }

    [Fact]
    public void Serialize_WithNullSubscription_Works()
    {
        var response = new WithSubscriptionGetResponse_apigw();
        var writer = Substitute.For<ISerializationWriter>();

        response.Serialize(writer);

        writer.Received(1).WriteObjectValue<SubscriptionDetailResponse>("subscription", null);
        writer.Received(1).WriteAdditionalData(response.AdditionalData);
    }

    [Fact]
    public void GetFieldDeserializers_SubscriptionAction_WorksCorrectly()
    {
        var response = new WithSubscriptionGetResponse_apigw();
        var deserializers = response.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();
        var value = new SubscriptionDetailResponse();
        parseNode.GetObjectValue(
            Arg.Any<ParsableFactory<SubscriptionDetailResponse>>())
            .Returns(value);

        deserializers["subscription"](parseNode);

        Assert.Equal(value, response.Subscription);
    }

    [Fact]
    public void Constructor_InitializesAdditionalData()
    {
        var response = new WithSubscriptionGetResponse_apigw();
        Assert.NotNull(response.AdditionalData);
        Assert.Empty(response.AdditionalData);
    }

    [Fact]
    public void AdditionalData_CanStoreCustomData()
    {
        var response = new WithSubscriptionGetResponse_apigw();
        response.AdditionalData["key"] = "value";
        Assert.True(response.AdditionalData.ContainsKey("key"));
        Assert.Equal("value", response.AdditionalData["key"]);
    }
}
