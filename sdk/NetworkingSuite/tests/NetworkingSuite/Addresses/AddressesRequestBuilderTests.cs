using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.NetworkingSuite.Models;
using Sacloud.Sdk.NetworkingSuite.NetworkingSuite.Addresses;
using Sacloud.Sdk.NetworkingSuite.NetworkingSuite.Addresses.Item;
using Sacloud.Sdk.Shared.Models;

namespace Sacloud.Sdk.NetworkingSuite.Tests.NetworkingSuite.Addresses;

/// <summary>
/// AddressesRequestBuilderのテスト
/// </summary>
public class AddressesRequestBuilderTests
{
    private readonly IRequestAdapter _mockRequestAdapter;
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<AddressesRequestBuilder> _mockLogger;

    public AddressesRequestBuilderTests()
    {
        _mockRequestAdapter = Substitute.For<IRequestAdapter>();
        _mockRequestAdapter.BaseUrl = "https://api.example.com";
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<AddressesRequestBuilder>>();
        _mockLoggerFactory.CreateLogger<AddressesRequestBuilder>().Returns(_mockLogger);
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

        var builder = new AddressesRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

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

        Assert.Throws<ArgumentNullException>(() => new AddressesRequestBuilder(pathParameters, _mockRequestAdapter, null));
    }

    /// <summary>
    /// コンストラクタ（RawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_InitializesSuccessfully()
    {
        var rawUrl = "https://api.example.com/networking-suite/addresses?subnetSRN={subnetSRN}{&interfaceConnectionSRN}";

        var builder = new AddressesRequestBuilder(rawUrl, _mockRequestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（RawUrl版）がnullのLoggerFactoryで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_WithNullLoggerFactory_ThrowsArgumentNullException()
    {
        var rawUrl = "https://api.example.com/networking-suite/addresses?subnetSRN={subnetSRN}{&interfaceConnectionSRN}";

        Assert.Throws<ArgumentNullException>(() => new AddressesRequestBuilder(rawUrl, _mockRequestAdapter, null));
    }

    /// <summary>
    /// インデクサーが正しいWithAddressItemRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_ReturnsWithAddressItemRequestBuilder()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new AddressesRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var itemBuilder = builder["test-addressid"];

        Assert.NotNull(itemBuilder);
        Assert.IsType<WithAddressItemRequestBuilder>(itemBuilder);
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
        var builder = new AddressesRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var itemBuilder1 = builder["test-addressid"];
        var itemBuilder2 = builder["test-addressid"];

        Assert.NotSame(itemBuilder1, itemBuilder2);
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
        var builder = new AddressesRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var itemBuilder = builder["test-addressid"];

        Assert.NotNull(itemBuilder);
        _mockLoggerFactory.Received(1).CreateLogger<WithAddressItemRequestBuilder>();
    }

    /// <summary>
    /// ToGetRequestInformationがGETメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_CreatesRequestInformationWithGetMethod()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new AddressesRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.GET, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToGetRequestInformationが正しいAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_SetsAcceptHeader()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new AddressesRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// ToGetRequestInformationがクエリパラメータを設定してリクエストに反映することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_WithQueryParameters_ConfiguresCorrectly()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new AddressesRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation(config =>
        {
        });

        Assert.NotNull(requestInfo);
    }

    /// <summary>
    /// GetAsync が期待されるSendAsyncを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_CallsRequestAdapterSendAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new AddressesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var mockResponse = new AddressListResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<AddressListResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(mockResponse);

        var result = await builder.GetAsync(config =>
        {
            config.QueryParameters.SubnetSRN = "test-subnet-srn";
        });

        Assert.NotNull(result);
        Assert.IsType<AddressListResponse>(result);

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<AddressListResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => string.Concat(state).Contains("アドレス一覧の取得を開始します。")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => string.Concat(state).Contains("アドレス一覧の取得が完了しました。")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncで必須のSubnetSRNを指定しない場合にValidationExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithoutRequestConfiguration_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new AddressesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync());

        Assert.Equal("subnetSRNが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("subnetSRNが入力されていません。");
    }

    /// <summary>
    /// GetAsyncで無効なSubnetSRNを指定した場合にValidationExceptionがスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("　")]
    public async Task GetAsync_WithNullOrEmptyOrWhitespaceSubnetSRN_ThrowsValidationException(string? invalidSubnetSRN)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new AddressesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync(config =>
        {
            config.QueryParameters.SubnetSRN = invalidSubnetSRN;
        }));

        Assert.Equal("subnetSRNが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("subnetSRNが入力されていません。");
    }

    /// <summary>
    /// GetAsyncがXXXキーに汎用ApiErrorのエラーマッピングを使用することを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_SendsWithXxxErrorMapping()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new AddressesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<AddressListResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(new AddressListResponse());

        await builder.GetAsync(config =>
        {
            config.QueryParameters.SubnetSRN = "test-subnet-srn";
        });

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<AddressListResponse>>(),
            Arg.Is<Dictionary<string, ParsableFactory<IParsable>>>(errorMapping =>
                errorMapping.ContainsKey("XXX")
                && errorMapping["XXX"] == ApiError.CreateFromDiscriminatorValue),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// GetAsyncで汎用ApiErrorがスローされた場合に再スローされることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithApiError_ThrowsApiError()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new AddressesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<AddressListResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<AddressListResponse?>(expectedException));

        var exception = await Assert.ThrowsAsync<ApiError>(() => builder.GetAsync(config =>
        {
            config.QueryParameters.SubnetSRN = "test-subnet-srn";
        }));

        Assert.Same(expectedException, exception);
        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => string.Concat(state).Contains("アドレス一覧取得のAPIエラーが発生しました。")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncでその他の例外がスローされた場合に再スローされることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithOtherException_RethrowsException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new AddressesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new InvalidOperationException("test exception");
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<AddressListResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<AddressListResponse?>(expectedException));

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => builder.GetAsync(config =>
        {
            config.QueryParameters.SubnetSRN = "test-subnet-srn";
        }));

        Assert.Same(expectedException, exception);
        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => string.Concat(state).Contains("アドレス一覧の取得中にエラーが発生しました。")),
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
        };
        var builder = new AddressesRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);
        var newUrl = "https://newapi.example.com/networking-suite/addresses?subnetSRN={subnetSRN}{&interfaceConnectionSRN}";

        var newBuilder = builder.WithUrl(newUrl);

        Assert.NotNull(newBuilder);
        Assert.NotSame(builder, newBuilder);
        Assert.IsType<AddressesRequestBuilder>(newBuilder);
    }
}
