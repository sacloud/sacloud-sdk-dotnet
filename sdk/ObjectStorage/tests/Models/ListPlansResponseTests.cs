using System.Reflection;
using System.Runtime.Serialization;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Models;
using Sacloud.Sdk.ObjectStorage.Models.Model;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models;

/// <summary>
/// ListPlansResponseモデルのテスト
/// </summary>
public class ListPlansResponseTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void ListPlansResponse_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new ListPlansResponse();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("data"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ListPlansResponse_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ListPlansResponse();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void ListPlansResponse_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new ListPlansResponse();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("data"));

        parseNode.GetCollectionOfObjectValues(Arg.Any<ParsableFactory<PlanItem>>()).Returns([new PlanItem()]);

        deserializers["data"](parseNode);
        Assert.NotNull(model.Data);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task ListPlansResponse_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ListPlansResponse
        {
            Data =
            [
                new PlanItem
                {
                    CapacityGib = 1,
                    ClusterId = "cluster-1",
                    Fee = new PlanItem_fee
                    {
                        Daily = 1,
                        ForMonth = 2,
                        Hourly = 3,
                        Monthly = 4,
                    },
                    ServiceClassPath = "storage/standard",
                    Type = PlanType.Standard,
                },
            ]
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(ListPlansResponse.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.Data);
        Assert.Single(deserializedModel.Data);
        Assert.Equal(originalModel.Data[0].CapacityGib, deserializedModel.Data[0].CapacityGib);
        Assert.Equal(originalModel.Data[0].ClusterId, deserializedModel.Data[0].ClusterId);
        Assert.NotNull(deserializedModel.Data[0].Fee);
        Assert.Equal(originalModel.Data[0].Fee.Daily, deserializedModel.Data[0].Fee.Daily);
        Assert.Equal(originalModel.Data[0].ServiceClassPath, deserializedModel.Data[0].ServiceClassPath);
        Assert.Equal(originalModel.Data[0].Type, deserializedModel.Data[0].Type);

        var enumMember = typeof(PlanType).GetField(nameof(PlanType.Standard))?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(enumMember);
        Assert.Equal("standard", enumMember.Value);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task ListPlansResponse_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new ListPlansResponse
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
        var deserializedModel = parseNode.GetObjectValue(ListPlansResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("extra_key"));
    }
}
