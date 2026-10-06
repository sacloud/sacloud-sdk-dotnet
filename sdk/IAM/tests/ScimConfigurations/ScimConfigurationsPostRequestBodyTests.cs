using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.IAM.ScimConfigurations;
using System.Text;
using System.Text.Json;

namespace Sacloud.Sdk.IAM.Tests.ScimConfigurations;

/// <summary>
/// ScimConfigurationsPostRequestBodyのテスト
/// </summary>
public class ScimConfigurationsPostRequestBodyTests
{
    /// <summary>
    /// コンストラクタ実行後にAdditionalDataが空のDictionaryで初期化されることを検証
    /// </summary>
    [Fact]
    public void コンストラクタ_AdditionalDataが空のDictionaryで初期化される()
    {
        var body = new ScimConfigurationsPostRequestBody();

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

        var result = ScimConfigurationsPostRequestBody.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<ScimConfigurationsPostRequestBody>(result);
    }

    /// <summary>
    /// GetFieldDeserializersがnameキーのみを含む1件のマッピングを返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_nameキーのみを含む()
    {
        var body = new ScimConfigurationsPostRequestBody();

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
        var body = new ScimConfigurationsPostRequestBody();
        var deserializers = body.GetFieldDeserializers();

        var parseNode = Substitute.For<IParseNode>();
        parseNode.GetStringValue().Returns("Test SCIM Configuration");

        deserializers["name"](parseNode);

        Assert.Equal("Test SCIM Configuration", body.Name);
    }

    /// <summary>
    /// SerializeがWriteStringValueを通じてnameプロパティを書き込むことを検証
    /// </summary>
    [Fact]
    public void Serialize_nameプロパティを書き込む()
    {
        var body = new ScimConfigurationsPostRequestBody
        {
            Name = "Test SCIM Configuration",
        };
        var writer = Substitute.For<ISerializationWriter>();

        body.Serialize(writer);

        writer.Received(1).WriteStringValue("name", "Test SCIM Configuration");
    }

    /// <summary>
    /// SerializeがWriteAdditionalDataを呼び出すことを検証
    /// </summary>
    [Fact]
    public void Serialize_AdditionalDataを書き込む()
    {
        var body = new ScimConfigurationsPostRequestBody();
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
        Assert.Throws<ArgumentNullException>(() => ScimConfigurationsPostRequestBody.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_nullのwriterでArgumentNullExceptionをスロー()
    {
        var body = new ScimConfigurationsPostRequestBody();

        Assert.Throws<ArgumentNullException>(() => body.Serialize(null));
    }

    /// <summary>
    /// 全プロパティでシリアライズとデシリアライズができることを検証
    /// </summary>
    [Fact]
    public void 全プロパティでシリアライズとデシリアライズができる()
    {
        var body = new ScimConfigurationsPostRequestBody
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
        var body = new ScimConfigurationsPostRequestBody
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

    private static string SerializeToJson(ScimConfigurationsPostRequestBody body)
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

    private static ScimConfigurationsPostRequestBody DeserializeFromJson(string jsonString)
    {
        var jsonDoc = JsonDocument.Parse(jsonString);
        var body = new ScimConfigurationsPostRequestBody();
        var deserializers = body.GetFieldDeserializers();

        foreach (var property in jsonDoc.RootElement.EnumerateObject())
        {
            if (deserializers.TryGetValue(property.Name, out Action<IParseNode>? value))
            {
                var parseNode = Substitute.For<IParseNode>();
                parseNode.GetStringValue().Returns(property.Value.GetString());
                value(parseNode);
            }
            else
            {
                body.AdditionalData[property.Name] = property.Value.GetString() ?? string.Empty;
            }
        }

        return body;
    }
}
