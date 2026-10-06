using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.IAM.Compat.ApiKeys.Item;
using Sacloud.Sdk.IAM.Models;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Sacloud.Sdk.IAM.Tests.Compat.ApiKeys.Item;

/// <summary>
/// WithApikey_ItemRequestBuilderの単体テスト
/// </summary>
public class WithApikey_ItemRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<WithApikey_ItemRequestBuilder> _mockLogger;

    public WithApikey_ItemRequestBuilderTests()
    {
        _mockLogger = Substitute.For<ILogger<WithApikey_ItemRequestBuilder>>();
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLoggerFactory.CreateLogger<WithApikey_ItemRequestBuilder>().Returns(_mockLogger);
    }

    /// <summary>
    /// PathParametersを使用したコンストラクタが正しく動作することを確認
    /// </summary>
    [Fact]
    public void WithApikey_ItemRequestBuilder_Constructor_WithPathParameters_SetsProperties()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://example.com" },
            { "apikey_id", "apikey123" }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();

        var builder = new WithApikey_ItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// rawUrlを使用したコンストラクタが正しく動作することを確認
    /// </summary>
    [Fact]
    public void WithApikey_ItemRequestBuilder_Constructor_WithRawUrl_SetsProperties()
    {
        var rawUrl = "https://example.com/compat/api-keys/apikey123";
        var requestAdapter = Substitute.For<IRequestAdapter>();

        var builder = new WithApikey_ItemRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// PathParametersを使用したコンストラクタにnullのILoggerFactoryを渡した時にArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void WithApikey_ItemRequestBuilder_Constructor_WithPathParametersAndNullLoggerFactory_ThrowsArgumentNullException()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://example.com" },
            { "apikey_id", "apikey123" }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();

        Assert.Throws<ArgumentNullException>(() =>
            new WithApikey_ItemRequestBuilder(pathParameters, requestAdapter, null));
    }

    /// <summary>
    /// rawUrlを使用したコンストラクタにnullのILoggerFactoryを渡した時にArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void WithApikey_ItemRequestBuilder_Constructor_WithRawUrlAndNullLoggerFactory_ThrowsArgumentNullException()
    {
        var rawUrl = "https://example.com/compat/api-keys/apikey123";
        var requestAdapter = Substitute.For<IRequestAdapter>();

        Assert.Throws<ArgumentNullException>(() =>
            new WithApikey_ItemRequestBuilder(rawUrl, requestAdapter, null));
    }

    /// <summary>
    /// DeleteAsyncが正常に完了することを確認
    /// </summary>
    [Fact]
    public async Task WithApikey_ItemRequestBuilder_DeleteAsync_CompletesSuccessfully()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://example.com" },
            { "apikey_id", "apikey123" }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(Task.CompletedTask);
        var builder = new WithApikey_ItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await builder.DeleteAsync();

        await requestAdapter.Received(1).SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^指定したAPIキーの削除を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^指定したAPIキーの削除が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 204\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// DeleteAsyncがnullのrequestConfigurationで正常に動作することを確認
    /// </summary>
    [Fact]
    public async Task WithApikey_ItemRequestBuilder_DeleteAsync_WithNullRequestConfiguration_CompletesSuccessfully()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://example.com" },
            { "apikey_id", "apikey123" }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(Task.CompletedTask);
        var builder = new WithApikey_ItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await builder.DeleteAsync(null);

        await requestAdapter.Received(1).SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^指定したAPIキーの削除を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^指定したAPIキーの削除が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 204\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// DeleteAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task WithApikey_ItemRequestBuilder_DeleteAsync_WithApiError_ThrowsApiException()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://example.com" },
            { "apikey_id", "apikey123" }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WithApikey_ItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError
        {
            ResponseStatusCode = 404
        };
        ((IAdditionalDataHolder)expectedException).AdditionalData = new Dictionary<string, object>
        {
            { "error_code", "not_found" },
            { "error_msg", "error-not-found" },
            { "is_fatal", true },
            { "serial", "1234567890" },
            { "status", "404 Not Found" }
        };

        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(async () =>
        {
            await builder.DeleteAsync();
        });
        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^指定したAPIキー削除のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 404\], \[error_code, not_found\], \[error_msg, error\-not\-found\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 404 Not Found\]$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// DeleteAsyncでその他のエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task WithApikey_ItemRequestBuilder_DeleteAsync_WithOtherError_ThrowsException()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://example.com" },
            { "apikey_id", "apikey123" }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WithApikey_ItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<Exception>(async () =>
        {
            await builder.DeleteAsync();
        });
        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^指定したAPIキーの削除中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncが正しいProjectApiKeyを返すことを確認
    /// </summary>
    [Fact]
    public async Task WithApikey_ItemRequestBuilder_GetAsync_ReturnsProjectApiKey()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://example.com" },
            { "apikey_id", "apikey123" }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var expectedResponse = new ProjectApiKey();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ProjectApiKey>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);
        var builder = new WithApikey_ItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var result = await builder.GetAsync();

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ProjectApiKey>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^指定したAPIキーの取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^指定したAPIキーの取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncがnullのrequestConfigurationで正常に動作することを確認
    /// </summary>
    [Fact]
    public async Task WithApikey_ItemRequestBuilder_GetAsync_WithNullRequestConfiguration_ReturnsProjectApiKey()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://example.com" },
            { "apikey_id", "apikey123" }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var expectedResponse = new ProjectApiKey();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ProjectApiKey>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);
        var builder = new WithApikey_ItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var result = await builder.GetAsync(null);

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ProjectApiKey>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^指定したAPIキーの取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^指定したAPIキーの取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task WithApikey_ItemRequestBuilder_GetAsync_WithApiError_ThrowsApiException()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://example.com" },
            { "apikey_id", "apikey123" }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WithApikey_ItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError
        {
            ResponseStatusCode = 404
        };
        ((IAdditionalDataHolder)expectedException).AdditionalData = new Dictionary<string, object>
        {
            { "error_code", "not_found" },
            { "error_msg", "error-not-found" },
            { "is_fatal", true },
            { "serial", "1234567890" },
            { "status", "404 Not Found" }
        };

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ProjectApiKey>>(),
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
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^指定したAPIキー取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 404\], \[error_code, not_found\], \[error_msg, error\-not\-found\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 404 Not Found\]$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncでその他のエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task WithApikey_ItemRequestBuilder_GetAsync_WithOtherError_ThrowsException()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://example.com" },
            { "apikey_id", "apikey123" }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WithApikey_ItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ProjectApiKey>>(),
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
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^指定したAPIキーの取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PutAsyncが正しいProjectApiKeyを返すことを確認
    /// </summary>
    [Fact]
    public async Task WithApikey_ItemRequestBuilder_PutAsync_ReturnsProjectApiKey()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://example.com" },
            { "apikey_id", "apikey123" }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var requestBody = new WithApikey_PutRequestBody
        {
            Description = "test-description",
            IamRoles = ["test-role"],
            Name = "test-name"
        };
        var expectedResponse = new ProjectApiKey();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ProjectApiKey>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);
        var builder = new WithApikey_ItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var result = await builder.PutAsync(requestBody);

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ProjectApiKey>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^指定したAPIキーの更新を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^指定したAPIキーの更新が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PutAsyncがnull bodyで例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task WithApikey_ItemRequestBuilder_PutAsync_WithNullBody_ThrowsValidationException()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://example.com" },
            { "apikey_id", "apikey123" }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WithApikey_ItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(null));
    }

    /// <summary>
    /// PutAsyncがnullのrequestConfigurationで正常に動作することを確認
    /// </summary>
    [Fact]
    public async Task WithApikey_ItemRequestBuilder_PutAsync_WithNullRequestConfiguration_ReturnsProjectApiKey()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://example.com" },
            { "apikey_id", "apikey123" }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var requestBody = new WithApikey_PutRequestBody
        {
            Description = "test-description",
            IamRoles = ["test-role"],
            Name = "test-name"
        };
        var expectedResponse = new ProjectApiKey();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ProjectApiKey>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);
        var builder = new WithApikey_ItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var result = await builder.PutAsync(requestBody, null);

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ProjectApiKey>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^指定したAPIキーの更新を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^指定したAPIキーの更新が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PutAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task WithApikey_ItemRequestBuilder_PutAsync_WithApiError_ThrowsApiException()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://example.com" },
            { "apikey_id", "apikey123" }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var requestBody = new WithApikey_PutRequestBody
        {
            Description = "test-description",
            IamRoles = ["test-role"],
            Name = "test-name"
        };
        var builder = new WithApikey_ItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError
        {
            ResponseStatusCode = 400
        };
        ((IAdditionalDataHolder)expectedException).AdditionalData = new Dictionary<string, object>
        {
            { "error_code", "bad_request" },
            { "error_msg", "error-bad-request" },
            { "is_fatal", true },
            { "serial", "1234567890" },
            { "status", "400 Bad Request" }
        };

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ProjectApiKey>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(async () =>
        {
            await builder.PutAsync(requestBody);
        });
        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^指定したAPIキー更新のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 400\], \[error_code, bad_request\], \[error_msg, error\-bad\-request\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 400 Bad Request\]$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PutAsyncでその他のエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task WithApikey_ItemRequestBuilder_PutAsync_WithOtherError_ThrowsException()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://example.com" },
            { "apikey_id", "apikey123" }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var requestBody = new WithApikey_PutRequestBody
        {
            Description = "test-description",
            IamRoles = ["test-role"],
            Name = "test-name"
        };
        var builder = new WithApikey_ItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ProjectApiKey>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<Exception>(async () =>
        {
            await builder.PutAsync(requestBody);
        });
        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^指定したAPIキーの更新中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// ToDeleteRequestInformationが正しいRequestInformationを返すことを確認
    /// </summary>
    [Fact]
    public void WithApikey_ItemRequestBuilder_ToDeleteRequestInformation_ReturnsValidRequestInformation()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://example.com" },
            { "apikey_id", "apikey123" }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WithApikey_ItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToDeleteRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.DELETE, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToGetRequestInformationが正しいRequestInformationを返すことを確認
    /// </summary>
    [Fact]
    public void WithApikey_ItemRequestBuilder_ToGetRequestInformation_ReturnsValidRequestInformation()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://example.com" },
            { "apikey_id", "apikey123" }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WithApikey_ItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.GET, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToPutRequestInformationが正しいRequestInformationを返すことを確認
    /// </summary>
    [Fact]
    public void WithApikey_ItemRequestBuilder_ToPutRequestInformation_ReturnsValidRequestInformation()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://example.com" },
            { "apikey_id", "apikey123" }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WithApikey_ItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var requestBody = new WithApikey_PutRequestBody();

        var requestInfo = builder.ToPutRequestInformation(requestBody);

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.PUT, requestInfo.HttpMethod);
    }

    /// <summary>
    /// DeleteAsyncに不正なパスパラメータが渡された場合にValidationExceptionをスローすることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("apikey_id", "")]
    [InlineData("apikey_id", " ")]
    [InlineData("apikey_id", "\u3000")]
    public async Task DeleteAsync_WithInvalidPathParameters_ThrowsValidationException(string key, string value)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { key, value }
        };
        var builder = new WithApikey_ItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await Assert.ThrowsAsync<ValidationException>(() => builder.DeleteAsync());
    }

    /// <summary>
    /// GetAsyncに不正なパスパラメータが渡された場合にValidationExceptionをスローすることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("apikey_id", "")]
    [InlineData("apikey_id", " ")]
    [InlineData("apikey_id", "\u3000")]
    public async Task GetAsync_WithInvalidPathParameters_ThrowsValidationException(string key, string value)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { key, value }
        };
        var builder = new WithApikey_ItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync());
    }

    /// <summary>
    /// PutAsyncに不正なパスパラメータが渡された場合にValidationExceptionをスローすることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("apikey_id", "")]
    [InlineData("apikey_id", " ")]
    [InlineData("apikey_id", "\u3000")]
    public async Task PutAsync_WithInvalidPathParameters_ThrowsValidationException(string key, string value)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { key, value }
        };
        var builder = new WithApikey_ItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new WithApikey_PutRequestBody
        {
            Description = "test-description",
            IamRoles = ["test-role"],
            Name = "test-name"
        };

        await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
    }

    /// <summary>
    /// PutAsyncにnullが含まれるリクエストボディが渡された場合にValidationExceptionをスローすることを確認
    /// </summary>
    [Theory]
    [InlineData("Description")]
    [InlineData("IamRoles")]
    [InlineData("Name")]
    public async Task PutAsync_WithNullBodyParameter_ThrowsValidationException(string nullParam)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "apikey_id", "test-apikey-id" }
        };
        var builder = new WithApikey_ItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new WithApikey_PutRequestBody
        {
            Description = nullParam == "Description" ? null : "test-description",
            IamRoles = nullParam == "IamRoles" ? null : ["test-role"],
            Name = nullParam == "Name" ? null : "test-name"
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));
        Assert.Equal($"{nullParam}が入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{nullParam}が入力されていません。");
    }

    /// <summary>
    /// WithUrlが新しいWithApikey_ItemRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void WithApikey_ItemRequestBuilder_WithUrl_ReturnsNewBuilder()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://example.com" },
            { "apikey_id", "apikey123" }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WithApikey_ItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var newUrl = "https://example.com/compat/api-keys/new-apikey";

        var newBuilder = builder.WithUrl(newUrl);

        Assert.NotNull(newBuilder);
        Assert.IsType<WithApikey_ItemRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }
}
