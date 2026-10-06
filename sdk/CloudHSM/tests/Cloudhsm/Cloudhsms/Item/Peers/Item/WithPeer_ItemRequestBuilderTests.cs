using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.CloudHSM.Cloudhsm.Cloudhsms.Item.Peers.Item;
using Sacloud.Sdk.Shared.Models;

namespace Sacloud.Sdk.CloudHSM.Tests.Cloudhsm.Cloudhsms.Item.Peers.Item;

/// <summary>
/// WithPeer_ItemRequestBuilderのテスト
/// </summary>
public class WithPeer_ItemRequestBuilderTests
{
    private readonly IRequestAdapter _mockRequestAdapter;
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger _mockLogger;

    public WithPeer_ItemRequestBuilderTests()
    {
        _mockRequestAdapter = Substitute.For<IRequestAdapter>();
        _mockRequestAdapter.BaseUrl = "https://api.example.com";
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger>();
        _mockLoggerFactory.CreateLogger(Arg.Any<string>()).Returns(_mockLogger);
    }

    /// <summary>
    /// コンストラクタ（PathParameters版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_InitializesSuccessfully()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "cloudhsm_resource_id", "test-cloudhsm-resource-id" },
            { "peer_id", "test-peer-id" },
        };

        var builder = new WithPeer_ItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（RawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_InitializesSuccessfully()
    {
        var rawUrl = "https://api.example.com/cloudhsm/cloudhsms/test-cloudhsm-resource-id/peers/test-peer-id";

        var builder = new WithPeer_ItemRequestBuilder(rawUrl, _mockRequestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// ToDeleteRequestInformationがDELETEメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToDeleteRequestInformation_CreatesRequestInformationWithDeleteMethod()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "cloudhsm_resource_id", "test-cloudhsm-resource-id" },
            { "peer_id", "test-peer-id" },
        };
        var builder = new WithPeer_ItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToDeleteRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.DELETE, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToDeleteRequestInformationがAcceptヘッダーを設定しないことを確認
    /// </summary>
    [Fact]
    public void ToDeleteRequestInformation_DoesNotSetAcceptHeader()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "cloudhsm_resource_id", "test-cloudhsm-resource-id" },
            { "peer_id", "test-peer-id" },
        };
        var builder = new WithPeer_ItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToDeleteRequestInformation();

        Assert.False(requestInfo.Headers.ContainsKey("Accept"));
    }

    /// <summary>
    /// ToDeleteRequestInformationが正しいPathParametersを設定することを確認
    /// </summary>
    [Fact]
    public void ToDeleteRequestInformation_SetsPathParameters()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "cloudhsm_resource_id", "test-cloudhsm-resource-id" },
            { "peer_id", "test-peer-id" },
        };
        var builder = new WithPeer_ItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToDeleteRequestInformation();

        Assert.True(requestInfo.PathParameters.ContainsKey("cloudhsm_resource_id"));
        Assert.Equal("test-cloudhsm-resource-id", requestInfo.PathParameters["cloudhsm_resource_id"]);
        Assert.True(requestInfo.PathParameters.ContainsKey("peer_id"));
        Assert.Equal("test-peer-id", requestInfo.PathParameters["peer_id"]);
    }

    /// <summary>
    /// DeleteAsync が期待されるSendNoContentAsyncを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_CallsRequestAdapterSendNoContentAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "cloudhsm_resource_id", "test-cloudhsm-resource-id" },
            { "peer_id", "test-peer-id" },
        };
        var builder = new WithPeer_ItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        await builder.DeleteAsync();

        await requestAdapter.Received(1).SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// WithUrlが新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewInstance()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "cloudhsm_resource_id", "test-cloudhsm-resource-id" },
            { "peer_id", "test-peer-id" },
        };
        var builder = new WithPeer_ItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);
        var newUrl = "https://newapi.example.com/cloudhsm/cloudhsms/new-cloudhsm-resource-%2Did/peers/new-peer-id";

        var newBuilder = builder.WithUrl(newUrl);

        Assert.NotNull(newBuilder);
        Assert.NotSame(builder, newBuilder);
        Assert.IsType<WithPeer_ItemRequestBuilder>(newBuilder);
    }

    /// <summary>
    /// コンストラクタ（PathParameters版）でLoggerFactoryがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParametersAndNullLoggerFactory_ThrowsArgumentNullException()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "cloudhsm_resource_id", "test-cloudhsm-resource-%2Did" },
            { "peer_id", "test-peer-id" },
        };

        Assert.Throws<ArgumentNullException>(() => new WithPeer_ItemRequestBuilder(pathParameters, _mockRequestAdapter, null!));
    }

    /// <summary>
    /// コンストラクタ（RawUrl版）でLoggerFactoryがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrlAndNullLoggerFactory_ThrowsArgumentNullException()
    {
        var rawUrl = "https://api.example.com/cloudhsm/cloudhsms/test-cloudhsm-resource-%2Did/peers/test-peer-id";

        Assert.Throws<ArgumentNullException>(() => new WithPeer_ItemRequestBuilder(rawUrl, _mockRequestAdapter, null!));
    }

    /// <summary>
    /// DeleteAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_WithApiError_ThrowsApiException()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "cloudhsm_resource_id", "test-hsm-id" },
            { "peer_id", "test-peer-id" }
        };
        var builder = new WithPeer_ItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError
        {
            ResponseStatusCode = 404
        };
        ((IAdditionalDataHolder)expectedException).AdditionalData = new Dictionary<string, object>
        {
            { "error_code", "not_found" },
            { "error_msg", "peer not found" },
            { "is_fatal", false },
            { "serial", "1234567890" },
            { "status", "404 Not Found" }
        };

        _mockRequestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(async () => await builder.DeleteAsync());

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^CloudHSMピア削除のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 404\], \[error_code, not_found\], \[error_msg, peer not found\], \[is_fatal, False\], \[serial, 1234567890\], \[status, 404 Not Found\]$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// DeleteAsyncで別のエラーが発生した場合に例外をスローし、エラーログが出力されることを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_WithOtherError_ThrowsExceptionAndLogsError()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "cloudhsm_resource_id", "test-hsm-id" },
            { "peer_id", "test-peer-id" }
        };
        var builder = new WithPeer_ItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);
        var exceptionMessage = "Internal Server Error";

        _mockRequestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .ThrowsAsync(new Exception(exceptionMessage));

        await Assert.ThrowsAsync<Exception>(async () => await builder.DeleteAsync());

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^CloudHSMピアの削除中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }
}
