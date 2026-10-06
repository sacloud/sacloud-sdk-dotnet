using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.Shared.Models;

namespace Sacloud.Sdk.Shared.Tests.Models;

/// <summary>
/// ApiErrorのテスト
/// </summary>
public class ApiErrorTests
{
    /// <summary>
    /// コンストラクタがResponseBodyを初期化することを確認
    /// </summary>
    [Fact]
    public void Constructor_InitializesResponseBody()
    {
        var apiError = new ApiError();

        Assert.NotNull(apiError.ResponseBody);
        Assert.Empty(apiError.ResponseBody);
    }

    /// <summary>
    /// ResponseBodyプロパティが内部ディクショナリを返すことを確認
    /// </summary>
    [Fact]
    public void ResponseBody_ReturnsInternalDictionary()
    {
        var apiError = new ApiError();
        apiError.ResponseBody["key1"] = "value1";
        apiError.ResponseBody["key2"] = 123;

        Assert.Equal(2, apiError.ResponseBody.Count);
        Assert.Equal("value1", apiError.ResponseBody["key1"]);
        Assert.Equal(123, apiError.ResponseBody["key2"]);
    }

    /// <summary>
    /// IAdditionalDataHolder.AdditionalDataがResponseBodyと同じ参照を返すことを確認
    /// </summary>
    [Fact]
    public void AdditionalData_ReturnsResponseBodyReference()
    {
        var apiError = new ApiError();
        var additionalDataHolder = (IAdditionalDataHolder)apiError;

        apiError.ResponseBody["testKey"] = "testValue";

        Assert.Same(apiError.ResponseBody, additionalDataHolder.AdditionalData);
        Assert.Equal("testValue", additionalDataHolder.AdditionalData["testKey"]);
    }

    /// <summary>
    /// IAdditionalDataHolder.AdditionalDataのgetterがResponseBodyを返すことを確認
    /// </summary>
    [Fact]
    public void AdditionalData_Getter_ReturnsResponseBody()
    {
        var apiError = new ApiError();
        var additionalDataHolder = (IAdditionalDataHolder)apiError;

        apiError.ResponseBody["key"] = "value";

        var additionalData = additionalDataHolder.AdditionalData;
        Assert.NotNull(additionalData);
        Assert.Equal("value", additionalData["key"]);
    }

    /// <summary>
    /// IAdditionalDataHolder.AdditionalDataのsetterがnull以外の値でResponseBodyを更新することを確認
    /// </summary>
    [Fact]
    public void AdditionalData_Setter_WithNonNullValue_UpdatesResponseBody()
    {
        var apiError = new ApiError();
        var additionalDataHolder = (IAdditionalDataHolder)apiError;

        var newData = new Dictionary<string, object>
        {
            { "newKey", "newValue" }
        };

        additionalDataHolder.AdditionalData = newData;

        Assert.Same(newData, apiError.ResponseBody);
        Assert.Equal("newValue", apiError.ResponseBody["newKey"]);
    }

    /// <summary>
    /// IAdditionalDataHolder.AdditionalDataのsetterがnull値で空のディクショナリを設定することを確認
    /// </summary>
    [Fact]
    public void AdditionalData_Setter_WithNullValue_SetsEmptyDictionary()
    {
        var apiError = new ApiError();
        var additionalDataHolder = (IAdditionalDataHolder)apiError;

        apiError.ResponseBody["existingKey"] = "existingValue";
        additionalDataHolder.AdditionalData = null!;

        Assert.NotNull(apiError.ResponseBody);
        Assert.Empty(apiError.ResponseBody);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeでArgumentNullExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
            ApiError.CreateFromDiscriminatorValue(null!));

        Assert.Equal("parseNode", exception.ParamName);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueが有効なparseNodeで新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithValidParseNode_ReturnsNewInstance()
    {
        var mockParseNode = Substitute.For<IParseNode>();

        var result = ApiError.CreateFromDiscriminatorValue(mockParseNode);

        Assert.NotNull(result);
        Assert.IsType<ApiError>(result);
        Assert.NotNull(result.ResponseBody);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueが呼び出し毎に新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsNewInstanceEachTime()
    {
        var mockParseNode = Substitute.For<IParseNode>();

        var result1 = ApiError.CreateFromDiscriminatorValue(mockParseNode);
        var result2 = ApiError.CreateFromDiscriminatorValue(mockParseNode);

        Assert.NotSame(result1, result2);
    }

    /// <summary>
    /// GetFieldDeserializersが空のディクショナリを返すことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsEmptyDictionary()
    {
        var apiError = new ApiError();

        var deserializers = apiError.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Empty(deserializers);
    }

    /// <summary>
    /// GetFieldDeserializersが呼び出し毎に新しいディクショナリを返すことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsNewDictionaryEachTime()
    {
        var apiError = new ApiError();

        var deserializers1 = apiError.GetFieldDeserializers();
        var deserializers2 = apiError.GetFieldDeserializers();

        Assert.NotSame(deserializers1, deserializers2);
    }

    /// <summary>
    /// SerializeがnullのwriterでArgumentNullExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var apiError = new ApiError();

        var exception = Assert.Throws<ArgumentNullException>(() =>
            apiError.Serialize(null!));

        Assert.Equal("writer", exception.ParamName);
    }

    /// <summary>
    /// SerializeがWriteAdditionalDataを呼び出すことを確認
    /// </summary>
    [Fact]
    public void Serialize_CallsWriteAdditionalData()
    {
        var apiError = new ApiError();
        apiError.ResponseBody["key1"] = "value1";
        apiError.ResponseBody["key2"] = 42;

        var mockWriter = Substitute.For<ISerializationWriter>();

        apiError.Serialize(mockWriter);

        mockWriter.Received(1).WriteAdditionalData(apiError.ResponseBody);
    }

    /// <summary>
    /// SerializeがResponseBodyが空の場合でもWriteAdditionalDataを呼び出すことを確認
    /// </summary>
    [Fact]
    public void Serialize_WithEmptyResponseBody_CallsWriteAdditionalData()
    {
        var apiError = new ApiError();
        var mockWriter = Substitute.For<ISerializationWriter>();

        apiError.Serialize(mockWriter);

        mockWriter.Received(1).WriteAdditionalData(apiError.ResponseBody);
    }

    /// <summary>
    /// SerializeがResponseBodyに複数のエントリがある場合に全て渡すことを確認
    /// </summary>
    [Fact]
    public void Serialize_WithMultipleResponseBodyEntries_PassesAllEntries()
    {
        var apiError = new ApiError();
        apiError.ResponseBody["field1"] = "value1";
        apiError.ResponseBody["field2"] = 100;
        apiError.ResponseBody["field3"] = true;

        var mockWriter = Substitute.For<ISerializationWriter>();

        apiError.Serialize(mockWriter);

        mockWriter.Received(1).WriteAdditionalData(
            Arg.Is<IDictionary<string, object>>(dict =>
                dict.Count == 3 &&
                dict["field1"].Equals("value1") &&
                dict["field2"].Equals(100) &&
                dict["field3"].Equals(true)));
    }

    /// <summary>
    /// ApiErrorがApiExceptionを継承していることを確認
    /// </summary>
    [Fact]
    public void ApiError_InheritsFromApiException()
    {
        var apiError = new ApiError();

        Assert.IsAssignableFrom<Microsoft.Kiota.Abstractions.ApiException>(apiError);
    }

    /// <summary>
    /// ApiErrorがIAdditionalDataHolderを実装していることを確認
    /// </summary>
    [Fact]
    public void ApiError_ImplementsIAdditionalDataHolder()
    {
        var apiError = new ApiError();

        Assert.IsAssignableFrom<IAdditionalDataHolder>(apiError);
    }

    /// <summary>
    /// ApiErrorがIParsableを実装していることを確認
    /// </summary>
    [Fact]
    public void ApiError_ImplementsIParsable()
    {
        var apiError = new ApiError();

        Assert.IsAssignableFrom<IParsable>(apiError);
    }

    /// <summary>
    /// ResponseBodyに複雑なネストされたオブジェクトを設定できることを確認
    /// </summary>
    [Fact]
    public void ResponseBody_SupportsNestedObjects()
    {
        var apiError = new ApiError();
        var nestedDict = new Dictionary<string, object>
        {
            { "innerKey", "innerValue" },
            { "innerNumber", 42 }
        };
        apiError.ResponseBody["outer"] = nestedDict;

        Assert.Single(apiError.ResponseBody);
        var retrievedDict = apiError.ResponseBody["outer"] as Dictionary<string, object>;
        Assert.NotNull(retrievedDict);
        Assert.Equal("innerValue", retrievedDict["innerKey"]);
        Assert.Equal(42, retrievedDict["innerNumber"]);
    }

    /// <summary>
    /// ResponseBodyからエントリを削除できることを確認
    /// </summary>
    [Fact]
    public void ResponseBody_SupportsRemoval()
    {
        var apiError = new ApiError();
        apiError.ResponseBody["key1"] = "value1";
        apiError.ResponseBody["key2"] = "value2";

        Assert.Equal(2, apiError.ResponseBody.Count);

        apiError.ResponseBody.Remove("key1");

        Assert.Single(apiError.ResponseBody);
        Assert.False(apiError.ResponseBody.ContainsKey("key1"));
        Assert.True(apiError.ResponseBody.ContainsKey("key2"));
    }

    /// <summary>
    /// ResponseBodyをクリアできることを確認
    /// </summary>
    [Fact]
    public void ResponseBody_SupportsClear()
    {
        var apiError = new ApiError();
        apiError.ResponseBody["key1"] = "value1";
        apiError.ResponseBody["key2"] = "value2";

        apiError.ResponseBody.Clear();

        Assert.Empty(apiError.ResponseBody);
    }
}
