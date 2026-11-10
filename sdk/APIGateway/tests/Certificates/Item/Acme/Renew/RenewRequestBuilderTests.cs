using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.APIGateway.Certificates.Item.Acme.Renew;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.APIGateway.Tests.Certificates.Item.Acme.Renew;

/// <summary>
/// ACME証明書更新のRequestBuilderテスト
/// </summary>
public class RenewRequestBuilderTests
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly ILogger<RenewRequestBuilder> _logger;

    public RenewRequestBuilderTests()
    {
        _logger = Substitute.For<ILogger<RenewRequestBuilder>>();
        _loggerFactory = Substitute.For<ILoggerFactory>();
        _loggerFactory.CreateLogger<RenewRequestBuilder>().Returns(_logger);
    }

    private static Dictionary<string, object> CreatePathParameters() =>
        new()
        {
            { "baseurl", "https://api.example.com" },
            { "certificateId", Guid.NewGuid() }
        };

    /// <summary>
    /// コンストラクタ（rawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();

        var builder = new RenewRequestBuilder("https://api.example.com/certificates/certificate-id/acme/renew", requestAdapter, NullLoggerFactory.Instance);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）がnullのrawUrlで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_WithNullRawUrl_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        string? rawUrl = null;

        Assert.Throws<ArgumentNullException>(() => new RenewRequestBuilder(rawUrl, requestAdapter, NullLoggerFactory.Instance));
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）がnullのRequestAdapterで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_WithNullRequestAdapter_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new RenewRequestBuilder("https://api.example.com/certificates/certificate-id/acme/renew", null, NullLoggerFactory.Instance));
    }

    /// <summary>
    /// WithUrlが正しい新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewInstance()
    {
        var builder = new RenewRequestBuilder(CreatePathParameters(), Substitute.For<IRequestAdapter>(), NullLoggerFactory.Instance);

        var newBuilder = builder.WithUrl("https://api.example.com/certificates/certificate-id/acme/renew");

        Assert.IsType<RenewRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }

    /// <summary>
    /// ToPostRequestInformationが正しいRequestInformationを生成することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_CreatesRequestInformationWithPostMethod()
    {
        var pathParameters = CreatePathParameters();
        var builder = new RenewRequestBuilder(pathParameters, Substitute.For<IRequestAdapter>(), NullLoggerFactory.Instance);

        var requestInfo = builder.ToPostRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.POST, requestInfo.HttpMethod);
    }

    /// <summary>
    /// PostAsyncがRequestAdapterを通じてリクエストを実行することを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_CallsSendNoContentAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new RenewRequestBuilder(CreatePathParameters(), requestAdapter, NullLoggerFactory.Instance);

        await builder.PostAsync();

        await requestAdapter.Received(1).SendNoContentAsync(
            Arg.Is<RequestInformation>(request => request.HttpMethod == Method.POST && request.UrlTemplate.Contains("/acme/renew")),
            Arg.Is<Dictionary<string, ParsableFactory<IParsable>>>(x => HasApiErrorMapping(x)),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// PostAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithApiError_ThrowsApiError()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new RenewRequestBuilder(CreatePathParameters(), requestAdapter, _loggerFactory);
        var expectedException = new ApiError { ResponseStatusCode = 404 };

        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(Task.FromException(expectedException));

        await Assert.ThrowsAsync<ApiError>(() => builder.PostAsync());

        AssertLog(LogLevel.Error, "自動発行証明書有効期限更新のAPIエラーが発生しました。");
    }

    /// <summary>
    /// PostAsyncが成功時に開始・完了ログを出力することを検証
    /// </summary>
    [Fact]
    public async Task PostAsync_Success_LogsStartAndCompletionMessages()
    {
        var builder = new RenewRequestBuilder(CreatePathParameters(), Substitute.For<IRequestAdapter>(), _loggerFactory);

        await builder.PostAsync();

        AssertLog(LogLevel.Information, "自動発行証明書の有効期限更新を開始します。");
        AssertLog(LogLevel.Information, "自動発行証明書の有効期限更新が完了しました。");
    }

    /// <summary>
    /// PostAsyncがエラー発生時にログを出力して例外を再スローすることを検証
    /// </summary>
    [Fact]
    public async Task PostAsync_WithOtherError_LogsAndRethrowsException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).ThrowsAsync(new InvalidOperationException());
        var builder = new RenewRequestBuilder(CreatePathParameters(), requestAdapter, _loggerFactory);

        await Assert.ThrowsAsync<InvalidOperationException>(() => builder.PostAsync());

        AssertLog(LogLevel.Error, "自動発行証明書の有効期限更新中にエラーが発生しました。");
    }

    /// <summary>
    /// PostAsyncが無効なcertificateIdでリクエスト送信前にバリデーション例外をスローすることを検証
    /// </summary>
    [Fact]
    public async Task PostAsync_WithInvalidCertificateId_ThrowsValidationExceptionBeforeSendingRequest()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = CreatePathParameters();
        pathParameters["certificateId"] = "invalid-guid";
        var builder = new RenewRequestBuilder(pathParameters, requestAdapter, NullLoggerFactory.Instance);

        await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync());

        await requestAdapter.DidNotReceive().SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());
    }

    private void AssertLog(LogLevel level, string message)
    {
        _logger.Received().Log(
            level,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => state.ToString()!.Contains(message)),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    private static bool HasApiErrorMapping(Dictionary<string, ParsableFactory<IParsable>> errorMapping)
    {
        return errorMapping.TryGetValue("XXX", out var handler)
            && handler == ApiError.CreateFromDiscriminatorValue;
    }
}
