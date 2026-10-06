using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.Workflows.Models;

namespace Sacloud.Sdk.Workflows.Tests.Models;

public class CreateSubscriptionTests
{
    [Fact]
    public void CreateSubscription_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = CreateSubscription.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<CreateSubscription>(result);
    }

    [Fact]
    public void CreateSubscription_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;
        Assert.Throws<ArgumentNullException>(() => CreateSubscription.CreateFromDiscriminatorValue(parseNode!));
    }

    [Fact]
    public void CreateSubscription_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new CreateSubscription();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.True(deserializers.Count >= 1);
    }

    [Fact]
    public void CreateSubscription_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new CreateSubscription();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    [Fact]
    public void CreateSubscription_AdditionalData_CanBeSetAndRetrieved()
    {
        var model = new CreateSubscription();
        var testData = new Dictionary<string, object> { { "custom", "value" } };
        model.AdditionalData = testData;
        Assert.Equal(testData, model.AdditionalData);
    }
}
