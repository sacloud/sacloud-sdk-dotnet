using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.IAM.Folders.Item.IamPolicy;

namespace Sacloud.Sdk.IAM.Tests.Folders.Item.IamPolicy;

/// <summary>
/// IamPolicyPutResponseのテスト
/// </summary>
public class IamPolicyPutResponseTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = IamPolicyPutResponse.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<IamPolicyPutResponse>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => IamPolicyPutResponse.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var response = new IamPolicyPutResponse();

        var deserializers = response.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("bindings"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var response = new IamPolicyPutResponse();

        Assert.Throws<ArgumentNullException>(() => response.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのbindingsアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_BindingsAction_WorksCorrectly()
    {
        var response = new IamPolicyPutResponse();
        var deserializers = response.GetFieldDeserializers();

        var parseNode = Substitute.For<IParseNode>();
        var policies = new List<IAM.Models.IamPolicy>();
        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<IAM.Models.IamPolicy>>())
            .Returns(policies);

        deserializers["bindings"](parseNode);

        Assert.NotNull(response.Bindings);
        Assert.Empty(response.Bindings);
    }

    /// <summary>
    /// AdditionalDataプロパティが正しく動作することを検証
    /// </summary>
    [Fact]
    public void AdditionalData_CanSetAndGet()
    {
        var response = new IamPolicyPutResponse();
        var additionalData = new Dictionary<string, object>
        {
            { "custom_field", "custom_value" }
        };

        response.AdditionalData = additionalData;

        Assert.NotNull(response.AdditionalData);
        Assert.True(response.AdditionalData.ContainsKey("custom_field"));
        Assert.Equal("custom_value", response.AdditionalData["custom_field"]);
    }

    /// <summary>
    /// Bindingsプロパティが正しく設定できることを検証
    /// </summary>
    [Fact]
    public void Bindings_CanSetAndGet()
    {
        var response = new IamPolicyPutResponse();
        var policies = new List<IAM.Models.IamPolicy>();

        response.Bindings = policies;

        Assert.NotNull(response.Bindings);
        Assert.Empty(response.Bindings);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAllProperties()
    {
        var original = new IamPolicyPutResponse
        {
            Bindings =
            [
                new IAM.Models.IamPolicy
                { 
                    Principals =
                    [
                        new() { Type = "user", Id = 1 } 
                    ] 
                }
            ]
        };

        using var stream = new MemoryStream();
        using var writer = new Microsoft.Kiota.Serialization.Json.JsonSerializationWriter();
        writer.WriteObjectValue(string.Empty, original);
        using var content = writer.GetSerializedContent();
        content.CopyTo(stream);

        var json = System.Text.Encoding.UTF8.GetString(stream.ToArray());

        using var jsonStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));
        var parseNode = await new Microsoft.Kiota.Serialization.Json.JsonParseNodeFactory().GetRootParseNodeAsync("application/json", jsonStream);
        var deserialized = parseNode.GetObjectValue(IamPolicyPutResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(deserialized);
        Assert.NotNull(deserialized.Bindings);
        Assert.Single(deserialized.Bindings);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAdditionalData()
    {
        var original = new IamPolicyPutResponse
        {
            AdditionalData = new Dictionary<string, object>
            {
                { "customField", "customValue" }
            }
        };

        using var stream = new MemoryStream();
        using var writer = new Microsoft.Kiota.Serialization.Json.JsonSerializationWriter();
        writer.WriteObjectValue(string.Empty, original);
        using var content = writer.GetSerializedContent();
        content.CopyTo(stream);

        var json = System.Text.Encoding.UTF8.GetString(stream.ToArray());

        using var jsonStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));
        var parseNode = await new Microsoft.Kiota.Serialization.Json.JsonParseNodeFactory().GetRootParseNodeAsync("application/json", jsonStream);
        var deserialized = parseNode.GetObjectValue(IamPolicyPutResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(deserialized);
        Assert.NotNull(deserialized.AdditionalData);
    }
}
