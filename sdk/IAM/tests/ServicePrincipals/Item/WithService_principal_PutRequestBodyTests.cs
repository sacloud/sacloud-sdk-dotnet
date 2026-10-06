using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.IAM.ServicePrincipals.Item;
using System.Text;
using System.Text.Json;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// WithService_principal_PutRequestBodyのテスト
/// </summary>
public class WithService_principal_PutRequestBodyTests
{
    /// <summary>
    /// パラメータなしコンストラクタでインスタンスが作成されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithoutParameters_CreatesInstance()
    {
        var requestBody = new WithService_principal_PutRequestBody();

        Assert.NotNull(requestBody);
    }

    /// <summary>
    /// Nameプロパティがnullで初期化されることを確認
    /// </summary>
    [Fact]
    public void Name_InitializedAsNull()
    {
        var requestBody = new WithService_principal_PutRequestBody();

        Assert.Null(requestBody.Name);
    }

    /// <summary>
    /// Nameプロパティが正しく設定されることを確認
    /// </summary>
    [Fact]
    public void Name_SetProperty_WorksCorrectly()
    {
        var requestBody = new WithService_principal_PutRequestBody
        {
            Name = "Updated Service Principal"
        };

        Assert.Equal("Updated Service Principal", requestBody.Name);
    }

    /// <summary>
    /// Descriptionプロパティがnullで初期化されることを確認
    /// </summary>
    [Fact]
    public void Description_InitializedAsNull()
    {
        var requestBody = new WithService_principal_PutRequestBody();

        Assert.Null(requestBody.Description);
    }

    /// <summary>
    /// Descriptionプロパティが正しく設定されることを確認
    /// </summary>
    [Fact]
    public void Description_SetProperty_WorksCorrectly()
    {
        var requestBody = new WithService_principal_PutRequestBody
        {
            Description = "Updated Description"
        };

        Assert.Equal("Updated Description", requestBody.Description);
    }

    /// <summary>
    /// AdditionalDataプロパティが正しく初期化されることを確認
    /// </summary>
    [Fact]
    public void AdditionalData_InitializedCorrectly()
    {
        var requestBody = new WithService_principal_PutRequestBody();

        Assert.NotNull(requestBody.AdditionalData);
        Assert.Empty(requestBody.AdditionalData);
    }

    /// <summary>
    /// AdditionalDataプロパティが正しく設定されることを確認
    /// </summary>
    [Fact]
    public void AdditionalData_SetProperty_WorksCorrectly()
    {
        var requestBody = new WithService_principal_PutRequestBody();
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
    /// GetFieldDeserializersが正しいマッピング（"name", "description"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var requestBody = new WithService_principal_PutRequestBody();

        var deserializers = requestBody.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("name"));
        Assert.True(deserializers.ContainsKey("description"));
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = WithService_principal_PutRequestBody.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<WithService_principal_PutRequestBody>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => WithService_principal_PutRequestBody.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var requestBody = new WithService_principal_PutRequestBody();

        Assert.Throws<ArgumentNullException>(() => requestBody.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクション（"name", "description"）が正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var requestBody = new WithService_principal_PutRequestBody();
        var deserializers = requestBody.GetFieldDeserializers();
        
        var parseNodeName = Substitute.For<IParseNode>();
        parseNodeName.GetStringValue().Returns("Test Service Principal");
        deserializers["name"](parseNodeName);
        
        var parseNodeDescription = Substitute.For<IParseNode>();
        parseNodeDescription.GetStringValue().Returns("Test Description");
        deserializers["description"](parseNodeDescription);

        Assert.Equal("Test Service Principal", requestBody.Name);
        Assert.Equal("Test Description", requestBody.Description);
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public void CanSerializeAndDeserializeWithAllProperties()
    {
        var requestBody = new WithService_principal_PutRequestBody
        {
            Name = "Test Service Principal",
            Description = "Test Description"
        };
        var jsonString = SerializeToJson(requestBody);
        var deserialized = DeserializeFromJson(jsonString);

        Assert.Equal("Test Service Principal", deserialized.Name);
        Assert.Equal("Test Description", deserialized.Description);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public void CanSerializeAndDeserializeWithAdditionalData()
    {
        var requestBody = new WithService_principal_PutRequestBody
        {
            Name = "Test Service Principal",
            Description = "Test Description",
            AdditionalData = new Dictionary<string, object>
            {
                { "customField", "customValue" }
            }
        };
        var jsonString = SerializeToJson(requestBody);
        var deserialized = DeserializeFromJson(jsonString);

        Assert.Equal("Test Service Principal", deserialized.Name);
        Assert.Equal("Test Description", deserialized.Description);
        Assert.NotNull(deserialized.AdditionalData);
        Assert.True(deserialized.AdditionalData.ContainsKey("customField"));
    }

    private static string SerializeToJson(WithService_principal_PutRequestBody requestBody)
    {
        var writer = Substitute.For<ISerializationWriter>();
        var memoryStream = new MemoryStream();
        var utf8JsonWriter = new Utf8JsonWriter(memoryStream);

        var capturedName = string.Empty;
        var capturedDescription = string.Empty;
        var capturedAdditionalData = new Dictionary<string, object>();

        writer.When(w => w.WriteStringValue("name", Arg.Any<string>())).Do(ci =>
        {
            capturedName = ci.ArgAt<string>(1) ?? string.Empty;
        });

        writer.When(w => w.WriteStringValue("description", Arg.Any<string>())).Do(ci =>
        {
            capturedDescription = ci.ArgAt<string>(1) ?? string.Empty;
        });

        writer.When(w => w.WriteAdditionalData(Arg.Any<IDictionary<string, object>>())).Do(ci =>
        {
            var additionalData = ci.ArgAt<IDictionary<string, object>>(0);
            if (additionalData is not null)
            {
                foreach (var kvp in additionalData)
                {
                    capturedAdditionalData[kvp.Key] = kvp.Value;
                }
            }
        });

        requestBody.Serialize(writer);

        utf8JsonWriter.WriteStartObject();
        if (!string.IsNullOrEmpty(capturedName))
        {
            utf8JsonWriter.WriteString("name", capturedName);
        }
        if (!string.IsNullOrEmpty(capturedDescription))
        {
            utf8JsonWriter.WriteString("description", capturedDescription);
        }
        foreach (var kvp in capturedAdditionalData)
        {
            utf8JsonWriter.WriteString(kvp.Key, kvp.Value.ToString());
        }
        utf8JsonWriter.WriteEndObject();
        utf8JsonWriter.Flush();

        return Encoding.UTF8.GetString(memoryStream.ToArray());
    }

    private static WithService_principal_PutRequestBody DeserializeFromJson(string jsonString)
    {
        var jsonDoc = JsonDocument.Parse(jsonString);
        var requestBody = new WithService_principal_PutRequestBody();

        if (jsonDoc.RootElement.TryGetProperty("name", out var nameElement))
        {
            requestBody.Name = nameElement.GetString();
        }

        if (jsonDoc.RootElement.TryGetProperty("description", out var descriptionElement))
        {
            requestBody.Description = descriptionElement.GetString();
        }

        foreach (var property in jsonDoc.RootElement.EnumerateObject())
        {
            if (property.Name != "name" && property.Name != "description")
            {
                requestBody.AdditionalData[property.Name] = property.Value.GetString() ?? string.Empty;
            }
        }

        return requestBody;
    }
}
