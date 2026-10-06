using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using NSubstitute;
using Sacloud.Sdk.NetworkingSuite.Models;
using Sacloud.Sdk.NetworkingSuite.NetworkingSuite.Subnets.Item;
using Sacloud.Sdk.NetworkingSuite.NetworkingSuite.Subnets;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.NetworkingSuite.Tests.NetworkingSuite.Subnets;

/// <summary>
/// SubnetsRequestBuilderのテスト
/// </summary>
public class SubnetsRequestBuilderTests
{
    private readonly IRequestAdapter _mockRequestAdapter;
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<SubnetsRequestBuilder> _mockLogger;
    private readonly ILogger<WithSubnetItemRequestBuilder> _mockItemLogger;

    public SubnetsRequestBuilderTests()
    {
        _mockRequestAdapter = Substitute.For<IRequestAdapter>();
        _mockRequestAdapter.BaseUrl = "https://api.example.com";
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<SubnetsRequestBuilder>>();
        _mockItemLogger = Substitute.For<ILogger<WithSubnetItemRequestBuilder>>();
        _mockLoggerFactory.CreateLogger<SubnetsRequestBuilder>().Returns(_mockLogger);
        _mockLoggerFactory.CreateLogger<WithSubnetItemRequestBuilder>().Returns(_mockItemLogger);
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

        var builder = new SubnetsRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

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

        Assert.Throws<ArgumentNullException>(() => new SubnetsRequestBuilder(pathParameters, _mockRequestAdapter, null));
    }

    /// <summary>
    /// コンストラクタ（RawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_InitializesSuccessfully()
    {
        var rawUrl = "https://api.example.com/networking-suite/subnets?subnetGroupSRN={subnetGroupSRN}";

        var builder = new SubnetsRequestBuilder(rawUrl, _mockRequestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（RawUrl版）がnullのLoggerFactoryで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_WithNullLoggerFactory_ThrowsArgumentNullException()
    {
        var rawUrl = "https://api.example.com/networking-suite/subnets?subnetGroupSRN={subnetGroupSRN}";

        Assert.Throws<ArgumentNullException>(() => new SubnetsRequestBuilder(rawUrl, _mockRequestAdapter, null));
    }

    /// <summary>
    /// インデクサーが正しいWithSubnetItemRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_ReturnsWithSubnetItemRequestBuilder()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new SubnetsRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var itemBuilder = builder["test-subnetid"];

        Assert.NotNull(itemBuilder);
        Assert.IsType<WithSubnetItemRequestBuilder>(itemBuilder);
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
        var builder = new SubnetsRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var itemBuilder1 = builder["test-subnetid"];
        var itemBuilder2 = builder["test-subnetid"];

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
        var builder = new SubnetsRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var itemBuilder = builder["test-subnetid"];

        Assert.NotNull(itemBuilder);
        _mockLoggerFactory.Received(1).CreateLogger<WithSubnetItemRequestBuilder>();
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
        var builder = new SubnetsRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

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
        var builder = new SubnetsRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

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
        var builder = new SubnetsRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation(config =>
        {
        });

        Assert.NotNull(requestInfo);
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
        var builder = new SubnetsRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);
        var body = new CreateSubnet
        {
            Description = "test-description",
            IPv4AddressRangeCIDR = "10.0.0.0/24",
            Name = "test-subnet",
            SubnetGroup = new SakuraResourceNameRef
            {
                SRN = "srnv1:sakura-is1:sakura.networking-suite.subnet-group:1234567890",
            },
            Zone = new Zone
            {
                Code = "is1c",
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
        var builder = new SubnetsRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);
        var body = new CreateSubnet
        {
            Description = "test-description",
            IPv4AddressRangeCIDR = "10.0.0.0/24",
            Name = "test-subnet",
            SubnetGroup = new SakuraResourceNameRef
            {
                SRN = "srnv1:sakura-is1:sakura.networking-suite.subnet-group:1234567890",
            },
            Zone = new Zone
            {
                Code = "is1c",
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
        var builder = new SubnetsRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);
        var body = new CreateSubnet
        {
            Description = "test-description",
            IPv4AddressRangeCIDR = "10.0.0.0/24",
            Name = "test-subnet",
            SubnetGroup = new SakuraResourceNameRef
            {
                SRN = "srnv1:sakura-is1:sakura.networking-suite.subnet-group:1234567890",
            },
            Zone = new Zone
            {
                Code = "is1c",
            },
        };

        var requestInfo = builder.ToPostRequestInformation(body);

        Assert.True(requestInfo.Headers.ContainsKey("Content-Type"));
        Assert.Contains("application/json", requestInfo.Headers["Content-Type"]);
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
        var builder = new SubnetsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var mockResponse = new SubnetListResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<SubnetListResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(mockResponse);

        var result = await builder.GetAsync(config =>
        {
            config.QueryParameters.SubnetGroupSRN = "srnv1:sakura-is1:sakura.networking-suite.subnet-group:1234567890";
        });

        Assert.NotNull(result);
        Assert.IsType<SubnetListResponse>(result);

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<SubnetListResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => string.Concat(state).Contains("サブネット一覧の取得を開始します。")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => string.Concat(state).Contains("サブネット一覧の取得が完了しました。")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncで必須のSubnetGroupSRNを指定しない場合にValidationExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithoutRequestConfiguration_ThrowsValidationException()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new SubnetsRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync());

        Assert.Equal("subnetGroupSRNが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("subnetGroupSRNが入力されていません。");
    }

    /// <summary>
    /// GetAsyncで無効なSubnetGroupSRNを指定した場合にValidationExceptionがスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("　")]
    public async Task GetAsync_WithNullOrEmptyOrWhitespaceSubnetGroupSRN_ThrowsValidationException(string? invalidSubnetGroupSRN)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new SubnetsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync(config =>
        {
            config.QueryParameters.SubnetGroupSRN = invalidSubnetGroupSRN;
        }));

        Assert.Equal("subnetGroupSRNが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("subnetGroupSRNが入力されていません。");
    }

    /// <summary>
    /// GetAsyncが"XXX"キーに汎用ApiErrorのエラーマッピングを使用することを確認
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
        var builder = new SubnetsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<SubnetListResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(new SubnetListResponse());

        await builder.GetAsync(config =>
        {
            config.QueryParameters.SubnetGroupSRN = "srnv1:sakura-is1:sakura.networking-suite.subnet-group:1234567890";
        });

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<SubnetListResponse>>(),
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
        var builder = new SubnetsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<SubnetListResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<SubnetListResponse?>(expectedException));

        var exception = await Assert.ThrowsAsync<ApiError>(() => builder.GetAsync(config =>
        {
            config.QueryParameters.SubnetGroupSRN = "srnv1:sakura-is1:sakura.networking-suite.subnet-group:1234567890";
        }));

        Assert.Same(expectedException, exception);
        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => string.Concat(state).Contains("サブネット一覧取得のAPIエラーが発生しました。")),
            Arg.Any<ApiException>(),
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
        var builder = new SubnetsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new InvalidOperationException("test exception");
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<SubnetListResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<SubnetListResponse?>(expectedException));

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => builder.GetAsync(config =>
        {
            config.QueryParameters.SubnetGroupSRN = "srnv1:sakura-is1:sakura.networking-suite.subnet-group:1234567890";
        }));

        Assert.Same(expectedException, exception);
        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => string.Concat(state).Contains("サブネット一覧の取得中にエラーが発生しました。")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
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
        var builder = new SubnetsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var mockResponse = new CreatedSubnetResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreatedSubnetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(mockResponse);

        var body = new CreateSubnet
        {
            Description = "test-description",
            IPv4AddressRangeCIDR = "10.0.0.0/24",
            Name = "test-subnet",
            SubnetGroup = new SakuraResourceNameRef
            {
                SRN = "srnv1:sakura-is1:sakura.networking-suite.subnet-group:1234567890",
            },
            Zone = new Zone
            {
                Code = "is1c",
            },
        };

        var result = await builder.PostAsync(body);

        Assert.NotNull(result);
        Assert.IsType<CreatedSubnetResponse>(result);

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreatedSubnetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => string.Concat(state).Contains("サブネットの作成を開始します。")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => string.Concat(state).Contains("サブネットの作成が完了しました。")),
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
        var builder = new SubnetsRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

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
        var builder = new SubnetsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreatedSubnetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(new CreatedSubnetResponse());

        var body = new CreateSubnet
        {
            Description = "test-description",
            IPv4AddressRangeCIDR = "10.0.0.0/24",
            Name = "test-subnet",
            SubnetGroup = new SakuraResourceNameRef
            {
                SRN = "srnv1:sakura-is1:sakura.networking-suite.subnet-group:1234567890",
            },
            Zone = new Zone
            {
                Code = "is1c",
            },
        };

        await builder.PostAsync(body);

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreatedSubnetResponse>>(),
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
        var builder = new SubnetsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreatedSubnetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<CreatedSubnetResponse?>(expectedException));

        var body = new CreateSubnet
        {
            Description = "test-description",
            IPv4AddressRangeCIDR = "10.0.0.0/24",
            Name = "test-subnet",
            SubnetGroup = new SakuraResourceNameRef
            {
                SRN = "srnv1:sakura-is1:sakura.networking-suite.subnet-group:1234567890",
            },
            Zone = new Zone
            {
                Code = "is1c",
            },
        };

        var exception = await Assert.ThrowsAsync<ApiError>(() => builder.PostAsync(body));

        Assert.Same(expectedException, exception);
        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => string.Concat(state).Contains("サブネット作成のAPIエラーが発生しました。")),
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
        var builder = new SubnetsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new InvalidOperationException("test exception");
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreatedSubnetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<CreatedSubnetResponse?>(expectedException));

        var body = new CreateSubnet
        {
            Description = "test-description",
            IPv4AddressRangeCIDR = "10.0.0.0/24",
            Name = "test-subnet",
            SubnetGroup = new SakuraResourceNameRef
            {
                SRN = "srnv1:sakura-is1:sakura.networking-suite.subnet-group:1234567890",
            },
            Zone = new Zone
            {
                Code = "is1c",
            },
        };

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => builder.PostAsync(body));

        Assert.Same(expectedException, exception);
        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => string.Concat(state).Contains("サブネットの作成中にエラーが発生しました。")),
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
        var builder = new SubnetsRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);
        var newUrl = "https://newapi.example.com/networking-suite/subnets?subnetGroupSRN={subnetGroupSRN}";

        var newBuilder = builder.WithUrl(newUrl);

        Assert.NotNull(newBuilder);
        Assert.NotSame(builder, newBuilder);
        Assert.IsType<SubnetsRequestBuilder>(newBuilder);
    }

    /// <summary>
    /// PostAsyncのリクエストボディのDescriptionがnull、空文字列、空白文字の場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("　")]
    public async Task PostAsync_WithNullOrEmptyOrWhitespaceDescription_ThrowsValidationException(string? invalidDescription)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new SubnetsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateSubnet
        {
            Description = invalidDescription,
            IPv4AddressRangeCIDR = "10.0.0.0/24",
            Name = "test-subnet",
            SubnetGroup = new SakuraResourceNameRef
            {
                SRN = "srnv1:sakura-is1:sakura.networking-suite.subnet-group:1234567890",
            },
            Zone = new Zone
            {
                Code = "is1c",
            },
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal("Descriptionが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("Descriptionが入力されていません。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのIPv4AddressRangeCIDRがnull、空文字列、空白文字の場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("　")]
    public async Task PostAsync_WithNullOrEmptyOrWhitespaceIPv4AddressRangeCIDR_ThrowsValidationException(string? invalidIPv4AddressRangeCIDR)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new SubnetsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateSubnet
        {
            Description = "test-description",
            IPv4AddressRangeCIDR = invalidIPv4AddressRangeCIDR,
            Name = "test-subnet",
            SubnetGroup = new SakuraResourceNameRef
            {
                SRN = "srnv1:sakura-is1:sakura.networking-suite.subnet-group:1234567890",
            },
            Zone = new Zone
            {
                Code = "is1c",
            },
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal("IPv4AddressRangeCIDRが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("IPv4AddressRangeCIDRが入力されていません。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのNameがnull、空文字列、空白文字の場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("　")]
    public async Task PostAsync_WithNullOrEmptyOrWhitespaceName_ThrowsValidationException(string? invalidName)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new SubnetsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateSubnet
        {
            Description = "test-description",
            IPv4AddressRangeCIDR = "10.0.0.0/24",
            Name = invalidName,
            SubnetGroup = new SakuraResourceNameRef
            {
                SRN = "srnv1:sakura-is1:sakura.networking-suite.subnet-group:1234567890",
            },
            Zone = new Zone
            {
                Code = "is1c",
            },
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal("Nameが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("Nameが入力されていません。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのSubnetGroupがnullの場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithNullSubnetGroup_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new SubnetsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateSubnet
        {
            Description = "test-description",
            IPv4AddressRangeCIDR = "10.0.0.0/24",
            Name = "test-subnet",
            SubnetGroup = null,
            Zone = new Zone
            {
                Code = "is1c",
            },
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal("SubnetGroupが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("SubnetGroupが入力されていません。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのSubnetGroup.SRNがnull、空文字列、空白文字の場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("　")]
    public async Task PostAsync_WithNullOrEmptyOrWhitespaceSubnetGroupSRN_ThrowsValidationException(string? invalidSRN)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new SubnetsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateSubnet
        {
            Description = "test-description",
            IPv4AddressRangeCIDR = "10.0.0.0/24",
            Name = "test-subnet",
            SubnetGroup = new SakuraResourceNameRef
            {
                SRN = invalidSRN,
            },
            Zone = new Zone
            {
                Code = "is1c",
            },
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal("SRNが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("SRNが入力されていません。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのZoneがnullの場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithNullZone_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new SubnetsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateSubnet
        {
            Description = "test-description",
            IPv4AddressRangeCIDR = "10.0.0.0/24",
            Name = "test-subnet",
            SubnetGroup = new SakuraResourceNameRef
            {
                SRN = "srnv1:sakura-is1:sakura.networking-suite.subnet-group:1234567890",
            },
            Zone = null,
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal("Zoneが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("Zoneが入力されていません。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのZone.Codeがnull、空文字列、空白文字の場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("　")]
    public async Task PostAsync_WithNullOrEmptyOrWhitespaceZoneCode_ThrowsValidationException(string? invalidCode)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new SubnetsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateSubnet
        {
            Description = "test-description",
            IPv4AddressRangeCIDR = "10.0.0.0/24",
            Name = "test-subnet",
            SubnetGroup = new SakuraResourceNameRef
            {
                SRN = "srnv1:sakura-is1:sakura.networking-suite.subnet-group:1234567890",
            },
            Zone = new Zone
            {
                Code = invalidCode,
            },
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal("Codeが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("Codeが入力されていません。");
    }
}
