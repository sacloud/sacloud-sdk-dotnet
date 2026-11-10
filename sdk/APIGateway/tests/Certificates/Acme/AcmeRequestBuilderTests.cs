using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.APIGateway.Certificates;
using Sacloud.Sdk.APIGateway.Certificates.Acme;
using Sacloud.Sdk.APIGateway.Models;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.APIGateway.Tests.Certificates.Acme;

/// <summary>
/// ACME証明書操作のRequestBuilderテスト
/// </summary>
public class AcmeRequestBuilderTests
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly ILogger<AcmeRequestBuilder> _logger;

    public AcmeRequestBuilderTests()
    {
        _logger = Substitute.For<ILogger<AcmeRequestBuilder>>();
        _loggerFactory = Substitute.For<ILoggerFactory>();
        _loggerFactory.CreateLogger<AcmeRequestBuilder>().Returns(_logger);
    }

    private static Dictionary<string, object> CreatePathParameters()
    {
        return new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
    }

    /// <summary>
    /// CertificatesRequestBuilderのAcmeナビゲーションがAcmeRequestBuilderを返すことを検証
    /// </summary>
    [Fact]
    public void AcmeNavigation_ReturnsExpectedRequestBuilders()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var certificates = new CertificatesRequestBuilder(CreatePathParameters(), requestAdapter, Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory.Instance);

        Assert.IsType<AcmeRequestBuilder>(certificates.Acme);
    }

    /// <summary>
    /// rawUrlを指定したコンストラクタでインスタンス化できることを検証
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();

        var builder = new AcmeRequestBuilder("https://api.example.com/certificates/acme", requestAdapter, NullLoggerFactory.Instance);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// rawUrlを指定したコンストラクタがnull rawUrlで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_WithNullRawUrl_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        string? rawUrl = null;

        Assert.Throws<ArgumentNullException>(() => new AcmeRequestBuilder(rawUrl, requestAdapter, NullLoggerFactory.Instance));
    }

    /// <summary>
    /// rawUrlを指定したコンストラクタがnull requestAdapterで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_WithNullRequestAdapter_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new AcmeRequestBuilder("https://api.example.com/certificates/acme", null, NullLoggerFactory.Instance));
    }

    /// <summary>
    /// WithUrlが新しいAcmeRequestBuilderインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewInstance()
    {
        var builder = new AcmeRequestBuilder(CreatePathParameters(), Substitute.For<IRequestAdapter>(), NullLoggerFactory.Instance);

        var newBuilder = builder.WithUrl("https://api.example.com/certificates/acme");

        Assert.IsType<AcmeRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }

    /// <summary>
    /// ToPostRequestInformationがPOSTメソッドと必要なヘッダーおよびContentを設定することを検証
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_SetsAcceptHeader()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.SerializationWriterFactory.Returns(new JsonSerializationWriterFactory());
        var builder = new AcmeRequestBuilder(CreatePathParameters(), requestAdapter, NullLoggerFactory.Instance);

        var requestInfo = builder.ToPostRequestInformation(new CertificatesAcme { Name = "example.com", Sans = ["www.example.com"] });

        Assert.Equal(Method.POST, requestInfo.HttpMethod);
        Assert.Contains("/certificates/acme", requestInfo.UrlTemplate);
        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Content-Type"]);
        Assert.NotNull(requestInfo.Content);
    }

    /// <summary>
    /// ToPostRequestInformationがnull bodyで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_WithNullBody_ThrowsArgumentNullException()
    {
        var builder = new AcmeRequestBuilder(CreatePathParameters(), Substitute.For<IRequestAdapter>(), NullLoggerFactory.Instance);

        Assert.Throws<ArgumentNullException>(() => builder.ToPostRequestInformation(null));
    }

    /// <summary>
    /// PostAsyncがACME証明書作成のNoContentリクエストを送信することを検証
    /// </summary>
    [Fact]
    public async Task PostAsync_SendsNoContentRequest()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new AcmeRequestBuilder(CreatePathParameters(), requestAdapter, NullLoggerFactory.Instance);

        await builder.PostAsync(new CertificatesAcme { Name = "example.com" });

        await requestAdapter.Received(1).SendNoContentAsync(
            Arg.Is<RequestInformation>(request => request.HttpMethod == Method.POST && request.UrlTemplate.Contains("/certificates/acme")),
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
        var builder = new AcmeRequestBuilder(CreatePathParameters(), requestAdapter, NullLoggerFactory.Instance);
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
    /// PostAsyncがAPIエラー発生時にApiErrorをそのまま送出することを検証
    /// </summary>
    [Fact]
    public async Task PostAsync_WithApiError_ThrowsApiError()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new AcmeRequestBuilder(CreatePathParameters(), requestAdapter, _loggerFactory);
        var expectedException = new ApiError { ResponseStatusCode = 404 };

        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(Task.FromException(expectedException));

        await Assert.ThrowsAsync<ApiError>(() => builder.PostAsync(new CertificatesAcme { Name = "example.com" }));

        AssertLog(LogLevel.Error, "証明書自動発行のAPIエラーが発生しました。");
    }

    /// <summary>
    /// PostAsyncが成功時に開始・完了ログを出力することを検証
    /// </summary>
    [Fact]
    public async Task PostAsync_Success_LogsStartAndCompletionMessages()
    {
        var builder = new AcmeRequestBuilder(CreatePathParameters(), Substitute.For<IRequestAdapter>(), _loggerFactory);

        await builder.PostAsync(new CertificatesAcme { Name = "example.com" });

        AssertLog(LogLevel.Information, "証明書の自動発行を開始します。");
        AssertLog(LogLevel.Information, "証明書の自動発行が完了しました。");
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
        var builder = new AcmeRequestBuilder(CreatePathParameters(), requestAdapter, _loggerFactory);

        await Assert.ThrowsAsync<InvalidOperationException>(() => builder.PostAsync(new CertificatesAcme { Name = "example.com" }));

        AssertLog(LogLevel.Error, "証明書の自動発行中にエラーが発生しました。");
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
