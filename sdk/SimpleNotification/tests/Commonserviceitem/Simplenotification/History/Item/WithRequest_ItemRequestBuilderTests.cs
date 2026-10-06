using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.SimpleNotification.Commonserviceitem.Simplenotification.History.Item;
using Sacloud.Sdk.SimpleNotification.Models;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.SimpleNotification.Tests.Commonserviceitem.Simplenotification.History.Item;

/// <summary>
/// WithRequest_ItemRequestBuilderのテスト
/// </summary>
public class WithRequest_ItemRequestBuilderTests
{
    /// <summary>
    /// コンストラクタ（PathParameters版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "request_id", "12345" }
        };

        var builder = new WithRequest_ItemRequestBuilder(pathParameters, requestAdapter);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();

        var builder = new WithRequest_ItemRequestBuilder("https://api.example.com/commonserviceitem/simplenotification/history/12345", requestAdapter);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// ToGetRequestInformationが正しいRequestInformationを生成することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_ReturnsCorrectRequestInformation()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "request_id", "12345" }
        };
        var builder = new WithRequest_ItemRequestBuilder(pathParameters, requestAdapter);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.GET, requestInfo.HttpMethod);
    }

    /// <summary>
    /// GetAsyncがRequestAdapter.SendAsyncを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_CallsRequestAdapterSendAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "request_id", "12345" }
        };
        var builder = new WithRequest_ItemRequestBuilder(pathParameters, requestAdapter);
        var expectedResponse = new GetSimpleNotificationHistoryResponse();

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<GetSimpleNotificationHistoryResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(expectedResponse);

        var result = await builder.GetAsync();

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<GetSimpleNotificationHistoryResponse>>(),
            Arg.Is<Dictionary<string, ParsableFactory<IParsable>>>(x => HasApiErrorMapping(x)),
            Arg.Any<CancellationToken>());
        Assert.Equal(expectedResponse, result);
    }

    /// <summary>
    /// GetAsyncでApiErrorがスローされた場合にそのまま再スローされることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithApiError_ThrowsApiError()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "request_id", "12345" }
        };
        var builder = new WithRequest_ItemRequestBuilder(pathParameters, requestAdapter);
        var expectedException = new ApiError();

        requestAdapter.SendAsync<GetSimpleNotificationHistoryResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<GetSimpleNotificationHistoryResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<GetSimpleNotificationHistoryResponse?>(expectedException));

        await Assert.ThrowsAsync<ApiError>(() => builder.GetAsync());
    }

    /// <summary>
    /// GetAsyncでその他の例外がスローされた場合にそのまま再スローされることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithOtherException_ThrowsException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "request_id", "12345" }
        };
        var builder = new WithRequest_ItemRequestBuilder(pathParameters, requestAdapter);
        var expectedException = new InvalidOperationException("test exception");

        requestAdapter.SendAsync<GetSimpleNotificationHistoryResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<GetSimpleNotificationHistoryResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<GetSimpleNotificationHistoryResponse?>(expectedException));

        await Assert.ThrowsAsync<InvalidOperationException>(() => builder.GetAsync());
    }

    private static bool HasApiErrorMapping(Dictionary<string, ParsableFactory<IParsable>> errorMapping)
    {
        return errorMapping.TryGetValue("XXX", out var handler)
            && handler == ApiError.CreateFromDiscriminatorValue;
    }

    /// <summary>
    /// WithUrlが正しい型を返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "request_id", "12345" }
        };
        var builder = new WithRequest_ItemRequestBuilder(pathParameters, requestAdapter);

        var result = builder.WithUrl("https://api.example.com/commonserviceitem/simplenotification/history/12345");

        Assert.NotNull(result);
        Assert.IsType<WithRequest_ItemRequestBuilder>(result);
    }

    /// <summary>
    /// GetAsyncが不正なrequest_id形式でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithInvalidRequestId_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "request_id", "abc" }
        };
        var builder = new WithRequest_ItemRequestBuilder(pathParameters, requestAdapter);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync());

        Assert.Contains("リクエストID", exception.Message);
    }
}
