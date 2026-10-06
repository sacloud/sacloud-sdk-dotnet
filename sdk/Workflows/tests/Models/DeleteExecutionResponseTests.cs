using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.Workflows.Models;

namespace Sacloud.Sdk.Workflows.Tests.Models;

public class DeleteExecutionResponseTests
{
    [Fact]
    public void DeleteExecutionResponse_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = DeleteExecutionResponse.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<DeleteExecutionResponse>(result);
    }

    [Fact]
    public void DeleteExecutionResponse_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;
        Assert.Throws<ArgumentNullException>(() => DeleteExecutionResponse.CreateFromDiscriminatorValue(parseNode!));
    }

    [Fact]
    public void DeleteExecutionResponse_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new DeleteExecutionResponse();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.True(deserializers.Count >= 1);
        Assert.True(deserializers.ContainsKey("is_ok"));
    }

    [Fact]
    public void DeleteExecutionResponse_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new DeleteExecutionResponse();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    [Fact]
    public void DeleteExecutionResponse_AdditionalData_CanBeSetAndRetrieved()
    {
        var model = new DeleteExecutionResponse();
        var testData = new Dictionary<string, object> { { "custom", "value" } };
        model.AdditionalData = testData;
        Assert.Equal(testData, model.AdditionalData);
    }
}
