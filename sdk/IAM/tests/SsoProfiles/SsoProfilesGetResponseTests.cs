using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using System.Text;
using System.Text.Json;
using Sacloud.Sdk.IAM.Models;
using Sacloud.Sdk.IAM.SsoProfiles;

namespace Sacloud.Sdk.IAM.Tests.SsoProfiles;

/// <summary>
/// SsoProfilesGetResponseのテスト
/// </summary>
public class SsoProfilesGetResponseTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = SsoProfilesGetResponse.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<SsoProfilesGetResponse>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => SsoProfilesGetResponse.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var response = new SsoProfilesGetResponse();

        var deserializers = response.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.True(deserializers.ContainsKey("items"));
    }

    /// <summary>
    /// GetFieldDeserializersがitemsキーを含むことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ContainsItemsKey()
    {
        var response = new SsoProfilesGetResponse();

        var deserializers = response.GetFieldDeserializers();

        Assert.True(deserializers.ContainsKey("items"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var response = new SsoProfilesGetResponse();

        Assert.Throws<ArgumentNullException>(() => response.Serialize(null));
    }

    /// <summary>
    /// Serializeが基底クラスのSerializeを呼び出すことを検証
    /// </summary>
    [Fact]
    public void Serialize_CallsBaseSerialize()
    {
        var response = new SsoProfilesGetResponse
        {
            Items =
            [
                new SSOProfile { Name = "Test SSO Profile" }
            ]
        };
        var writer = Substitute.For<ISerializationWriter>();

        response.Serialize(writer);

        writer.Received(1).WriteCollectionOfObjectValues("items", response.Items);
    }

    /// <summary>
    /// GetFieldDeserializersのItemsアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ItemsAction_WorksCorrectly()
    {
        var response = new SsoProfilesGetResponse();
        var deserializers = response.GetFieldDeserializers();
        
        var ssoProfiles = new List<SSOProfile>
        {
            new SSOProfile { Name = "SSO Profile 1" },
            new SSOProfile { Name = "SSO Profile 2" }
        };
        
        var parseNode = Substitute.For<IParseNode>();
        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<SSOProfile>>())
            .Returns(ssoProfiles);
        
        deserializers["items"](parseNode);

        Assert.NotNull(response.Items);
        Assert.Equal(2, response.Items.Count);
    }

    /// <summary>
    /// GetFieldDeserializersが基底クラスのフィールドを含むことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_IncludesBaseClassFields()
    {
        var response = new SsoProfilesGetResponse();

        var deserializers = response.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Contains(deserializers, kvp => kvp.Key == "count" || kvp.Key == "next" || kvp.Key == "previous");
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public void CanSerializeAndDeserializeWithAllProperties()
    {
        var response = new SsoProfilesGetResponse
        {
            Items =
            [
                new SSOProfile { Name = "Test SSO Profile" }
            ]
        };
        var jsonString = SerializeToJson(response);
        var deserialized = DeserializeFromJson(jsonString);

        Assert.NotNull(deserialized.Items);
        Assert.Single(deserialized.Items);
    }

    /// <summary>
    /// 空のItemsでシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public void CanSerializeAndDeserializeWithEmptyItems()
    {
        var response = new SsoProfilesGetResponse
        {
            Items = []
        };
        var jsonString = SerializeToJson(response);
        var deserialized = DeserializeFromJson(jsonString);

        Assert.NotNull(deserialized.Items);
        Assert.Empty(deserialized.Items);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public void CanSerializeAndDeserializeWithAdditionalData()
    {
        var response = new SsoProfilesGetResponse
        {
            Items = [],
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

    /// <summary>
    /// AdditionalDataとItemsプロパティが共存できることを検証
    /// </summary>
    [Fact]
    public void AdditionalDataAndItems_CanCoexist()
    {
        var response = new SsoProfilesGetResponse
        {
            Items =
            [
                new SSOProfile { Name = "Test SSO Profile" }
            ],
            AdditionalData = new Dictionary<string, object>
            {
                { "customField", "customValue" }
            }
        };
        var jsonString = SerializeToJson(response);
        var deserialized = DeserializeFromJson(jsonString);

        Assert.NotNull(deserialized.Items);
        Assert.Single(deserialized.Items);
        Assert.NotNull(deserialized.AdditionalData);
        Assert.True(deserialized.AdditionalData.ContainsKey("customField"));
    }

    private static string SerializeToJson(SsoProfilesGetResponse response)
    {
        var writer = Substitute.For<ISerializationWriter>();
        var memoryStream = new MemoryStream();
        var utf8JsonWriter = new Utf8JsonWriter(memoryStream);

        var capturedItems = new List<SSOProfile>();
        var capturedAdditionalData = new Dictionary<string, object>();

        writer.When(w => w.WriteCollectionOfObjectValues("items", Arg.Any<IEnumerable<SSOProfile>>())).Do(ci =>
        {
            var items = ci.ArgAt<IEnumerable<SSOProfile>>(1);
            if (items is not null)
            {
                capturedItems.AddRange(items);
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
        if (capturedItems.Count > 0)
        {
            utf8JsonWriter.WriteStartArray("items");
            foreach (var item in capturedItems)
            {
                utf8JsonWriter.WriteStartObject();
                utf8JsonWriter.WriteNumber("id", item.Id ?? 0);
                utf8JsonWriter.WriteString("name", item.Name);
                utf8JsonWriter.WriteEndObject();
            }
            utf8JsonWriter.WriteEndArray();
        }
        else
        {
            utf8JsonWriter.WriteStartArray("items");
            utf8JsonWriter.WriteEndArray();
        }
        foreach (var kvp in capturedAdditionalData)
        {
            utf8JsonWriter.WriteString(kvp.Key, kvp.Value.ToString());
        }
        utf8JsonWriter.WriteEndObject();
        utf8JsonWriter.Flush();

        return Encoding.UTF8.GetString(memoryStream.ToArray());
    }

    private static SsoProfilesGetResponse DeserializeFromJson(string jsonString)
    {
        var jsonDoc = JsonDocument.Parse(jsonString);
        var response = new SsoProfilesGetResponse();

        if (jsonDoc.RootElement.TryGetProperty("items", out var itemsElement))
        {
            var items = new List<SSOProfile>();
            foreach (var item in itemsElement.EnumerateArray())
            {
                var ssoProfile = new SSOProfile();
                // Idはprivate setなのでテストでは設定しない
                if (item.TryGetProperty("name", out var nameElement))
                {
                    ssoProfile.Name = nameElement.GetString();
                }
                items.Add(ssoProfile);
            }
            response.Items = items;
        }

        foreach (var property in jsonDoc.RootElement.EnumerateObject())
        {
            if (property.Name != "items" && property.Name != "page" && property.Name != "per_page" && property.Name != "total")
            {
                response.AdditionalData[property.Name] = property.Value.GetString() ?? string.Empty;
            }
        }

        return response;
    }
}
