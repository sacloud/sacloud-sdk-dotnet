using Microsoft.Extensions.Logging;
using NSubstitute;
using Sacloud.Sdk.Shared.Helpers;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Sacloud.Sdk.Shared.Tests.Helpers;

/// <summary>
/// AccessTokenHelper のテスト
/// </summary>
public class AccessTokenHelperTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger _mockLogger;

    public AccessTokenHelperTests()
    {
        _mockLogger = Substitute.For<ILogger>();
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        // CreateLogger(Type) は内部で CreateLogger(string) を呼び出す拡張メソッドのため string で受け取る
        _mockLoggerFactory.CreateLogger(Arg.Any<string>()).Returns(_mockLogger);
    }

    private static HttpClient CreateHttpClient(HttpStatusCode statusCode, string content)
    {
        var response = new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(content, Encoding.UTF8, "application/json")
        };
        return new HttpClient(new FakeHttpMessageHandler(response));
    }

    private static string CreateTokenJson(string accessToken = "test-token")
        => JsonSerializer.Serialize(new { access_token = accessToken, expires_in = 3600, token_type = "Bearer" });

    /// <summary>
    /// jwt が null の場合に ArgumentNullException をスローすることを確認
    /// </summary>
    [Fact]
    public async Task GetAccessTokenAsync_WithNullJwt_ThrowsArgumentNullException()
    {
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() =>
            AccessTokenHelper.GetAccessTokenAsync(null!));

        Assert.Equal("jwt", exception.ParamName);
    }

    /// <summary>
    /// jwt が空文字または空白文字列の場合に ArgumentException をスローすることを確認
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("　")]
    public async Task GetAccessTokenAsync_WithWhiteSpaceJwt_ThrowsArgumentException(string jwt)
    {
        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            AccessTokenHelper.GetAccessTokenAsync(jwt));

        Assert.Equal("jwt", exception.ParamName);
    }

    /// <summary>
    /// 正常なレスポンスの場合にアクセストークンを返すことを確認
    /// </summary>
    [Fact]
    public async Task GetAccessTokenAsync_WithSuccessResponse_ReturnsAccessToken()
    {
        using var httpClient = CreateHttpClient(HttpStatusCode.OK, CreateTokenJson("my-access-token"));

        var result = await AccessTokenHelper.GetAccessTokenAsync("test-jwt", httpClient, _mockLoggerFactory);

        Assert.Equal("my-access-token", result.AccessToken);
    }

    /// <summary>
    /// loggerFactory が null でも正常にアクセストークンを返すことを確認
    /// </summary>
    [Fact]
    public async Task GetAccessTokenAsync_WithNullLoggerFactory_ReturnsAccessToken()
    {
        using var httpClient = CreateHttpClient(HttpStatusCode.OK, CreateTokenJson("my-access-token"));

        var result = await AccessTokenHelper.GetAccessTokenAsync("test-jwt", httpClient);

        Assert.Equal("my-access-token", result.AccessToken);
    }

    /// <summary>
    /// 正常完了時に開始・完了のログが出力されることを確認
    /// </summary>
    [Fact]
    public async Task GetAccessTokenAsync_WithSuccessResponse_LogsStartAndComplete()
    {
        using var httpClient = CreateHttpClient(HttpStatusCode.OK, CreateTokenJson());

        await AccessTokenHelper.GetAccessTokenAsync("test-jwt", httpClient, _mockLoggerFactory);

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString()!,
                @"^アクセストークンの取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString()!,
                @"^アクセストークンの取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// HTTP ステータスコードが非成功の場合に InvalidOperationException をスローすることを確認
    /// </summary>
    [Theory]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.Forbidden)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async Task GetAccessTokenAsync_WithNonSuccessResponse_ThrowsInvalidOperationException(HttpStatusCode statusCode)
    {
        using var httpClient = CreateHttpClient(statusCode, "{\"error\":\"unauthorized\"}");

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            AccessTokenHelper.GetAccessTokenAsync("test-jwt", httpClient, _mockLoggerFactory));

        Assert.Contains($"StatusCode: {(int)statusCode}", exception.Message);
    }

    /// <summary>
    /// 非成功レスポンスの場合にエラーログが出力されることを確認
    /// </summary>
    [Fact]
    public async Task GetAccessTokenAsync_WithNonSuccessResponse_LogsError()
    {
        using var httpClient = CreateHttpClient(HttpStatusCode.Unauthorized, "{\"error\":\"unauthorized\"}");

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            AccessTokenHelper.GetAccessTokenAsync("test-jwt", httpClient, _mockLoggerFactory));

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString()!,
                @"^アクセストークン取得に失敗しました。StatusCode: 401, EndTime: \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// レスポンスに AccessToken が含まれていない場合に InvalidOperationException をスローすることを確認
    /// </summary>
    [Theory]
    [InlineData("{}")]
    [InlineData("{\"access_token\":null}")]
    [InlineData("{\"access_token\":\"\"}")]
    [InlineData("{\"access_token\":\"  \"}")]
    public async Task GetAccessTokenAsync_WithMissingAccessToken_ThrowsInvalidOperationException(string jsonBody)
    {
        using var httpClient = CreateHttpClient(HttpStatusCode.OK, jsonBody);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            AccessTokenHelper.GetAccessTokenAsync("test-jwt", httpClient, _mockLoggerFactory));

        Assert.Equal("アクセストークンがレスポンスに含まれていません。", exception.Message);
    }

    /// <summary>
    /// アクセストークンがレスポンスに含まれていない場合にエラーログが出力されることを確認
    /// </summary>
    [Fact]
    public async Task GetAccessTokenAsync_WithMissingAccessToken_LogsError()
    {
        using var httpClient = CreateHttpClient(HttpStatusCode.OK, "{}");

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            AccessTokenHelper.GetAccessTokenAsync("test-jwt", httpClient, _mockLoggerFactory));

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString()!,
                @"^アクセストークンがレスポンスに含まれていません。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// HttpClient から例外がスローされた場合に再スローしてエラーログを出力することを確認
    /// </summary>
    [Fact]
    public async Task GetAccessTokenAsync_WithHttpClientException_RethrowsAndLogsError()
    {
        var expectedException = new HttpRequestException("network error");
        using var httpClient = new HttpClient(new FakeHttpMessageHandler(expectedException));

        var exception = await Assert.ThrowsAsync<HttpRequestException>(() =>
            AccessTokenHelper.GetAccessTokenAsync("test-jwt", httpClient, _mockLoggerFactory));

        Assert.Same(expectedException, exception);
        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString()!,
                @"^アクセストークンの取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            expectedException,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// キャンセルトークンが通知された場合に OperationCanceledException をスローすることを確認
    /// </summary>
    [Fact]
    public async Task GetAccessTokenAsync_WithCancelledToken_ThrowsOperationCanceledException()
    {
        using var cts = new CancellationTokenSource();
        cts.Cancel();
        using var httpClient = CreateHttpClient(HttpStatusCode.OK, CreateTokenJson());

        // TaskCanceledException は OperationCanceledException のサブクラスのため ThrowsAnyAsync を使用する
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
            AccessTokenHelper.GetAccessTokenAsync("test-jwt", httpClient, _mockLoggerFactory, cts.Token));
    }

    /// <summary>
    /// 正しい grant_type と assertion がリクエストボディに含まれることを確認
    /// </summary>
    [Fact]
    public async Task GetAccessTokenAsync_WithValidJwt_SendsCorrectFormContent()
    {
        const string testJwt = "test-jwt-value";
        var capturedFormData = new Dictionary<string, string>();
        using var httpClient = new HttpClient(new CapturingHttpMessageHandler(
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(CreateTokenJson(), Encoding.UTF8, "application/json")
            },
            capturedFormData));

        await AccessTokenHelper.GetAccessTokenAsync(testJwt, httpClient, _mockLoggerFactory);

        Assert.Equal("urn:ietf:params:oauth:grant-type:jwt-bearer", capturedFormData["grant_type"]);
        Assert.Equal(testJwt, capturedFormData["assertion"]);
    }

    /// <summary>
    /// 非成功レスポンスの場合に catch (Exception ex) 側のエラーログが出力されないことを確認（二重出力防止）
    /// </summary>
    [Fact]
    public async Task GetAccessTokenAsync_WithNonSuccessResponse_DoesNotLogGenericError()
    {
        using var httpClient = CreateHttpClient(HttpStatusCode.Unauthorized, "{\"error\":\"unauthorized\"}");

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            AccessTokenHelper.GetAccessTokenAsync("test-jwt", httpClient, _mockLoggerFactory));

        _mockLogger.DidNotReceive().Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString()!,
                @"^アクセストークンの取得中にエラーが発生しました。")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// アクセストークン不在の場合に catch (Exception ex) 側のエラーログが出力されないことを確認（二重出力防止）
    /// </summary>
    [Fact]
    public async Task GetAccessTokenAsync_WithMissingAccessToken_DoesNotLogGenericError()
    {
        using var httpClient = CreateHttpClient(HttpStatusCode.OK, "{}");

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            AccessTokenHelper.GetAccessTokenAsync("test-jwt", httpClient, _mockLoggerFactory));

        _mockLogger.DidNotReceive().Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString()!,
                @"^アクセストークンの取得中にエラーが発生しました。")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// 成功レスポンス時に HttpResponseMessage が Dispose されることを確認
    /// </summary>
    [Fact]
    public async Task GetAccessTokenAsync_WithSuccessResponse_DisposesHttpResponseMessage()
    {
        var trackingResponse = new TrackingHttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(CreateTokenJson(), Encoding.UTF8, "application/json")
        };
        using var httpClient = new HttpClient(new FakeHttpMessageHandler(trackingResponse));

        await AccessTokenHelper.GetAccessTokenAsync("test-jwt", httpClient, _mockLoggerFactory);

        Assert.True(trackingResponse.IsDisposed);
    }

    /// <summary>
    /// 非成功レスポンス時に HttpResponseMessage が Dispose されることを確認
    /// </summary>
    [Fact]
    public async Task GetAccessTokenAsync_WithNonSuccessResponse_DisposesHttpResponseMessage()
    {
        var trackingResponse = new TrackingHttpResponseMessage(HttpStatusCode.Unauthorized)
        {
            Content = new StringContent("{\"error\":\"unauthorized\"}", Encoding.UTF8, "application/json")
        };
        using var httpClient = new HttpClient(new FakeHttpMessageHandler(trackingResponse));

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            AccessTokenHelper.GetAccessTokenAsync("test-jwt", httpClient, _mockLoggerFactory));

        Assert.True(trackingResponse.IsDisposed);
    }

    /// <summary>
    /// アクセストークン不在時に HttpResponseMessage が Dispose されることを確認
    /// </summary>
    [Fact]
    public async Task GetAccessTokenAsync_WithMissingAccessToken_DisposesHttpResponseMessage()
    {
        var trackingResponse = new TrackingHttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{}", Encoding.UTF8, "application/json")
        };
        using var httpClient = new HttpClient(new FakeHttpMessageHandler(trackingResponse));

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            AccessTokenHelper.GetAccessTokenAsync("test-jwt", httpClient, _mockLoggerFactory));

        Assert.True(trackingResponse.IsDisposed);
    }

    /// <summary>
    /// HttpMessageHandler のフェイク実装。テスト用に固定のレスポンスまたは例外を返す。
    /// </summary>
    private sealed class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpResponseMessage? _response;
        private readonly Exception? _exception;

        public FakeHttpMessageHandler(HttpResponseMessage response)
        {
            _response = response;
        }

        public FakeHttpMessageHandler(Exception exception)
        {
            _exception = exception;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (_exception is not null)
            {
                throw _exception;
            }
            return Task.FromResult(_response!);
        }
    }

    /// <summary>
    /// リクエスト内容をキャプチャして固定レスポンスを返す HttpMessageHandler のフェイク実装。
    /// </summary>
    private sealed class CapturingHttpMessageHandler(HttpResponseMessage response, Dictionary<string, string> capturedFormData) : HttpMessageHandler
    {
        private readonly HttpResponseMessage _response = response;
        private readonly Dictionary<string, string> _capturedFormData = capturedFormData;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var content = await request.Content!.ReadAsStringAsync(cancellationToken);
            foreach (var pair in content.Split('&'))
            {
                var idx = pair.IndexOf('=');
                if (idx >= 0)
                {
                    var key = Uri.UnescapeDataString(pair.Substring(0, idx).Replace('+', ' '));
                    var value = Uri.UnescapeDataString(pair.Substring(idx + 1).Replace('+', ' '));
                    _capturedFormData[key] = value;
                }
            }
            return _response;
        }
    }

    /// <summary>
    /// Dispose が呼ばれたかどうかを追跡する HttpResponseMessage のサブクラス
    /// </summary>
    private sealed class TrackingHttpResponseMessage(HttpStatusCode statusCode) : HttpResponseMessage(statusCode)
    {
        public bool IsDisposed { get; private set; }

        protected override void Dispose(bool disposing)
        {
            IsDisposed = true;
            base.Dispose(disposing);
        }
    }
}
