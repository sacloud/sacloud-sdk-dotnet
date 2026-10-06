using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using System.ComponentModel.DataAnnotations;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.CloudHSM.Cloudhsm.Cloudhsms.Item.Clients;
using Sacloud.Sdk.CloudHSM.Cloudhsm.Cloudhsms.Item.Peers;
using Sacloud.Sdk.CloudHSM.Cloudhsm.Cloudhsms.Item;
using Sacloud.Sdk.CloudHSM.Models;
using Sacloud.Sdk.Shared.Models;

namespace Sacloud.Sdk.CloudHSM.Tests.Cloudhsm.Cloudhsms.Item;

/// <summary>
/// Cloudhsm_resource_ItemRequestBuilderのテスト
/// </summary>
public class Cloudhsm_resource_ItemRequestBuilderTests
{
    private readonly IRequestAdapter _mockRequestAdapter;
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger _mockLogger;

    public Cloudhsm_resource_ItemRequestBuilderTests()
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
        };

        var builder = new Cloudhsm_resource_ItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（RawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_InitializesSuccessfully()
    {
        var rawUrl = "https://api.example.com/cloudhsm/cloudhsms/test-cloudhsm-resource-id";

        var builder = new Cloudhsm_resource_ItemRequestBuilder(rawUrl, _mockRequestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// Clientsプロパティが正しくClientsRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Clients_ReturnsClientsRequestBuilder()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "cloudhsm_resource_id", "test-cloudhsm-resource-id" },
        };
        var builder = new Cloudhsm_resource_ItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var result = builder.Clients;

        Assert.NotNull(result);
        Assert.IsType<ClientsRequestBuilder>(result);
    }

    /// <summary>
    /// Peersプロパティが正しくPeersRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Peers_ReturnsPeersRequestBuilder()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "cloudhsm_resource_id", "test-cloudhsm-resource-id" },
        };
        var builder = new Cloudhsm_resource_ItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var result = builder.Peers;

        Assert.NotNull(result);
        Assert.IsType<PeersRequestBuilder>(result);
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
        };
        var builder = new Cloudhsm_resource_ItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

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
        };
        var builder = new Cloudhsm_resource_ItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

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
        };
        var builder = new Cloudhsm_resource_ItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToDeleteRequestInformation();

        Assert.True(requestInfo.PathParameters.ContainsKey("cloudhsm_resource_id"));
        Assert.Equal("test-cloudhsm-resource-id", requestInfo.PathParameters["cloudhsm_resource_id"]);
    }

    /// <summary>
    /// ToGetRequestInformationがGETメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_CreatesRequestInformationWithGetMethod()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "cloudhsm_resource_id", "test-cloudhsm-resource-id" },
        };
        var builder = new Cloudhsm_resource_ItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.GET, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToGetRequestInformationが正しいAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_SetsAcceptHeader()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "cloudhsm_resource_id", "test-cloudhsm-resource-id" },
        };
        var builder = new Cloudhsm_resource_ItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// ToGetRequestInformationが正しいPathParametersを設定することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_SetsPathParameters()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "cloudhsm_resource_id", "test-cloudhsm-resource-id" },
        };
        var builder = new Cloudhsm_resource_ItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.True(requestInfo.PathParameters.ContainsKey("cloudhsm_resource_id"));
        Assert.Equal("test-cloudhsm-resource-id", requestInfo.PathParameters["cloudhsm_resource_id"]);
    }

    /// <summary>
    /// ToPutRequestInformationがPUTメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_CreatesRequestInformationWithPutMethod()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "cloudhsm_resource_id", "test-cloudhsm-resource-id" },
        };
        var builder = new Cloudhsm_resource_ItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToPutRequestInformation(new WrappedCloudHSM());

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.PUT, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToPutRequestInformationが正しいAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_SetsAcceptHeader()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "cloudhsm_resource_id", "test-cloudhsm-resource-id" },
        };
        var builder = new Cloudhsm_resource_ItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToPutRequestInformation(new WrappedCloudHSM());

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// ToPutRequestInformationが正しいContent-Typeヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_SetsContentTypeHeader()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "cloudhsm_resource_id", "test-cloudhsm-resource-id" },
        };
        var builder = new Cloudhsm_resource_ItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToPutRequestInformation(new WrappedCloudHSM());

        Assert.True(requestInfo.Headers.ContainsKey("Content-Type"));
        Assert.Contains("application/json", requestInfo.Headers["Content-Type"]);
    }

    /// <summary>
    /// ToPutRequestInformationが正しいPathParametersを設定することを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_SetsPathParameters()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "cloudhsm_resource_id", "test-cloudhsm-resource-id" },
        };
        var builder = new Cloudhsm_resource_ItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToPutRequestInformation(new WrappedCloudHSM());

        Assert.True(requestInfo.PathParameters.ContainsKey("cloudhsm_resource_id"));
        Assert.Equal("test-cloudhsm-resource-id", requestInfo.PathParameters["cloudhsm_resource_id"]);
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
        };
        var builder = new Cloudhsm_resource_ItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
    /// GetAsync が期待されるSendAsyncを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_CallsRequestAdapterSendAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "cloudhsm_resource_id", "test-cloudhsm-resource-id" },
        };
        var builder = new Cloudhsm_resource_ItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var mockResponse = new WrappedCloudHSM();
        requestAdapter.SendAsync<WrappedCloudHSM>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedCloudHSM>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(mockResponse);

        var result = await builder.GetAsync();

        Assert.NotNull(result);
        Assert.IsType<WrappedCloudHSM>(result);

        await requestAdapter.Received(1).SendAsync<WrappedCloudHSM>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedCloudHSM>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// PutAsync が期待されるSendAsyncを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_CallsRequestAdapterSendAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "cloudhsm_resource_id", "test-cloudhsm-resource-id" },
        };
        var builder = new Cloudhsm_resource_ItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var mockResponse = new WrappedCloudHSM();
        requestAdapter.SendAsync<WrappedCloudHSM>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedCloudHSM>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(mockResponse);

        var result = await builder.PutAsync(new WrappedCloudHSM
        {
            CloudHSM = new Sacloud.Sdk.CloudHSM.Models.CloudHSM
            {
                IPv4NetworkAddress = "192.168.0.0",
                IPv4PrefixLength = 24,
                Name = "test-cloudhsm"
            }
        });

        Assert.NotNull(result);
        Assert.IsType<WrappedCloudHSM>(result);

        await requestAdapter.Received(1).SendAsync<WrappedCloudHSM>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedCloudHSM>>(),
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
        };
        var builder = new Cloudhsm_resource_ItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);
        var newUrl = "https://newapi.example.com/cloudhsm/cloudhsms/new-cloudhsm-resource-%2Did";

        var newBuilder = builder.WithUrl(newUrl);

        Assert.NotNull(newBuilder);
        Assert.NotSame(builder, newBuilder);
        Assert.IsType<Cloudhsm_resource_ItemRequestBuilder>(newBuilder);
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
        };

        Assert.Throws<ArgumentNullException>(() => new Cloudhsm_resource_ItemRequestBuilder(pathParameters, _mockRequestAdapter, null!));
    }

    /// <summary>
    /// コンストラクタ（RawUrl版）でLoggerFactoryがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrlAndNullLoggerFactory_ThrowsArgumentNullException()
    {
        var rawUrl = "https://api.example.com/cloudhsm/cloudhsms/test-cloudhsm-resource-%2Did";

        Assert.Throws<ArgumentNullException>(() => new Cloudhsm_resource_ItemRequestBuilder(rawUrl, _mockRequestAdapter, null!));
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
            { "cloudhsm_resource_id", "test-id" }
        };
        var builder = new Cloudhsm_resource_ItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError
        {
            ResponseStatusCode = 404
        };
        ((IAdditionalDataHolder)expectedException).AdditionalData = new Dictionary<string, object>
        {
            { "error_code", "not_found" },
            { "error_msg", "resource not found" },
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
                    @"^CloudHSM削除のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 404\], \[error_code, not_found\], \[error_msg, resource not found\], \[is_fatal, False\], \[serial, 1234567890\], \[status, 404 Not Found\]$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithApiError_ThrowsApiException()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "cloudhsm_resource_id", "test-id" }
        };
        var builder = new Cloudhsm_resource_ItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError
        {
            ResponseStatusCode = 401
        };
        ((IAdditionalDataHolder)expectedException).AdditionalData = new Dictionary<string, object>
        {
            { "error_code", "unauthorized" },
            { "error_msg", "error-unauthorized" },
            { "is_fatal", true },
            { "serial", "1234567890" },
            { "status", "401 Unauthorized" }
        };

        _mockRequestAdapter.SendAsync<WrappedCloudHSM>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedCloudHSM>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(async () => await builder.GetAsync());

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^CloudHSM詳細取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PutAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithApiError_ThrowsApiException()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "cloudhsm_resource_id", "test-id" }
        };
        var builder = new Cloudhsm_resource_ItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);
        var body = new WrappedCloudHSM
        {
            CloudHSM = new Sacloud.Sdk.CloudHSM.Models.CloudHSM
            {
                IPv4NetworkAddress = "192.168.0.0",
                IPv4PrefixLength = 24,
                Name = "test-name"
            }
        };
        var expectedException = new ApiError
        {
            ResponseStatusCode = 400
        };
        ((IAdditionalDataHolder)expectedException).AdditionalData = new Dictionary<string, object>
        {
            { "error_code", "bad_request" },
            { "error_msg", "invalid request" },
            { "is_fatal", false },
            { "serial", "9876543210" },
            { "status", "400 Bad Request" }
        };

        _mockRequestAdapter.SendAsync<WrappedCloudHSM>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedCloudHSM>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(async () => await builder.PutAsync(body));

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^CloudHSM更新のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 400\], \[error_code, bad_request\], \[error_msg, invalid request\], \[is_fatal, False\], \[serial, 9876543210\], \[status, 400 Bad Request\]$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PutAsyncのリクエストボディに必須プロパティがない場合にバリデーションエラーと警告ログが出力されることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithMissingCloudHSM_ThrowsValidationExceptionAndLogsWarning()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "cloudhsm_resource_id", "test-cloudhsm-resource-id" },
        };
        var builder = new Cloudhsm_resource_ItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(new WrappedCloudHSM()));

        Assert.Equal("CloudHSMが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("CloudHSMが入力されていません。");
        await _mockRequestAdapter.DidNotReceive().SendAsync<WrappedCloudHSM>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedCloudHSM>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteAsync_Success_LogsInformation()
    {
        var builder = new Cloudhsm_resource_ItemRequestBuilder(new Dictionary<string, object> { { "baseurl", "https://api.example.com" }, { "cloudhsm_resource_id", "test-id" } }, _mockRequestAdapter, _mockLoggerFactory);
        _mockRequestAdapter.SendNoContentAsync(Arg.Any<RequestInformation>(), Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        await builder.DeleteAsync();

        _mockLogger.Received(1).Log(LogLevel.Information, Arg.Any<EventId>(), Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^CloudHSMの削除を開始します。 \d{4}-\d{2}-\d{2}T.*Z$")), null, Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(LogLevel.Information, Arg.Any<EventId>(), Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^CloudHSMの削除が完了しました。 \d{4}-\d{2}-\d{2}T.*Z \[StatusCode, 204\]$")), null, Arg.Any<Func<object, Exception?, string>>());
    }

    [Fact]
    public async Task DeleteAsync_WithOtherError_ThrowsExceptionAndLogsError()
    {
        var builder = new Cloudhsm_resource_ItemRequestBuilder(new Dictionary<string, object> { { "baseurl", "https://api.example.com" }, { "cloudhsm_resource_id", "test-id" } }, _mockRequestAdapter, _mockLoggerFactory);
        _mockRequestAdapter.SendNoContentAsync(Arg.Any<RequestInformation>(), Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(), Arg.Any<CancellationToken>()).ThrowsAsync(new Exception());

        await Assert.ThrowsAsync<Exception>(() => builder.DeleteAsync());

        _mockLogger.Received(1).Log(LogLevel.Error, Arg.Any<EventId>(), Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^CloudHSMの削除中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T.*Z$")), Arg.Any<Exception>(), Arg.Any<Func<object, Exception?, string>>());
    }

    [Fact]
    public async Task GetAsync_Success_LogsInformation()
    {
        var builder = new Cloudhsm_resource_ItemRequestBuilder(new Dictionary<string, object> { { "baseurl", "https://api.example.com" }, { "cloudhsm_resource_id", "test-id" } }, _mockRequestAdapter, _mockLoggerFactory);
        _mockRequestAdapter.SendAsync<WrappedCloudHSM>(Arg.Any<RequestInformation>(), Arg.Any<ParsableFactory<WrappedCloudHSM>>(), Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(), Arg.Any<CancellationToken>()).Returns(new WrappedCloudHSM());

        await builder.GetAsync();

        _mockLogger.Received(1).Log(LogLevel.Information, Arg.Any<EventId>(), Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^CloudHSM詳細の取得を開始します。 \d{4}-\d{2}-\d{2}T.*Z$")), null, Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(LogLevel.Information, Arg.Any<EventId>(), Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^CloudHSM詳細の取得が完了しました。 \d{4}-\d{2}-\d{2}T.*Z \[StatusCode, 200\]$")), null, Arg.Any<Func<object, Exception?, string>>());
    }

    [Fact]
    public async Task GetAsync_WithOtherError_ThrowsExceptionAndLogsError()
    {
        var builder = new Cloudhsm_resource_ItemRequestBuilder(new Dictionary<string, object> { { "baseurl", "https://api.example.com" }, { "cloudhsm_resource_id", "test-id" } }, _mockRequestAdapter, _mockLoggerFactory);
        _mockRequestAdapter.SendAsync<WrappedCloudHSM>(Arg.Any<RequestInformation>(), Arg.Any<ParsableFactory<WrappedCloudHSM>>(), Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(), Arg.Any<CancellationToken>()).ThrowsAsync(new Exception());

        await Assert.ThrowsAsync<Exception>(() => builder.GetAsync());

        _mockLogger.Received(1).Log(LogLevel.Error, Arg.Any<EventId>(), Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^CloudHSM詳細の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T.*Z$")), Arg.Any<Exception>(), Arg.Any<Func<object, Exception?, string>>());
    }

    [Fact]
    public async Task PutAsync_Success_LogsInformation()
    {
        var builder = new Cloudhsm_resource_ItemRequestBuilder(new Dictionary<string, object> { { "baseurl", "https://api.example.com" }, { "cloudhsm_resource_id", "test-id" } }, _mockRequestAdapter, _mockLoggerFactory);
        var body = new WrappedCloudHSM { CloudHSM = new Sacloud.Sdk.CloudHSM.Models.CloudHSM { IPv4NetworkAddress = "192.168.0.0", IPv4PrefixLength = 24, Name = "test" } };
        _mockRequestAdapter.SendAsync<WrappedCloudHSM>(Arg.Any<RequestInformation>(), Arg.Any<ParsableFactory<WrappedCloudHSM>>(), Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(), Arg.Any<CancellationToken>()).Returns(new WrappedCloudHSM());

        await builder.PutAsync(body);

        _mockLogger.Received(1).Log(LogLevel.Information, Arg.Any<EventId>(), Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^CloudHSMの更新を開始します。 \d{4}-\d{2}-\d{2}T.*Z$")), null, Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(LogLevel.Information, Arg.Any<EventId>(), Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^CloudHSMの更新が完了しました。 \d{4}-\d{2}-\d{2}T.*Z \[StatusCode, 200\]$")), null, Arg.Any<Func<object, Exception?, string>>());
    }

    [Fact]
    public async Task PutAsync_WithOtherError_ThrowsExceptionAndLogsError()
    {
        var builder = new Cloudhsm_resource_ItemRequestBuilder(new Dictionary<string, object> { { "baseurl", "https://api.example.com" }, { "cloudhsm_resource_id", "test-id" } }, _mockRequestAdapter, _mockLoggerFactory);
        var body = new WrappedCloudHSM { CloudHSM = new Sacloud.Sdk.CloudHSM.Models.CloudHSM { IPv4NetworkAddress = "192.168.0.0", IPv4PrefixLength = 24, Name = "test" } };
        _mockRequestAdapter.SendAsync<WrappedCloudHSM>(Arg.Any<RequestInformation>(), Arg.Any<ParsableFactory<WrappedCloudHSM>>(), Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(), Arg.Any<CancellationToken>()).ThrowsAsync(new Exception());

        await Assert.ThrowsAsync<Exception>(() => builder.PutAsync(body));

        _mockLogger.Received(1).Log(LogLevel.Error, Arg.Any<EventId>(), Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^CloudHSMの更新中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T.*Z$")), Arg.Any<Exception>(), Arg.Any<Func<object, Exception?, string>>());
    }
}
