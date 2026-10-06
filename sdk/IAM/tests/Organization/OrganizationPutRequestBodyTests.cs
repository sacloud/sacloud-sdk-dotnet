using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.IAM.Organization;
using System.Text;
using System.Text.Json;

namespace Sacloud.Sdk.IAM.Tests.Organization;

/// <summary>
/// OrganizationPutRequestBodyのテスト
/// </summary>
public class OrganizationPutRequestBodyTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = OrganizationPutRequestBody.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<OrganizationPutRequestBody>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => OrganizationPutRequestBody.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"name"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var body = new OrganizationPutRequestBody();

        var deserializers = body.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("name"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var body = new OrganizationPutRequestBody();

        Assert.Throws<ArgumentNullException>(() => body.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクション（"name"）が正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var body = new OrganizationPutRequestBody();
        var deserializers = body.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();
        parseNode.GetStringValue().Returns("Test Org");

        deserializers["name"](parseNode);

        Assert.Equal("Test Org", body.Name);
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public void CanSerializeAndDeserializeWithAllProperties()
    {
        var body = new OrganizationPutRequestBody
        {
            Name = "Test Organization"
        };
        var jsonString = SerializeToJson(body);
        var deserialized = DeserializeFromJson(jsonString);

        Assert.Equal("Test Organization", deserialized.Name);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public void CanSerializeAndDeserializeWithAdditionalData()
    {
        var body = new OrganizationPutRequestBody
        {
            Name = "Test Organization",
            AdditionalData = new Dictionary<string, object>
            {
                { "customField", "customValue" }
            }
        };
        var jsonString = SerializeToJson(body);
        var deserialized = DeserializeFromJson(jsonString);

        Assert.Equal("Test Organization", deserialized.Name);
        Assert.NotNull(deserialized.AdditionalData);
        Assert.True(deserialized.AdditionalData.ContainsKey("customField"));
    }

    private static string SerializeToJson(OrganizationPutRequestBody body)
    {
        var writer = Substitute.For<ISerializationWriter>();
        var memoryStream = new MemoryStream();
        var utf8JsonWriter = new Utf8JsonWriter(memoryStream);

        var capturedName = string.Empty;
        var capturedAdditionalData = new Dictionary<string, object>();

        writer.When(w => w.WriteStringValue("name", Arg.Any<string>())).Do(ci =>
        {
            capturedName = ci.ArgAt<string>(1) ?? string.Empty;
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

        body.Serialize(writer);

        utf8JsonWriter.WriteStartObject();
        if (!string.IsNullOrEmpty(capturedName))
        {
            utf8JsonWriter.WriteString("name", capturedName);
        }
        foreach (var kvp in capturedAdditionalData)
        {
            utf8JsonWriter.WriteString(kvp.Key, kvp.Value.ToString());
        }
        utf8JsonWriter.WriteEndObject();
        utf8JsonWriter.Flush();

        return Encoding.UTF8.GetString(memoryStream.ToArray());
    }

    private static OrganizationPutRequestBody DeserializeFromJson(string jsonString)
    {
        var jsonDoc = JsonDocument.Parse(jsonString);
        var parseNode = Substitute.For<IParseNode>();
        var body = new OrganizationPutRequestBody();

        if (jsonDoc.RootElement.TryGetProperty("name", out var nameElement))
        {
            body.Name = nameElement.GetString();
        }

        foreach (var property in jsonDoc.RootElement.EnumerateObject())
        {
            if (property.Name != "name")
            {
                body.AdditionalData[property.Name] = property.Value.GetString() ?? string.Empty;
            }
        }

        return body;
    }
}
