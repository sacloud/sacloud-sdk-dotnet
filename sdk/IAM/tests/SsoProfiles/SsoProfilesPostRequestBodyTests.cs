using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using System.Text;
using System.Text.Json;
using Sacloud.Sdk.IAM.SsoProfiles;

namespace Sacloud.Sdk.IAM.Tests.SsoProfiles;

/// <summary>
/// SsoProfilesPostRequestBodyのテスト
/// </summary>
public class SsoProfilesPostRequestBodyTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = SsoProfilesPostRequestBody.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<SsoProfilesPostRequestBody>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => SsoProfilesPostRequestBody.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var body = new SsoProfilesPostRequestBody();

        var deserializers = body.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(6, deserializers.Count);
        Assert.True(deserializers.ContainsKey("name"));
        Assert.True(deserializers.ContainsKey("description"));
        Assert.True(deserializers.ContainsKey("idp_entity_id"));
        Assert.True(deserializers.ContainsKey("idp_login_url"));
        Assert.True(deserializers.ContainsKey("idp_logout_url"));
        Assert.True(deserializers.ContainsKey("idp_certificate"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var body = new SsoProfilesPostRequestBody();

        Assert.Throws<ArgumentNullException>(() => body.Serialize(null));
    }

    /// <summary>
    /// Serializeがすべてのプロパティを書き込むことを検証
    /// </summary>
    [Fact]
    public void Serialize_WritesAllProperties()
    {
        var body = new SsoProfilesPostRequestBody
        {
            Name = "Test SSO Profile",
            Description = "Test Description",
            IdpEntityId = "https://idp.example.com",
            IdpLoginUrl = "https://idp.example.com/login",
            IdpLogoutUrl = "https://idp.example.com/logout",
            IdpCertificate = "CERTIFICATE"
        };
        var writer = Substitute.For<ISerializationWriter>();

        body.Serialize(writer);

        writer.Received(1).WriteStringValue("name", "Test SSO Profile");
        writer.Received(1).WriteStringValue("description", "Test Description");
        writer.Received(1).WriteStringValue("idp_entity_id", "https://idp.example.com");
        writer.Received(1).WriteStringValue("idp_login_url", "https://idp.example.com/login");
        writer.Received(1).WriteStringValue("idp_logout_url", "https://idp.example.com/logout");
        writer.Received(1).WriteStringValue("idp_certificate", "CERTIFICATE");
        writer.Received(1).WriteAdditionalData(body.AdditionalData);
    }

    /// <summary>
    /// GetFieldDeserializersのNameアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_NameAction_WorksCorrectly()
    {
        var body = new SsoProfilesPostRequestBody();
        var deserializers = body.GetFieldDeserializers();
        
        var parseNode = Substitute.For<IParseNode>();
        parseNode.GetStringValue().Returns("Test SSO Profile");
        
        deserializers["name"](parseNode);

        Assert.Equal("Test SSO Profile", body.Name);
    }

    /// <summary>
    /// GetFieldDeserializersのDescriptionアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_DescriptionAction_WorksCorrectly()
    {
        var body = new SsoProfilesPostRequestBody();
        var deserializers = body.GetFieldDeserializers();
        
        var parseNode = Substitute.For<IParseNode>();
        parseNode.GetStringValue().Returns("Test Description");
        
        deserializers["description"](parseNode);

        Assert.Equal("Test Description", body.Description);
    }

    /// <summary>
    /// GetFieldDeserializersのIdpEntityIdアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_IdpEntityIdAction_WorksCorrectly()
    {
        var body = new SsoProfilesPostRequestBody();
        var deserializers = body.GetFieldDeserializers();
        
        var parseNode = Substitute.For<IParseNode>();
        parseNode.GetStringValue().Returns("https://idp.example.com");
        
        deserializers["idp_entity_id"](parseNode);

        Assert.Equal("https://idp.example.com", body.IdpEntityId);
    }

    /// <summary>
    /// GetFieldDeserializersのIdpLoginUrlアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_IdpLoginUrlAction_WorksCorrectly()
    {
        var body = new SsoProfilesPostRequestBody();
        var deserializers = body.GetFieldDeserializers();
        
        var parseNode = Substitute.For<IParseNode>();
        parseNode.GetStringValue().Returns("https://idp.example.com/login");
        
        deserializers["idp_login_url"](parseNode);

        Assert.Equal("https://idp.example.com/login", body.IdpLoginUrl);
    }

    /// <summary>
    /// GetFieldDeserializersのIdpLogoutUrlアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_IdpLogoutUrlAction_WorksCorrectly()
    {
        var body = new SsoProfilesPostRequestBody();
        var deserializers = body.GetFieldDeserializers();
        
        var parseNode = Substitute.For<IParseNode>();
        parseNode.GetStringValue().Returns("https://idp.example.com/logout");
        
        deserializers["idp_logout_url"](parseNode);

        Assert.Equal("https://idp.example.com/logout", body.IdpLogoutUrl);
    }

    /// <summary>
    /// GetFieldDeserializersのIdpCertificateアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_IdpCertificateAction_WorksCorrectly()
    {
        var body = new SsoProfilesPostRequestBody();
        var deserializers = body.GetFieldDeserializers();
        
        var parseNode = Substitute.For<IParseNode>();
        parseNode.GetStringValue().Returns("CERTIFICATE");
        
        deserializers["idp_certificate"](parseNode);

        Assert.Equal("CERTIFICATE", body.IdpCertificate);
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public void CanSerializeAndDeserializeWithAllProperties()
    {
        var body = new SsoProfilesPostRequestBody
        {
            Name = "Test SSO Profile",
            Description = "Test Description",
            IdpEntityId = "https://idp.example.com",
            IdpLoginUrl = "https://idp.example.com/login",
            IdpLogoutUrl = "https://idp.example.com/logout",
            IdpCertificate = "CERTIFICATE"
        };
        var jsonString = SerializeToJson(body);
        var deserialized = DeserializeFromJson(jsonString);

        Assert.Equal("Test SSO Profile", deserialized.Name);
        Assert.Equal("Test Description", deserialized.Description);
        Assert.Equal("https://idp.example.com", deserialized.IdpEntityId);
        Assert.Equal("https://idp.example.com/login", deserialized.IdpLoginUrl);
        Assert.Equal("https://idp.example.com/logout", deserialized.IdpLogoutUrl);
        Assert.Equal("CERTIFICATE", deserialized.IdpCertificate);
    }

    /// <summary>
    /// 部分的なプロパティでシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public void CanSerializeAndDeserializeWithPartialProperties()
    {
        var body = new SsoProfilesPostRequestBody
        {
            Name = "Test SSO Profile",
            IdpEntityId = "https://idp.example.com"
        };
        var jsonString = SerializeToJson(body);
        var deserialized = DeserializeFromJson(jsonString);

        Assert.Equal("Test SSO Profile", deserialized.Name);
        Assert.Equal("https://idp.example.com", deserialized.IdpEntityId);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public void CanSerializeAndDeserializeWithAdditionalData()
    {
        var body = new SsoProfilesPostRequestBody
        {
            Name = "Test SSO Profile",
            AdditionalData = new Dictionary<string, object>
            {
                { "customField", "customValue" }
            }
        };
        var jsonString = SerializeToJson(body);
        var deserialized = DeserializeFromJson(jsonString);

        Assert.Equal("Test SSO Profile", deserialized.Name);
        Assert.NotNull(deserialized.AdditionalData);
        Assert.True(deserialized.AdditionalData.ContainsKey("customField"));
    }

    /// <summary>
    /// AdditionalDataとプロパティが共存できることを検証
    /// </summary>
    [Fact]
    public void AdditionalDataAndProperties_CanCoexist()
    {
        var body = new SsoProfilesPostRequestBody
        {
            Name = "Test SSO Profile",
            Description = "Test Description",
            AdditionalData = new Dictionary<string, object>
            {
                { "customField", "customValue" }
            }
        };
        var jsonString = SerializeToJson(body);
        var deserialized = DeserializeFromJson(jsonString);

        Assert.Equal("Test SSO Profile", deserialized.Name);
        Assert.Equal("Test Description", deserialized.Description);
        Assert.NotNull(deserialized.AdditionalData);
        Assert.True(deserialized.AdditionalData.ContainsKey("customField"));
    }

    private static string SerializeToJson(SsoProfilesPostRequestBody body)
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

    private static SsoProfilesPostRequestBody DeserializeFromJson(string jsonString)
    {
        var jsonDoc = JsonDocument.Parse(jsonString);
        var body = new SsoProfilesPostRequestBody();
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
