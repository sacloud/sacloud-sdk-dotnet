using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.AppRunDedicated.Clusters.Item.Certificates.Item;
using Sacloud.Sdk.AppRunDedicated.Models;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.AppRunDedicated.Tests.Clusters.Item.Certificates.Item;

/// <summary>
/// WithCertificateItemRequestBuilderの単体テスト
/// </summary>
public class WithCertificateItemRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<WithCertificateItemRequestBuilder> _mockLogger;
    private readonly Guid _validGuid;

    public WithCertificateItemRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<WithCertificateItemRequestBuilder>>();
        _mockLoggerFactory.CreateLogger(Arg.Any<string>()).Returns(_mockLogger);
        _validGuid = Guid.NewGuid();
    }

    /// <summary>
    /// PathParametersを使用したコンストラクタが正しく動作することを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_SetsProperties()
    {
        var pathParameters = new Dictionary<string, object>();
        var requestAdapter = Substitute.For<IRequestAdapter>();

        var builder = new WithCertificateItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// rawUrlを使用したコンストラクタが正しく動作することを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_SetsProperties()
    {
        var rawUrl = "https://example.com/clusters/123/certificates/456";
        var requestAdapter = Substitute.For<IRequestAdapter>();

        var builder = new WithCertificateItemRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// ToDeleteRequestInformationが正しいRequestInformationを生成することを確認
    /// </summary>
    [Fact]
    public void ToDeleteRequestInformation_ReturnsValidRequestInformation()
    {
        var pathParameters = new Dictionary<string, object> 
        { 
            { "baseurl", "https://example.com" }, 
            { "clusterID", _validGuid },
            { "certificateID", _validGuid }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WithCertificateItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToDeleteRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.DELETE, requestInfo.HttpMethod);
        Assert.Contains("certificates", requestInfo.UrlTemplate);
    }

    /// <summary>
    /// ToGetRequestInformationが正しいRequestInformationを生成することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_ReturnsValidRequestInformation()
    {
        var pathParameters = new Dictionary<string, object> 
        { 
            { "baseurl", "https://example.com" }, 
            { "clusterID", _validGuid },
            { "certificateID", _validGuid }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WithCertificateItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.GET, requestInfo.HttpMethod);
        Assert.Contains("certificates", requestInfo.UrlTemplate);
    }

    /// <summary>
    /// ToPutRequestInformationが正しいRequestInformationを生成することを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_ReturnsValidRequestInformation()
    {
        var pathParameters = new Dictionary<string, object> 
        { 
            { "baseurl", "https://example.com" }, 
            { "clusterID", _validGuid },
            { "certificateID", _validGuid }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WithCertificateItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var updateCertificate = new UpdateCertificateRequest
        {
            Name = "updated-cert",
            CertificatePem = "-----BEGIN CERTIFICATE-----"
        };

        var requestInfo = builder.ToPutRequestInformation(updateCertificate);

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.PUT, requestInfo.HttpMethod);
        Assert.Contains("certificates", requestInfo.UrlTemplate);
    }

    /// <summary>
    /// DeleteAsyncのパスパラメータにclusterIDがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("clusterID", "")]
    [InlineData("clusterID", " ")]
    [InlineData("clusterID", "　")]
    public async Task DeleteAsync_WithEmptyClusterID_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { pathParameterKey, pathParameterValue },
            { "certificateID", _validGuid }
        };
        var builder = new WithCertificateItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.DeleteAsync();
        });

        Assert.Equal("クラスタIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("クラスタIDを入力してください。");
    }

    /// <summary>
    /// DeleteAsyncのパスパラメータが有効なGUID形式の値でない場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("test-value")]
    [InlineData("1234-567890ab-cdef-1234-567890abcdef")]
    [InlineData("12345678-90ab-cdef-1234-567890ghijkl")]
    public async Task DeleteAsync_WithInvalidFormatClusterID_ThrowsValidationException(string invalidPathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", invalidPathParameterValue },
            { "certificateID", _validGuid }
        };
        var builder = new WithCertificateItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.DeleteAsync();
        });

        Assert.Equal("クラスタIDはGUID形式で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("クラスタIDはGUID形式で入力してください。");
    }

    /// <summary>
    /// DeleteAsyncのパスパラメータにcertificateIDがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("certificateID", "")]
    [InlineData("certificateID", " ")]
    [InlineData("certificateID", "　")]
    public async Task DeleteAsync_WithEmptyCertificateID_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validGuid },
            { pathParameterKey, pathParameterValue }
        };
        var builder = new WithCertificateItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.DeleteAsync();
        });

        Assert.Equal("証明書IDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("証明書IDを入力してください。");
    }

    /// <summary>
    /// DeleteAsyncのパスパラメータが有効なGUID形式の値でない場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("test-value")]
    [InlineData("1234-567890ab-cdef-1234-567890abcdef")]
    [InlineData("12345678-90ab-cdef-1234-567890ghijkl")]
    public async Task DeleteAsync_WithInvalidFormatCertificateID_ThrowsValidationException(string invalidPathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validGuid },
            { "certificateID", invalidPathParameterValue }
        };
        var builder = new WithCertificateItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.DeleteAsync();
        });

        Assert.Equal("証明書IDはGUID形式で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("証明書IDはGUID形式で入力してください。");
    }

    /// <summary>
    /// DeleteAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_WithApiError_ThrowsApiException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validGuid },
            { "certificateID", _validGuid }
        };
        var builder = new WithCertificateItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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

        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(async () =>
        {
            await builder.DeleteAsync();
        });

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^証明書削除のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// DeleteAsyncでその他のエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_WithOtherError_ThrowsException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validGuid },
            { "certificateID", _validGuid }
        };
        var builder = new WithCertificateItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<Exception>(async () =>
        {
            await builder.DeleteAsync();
        });

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^証明書の削除中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// DeleteAsyncが正常に完了することを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_CompletesSuccessfully()
    {
        var pathParameters = new Dictionary<string, object> 
        { 
            { "baseurl", "https://example.com" },
            { "clusterID", _validGuid },
            { "certificateID", _validGuid }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(Task.CompletedTask);
        var builder = new WithCertificateItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await builder.DeleteAsync();

        await requestAdapter.Received(1).SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^証明書の削除を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^証明書の削除が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 204\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncのパスパラメータにclusterIDがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("clusterID", "")]
    [InlineData("clusterID", " ")]
    [InlineData("clusterID", "　")]
    public async Task GetAsync_WithEmptyClusterID_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { pathParameterKey, pathParameterValue },
            { "certificateID", _validGuid }
        };
        var builder = new WithCertificateItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync();
        });

        Assert.Equal("クラスタIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("クラスタIDを入力してください。");
    }

    /// <summary>
    /// GetAsyncのパスパラメータが有効なGUID形式の値でない場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("test-value")]
    [InlineData("1234-567890ab-cdef-1234-567890abcdef")]
    [InlineData("12345678-90ab-cdef-1234-567890ghijkl")]
    public async Task GetAsync_WithInvalidFormatClusterID_ThrowsValidationException(string invalidPathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", invalidPathParameterValue },
            { "certificateID", _validGuid }
        };
        var builder = new WithCertificateItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync();
        });

        Assert.Equal("クラスタIDはGUID形式で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("クラスタIDはGUID形式で入力してください。");
    }

    /// <summary>
    /// GetAsyncのパスパラメータにcertificateIDがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("certificateID", "")]
    [InlineData("certificateID", " ")]
    [InlineData("certificateID", "　")]
    public async Task GetAsync_WithEmptyCertificateID_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validGuid },
            { pathParameterKey, pathParameterValue }
        };
        var builder = new WithCertificateItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync();
        });

        Assert.Equal("証明書IDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("証明書IDを入力してください。");
    }

    /// <summary>
    /// GetAsyncのパスパラメータが有効なGUID形式の値でない場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("test-value")]
    [InlineData("1234-567890ab-cdef-1234-567890abcdef")]
    [InlineData("12345678-90ab-cdef-1234-567890ghijkl")]
    public async Task GetAsync_WithInvalidFormatCertificateID_ThrowsValidationException(string invalidPathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validGuid },
            { "certificateID", invalidPathParameterValue }
        };
        var builder = new WithCertificateItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync();
        });

        Assert.Equal("証明書IDはGUID形式で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("証明書IDはGUID形式で入力してください。");
    }

    /// <summary>
    /// GetAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithApiError_ThrowsApiException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validGuid },
            { "certificateID", _validGuid }
        };
        var builder = new WithCertificateItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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

        requestAdapter.SendAsync<ReadCertificateResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ReadCertificateResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(async () =>
        {
            await builder.GetAsync();
        });

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^証明書の詳細取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncでその他のエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithOtherError_ThrowsException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validGuid },
            { "certificateID", _validGuid }
        };
        var builder = new WithCertificateItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync<ReadCertificateResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ReadCertificateResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<Exception>(async () =>
        {
            await builder.GetAsync();
        });

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^証明書の詳細の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncがReadCertificateResponseを返すことを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_ReturnsGetCertificateResponse()
    {
        var pathParameters = new Dictionary<string, object> 
        { 
            { "baseurl", "https://example.com" }, 
            { "clusterID", _validGuid },
            { "certificateID", _validGuid }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var expectedResponse = new ReadCertificateResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ReadCertificateResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);
        var builder = new WithCertificateItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var response = await builder.GetAsync();

        Assert.NotNull(response);
        Assert.IsType<ReadCertificateResponse>(response);

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^証明書の詳細の取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^証明書の詳細の取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PutAsyncのパスパラメータにclusterIDがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("clusterID", "")]
    [InlineData("clusterID", " ")]
    [InlineData("clusterID", "　")]
    public async Task PutAsync_WithEmptyClusterID_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { pathParameterKey, pathParameterValue },
            { "certificateID", _validGuid }
        };
        var builder = new WithCertificateItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateCertificateRequest
        {
            Name = "test-cert",
            CertificatePem = "test-certificate-pem",
            PrivatekeyPem = "test-privatekey-pem"
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(body);
        });

        Assert.Equal("クラスタIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("クラスタIDを入力してください。");
    }

    /// <summary>
    /// PutAsyncのパスパラメータが有効なGUID形式の値でない場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("test-value")]
    [InlineData("1234-567890ab-cdef-1234-567890abcdef")]
    [InlineData("12345678-90ab-cdef-1234-567890ghijkl")]
    public async Task PutAsync_WithInvalidFormatClusterID_ThrowsValidationException(string invalidPathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", invalidPathParameterValue },
            { "certificateID", _validGuid }
        };
        var builder = new WithCertificateItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateCertificateRequest
        {
            Name = "test-cert",
            CertificatePem = "test-certificate-pem",
            PrivatekeyPem = "test-privatekey-pem"
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(body);
        });

        Assert.Equal("クラスタIDはGUID形式で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("クラスタIDはGUID形式で入力してください。");
    }

    /// <summary>
    /// PutAsyncのパスパラメータにcertificateIDがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("certificateID", "")]
    [InlineData("certificateID", " ")]
    [InlineData("certificateID", "　")]
    public async Task PutAsync_WithEmptyCertificateID_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validGuid },
            { pathParameterKey, pathParameterValue }
        };
        var builder = new WithCertificateItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateCertificateRequest
        {
            Name = "test-cert",
            CertificatePem = "test-certificate-pem",
            PrivatekeyPem = "test-privatekey-pem"
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(body);
        });

        Assert.Equal("証明書IDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("証明書IDを入力してください。");
    }

    /// <summary>
    /// PutAsyncのパスパラメータが有効なGUID形式の値でない場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("test-value")]
    [InlineData("1234-567890ab-cdef-1234-567890abcdef")]
    [InlineData("12345678-90ab-cdef-1234-567890ghijkl")]
    public async Task PutAsync_WithInvalidFormatCertificateID_ThrowsValidationException(string invalidPathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validGuid },
            { "certificateID", invalidPathParameterValue }
        };
        var builder = new WithCertificateItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateCertificateRequest
        {
            Name = "test-cert",
            CertificatePem = "test-certificate-pem",
            PrivatekeyPem = "test-privatekey-pem"
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(body);
        });

        Assert.Equal("証明書IDはGUID形式で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("証明書IDはGUID形式で入力してください。");
    }

    /// <summary>
    /// PutAsyncのリクエストボディがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithNullBody_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validGuid },
            { "certificateID", _validGuid }
        };
        var builder = new WithCertificateItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(null);
        });

        Assert.Equal("リクエストボディは必須です。", exception.Message);
        _mockLogger.Received(1).LogWarning("リクエストボディは必須です。");
    }

    /// <summary>
    /// PutAsyncのリクエストボディのパラメータがnullの場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("Name")]
    [InlineData("CertificatePem")]
    [InlineData("PrivatekeyPem")]
    public async Task PutAsync_WithNullRequestBodyParameter_ThrowsValidationException(string invalidParameterName)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validGuid },
            { "certificateID", _validGuid }
        };
        var builder = new WithCertificateItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateCertificateRequest
        {
            Name = invalidParameterName == "Name" ? null : "test-cert",
            CertificatePem = invalidParameterName == "CertificatePem" ? null : "test-certificate-pem",
            PrivatekeyPem = invalidParameterName == "PrivatekeyPem" ? null : "test-privatekey-pem"
        };
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(body);
        });
        Assert.Equal($"{invalidParameterName}が入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}が入力されていません。");
    }

    /// <summary>
    /// PutAsyncのリクエストボディのパラメータの値が有効な最大文字数を超過した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("CertificatePem", 1000000)]
    [InlineData("PrivatekeyPem", 1000000)]
    [InlineData("IntermediateCertificatePem", 1000000)]
    public async Task PutAsync_WithInvalidMaxLengthRequestBodyParameter_ThrowsValidationException(string invalidParameterName, int validMaxLength)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validGuid },
            { "certificateID", _validGuid }
        };
        var builder = new WithCertificateItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateCertificateRequest
        {
            Name = invalidParameterName == "Name" ? "123456789012345678901" : "test-cert",
            CertificatePem = invalidParameterName == "CertificatePem" ? new string('a', 1000001) : "test-certificate-pem",
            PrivatekeyPem = invalidParameterName == "PrivatekeyPem" ? new string('a', 1000001) : "test-privatekey-pem",
            IntermediateCertificatePem = invalidParameterName == "IntermediateCertificatePem" ? new string('a', 1000001) : "test-intermediate-certificate-pem",
        };
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(body);
        });
        Assert.Equal($"{invalidParameterName}は{validMaxLength}文字以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}は{validMaxLength}文字以下で入力してください。");
    }

    /// <summary>
    /// PutAsyncのリクエストボディのNameが有効な最大文字数を超過した場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithInvalidMaxLengthName_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validGuid },
            { "certificateID", _validGuid }
        };
        var builder = new WithCertificateItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateCertificateRequest
        {
            Name = "123456789012345678901",
            CertificatePem = "test-certificate-pem",
            PrivatekeyPem = "test-privatekey-pem"
        };
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(body);
        });
        Assert.Equal("Nameは1文字以上20文字以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("Nameは1文字以上20文字以下で入力してください。");
    }

    /// <summary>
    /// PutAsyncのパスパラメータの文字数が正規表現パターンに一致しなかった場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalid+name")]
    [InlineData("無効なName")]
    public async Task PutAsync_WithInvalidRegexPatternRequestBodyParameter_ThrowsValidationException(string invalidParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validGuid },
            { "certificateID", _validGuid }
        };
        var builder = new WithCertificateItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateCertificateRequest
        {
            Name = invalidParameterValue,
            CertificatePem = "test-certificate-pem",
            PrivatekeyPem = "test-private-key-pem"
        };
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(body);
        });
        Assert.Equal("Nameの形式が正しくありません。", exception.Message);
        _mockLogger.Received(1).LogWarning("Nameの形式が正しくありません。");
    }

    /// <summary>
    /// PutAsyncのリクエストボディの複数パラメータが無効な値の場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithMultipleInvalidRequestBodyParameters_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validGuid },
            { "certificateID", _validGuid }
        };
        var builder = new WithCertificateItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateCertificateRequest
        {
            Name = "invalid+name",
            CertificatePem = null,
            PrivatekeyPem = new string('a', 1000001),
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(body);
        });

        var expectedMessage = """
            CertificatePemが入力されていません。
            Nameの形式が正しくありません。
            PrivatekeyPemは1000000文字以下で入力してください。
            """;
        Assert.Equal(expectedMessage, exception.Message);
        _mockLogger.Received(1).LogWarning("CertificatePemが入力されていません。");
        _mockLogger.Received(1).LogWarning("Nameの形式が正しくありません。");
        _mockLogger.Received(1).LogWarning("PrivatekeyPemは1000000文字以下で入力してください。");
    }

    /// <summary>
    /// PutAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithApiError_ThrowsApiException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validGuid },
            { "certificateID", _validGuid }
        };
        var builder = new WithCertificateItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateCertificateRequest
        {
            Name = "test-cert",
            CertificatePem = "test-certificate-pem",
            PrivatekeyPem = "test-privatekey-pem"
        };
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

        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(async () =>
        {
            await builder.PutAsync(body);
        });

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^証明書更新のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PutAsyncでその他のエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithOtherError_ThrowsException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validGuid },
            { "certificateID", _validGuid }
        };
        var builder = new WithCertificateItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateCertificateRequest
        {
            Name = "test-cert",
            CertificatePem = "test-certificate-pem",
            PrivatekeyPem = "test-privatekey-pem"
        };
        var expectedException = new Exception();

        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<Exception>(async () =>
        {
            await builder.PutAsync(body);
        });

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^証明書の更新中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PutAsyncが正常に完了することを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_CompletesSuccessfully()
    {
        var pathParameters = new Dictionary<string, object> 
        { 
            { "baseurl", "https://example.com" }, 
            { "clusterID", _validGuid },
            { "certificateID", _validGuid }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(Task.CompletedTask);
        var builder = new WithCertificateItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var updateCertificate = new UpdateCertificateRequest
        {
            Name = "test-cert",
            CertificatePem = "test-certificate-pem",
            PrivatekeyPem = "test-privatekey-pem"
        };

        await builder.PutAsync(updateCertificate);

        await requestAdapter.Received(1).SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^証明書の更新を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^証明書の更新が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 204\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// WithUrlが新しいWithCertificateItemRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewBuilder()
    {
        var pathParameters = new Dictionary<string, object>();
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WithCertificateItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var rawUrl = "https://example.com/clusters/123/certificates/456";

        var newBuilder = builder.WithUrl(rawUrl);

        Assert.NotNull(newBuilder);
        Assert.IsType<WithCertificateItemRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }
}
