using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.NetworkingSuite.NetworkingSuite.InterfaceConnections.Item;
using Sacloud.Sdk.Shared.Models;

namespace Sacloud.Sdk.NetworkingSuite.Tests.NetworkingSuite.InterfaceConnections.Item;

/// <summary>
/// WithInterfaceconnectionItemRequestBuilderのテスト
/// </summary>
public class WithInterfaceconnectionItemRequestBuilderTests
{
    private readonly IRequestAdapter _mockRequestAdapter;
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<WithInterfaceconnectionItemRequestBuilder> _mockLogger;

    public WithInterfaceconnectionItemRequestBuilderTests()
    {
        _mockRequestAdapter = Substitute.For<IRequestAdapter>();
        _mockRequestAdapter.BaseUrl = "https://api.example.com";
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<WithInterfaceconnectionItemRequestBuilder>>();
        _mockLoggerFactory.CreateLogger<WithInterfaceconnectionItemRequestBuilder>().Returns(_mockLogger);
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
            { "interfaceconnectionid", "test-interfaceconnectionid" },
        };

        var builder = new WithInterfaceconnectionItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（PathParameters版）がnullのLoggerFactoryで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_WithNullLoggerFactory_ThrowsArgumentNullException()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "interfaceconnectionid", "test-interfaceconnectionid" },
        };

        Assert.Throws<ArgumentNullException>(() => new WithInterfaceconnectionItemRequestBuilder(pathParameters, _mockRequestAdapter, null));
    }

    /// <summary>
    /// コンストラクタ（RawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_InitializesSuccessfully()
    {
        var rawUrl = "https://api.example.com/networking-suite/interface-connections/test-interfaceconnectionid";

        var builder = new WithInterfaceconnectionItemRequestBuilder(rawUrl, _mockRequestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（RawUrl版）がnullのLoggerFactoryで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_WithNullLoggerFactory_ThrowsArgumentNullException()
    {
        var rawUrl = "https://api.example.com/networking-suite/interface-connections/test-interfaceconnectionid";

        Assert.Throws<ArgumentNullException>(() => new WithInterfaceconnectionItemRequestBuilder(rawUrl, _mockRequestAdapter, null));
    }

    /// <summary>
    /// ToDeleteRequestInformationがDELETEメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToDeleteRequestInformation_CreatesRequestInformationWithDeleteMethod()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "interfaceconnectionid", "test-interfaceconnectionid" },
        };
        var builder = new WithInterfaceconnectionItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToDeleteRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.DELETE, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToDeleteRequestInformationが正しいAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToDeleteRequestInformation_SetsAcceptHeader()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "interfaceconnectionid", "test-interfaceconnectionid" },
        };
        var builder = new WithInterfaceconnectionItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToDeleteRequestInformation();

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// ToDeleteRequestInformationが正しいPathParametersを設定することを確認
    /// </summary>
    [Fact]
    public void ToDeleteRequestInformation_SetsPathParameters()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "interfaceconnectionid", "test-interfaceconnectionid" },
        };
        var builder = new WithInterfaceconnectionItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToDeleteRequestInformation();

        Assert.True(requestInfo.PathParameters.ContainsKey("interfaceconnectionid"));
        Assert.Equal("test-interfaceconnectionid", requestInfo.PathParameters["interfaceconnectionid"]);
    }

    /// <summary>
    /// DeleteAsync が期待されるSendNoContentAsyncを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_CallsRequestAdapterSendNoContentAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "interfaceconnectionid", "test-interfaceconnectionid" },
        };
        var builder = new WithInterfaceconnectionItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        await builder.DeleteAsync();

        await requestAdapter.Received(1).SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => string.Concat(state).Contains("インターフェースコネクションの削除を開始します。")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => string.Concat(state).Contains("インターフェースコネクションの削除が完了しました。")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// DeleteAsyncで無効なインターフェースコネクションIDを指定した場合にValidationExceptionがスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("　")]
    public async Task DeleteAsync_WithNullOrEmptyOrWhitespaceInterfaceConnectionId_ThrowsValidationException(string? invalidInterfaceConnectionId)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object?>
        {
            { "baseurl", "https://api.example.com" },
            { "interfaceconnectionid", invalidInterfaceConnectionId },
        };
        var builder = new WithInterfaceconnectionItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.DeleteAsync());

        Assert.Equal("インターフェースコネクションIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("インターフェースコネクションIDを入力してください。");
    }

    /// <summary>
    /// DeleteAsyncが"XXX"キーに汎用ApiErrorのエラーマッピングを使用することを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_SendsWithXxxErrorMapping()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "interfaceconnectionid", "test-interfaceconnectionid" },
        };
        var builder = new WithInterfaceconnectionItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await builder.DeleteAsync();

        await requestAdapter.Received(1).SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Is<Dictionary<string, ParsableFactory<IParsable>>>(x =>
                x.ContainsKey("XXX")
                && x["XXX"] == ApiError.CreateFromDiscriminatorValue),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// DeleteAsyncで汎用ApiErrorがスローされた場合に再スローされることを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_WithApiError_ThrowsApiError()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "interfaceconnectionid", "test-interfaceconnectionid" },
        };
        var builder = new WithInterfaceconnectionItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError();
        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException(expectedException));

        var exception = await Assert.ThrowsAsync<ApiError>(() => builder.DeleteAsync());

        Assert.Same(expectedException, exception);
        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => string.Concat(state).Contains("インターフェースコネクション削除のAPIエラーが発生しました。")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// DeleteAsyncでその他の例外がスローされた場合に再スローされることを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_WithOtherException_RethrowsException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "interfaceconnectionid", "test-interfaceconnectionid" },
        };
        var builder = new WithInterfaceconnectionItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new InvalidOperationException("test exception");
        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException(expectedException));

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => builder.DeleteAsync());

        Assert.Same(expectedException, exception);
        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => string.Concat(state).Contains("インターフェースコネクションの削除中にエラーが発生しました。")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// WithUrlが新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewInstance()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "interfaceconnectionid", "test-interfaceconnectionid" },
        };
        var builder = new WithInterfaceconnectionItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);
        var newUrl = "https://newapi.example.com/networking-suite/interface-connections/new-interfaceconnectionid";

        var newBuilder = builder.WithUrl(newUrl);

        Assert.NotNull(newBuilder);
        Assert.NotSame(builder, newBuilder);
        Assert.IsType<WithInterfaceconnectionItemRequestBuilder>(newBuilder);
    }
}
