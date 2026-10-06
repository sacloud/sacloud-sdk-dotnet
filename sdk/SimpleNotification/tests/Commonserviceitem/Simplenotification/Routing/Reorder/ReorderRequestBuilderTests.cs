using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.SimpleNotification.Commonserviceitem.Simplenotification.Routing.Reorder;
using Sacloud.Sdk.SimpleNotification.Models;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.SimpleNotification.Tests.Commonserviceitem.Simplenotification.Routing.Reorder;

/// <summary>
/// ReorderRequestBuilderのテスト
/// </summary>
public class ReorderRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<ReorderRequestBuilder> _mockLogger;

    public ReorderRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<ReorderRequestBuilder>>();
        _mockLoggerFactory.CreateLogger<ReorderRequestBuilder>().Returns(_mockLogger);
    }

    /// <summary>
    /// コンストラクタ（PathParameters版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };

        var builder = new ReorderRequestBuilder(pathParameters, requestAdapter);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();

        var builder = new ReorderRequestBuilder("https://api.example.com/commonserviceitem/simplenotification/routing/reorder", requestAdapter);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// ToPutRequestInformationが正しいRequestInformationを生成することを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_ReturnsCorrectRequestInformation()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var serializationWriter = Substitute.For<ISerializationWriter>();
        serializationWriter.GetSerializedContent().Returns(new MemoryStream());
        var serializationWriterFactory = Substitute.For<ISerializationWriterFactory>();
        serializationWriterFactory.GetSerializationWriter(Arg.Any<string>()).Returns(serializationWriter);
        requestAdapter.SerializationWriterFactory.Returns(serializationWriterFactory);
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new ReorderRequestBuilder(pathParameters, requestAdapter);
        var body = new PutCommonServiceItemRoutingReorderRequest
        {
            Orders = new List<PutCommonServiceItemRoutingReorderRequest_Orders>
            {
                new PutCommonServiceItemRoutingReorderRequest_Orders
                {
                    PriorityRank = 1,
                    RoutingID = "123456789012"
                }
            }
        };

        var requestInfo = builder.ToPutRequestInformation(body);

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.PUT, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToPutRequestInformationがnullボディで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_WithNullBody_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new ReorderRequestBuilder(pathParameters, requestAdapter);

        Assert.Throws<ArgumentNullException>(() => builder.ToPutRequestInformation(null!));
    }

    /// <summary>
    /// PutAsyncがRequestAdapter.SendAsyncを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_CallsRequestAdapterSendAsync()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new ReorderRequestBuilder(pathParameters, requestAdapter);
        var body = CreateValidRequestBody();
        var expectedResponse = new ReorderPutResponse();

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ReorderPutResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(expectedResponse);

        var result = await builder.PutAsync(body);

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ReorderPutResponse>>(),
            Arg.Is<Dictionary<string, ParsableFactory<IParsable>>>(x => HasApiErrorMapping(x)),
            Arg.Any<CancellationToken>());
        Assert.Equal(expectedResponse, result);
    }

    /// <summary>
    /// PutAsyncがnullボディでバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithNullBody_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new ReorderRequestBuilder(pathParameters, requestAdapter);

        await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(null!));
    }

    /// <summary>
    /// PutAsyncが空配列のOrdersを許容することを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithEmptyOrders_CallsRequestAdapterSendAsync()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new ReorderRequestBuilder(pathParameters, requestAdapter);
        var body = new PutCommonServiceItemRoutingReorderRequest
        {
            Orders = new List<PutCommonServiceItemRoutingReorderRequest_Orders>()
        };
        var expectedResponse = new ReorderPutResponse();

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ReorderPutResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(expectedResponse);

        var result = await builder.PutAsync(body);

        Assert.Equal(expectedResponse, result);
    }

    /// <summary>
    /// PutAsyncのリクエストボディの必須パラメータがnullの場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Theory]
    [InlineData("Orders")]
    [InlineData("PriorityRank")]
    [InlineData("RoutingID")]
    public async Task PutAsync_WithNullRequestBodyParameter_ThrowsValidationException(string invalidParameterName)
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new ReorderRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = CreateRequestBodyWithNullParameter(invalidParameterName);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal($"{invalidParameterName}が入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}が入力されていません。");
    }

    /// <summary>
    /// PutAsyncのリクエストボディのPriorityRankが範囲外の場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(101)]
    public async Task PutAsync_WithOutOfRangePriorityRank_ThrowsValidationException(int priorityRank)
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new ReorderRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PutCommonServiceItemRoutingReorderRequest
        {
            Orders = new List<PutCommonServiceItemRoutingReorderRequest_Orders>
            {
                new PutCommonServiceItemRoutingReorderRequest_Orders
                {
                    PriorityRank = priorityRank,
                    RoutingID = "123456789012"
                }
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal("PriorityRankの値は1以上100以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("PriorityRankの値は1以上100以下で入力してください。");
    }

    /// <summary>
    /// PutAsyncのリクエストボディのRoutingIDが不正な形式の場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Theory]
    [InlineData("RoutingID")]
    public async Task PutAsync_WithInvalidFormatRequestBodyParameter_ThrowsValidationException(string invalidParameterName)
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new ReorderRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PutCommonServiceItemRoutingReorderRequest
        {
            Orders = new List<PutCommonServiceItemRoutingReorderRequest_Orders>
            {
                new PutCommonServiceItemRoutingReorderRequest_Orders
                {
                    PriorityRank = 1,
                    RoutingID = "invalid-id"
                }
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal($"{invalidParameterName}の形式が正しくありません。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}の形式が正しくありません。");
    }

    /// <summary>
    /// PutAsyncでApiErrorがスローされた場合にそのまま再スローされることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithApiError_ThrowsApiError()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new ReorderRequestBuilder(pathParameters, requestAdapter);
        var body = CreateValidRequestBody();
        var expectedException = new ApiError();

        requestAdapter.SendAsync<ReorderPutResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ReorderPutResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<ReorderPutResponse?>(expectedException));

        await Assert.ThrowsAsync<ApiError>(() => builder.PutAsync(body));
    }

    /// <summary>
    /// PutAsyncでその他の例外がスローされた場合にそのまま再スローされることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithOtherException_ThrowsException()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new ReorderRequestBuilder(pathParameters, requestAdapter);
        var body = CreateValidRequestBody();
        var expectedException = new InvalidOperationException("test exception");

        requestAdapter.SendAsync<ReorderPutResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ReorderPutResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<ReorderPutResponse?>(expectedException));

        await Assert.ThrowsAsync<InvalidOperationException>(() => builder.PutAsync(body));
    }

    private static bool HasApiErrorMapping(Dictionary<string, ParsableFactory<IParsable>> errorMapping)
    {
        return errorMapping.TryGetValue("XXX", out var handler)
            && handler == ApiError.CreateFromDiscriminatorValue;
    }

    private static IRequestAdapter CreateRequestAdapter()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var serializationWriter = Substitute.For<ISerializationWriter>();
        serializationWriter.GetSerializedContent().Returns(new MemoryStream());
        var serializationWriterFactory = Substitute.For<ISerializationWriterFactory>();
        serializationWriterFactory.GetSerializationWriter(Arg.Any<string>()).Returns(serializationWriter);
        requestAdapter.SerializationWriterFactory.Returns(serializationWriterFactory);
        return requestAdapter;
    }

    private static PutCommonServiceItemRoutingReorderRequest CreateValidRequestBody()
    {
        return new PutCommonServiceItemRoutingReorderRequest
        {
            Orders = new List<PutCommonServiceItemRoutingReorderRequest_Orders>
            {
                new PutCommonServiceItemRoutingReorderRequest_Orders
                {
                    PriorityRank = 1,
                    RoutingID = "123456789012"
                }
            }
        };
    }

    private static PutCommonServiceItemRoutingReorderRequest CreateRequestBodyWithNullParameter(string invalidParameterName)
    {
        switch (invalidParameterName)
        {
            case "Orders":
                return new PutCommonServiceItemRoutingReorderRequest
                {
                    Orders = null
                };
            case "PriorityRank":
                return new PutCommonServiceItemRoutingReorderRequest
                {
                    Orders = new List<PutCommonServiceItemRoutingReorderRequest_Orders>
                    {
                        new PutCommonServiceItemRoutingReorderRequest_Orders
                        {
                            PriorityRank = null,
                            RoutingID = "123456789012"
                        }
                    }
                };
            case "RoutingID":
                return new PutCommonServiceItemRoutingReorderRequest
                {
                    Orders = new List<PutCommonServiceItemRoutingReorderRequest_Orders>
                    {
                        new PutCommonServiceItemRoutingReorderRequest_Orders
                        {
                            PriorityRank = 1,
                            RoutingID = null
                        }
                    }
                };
            default:
                throw new ArgumentOutOfRangeException(nameof(invalidParameterName), invalidParameterName, null);
        }
    }

    /// <summary>
    /// WithUrlが正しい型を返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new ReorderRequestBuilder(pathParameters, requestAdapter);

        var result = builder.WithUrl("https://api.example.com/commonserviceitem/simplenotification/routing/reorder");

        Assert.NotNull(result);
        Assert.IsType<ReorderRequestBuilder>(result);
    }
}
