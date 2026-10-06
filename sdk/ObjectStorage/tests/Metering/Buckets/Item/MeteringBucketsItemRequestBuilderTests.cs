using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Metering.Buckets.Item;
using Sacloud.Sdk.ObjectStorage.Models;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.ObjectStorage.Tests.Metering.Buckets.Item;

/// <summary>
/// WithNameItemRequestBuilder（Metering.Buckets.Item）のテスト
/// </summary>
public class MeteringBucketsItemRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<WithNameItemRequestBuilder> _mockLogger;

    public MeteringBucketsItemRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<WithNameItemRequestBuilder>>();
        _mockLoggerFactory.CreateLogger(Arg.Any<string>()).Returns(_mockLogger);
    }

    /// <summary>
    /// コンストラクタ（pathParameters版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };

        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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

        var builder = new WithNameItemRequestBuilder("https://api.example.com/metering/buckets/mybucket", requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// ToGetRequestInformationが正しいRequestInformationを返すことを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_ReturnsRequestInformation()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.GET, requestInfo.HttpMethod);
    }

    /// <summary>
    /// QueryParametersのFromプロパティが正しく設定できることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_From_CanBeSet()
    {
        var queryParams = new WithNameItemRequestBuilder.WithNameItemRequestBuilderGetQueryParameters();
        var month = "2024-01";

        queryParams.From = month;

        Assert.Equal(month, queryParams.From);
    }

    /// <summary>
    /// QueryParametersのToプロパティが正しく設定できることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_To_CanBeSet()
    {
        var queryParams = new WithNameItemRequestBuilder.WithNameItemRequestBuilderGetQueryParameters();
        var month = "2024-12";

        queryParams.To = month;

        Assert.Equal(month, queryParams.To);
    }

    /// <summary>
    /// WithUrlが正しいWithNameItemRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var newBuilder = builder.WithUrl("https://api.example.com/metering/buckets/otherbucket");

        Assert.NotNull(newBuilder);
        Assert.IsType<WithNameItemRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
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
            { "name", "mybucket" }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListBucketMeteringResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(new ListBucketMeteringResponse());

        var result = await builder.GetAsync(config =>
        {
            config.QueryParameters.From = "2025-01";
            config.QueryParameters.To = "2025-02";
        });

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListBucketMeteringResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// GetAsyncでハイフンを含むバケット名が受け入れられることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithHyphenatedBucketName_CallsRequestAdapter()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "myb-ucket" }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListBucketMeteringResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(new ListBucketMeteringResponse());

        var result = await builder.GetAsync(config =>
        {
            config.QueryParameters.From = "2025-01";
            config.QueryParameters.To = "2025-02";
        });

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListBucketMeteringResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// GetAsyncが"XXX"キーにApiErrorのエラーマッピングを使用することを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_SendsWithXxxErrorMapping()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListBucketMeteringResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(new ListBucketMeteringResponse());

        await builder.GetAsync(config =>
        {
            config.QueryParameters.From = "2025-01";
            config.QueryParameters.To = "2025-02";
        });

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListBucketMeteringResponse>>(),
            Arg.Is<Dictionary<string, ParsableFactory<IParsable>>>(x => HasApiErrorMapping(x)),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// GetAsyncでApiErrorがスローされた場合に再スローされることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithApiError_ThrowsApiError()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListBucketMeteringResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<ListBucketMeteringResponse?>(expectedException));

        await Assert.ThrowsAsync<ApiError>(() => builder.GetAsync(config =>
        {
            config.QueryParameters.From = "2025-01";
            config.QueryParameters.To = "2025-02";
        }));
    }

    /// <summary>
    /// GetAsyncでその他の例外がスローされた場合に再スローされることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithOtherException_RethrowsException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new InvalidOperationException("test exception");
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListBucketMeteringResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<ListBucketMeteringResponse?>(expectedException));

        await Assert.ThrowsAsync<InvalidOperationException>(() => builder.GetAsync(config =>
        {
            config.QueryParameters.From = "2025-01";
            config.QueryParameters.To = "2025-02";
        }));
    }

    /// <summary>
    /// GetAsyncのfromがnull、空文字、または空白文字の場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("　")]
    public async Task GetAsync_WithNullOrEmptyOrWhitespaceFrom_ThrowsValidationException(string? invalidFrom)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync(config =>
            {
                config.QueryParameters.From = invalidFrom;
                config.QueryParameters.To = "2025-01";
            });
        });

        Assert.Equal("fromが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("fromが入力されていません。");
    }

    /// <summary>
    /// GetAsyncのtoがnull、空文字、または空白文字の場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("　")]
    public async Task GetAsync_WithNullOrEmptyOrWhitespaceTo_ThrowsValidationException(string? invalidTo)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync(config =>
            {
                config.QueryParameters.From = "2025-01";
                config.QueryParameters.To = invalidTo;
            });
        });

        Assert.Equal("toが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("toが入力されていません。");
    }

    /// <summary>
    /// GetAsyncで必須クエリパラメータを指定しない場合にValidationExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithoutRequestConfiguration_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync());

        Assert.Contains("fromが入力されていません。", exception.Message);
        Assert.Contains("toが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("fromが入力されていません。");
        _mockLogger.Received(1).LogWarning("toが入力されていません。");
    }

    /// <summary>
    /// GetAsync成功時に開始・完了ログが出力されることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_Success_LogsInformation()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListBucketMeteringResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(new ListBucketMeteringResponse());

        await builder.GetAsync(config =>
        {
            config.QueryParameters.From = "2025-01";
            config.QueryParameters.To = "2025-02";
        });

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^バケット使用量の取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^バケット使用量の取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, \d+\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncでApiErrorがスローされた場合にAPIエラーログが出力されることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithApiError_LogsError()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListBucketMeteringResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<ListBucketMeteringResponse?>(expectedException));

        var exception = await Assert.ThrowsAsync<ApiError>(() => builder.GetAsync(config =>
        {
            config.QueryParameters.From = "2025-01";
            config.QueryParameters.To = "2025-02";
        }));
        Assert.Same(expectedException, exception);

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^バケット使用量取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z .*$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncでその他の例外がスローされた場合にエラーログが出力されることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithOtherException_LogsError()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new InvalidOperationException("test exception");
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListBucketMeteringResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<ListBucketMeteringResponse?>(expectedException));

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => builder.GetAsync(config =>
        {
            config.QueryParameters.From = "2025-01";
            config.QueryParameters.To = "2025-02";
        }));
        Assert.Same(expectedException, exception);

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^バケット使用量の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }
    
    /// <summary>
    /// GetAsyncのリクエストパスパラメータのnameがnull、空文字、または空白文字の場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("　")]
    [InlineData(null)]
    public async Task GetAsync_WithNullOrWhitespacePathParameters_ThrowsValidationException(string? invalidName)
    {
        var pathParameters = new Dictionary<string, object?>
        {
            { "baseurl", "https://api.example.com" },
            { "name", invalidName }
        };
        var expectedMessage = "バケット名を入力してください。";
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync());

        Assert.Equal(expectedMessage, exception.Message);
        _mockLogger.Received(1).LogWarning(expectedMessage);
    }

    /// <summary>
    /// GetAsyncのリクエストパスパラメータのnameが不正な場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("-ab")]
    [InlineData("Ａbc")]
    [InlineData("あab")]
    [InlineData(" ab")]
    [InlineData("　ab")]
    public async Task GetAsync_WithInvalidPathParameters_ThrowsValidationException_AdditionalCases(string invalidName)
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", invalidName }
        };
        var expectedMessage = "バケット名の形式が正しくありません。";
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync());

        Assert.Equal(expectedMessage, exception.Message);
        _mockLogger.Received(1).LogWarning(expectedMessage);
    }

    /// <summary>
    /// GetAsyncのリクエストパスパラメータのnameの長さが許容範囲外の場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(2, "バケット名は3文字以上で入力してください。")]
    [InlineData(65, "バケット名は64文字以下で入力してください。")]
    public async Task GetAsync_WithBucketNameOutsideLengthRange_ThrowsValidationException(int nameLength, string expectedMessage)
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", new string('a', nameLength) }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync());

        Assert.Equal(expectedMessage, exception.Message);
        _mockLogger.Received(1).LogWarning(expectedMessage);
    }

    /// <summary>
    /// GetAsyncで3文字または64文字のバケット名が受け入れられることを確認
    /// </summary>
    [Theory]
    [InlineData(3)]
    [InlineData(64)]
    public async Task GetAsync_WithBoundaryLengthBucketName_CallsRequestAdapter(int nameLength)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListBucketMeteringResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(new ListBucketMeteringResponse());
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", new string('a', nameLength) }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var result = await builder.GetAsync(config =>
        {
            config.QueryParameters.From = "2025-01";
            config.QueryParameters.To = "2025-02";
        });

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListBucketMeteringResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// <summary>
    /// GetAsyncのfromが不正なフォーマットの場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("2025-1")]
    [InlineData("2025-00")]
    [InlineData("2025-13")]
    [InlineData("2025/01")]
    [InlineData("2025-01-01")]
    [InlineData("invalid")]
    public async Task GetAsync_WithInvalidFromFormat_ThrowsValidationException(string invalidFrom)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync(config =>
            {
                config.QueryParameters.From = invalidFrom;
                config.QueryParameters.To = "2025-01";
            });
        });

        Assert.Equal("fromの形式が正しくありません。", exception.Message);
        _mockLogger.Received(1).LogWarning("fromの形式が正しくありません。");
    }

    /// <summary>
    /// GetAsyncのfromが有効なフォーマットの場合に例外がスローされないことを確認
    /// </summary>
    [Theory]
    [InlineData("2025-01")]
    [InlineData("2025-12")]
    [InlineData("2000-06")]
    public async Task GetAsync_WithValidFromFormat_DoesNotThrowValidationException(string validFrom)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListBucketMeteringResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(new ListBucketMeteringResponse());

        var result = await builder.GetAsync(config =>
        {
            config.QueryParameters.From = validFrom;
            config.QueryParameters.To = "2025-12";
        });

        Assert.NotNull(result);
    }

    /// <summary>
    /// GetAsyncのtoが不正なフォーマットの場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("2025-1")]
    [InlineData("2025-00")]
    [InlineData("2025-13")]
    [InlineData("2025/01")]
    [InlineData("2025-01-01")]
    [InlineData("invalid")]
    public async Task GetAsync_WithInvalidToFormat_ThrowsValidationException(string invalidTo)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync(config =>
            {
                config.QueryParameters.From = "2025-01";
                config.QueryParameters.To = invalidTo;
            });
        });

        Assert.Equal("toの形式が正しくありません。", exception.Message);
        _mockLogger.Received(1).LogWarning("toの形式が正しくありません。");
    }

    /// <summary>
    /// GetAsyncのtoが有効なフォーマットの場合に例外がスローされないことを確認
    /// </summary>
    [Theory]
    [InlineData("2025-01")]
    [InlineData("2025-12")]
    [InlineData("2000-06")]
    public async Task GetAsync_WithValidToFormat_DoesNotThrowValidationException(string validTo)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListBucketMeteringResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(new ListBucketMeteringResponse());

        var result = await builder.GetAsync(config =>
        {
            config.QueryParameters.From = "2025-01";
            config.QueryParameters.To = validTo;
        });

        Assert.NotNull(result);
    }

    private static bool HasApiErrorMapping(Dictionary<string, ParsableFactory<IParsable>> errorMapping)
    {
        return errorMapping.TryGetValue("XXX", out var handler)
            && handler == ApiError.CreateFromDiscriminatorValue;
    }
}
