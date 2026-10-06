using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.AppRunShared.Applications;
using Sacloud.Sdk.AppRunShared.Applications.Item;
using Sacloud.Sdk.AppRunShared.Models;
using Sacloud.Sdk.AppRunShared.Models.Handler;
using Sacloud.Sdk.AppRunShared.Models.Request;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.AppRunShared.Tests.Applications;

/// <summary>
/// ApplicationsRequestBuilderのテスト
/// </summary>
public class ApplicationsRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<ApplicationsRequestBuilder> _mockLogger;

    public ApplicationsRequestBuilderTests()
    {
        _mockLogger = Substitute.For<ILogger<ApplicationsRequestBuilder>>();
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLoggerFactory.CreateLogger<ApplicationsRequestBuilder>().Returns(_mockLogger);
    }

    /// <summary>
    /// コンストラクタ（PathParameters版）の初期化でLoggerFactoryがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParametersAndNullLoggerFactory_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };

        Assert.Throws<ArgumentNullException>(() => new ApplicationsRequestBuilder(pathParameters, requestAdapter, null));
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）の初期化でLoggerFactoryがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrlAndNullLoggerFactory_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var rawUrl = "https://api.example.com/applications";

        Assert.Throws<ArgumentNullException>(() => new ApplicationsRequestBuilder(rawUrl, requestAdapter, null));
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
            { "baseurl", "https://api.example.com" }
        };

        var builder = new ApplicationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var rawUrl = "https://api.example.com/applications";

        var builder = new ApplicationsRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// インデクサーが正しいApplicationsItemRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_ReturnsApplicationsItemRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ApplicationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var itemBuilder = builder["test-id"];

        Assert.NotNull(itemBuilder);
        Assert.IsType<ApplicationsItemRequestBuilder>(itemBuilder);
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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ApplicationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var itemBuilder1 = builder["test-id"];
        var itemBuilder2 = builder["test-id"];

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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ApplicationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ApplicationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// ToGetRequestInformationがクエリパラメータを正しく処理することを確認
    /// </summary>
    [Theory]
    [InlineData(1, 10, "name", GetSort_orderQueryParameterType.Asc)]
    [InlineData(2, 20, "created_at", GetSort_orderQueryParameterType.Desc)]
    [InlineData(null, null, null, null)]
    public void ToGetRequestInformation_WithQueryParameters_ConfiguresCorrectly(
        int? pageNum, int? pageSize, string? sortField, GetSort_orderQueryParameterType? sortOrder)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ApplicationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation(config =>
        {
            config.QueryParameters.PageNum = pageNum;
            config.QueryParameters.PageSize = pageSize;
            config.QueryParameters.SortField = sortField;
            config.QueryParameters.SortOrder = sortOrder;
        });

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.GET, requestInfo.HttpMethod);
    }

    /// <summary>
    /// QueryParametersのSortOrderプロパティが正しく設定されることを確認
    /// </summary>
    [Theory]
    [InlineData(Sacloud.Sdk.AppRunShared.Applications.GetSort_orderQueryParameterType.Asc)]
    [InlineData(Sacloud.Sdk.AppRunShared.Applications.GetSort_orderQueryParameterType.Desc)]
    [InlineData(null)]
    public void QueryParameters_SortOrder_EnumProperty_CanBeSet(
        Sacloud.Sdk.AppRunShared.Applications.GetSort_orderQueryParameterType? sortOrder)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ApplicationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation(config =>
        {
            config.QueryParameters.SortOrder = sortOrder;
        });

        Assert.NotNull(requestInfo);
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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ApplicationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateApplicationBody { Name = "test-app" };

        var requestInfo = builder.ToPostRequestInformation(body);

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.POST, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToPostRequestInformationがnullボディで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_WithNullBody_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ApplicationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.Throws<ArgumentNullException>(() => builder.ToPostRequestInformation(null!));
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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ApplicationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateApplicationBody { Name = "test-app" };

        var requestInfo = builder.ToPostRequestInformation(body);

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// GetAsyncのリクエストパラメータのPageNumに無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public async Task GetAsync_WithInvalidPageNum_ThrowsValidationException(int? invalidPageNum)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ApplicationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync(config =>
            {
                config.QueryParameters.PageNum = invalidPageNum;
                config.QueryParameters.PageSize = 50;
                config.QueryParameters.SortField = "created_at";
                config.QueryParameters.SortOrder = GetSort_orderQueryParameterType.Desc;
            });
        });

        Assert.Equal($"page_numの値は1以上で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("page_numの値は1以上で入力してください。");
    }

    /// <summary>
    /// GetAsyncのリクエストパラメータのPageSizeに無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(101)]
    public async Task GetAsync_WithInvalidPageSize_ThrowsValidationException(int? invalidPageSize)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ApplicationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync(config =>
            {
                config.QueryParameters.PageNum = 1;
                config.QueryParameters.PageSize = invalidPageSize;
                config.QueryParameters.SortField = "created_at";
                config.QueryParameters.SortOrder = GetSort_orderQueryParameterType.Desc;
            });
        });

        Assert.Equal("page_sizeの値は1以上100以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("page_sizeの値は1以上100以下で入力してください。");
    }

    /// <summary>
    /// GetAsyncのリクエストパラメータのSortOrderに無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithInvalidSortOrder_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ApplicationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync(config =>
            {
                config.QueryParameters.PageNum = 1;
                config.QueryParameters.PageSize = 50;
                config.QueryParameters.SortField = "created_at";
                config.QueryParameters.SortOrder = (GetSort_orderQueryParameterType)999;
            });
        });

        Assert.Equal("sort_orderはEnumに含まれる値を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("sort_orderはEnumに含まれる値を入力してください。");
    }

    /// <summary>
    /// GetAsyncの複数のリクエストパラメータに無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithMultipleInvalidParams_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ApplicationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync(config =>
            {
                config.QueryParameters.PageNum = 0;
                config.QueryParameters.PageSize = 0;
                config.QueryParameters.SortField = "created_at";
                config.QueryParameters.SortOrder = GetSort_orderQueryParameterType.Desc;
            });
        });

        var expectedMessage = """
            page_numの値は1以上で入力してください。
            page_sizeの値は1以上100以下で入力してください。
            """;
        Assert.Equal(expectedMessage.ReplaceLineEndings(), exception.Message);
        _mockLogger.Received(1).LogWarning("page_numの値は1以上で入力してください。");
        _mockLogger.Received(1).LogWarning("page_sizeの値は1以上100以下で入力してください。");
    }

    /// <summary>
    /// GetAsyncが有効なクエリパラメータ付きでrequestConfiguration経由のバリデーションを通過できることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithValidQueryParameters_CallsRequestAdapterSendAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ApplicationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new ListApplications();

        requestAdapter.SendAsync<ListApplications>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListApplications>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.GetAsync(config =>
        {
            config.QueryParameters.PageNum = 1;
            config.QueryParameters.PageSize = 50;
            config.QueryParameters.SortField = "created_at";
            config.QueryParameters.SortOrder = GetSort_orderQueryParameterType.Desc;
        });

        await requestAdapter.Received(1).SendAsync<ListApplications>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListApplications>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
        Assert.Equal(expectedResponse, result);
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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ApplicationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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

        requestAdapter.SendAsync<ListApplications>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListApplications>>(),
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
                @"^アプリケーション一覧取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ApplicationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync<ListApplications>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListApplications>>(),
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
                @"^アプリケーション一覧の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncがRequestAdapter.SendAsyncを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_CallsRequestAdapterSendAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ApplicationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new ListApplications();

        requestAdapter.SendAsync<ListApplications>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListApplications>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.GetAsync();

        await requestAdapter.Received(1).SendAsync<ListApplications>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListApplications>>(),
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
                    @"^アプリケーション一覧の取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^アプリケーション一覧の取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ApplicationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
    [InlineData("TimeoutSeconds")]
    [InlineData("Port")]
    [InlineData("MinScale")]
    [InlineData("MaxScale")]
    [InlineData("Components")]
    [InlineData("Components.Name")]
    [InlineData("Components.MaxCpu")]
    [InlineData("Components.MaxMemory")]
    [InlineData("Components.DeploySource")]
    [InlineData("Components.DeploySource.ContainerRegistry.Image")]
    [InlineData("Components.Env.Key")]
    [InlineData("Components.Env.Value")]
    [InlineData("Components.Secret.Key")]
    [InlineData("Components.Secret.Value")]
    [InlineData("Components.Probe.HttpGet.Path")]
    [InlineData("Components.Probe.HttpGet.Port")]
    public async Task PostAsync_WithNullParameter_ThrowsValidationException(string invalidParameterName)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ApplicationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var components = invalidParameterName == "Components" ? null : new List<CreateApplicationBody_components>()
        {
            new CreateApplicationBody_components()
            {
                DeploySource = invalidParameterName == "Components.DeploySource" ? null : new CreateApplicationBody_components_deploy_source()
                {
                    ContainerRegistry = new CreateApplicationBody_components_deploy_source_container_registry()
                    {
                        Image = invalidParameterName == "Components.DeploySource.ContainerRegistry.Image" ? null : "test-image",
                        Password = "test-password",
                        Server = "test-server",
                        Username = "test-username"
                    }
                },
                Env = new List<EnvItem>()
                {
                    new EnvItem()
                    {
                        Key = invalidParameterName == "Components.Env.Key" ? null : "test-env-key",
                        Value = invalidParameterName == "Components.Env.Value" ? null : "test-env-value"
                    }
                },
                Secret = new List<CreateApplicationBody_components_secret>
                {
                    new CreateApplicationBody_components_secret
                    {
                        Key = invalidParameterName == "Components.Secret.Key" ? null : "test-secret-key",
                        Value = invalidParameterName == "Components.Secret.Value" ? null : "test-secret-value"
                    }
                },
                MaxCpu = invalidParameterName == "Components.MaxCpu" ? null : new CreateApplicationBody_components_max_cpu(),
                MaxMemory = invalidParameterName == "Components.MaxMemory" ? null : new CreateApplicationBody_components_max_memory(),
                Name = invalidParameterName == "Components.Name" ? null : "test-component-name",
                Probe = new CreateApplicationBody_components_probe()
                {
                    HttpGet = new CreateApplicationBody_components_probe_http_get()
                    {
                        Path = invalidParameterName == "Components.Probe.HttpGet.Path" ? null : "/health",
                        Port = invalidParameterName == "Components.Probe.HttpGet.Port" ? null : 1234,
                        Headers = new List<CreateApplicationBody_components_probe_http_get_headers>()
                        {
                            new CreateApplicationBody_components_probe_http_get_headers()
                            {
                                Name = invalidParameterName == "Components.Probe.HttpGet.Headers.Name" ? null : "test-header-name",
                                Value = invalidParameterName == "Components.Probe.HttpGet.Headers.Value" ? null : "test-header-value"
                            }
                        }
                    }
                }
            }
        };
        var body = new CreateApplicationBody
        {
            Components = components,
            MaxScale = invalidParameterName == "MaxScale" ? null : 10,
            MinScale = invalidParameterName == "MinScale" ? null : 0,
            Name = invalidParameterName == "Name" ? null : "test-app",
            Port = invalidParameterName == "Port" ? null : 1234,
            ScaleTargetConcurrency = invalidParameterName == "ScaleTargetConcurrency" ? null : 100,
            TimeoutSeconds = invalidParameterName == "TimeoutSeconds" ? null : 60
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        Assert.Equal($"{invalidParameterName}が入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}が入力されていません。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのパラメータが有効な数値範囲でない値の場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("Components.Probe.HttpGet.Port", 0, 1, 65535)]
    [InlineData("MaxScale",-1, 1, 10)]
    [InlineData("MaxScale", 0, 1, 10)]
    [InlineData("MaxScale", 11, 1, 10)]
    [InlineData("MinScale", -1, 0, 10)]
    [InlineData("MinScale", 11, 0, 10)]
    [InlineData("Port", -1, 1, 65535)]
    [InlineData("Port", 0, 1, 65535)]
    [InlineData("Port", 65536, 1, 65535)]
    [InlineData("ScaleTargetConcurrency", -1, 50, 200)]
    [InlineData("ScaleTargetConcurrency", 49, 50, 200)]
    [InlineData("ScaleTargetConcurrency", 201, 50, 200)]
    [InlineData("TimeoutSeconds", -1, 1, 300)]
    [InlineData("TimeoutSeconds", 0, 1, 300)]
    [InlineData("TimeoutSeconds", 301, 1, 300)]
    public async Task PostAsync_WithOutOfRangeParameter_ThrowsValidationException(string invalidParameterName, int invalidParameterValue, int validMinimumValue, int validMaximumValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ApplicationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var components = new List<CreateApplicationBody_components>()
        {
            new CreateApplicationBody_components()
            {
                DeploySource = new CreateApplicationBody_components_deploy_source()
                {
                    ContainerRegistry = new CreateApplicationBody_components_deploy_source_container_registry()
                    {
                        Image = "test-image",
                        Password = "test-password",
                        Server = "test-server",
                        Username = "test-username"
                    }
                },
                MaxCpu = new CreateApplicationBody_components_max_cpu(),
                MaxMemory = new CreateApplicationBody_components_max_memory(),
                Name = "test-component-name",
                Probe = new CreateApplicationBody_components_probe()
                {
                    HttpGet = new CreateApplicationBody_components_probe_http_get()
                    {
                        Path = "/health",
                        Port = invalidParameterName == "Components.Probe.HttpGet.Port" ? invalidParameterValue : 1234
                    }
                }
            }
        };
        var body = new CreateApplicationBody
        {
            Components = components,
            MaxScale = invalidParameterName == "MaxScale" ? invalidParameterValue : 10,
            MinScale = invalidParameterName == "MinScale" ? invalidParameterValue : 0,
            Name = "test-app",
            Port = invalidParameterName == "Port" ? invalidParameterValue : 1234,
            ScaleTargetConcurrency = invalidParameterName == "ScaleTargetConcurrency" ? invalidParameterValue : 100,
            TimeoutSeconds = invalidParameterName == "TimeoutSeconds" ? invalidParameterValue : 60
        };

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
    [InlineData("Components.MaxCpu")]
    [InlineData("Components.MaxMemory")]
    public async Task PostAsync_WithOutOfEnumParameter_ThrowsValidationException(string invalidParameterName)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ApplicationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var components = new List<CreateApplicationBody_components>()
        {
            new CreateApplicationBody_components()
            {
                DeploySource = new CreateApplicationBody_components_deploy_source()
                {
                    ContainerRegistry = new CreateApplicationBody_components_deploy_source_container_registry()
                    {
                        Image = "test-image",
                        Password = "test-password",
                        Server = "test-server",
                        Username = "test-username"
                    }
                },
                MaxCpu = invalidParameterName == "Components.MaxCpu" ? (CreateApplicationBody_components_max_cpu)999 : new CreateApplicationBody_components_max_cpu(),
                MaxMemory = invalidParameterName == "Components.MaxMemory" ? (CreateApplicationBody_components_max_memory)999 : new CreateApplicationBody_components_max_memory(),
                Name = "test-name",
                Probe = new CreateApplicationBody_components_probe()
                {
                    HttpGet = new CreateApplicationBody_components_probe_http_get()
                    {
                        Path = "/health",
                        Port = 1234
                    }
                }
            }
        };
        var body = new CreateApplicationBody
        {
            Components = components,
            MaxScale = 10,
            MinScale = 0,
            Name = "test-name",
            Port = 1234,
            ScaleTargetConcurrency = 100,
            TimeoutSeconds = 60
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        Assert.Equal($"{invalidParameterName}はEnumに含まれる値を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}はEnumに含まれる値を入力してください。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディの文字列パラメータが有効な文字数範囲を外れた場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("Components.DeploySource.ContainerRegistry.Password", "", 1, 63)]
    [InlineData("Components.DeploySource.ContainerRegistry.Password", 64, 1, 63)]
    [InlineData("Components.DeploySource.ContainerRegistry.Server", "", 1, 128)]
    [InlineData("Components.DeploySource.ContainerRegistry.Server", 129, 1, 128)]
    [InlineData("Components.DeploySource.ContainerRegistry.Username", "", 1, 63)]
    [InlineData("Components.DeploySource.ContainerRegistry.Username", 64, 1, 63)]
    [InlineData("Components.DeploySource.ContainerRegistry.Image", 129, 1, 128)]
    [InlineData("Components.Name", 256, 1, 255)]
    [InlineData("Name", 256, 1, 255)]
    public async Task PostAsync_WithInvalidLengthParameter_ThrowsValidationException(string invalidParameterName, object invalidParameterValue, int validMinLengthValue, int validMaxLengthValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ApplicationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var invalidStringValue = invalidParameterValue is int invalidLength
            ? new string('a', invalidLength)
            : (string)invalidParameterValue;
        var components = new List<CreateApplicationBody_components>()
        {
            new CreateApplicationBody_components()
            {
                DeploySource = new CreateApplicationBody_components_deploy_source()
                {
                    ContainerRegistry = new CreateApplicationBody_components_deploy_source_container_registry()
                    {
                        Image = invalidParameterName == "Components.DeploySource.ContainerRegistry.Image" ? invalidStringValue : "test-image",
                        Password = invalidParameterName == "Components.DeploySource.ContainerRegistry.Password" ? invalidStringValue : "test-password",
                        Server = invalidParameterName == "Components.DeploySource.ContainerRegistry.Server" ? invalidStringValue : "test-server",
                        Username = invalidParameterName == "Components.DeploySource.ContainerRegistry.Username" ? invalidStringValue : "test-username"
                    }
                },
                MaxCpu = new CreateApplicationBody_components_max_cpu(),
                MaxMemory = new CreateApplicationBody_components_max_memory(),
                Name = invalidParameterName == "Components.Name" ? invalidStringValue : "test-component-name",
                Probe = new CreateApplicationBody_components_probe()
                {
                    HttpGet = new CreateApplicationBody_components_probe_http_get()
                    {
                        Path = "/health",
                        Port = 1234
                    }
                }
            }
        };
        var body = new CreateApplicationBody
        {
            Components = components,
            MaxScale = 10,
            MinScale = 0,
            Name = invalidParameterName == "Name" ? invalidStringValue : "test-app",
            Port = 1234,
            ScaleTargetConcurrency = 100,
            TimeoutSeconds = 60
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        var expectedMessage = $"{invalidParameterName}は{validMinLengthValue}文字以上{validMaxLengthValue}文字以下で入力してください。";
        Assert.Equal(expectedMessage, exception.Message);
        _mockLogger.Received(1).LogWarning(expectedMessage);
    }

    /// <summary>
    /// PostAsyncのリクエストボディのパラメータの値が許可されていない場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("Port", 8012)]
    [InlineData("Port", 8013)]
    [InlineData("Port", 8022)]
    [InlineData("Port", 9090)]
    [InlineData("Port", 9091)]
    [InlineData("Port", 8008)]
    [InlineData("Components.Env.Key", "K_SERVICE")]
    [InlineData("Components.Env.Key", "K_CONFIGURATION")]
    [InlineData("Components.Env.Key", "K_REVISION")]
    [InlineData("Components.Env.Key", "PORT")]
    [InlineData("Components.Secret.Key", "K_SERVICE")]
    [InlineData("Components.Secret.Key", "K_CONFIGURATION")]
    [InlineData("Components.Secret.Key", "K_REVISION")]
    [InlineData("Components.Secret.Key", "PORT")]
    [InlineData("Components.Probe.HttpGet.Port", 8012)]
    [InlineData("Components.Probe.HttpGet.Port", 8013)]
    [InlineData("Components.Probe.HttpGet.Port", 8022)]
    [InlineData("Components.Probe.HttpGet.Port", 9090)]
    [InlineData("Components.Probe.HttpGet.Port", 9091)]
    [InlineData("Components.Probe.HttpGet.Port", 8008)]
    public async Task PostAsync_WithDeniedValue_ThrowsValidationException(string invalidParameterName, object invalidParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ApplicationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var components = new List<CreateApplicationBody_components>()
        {
            new CreateApplicationBody_components()
            {
                DeploySource = new CreateApplicationBody_components_deploy_source()
                {
                    ContainerRegistry = new CreateApplicationBody_components_deploy_source_container_registry()
                    {
                        Image = "test-image",
                        Password = "test-password",
                        Server = "test-server",
                        Username = "test-username"
                    }
                },
                Env = new List<EnvItem>()
                {
                    new EnvItem()
                    {
                        Key = invalidParameterName == "Components.Env.Key" ? (string)invalidParameterValue : "test-env-key",
                        Value = "test-env-value"
                    }
                },
                Secret = new List<CreateApplicationBody_components_secret>()
                {
                    new CreateApplicationBody_components_secret()
                    {
                        Key = invalidParameterName == "Components.Secret.Key" ? (string)invalidParameterValue : "test-secret-key",
                        Value = "test-secret-value"
                    }
                },
                MaxCpu = new CreateApplicationBody_components_max_cpu(),
                MaxMemory = new CreateApplicationBody_components_max_memory(),
                Name = "test-name",
                Probe = new CreateApplicationBody_components_probe()
                {
                    HttpGet = new CreateApplicationBody_components_probe_http_get()
                    {
                        Path = "/health",
                        Port = invalidParameterName == "Components.Probe.HttpGet.Port" ? (int)invalidParameterValue : 1234
                    }
                }
            }
        };
        var body = new CreateApplicationBody
        {
            Components = components,
            MaxScale = 10,
            MinScale = 0,
            Name = "test-name",
            Port = invalidParameterName == "Port" ? (int)invalidParameterValue : 1234,
            ScaleTargetConcurrency = 100,
            TimeoutSeconds = 60
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        Assert.Equal($"{invalidParameterName}には許可された値を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}には許可された値を入力してください。");
    }

    /// <summary>
    /// PostAsyncでCloudctrlErrorによるAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithCloudctrlError_ThrowsApiException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ApplicationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateApplicationBody
        {
            Components = new List<CreateApplicationBody_components>()
            {
                new CreateApplicationBody_components()
                {
                    DeploySource = new CreateApplicationBody_components_deploy_source()
                    {
                        ContainerRegistry = new CreateApplicationBody_components_deploy_source_container_registry()
                        {
                            Image = "test-image",
                            Password = "test-password",
                            Server = "test-server",
                            Username = "test-username"
                        }
                    },
                    MaxCpu = new CreateApplicationBody_components_max_cpu(),
                    MaxMemory = new CreateApplicationBody_components_max_memory(),
                    Name = "test-component-name",
                    Probe = new CreateApplicationBody_components_probe()
                    {
                        HttpGet = new CreateApplicationBody_components_probe_http_get()
                        {
                            Path = "/health",
                            Port = 1234
                        }
                    }
                }
            },
            MaxScale = 10,
            MinScale = 1,
            Name = "test-app",
            Port = 1234,
            ScaleTargetConcurrency = 50,
            TimeoutSeconds = 60
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

        requestAdapter.SendAsync<CreateApplication>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreateApplication>>(),
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
                @"^アプリケーション作成のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ApplicationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateApplicationBody
        {
            Components = new List<CreateApplicationBody_components>()
            {
                new CreateApplicationBody_components()
                {
                    DeploySource = new CreateApplicationBody_components_deploy_source()
                    {
                        ContainerRegistry = new CreateApplicationBody_components_deploy_source_container_registry()
                        {
                            Image = "test-image",
                            Password = "test-password",
                            Server = "test-server",
                            Username = "test-username"
                        }
                    },
                    MaxCpu = new CreateApplicationBody_components_max_cpu(),
                    MaxMemory = new CreateApplicationBody_components_max_memory(),
                    Name = "test-component-name",
                    Probe = new CreateApplicationBody_components_probe()
                    {
                        HttpGet = new CreateApplicationBody_components_probe_http_get()
                        {
                            Path = "/health",
                            Port = 1234
                        }
                    }
                }
            },
            MaxScale = 10,
            MinScale = 1,
            Name = "test-app",
            Port = 1234,
            ScaleTargetConcurrency = 50,
            TimeoutSeconds = 60
        };
        var expectedException = new Exception();

        requestAdapter.SendAsync<CreateApplication>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreateApplication>>(),
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
                @"^アプリケーションの作成中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncがRequestAdapter.SendAsyncを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_CallsRequestAdapterSendAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ApplicationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateApplicationBody
        {
            Components = new List<CreateApplicationBody_components>()
            {
                new CreateApplicationBody_components()
                {
                    DeploySource = new CreateApplicationBody_components_deploy_source()
                    {
                        ContainerRegistry = new CreateApplicationBody_components_deploy_source_container_registry()
                        {
                            Image = "test-image",
                            Password = "test-password",
                            Server = "test-server",
                            Username = "test-username"
                        }
                    },
                    MaxCpu = new CreateApplicationBody_components_max_cpu(),
                    MaxMemory = new CreateApplicationBody_components_max_memory(),
                    Name = "test-component-name",
                    Probe = new CreateApplicationBody_components_probe()
                    {
                        HttpGet = new CreateApplicationBody_components_probe_http_get()
                        {
                            Path = "/health",
                            Port = 1234
                        }
                    }
                }
            },
            MaxScale = 10,
            MinScale = 1,
            Name = "test-app",
            Port = 1234,
            ScaleTargetConcurrency = 50,
            TimeoutSeconds = 60
        };
        var expectedResponse = new CreateApplication();

        requestAdapter.SendAsync<CreateApplication>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreateApplication>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.PostAsync(body);

        await requestAdapter.Received(1).SendAsync<CreateApplication>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreateApplication>>(),
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
                    @"^アプリケーションの作成を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^アプリケーションの作成が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 201\]$")),
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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ApplicationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var newUrl = "https://newapi.example.com/applications";

        var newBuilder = builder.WithUrl(newUrl);

        Assert.NotNull(newBuilder);
        Assert.NotSame(builder, newBuilder);
        Assert.IsType<ApplicationsRequestBuilder>(newBuilder);
    }
}
