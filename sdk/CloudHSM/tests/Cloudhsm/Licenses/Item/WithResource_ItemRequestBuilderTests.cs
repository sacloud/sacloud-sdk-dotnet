using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using System.ComponentModel.DataAnnotations;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.CloudHSM.Cloudhsm.Licenses.Item;
using Sacloud.Sdk.CloudHSM.Models;
using Sacloud.Sdk.Shared.Models;

namespace Sacloud.Sdk.CloudHSM.Tests.Cloudhsm.Licenses.Item;

/// <summary>
/// WithResource_ItemRequestBuilderのテスト
/// </summary>
public class WithResource_ItemRequestBuilderTests
{
    private readonly IRequestAdapter _mockRequestAdapter;
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger _mockLogger;

    public WithResource_ItemRequestBuilderTests()
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
            { "resource_id", "test-resource-id" },
        };

        var builder = new WithResource_ItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（RawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_InitializesSuccessfully()
    {
        var rawUrl = "https://api.example.com/cloudhsm/licenses/test-resource-id";

        var builder = new WithResource_ItemRequestBuilder(rawUrl, _mockRequestAdapter, _mockLoggerFactory);

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
            { "resource_id", "test-resource-id" },
        };
        var builder = new WithResource_ItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

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
            { "resource_id", "test-resource-id" },
        };
        var builder = new WithResource_ItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

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
            { "resource_id", "test-resource-id" },
        };
        var builder = new WithResource_ItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToDeleteRequestInformation();

        Assert.True(requestInfo.PathParameters.ContainsKey("resource_id"));
        Assert.Equal("test-resource-id", requestInfo.PathParameters["resource_id"]);
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
            { "resource_id", "test-resource-id" },
        };
        var builder = new WithResource_ItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

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
            { "resource_id", "test-resource-id" },
        };
        var builder = new WithResource_ItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

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
            { "resource_id", "test-resource-id" },
        };
        var builder = new WithResource_ItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.True(requestInfo.PathParameters.ContainsKey("resource_id"));
        Assert.Equal("test-resource-id", requestInfo.PathParameters["resource_id"]);
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
            { "resource_id", "test-resource-id" },
        };
        var builder = new WithResource_ItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToPutRequestInformation(new WrappedCloudHSMSoftwareLicense());

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
            { "resource_id", "test-resource-id" },
        };
        var builder = new WithResource_ItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToPutRequestInformation(new WrappedCloudHSMSoftwareLicense());

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
            { "resource_id", "test-resource-id" },
        };
        var builder = new WithResource_ItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToPutRequestInformation(new WrappedCloudHSMSoftwareLicense());

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
            { "resource_id", "test-resource-id" },
        };
        var builder = new WithResource_ItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToPutRequestInformation(new WrappedCloudHSMSoftwareLicense());

        Assert.True(requestInfo.PathParameters.ContainsKey("resource_id"));
        Assert.Equal("test-resource-id", requestInfo.PathParameters["resource_id"]);
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
            { "resource_id", "test-resource-id" },
        };
        var builder = new WithResource_ItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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

        _mockLogger.Received(1).Log(LogLevel.Information, Arg.Any<EventId>(), Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^CloudHSMライセンスの削除を開始します。 \d{4}-\d{2}-\d{2}T.*Z$")), null, Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(LogLevel.Information, Arg.Any<EventId>(), Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^CloudHSMライセンスの削除が完了しました。 \d{4}-\d{2}-\d{2}T.*Z \[StatusCode, 204\]$")), null, Arg.Any<Func<object, Exception?, string>>());
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
            { "resource_id", "test-resource-id" },
        };
        var builder = new WithResource_ItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var mockResponse = new WrappedCloudHSMSoftwareLicense();
        requestAdapter.SendAsync<WrappedCloudHSMSoftwareLicense>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedCloudHSMSoftwareLicense>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(mockResponse);

        var result = await builder.GetAsync();

        Assert.NotNull(result);
        Assert.IsType<WrappedCloudHSMSoftwareLicense>(result);

        await requestAdapter.Received(1).SendAsync<WrappedCloudHSMSoftwareLicense>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedCloudHSMSoftwareLicense>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());

        _mockLogger.Received(1).Log(LogLevel.Information, Arg.Any<EventId>(), Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^CloudHSMライセンス詳細の取得を開始します。 \d{4}-\d{2}-\d{2}T.*Z$")), null, Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(LogLevel.Information, Arg.Any<EventId>(), Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^CloudHSMライセンス詳細の取得が完了しました。 \d{4}-\d{2}-\d{2}T.*Z \[StatusCode, 200\]$")), null, Arg.Any<Func<object, Exception?, string>>());
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
            { "resource_id", "test-resource-id" },
        };
        var builder = new WithResource_ItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var mockResponse = new WrappedCloudHSMSoftwareLicense();
        requestAdapter.SendAsync<WrappedCloudHSMSoftwareLicense>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedCloudHSMSoftwareLicense>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(mockResponse);

        var result = await builder.PutAsync(new WrappedCloudHSMSoftwareLicense
        {
            CloudHSM = new CloudHSMSoftwareLicense
            {
                Description = "test-description",
                Name = "test-license",
                Tags = new List<string> { "tag1" }
            }
        });

        Assert.NotNull(result);
        Assert.IsType<WrappedCloudHSMSoftwareLicense>(result);

        await requestAdapter.Received(1).SendAsync<WrappedCloudHSMSoftwareLicense>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedCloudHSMSoftwareLicense>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());

        _mockLogger.Received(1).Log(LogLevel.Information, Arg.Any<EventId>(), Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^CloudHSMライセンスの更新を開始します。 \d{4}-\d{2}-\d{2}T.*Z$")), null, Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(LogLevel.Information, Arg.Any<EventId>(), Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^CloudHSMライセンスの更新が完了しました。 \d{4}-\d{2}-\d{2}T.*Z \[StatusCode, 200\]$")), null, Arg.Any<Func<object, Exception?, string>>());
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
            { "resource_id", "test-resource-id" },
        };
        var builder = new WithResource_ItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);
        var newUrl = "https://newapi.example.com/cloudhsm/licenses/new-resource-id";

        var newBuilder = builder.WithUrl(newUrl);

        Assert.NotNull(newBuilder);
        Assert.NotSame(builder, newBuilder);
        Assert.IsType<WithResource_ItemRequestBuilder>(newBuilder);
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
            { "resource_id", "test-resource-id" },
        };

        Assert.Throws<ArgumentNullException>(() => new WithResource_ItemRequestBuilder(pathParameters, _mockRequestAdapter, null!));
    }

    /// <summary>
    /// コンストラクタ（RawUrl版）でLoggerFactoryがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrlAndNullLoggerFactory_ThrowsArgumentNullException()
    {
        var rawUrl = "https://api.example.com/cloudhsm/licenses/test-resource-id";

        Assert.Throws<ArgumentNullException>(() => new WithResource_ItemRequestBuilder(rawUrl, _mockRequestAdapter, null!));
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
            { "resource_id", "test-id" }
        };
        var builder = new WithResource_ItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);
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

        _mockRequestAdapter.SendAsync<WrappedCloudHSMSoftwareLicense>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedCloudHSMSoftwareLicense>>(),
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
                    @"^CloudHSMライセンス詳細取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// DeleteAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_WithApiError_ThrowsApiException()
    {
        var builder = new WithResource_ItemRequestBuilder(new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }, { "resource_id", "test-resource-id" }
        }, _mockRequestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError { ResponseStatusCode = 404 };
        ((IAdditionalDataHolder)expectedException).AdditionalData = new Dictionary<string, object>
        {
            { "error_code", "not_found" }, { "error_msg", "license not found" }, { "is_fatal", false }, { "serial", "1234567890" }, { "status", "404 Not Found" }
        };
        _mockRequestAdapter.SendNoContentAsync(Arg.Any<RequestInformation>(), Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(), Arg.Any<CancellationToken>()).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(() => builder.DeleteAsync());

        _mockLogger.Received(1).Log(LogLevel.Error, Arg.Any<EventId>(), Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^CloudHSMライセンス削除のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 404\], \[error_code, not_found\], \[error_msg, license not found\], \[is_fatal, False\], \[serial, 1234567890\], \[status, 404 Not Found\]$")), Arg.Any<ApiException>(), Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PutAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithApiError_ThrowsApiException()
    {
        var builder = new WithResource_ItemRequestBuilder(new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }, { "resource_id", "test-resource-id" }
        }, _mockRequestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError { ResponseStatusCode = 400 };
        ((IAdditionalDataHolder)expectedException).AdditionalData = new Dictionary<string, object>
        {
            { "error_code", "bad_request" }, { "error_msg", "invalid license" }, { "is_fatal", false }, { "serial", "9876543210" }, { "status", "400 Bad Request" }
        };
        _mockRequestAdapter.SendAsync<WrappedCloudHSMSoftwareLicense>(Arg.Any<RequestInformation>(), Arg.Any<ParsableFactory<WrappedCloudHSMSoftwareLicense>>(), Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(), Arg.Any<CancellationToken>()).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(() => builder.PutAsync(new WrappedCloudHSMSoftwareLicense { CloudHSM = new CloudHSMSoftwareLicense { Description = "test-description", Name = "test-license", Tags = new List<string> { "tag1" } } }));

        _mockLogger.Received(1).Log(LogLevel.Error, Arg.Any<EventId>(), Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^CloudHSMライセンス更新のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 400\], \[error_code, bad_request\], \[error_msg, invalid license\], \[is_fatal, False\], \[serial, 9876543210\], \[status, 400 Bad Request\]$")), Arg.Any<ApiException>(), Arg.Any<Func<object, Exception?, string>>());
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
            { "resource_id", "test-resource-id" },
        };
        var builder = new WithResource_ItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(new WrappedCloudHSMSoftwareLicense()));

        Assert.Equal("CloudHSMが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("CloudHSMが入力されていません。");
        await _mockRequestAdapter.DidNotReceive().SendAsync<WrappedCloudHSMSoftwareLicense>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedCloudHSMSoftwareLicense>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// DeleteAsync が他のエラーで失敗した場合に例外をスローし、エラーログを出力することを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_WithOtherError_ThrowsExceptionAndLogsError()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "resource_id", "test-resource-id" },
        };
        var builder = new WithResource_ItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        requestAdapter.SendNoContentAsync(Arg.Any<RequestInformation>(), Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(), Arg.Any<CancellationToken>()).ThrowsAsync(new Exception());

        await Assert.ThrowsAsync<Exception>(() => builder.DeleteAsync());

        _mockLogger.Received(1).Log(LogLevel.Error, Arg.Any<EventId>(), Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^CloudHSMライセンスの削除中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T.*Z$")), Arg.Any<Exception>(), Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsync が他のエラーで失敗した場合に例外をスローし、エラーログを出力することを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithOtherError_ThrowsExceptionAndLogsError()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "resource_id", "test-resource-id" },
        };
        var builder = new WithResource_ItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        requestAdapter.SendAsync<WrappedCloudHSMSoftwareLicense>(Arg.Any<RequestInformation>(), Arg.Any<ParsableFactory<WrappedCloudHSMSoftwareLicense>>(), Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(), Arg.Any<CancellationToken>()).ThrowsAsync(new Exception());

        await Assert.ThrowsAsync<Exception>(() => builder.GetAsync());

        _mockLogger.Received(1).Log(LogLevel.Error, Arg.Any<EventId>(), Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^CloudHSMライセンス詳細の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T.*Z$")), Arg.Any<Exception>(), Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PutAsync が他のエラーで失敗した場合に例外をスローし、エラーログを出力することを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithOtherError_ThrowsExceptionAndLogsError()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "resource_id", "test-resource-id" },
        };
        var builder = new WithResource_ItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        requestAdapter.SendAsync<WrappedCloudHSMSoftwareLicense>(Arg.Any<RequestInformation>(), Arg.Any<ParsableFactory<WrappedCloudHSMSoftwareLicense>>(), Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(), Arg.Any<CancellationToken>()).ThrowsAsync(new Exception());

        await Assert.ThrowsAsync<Exception>(async () => await builder.PutAsync(new WrappedCloudHSMSoftwareLicense
        {
            CloudHSM = new CloudHSMSoftwareLicense
            {
                Description = "test-description",
                Name = "test-license",
                Tags = new List<string> { "tag1" }
            }
        }));

        _mockLogger.Received(1).Log(LogLevel.Error, Arg.Any<EventId>(), Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^CloudHSMライセンスの更新中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T.*Z$")), Arg.Any<Exception>(), Arg.Any<Func<object, Exception?, string>>());
    }
}
