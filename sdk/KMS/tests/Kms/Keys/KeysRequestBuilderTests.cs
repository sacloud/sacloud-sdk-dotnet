using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.KMS.Kms.Keys;
using Sacloud.Sdk.KMS.Kms.Keys.Item;
using Sacloud.Sdk.KMS.Models;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.KMS.Tests.Kms.Keys;

/// <summary>
/// KeysRequestBuilderのテスト
/// </summary>
public class KeysRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<KeysRequestBuilder> _mockLogger;

    public KeysRequestBuilderTests()
    {
        _mockLogger = Substitute.For<ILogger<KeysRequestBuilder>>();
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLoggerFactory.CreateLogger(Arg.Any<string>()).Returns(_mockLogger);
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

        Assert.Throws<ArgumentNullException>(() => new KeysRequestBuilder(pathParameters, requestAdapter, null!));
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）の初期化でLoggerFactoryがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrlAndNullLoggerFactory_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var rawUrl = "https://api.example.com/kms/keys";

        Assert.Throws<ArgumentNullException>(() => new KeysRequestBuilder(rawUrl, requestAdapter, null!));
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

        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var rawUrl = "https://api.example.com/kms/keys";

        var builder = new KeysRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// インデクサーが正しいWithResource_ItemRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_ReturnsWithResourceItemRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new WrappedCreateKey();

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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new WrappedCreateKey();

        var requestInfo = builder.ToPostRequestInformation(body);

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
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
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.Throws<ArgumentNullException>(() => builder.ToPostRequestInformation(null!));
    }

    /// <summary>
    /// WithUrlが新しいKeysRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewKeysRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var newBuilder = builder.WithUrl("https://api.example.com/kms/keys/custom");

        Assert.NotNull(newBuilder);
        Assert.IsType<KeysRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }

    /// <summary>
    /// GetAsyncが結果を返すことを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_ReturnsResult()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResult = new PaginatedKeyList();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PaginatedKeyList>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(expectedResult);

        var result = await builder.GetAsync();

        Assert.NotNull(result);
        Assert.Same(expectedResult, result);
    }

    /// <summary>
    /// PostAsyncが結果を返すことを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_ReturnsResult()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = CreateValidWrappedCreateKey();
        var expectedResult = new WrappedCreateKey();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedCreateKey>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(expectedResult);

        var result = await builder.PostAsync(body);

        Assert.NotNull(result);
        Assert.Same(expectedResult, result);
    }



    /// <summary>
    /// GetAsyncがRequestAdapter.SendAsyncを呼び出し、開始・成功ログを出力することを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_CallsRequestAdapterSendAsync_LogsStartAndSuccess()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new PaginatedKeyList();

        requestAdapter.SendAsync<PaginatedKeyList>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PaginatedKeyList>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.GetAsync();

        await requestAdapter.Received(1).SendAsync<PaginatedKeyList>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PaginatedKeyList>>(),
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
                    @"^キー一覧の取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^キー一覧の取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncでAPIエラーが発生した場合にエラーログを出力することを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithApiError_LogsError()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError
        {
            ResponseStatusCode = 401
        };

        requestAdapter.SendAsync<PaginatedKeyList>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PaginatedKeyList>>(),
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
                state.ToString()!.Contains("キー一覧取得のAPIエラーが発生しました。")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncでその他のエラーが発生した場合にエラーログを出力することを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithOtherError_LogsError()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception("Test error");

        requestAdapter.SendAsync<PaginatedKeyList>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PaginatedKeyList>>(),
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
                    @"^キー一覧の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncがRequestAdapter.SendAsyncを呼び出し、開始・成功ログを出力することを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_CallsRequestAdapterSendAsync_LogsStartAndSuccess()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = CreateValidWrappedCreateKey();
        var expectedResponse = new WrappedCreateKey();

        requestAdapter.SendAsync<WrappedCreateKey>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedCreateKey>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.PostAsync(body);

        await requestAdapter.Received(1).SendAsync<WrappedCreateKey>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedCreateKey>>(),
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
                    @"^キーの作成を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^キーの作成が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 201\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncでAPIエラーが発生した場合にエラーログを出力することを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithApiError_LogsError()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = CreateValidWrappedCreateKey();
        var expectedException = new ApiError
        {
            ResponseStatusCode = 400
        };

        requestAdapter.SendAsync<WrappedCreateKey>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedCreateKey>>(),
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
                state.ToString()!.Contains("キー作成のAPIエラーが発生しました。")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncでその他のエラーが発生した場合にエラーログを出力することを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithOtherError_LogsError()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = CreateValidWrappedCreateKey();
        var expectedException = new Exception("Test error");

        requestAdapter.SendAsync<WrappedCreateKey>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedCreateKey>>(),
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
                    @"^キーの作成中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncでリクエストボディがnullの場合にValidationExceptionがスローされることを確認
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
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(null!);
        });

        Assert.Equal("リクエストボディは必須です。", exception.Message);
        _mockLogger.Received(1).Log(
            LogLevel.Warning,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => state.ToString() == "リクエストボディは必須です。"),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncのリクエストボディのKeyパラメータがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithNullKeyParameter_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new WrappedCreateKey
        {
            Key = null
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        Assert.Equal("Keyが入力されていません。", exception.Message);
    }

    /// <summary>
    /// PostAsyncのリクエストボディのKey.Nameパラメータがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithNullKeyNameParameter_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new WrappedCreateKey
        {
            Key = new CreateKey
            {
                Name = null
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        Assert.Equal("Nameが入力されていません。", exception.Message);
    }

    /// <summary>
    /// PostAsyncのリクエストボディのKey.Nameパラメータが255文字を超える場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithKeyNameExceeding255Characters_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new WrappedCreateKey
        {
            Key = new CreateKey
            {
                Name = new string('a', 256)
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        Assert.Equal("Nameは255文字以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// バリデーションを通過する有効なWrappedCreateKeyを作成するヘルパーメソッド
    /// </summary>
    private static WrappedCreateKey CreateValidWrappedCreateKey()
    {
        return new WrappedCreateKey
        {
            Key = new CreateKey
            {
                Name = "test-key-name"
            }
        };
    }
}
