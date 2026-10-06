using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.IAM.Models;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// RuleTemplateモデルのテスト
/// </summary>
public class RuleTemplateTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = RuleTemplate.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<RuleTemplate>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() => RuleTemplate.CreateFromDiscriminatorValue(parseNode));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new RuleTemplate();

        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(6, deserializers.Count);
        Assert.True(deserializers.ContainsKey("code"));
        Assert.True(deserializers.ContainsKey("description"));
        Assert.True(deserializers.ContainsKey("name"));
        Assert.True(deserializers.ContainsKey("prefixes"));
        Assert.True(deserializers.ContainsKey("supports_dry_run"));
        Assert.True(deserializers.ContainsKey("type"));
    }

    /// <summary>
    /// GetFieldDeserializersが"code"キーを含むことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ContainsCodeKey()
    {
        var model = new RuleTemplate();

        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.True(deserializers.ContainsKey("code"));
    }

    /// <summary>
    /// GetFieldDeserializersが"description"キーを含むことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ContainsDescriptionKey()
    {
        var model = new RuleTemplate();

        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.True(deserializers.ContainsKey("description"));
    }

    /// <summary>
    /// GetFieldDeserializersが"name"キーを含むことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ContainsNameKey()
    {
        var model = new RuleTemplate();

        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.True(deserializers.ContainsKey("name"));
    }

    /// <summary>
    /// GetFieldDeserializersが"prefixes"キーを含むことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ContainsPrefixesKey()
    {
        var model = new RuleTemplate();

        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.True(deserializers.ContainsKey("prefixes"));
    }

    /// <summary>
    /// GetFieldDeserializersが"supports_dry_run"キーを含むことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ContainsSupportsDryRunKey()
    {
        var model = new RuleTemplate();

        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.True(deserializers.ContainsKey("supports_dry_run"));
    }

    /// <summary>
    /// GetFieldDeserializersが"type"キーを含むことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ContainsTypeKey()
    {
        var model = new RuleTemplate();

        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.True(deserializers.ContainsKey("type"));
    }

    /// <summary>
    /// Serializeがcodeプロパティを書き込むことを確認
    /// </summary>
    [Fact]
    public void Serialize_WritesCodeProperty()
    {
        var model = new RuleTemplate
        {
            Code = "test-code"
        };
        var writer = Substitute.For<ISerializationWriter>();

        model.Serialize(writer);

        writer.Received(1).WriteStringValue("code", "test-code");
    }

    /// <summary>
    /// Serializeがdescriptionプロパティを書き込むことを確認
    /// </summary>
    [Fact]
    public void Serialize_WritesDescriptionProperty()
    {
        var model = new RuleTemplate
        {
            Description = "Test Description"
        };
        var writer = Substitute.For<ISerializationWriter>();

        model.Serialize(writer);

        writer.Received(1).WriteStringValue("description", "Test Description");
    }

    /// <summary>
    /// Serializeがnameプロパティを書き込むことを確認
    /// </summary>
    [Fact]
    public void Serialize_WritesNameProperty()
    {
        var model = new RuleTemplate
        {
            Name = "Test Name"
        };
        var writer = Substitute.For<ISerializationWriter>();

        model.Serialize(writer);

        writer.Received(1).WriteStringValue("name", "Test Name");
    }

    /// <summary>
    /// Serializeがprefixesプロパティを書き込むことを確認
    /// </summary>
    [Fact]
    public void Serialize_WritesPrefixesProperty()
    {
        var model = new RuleTemplate
        {
            Prefixes = ["prefix1", "prefix2"]
        };
        var writer = Substitute.For<ISerializationWriter>();

        model.Serialize(writer);

        writer.Received(1).WriteCollectionOfPrimitiveValues("prefixes", model.Prefixes);
    }

    /// <summary>
    /// Serializeがsupports_dry_runプロパティを書き込むことを確認
    /// </summary>
    [Fact]
    public void Serialize_WritesSupportsDryRunProperty()
    {
        var model = new RuleTemplate
        {
            SupportsDryRun = true
        };
        var writer = Substitute.For<ISerializationWriter>();

        model.Serialize(writer);

        writer.Received(1).WriteBoolValue("supports_dry_run", true);
    }

    /// <summary>
    /// Serializeがtypeプロパティを書き込むことを確認
    /// </summary>
    [Fact]
    public void Serialize_WritesTypeProperty()
    {
        var model = new RuleTemplate
        {
            Type = "boolean"
        };
        var writer = Substitute.For<ISerializationWriter>();

        model.Serialize(writer);

        writer.Received(1).WriteStringValue("type", "boolean");
    }

    /// <summary>
    /// SerializeがAdditionalDataを書き込むことを確認
    /// </summary>
    [Fact]
    public void Serialize_WritesAdditionalData()
    {
        var model = new RuleTemplate();
        model.AdditionalData["customKey"] = "customValue";
        var writer = Substitute.For<ISerializationWriter>();

        model.Serialize(writer);

        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new RuleTemplate();

        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void AllActions_WorkCorrectly()
    {
        var model = new RuleTemplate();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetStringValue().Returns("test-code");
        deserializers["code"](parseNode);
        Assert.Equal("test-code", model.Code);

        parseNode.GetStringValue().Returns("Test Description");
        deserializers["description"](parseNode);
        Assert.Equal("Test Description", model.Description);

        parseNode.GetStringValue().Returns("Test Name");
        deserializers["name"](parseNode);
        Assert.Equal("Test Name", model.Name);

        var prefixes = new List<string> { "prefix1", "prefix2" };
        parseNode.GetCollectionOfPrimitiveValues<string>().Returns(prefixes);
        deserializers["prefixes"](parseNode);
        Assert.Equal(prefixes, model.Prefixes);

        parseNode.GetBoolValue().Returns(true);
        deserializers["supports_dry_run"](parseNode);
        Assert.Equal(true, model.SupportsDryRun);

        parseNode.GetStringValue().Returns("boolean");
        deserializers["type"](parseNode);
        Assert.Equal("boolean", model.Type);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAllProperties()
    {
        var jsonContent = @"{
            ""code"": ""test-code"",
            ""description"": ""Test Description"",
            ""name"": ""Test Name"",
            ""prefixes"": [""prefix1"", ""prefix2""],
            ""supports_dry_run"": true,
            ""type"": ""boolean""
        }";

        using var serializeStream = new MemoryStream();
        using var streamWriter = new StreamWriter(serializeStream);
        await streamWriter.WriteAsync(jsonContent);
        await streamWriter.FlushAsync();

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(RuleTemplate.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal("test-code", deserializedModel.Code);
        Assert.Equal("Test Description", deserializedModel.Description);
        Assert.Equal("Test Name", deserializedModel.Name);
        Assert.NotNull(deserializedModel.Prefixes);
        Assert.Equal(2, deserializedModel.Prefixes.Count);
        Assert.Contains("prefix1", deserializedModel.Prefixes);
        Assert.Contains("prefix2", deserializedModel.Prefixes);
        Assert.Equal(true, deserializedModel.SupportsDryRun);
        Assert.Equal("boolean", deserializedModel.Type);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを確認
    /// </summary>
    [Fact]
    public async Task CanSerializeAndDeserializeWithAdditionalData()
    {
        var jsonContent = @"{
            ""code"": ""test-code"",
            ""name"": ""Test Name"",
            ""customField"": ""customValue""
        }";

        using var serializeStream = new MemoryStream();
        using var streamWriter = new StreamWriter(serializeStream);
        await streamWriter.WriteAsync(jsonContent);
        await streamWriter.FlushAsync();

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(RuleTemplate.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal("test-code", deserializedModel.Code);
        Assert.Equal("Test Name", deserializedModel.Name);
        Assert.NotNull(deserializedModel.AdditionalData);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
