using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using System.ComponentModel.DataAnnotations;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.CloudHSM.Cloudhsm.Licenses.Item;
using Sacloud.Sdk.CloudHSM.Cloudhsm.Licenses;
using Sacloud.Sdk.CloudHSM.Models;
using Sacloud.Sdk.Shared.Models;

namespace Sacloud.Sdk.CloudHSM.Tests.Cloudhsm.Licenses;

/// <summary>
/// LicensesRequestBuilderのテスト
/// </summary>
public class LicensesRequestBuilderTests
{
    private readonly IRequestAdapter _mockRequestAdapter;
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger _mockLogger;

    public LicensesRequestBuilderTests()
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
        };

        var builder = new LicensesRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（RawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_InitializesSuccessfully()
    {
        var rawUrl = "https://api.example.com/cloudhsm/licenses";

        var builder = new LicensesRequestBuilder(rawUrl, _mockRequestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// インデクサーが正しいWithResource_ItemRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_ReturnsWithResource_ItemRequestBuilder()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new LicensesRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var itemBuilder = builder["test-resource-id"];

        Assert.NotNull(itemBuilder);
        Assert.IsType<WithResource_ItemRequestBuilder>(itemBuilder);
    }

    /// <summary>
    /// インデクサーが毎回新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_ReturnsNewInstance_OnEachCall()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new LicensesRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var itemBuilder1 = builder["test-resource-id"];
        var itemBuilder2 = builder["test-resource-id"];

        Assert.NotSame(itemBuilder1, itemBuilder2);
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
        };
        var builder = new LicensesRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

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
        };
        var builder = new LicensesRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// ToPostRequestInformationがPOSTメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_CreatesRequestInformationWithPostMethod()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new LicensesRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToPostRequestInformation(new WrappedCreateCloudHSMSoftwareLicense());

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.POST, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToPostRequestInformationが正しいAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_SetsAcceptHeader()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new LicensesRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToPostRequestInformation(new WrappedCreateCloudHSMSoftwareLicense());

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// ToPostRequestInformationが正しいContent-Typeヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_SetsContentTypeHeader()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new LicensesRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToPostRequestInformation(new WrappedCreateCloudHSMSoftwareLicense());

        Assert.True(requestInfo.Headers.ContainsKey("Content-Type"));
        Assert.Contains("application/json", requestInfo.Headers["Content-Type"]);
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
        };
        var builder = new LicensesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var mockResponse = new PaginatedCloudHSMSoftwareLicenseList();
        requestAdapter.SendAsync<PaginatedCloudHSMSoftwareLicenseList>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PaginatedCloudHSMSoftwareLicenseList>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(mockResponse);

        var result = await builder.GetAsync();

        Assert.NotNull(result);
        Assert.IsType<PaginatedCloudHSMSoftwareLicenseList>(result);

        await requestAdapter.Received(1).SendAsync<PaginatedCloudHSMSoftwareLicenseList>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PaginatedCloudHSMSoftwareLicenseList>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());

        _mockLogger.Received(1).Log(LogLevel.Information, Arg.Any<EventId>(), Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^CloudHSMライセンス一覧の取得を開始します。 \d{4}-\d{2}-\d{2}T.*Z$")), null, Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(LogLevel.Information, Arg.Any<EventId>(), Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^CloudHSMライセンス一覧の取得が完了しました。 \d{4}-\d{2}-\d{2}T.*Z \[StatusCode, 200\]$")), null, Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsync が期待されるSendAsyncを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_CallsRequestAdapterSendAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new LicensesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var mockResponse = new WrappedCreateCloudHSMSoftwareLicense();
        requestAdapter.SendAsync<WrappedCreateCloudHSMSoftwareLicense>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedCreateCloudHSMSoftwareLicense>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(mockResponse);

        var result = await builder.PostAsync(new WrappedCreateCloudHSMSoftwareLicense
        {
            CloudHSM = new CreateCloudHSMSoftwareLicense
            {
                Name = "test-license",
                ServiceClass = CloudHSMSoftwareLicenseServiceClassEnum.CloudCloudhsmLicenseL7,
                Tags = new List<string> { "tag1" }
            }
        });

        Assert.NotNull(result);
        Assert.IsType<WrappedCreateCloudHSMSoftwareLicense>(result);

        await requestAdapter.Received(1).SendAsync<WrappedCreateCloudHSMSoftwareLicense>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedCreateCloudHSMSoftwareLicense>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());

        _mockLogger.Received(1).Log(LogLevel.Information, Arg.Any<EventId>(), Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^CloudHSMライセンスの作成を開始します。 \d{4}-\d{2}-\d{2}T.*Z$")), null, Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(LogLevel.Information, Arg.Any<EventId>(), Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^CloudHSMライセンスの作成が完了しました。 \d{4}-\d{2}-\d{2}T.*Z \[StatusCode, 201\]$")), null, Arg.Any<Func<object, Exception?, string>>());
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
        };
        var builder = new LicensesRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);
        var newUrl = "https://newapi.example.com/cloudhsm/licenses";

        var newBuilder = builder.WithUrl(newUrl);

        Assert.NotNull(newBuilder);
        Assert.NotSame(builder, newBuilder);
        Assert.IsType<LicensesRequestBuilder>(newBuilder);
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
        };

        Assert.Throws<ArgumentNullException>(() => new LicensesRequestBuilder(pathParameters, _mockRequestAdapter, null!));
    }

    /// <summary>
    /// コンストラクタ（RawUrl版）でLoggerFactoryがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrlAndNullLoggerFactory_ThrowsArgumentNullException()
    {
        var rawUrl = "https://api.example.com/cloudhsm/licenses";

        Assert.Throws<ArgumentNullException>(() => new LicensesRequestBuilder(rawUrl, _mockRequestAdapter, null!));
    }

    /// <summary>
    /// GetAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithApiError_ThrowsApiException()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new LicensesRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);
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

        _mockRequestAdapter.SendAsync<PaginatedCloudHSMSoftwareLicenseList>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PaginatedCloudHSMSoftwareLicenseList>>(),
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
                @"^CloudHSMライセンス一覧取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithApiError_ThrowsApiException()
    {
        var builder = new LicensesRequestBuilder(new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        }, _mockRequestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError { ResponseStatusCode = 400 };
        ((IAdditionalDataHolder)expectedException).AdditionalData = new Dictionary<string, object>
        {
            { "error_code", "bad_request" }, { "error_msg", "invalid license" }, { "is_fatal", false }, { "serial", "9876543210" }, { "status", "400 Bad Request" }
        };
        _mockRequestAdapter.SendAsync<WrappedCreateCloudHSMSoftwareLicense>(Arg.Any<RequestInformation>(), Arg.Any<ParsableFactory<WrappedCreateCloudHSMSoftwareLicense>>(), Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(), Arg.Any<CancellationToken>()).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(() => builder.PostAsync(new WrappedCreateCloudHSMSoftwareLicense { CloudHSM = new CreateCloudHSMSoftwareLicense { Name = "test-license", ServiceClass = CloudHSMSoftwareLicenseServiceClassEnum.CloudCloudhsmLicenseL7, Tags = new List<string> { "tag1" } } }));

        _mockLogger.Received(1).Log(LogLevel.Error, Arg.Any<EventId>(), Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^CloudHSMライセンス作成のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 400\], \[error_code, bad_request\], \[error_msg, invalid license\], \[is_fatal, False\], \[serial, 9876543210\], \[status, 400 Bad Request\]$")), Arg.Any<ApiException>(), Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncのリクエストボディに必須プロパティがない場合にバリデーションエラーと警告ログが出力されることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithMissingCloudHSM_ThrowsValidationExceptionAndLogsWarning()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new LicensesRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(new WrappedCreateCloudHSMSoftwareLicense()));

        Assert.Equal("CloudHSMが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("CloudHSMが入力されていません。");
        await _mockRequestAdapter.DidNotReceive().SendAsync<WrappedCreateCloudHSMSoftwareLicense>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedCreateCloudHSMSoftwareLicense>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// GetAsyncでその他のエラーが発生した場合に例外がスローされ、エラーログが出力されることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithOtherError_ThrowsExceptionAndLogsError()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new LicensesRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);
        var expectedException = new Exception("その他のエラー");

        _mockRequestAdapter.SendAsync<PaginatedCloudHSMSoftwareLicenseList>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PaginatedCloudHSMSoftwareLicenseList>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .ThrowsAsync(expectedException);

        var exception = await Assert.ThrowsAsync<Exception>(async () => await builder.GetAsync());

        Assert.Equal("その他のエラー", exception.Message);

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^CloudHSMライセンス一覧の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncでその他のエラーが発生した場合に例外がスローされ、エラーログが出力されることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithOtherError_ThrowsExceptionAndLogsError()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new LicensesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var expectedException = new Exception("その他のエラー");
        requestAdapter.SendAsync<WrappedCreateCloudHSMSoftwareLicense>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedCreateCloudHSMSoftwareLicense>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .ThrowsAsync(expectedException);

        var exception = await Assert.ThrowsAsync<Exception>(async () => await builder.PostAsync(new WrappedCreateCloudHSMSoftwareLicense
        {
            CloudHSM = new CreateCloudHSMSoftwareLicense
            {
                Name = "test-license",
                ServiceClass = CloudHSMSoftwareLicenseServiceClassEnum.CloudCloudhsmLicenseL7,
                Tags = new List<string> { "tag1" }
            }
        }));

        Assert.Equal("その他のエラー", exception.Message);

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^CloudHSMライセンスの作成中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }
}
