using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.SimpleNotification.Commonserviceitem.Item.Simplenotification.Status;
using Sacloud.Sdk.SimpleNotification.Models;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.SimpleNotification.Tests.Commonserviceitem.Item.Simplenotification.Status;

/// <summary>
/// StatusRequestBuilderのテスト
/// </summary>
public class StatusRequestBuilderTests
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
            { "id", "123456789012" }
        };

        var builder = new StatusRequestBuilder(pathParameters, requestAdapter);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();

        var builder = new StatusRequestBuilder("https://api.example.com/commonserviceitem/123456789012/simplenotification/status", requestAdapter);

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
            { "id", "123456789012" }
        };
        var builder = new StatusRequestBuilder(pathParameters, requestAdapter);

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
            { "id", "123456789012" }
        };
        var builder = new StatusRequestBuilder(pathParameters, requestAdapter);
        var expectedResponse = new GetCommonServiceItemStatusResponse();

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<GetCommonServiceItemStatusResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(expectedResponse);

        var result = await builder.GetAsync();

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<GetCommonServiceItemStatusResponse>>(),
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
            { "id", "123456789012" }
        };
        var builder = new StatusRequestBuilder(pathParameters, requestAdapter);
        var expectedException = new ApiError();

        requestAdapter.SendAsync<GetCommonServiceItemStatusResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<GetCommonServiceItemStatusResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<GetCommonServiceItemStatusResponse?>(expectedException));

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
            { "id", "123456789012" }
        };
        var builder = new StatusRequestBuilder(pathParameters, requestAdapter);
        var expectedException = new InvalidOperationException("test exception");

        requestAdapter.SendAsync<GetCommonServiceItemStatusResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<GetCommonServiceItemStatusResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<GetCommonServiceItemStatusResponse?>(expectedException));

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
            { "id", "123456789012" }
        };
        var builder = new StatusRequestBuilder(pathParameters, requestAdapter);

        var result = builder.WithUrl("https://api.example.com/commonserviceitem/123456789012/simplenotification/status");

        Assert.NotNull(result);
        Assert.IsType<StatusRequestBuilder>(result);
    }

    /// <summary>
    /// GetAsyncが不正なid形式でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithInvalidId_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "123" }
        };
        var builder = new StatusRequestBuilder(pathParameters, requestAdapter);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync());

        Assert.Contains("通知先ID", exception.Message);
    }
}
