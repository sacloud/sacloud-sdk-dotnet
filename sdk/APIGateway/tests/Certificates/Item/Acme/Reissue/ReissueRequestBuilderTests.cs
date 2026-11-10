using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.APIGateway.Certificates.Item.Acme.Reissue;
using Sacloud.Sdk.APIGateway.Models;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.APIGateway.Tests.Certificates.Item.Acme.Reissue;

/// <summary>
/// ACME証明書再発行のRequestBuilderテスト
/// </summary>
public class ReissueRequestBuilderTests
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly ILogger<ReissueRequestBuilder> _logger;

    public ReissueRequestBuilderTests()
    {
        _logger = Substitute.For<ILogger<ReissueRequestBuilder>>();
        _loggerFactory = Substitute.For<ILoggerFactory>();
        _loggerFactory.CreateLogger<ReissueRequestBuilder>().Returns(_logger);
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

        var builder = new ReissueRequestBuilder("https://api.example.com/certificates/certificate-id/acme/reissue", requestAdapter, NullLoggerFactory.Instance);

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

        Assert.Throws<ArgumentNullException>(() => new ReissueRequestBuilder(rawUrl, requestAdapter, NullLoggerFactory.Instance));
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）がnullのRequestAdapterで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_WithNullRequestAdapter_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new ReissueRequestBuilder("https://api.example.com/certificates/certificate-id/acme/reissue", null, NullLoggerFactory.Instance));
    }

    /// <summary>
    /// WithUrlが正しい新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewInstance()
    {
        var builder = new ReissueRequestBuilder(CreatePathParameters(), Substitute.For<IRequestAdapter>(), NullLoggerFactory.Instance);

        var newBuilder = builder.WithUrl("https://api.example.com/certificates/certificate-id/acme/reissue");

        Assert.IsType<ReissueRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }

    /// <summary>
    /// ToPostRequestInformationが正しいRequestInformationを生成することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_CreatesRequestInformationWithPostMethod()
    {
        var pathParameters = CreatePathParameters();
        var builder = new ReissueRequestBuilder(pathParameters, Substitute.For<IRequestAdapter>(), NullLoggerFactory.Instance);

        var requestInfo = builder.ToPostRequestInformation(new CertificatesAcme { Name = "example.com" });

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.POST, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToPostRequestInformationがnullボディで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_WithNullBody_ThrowsArgumentNullException()
    {
        var builder = new ReissueRequestBuilder(CreatePathParameters(), Substitute.For<IRequestAdapter>(), NullLoggerFactory.Instance);

        Assert.Throws<ArgumentNullException>(() => builder.ToPostRequestInformation(null));
    }

    /// <summary>
    /// PostAsyncがRequestAdapterを通じてリクエストを実行することを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_CallsSendNoContentAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new ReissueRequestBuilder(CreatePathParameters(), requestAdapter, NullLoggerFactory.Instance);

        await builder.PostAsync(new CertificatesAcme { Name = "example.com" });

        await requestAdapter.Received(1).SendNoContentAsync(
            Arg.Is<RequestInformation>(request => request.HttpMethod == Method.POST && request.UrlTemplate.Contains("/acme/reissue")),
            Arg.Is<Dictionary<string, ParsableFactory<IParsable>>>(x => HasApiErrorMapping(x)),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// PostAsyncが重複したSANを含むリクエストボディで送信前にバリデーション例外をスローすることを検証
    /// </summary>
    [Fact]
    public async Task PostAsync_WithDuplicateSans_ThrowsValidationExceptionBeforeSendingRequest()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new ReissueRequestBuilder(CreatePathParameters(), requestAdapter, NullLoggerFactory.Instance);
        var body = new CertificatesAcme
        {
            Sans = ["www.example.com", "www.example.com"]
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal("Sansの要素は重複できません。", exception.Message);
        await requestAdapter.DidNotReceive().SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// GetAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithApiError_ThrowsApiError()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new ReissueRequestBuilder(CreatePathParameters(), requestAdapter, _loggerFactory);
        var expectedException = new ApiError { ResponseStatusCode = 404 };

        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(Task.FromException(expectedException));

        await Assert.ThrowsAsync<ApiError>(() => builder.PostAsync(new CertificatesAcme { Name = "example.com" }));

        AssertLog(LogLevel.Error, "自動発行証明書再発行のAPIエラーが発生しました。");
    }

    /// <summary>
    /// PostAsyncが成功時に開始・完了ログを出力することを検証
    /// </summary>
    [Fact]
    public async Task PostAsync_Success_LogsStartAndCompletionMessages()
    {
        var builder = new ReissueRequestBuilder(CreatePathParameters(), Substitute.For<IRequestAdapter>(), _loggerFactory);

        await builder.PostAsync(new CertificatesAcme { Name = "example.com" });

        AssertLog(LogLevel.Information, "自動発行証明書の再発行を開始します。");
        AssertLog(LogLevel.Information, "自動発行証明書の再発行が完了しました。");
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
        var builder = new ReissueRequestBuilder(CreatePathParameters(), requestAdapter, _loggerFactory);

        await Assert.ThrowsAsync<InvalidOperationException>(() => builder.PostAsync(new CertificatesAcme { Name = "example.com" }));

        AssertLog(LogLevel.Error, "自動発行証明書の再発行中にエラーが発生しました。");
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
        var builder = new ReissueRequestBuilder(pathParameters, requestAdapter, NullLoggerFactory.Instance);

        await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(new CertificatesAcme { Name = "example.com" }));

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
