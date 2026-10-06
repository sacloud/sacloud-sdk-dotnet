using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.SecretManager.Models;
using Sacloud.Sdk.SecretManager.Secretmanager.Vaults.Item.Secrets.Unveil;
using Sacloud.Sdk.Shared.Models;

namespace Sacloud.Sdk.SecretManager.Tests.Secretmanager.Vaults.Item.Secrets.Unveil;

/// <summary>
/// UnveilRequestBuilderのテスト
/// </summary>
public class UnveilRequestBuilderTests
{
    private readonly ILogger _mockLogger;
    private readonly ILoggerFactory _mockLoggerFactory;

    public UnveilRequestBuilderTests()
    {
        _mockLogger = Substitute.For<ILogger>();
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLoggerFactory.CreateLogger(Arg.Any<string>()).Returns(_mockLogger);
    }

    /// <summary>
    /// コンストラクタ（PathParameters版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_InitializesSuccessfully()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "resource_id", "vault-id" }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new Sacloud.Sdk.SecretManager.Secretmanager.Vaults.Item.Secrets.Unveil.UnveilRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new Sacloud.Sdk.SecretManager.Secretmanager.Vaults.Item.Secrets.Unveil.UnveilRequestBuilder("https://api.example.com/secretmanager/vaults/vault-id/secrets/unveil", requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
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
            { "baseurl", "https://api.example.com" },
            { "resource_id", "vault-id" }
        };
        var builder = new UnveilRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new WrappedUnvail
        {
            Secret = new Unvail
            {
                Name = "test-secret",
                Version = 1
            }
        };
        var expectedResponse = new WrappedUnvail();

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedUnvail>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.PostAsync(body);

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedUnvail>>(),
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
                    @"^Secretアンベールを開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^Secretアンベールが完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncでAPIエラー発生時にログが出力されることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithApiError_LogsApiException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "resource_id", "vault-id" }
        };
        var builder = new UnveilRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new WrappedUnvail
        {
            Secret = new Unvail
            {
                Name = "test-secret",
                Version = 1
            }
        };
        var expectedException = new ApiError { ResponseStatusCode = 403 };

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedUnvail>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(System.Threading.Tasks.Task.FromException<WrappedUnvail?>(expectedException));

        await Assert.ThrowsAsync<ApiError>(async () => await builder.PostAsync(body));

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => IsApiErrorMessage(state, "Secretアンベール", "403")),
            expectedException,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncでAPI例外以外の例外発生時にログが出力されることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithException_LogsError()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "resource_id", "vault-id" }
        };
        var builder = new UnveilRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new WrappedUnvail
        {
            Secret = new Unvail
            {
                Name = "test-secret",
                Version = 1
            }
        };
        var expectedException = new Exception("test-exception");

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedUnvail>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(System.Threading.Tasks.Task.FromException<WrappedUnvail?>(expectedException));

        await Assert.ThrowsAsync<Exception>(async () => await builder.PostAsync(body));

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^Secretアンベール中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            expectedException,
            Arg.Any<Func<object, Exception?, string>>());
    }

    private static bool IsApiErrorMessage(object state, string operationName, string statusCode)
    {
        if (state is not IEnumerable<KeyValuePair<string, object>> stateProperties)
        {
            return false;
        }

        string? actualOperationName = null;
        IDictionary<string, object>? errorDetails = null;

        foreach (var kvp in stateProperties)
        {
            if (kvp.Key == "OperationName")
            {
                actualOperationName = kvp.Value?.ToString();
            }
            else if (kvp.Key == "ErrorDetails" || kvp.Key == "@ErrorDetails")
            {
                errorDetails = kvp.Value as IDictionary<string, object>;
            }
        }

        if (!string.Equals(actualOperationName, operationName, StringComparison.Ordinal) || errorDetails == null)
        {
            return false;
        }

        return errorDetails.TryGetValue("StatusCode", out var actualStatusCode)
            && string.Equals(actualStatusCode?.ToString(), statusCode, StringComparison.Ordinal);
    }

    /// <summary>
    /// PostAsyncの無効なボディでValidationExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithInvalidBody_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "resource_id", "vault-id" }
        };
        var builder = new UnveilRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new WrappedUnvail { Secret = null };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () => await builder.PostAsync(body));

        Assert.Equal("Secretが入力されていません。", exception.Message);
        _mockLogger.Received(1).Log(
            LogLevel.Warning,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => state.ToString() == "Secretが入力されていません。"),
            Arg.Any<Exception?>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// WithUrlが新しいインスタンスを返し、LoggerFactoryが引き継がれることを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewInstance_WithLoggerFactory()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "resource_id", "vault-id" }
        };
        var logger = Substitute.For<ILogger>();
        var loggerFactory = Substitute.For<ILoggerFactory>();
        loggerFactory.CreateLogger(Arg.Any<string>()).Returns(logger);
        var builder = new UnveilRequestBuilder(pathParameters, requestAdapter, loggerFactory);
        var newUrl = "https://newapi.example.com/secretmanager/vaults/vault-id/secrets/unveil";

        var newBuilder = builder.WithUrl(newUrl);

        Assert.NotNull(newBuilder);
        Assert.NotSame(builder, newBuilder);
        Assert.IsType<UnveilRequestBuilder>(newBuilder);
        loggerFactory.Received(2).CreateLogger(Arg.Any<string>());
    }
}
