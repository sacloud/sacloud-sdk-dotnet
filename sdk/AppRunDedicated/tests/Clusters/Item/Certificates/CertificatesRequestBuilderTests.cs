using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.AppRunDedicated.Clusters.Item.Certificates;
using Sacloud.Sdk.AppRunDedicated.Clusters.Item.Certificates.Item;
using Sacloud.Sdk.AppRunDedicated.Models;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.AppRunDedicated.Tests.Clusters.Item.Certificates;

/// <summary>
/// CertificatesRequestBuilderの単体テスト
/// </summary>
public class CertificatesRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<CertificatesRequestBuilder> _mockLogger;
    private readonly Guid _validClusterID;

    public CertificatesRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<CertificatesRequestBuilder>>();
        _mockLoggerFactory.CreateLogger(Arg.Any<string>()).Returns(_mockLogger);
        _validClusterID = Guid.NewGuid();
    }

    /// <summary>
    /// PathParametersを使用したコンストラクタが正しく動作することを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_SetsProperties()
    {
        var pathParameters = new Dictionary<string, object>();
        var requestAdapter = Substitute.For<IRequestAdapter>();

        var builder = new CertificatesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// rawUrlを使用したコンストラクタが正しく動作することを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_SetsProperties()
    {
        var rawUrl = "https://example.com/clusters/123/certificates";
        var requestAdapter = Substitute.For<IRequestAdapter>();

        var builder = new CertificatesRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// Guidインデクサーが正しくWithCertificateItemRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_WithGuid_ReturnsWithCertificateItemRequestBuilder()
    {
        var pathParameters = new Dictionary<string, object>();
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new CertificatesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var certificateId = Guid.NewGuid();

        var itemBuilder = builder[certificateId];

        Assert.NotNull(itemBuilder);
        Assert.IsType<WithCertificateItemRequestBuilder>(itemBuilder);
    }

    /// <summary>
    /// ToGetRequestInformationが正しいRequestInformationを生成することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_ReturnsValidRequestInformation()
    {
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://example.com" }, { "clusterID", _validClusterID } };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new CertificatesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.GET, requestInfo.HttpMethod);
        Assert.Contains("certificates", requestInfo.UrlTemplate);
    }

    /// <summary>
    /// ToGetRequestInformationがQueryParametersを正しく設定することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_WithQueryParameters_SetsQueryParameters()
    {
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://example.com" }, { "clusterID", _validClusterID } };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new CertificatesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var cursor = Guid.NewGuid();
        var lastCertificateId = Guid.NewGuid();

        var requestInfo = builder.ToGetRequestInformation(config =>
        {
            config.QueryParameters.Cursor = cursor;
            config.QueryParameters.MaxItems = 10;
        });

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.GET, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToPostRequestInformationが正しいRequestInformationを生成することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_ReturnsValidRequestInformation()
    {
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://example.com" }, { "clusterID", _validClusterID } };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new CertificatesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var createCertificate = new CreateCertificateRequest
        {
            Name = "test-cert",
            CertificatePem = "-----BEGIN CERTIFICATE-----",
            PrivatekeyPem = "-----BEGIN PRIVATE KEY-----"
        };

        var requestInfo = builder.ToPostRequestInformation(createCertificate);

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.POST, requestInfo.HttpMethod);
        Assert.Contains("certificates", requestInfo.UrlTemplate);
    }

    /// <summary>
    /// GetAsyncのパスパラメータにclusterIDがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("clusterID", "")]
    [InlineData("clusterID", " ")]
    [InlineData("clusterID", "　")]
    public async Task GetAsync_WithNullOrEmptyPathParameters_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { pathParameterKey, pathParameterValue }
        };
        var builder = new CertificatesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
    public async Task GetAsync_WithInvalidFormatPathParameters_ThrowsValidationException(string invalidPathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", invalidPathParameterValue }
        };
        var builder = new CertificatesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync();
        });

        Assert.Equal("クラスタIDはGUID形式で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("クラスタIDはGUID形式で入力してください。");
    }

    /// <summary>
    /// GetAsyncのリクエストパラメータのMaxItemsがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithNullMaxItems_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validClusterID }
        };
        var builder = new CertificatesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync(config =>
            {
                config.QueryParameters.MaxItems = null;
            });
        });

        Assert.Equal("MaxItemsが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("MaxItemsが入力されていません。");
    }

    /// <summary>
    /// GetAsyncのリクエストパラメータのMaxItemsが有効な数値範囲でない値の場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(4)]
    [InlineData(31)]
    public async Task GetAsync_WithInvalidRangeMaxItems_ThrowsValidationException(int invalidMaxItemsValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validClusterID }
        };
        var builder = new CertificatesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync(config =>
            {
                config.QueryParameters.MaxItems = invalidMaxItemsValue;
            });
        });

        Assert.Equal("MaxItemsの値は5以上30以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("MaxItemsの値は5以上30以下で入力してください。");
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
            { "clusterID", _validClusterID }
        };
        var builder = new CertificatesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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

        requestAdapter.SendAsync<ListCertificatesResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListCertificatesResponse>>(),
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
                @"^証明書の一覧取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
            { "clusterID", _validClusterID }
        };
        var builder = new CertificatesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync<ListCertificatesResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListCertificatesResponse>>(),
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
                @"^証明書の一覧の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncがListCertificatesResponseを返すことを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_ReturnsListCertificateResponse()
    {
        var pathParameters = new Dictionary<string, object>
        { 
            { "baseurl", "https://example.com" },
            { "clusterID", _validClusterID }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var expectedResponse = new ListCertificatesResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListCertificatesResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);
        var builder = new CertificatesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var response = await builder.GetAsync();

        Assert.NotNull(response);
        Assert.IsType<ListCertificatesResponse>(response);

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^証明書の一覧の取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^証明書の一覧の取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncのパスパラメータにclusterIDがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("clusterID", "")]
    [InlineData("clusterID", " ")]
    [InlineData("clusterID", "　")]
    public async Task PostAsync_WithNullOrEmptyPathParameters_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { pathParameterKey, pathParameterValue }
        };
        var builder = new CertificatesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateCertificateRequest
        {
            Name = "test-cert",
            CertificatePem = "test-certificate-pem",
            PrivatekeyPem = "test-private-pem"
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        Assert.Equal("クラスタIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("クラスタIDを入力してください。");
    }

    /// <summary>
    /// PostAsyncのパスパラメータが有効なGUID形式の値でない場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("test-value")]
    [InlineData("1234-567890ab-cdef-1234-567890abcdef")]
    [InlineData("12345678-90ab-cdef-1234-567890ghijkl")]
    public async Task PostAsync_WithInvalidFormatPathParameters_ThrowsValidationException(string invalidPathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", invalidPathParameterValue }
        };
        var builder = new CertificatesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateCertificateRequest
        {
            Name = "test-cert",
            CertificatePem = "test-certificate-pem",
            PrivatekeyPem = "test-private-pem"
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        Assert.Equal("クラスタIDはGUID形式で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("クラスタIDはGUID形式で入力してください。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithNullBody_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validClusterID }
        };
        var builder = new CertificatesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(null);
        });

        Assert.Equal("リクエストボディは必須です。", exception.Message);
        _mockLogger.Received(1).LogWarning("リクエストボディは必須です。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのパラメータがnullの場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("Name")]
    [InlineData("CertificatePem")]
    [InlineData("PrivatekeyPem")]
    public async Task PostAsync_WithNullRequestBodyParameter_ThrowsValidationException(string invalidParameterName)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validClusterID }
        };
        var builder = new CertificatesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateCertificateRequest
        {
            Name = invalidParameterName == "Name" ? null : "test-cert",
            CertificatePem = invalidParameterName == "CertificatePem" ? null : "test-certificate-pem",
            PrivatekeyPem = invalidParameterName == "PrivatekeyPem" ? null : "test-privatekey-pem"
        };
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });
        Assert.Equal($"{invalidParameterName}が入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}が入力されていません。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのパラメータの値が有効な最大文字数を超過した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("CertificatePem", 1000000)]
    [InlineData("PrivatekeyPem", 1000000)]
    [InlineData("IntermediateCertificatePem", 1000000)]
    public async Task PostAsync_WithInvalidMaxLengthRequestBodyParameter_ThrowsValidationException(string invalidParameterName, int validMaxLength)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validClusterID }
        };
        var builder = new CertificatesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateCertificateRequest
        {
            Name = invalidParameterName == "Name" ? "123456789012345678901" : "test-cert",
            CertificatePem = invalidParameterName == "CertificatePem" ? new string('a', 1000001) : "test-certificate-pem",
            PrivatekeyPem = invalidParameterName == "PrivatekeyPem" ? new string('a', 1000001) : "test-privatekey-pem",
            IntermediateCertificatePem = invalidParameterName == "IntermediateCertificatePem" ? new string('a', 1000001) : "test-intermediate-certificate-pem",
        };
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });
        Assert.Equal($"{invalidParameterName}は{validMaxLength}文字以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}は{validMaxLength}文字以下で入力してください。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのNameが有効な最大文字数を超過した場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithInvalidMaxLengthName_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validClusterID }
        };
        var builder = new CertificatesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateCertificateRequest
        {
            Name = "123456789012345678901",
            PrivatekeyPem = "test-privatekey-pem",
            CertificatePem = "test-certificate-pem"
        };
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });
        Assert.Equal("Nameは1文字以上20文字以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("Nameは1文字以上20文字以下で入力してください。");
    }

    /// <summary>
    /// PostAsyncのパスパラメータの文字数が正規表現パターンに一致しなかった場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalid+name")]
    [InlineData("無効なName")]
    public async Task PostAsync_WithInvalidRegexPatternRequestBodyParameter_ThrowsValidationException(string invalidParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validClusterID }
        };
        var builder = new CertificatesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateCertificateRequest
        {
            Name = invalidParameterValue,
            CertificatePem = "test-certificate-pem",
            PrivatekeyPem = "test-private-key-pem"
        };
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });
        Assert.Equal("Nameの形式が正しくありません。", exception.Message);
        _mockLogger.Received(1).LogWarning("Nameの形式が正しくありません。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディの複数パラメータが無効な値の場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithMultipleInvalidRequestBodyParameters_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validClusterID }
        };
        var builder = new CertificatesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateCertificateRequest
        {
            Name = "invalid+name",
            CertificatePem = null,
            PrivatekeyPem = new string('a', 1000001),
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
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
    /// PostAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithApiError_ThrowsApiException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validClusterID }
        };
        var builder = new CertificatesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateCertificateRequest
        {
            Name = "test-cert",
            CertificatePem = "test-certificate-pem",
            PrivatekeyPem = "test-private-key-pem"
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

        requestAdapter.SendAsync<CreateCertificateResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreateCertificateResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(async () =>
        {
            await builder.PostAsync(body);
        });

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^証明書作成のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncでその他のエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithOtherError_ThrowsException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validClusterID }
        };
        var builder = new CertificatesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateCertificateRequest
        {
            Name = "test-cert",
            CertificatePem = "test-certificate-pem",
            PrivatekeyPem = "test-private-key-pem"
        };
        var expectedException = new Exception();

        requestAdapter.SendAsync<CreateCertificateResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreateCertificateResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<Exception>(async () =>
        {
            await builder.PostAsync(body);
        });

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^証明書の作成中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncがCreateCertificateResponseを返すことを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_ReturnsCreateCertificateResponse()
    {
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://example.com" }, { "clusterID", _validClusterID } };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var expectedResponse = new CreateCertificateResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreateCertificateResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);
        var builder = new CertificatesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var createCertificate = new CreateCertificateRequest
        {
            Name = "test-cert",
            CertificatePem = "-----BEGIN CERTIFICATE-----",
            PrivatekeyPem = "-----BEGIN PRIVATE KEY-----"
        };

        var response = await builder.PostAsync(createCertificate);

        Assert.NotNull(response);
        Assert.IsType<CreateCertificateResponse>(response);

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^証明書の作成を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^証明書の作成が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// WithUrlが新しいCertificatesRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewBuilder()
    {
        var pathParameters = new Dictionary<string, object>();
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new CertificatesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var rawUrl = "https://example.com/clusters/123/certificates";

        var newBuilder = builder.WithUrl(rawUrl);

        Assert.NotNull(newBuilder);
        Assert.IsType<CertificatesRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }

    /// <summary>
    /// QueryParametersのCursorプロパティが正しく設定されることを確認
    /// </summary>
    [Fact]
    public void CertificatesRequestBuilderGetQueryParameters_Cursor_SetAndGet()
    {
        var queryParameters = new CertificatesRequestBuilder.CertificatesRequestBuilderGetQueryParameters();
        var cursor = Guid.NewGuid();

        queryParameters.Cursor = cursor;

        Assert.Equal(cursor, queryParameters.Cursor);
    }

    /// <summary>
    /// QueryParametersのMaxItemsプロパティが正しく設定されることを確認
    /// </summary>
    [Fact]
    public void CertificatesRequestBuilderGetQueryParameters_MaxItems_SetAndGet()
    {
        var queryParameters = new CertificatesRequestBuilder.CertificatesRequestBuilderGetQueryParameters
        {
            MaxItems = 10
        };

        Assert.Equal(10, queryParameters.MaxItems);
    }
}
