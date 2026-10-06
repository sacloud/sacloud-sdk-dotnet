using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.Workflows.Models;

namespace Sacloud.Sdk.Workflows.Tests.Models;

public class UpdateWorkflowRevisionAliasTests
{
    [Fact]
    public void UpdateWorkflowRevisionAlias_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = UpdateWorkflowRevisionAlias.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<UpdateWorkflowRevisionAlias>(result);
    }

    [Fact]
    public void UpdateWorkflowRevisionAlias_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;
        Assert.Throws<ArgumentNullException>(() => UpdateWorkflowRevisionAlias.CreateFromDiscriminatorValue(parseNode!));
    }

    [Fact]
    public void UpdateWorkflowRevisionAlias_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new UpdateWorkflowRevisionAlias();
        var deserializers = model.GetFieldDeserializers();
        Assert.NotNull(deserializers);
        Assert.True(deserializers.Count >= 1);
    }

    [Fact]
    public void UpdateWorkflowRevisionAlias_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new UpdateWorkflowRevisionAlias();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    [Fact]
    public void UpdateWorkflowRevisionAlias_AdditionalData_CanBeSetAndRetrieved()
    {
        var model = new UpdateWorkflowRevisionAlias();
        var testData = new Dictionary<string, object> { { "custom", "value" } };
        model.AdditionalData = testData;
        Assert.Equal(testData, model.AdditionalData);
    }
}
