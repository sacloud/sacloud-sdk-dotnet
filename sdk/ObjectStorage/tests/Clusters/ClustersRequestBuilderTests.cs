using Microsoft.Kiota.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Clusters;
using Sacloud.Sdk.ObjectStorage.Clusters.Item;
using Sacloud.Sdk.ObjectStorage.Models.Handler;
using Sacloud.Sdk.Shared.Models;

namespace Sacloud.Sdk.ObjectStorage.Tests.Clusters;

/// <summary>
/// ClustersRequestBuilderのテスト
/// </summary>
public class ClustersRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<ClustersRequestBuilder> _mockLogger;

    public ClustersRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<ClustersRequestBuilder>>();
        _mockLoggerFactory.CreateLogger(Arg.Any<string>()).Returns(_mockLogger);
    }

    /// <summary>
    /// コンストラクタ（pathParameters版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };

        var builder = new ClustersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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

        var builder = new ClustersRequestBuilder("https://api.example.com/clusters", requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// インデクサーが正しいClustersItemRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_WithId_ReturnsClustersItemRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ClustersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var item = builder["site-id-1"];

        Assert.NotNull(item);
        Assert.IsType<ClustersItemRequestBuilder>(item);
    }

    /// <summary>
    /// インデクサーが毎回新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_ReturnsNewInstanceOnEachCall()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ClustersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var item1 = builder["site-id-1"];
        var item2 = builder["site-id-1"];

        Assert.NotSame(item1, item2);
    }

    /// <summary>
    /// ToGetRequestInformationが正しいRequestInformationを返すことを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_ReturnsRequestInformation()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ClustersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.GET, requestInfo.HttpMethod);
    }

    /// <summary>
    /// GetAsyncがRequestAdapterを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_CallsRequestAdapter()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ClustersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListClustersResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(new ListClustersResponse());

        var result = await builder.GetAsync();

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListClustersResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// WithUrlが正しいClustersRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ClustersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var newBuilder = builder.WithUrl("https://api.example.com/clusters");

        Assert.NotNull(newBuilder);
        Assert.IsType<ClustersRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }

    /// <summary>
    /// GetAsyncが"XXX"キーにApiErrorのエラーマッピングを使用することを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_SendsWithXxxErrorMapping()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ClustersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListClustersResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(new ListClustersResponse());

        await builder.GetAsync();

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListClustersResponse>>(),
            Arg.Is<Dictionary<string, ParsableFactory<IParsable>>>(x => HasApiErrorMapping(x)),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// GetAsyncでApiErrorがスローされた場合に再スローされることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithApiError_ThrowsApiError()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ClustersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListClustersResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<ListClustersResponse?>(expectedException));

        await Assert.ThrowsAsync<ApiError>(() => builder.GetAsync());
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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ClustersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new InvalidOperationException("test exception");
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListClustersResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<ListClustersResponse?>(expectedException));

        await Assert.ThrowsAsync<InvalidOperationException>(() => builder.GetAsync());
    }

    private static bool HasApiErrorMapping(Dictionary<string, ParsableFactory<IParsable>> errorMapping)
    {
        return errorMapping.TryGetValue("XXX", out var handler)
            && handler == ApiError.CreateFromDiscriminatorValue;
    }
}
