using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.AppRunDedicated.Applications.Item.Versions;
using Sacloud.Sdk.AppRunDedicated.Applications.Item.Versions.Item;
using Sacloud.Sdk.AppRunDedicated.Models;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.AppRunDedicated.Tests.Applications.Item.Versions;

/// <summary>
/// VersionsRequestBuilderのテスト
/// </summary>
public class VersionsRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<VersionsRequestBuilder> _mockLogger;
    private readonly Guid _validApplicationID;

    public VersionsRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<VersionsRequestBuilder>>();
        _mockLoggerFactory.CreateLogger<VersionsRequestBuilder>().Returns(_mockLogger);
        _validApplicationID = Guid.NewGuid();
    }

    /// <summary>
    /// コンストラクタ（PathParameters版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "applicationID", _validApplicationID }
        };

        var builder = new VersionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var rawUrl = "https://api.example.com/applications/12345678-1234-1234-1234-123456789012/versions";

        var builder = new VersionsRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// インデクサー（int版）が正しいWithVersionItemRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_WithInt_ReturnsWithVersionItemRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "applicationID", _validApplicationID }
        };
        var builder = new VersionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var itemBuilder = builder[1];

        Assert.NotNull(itemBuilder);
        Assert.IsType<WithVersionItemRequestBuilder>(itemBuilder);
    }

    /// <summary>
    /// インデクサーが毎回新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_ReturnsNewInstance_OnEachCall()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "applicationID", _validApplicationID }
        };
        var builder = new VersionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var itemBuilder1 = builder[1];
        var itemBuilder2 = builder[1];

        Assert.NotSame(itemBuilder1, itemBuilder2);
    }

    /// <summary>
    /// ToGetRequestInformationがGETメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_CreatesRequestInformationWithGetMethod()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "applicationID", _validApplicationID }
        };
        var builder = new VersionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "applicationID", _validApplicationID }
        };
        var builder = new VersionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// ToGetRequestInformationがクエリパラメータを正しく処理することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_WithQueryParameters_ConfiguresCorrectly()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "applicationID", _validApplicationID }
        };
        var builder = new VersionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation(config =>
        {
            config.QueryParameters.Cursor = 1;
            config.QueryParameters.MaxItems = 10;
        });

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.GET, requestInfo.HttpMethod);
    }

    /// <summary>
    /// QueryParametersのCursorプロパティセッターが正常に動作することを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Cursor_CanBeSet()
    {
        var queryParams = new VersionsRequestBuilder.VersionsRequestBuilderGetQueryParameters
        {
            Cursor = 10
        };

        Assert.Equal(10, queryParams.Cursor);
    }

    /// <summary>
    /// QueryParametersのMaxItemsプロパティセッターが正常に動作することを確認
    /// </summary>
    [Fact]
    public void QueryParameters_MaxItems_CanBeSet()
    {
        var queryParams = new VersionsRequestBuilder.VersionsRequestBuilderGetQueryParameters
        {
            MaxItems = 100
        };

        Assert.Equal(100, queryParams.MaxItems);
    }

    /// <summary>
    /// ToPostRequestInformationがPOSTメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_CreatesRequestInformationWithPostMethod()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "applicationID", _validApplicationID }
        };
        var builder = new VersionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateApplicationVersionRequest
        {
            Image = "test-image"
        };

        var requestInfo = builder.ToPostRequestInformation(body);

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.POST, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToPostRequestInformationが正しいAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_SetsAcceptHeader()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "applicationID", _validApplicationID }
        };
        var builder = new VersionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateApplicationVersionRequest
        {
            Image = "test-image"
        };

        var requestInfo = builder.ToPostRequestInformation(body);

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// GetAsyncのパスパラメータにapplicationIDがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("applicationID", "")]
    [InlineData("applicationID", " ")]
    [InlineData("applicationID", "　")]
    public async Task GetAsync_WithNullOrEmptyPathParameters_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { pathParameterKey, pathParameterValue }
        };
        var builder = new VersionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new ReadApplicationResponse();

        requestAdapter.SendAsync<ReadApplicationResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ReadApplicationResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync();
        });

        Assert.Equal("アプリケーションIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("アプリケーションIDを入力してください。");
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
            { "applicationID", invalidPathParameterValue }
        };
        var builder = new VersionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new ReadApplicationResponse();

        requestAdapter.SendAsync<ReadApplicationResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ReadApplicationResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync();
        });

        Assert.Equal("アプリケーションIDはGUID形式で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("アプリケーションIDはGUID形式で入力してください。");
    }

    /// <summary>
    /// GetAsyncのクエリパラメータのMaxItemsがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithNullQueryParameter_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "applicationID", _validApplicationID }
        };
        var builder = new VersionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
    /// GetAsyncのクエリパラメータが有効な最小値を下回る値の場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithInvalidMinimumQueryParameter_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "applicationID", _validApplicationID }
        };
        var builder = new VersionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync(config =>
            {
                config.QueryParameters.Cursor = 0;
                config.QueryParameters.MaxItems = 30;
            });
        });

        Assert.Equal("Cursorの値は1以上で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("Cursorの値は1以上で入力してください。");
    }

    /// <summary>
    /// GetAsyncのクエリパラメータが有効な数値範囲でない値の場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(31)]
    public async Task GetAsync_WithInvalidRangeQueryParameter_ThrowsValidationException(int invalidMaxItemsValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "applicationID", _validApplicationID }
        };
        var builder = new VersionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync(config =>
            {
                config.QueryParameters.MaxItems = invalidMaxItemsValue;
            });
        });

        Assert.Equal("MaxItemsの値は1以上30以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("MaxItemsの値は1以上30以下で入力してください。");
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
            { "applicationID", _validApplicationID }
        };
        var builder = new VersionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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

        requestAdapter.SendAsync<ListApplicationVersionsResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListApplicationVersionsResponse>>(),
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
                @"^アプリケーションバージョン一覧取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
            { "applicationID", _validApplicationID }
        };
        var builder = new VersionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync<ListApplicationVersionsResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListApplicationVersionsResponse>>(),
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
                @"^アプリケーションバージョン一覧の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncがRequestAdapterを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_CallsRequestAdapter()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "applicationID", _validApplicationID }
        };
        var builder = new VersionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new ListApplicationVersionsResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListApplicationVersionsResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.GetAsync();

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListApplicationVersionsResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );

        Assert.Equal(expectedResponse, result);
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^アプリケーションバージョン一覧の取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^アプリケーションバージョン一覧の取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncのパスパラメータにapplicationIDがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("applicationID", "")]
    [InlineData("applicationID", " ")]
    [InlineData("applicationID", "　")]
    public async Task PostAsync_WithNullOrEmptyPathParameters_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { pathParameterKey, pathParameterValue }
        };
        var builder = new VersionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateApplicationVersionRequest
        {
            Cpu = 100,
            Memory = 128,
            ScalingMode = ScalingMode.Cpu,
            Image = "test-image",
            RegistryPasswordAction = RegistryPasswordAction.New,
            ExposedPorts = new List<ExposedPort>(),
            Env = new List<CreateEnvironmentVariable>()
        };
        var expectedResponse = new CreateApplicationVersionResponse();

        requestAdapter.SendAsync<CreateApplicationVersionResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreateApplicationVersionResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        Assert.Equal("アプリケーションIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("アプリケーションIDを入力してください。");
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
            { "applicationID", invalidPathParameterValue }
        };
        var builder = new VersionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateApplicationVersionRequest
        {
            Cpu = 100,
            Memory = 128,
            ScalingMode = ScalingMode.Manual,
            Image = "test-image",
            RegistryPasswordAction = RegistryPasswordAction.New,
            ExposedPorts = new List<ExposedPort>(),
            Env = new List<CreateEnvironmentVariable>()
        };
        var expectedResponse = new CreateApplicationVersionResponse();

        requestAdapter.SendAsync<CreateApplicationVersionResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreateApplicationVersionResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        Assert.Equal("アプリケーションIDはGUID形式で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("アプリケーションIDはGUID形式で入力してください。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithNullRequestBody_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "applicationID", _validApplicationID }
        };
        var builder = new VersionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var expectedResponse = new CreateApplicationVersionResponse();

        requestAdapter.SendAsync<CreateApplicationVersionResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreateApplicationVersionResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

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
    [InlineData("Cpu")]
    [InlineData("Memory")]
    [InlineData("ScalingMode")]
    [InlineData("Image")]
    [InlineData("RegistryPasswordAction")]
    [InlineData("ExposedPorts")]
    [InlineData("Env")]
    [InlineData("ExposedPorts.TargetPort")]
    [InlineData("ExposedPorts.UseLetsEncrypt")]
    [InlineData("ExposedPorts.HealthCheck.Path")]
    [InlineData("ExposedPorts.HealthCheck.IntervalSeconds")]
    [InlineData("ExposedPorts.HealthCheck.TimeoutSeconds")]
    [InlineData("Env.Key")]
    [InlineData("Env.Secret")]
    public async Task PostAsync_WithNullRequestBodyParameter_ThrowsValidationException(string invalidParameterName)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "applicationID", _validApplicationID }
        };
        var builder = new VersionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateApplicationVersionRequest
        {
            Cpu = invalidParameterName == "Cpu" ? null : 100,
            Memory = invalidParameterName == "Memory" ? null : 128,
            ScalingMode = invalidParameterName == "ScalingMode" ? null : ScalingMode.Manual,
            Image = invalidParameterName == "Image" ? null : "test-image",
            RegistryPasswordAction = invalidParameterName == "RegistryPasswordAction" ? null : RegistryPasswordAction.New,
            ExposedPorts = invalidParameterName == "ExposedPorts" ? null : new List<ExposedPort>
            {
                new ExposedPort
                {
                    TargetPort = invalidParameterName == "ExposedPorts.TargetPort" ? null : 1234,
                    UseLetsEncrypt = invalidParameterName == "ExposedPorts.UseLetsEncrypt" ? null : false,
                    HealthCheck = new ExposedPort_healthCheck
                    {
                        Path = invalidParameterName == "ExposedPorts.HealthCheck.Path" ? null : "/health",
                        IntervalSeconds = invalidParameterName == "ExposedPorts.HealthCheck.IntervalSeconds" ? null : 30,
                        TimeoutSeconds = invalidParameterName == "ExposedPorts.HealthCheck.TimeoutSeconds" ? null : 5
                    }
                }
            },
            Env = invalidParameterName == "Env" ? null : new List<CreateEnvironmentVariable>
            {
                new CreateEnvironmentVariable
                {
                    Key = invalidParameterName == "Env.Key" ? null : "test-key",
                    Secret = invalidParameterName == "Env.Secret" ? null : false
                }
            }
        };
        var expectedResponse = new CreateApplicationVersionResponse();
        requestAdapter.SendAsync<CreateApplicationVersionResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreateApplicationVersionResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });
        Assert.Equal($"{invalidParameterName}が入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}が入力されていません。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのパラメータが有効な最大数値を超過する値の場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithInvalidMaximumRequestParameter_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "applicationID", _validApplicationID }
        };
        var builder = new VersionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateApplicationVersionRequest
        {
            Cpu = 100,
            Memory = 128,
            ScalingMode = ScalingMode.Manual,
            Image = "test-image",
            RegistryPasswordAction = RegistryPasswordAction.New,
            ExposedPorts = new List<ExposedPort>
            {
                new ExposedPort
                {
                    TargetPort = 1234,
                    UseLetsEncrypt = false,
                    HealthCheck = new ExposedPort_healthCheck
                    {
                        Path = "/health",
                        IntervalSeconds = 30,
                        TimeoutSeconds = 61
                    }
                }
            },
            Env = new List<CreateEnvironmentVariable>()
        };
        var expectedResponse = new CreateApplicationVersionResponse();
        requestAdapter.SendAsync<CreateApplicationVersionResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreateApplicationVersionResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        Assert.Equal("ExposedPorts.HealthCheck.TimeoutSecondsの値は60以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("ExposedPorts.HealthCheck.TimeoutSecondsの値は60以下で入力してください。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのパラメータが有効な数値範囲でない値の場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("Cpu", 99, 100, 64000)]
    [InlineData("Cpu", 64001, 100, 64000)]
    [InlineData("Memory", 127, 128, 131072)]
    [InlineData("Memory", 131073, 128, 131072)]
    [InlineData("FixedScale", 0, 1, 50)]
    [InlineData("FixedScale", 51, 1, 50)]
    [InlineData("MinScale", 0, 1, 50)]
    [InlineData("MinScale", 51, 1, 50)]
    [InlineData("MaxScale", 0, 1, 50)]
    [InlineData("MaxScale", 51, 1, 50)]
    [InlineData("ScaleInThreshold", 29, 30, 70)]
    [InlineData("ScaleInThreshold", 71, 30, 70)]
    [InlineData("ScaleOutThreshold", 49, 50, 99)]
    [InlineData("ScaleOutThreshold", 100, 50, 99)]
    [InlineData("ExposedPorts.TargetPort", 0, 1, 65535)]
    [InlineData("ExposedPorts.TargetPort", 65536, 1, 65535)]
    [InlineData("ExposedPorts.LoadBalancerPort", 0, 1, 65535)]
    [InlineData("ExposedPorts.LoadBalancerPort", 65536, 1, 65535)]
    [InlineData("ExposedPorts.HealthCheck.IntervalSeconds", 2, 3, 60)]
    [InlineData("ExposedPorts.HealthCheck.IntervalSeconds", 61, 3, 60)]
    public async Task PostAsync_WithInvalidRangeRequestBodyParameter_ThrowsValidationException(string invalidParameterName, int invalidParameterValue, int validMinimumValue, int validMaximumValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "applicationID", _validApplicationID }
        };
        var builder = new VersionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateApplicationVersionRequest
        {
            Cpu = invalidParameterName == "Cpu" ? invalidParameterValue : 100,
            Memory = invalidParameterName == "Memory" ? invalidParameterValue : 128,
            ScalingMode = ScalingMode.Manual,
            FixedScale = invalidParameterName == "FixedScale" ? invalidParameterValue : 1,
            MinScale = invalidParameterName == "MinScale" ? invalidParameterValue : 1,
            MaxScale = invalidParameterName == "MaxScale" ? invalidParameterValue : 1,
            ScaleInThreshold = invalidParameterName == "ScaleInThreshold" ? invalidParameterValue : 30,
            ScaleOutThreshold = invalidParameterName == "ScaleOutThreshold" ? invalidParameterValue : 50,
            Image = "test-image",
            RegistryPasswordAction = RegistryPasswordAction.New,
            ExposedPorts = new List<ExposedPort>
            {
                new ExposedPort
                {
                    TargetPort = invalidParameterName == "ExposedPorts.TargetPort" ? invalidParameterValue : 1234,
                    LoadBalancerPort = invalidParameterName == "ExposedPorts.LoadBalancerPort" ? invalidParameterValue : 1234,
                    UseLetsEncrypt = false,
                    HealthCheck = new ExposedPort_healthCheck
                    {
                        Path = "/health",
                        IntervalSeconds = invalidParameterName == "ExposedPorts.HealthCheck.IntervalSeconds" ? invalidParameterValue : 30,
                        TimeoutSeconds = 5
                    }
                }
            },
            Env = new List<CreateEnvironmentVariable>()
        };
        var expectedResponse = new CreateApplicationVersionResponse();
        requestAdapter.SendAsync<CreateApplicationVersionResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreateApplicationVersionResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        Assert.Equal($"{invalidParameterName}の値は{validMinimumValue}以上{validMaximumValue}以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}の値は{validMinimumValue}以上{validMaximumValue}以下で入力してください。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのパラメータの値がEnumに含まれていない場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("ScalingMode")]
    [InlineData("RegistryPasswordAction")]
    public async Task PostAsync_WithOutOfEnumParameter_ThrowsValidationException(string invalidParameterName)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "applicationID", _validApplicationID }
        };
        var builder = new VersionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateApplicationVersionRequest
        {
            Cpu = 100,
            Memory = 128,
            ScalingMode = invalidParameterName == "ScalingMode" ? (ScalingMode)99 : ScalingMode.Manual,
            Image = "test-image",
            RegistryPasswordAction = invalidParameterName == "RegistryPasswordAction" ? (RegistryPasswordAction)99 : RegistryPasswordAction.New,
            ExposedPorts = new List<ExposedPort>(),
            Env = new List<CreateEnvironmentVariable>()
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        Assert.Equal($"{invalidParameterName}はEnumに含まれる値を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}はEnumに含まれる値を入力してください。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのパラメータの値が有効な最大文字数を超過した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("Image", 513, 512)]
    [InlineData("RegistryUsername", 256, 255)]
    [InlineData("RegistryPassword", 256, 255)]
    [InlineData("ExposedPorts.HealthCheck.Path", 201, 200)]
    [InlineData("Env.Key", 256, 255)]
    [InlineData("Env.Value", 4097, 4096)]
    public async Task PostAsync_WithInvalidLengthParameter_ThrowsValidationException(string invalidParameterName, int invalidParameterValue, int? validMaxLengthValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "applicationID", _validApplicationID }
        };
        var builder = new VersionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateApplicationVersionRequest
        {
            Cpu = 100,
            Memory = 128,
            ScalingMode = invalidParameterName == "ScalingMode" ? (ScalingMode)99 : ScalingMode.Manual,
            Image = invalidParameterName == "Image" ? new string('a', invalidParameterValue) : "test-image",
            RegistryUsername = invalidParameterName == "RegistryUsername" ? new string('a', invalidParameterValue) : "test-username",
            RegistryPassword = invalidParameterName == "RegistryPassword" ? new string('a', invalidParameterValue) : "test-password",
            RegistryPasswordAction = RegistryPasswordAction.New,
            ExposedPorts = new List<ExposedPort>
            {
                new ExposedPort
                {
                    TargetPort = 1234,
                    UseLetsEncrypt = false,
                    HealthCheck = new ExposedPort_healthCheck
                    {
                        Path = invalidParameterName == "ExposedPorts.HealthCheck.Path" ? new string('a', invalidParameterValue) : "/health",
                        IntervalSeconds = 30,
                        TimeoutSeconds = 5
                    }
                }
            },
            Env = new List<CreateEnvironmentVariable>
            {
                new CreateEnvironmentVariable
                {
                    Key = invalidParameterName == "Env.Key" ? new string('a', invalidParameterValue) : "test-env-key",
                    Value = invalidParameterName == "Env.Value" ? new string('a', invalidParameterValue) : "test-env-value",
                    Secret = false
                }
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        Assert.Equal($"{invalidParameterName}は{validMaxLengthValue}文字以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}は{validMaxLengthValue}文字以下で入力してください。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのパラメータが有効な要素数でない値の場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("Cmd", 20)]
    [InlineData("ExposedPorts", 5)]
    [InlineData("ExposedPorts.Host", 5)]
    [InlineData("Env", 50)]
    public async Task PostAsync_WithInvalidElementCountRequestBodyParameter_ThrowsValidationException(string invalidParameterName, int validMaxElementCount)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "applicationID", _validApplicationID }
        };
        var builder = new VersionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateApplicationVersionRequest
        {
            Cpu = 100,
            Memory = 128,
            ScalingMode = ScalingMode.Manual,
            Image = "test-image",
            Cmd = invalidParameterName == "Cmd" ? Enumerable.Range(0, 21).Select(_ => "test-cmd").ToList() : new List<string>(),
            RegistryPasswordAction = RegistryPasswordAction.New,
            ExposedPorts = invalidParameterName == "ExposedPorts"
            ? Enumerable.Range(0, 6).Select(_ => new ExposedPort { TargetPort = 1234, UseLetsEncrypt = false }).ToList()
            : new List<ExposedPort>
            {
                new ExposedPort
                {
                    TargetPort = 1234,
                    LoadBalancerPort = 1234,
                    UseLetsEncrypt = false,
                    Host = invalidParameterName == "ExposedPorts.Host" ? Enumerable.Range(0, 6).Select(_ => "127.0.0.1").ToList() : new List<string>()
                }
            },
            Env = invalidParameterName == "Env" ? Enumerable.Range(0, 51).Select(_ => new CreateEnvironmentVariable { Key = "test-env-key", Secret = false }).ToList() : new List<CreateEnvironmentVariable>()
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);  
        });
        Assert.Equal($"{invalidParameterName}の要素数は{validMaxElementCount}以下にしてください。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}の要素数は{validMaxElementCount}以下にしてください。");
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
            { "applicationID", _validApplicationID }
        };
        var builder = new VersionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateApplicationVersionRequest
        {
            Cpu = null,
            Memory = 127,
            ScalingMode = (ScalingMode)99,
            Image = new string('a', 513),
            RegistryPasswordAction = null,
            ExposedPorts = Enumerable.Range(0, 6).Select(_ => new ExposedPort { TargetPort = 1234, UseLetsEncrypt = false }).ToList(),
            Env = new List<CreateEnvironmentVariable>()
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        var expectedMessage = """
            Cpuが入力されていません。
            ExposedPortsの要素数は5以下にしてください。
            Imageは512文字以下で入力してください。
            Memoryの値は128以上131072以下で入力してください。
            RegistryPasswordActionが入力されていません。
            ScalingModeはEnumに含まれる値を入力してください。
            """;
        Assert.Equal(expectedMessage, exception.Message);
        _mockLogger.Received(1).LogWarning("Cpuが入力されていません。");
        _mockLogger.Received(1).LogWarning("ExposedPortsの要素数は5以下にしてください。");
        _mockLogger.Received(1).LogWarning("Imageは512文字以下で入力してください。");
        _mockLogger.Received(1).LogWarning("Memoryの値は128以上131072以下で入力してください。");
        _mockLogger.Received(1).LogWarning("RegistryPasswordActionが入力されていません。");
        _mockLogger.Received(1).LogWarning("ScalingModeはEnumに含まれる値を入力してください。");
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
            { "applicationID", _validApplicationID }
        };
        var builder = new VersionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateApplicationVersionRequest
        {
            Cpu = 100,
            Memory = 128,
            ScalingMode = ScalingMode.Cpu,
            Image = "test-image",
            RegistryPasswordAction = RegistryPasswordAction.New,
            ExposedPorts = new List<ExposedPort>(),
            Env = new List<CreateEnvironmentVariable>()
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

        requestAdapter.SendAsync<CreateApplicationVersionResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreateApplicationVersionResponse>>(),
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
                @"^アプリケーションバージョン作成のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
            { "applicationID", _validApplicationID }
        };
        var builder = new VersionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateApplicationVersionRequest
        {
            Cpu = 100,
            Memory = 128,
            ScalingMode = ScalingMode.Cpu,
            Image = "test-image",
            RegistryPasswordAction = RegistryPasswordAction.New,
            ExposedPorts = new List<ExposedPort>(),
            Env = new List<CreateEnvironmentVariable>()
        };
        var expectedException = new Exception();

        requestAdapter.SendAsync<CreateApplicationVersionResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreateApplicationVersionResponse>>(),
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
                @"^アプリケーションバージョンの作成中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncがRequestAdapterを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_CallsRequestAdapter()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "applicationID", _validApplicationID }
        };
        var builder = new VersionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateApplicationVersionRequest
        {
            Cpu = 100,
            Memory = 128,
            ScalingMode = ScalingMode.Cpu,
            Image = "test-image",
            RegistryPasswordAction = RegistryPasswordAction.New,
            ExposedPorts = new List<ExposedPort>(),
            Env = new List<CreateEnvironmentVariable>()
        };
        var expectedResponse = new CreateApplicationVersionResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreateApplicationVersionResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.PostAsync(body);

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreateApplicationVersionResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );

        Assert.Equal(expectedResponse, result);
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^アプリケーションバージョンの作成を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^アプリケーションバージョンの作成が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// WithUrlが新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "applicationID", _validApplicationID }
        };
        var builder = new VersionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var newUrl = "https://api.example.com/applications/12345678-1234-1234-1234-123456789012/versions";

        var newBuilder = builder.WithUrl(newUrl);

        Assert.NotNull(newBuilder);
        Assert.IsType<VersionsRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }
}
