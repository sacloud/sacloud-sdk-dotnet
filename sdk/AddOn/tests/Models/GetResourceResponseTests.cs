using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;

namespace Sacloud.Sdk.AddOn.Tests.Models;

/// <summary>
/// GetResourceResponseモデルのテスト
/// </summary>
public class GetResourceResponseTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void GetResourceResponse_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new global::Sacloud.Sdk.AddOn.Models.GetResourceResponse();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("data"));
        Assert.True(deserializers.ContainsKey("url"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void GetResourceResponse_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new global::Sacloud.Sdk.AddOn.Models.GetResourceResponse();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void GetResourceResponse_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new global::Sacloud.Sdk.AddOn.Models.GetResourceResponse();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("data"));

        parseNode.GetObjectValue<UntypedNode>(Arg.Any<ParsableFactory<UntypedNode>>()).Returns(Substitute.For<UntypedNode>());

        deserializers["data"](parseNode);
        Assert.NotNull(model.Data);

        Assert.True(deserializers.ContainsKey("url"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["url"](parseNode);
        Assert.NotNull(model.Url);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Theory]
    [MemberData(nameof(GetResourceResponse_DataRoundTripCases))]
    public async Task GetResourceResponse_CanSerializeAndDeserializeWithAllProperties(string caseId)
    {
        // ケースIDに基づいてテスト用のデータを作成
        var (inputData, expectedTypeName, expectedValue) = CreateRoundTripCase(caseId);

        var originalModel = new global::Sacloud.Sdk.AddOn.Models.GetResourceResponse
        {
            Data = inputData,
            Url = "sample"
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue<global::Sacloud.Sdk.AddOn.Models.GetResourceResponse>(global::Sacloud.Sdk.AddOn.Models.GetResourceResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);

        // GetResourceResponse.DataにUntypedNullが設定されている場合、デシリアライズ後もDataはnullであることを確認
        if (expectedTypeName == nameof(UntypedNull))
        {
            Assert.Null(deserializedModel!.Data);
            return;
        }

        // GetResourceResponse.DataにUntypedNull以外の型が設定されている場合、デシリアライズ後もDataはnullでないことを確認
        Assert.NotNull(deserializedModel!.Data);
        Assert.Equal(expectedTypeName, deserializedModel.Data.GetType().Name);

        // デシリアライズ後のDataの型に応じて、その値が期待値と一致することを確認
        switch (deserializedModel.Data)
        {
            case UntypedString untypedString:
                Assert.Equal((string)expectedValue!, untypedString.GetValue());
                break;
            case UntypedInteger untypedInteger:
                Assert.Equal((int)expectedValue!, untypedInteger.GetValue());
                break;
            case UntypedBoolean untypedBoolean:
                Assert.Equal((bool)expectedValue!, untypedBoolean.GetValue());
                break;
            case UntypedDecimal untypedDecimal:
                Assert.Equal((decimal)expectedValue!, untypedDecimal.GetValue());
                break;
            case UntypedLong untypedLong:
                Assert.Equal((long)expectedValue!, untypedLong.GetValue());
                break;
            case UntypedArray untypedArray:
                Assert.NotNull(untypedArray.GetValue());
                Assert.Equal((int)expectedValue!, untypedArray.GetValue()!.Count());
                break;
            case UntypedObject untypedObject:
                Assert.NotNull(untypedObject.GetValue());
                Assert.Equal((int)expectedValue!, untypedObject.GetValue()!.Count());
                break;
            default:
                throw new InvalidOperationException($"未対応のUntypedNode型です: {deserializedModel.Data.GetType().FullName}");
        }

        Assert.Equal("sample", deserializedModel.Url);
    }

    /// <summary>
    /// GetResourceResponse_CanSerializeAndDeserializeWithAllPropertiesテストで使用するケースIDを提供
    /// </summary>
    public static TheoryData<string> GetResourceResponse_DataRoundTripCases => new()
    {
        nameof(UntypedString),
        nameof(UntypedInteger),
        nameof(UntypedBoolean),
        nameof(UntypedDecimal),
        nameof(UntypedLong),
        nameof(UntypedArray),
        nameof(UntypedObject),
        nameof(UntypedNull)
    };

    /// <summary>
    /// 指定されたケースIDに基づいて、シリアライズとデシリアライズのテスト用のデータを作成
    /// </summary>
    /// <param name="caseId">ケースID</param>
    private static (UntypedNode InputData, string ExpectedTypeName, object? ExpectedValue) CreateRoundTripCase(string caseId)
    {
        return caseId switch
        {
            nameof(UntypedString) => (new UntypedString("text-value"), nameof(UntypedString), "text-value"),
            nameof(UntypedInteger) => (new UntypedInteger(123), nameof(UntypedInteger), 123),
            nameof(UntypedBoolean) => (new UntypedBoolean(true), nameof(UntypedBoolean), true),
            nameof(UntypedDecimal) => (new UntypedDecimal(99.5m), nameof(UntypedDecimal), 99.5m),
            nameof(UntypedLong) => (new UntypedLong(1234567890123L), nameof(UntypedLong), 1234567890123L),
            nameof(UntypedArray) =>
                (
                    new UntypedArray(new List<UntypedNode>
                    {
                        new UntypedString("item1"),
                        new UntypedInteger(2)
                    }),
                    nameof(UntypedArray),
                    2
                ),
            nameof(UntypedObject) =>
                (
                    new UntypedObject(new Dictionary<string, UntypedNode>
                    {
                        ["name"] = new UntypedString("value"),
                        ["enabled"] = new UntypedBoolean(true)
                    }),
                    nameof(UntypedObject),
                    2
                ),
            nameof(UntypedNull) => (new UntypedNull(), nameof(UntypedNull), null),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, "未対応のテストケースです。")
        };
    }
}
