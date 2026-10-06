using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.SecretManager.Models;
using Sacloud.Sdk.SecretManager.Secretmanager.Vaults.Item.Secrets.Unveil;
using Sacloud.Sdk.SecretManager.Secretmanager.Vaults.Item.Secrets;
using Sacloud.Sdk.Shared.Models;

namespace Sacloud.Sdk.SecretManager.Tests.Secretmanager.Vaults.Item.Secrets;

/// <summary>
/// SecretsRequestBuilderのテスト
/// </summary>
public class SecretsRequestBuilderTests
{
    private readonly ILogger _mockLogger;
    private readonly ILoggerFactory _mockLoggerFactory;

    public SecretsRequestBuilderTests()
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
        var builder = new Sacloud.Sdk.SecretManager.Secretmanager.Vaults.Item.Secrets.SecretsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new Sacloud.Sdk.SecretManager.Secretmanager.Vaults.Item.Secrets.SecretsRequestBuilder("https://api.example.com/secretmanager/vaults/vault-id/secrets", requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// Unveilプロパティが正しいUnveilRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Unveil_ReturnsUnveilRequestBuilder()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "resource_id", "vault-id" }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new Sacloud.Sdk.SecretManager.Secretmanager.Vaults.Item.Secrets.SecretsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var unveilBuilder = builder.Unveil;

        Assert.NotNull(unveilBuilder);
        Assert.IsType<UnveilRequestBuilder>(unveilBuilder);
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
            { "baseurl", "https://api.example.com" },
            { "resource_id", "vault-id" }
        };
        var builder = new SecretsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new PaginatedSecretList();

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PaginatedSecretList>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.GetAsync();

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PaginatedSecretList>>(),
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
                    @"^Secret一覧取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^Secret一覧取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncでAPIエラー発生時にログが出力されることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithApiError_LogsApiException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "resource_id", "vault-id" }
        };
        var builder = new SecretsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError { ResponseStatusCode = 404 };

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PaginatedSecretList>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(System.Threading.Tasks.Task.FromException<PaginatedSecretList?>(expectedException));

        await Assert.ThrowsAsync<ApiError>(async () => await builder.GetAsync());

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => IsApiErrorMessage(state, "Secret一覧取得", "404")),
            expectedException,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncでAPI例外以外の例外発生時にログが出力されることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithException_LogsError()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "resource_id", "vault-id" }
        };
        var builder = new SecretsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception("test-exception");

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PaginatedSecretList>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(System.Threading.Tasks.Task.FromException<PaginatedSecretList?>(expectedException));

        await Assert.ThrowsAsync<Exception>(async () => await builder.GetAsync());

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^Secret一覧取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            expectedException,
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
            { "baseurl", "https://api.example.com" },
            { "resource_id", "vault-id" }
        };
        var builder = new SecretsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new WrappedCreateSecret
        {
            Secret = new CreateSecret
            {
                Name = "test-secret",
                Value = "test-value"
            }
        };
        var expectedResponse = new WrappedCreateSecret();

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedCreateSecret>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.PostAsync(body);

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedCreateSecret>>(),
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
                    @"^Secret作成を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^Secret作成が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 201\]$")),
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
        var builder = new SecretsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new WrappedCreateSecret
        {
            Secret = new CreateSecret
            {
                Name = "test-secret",
                Value = "test-value"
            }
        };
        var expectedException = new ApiError { ResponseStatusCode = 400 };

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedCreateSecret>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(System.Threading.Tasks.Task.FromException<WrappedCreateSecret?>(expectedException));

        await Assert.ThrowsAsync<ApiError>(async () => await builder.PostAsync(body));

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => IsApiErrorMessage(state, "Secret作成", "400")),
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
        var builder = new SecretsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new WrappedCreateSecret
        {
            Secret = new CreateSecret
            {
                Name = "test-secret",
                Value = "test-value"
            }
        };
        var expectedException = new Exception("test-exception");

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedCreateSecret>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(System.Threading.Tasks.Task.FromException<WrappedCreateSecret?>(expectedException));

        await Assert.ThrowsAsync<Exception>(async () => await builder.PostAsync(body));

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^Secret作成中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            expectedException,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// DeleteAsyncがRequestAdapter.SendNoContentAsyncを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_CallsRequestAdapterSendNoContentAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "resource_id", "vault-id" }
        };
        var builder = new SecretsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new WrappedDeleteSecret
        {
            Secret = new DeleteSecret
            {
                Name = "test-secret"
            }
        };

        await builder.DeleteAsync(body);

        await requestAdapter.Received(1).SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^Secret削除を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^Secret削除が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 204\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// DeleteAsyncでAPIエラー発生時にログが出力されることを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_WithApiError_LogsApiException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "resource_id", "vault-id" }
        };
        var builder = new SecretsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new WrappedDeleteSecret
        {
            Secret = new DeleteSecret
            {
                Name = "test-secret"
            }
        };
        var expectedException = new ApiError { ResponseStatusCode = 404 };

        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(System.Threading.Tasks.Task.FromException(expectedException));

        await Assert.ThrowsAsync<ApiError>(async () => await builder.DeleteAsync(body));

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => IsApiErrorMessage(state, "Secret削除", "404")),
            expectedException,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// DeleteAsyncでAPI例外以外の例外発生時にログが出力されることを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_WithException_LogsError()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "resource_id", "vault-id" }
        };
        var builder = new SecretsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new WrappedDeleteSecret
        {
            Secret = new DeleteSecret
            {
                Name = "test-secret"
            }
        };
        var expectedException = new Exception("test-exception");

        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(System.Threading.Tasks.Task.FromException(expectedException));

        await Assert.ThrowsAsync<Exception>(async () => await builder.DeleteAsync(body));

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^Secret削除中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            expectedException,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// DeleteAsyncの無効なボディでValidationExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_WithInvalidBody_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "resource_id", "vault-id" }
        };
        var builder = new SecretsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new WrappedDeleteSecret { Secret = null };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () => await builder.DeleteAsync(body));

        Assert.Equal("Secretが入力されていません。", exception.Message);
        _mockLogger.Received(1).Log(
            LogLevel.Warning,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => state.ToString() == "Secretが入力されていません。"),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    private static bool IsApiErrorMessage(object state, string operationName, string statusCode)
    {
        if (state is not IEnumerable<KeyValuePair<string, object>> stateProperties)
        {
            return false;
        }

        string actualOperationName = null;
        IDictionary<string, object> errorDetails = null;

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
        var builder = new SecretsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new WrappedCreateSecret { Secret = null };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () => await builder.PostAsync(body));

        Assert.Equal("Secretが入力されていません。", exception.Message);
        _mockLogger.Received(1).Log(
            LogLevel.Warning,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => state.ToString() == "Secretが入力されていません。"),
            Arg.Any<Exception>(),
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
        var builder = new SecretsRequestBuilder(pathParameters, requestAdapter, loggerFactory);
        var newUrl = "https://newapi.example.com/secretmanager/vaults/vault-id/secrets";

        var newBuilder = builder.WithUrl(newUrl);

        Assert.NotNull(newBuilder);
        Assert.NotSame(builder, newBuilder);
        Assert.IsType<SecretsRequestBuilder>(newBuilder);
        loggerFactory.Received(2).CreateLogger(Arg.Any<string>());
    }
}
