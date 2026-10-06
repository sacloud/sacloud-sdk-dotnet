using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.IAM.ScimConfigurations.Item;
using System.Text;
using System.Text.Json;

namespace Sacloud.Sdk.IAM.Tests.ScimConfigurations.Item;

/// <summary>
/// ScimConfigurationsPutRequestBodyのテスト
/// </summary>
public class ScimConfigurationsPutRequestBodyTests
{
    /// <summary>
    /// コンストラクタ実行後にAdditionalDataが空のDictionaryで初期化されることを検証
    /// </summary>
    [Fact]
    public void コンストラクタ_AdditionalDataが空のDictionaryで初期化される()
    {
        var body = new ScimConfigurationsPutRequestBody();

        Assert.NotNull(body.AdditionalData);
        Assert.Empty(body.AdditionalData);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_インスタンスを返す()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = ScimConfigurationsPutRequestBody.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<ScimConfigurationsPutRequestBody>(result);
    }

    /// <summary>
    /// GetFieldDeserializersがnameキーのみを含む1件のマッピングを返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_nameキーのみを含む()
    {
        var body = new ScimConfigurationsPutRequestBody();

        var deserializers = body.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("name"));
    }

    /// <summary>
    /// GetFieldDeserializersのnameアクションがNameプロパティをセットすることを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_nameアクションがNameプロパティをセットする()
    {
        var body = new ScimConfigurationsPutRequestBody();
        var parseNode = Substitute.For<IParseNode>();
        parseNode.GetStringValue().Returns("test-name");

        var deserializers = body.GetFieldDeserializers();
        deserializers["name"](parseNode);

        Assert.Equal("test-name", body.Name);
    }

    /// <summary>
    /// SerializeがWriteStringValueを通じてnameプロパティを書き込むことを検証
    /// </summary>
    [Fact]
    public void Serialize_nameプロパティを書き込む()
    {
        var body = new ScimConfigurationsPutRequestBody { Name = "my-scim-config" };
        var writer = Substitute.For<ISerializationWriter>();

        body.Serialize(writer);

        writer.Received(1).WriteStringValue("name", "my-scim-config");
    }

    /// <summary>
    /// SerializeがWriteAdditionalDataを呼び出すことを検証
    /// </summary>
    [Fact]
    public void Serialize_AdditionalDataを書き込む()
    {
        var body = new ScimConfigurationsPutRequestBody();
        var writer = Substitute.For<ISerializationWriter>();

        body.Serialize(writer);

        writer.Received(1).WriteAdditionalData(body.AdditionalData);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_nullのParseNodeでArgumentNullExceptionをスロー()
    {
        Assert.Throws<ArgumentNullException>(() => ScimConfigurationsPutRequestBody.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_nullのwriterでArgumentNullExceptionをスロー()
    {
        var body = new ScimConfigurationsPutRequestBody();

        Assert.Throws<ArgumentNullException>(() => body.Serialize(null));
    }

    /// <summary>
    /// 全プロパティでシリアライズとデシリアライズができることを検証
    /// </summary>
    [Fact]
    public void 全プロパティでシリアライズとデシリアライズができる()
    {
        var body = new ScimConfigurationsPutRequestBody
        {
            Name = "Test SCIM Configuration"
        };
        var jsonString = SerializeToJson(body);
        var deserialized = DeserializeFromJson(jsonString);

        Assert.Equal("Test SCIM Configuration", deserialized.Name);
    }

    /// <summary>
    /// AdditionalDataでシリアライズとデシリアライズができることを検証
    /// </summary>
    [Fact]
    public void AdditionalDataでシリアライズとデシリアライズができる()
    {
        var body = new ScimConfigurationsPutRequestBody
        {
            Name = "Test SCIM Configuration",
            AdditionalData = new Dictionary<string, object>
            {
                { "customField", "customValue" }
            }
        };
        var jsonString = SerializeToJson(body);
        var deserialized = DeserializeFromJson(jsonString);

        Assert.Equal("Test SCIM Configuration", deserialized.Name);
        Assert.NotNull(deserialized.AdditionalData);
        Assert.True(deserialized.AdditionalData.ContainsKey("customField"));
    }

    private static string SerializeToJson(ScimConfigurationsPutRequestBody body)
    {
        var writer = Substitute.For<ISerializationWriter>();
        var memoryStream = new MemoryStream();
        var utf8JsonWriter = new Utf8JsonWriter(memoryStream);

        var capturedValues = new Dictionary<string, string?>();
        var capturedAdditionalData = new Dictionary<string, object>();

        writer.When(w => w.WriteStringValue(Arg.Any<string>(), Arg.Any<string>())).Do(ci =>
        {
            var key = ci.ArgAt<string>(0);
            var value = ci.ArgAt<string>(1);
            capturedValues[key] = value;
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
        foreach (var kvp in capturedValues)
        {
            if (kvp.Value is not null)
            {
                utf8JsonWriter.WriteString(kvp.Key, kvp.Value);
            }
        }
        foreach (var kvp in capturedAdditionalData)
        {
            utf8JsonWriter.WriteString(kvp.Key, kvp.Value.ToString());
        }
        utf8JsonWriter.WriteEndObject();
        utf8JsonWriter.Flush();

        return Encoding.UTF8.GetString(memoryStream.ToArray());
    }

    private static ScimConfigurationsPutRequestBody DeserializeFromJson(string jsonString)
    {
        var jsonDoc = JsonDocument.Parse(jsonString);
        var body = new ScimConfigurationsPutRequestBody();
        var deserializers = body.GetFieldDeserializers();

        foreach (var property in jsonDoc.RootElement.EnumerateObject())
        {
            if (deserializers.ContainsKey(property.Name))
            {
                var parseNode = Substitute.For<IParseNode>();
                parseNode.GetStringValue().Returns(property.Value.GetString());
                deserializers[property.Name](parseNode);
            }
            else
            {
                body.AdditionalData[property.Name] = property.Value.GetString() ?? string.Empty;
            }
        }

        return body;
    }
}
