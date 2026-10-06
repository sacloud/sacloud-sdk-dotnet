using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.Shared.Models;
using Sacloud.Sdk.Workflows.Models;
using Sacloud.Sdk.Workflows.Workflows.Suggest;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.Workflows.Tests.Workflows.Suggest;

/// <summary>
/// SuggestRequestBuilderのテスト
/// </summary>
public class SuggestRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<SuggestRequestBuilder> _mockLogger;

    public SuggestRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<SuggestRequestBuilder>>();
        _mockLoggerFactory.CreateLogger<SuggestRequestBuilder>().Returns(_mockLogger);
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

        var builder = new SuggestRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var rawUrl = "https://api.example.com/workflows/suggest";

        var builder = new SuggestRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
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
        var builder = new SuggestRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var builder = new SuggestRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.True(requestInfo.Headers.TryGetValue("Accept", out var acceptHeader));
        Assert.Contains("application/json", acceptHeader);
    }

    /// <summary>
    /// WithUrlが正しい新しいインスタンスを返すことを確認
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
        var builder = new SuggestRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var rawUrl = "https://api.example.com/workflows/suggest";

        var newBuilder = builder.WithUrl(rawUrl);

        Assert.NotNull(newBuilder);
        Assert.IsType<SuggestRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }

    /// <summary>
    /// GetAsyncのリクエストパラメータのPageに無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public async Task GetAsync_WithInvalidPage_ThrowsValidationException(int? invalidPage)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new SuggestRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync(config =>
            {
                config.QueryParameters.Name = "test-name";
                config.QueryParameters.Page = invalidPage;
            });
        });

        Assert.Equal($"Pageの値は1以上で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("Pageの値は1以上で入力してください。");
    }

    /// <summary>
    /// GetAsyncのリクエストパラメータのPageLimitに無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(-1)]
    [InlineData(4)]
    [InlineData(501)]
    public async Task GetAsync_WithInvalidPageLimit_ThrowsValidationException(int? invalidPageLimit)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new SuggestRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync(config =>
            {
                config.QueryParameters.Name = "test-name";
                config.QueryParameters.PageLimit = invalidPageLimit;
            });
        });

        Assert.Equal("PageLimitの値は5以上500以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("PageLimitの値は5以上500以下で入力してください。");
    }

    /// <summary>
    /// GetAsyncのリクエストパラメータのSortByに無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithInvalidSortBy_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new SuggestRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync(config =>
            {
                config.QueryParameters.Name = "test-name";
                config.QueryParameters.SortBy = (SortBy)999;
            });
        });

        Assert.Equal("SortByはEnumに含まれる値を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("SortByはEnumに含まれる値を入力してください。");
    }

    /// <summary>
    /// GetAsyncのリクエストパラメータのOrderに無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithInvalidOrder_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new SuggestRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync(config =>
            {
                config.QueryParameters.Name = "test-name";
                config.QueryParameters.Order = (Order)999;
            });
        });

        Assert.Equal("OrderはEnumに含まれる値を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("OrderはEnumに含まれる値を入力してください。");
    }

    /// <summary>
    /// GetAsyncのリクエストパラメータのNameに有効な最大文字数より多い文字数の値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithInvalidMaxLengthName_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new SuggestRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync(config =>
            {
                config.QueryParameters.Name = "12345678901234567890123456789012345678901234567890123456789012345";
            });
        });

        Assert.Equal("Nameは64文字以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("Nameは64文字以下で入力してください。");
    }

    /// <summary>
    /// GetAsyncのリクエストパラメータのNameに無効な正規表現パターンの値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalid+name")]
    [InlineData("invalid&name")]
    public async Task GetAsync_WithInvalidRegexPatternName_ThrowsValidationException(string invalidName)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new SuggestRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync(config =>
            {
                config.QueryParameters.Name = invalidName;
            });
        });

        Assert.Equal("Nameの形式が正しくありません。", exception.Message);
        _mockLogger.Received(1).LogWarning("Nameの形式が正しくありません。");
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
        var builder = new SuggestRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync(config =>
            {
                config.QueryParameters.Page = 0;
                config.QueryParameters.PageLimit = 4;
                config.QueryParameters.SortBy = (SortBy)999;
                config.QueryParameters.Order = (Order)999;
                config.QueryParameters.Name = "invalid@name";
            });
        });

        var expectedMessage = """
            Nameの形式が正しくありません。
            OrderはEnumに含まれる値を入力してください。
            Pageの値は1以上で入力してください。
            PageLimitの値は5以上500以下で入力してください。
            SortByはEnumに含まれる値を入力してください。
            """;
        Assert.Equal(expectedMessage.ReplaceLineEndings(), exception.Message);
        _mockLogger.Received(1).LogWarning("Nameの形式が正しくありません。");
        _mockLogger.Received(1).LogWarning("OrderはEnumに含まれる値を入力してください。");
        _mockLogger.Received(1).LogWarning("Pageの値は1以上で入力してください。");
        _mockLogger.Received(1).LogWarning("PageLimitの値は5以上500以下で入力してください。");
        _mockLogger.Received(1).LogWarning("SortByはEnumに含まれる値を入力してください。");
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
        var builder = new SuggestRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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

        requestAdapter.SendAsync<ListWorkflowSuggestResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListWorkflowSuggestResponse>>(),
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
                @"^ワークフローのサジェスト取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var builder = new SuggestRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync<ListWorkflowSuggestResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListWorkflowSuggestResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<Exception>(async () => await builder.GetAsync());

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^ワークフローのサジェストの取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var builder = new SuggestRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new ListWorkflowSuggestResponse();

        requestAdapter.SendAsync<ListWorkflowSuggestResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListWorkflowSuggestResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(expectedResponse);

        var result = await builder.GetAsync();

        await requestAdapter.Received(1).SendAsync<ListWorkflowSuggestResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListWorkflowSuggestResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());
        Assert.Equal(expectedResponse, result);

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^ワークフローのサジェストの取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^ワークフローのサジェストの取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// SuggestRequestBuilderGetQueryParameters.Nameプロパティセッターが正しく動作することを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Name_CanBeSet()
    {
        var queryParams = new SuggestRequestBuilder.SuggestRequestBuilderGetQueryParameters
        {
            Name = "test-suggest"
        };

        Assert.Equal("test-suggest", queryParams.Name);
    }

    /// <summary>
    /// SuggestRequestBuilderGetQueryParameters.Orderプロパティセッターが正しく動作することを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Order_CanBeSet()
    {
        var queryParams = new SuggestRequestBuilder.SuggestRequestBuilderGetQueryParameters
        {
            Order = Order.Desc
        };

        Assert.Equal(Order.Desc, queryParams.Order);
    }

    /// <summary>
    /// SuggestRequestBuilderGetQueryParameters.Pageプロパティセッターが正しく動作することを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Page_CanBeSet()
    {
        var queryParams = new SuggestRequestBuilder.SuggestRequestBuilderGetQueryParameters
        {
            Page = 3
        };

        Assert.Equal(3, queryParams.Page);
    }

    /// <summary>
    /// SuggestRequestBuilderGetQueryParameters.PageLimitプロパティセッターが正しく動作することを確認
    /// </summary>
    [Fact]
    public void QueryParameters_PageLimit_CanBeSet()
    {
        var queryParams = new SuggestRequestBuilder.SuggestRequestBuilderGetQueryParameters
        {
            PageLimit = 100
        };

        Assert.Equal(100, queryParams.PageLimit);
    }

    /// <summary>
    /// SuggestRequestBuilderGetQueryParameters.SortByプロパティセッターが正しく動作することを確認
    /// </summary>
    [Fact]
    public void QueryParameters_SortBy_CanBeSet()
    {
        var queryParams = new SuggestRequestBuilder.SuggestRequestBuilderGetQueryParameters
        {
            SortBy = SortBy.CreatedAt
        };

        Assert.Equal(SortBy.CreatedAt, queryParams.SortBy);
    }
}
