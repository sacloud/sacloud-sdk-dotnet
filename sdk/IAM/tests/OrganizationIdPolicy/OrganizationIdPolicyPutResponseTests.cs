using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.IAM.Models;
using Sacloud.Sdk.IAM.OrganizationIdPolicy;
using System.Text;
using System.Text.Json;

namespace Sacloud.Sdk.IAM.Tests.OrganizationIdPolicy;

/// <summary>
/// OrganizationIdPolicyPutResponseのテスト
/// </summary>
public class OrganizationIdPolicyPutResponseTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = OrganizationIdPolicyPutResponse.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<OrganizationIdPolicyPutResponse>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => OrganizationIdPolicyPutResponse.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"bindings"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var response = new OrganizationIdPolicyPutResponse();

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
        var response = new OrganizationIdPolicyPutResponse();

        Assert.Throws<ArgumentNullException>(() => response.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクション（"bindings"）が正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var response = new OrganizationIdPolicyPutResponse();
        var deserializers = response.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();
        var idPolicyList = new List<IdPolicy> { new IdPolicy() };
        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<IdPolicy>>()).Returns(idPolicyList);

        deserializers["bindings"](parseNode);

        Assert.NotNull(response.Bindings);
        Assert.Single(response.Bindings);
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public void CanSerializeAndDeserializeWithAllProperties()
    {
        var response = new OrganizationIdPolicyPutResponse
        {
            Bindings =
            [
                new IdPolicy()
            ]
        };
        var jsonString = SerializeToJson(response);
        var deserialized = DeserializeFromJson(jsonString);

        Assert.NotNull(deserialized.Bindings);
        Assert.Single(deserialized.Bindings);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public void CanSerializeAndDeserializeWithAdditionalData()
    {
        var response = new OrganizationIdPolicyPutResponse
        {
            Bindings = [],
            AdditionalData = new Dictionary<string, object>
            {
                { "customField", "customValue" }
            }
        };
        var jsonString = SerializeToJson(response);
        var deserialized = DeserializeFromJson(jsonString);

        Assert.NotNull(deserialized.AdditionalData);
        Assert.True(deserialized.AdditionalData.ContainsKey("customField"));
    }

    private static string SerializeToJson(OrganizationIdPolicyPutResponse response)
    {
        var writer = Substitute.For<ISerializationWriter>();
        var memoryStream = new MemoryStream();
        var utf8JsonWriter = new Utf8JsonWriter(memoryStream);

        var capturedBindings = new List<IdPolicy>();
        var capturedAdditionalData = new Dictionary<string, object>();

        writer.When(w => w.WriteCollectionOfObjectValues("bindings", Arg.Any<IEnumerable<IdPolicy>>())).Do(ci =>
        {
            var bindings = ci.ArgAt<IEnumerable<IdPolicy>>(1);
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

        response.Serialize(writer);

        utf8JsonWriter.WriteStartObject();
        if (capturedBindings.Count > 0)
        {
            utf8JsonWriter.WriteNumber("bindings", capturedBindings.Count);
        }
        foreach (var kvp in capturedAdditionalData)
        {
            utf8JsonWriter.WriteString(kvp.Key, kvp.Value.ToString());
        }
        utf8JsonWriter.WriteEndObject();
        utf8JsonWriter.Flush();

        return Encoding.UTF8.GetString(memoryStream.ToArray());
    }

    private static OrganizationIdPolicyPutResponse DeserializeFromJson(string jsonString)
    {
        var jsonDoc = JsonDocument.Parse(jsonString);
        var response = new OrganizationIdPolicyPutResponse();

        if (jsonDoc.RootElement.TryGetProperty("bindings", out var bindingsElement))
        {
            var count = bindingsElement.GetInt32();
            response.Bindings = [];
            for (int i = 0; i < count; i++)
            {
                response.Bindings.Add(new IdPolicy());
            }
        }

        foreach (var property in jsonDoc.RootElement.EnumerateObject())
        {
            if (property.Name != "bindings")
            {
                response.AdditionalData[property.Name] = property.Value.GetString() ?? string.Empty;
            }
        }

        return response;
    }
}
