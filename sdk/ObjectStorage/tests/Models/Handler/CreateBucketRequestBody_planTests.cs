using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Models.Handler;
using Sacloud.Sdk.ObjectStorage.Models.Model;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models.Handler;

/// <summary>
/// CreateBucketRequestBody_planモデルのテスト
/// </summary>
public class CreateBucketRequestBody_planTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void CreateBucketRequestBody_plan_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new CreateBucketRequestBody_plan();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("service_class_path"));
        Assert.True(deserializers.ContainsKey("type"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateBucketRequestBody_plan_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new CreateBucketRequestBody_plan();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void CreateBucketRequestBody_plan_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new CreateBucketRequestBody_plan();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("service_class_path"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["service_class_path"](parseNode);
        Assert.NotNull(model.ServiceClassPath);

        Assert.True(deserializers.ContainsKey("type"));

        parseNode.GetEnumValue<PlanType>().Returns(PlanType.Standard);

        deserializers["type"](parseNode);
        Assert.NotNull(model.Type);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task CreateBucketRequestBody_plan_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new CreateBucketRequestBody_plan
        {
            ServiceClassPath = "sample"
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(CreateBucketRequestBody_plan.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.ServiceClassPath, deserializedModel.ServiceClassPath);
        Assert.Equal(originalModel.Type, deserializedModel.Type);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task CreateBucketRequestBody_plan_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new CreateBucketRequestBody_plan
        {
            AdditionalData = new Dictionary<string, object>
            {
                { "extra_key", "extra-value" },
            },
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, model);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(CreateBucketRequestBody_plan.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
