using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.IAM.ServicePrincipals.Item.Keys.Item;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Sacloud.Sdk.IAM.Tests.ServicePrincipals.Item.Keys.Item;

/// <summary>
/// WithService_principal_key_ItemRequestBuilderのテスト
/// </summary>
public class WithService_principal_key_ItemRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<WithService_principal_key_ItemRequestBuilder> _mockLogger;

    public WithService_principal_key_ItemRequestBuilderTests()
    {
        _mockLogger = Substitute.For<ILogger<WithService_principal_key_ItemRequestBuilder>>();
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLoggerFactory.CreateLogger<WithService_principal_key_ItemRequestBuilder>().Returns(_mockLogger);
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
            { "baseurl", "https://api.example.com" },
            { "service_principal_id", 12345L },
            { "service_principal_key_id", 67890L }
        };

        var builder = new WithService_principal_key_ItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var rawUrl = "https://api.example.com/service-principals/12345/keys/67890";

        var builder = new WithService_principal_key_ItemRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（PathParameters版）にnullのILoggerFactoryを渡した時にArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParametersAndNullLoggerFactory_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "service_principal_id", 12345L },
            { "service_principal_key_id", 67890L }
        };

        Assert.Throws<ArgumentNullException>(() =>
            new WithService_principal_key_ItemRequestBuilder(pathParameters, requestAdapter, null));
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）にnullのILoggerFactoryを渡した時にArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrlAndNullLoggerFactory_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var rawUrl = "https://api.example.com/service-principals/12345/keys/67890";

        Assert.Throws<ArgumentNullException>(() =>
            new WithService_principal_key_ItemRequestBuilder(rawUrl, requestAdapter, null));
    }

    /// <summary>
    /// Disableプロパティが正しいDisableRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Disable_ReturnsDisableRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "service_principal_id", 12345L },
            { "service_principal_key_id", 67890L }
        };
        var builder = new WithService_principal_key_ItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var disableBuilder = builder.Disable;

        Assert.NotNull(disableBuilder);
    }

    /// <summary>
    /// Enableプロパティが正しいEnableRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Enable_ReturnsEnableRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "service_principal_id", 12345L },
            { "service_principal_key_id", 67890L }
        };
        var builder = new WithService_principal_key_ItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var enableBuilder = builder.Enable;

        Assert.NotNull(enableBuilder);
    }

    /// <summary>
    /// ToDeleteRequestInformationがDELETEメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToDeleteRequestInformation_CreatesRequestInformationWithDeleteMethod()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "service_principal_id", 12345L },
            { "service_principal_key_id", 67890L }
        };
        var builder = new WithService_principal_key_ItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToDeleteRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.DELETE, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToDeleteRequestInformationが正しいURLテンプレートを設定することを確認
    /// </summary>
    [Fact]
    public void ToDeleteRequestInformation_SetsCorrectUrlTemplate()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "service_principal_id", 12345L },
            { "service_principal_key_id", 67890L }
        };
        var builder = new WithService_principal_key_ItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToDeleteRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Contains("keys", requestInfo.UrlTemplate);
        Assert.Contains("service_principal_key_id", requestInfo.UrlTemplate);
    }

    /// <summary>
    /// DeleteAsyncが正常に動作することを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_CompletesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "service_principal_id", 12345L },
            { "service_principal_key_id", "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa" }
        };
        var builder = new WithService_principal_key_ItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(Task.CompletedTask);

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
                @"^指定したサービスプリンシパルキーの削除を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^指定したサービスプリンシパルキーの削除が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 204\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// DeleteAsyncがCancellationTokenを受け取れることを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_AcceptsCancellationToken()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "service_principal_id", 12345L },
            { "service_principal_key_id", "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa" }
        };
        var builder = new WithService_principal_key_ItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(Task.CompletedTask);
        var cancellationToken = new CancellationToken();

        await builder.DeleteAsync(cancellationToken: cancellationToken);

        await requestAdapter.Received(1).SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^指定したサービスプリンシパルキーの削除を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^指定したサービスプリンシパルキーの削除が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 204\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// DeleteAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_WithApiError_ThrowsApiException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "service_principal_id", 12345L },
            { "service_principal_key_id", "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa" }
        };
        var builder = new WithService_principal_key_ItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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
                @"^指定したサービスプリンシパルキー削除のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 404\], \[error_code, not_found\], \[error_msg, error\-not\-found\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 404 Not Found\]$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// DeleteAsyncでその他のエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_WithOtherError_ThrowsException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "service_principal_id", 12345L },
            { "service_principal_key_id", "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa" }
        };
        var builder = new WithService_principal_key_ItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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
                @"^指定したサービスプリンシパルキーの削除中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// DeleteAsyncに不正なパスパラメータが渡された場合にValidationExceptionをスローすることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value", "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")]
    [InlineData("service_principal_id", "", "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")]
    [InlineData("service_principal_id", " ", "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")]
    [InlineData("service_principal_id", "\u3000", "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")]
    [InlineData("service_principal_key_id", "test-service-principal-id", "")]
    [InlineData("service_principal_key_id", "test-service-principal-id", " ")]
    [InlineData("service_principal_key_id", "test-service-principal-id", "\u3000")]
    [InlineData("service_principal_key_id", "test-service-principal-id", "not-a-guid")]
    public async Task DeleteAsync_WithInvalidPathParameters_ThrowsValidationException(string invalidKey, string servicePrincipalIdValue, string servicePrincipalKeyIdValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "service_principal_id", invalidKey == "service_principal_id" ? (object)servicePrincipalIdValue : servicePrincipalIdValue },
            { "service_principal_key_id", invalidKey == "service_principal_key_id" ? (object)servicePrincipalKeyIdValue : servicePrincipalKeyIdValue }
        };
        if (invalidKey == "invalidKey")
        {
            pathParameters.Remove("service_principal_id");
            pathParameters["invalidKey"] = servicePrincipalIdValue;
        }
        var builder = new WithService_principal_key_ItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await Assert.ThrowsAsync<ValidationException>(() => builder.DeleteAsync());
    }

    /// <summary>
    /// WithUrlが正しいWithService_principal_key_ItemRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewWithService_principal_key_ItemRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "service_principal_id", 12345L },
            { "service_principal_key_id", 67890L }
        };
        var builder = new WithService_principal_key_ItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var rawUrl = "https://api.example.com/service-principals/12345/keys/67890";

        var newBuilder = builder.WithUrl(rawUrl);

        Assert.NotNull(newBuilder);
    }

    /// <summary>
    /// WithUrlが異なるインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsDifferentInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "service_principal_id", 12345L },
            { "service_principal_key_id", 67890L }
        };
        var builder = new WithService_principal_key_ItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var rawUrl = "https://api.example.com/service-principals/12345/keys/67890";

        var newBuilder = builder.WithUrl(rawUrl);

        Assert.NotSame(builder, newBuilder);
    }
}
