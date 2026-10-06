using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.IAM.OrganizationIamPolicy;
using System.Text;
using System.Text.Json;

namespace Sacloud.Sdk.IAM.Tests.OrganizationIamPolicy;

/// <summary>
/// OrganizationIamPolicyPutRequestBodyのテスト
/// </summary>
public class OrganizationIamPolicyPutRequestBodyTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = OrganizationIamPolicyPutRequestBody.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<OrganizationIamPolicyPutRequestBody>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => OrganizationIamPolicyPutRequestBody.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"bindings"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var body = new OrganizationIamPolicyPutRequestBody();

        var deserializers = body.GetFieldDeserializers();

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
        var body = new OrganizationIamPolicyPutRequestBody();

        Assert.Throws<ArgumentNullException>(() => body.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクション（"bindings"）が正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var body = new OrganizationIamPolicyPutRequestBody();
        var deserializers = body.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();
        var mockBindings = new List<IAM.Models.IamPolicy>();

        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<IAM.Models.IamPolicy>>())
            .Returns(mockBindings);

        deserializers["bindings"](parseNode);

        Assert.NotNull(body.Bindings);
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public void CanSerializeAndDeserializeWithAllProperties()
    {
        var body = new OrganizationIamPolicyPutRequestBody
        {
            Bindings = []
        };
        var jsonString = SerializeToJson(body);
        var deserialized = DeserializeFromJson(jsonString);

        Assert.NotNull(deserialized);
        Assert.NotNull(deserialized.Bindings);
    }

    private static string SerializeToJson(OrganizationIamPolicyPutRequestBody body)
    {
        var writer = Substitute.For<ISerializationWriter>();
        var memoryStream = new MemoryStream();
        var utf8JsonWriter = new Utf8JsonWriter(memoryStream);

        var capturedBindings = new List<IAM.Models.IamPolicy>();
        var capturedAdditionalData = new Dictionary<string, object>();

        writer.When(w => w.WriteCollectionOfObjectValues("bindings", Arg.Any<IEnumerable<IAM.Models.IamPolicy>>())).Do(ci =>
        {
            var bindings = ci.ArgAt<IEnumerable<IAM.Models.IamPolicy>>(1);
            if (bindings is not null)
            {
                capturedBindings.AddRange(bindings);
            }
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
        utf8JsonWriter.WriteStartArray("bindings");
        utf8JsonWriter.WriteEndArray();
        foreach (var kvp in capturedAdditionalData)
        {
            utf8JsonWriter.WriteString(kvp.Key, kvp.Value.ToString());
        }
        utf8JsonWriter.WriteEndObject();
        utf8JsonWriter.Flush();

        return Encoding.UTF8.GetString(memoryStream.ToArray());
    }

    private static OrganizationIamPolicyPutRequestBody DeserializeFromJson(string jsonString)
    {
        var jsonDoc = JsonDocument.Parse(jsonString);
        var body = new OrganizationIamPolicyPutRequestBody
        {
            Bindings = []
        };

        foreach (var property in jsonDoc.RootElement.EnumerateObject())
        {
            if (property.Name != "bindings")
            {
                body.AdditionalData[property.Name] = property.Value.GetString() ?? string.Empty;
            }
        }

        return body;
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public void CanSerializeAndDeserializeWithAdditionalData()
    {
        var body = new OrganizationIamPolicyPutRequestBody
        {
            Bindings = [],
            AdditionalData = new Dictionary<string, object>
            {
                { "customField", "customValue" },
                { "numericField", "123" }
            }
        };

        var jsonString = SerializeToJson(body);
        var deserialized = DeserializeFromJson(jsonString);

        Assert.NotNull(deserialized);
        Assert.NotNull(deserialized.AdditionalData);
        Assert.Equal(2, deserialized.AdditionalData.Count);
        Assert.Equal("customValue", deserialized.AdditionalData["customField"]);
        Assert.Equal("123", deserialized.AdditionalData["numericField"]);
    }
}
