using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.IAM.OrganizationIamPolicy;

namespace Sacloud.Sdk.IAM.Tests.OrganizationIamPolicy;

/// <summary>
/// OrganizationIamPolicyPutResponseモデルのテスト
/// </summary>
public class OrganizationIamPolicyPutResponseTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = OrganizationIamPolicyPutResponse.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<OrganizationIamPolicyPutResponse>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() =>
            OrganizationIamPolicyPutResponse.CreateFromDiscriminatorValue(parseNode));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new OrganizationIamPolicyPutResponse();

        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.True(deserializers.ContainsKey("bindings"));
        Assert.Single(deserializers);
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new OrganizationIamPolicyPutResponse();

        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new OrganizationIamPolicyPutResponse();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();
        var mockBindings = new List<IAM.Models.IamPolicy>();

        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<IAM.Models.IamPolicy>>())
            .Returns(mockBindings);

        deserializers["bindings"](parseNode);

        Assert.NotNull(model.Bindings);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAllProperties()
    {
        var jsonContent = @"{""bindings"": []}";

        using var serializeStream = new MemoryStream();
        using var streamWriter = new StreamWriter(serializeStream);
        await streamWriter.WriteAsync(jsonContent);
        await streamWriter.FlushAsync();

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(OrganizationIamPolicyPutResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Bindings);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAdditionalData()
    {
        var jsonContent = @"{""bindings"": [], ""customField"": ""customValue"", ""numericField"": 123}";

        using var serializeStream = new MemoryStream();
        using var streamWriter = new StreamWriter(serializeStream);
        await streamWriter.WriteAsync(jsonContent);
        await streamWriter.FlushAsync();

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(OrganizationIamPolicyPutResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.AdditionalData);
        Assert.Equal(2, deserializedModel.AdditionalData.Count);
        Assert.Equal("customValue", deserializedModel.AdditionalData["customField"].ToString());
    }
}
