using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions.Store;
using NSubstitute;

namespace Sacloud.Sdk.ObjectStorage.Tests;

/// <summary>
/// FedBaseUrlRequestAdapterのテスト
/// </summary>
public class FedBaseUrlRequestAdapterTests
{
    private static readonly string FedBaseUrl = "https://secure.sakura.ad.jp/cloud/zone/is1a/api/objectstorage/1.0/fed/v1";

    /// <summary>
    /// コンストラクタにnullを渡した場合に例外が発生することを確認
    /// </summary>
    [Fact]
    public void Constructor_WithNullInnerRequestAdapter_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new FedBaseUrlRequestAdapter(null));
    }

    /// <summary>
    /// BaseUrlが固定値を返し、setterが無視されることを確認
    /// </summary>
    [Fact]
    public void BaseUrl_ReturnsFixedValue_AndSetterIsIgnored()
    {
        var innerRequestAdapter = Substitute.For<IRequestAdapter>();
        var adapter = new FedBaseUrlRequestAdapter(innerRequestAdapter)
        {
            BaseUrl = "https://api.example.com"
        };

        Assert.Equal(FedBaseUrl, adapter.BaseUrl);
    }

    /// <summary>
    /// SerializationWriterFactoryが内部アダプターから委譲されることを確認
    /// </summary>
    [Fact]
    public void SerializationWriterFactory_DelegatesToInnerRequestAdapter()
    {
        var innerRequestAdapter = Substitute.For<IRequestAdapter>();
        var writerFactory = Substitute.For<ISerializationWriterFactory>();
        innerRequestAdapter.SerializationWriterFactory.Returns(writerFactory);
        var adapter = new FedBaseUrlRequestAdapter(innerRequestAdapter);

        var result = adapter.SerializationWriterFactory;

        Assert.Same(writerFactory, result);
    }

    /// <summary>
    /// EnableBackingStoreが内部アダプターへ委譲されることを確認
    /// </summary>
    [Fact]
    public void EnableBackingStore_DelegatesToInnerRequestAdapter()
    {
        var innerRequestAdapter = Substitute.For<IRequestAdapter>();
        var backingStoreFactory = Substitute.For<IBackingStoreFactory>();
        var adapter = new FedBaseUrlRequestAdapter(innerRequestAdapter);

        adapter.EnableBackingStore(backingStoreFactory);

        innerRequestAdapter.Received(1).EnableBackingStore(backingStoreFactory);
    }

    /// <summary>
    /// SendAsyncがbaseurlを書き換えて内部アダプターを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task SendAsync_OverridesBaseUrl_AndDelegatesToInnerRequestAdapter()
    {
        var innerRequestAdapter = Substitute.For<IRequestAdapter>();
        var expected = Substitute.For<IParsable>();
        innerRequestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<IParsable>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(expected);
        var adapter = new FedBaseUrlRequestAdapter(innerRequestAdapter);
        var requestInfo = CreateRequestInformation();

        var result = await adapter.SendAsync(requestInfo, parseNode => expected);

        Assert.Same(expected, result);
        await innerRequestAdapter.Received(1).SendAsync(
            Arg.Is<RequestInformation>(x => HasFedBaseUrl(x)),
            Arg.Any<ParsableFactory<IParsable>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// SendCollectionAsyncがbaseurlを書き換えて内部アダプターを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task SendCollectionAsync_OverridesBaseUrl_AndDelegatesToInnerRequestAdapter()
    {
        var innerRequestAdapter = Substitute.For<IRequestAdapter>();
        var mockParsable = Substitute.For<IParsable>();
        IEnumerable<IParsable> expected = [mockParsable];
        innerRequestAdapter.SendCollectionAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<IParsable>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(expected);
        var adapter = new FedBaseUrlRequestAdapter(innerRequestAdapter);
        var requestInfo = CreateRequestInformation();

        var result = await adapter.SendCollectionAsync(requestInfo, parseNode => mockParsable);

        Assert.Same(expected, result);
        await innerRequestAdapter.Received(1).SendCollectionAsync(
            Arg.Is<RequestInformation>(x => HasFedBaseUrl(x)),
            Arg.Any<ParsableFactory<IParsable>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// SendPrimitiveAsyncがbaseurlを書き換えて内部アダプターを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task SendPrimitiveAsync_OverridesBaseUrl_AndDelegatesToInnerRequestAdapter()
    {
        var innerRequestAdapter = Substitute.For<IRequestAdapter>();
        const string expected = "ok";
        innerRequestAdapter.SendPrimitiveAsync<string>(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(expected);
        var adapter = new FedBaseUrlRequestAdapter(innerRequestAdapter);
        var requestInfo = CreateRequestInformation();

        var result = await adapter.SendPrimitiveAsync<string>(requestInfo);

        Assert.Equal(expected, result);
        await innerRequestAdapter.Received(1).SendPrimitiveAsync<string>(
            Arg.Is<RequestInformation>(x => HasFedBaseUrl(x)),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// SendPrimitiveCollectionAsyncがbaseurlを書き換えて内部アダプターを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task SendPrimitiveCollectionAsync_OverridesBaseUrl_AndDelegatesToInnerRequestAdapter()
    {
        var innerRequestAdapter = Substitute.For<IRequestAdapter>();
        IEnumerable<string> expected = ["a", "b"];
        innerRequestAdapter.SendPrimitiveCollectionAsync<string>(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(expected);
        var adapter = new FedBaseUrlRequestAdapter(innerRequestAdapter);
        var requestInfo = CreateRequestInformation();

        var result = await adapter.SendPrimitiveCollectionAsync<string>(requestInfo);

        Assert.Same(expected, result);
        await innerRequestAdapter.Received(1).SendPrimitiveCollectionAsync<string>(
            Arg.Is<RequestInformation>(x => HasFedBaseUrl(x)),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// SendNoContentAsyncがbaseurlを書き換えて内部アダプターを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task SendNoContentAsync_OverridesBaseUrl_AndDelegatesToInnerRequestAdapter()
    {
        var innerRequestAdapter = Substitute.For<IRequestAdapter>();
        innerRequestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
        var adapter = new FedBaseUrlRequestAdapter(innerRequestAdapter);
        var requestInfo = CreateRequestInformation();

        await adapter.SendNoContentAsync(requestInfo);

        await innerRequestAdapter.Received(1).SendNoContentAsync(
            Arg.Is<RequestInformation>(x => HasFedBaseUrl(x)),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// ConvertToNativeRequestAsyncがbaseurlを書き換えて内部アダプターを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task ConvertToNativeRequestAsync_OverridesBaseUrl_AndDelegatesToInnerRequestAdapter()
    {
        var innerRequestAdapter = Substitute.For<IRequestAdapter>();
        var expected = new object();
        innerRequestAdapter.ConvertToNativeRequestAsync<object>(
            Arg.Any<RequestInformation>(),
            Arg.Any<CancellationToken>())
            .Returns(expected);
        var adapter = new FedBaseUrlRequestAdapter(innerRequestAdapter);
        var requestInfo = CreateRequestInformation();

        var result = await adapter.ConvertToNativeRequestAsync<object>(requestInfo);

        Assert.Same(expected, result);
        await innerRequestAdapter.Received(1).ConvertToNativeRequestAsync<object>(
            Arg.Is<RequestInformation>(x => HasFedBaseUrl(x)),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// SendAsyncにnullのrequestInfoを渡した場合にArgumentNullExceptionが発生することを確認
    /// </summary>
    [Fact]
    public async Task SendAsync_WithNullRequestInfo_ThrowsArgumentNullException()
    {
        var innerRequestAdapter = Substitute.For<IRequestAdapter>();
        var adapter = new FedBaseUrlRequestAdapter(innerRequestAdapter);

        await Assert.ThrowsAsync<ArgumentNullException>(
            () => adapter.SendAsync(null, parseNode => Substitute.For<IParsable>()));
    }

    /// <summary>
    /// SendCollectionAsyncにnullのrequestInfoを渡した場合にArgumentNullExceptionが発生することを確認
    /// </summary>
    [Fact]
    public async Task SendCollectionAsync_WithNullRequestInfo_ThrowsArgumentNullException()
    {
        var innerRequestAdapter = Substitute.For<IRequestAdapter>();
        var adapter = new FedBaseUrlRequestAdapter(innerRequestAdapter);

        await Assert.ThrowsAsync<ArgumentNullException>(
            () => adapter.SendCollectionAsync(null, parseNode => Substitute.For<IParsable>()));
    }

    /// <summary>
    /// SendPrimitiveAsyncにnullのrequestInfoを渡した場合にArgumentNullExceptionが発生することを確認
    /// </summary>
    [Fact]
    public async Task SendPrimitiveAsync_WithNullRequestInfo_ThrowsArgumentNullException()
    {
        var innerRequestAdapter = Substitute.For<IRequestAdapter>();
        var adapter = new FedBaseUrlRequestAdapter(innerRequestAdapter);

        await Assert.ThrowsAsync<ArgumentNullException>(
            () => adapter.SendPrimitiveAsync<string>(null));
    }

    /// <summary>
    /// SendPrimitiveCollectionAsyncにnullのrequestInfoを渡した場合にArgumentNullExceptionが発生することを確認
    /// </summary>
    [Fact]
    public async Task SendPrimitiveCollectionAsync_WithNullRequestInfo_ThrowsArgumentNullException()
    {
        var innerRequestAdapter = Substitute.For<IRequestAdapter>();
        var adapter = new FedBaseUrlRequestAdapter(innerRequestAdapter);

        await Assert.ThrowsAsync<ArgumentNullException>(
            () => adapter.SendPrimitiveCollectionAsync<string>(null));
    }

    /// <summary>
    /// ConvertToNativeRequestAsyncにnullのrequestInfoを渡した場合にArgumentNullExceptionが発生することを確認
    /// </summary>
    [Fact]
    public async Task ConvertToNativeRequestAsync_WithNullRequestInfo_ThrowsArgumentNullException()
    {
        var innerRequestAdapter = Substitute.For<IRequestAdapter>();
        var adapter = new FedBaseUrlRequestAdapter(innerRequestAdapter);

        await Assert.ThrowsAsync<ArgumentNullException>(
            () => adapter.ConvertToNativeRequestAsync<object>(null));
    }

    /// <summary>
    /// SendAsyncにerrorMappingを渡した場合に内部アダプターへ転送されることを確認
    /// </summary>
    [Fact]
    public async Task SendAsync_WithErrorMapping_ForwardsErrorMappingToInnerAdapter()
    {
        var innerRequestAdapter = Substitute.For<IRequestAdapter>();
        var expected = Substitute.For<IParsable>();
        innerRequestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<IParsable>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(expected);
        var adapter = new FedBaseUrlRequestAdapter(innerRequestAdapter);
        var requestInfo = CreateRequestInformation();
        var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
        {
            { "4XX", parseNode => Substitute.For<IParsable>() }
        };

        var result = await adapter.SendAsync(requestInfo, parseNode => expected, errorMapping);

        Assert.Same(expected, result);
        await innerRequestAdapter.Received(1).SendAsync(
            Arg.Is<RequestInformation>(x => HasFedBaseUrl(x)),
            Arg.Any<ParsableFactory<IParsable>>(),
            Arg.Is<Dictionary<string, ParsableFactory<IParsable>>>(d => d == errorMapping),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// SendCollectionAsyncにerrorMappingを渡した場合に内部アダプターへ転送されることを確認
    /// </summary>
    [Fact]
    public async Task SendCollectionAsync_WithErrorMapping_ForwardsErrorMappingToInnerAdapter()
    {
        var innerRequestAdapter = Substitute.For<IRequestAdapter>();
        var mockParsable = Substitute.For<IParsable>();
        IEnumerable<IParsable> expected = [mockParsable];
        innerRequestAdapter.SendCollectionAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<IParsable>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(expected);
        var adapter = new FedBaseUrlRequestAdapter(innerRequestAdapter);
        var requestInfo = CreateRequestInformation();
        var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
        {
            { "4XX", parseNode => Substitute.For<IParsable>() }
        };

        var result = await adapter.SendCollectionAsync(requestInfo, parseNode => mockParsable, errorMapping);

        Assert.Same(expected, result);
        await innerRequestAdapter.Received(1).SendCollectionAsync(
            Arg.Is<RequestInformation>(x => HasFedBaseUrl(x)),
            Arg.Any<ParsableFactory<IParsable>>(),
            Arg.Is<Dictionary<string, ParsableFactory<IParsable>>>(d => d == errorMapping),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// SendPrimitiveAsyncにerrorMappingを渡した場合に内部アダプターへ転送されることを確認
    /// </summary>
    [Fact]
    public async Task SendPrimitiveAsync_WithErrorMapping_ForwardsErrorMappingToInnerAdapter()
    {
        var innerRequestAdapter = Substitute.For<IRequestAdapter>();
        const string expected = "ok";
        innerRequestAdapter.SendPrimitiveAsync<string>(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(expected);
        var adapter = new FedBaseUrlRequestAdapter(innerRequestAdapter);
        var requestInfo = CreateRequestInformation();
        var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
        {
            { "4XX", parseNode => Substitute.For<IParsable>() }
        };

        var result = await adapter.SendPrimitiveAsync<string>(requestInfo, errorMapping);

        Assert.Equal(expected, result);
        await innerRequestAdapter.Received(1).SendPrimitiveAsync<string>(
            Arg.Is<RequestInformation>(x => HasFedBaseUrl(x)),
            Arg.Is<Dictionary<string, ParsableFactory<IParsable>>>(d => d == errorMapping),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// SendPrimitiveCollectionAsyncにerrorMappingを渡した場合に内部アダプターへ転送されることを確認
    /// </summary>
    [Fact]
    public async Task SendPrimitiveCollectionAsync_WithErrorMapping_ForwardsErrorMappingToInnerAdapter()
    {
        var innerRequestAdapter = Substitute.For<IRequestAdapter>();
        IEnumerable<string> expected = ["a", "b"];
        innerRequestAdapter.SendPrimitiveCollectionAsync<string>(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(expected);
        var adapter = new FedBaseUrlRequestAdapter(innerRequestAdapter);
        var requestInfo = CreateRequestInformation();
        var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
        {
            { "4XX", parseNode => Substitute.For<IParsable>() }
        };

        var result = await adapter.SendPrimitiveCollectionAsync<string>(requestInfo, errorMapping);

        Assert.Same(expected, result);
        await innerRequestAdapter.Received(1).SendPrimitiveCollectionAsync<string>(
            Arg.Is<RequestInformation>(x => HasFedBaseUrl(x)),
            Arg.Is<Dictionary<string, ParsableFactory<IParsable>>>(d => d == errorMapping),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// SendAsyncにCancellationTokenを渡した場合に内部アダプターへ転送されることを確認
    /// </summary>
    [Fact]
    public async Task SendAsync_WithCancellationToken_ForwardsCancellationTokenToInnerAdapter()
    {
        var innerRequestAdapter = Substitute.For<IRequestAdapter>();
        var expected = Substitute.For<IParsable>();
        var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        innerRequestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<IParsable>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            cancellationToken)
            .Returns(expected);
        var adapter = new FedBaseUrlRequestAdapter(innerRequestAdapter);
        var requestInfo = CreateRequestInformation();

        var result = await adapter.SendAsync(requestInfo, parseNode => expected, cancellationToken: cancellationToken);

        Assert.Same(expected, result);
        await innerRequestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<IParsable>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            cancellationToken);
    }

    /// <summary>
    /// SendCollectionAsyncにCancellationTokenを渡した場合に内部アダプターへ転送されることを確認
    /// </summary>
    [Fact]
    public async Task SendCollectionAsync_WithCancellationToken_ForwardsCancellationTokenToInnerAdapter()
    {
        var innerRequestAdapter = Substitute.For<IRequestAdapter>();
        var mockParsable = Substitute.For<IParsable>();
        IEnumerable<IParsable> expected = [mockParsable];
        var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        innerRequestAdapter.SendCollectionAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<IParsable>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            cancellationToken)
            .Returns(expected);
        var adapter = new FedBaseUrlRequestAdapter(innerRequestAdapter);
        var requestInfo = CreateRequestInformation();

        var result = await adapter.SendCollectionAsync(requestInfo, parseNode => mockParsable, cancellationToken: cancellationToken);

        Assert.Same(expected, result);
        await innerRequestAdapter.Received(1).SendCollectionAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<IParsable>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            cancellationToken);
    }

    /// <summary>
    /// SendPrimitiveAsyncにCancellationTokenを渡した場合に内部アダプターへ転送されることを確認
    /// </summary>
    [Fact]
    public async Task SendPrimitiveAsync_WithCancellationToken_ForwardsCancellationTokenToInnerAdapter()
    {
        var innerRequestAdapter = Substitute.For<IRequestAdapter>();
        const string expected = "ok";
        var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        innerRequestAdapter.SendPrimitiveAsync<string>(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            cancellationToken)
            .Returns(expected);
        var adapter = new FedBaseUrlRequestAdapter(innerRequestAdapter);
        var requestInfo = CreateRequestInformation();

        var result = await adapter.SendPrimitiveAsync<string>(requestInfo, cancellationToken: cancellationToken);

        Assert.Equal(expected, result);
        await innerRequestAdapter.Received(1).SendPrimitiveAsync<string>(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            cancellationToken);
    }

    /// <summary>
    /// SendPrimitiveCollectionAsyncにCancellationTokenを渡した場合に内部アダプターへ転送されることを確認
    /// </summary>
    [Fact]
    public async Task SendPrimitiveCollectionAsync_WithCancellationToken_ForwardsCancellationTokenToInnerAdapter()
    {
        var innerRequestAdapter = Substitute.For<IRequestAdapter>();
        IEnumerable<string> expected = ["a", "b"];
        var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        innerRequestAdapter.SendPrimitiveCollectionAsync<string>(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            cancellationToken)
            .Returns(expected);
        var adapter = new FedBaseUrlRequestAdapter(innerRequestAdapter);
        var requestInfo = CreateRequestInformation();

        var result = await adapter.SendPrimitiveCollectionAsync<string>(requestInfo, cancellationToken: cancellationToken);

        Assert.Same(expected, result);
        await innerRequestAdapter.Received(1).SendPrimitiveCollectionAsync<string>(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            cancellationToken);
    }

    /// <summary>
    /// ConvertToNativeRequestAsyncにCancellationTokenを渡した場合に内部アダプターへ転送されることを確認
    /// </summary>
    [Fact]
    public async Task ConvertToNativeRequestAsync_WithCancellationToken_ForwardsCancellationTokenToInnerAdapter()
    {
        var innerRequestAdapter = Substitute.For<IRequestAdapter>();
        var expected = new object();
        var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        innerRequestAdapter.ConvertToNativeRequestAsync<object>(
            Arg.Any<RequestInformation>(),
            cancellationToken)
            .Returns(expected);
        var adapter = new FedBaseUrlRequestAdapter(innerRequestAdapter);
        var requestInfo = CreateRequestInformation();

        var result = await adapter.ConvertToNativeRequestAsync<object>(requestInfo, cancellationToken);

        Assert.Same(expected, result);
        await innerRequestAdapter.Received(1).ConvertToNativeRequestAsync<object>(
            Arg.Any<RequestInformation>(),
            cancellationToken);
    }

    private static RequestInformation CreateRequestInformation()
    {
        var requestInfo = new RequestInformation
        {
            HttpMethod = Method.GET,
            UrlTemplate = "{+baseurl}/clusters"
        };
        return requestInfo;
    }

    private static bool HasFedBaseUrl(RequestInformation requestInfo)
    {
        var uri = requestInfo.URI;
        return uri is not null
            && uri.AbsoluteUri.StartsWith(FedBaseUrl, StringComparison.Ordinal);
    }
}
