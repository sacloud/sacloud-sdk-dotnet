using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.IAM.Models;
using Sacloud.Sdk.IAM.ScimConfigurations;
using System.Text;
using System.Text.Json;

namespace Sacloud.Sdk.IAM.Tests.ScimConfigurations;

/// <summary>
/// ScimConfigurationsGetResponseのテスト
/// </summary>
public class ScimConfigurationsGetResponseTests
{
    /// <summary>
    /// コンストラクタ実行後にAdditionalDataが空のDictionaryで初期化されることを検証
    /// </summary>
    [Fact]
    public void コンストラクタ_AdditionalDataが空のDictionaryで初期化される()
    {
        var response = new ScimConfigurationsGetResponse();

        Assert.NotNull(response.AdditionalData);
        Assert.Empty(response.AdditionalData);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_インスタンスを返す()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = ScimConfigurationsGetResponse.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<ScimConfigurationsGetResponse>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_nullのParseNodeでArgumentNullExceptionをスロー()
    {
        Assert.Throws<ArgumentNullException>(() => ScimConfigurationsGetResponse.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが"items"キーを含むことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_itemsキーを含む()
    {
        var response = new ScimConfigurationsGetResponse();

        var deserializers = response.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.True(deserializers.ContainsKey("items"));
    }

    /// <summary>
    /// GetFieldDeserializersの"items"アクションがItemsプロパティをセットすることを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_itemsアクションがItemsプロパティをセットする()
    {
        var response = new ScimConfigurationsGetResponse();
        var deserializers = response.GetFieldDeserializers();

        var scimConfigurations = new List<ScimConfigurationPublic>
        {
            new(),
            new(),
        };

        var parseNode = Substitute.For<IParseNode>();
        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<ScimConfigurationPublic>>())
            .Returns(scimConfigurations);

        deserializers["items"](parseNode);

        Assert.NotNull(response.Items);
        Assert.Equal(2, response.Items.Count);
    }

    /// <summary>
    /// SerializeがWriteCollectionOfObjectValuesを1回呼び出すことを検証
    /// </summary>
    [Fact]
    public void Serialize_Itemsプロパティを書き込む()
    {
        var response = new ScimConfigurationsGetResponse
        {
            Items =
            [
                new ScimConfigurationPublic(),
            ]
        };
        var writer = Substitute.For<ISerializationWriter>();

        response.Serialize(writer);

        writer.Received(1).WriteCollectionOfObjectValues("items", response.Items);
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_nullのwriterでArgumentNullExceptionをスロー()
    {
        var response = new ScimConfigurationsGetResponse();

        Assert.Throws<ArgumentNullException>(() => response.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersが基底クラスのフィールドを含むことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_基底クラスフィールドを含む()
    {
        var response = new ScimConfigurationsGetResponse();

        var deserializers = response.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Contains(deserializers, kvp => kvp.Key == "count" || kvp.Key == "next" || kvp.Key == "previous");
    }

    /// <summary>
    /// 全プロパティでシリアライズとデシリアライズができることを検証
    /// </summary>
    [Fact]
    public void 全プロパティでシリアライズとデシリアライズができる()
    {
        var response = new ScimConfigurationsGetResponse
        {
            Items =
            [
                new ScimConfigurationPublic { Name = "Test SCIM Configuration" }
            ]
        };
        var jsonString = SerializeToJson(response);
        var deserialized = DeserializeFromJson(jsonString);

        Assert.NotNull(deserialized.Items);
        Assert.Single(deserialized.Items);
        Assert.Equal("Test SCIM Configuration", deserialized.Items[0].Name);
    }

    /// <summary>
    /// 空のItemsでシリアライズとデシリアライズができることを検証
    /// </summary>
    [Fact]
    public void 空のItemsでシリアライズとデシリアライズができる()
    {
        var response = new ScimConfigurationsGetResponse
        {
            Items = []
        };
        var jsonString = SerializeToJson(response);
        var deserialized = DeserializeFromJson(jsonString);

        Assert.NotNull(deserialized.Items);
        Assert.Empty(deserialized.Items);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを検証
    /// </summary>
    [Fact]
    public void AdditionalDataでシリアライズとデシリアライズができる()
    {
        var response = new ScimConfigurationsGetResponse
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
    /// AdditionalDataとItemsが共存できることを検証
    /// </summary>
    [Fact]
    public void AdditionalDataとItemsが共存できる()
    {
        var response = new ScimConfigurationsGetResponse
        {
            Items =
            [
                new ScimConfigurationPublic { Name = "Test SCIM Configuration" }
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

    private static string SerializeToJson(ScimConfigurationsGetResponse response)
    {
        var writer = Substitute.For<ISerializationWriter>();
        var memoryStream = new MemoryStream();
        var utf8JsonWriter = new Utf8JsonWriter(memoryStream);

        var capturedItems = new List<ScimConfigurationPublic>();
        var capturedAdditionalData = new Dictionary<string, object>();

        writer.When(w => w.WriteCollectionOfObjectValues("items", Arg.Any<IEnumerable<ScimConfigurationPublic>>())).Do(ci =>
        {
            var items = ci.ArgAt<IEnumerable<ScimConfigurationPublic>>(1);
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
        utf8JsonWriter.WriteStartArray("items");
        foreach (var item in capturedItems)
        {
            utf8JsonWriter.WriteStartObject();
            utf8JsonWriter.WriteString("name", item.Name);
            utf8JsonWriter.WriteEndObject();
        }
        utf8JsonWriter.WriteEndArray();
        foreach (var kvp in capturedAdditionalData)
        {
            utf8JsonWriter.WriteString(kvp.Key, kvp.Value.ToString());
        }
        utf8JsonWriter.WriteEndObject();
        utf8JsonWriter.Flush();

        return Encoding.UTF8.GetString(memoryStream.ToArray());
    }

    private static ScimConfigurationsGetResponse DeserializeFromJson(string jsonString)
    {
        var jsonDoc = JsonDocument.Parse(jsonString);
        var response = new ScimConfigurationsGetResponse();

        if (jsonDoc.RootElement.TryGetProperty("items", out var itemsElement))
        {
            var items = new List<ScimConfigurationPublic>();
            foreach (var item in itemsElement.EnumerateArray())
            {
                var scimConfig = new ScimConfigurationPublic();
                if (item.TryGetProperty("name", out var nameElement))
                {
                    scimConfig.Name = nameElement.GetString();
                }
                items.Add(scimConfig);
            }
            response.Items = items;
        }

        foreach (var property in jsonDoc.RootElement.EnumerateObject())
        {
            if (property.Name == "items") continue;
            response.AdditionalData[property.Name] = property.Value.GetString() ?? string.Empty;
        }

        return response;
    }
}
