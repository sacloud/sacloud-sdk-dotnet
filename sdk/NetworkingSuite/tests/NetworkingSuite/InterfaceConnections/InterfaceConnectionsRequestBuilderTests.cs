using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.NetworkingSuite.Models;
using Sacloud.Sdk.NetworkingSuite.NetworkingSuite.InterfaceConnections;
using Sacloud.Sdk.NetworkingSuite.NetworkingSuite.InterfaceConnections.Item;
using Sacloud.Sdk.Shared.Models;

namespace Sacloud.Sdk.NetworkingSuite.Tests.NetworkingSuite.InterfaceConnections;

/// <summary>
/// InterfaceConnectionsRequestBuilderのテスト
/// </summary>
public class InterfaceConnectionsRequestBuilderTests
{
    private readonly IRequestAdapter _mockRequestAdapter;
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<InterfaceConnectionsRequestBuilder> _mockLogger;
    private readonly ILogger<WithInterfaceconnectionItemRequestBuilder> _mockItemLogger;

    public InterfaceConnectionsRequestBuilderTests()
    {
        _mockRequestAdapter = Substitute.For<IRequestAdapter>();
        _mockRequestAdapter.BaseUrl = "https://api.example.com";
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<InterfaceConnectionsRequestBuilder>>();
        _mockItemLogger = Substitute.For<ILogger<WithInterfaceconnectionItemRequestBuilder>>();
        _mockLoggerFactory.CreateLogger<InterfaceConnectionsRequestBuilder>().Returns(_mockLogger);
        _mockLoggerFactory.CreateLogger<WithInterfaceconnectionItemRequestBuilder>().Returns(_mockItemLogger);
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
        };

        var builder = new InterfaceConnectionsRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

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
        };

        Assert.Throws<ArgumentNullException>(() => new InterfaceConnectionsRequestBuilder(pathParameters, _mockRequestAdapter, null));
    }

    /// <summary>
    /// コンストラクタ（RawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_InitializesSuccessfully()
    {
        var rawUrl = "https://api.example.com/networking-suite/interface-connections";

        var builder = new InterfaceConnectionsRequestBuilder(rawUrl, _mockRequestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（RawUrl版）がnullのLoggerFactoryで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_WithNullLoggerFactory_ThrowsArgumentNullException()
    {
        var rawUrl = "https://api.example.com/networking-suite/interface-connections";

        Assert.Throws<ArgumentNullException>(() => new InterfaceConnectionsRequestBuilder(rawUrl, _mockRequestAdapter, null));
    }

    /// <summary>
    /// インデクサーが正しいWithInterfaceconnectionItemRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_ReturnsWithInterfaceconnectionItemRequestBuilder()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new InterfaceConnectionsRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var itemBuilder = builder["test-interfaceconnectionid"];

        Assert.NotNull(itemBuilder);
        Assert.IsType<WithInterfaceconnectionItemRequestBuilder>(itemBuilder);
    }

    /// <summary>
    /// インデクサーが毎回新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_ReturnsNewInstance_OnEachCall()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new InterfaceConnectionsRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var itemBuilder1 = builder["test-interfaceconnectionid"];
        var itemBuilder2 = builder["test-interfaceconnectionid"];

        Assert.NotSame(itemBuilder1, itemBuilder2);
    }

    /// <summary>
    /// ToPostRequestInformationがPOSTメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_CreatesRequestInformationWithPostMethod()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new InterfaceConnectionsRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);
        var body = new CreateInterfaceConnection
        {
            Interface = new SakuraResourceNameRef
            {
                SRN = "srnv1:sakura-is1c:sakura.iaas.interface:3456789012",
            },
            Subnet = new SakuraResourceNameRef
            {
                SRN = "srnv1:sakura-is1c:sakura.networking-suite.subnet:2345678901",
            },
        };

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
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new InterfaceConnectionsRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);
        var body = new CreateInterfaceConnection
        {
            Interface = new SakuraResourceNameRef
            {
                SRN = "srnv1:sakura-is1c:sakura.iaas.interface:3456789012",
            },
            Subnet = new SakuraResourceNameRef
            {
                SRN = "srnv1:sakura-is1c:sakura.networking-suite.subnet:2345678901",
            },
        };

        var requestInfo = builder.ToPostRequestInformation(body);

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// ToPostRequestInformationが正しいContent-Typeヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_SetsContentTypeHeader()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new InterfaceConnectionsRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);
        var body = new CreateInterfaceConnection
        {
            Interface = new SakuraResourceNameRef
            {
                SRN = "srnv1:sakura-is1c:sakura.iaas.interface:3456789012",
            },
            Subnet = new SakuraResourceNameRef
            {
                SRN = "srnv1:sakura-is1c:sakura.networking-suite.subnet:2345678901",
            },
        };

        var requestInfo = builder.ToPostRequestInformation(body);

        Assert.True(requestInfo.Headers.ContainsKey("Content-Type"));
        Assert.Contains("application/json", requestInfo.Headers["Content-Type"]);
    }

    /// <summary>
    /// PostAsync が期待されるSendAsyncを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_CallsRequestAdapterSendAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new InterfaceConnectionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var mockResponse = new CreatedInterfaceConnectionResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreatedInterfaceConnectionResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(mockResponse);

        var body = new CreateInterfaceConnection
        {
            Interface = new SakuraResourceNameRef
            {
                SRN = "srnv1:sakura-is1c:sakura.iaas.interface:3456789012",
            },
            Subnet = new SakuraResourceNameRef
            {
                SRN = "srnv1:sakura-is1c:sakura.networking-suite.subnet:2345678901",
            },
        };

        var result = await builder.PostAsync(body);

        Assert.NotNull(result);
        Assert.IsType<CreatedInterfaceConnectionResponse>(result);

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreatedInterfaceConnectionResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => string.Concat(state).Contains("インターフェースコネクションの作成を開始します。")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => string.Concat(state).Contains("インターフェースコネクションの作成が完了しました。")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncがnullボディで例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithNullBody_ThrowsValidationException()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new InterfaceConnectionsRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(null));

        Assert.Equal("リクエストボディは必須です。", exception.Message);
        _mockLogger.Received(1).LogWarning("リクエストボディは必須です。");
    }

    /// <summary>
    /// PostAsyncが"XXX"キーに汎用ApiErrorのエラーマッピングを使用することを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_SendsWithXxxErrorMapping()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new InterfaceConnectionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreatedInterfaceConnectionResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(new CreatedInterfaceConnectionResponse());

        var body = new CreateInterfaceConnection
        {
            Interface = new SakuraResourceNameRef
            {
                SRN = "srnv1:sakura-is1c:sakura.iaas.interface:3456789012",
            },
            Subnet = new SakuraResourceNameRef
            {
                SRN = "srnv1:sakura-is1c:sakura.networking-suite.subnet:2345678901",
            },
        };

        await builder.PostAsync(body);

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreatedInterfaceConnectionResponse>>(),
            Arg.Is<Dictionary<string, ParsableFactory<IParsable>>>(x =>
                x.ContainsKey("XXX")
                && x["XXX"] == ApiError.CreateFromDiscriminatorValue),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// PostAsyncで汎用ApiErrorがスローされた場合に再スローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithApiError_ThrowsApiError()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new InterfaceConnectionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreatedInterfaceConnectionResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(_ => Task.FromException<CreatedInterfaceConnectionResponse?>(expectedException));

        var body = new CreateInterfaceConnection
        {
            Interface = new SakuraResourceNameRef
            {
                SRN = "srnv1:sakura-is1c:sakura.iaas.interface:3456789012",
            },
            Subnet = new SakuraResourceNameRef
            {
                SRN = "srnv1:sakura-is1c:sakura.networking-suite.subnet:2345678901",
            },
        };

        var exception = await Assert.ThrowsAsync<ApiError>(() => builder.PostAsync(body));

        Assert.Same(expectedException, exception);
        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => string.Concat(state).Contains("インターフェースコネクション作成のAPIエラーが発生しました。")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncでその他の例外がスローされた場合に再スローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithOtherException_RethrowsException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new InterfaceConnectionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new InvalidOperationException("test exception");
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreatedInterfaceConnectionResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(_ => Task.FromException<CreatedInterfaceConnectionResponse?>(expectedException));

        var body = new CreateInterfaceConnection
        {
            Interface = new SakuraResourceNameRef
            {
                SRN = "srnv1:sakura-is1c:sakura.iaas.interface:3456789012",
            },
            Subnet = new SakuraResourceNameRef
            {
                SRN = "srnv1:sakura-is1c:sakura.networking-suite.subnet:2345678901",
            },
        };

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => builder.PostAsync(body));

        Assert.Same(expectedException, exception);
        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => string.Concat(state).Contains("インターフェースコネクションの作成中にエラーが発生しました。")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncのリクエストボディのInterfaceがnullの場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithNullInterface_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new InterfaceConnectionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateInterfaceConnection
        {
            Interface = null,
            Subnet = new SakuraResourceNameRef
            {
                SRN = "srnv1:sakura-is1c:sakura.networking-suite.subnet:2345678901",
            },
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal("Interfaceが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("Interfaceが入力されていません。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのSubnetがnullの場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithNullSubnet_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new InterfaceConnectionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateInterfaceConnection
        {
            Interface = new SakuraResourceNameRef
            {
                SRN = "srnv1:sakura-is1c:sakura.iaas.interface:3456789012",
            },
            Subnet = null,
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal("Subnetが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("Subnetが入力されていません。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのInterface.SRNがnull、空文字列、空白文字の場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("　")]
    public async Task PostAsync_WithNullOrEmptyOrWhitespaceInterfaceSrn_ThrowsValidationException(string? invalidInterfaceSrn)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new InterfaceConnectionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateInterfaceConnection
        {
            Interface = new SakuraResourceNameRef
            {
                SRN = invalidInterfaceSrn,
            },
            Subnet = new SakuraResourceNameRef
            {
                SRN = "srnv1:sakura-is1c:sakura.networking-suite.subnet:2345678901",
            },
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal("SRNが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("SRNが入力されていません。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのSubnet.SRNがnull、空文字列、空白文字の場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("　")]
    public async Task PostAsync_WithNullOrEmptyOrWhitespaceSubnetSrn_ThrowsValidationException(string? invalidSubnetSrn)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new InterfaceConnectionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateInterfaceConnection
        {
            Interface = new SakuraResourceNameRef
            {
                SRN = "srnv1:sakura-is1c:sakura.iaas.interface:3456789012",
            },
            Subnet = new SakuraResourceNameRef
            {
                SRN = invalidSubnetSrn,
            },
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal("SRNが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("SRNが入力されていません。");
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
        };
        var builder = new InterfaceConnectionsRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);
        var newUrl = "https://newapi.example.com/networking-suite/interface-connections";

        var newBuilder = builder.WithUrl(newUrl);

        Assert.NotNull(newBuilder);
        Assert.NotSame(builder, newBuilder);
        Assert.IsType<InterfaceConnectionsRequestBuilder>(newBuilder);
    }

    /// <summary>
    /// インデクサーがLoggerFactoryをitem RequestBuilderへ引き渡すことを確認
    /// </summary>
    [Fact]
    public void Indexer_PropagatesLoggerFactory()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new InterfaceConnectionsRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var itemBuilder = builder["test-interfaceconnectionid"];

        Assert.NotNull(itemBuilder);
        _mockLoggerFactory.Received(1).CreateLogger<WithInterfaceconnectionItemRequestBuilder>();
    }
}
