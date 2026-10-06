using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.Workflows.Models;

namespace Sacloud.Sdk.Workflows.Tests.Models;

public class DeleteWorkflowResponseTests
{
    [Fact]
    public void DeleteWorkflowResponse_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = DeleteWorkflowResponse.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<DeleteWorkflowResponse>(result);
    }

    [Fact]
    public void DeleteWorkflowResponse_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;
        Assert.Throws<ArgumentNullException>(() => DeleteWorkflowResponse.CreateFromDiscriminatorValue(parseNode!));
    }

    [Fact]
    public void DeleteWorkflowResponse_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new DeleteWorkflowResponse();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.True(deserializers.Count >= 1);
        Assert.True(deserializers.ContainsKey("is_ok"));
    }

    [Fact]
    public void DeleteWorkflowResponse_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new DeleteWorkflowResponse();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    [Fact]
    public void DeleteWorkflowResponse_AdditionalData_CanBeSetAndRetrieved()
    {
        var model = new DeleteWorkflowResponse();
        var testData = new Dictionary<string, object> { { "custom", "value" } };
        model.AdditionalData = testData;
        Assert.Equal(testData, model.AdditionalData);
    }
}
