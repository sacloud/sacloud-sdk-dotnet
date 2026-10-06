using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.IAM.ServicePrincipals;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// ServicePrincipalsPostRequestBodyのテスト
/// </summary>
public class ServicePrincipalsPostRequestBodyTests
{
    /// <summary>
    /// パラメータなしコンストラクタでインスタンスが作成されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithoutParameters_CreatesInstance()
    {
        var requestBody = new ServicePrincipalsPostRequestBody();

        Assert.NotNull(requestBody);
    }

    /// <summary>
    /// Nameプロパティがnullで初期化されることを確認
    /// </summary>
    [Fact]
    public void Name_InitializedAsNull()
    {
        var requestBody = new ServicePrincipalsPostRequestBody();

        Assert.Null(requestBody.Name);
    }

    /// <summary>
    /// Nameプロパティが正しく設定されることを確認
    /// </summary>
    [Fact]
    public void Name_SetProperty_WorksCorrectly()
    {
        var requestBody = new ServicePrincipalsPostRequestBody
        {
            Name = "Test Service Principal"
        };

        Assert.Equal("Test Service Principal", requestBody.Name);
    }

    /// <summary>
    /// AdditionalDataプロパティが正しく初期化されることを確認
    /// </summary>
    [Fact]
    public void AdditionalData_InitializedCorrectly()
    {
        var requestBody = new ServicePrincipalsPostRequestBody();

        Assert.NotNull(requestBody.AdditionalData);
        Assert.Empty(requestBody.AdditionalData);
    }

    /// <summary>
    /// AdditionalDataプロパティが正しく設定されることを確認
    /// </summary>
    [Fact]
    public void AdditionalData_SetProperty_WorksCorrectly()
    {
        var requestBody = new ServicePrincipalsPostRequestBody();
        var additionalData = new Dictionary<string, object>
        {
            { "key1", "value1" },
            { "key2", 123 }
        };

        requestBody.AdditionalData = additionalData;

        Assert.NotNull(requestBody.AdditionalData);
        Assert.Equal(2, requestBody.AdditionalData.Count);
        Assert.Equal("value1", requestBody.AdditionalData["key1"]);
        Assert.Equal(123, requestBody.AdditionalData["key2"]);
    }

    /// <summary>
    /// GetFieldDeserializersが正しいデシリアライザを返すことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectDeserializers()
    {
        var requestBody = new ServicePrincipalsPostRequestBody();

        var deserializers = requestBody.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.True(deserializers.Count > 0);
        Assert.True(deserializers.ContainsKey("name"));
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = NSubstitute.Substitute.For<IParseNode>();

        var result = ServicePrincipalsPostRequestBody.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<ServicePrincipalsPostRequestBody>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            ServicePrincipalsPostRequestBody.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング(name, description, project_id)を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var requestBody = new ServicePrincipalsPostRequestBody();

        var deserializers = requestBody.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(3, deserializers.Count);
        Assert.True(deserializers.ContainsKey("name"));
        Assert.True(deserializers.ContainsKey("description"));
        Assert.True(deserializers.ContainsKey("project_id"));
    }

    /// <summary>
    /// Serializeがnullのwriterで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var requestBody = new ServicePrincipalsPostRequestBody();

        Assert.Throws<ArgumentNullException>(() => requestBody.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクション(name, description, project_id)が正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var requestBody = new ServicePrincipalsPostRequestBody();
        var deserializers = requestBody.GetFieldDeserializers();

        // nameのテスト
        var parseNodeName = NSubstitute.Substitute.For<IParseNode>();
        parseNodeName.GetStringValue().Returns("Test Service Principal");
        deserializers["name"](parseNodeName);
        Assert.Equal("Test Service Principal", requestBody.Name);

        // descriptionのテスト
        var parseNodeDescription = NSubstitute.Substitute.For<IParseNode>();
        parseNodeDescription.GetStringValue().Returns("Test Description");
        deserializers["description"](parseNodeDescription);
        Assert.Equal("Test Description", requestBody.Description);

        // project_idのテスト
        var parseNodeProjectId = NSubstitute.Substitute.For<IParseNode>();
        parseNodeProjectId.GetLongValue().Returns(123456L);
        deserializers["project_id"](parseNodeProjectId);
        Assert.Equal(123456L, requestBody.ProjectId);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAllProperties()
    {
        var original = new ServicePrincipalsPostRequestBody
        {
            Name = "Test Service Principal",
            Description = "Test Description",
            ProjectId = 123456L
        };

        using var stream = new MemoryStream();
        using var writer = new Microsoft.Kiota.Serialization.Json.JsonSerializationWriter();
        writer.WriteObjectValue(string.Empty, original);
        using var content = writer.GetSerializedContent();
        content.CopyTo(stream);

        var json = System.Text.Encoding.UTF8.GetString(stream.ToArray());

        using var jsonStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));
        var parseNode = await new Microsoft.Kiota.Serialization.Json.JsonParseNodeFactory().GetRootParseNodeAsync("application/json", jsonStream);
        var deserialized = parseNode.GetObjectValue(ServicePrincipalsPostRequestBody.CreateFromDiscriminatorValue);

        Assert.NotNull(deserialized);
        Assert.Equal("Test Service Principal", deserialized.Name);
        Assert.Equal("Test Description", deserialized.Description);
        Assert.Equal(123456L, deserialized.ProjectId);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAdditionalData()
    {
        var original = new ServicePrincipalsPostRequestBody
        {
            Name = "Test Service Principal",
            AdditionalData = new Dictionary<string, object>
            {
                { "customField", "customValue" },
                { "numericField", 123 }
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
        var deserialized = parseNode.GetObjectValue(ServicePrincipalsPostRequestBody.CreateFromDiscriminatorValue);

        Assert.NotNull(deserialized);
        Assert.Equal("Test Service Principal", deserialized.Name);
        Assert.NotNull(deserialized.AdditionalData);
        Assert.True(deserialized.AdditionalData.Count >= 1);
    }
}
