using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using NSubstitute;
using Sacloud.Sdk.SimpleMQ.Queues;

namespace Sacloud.Sdk.SimpleMQ.Tests;

/// <summary>
/// SimpleMQClientのテスト
/// </summary>
public class SimpleMQClientTests
{
    /// <summary>
    /// コンストラクタがIRequestAdapterを受け取って正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithValidRequestAdapter_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";

        var client = new SimpleMQClient(requestAdapter, Substitute.For<ILoggerFactory>());

        Assert.NotNull(client);
    }

    /// <summary>
    /// コンストラクタがBaseUrlを設定することを確認
    /// </summary>
    [Fact]
    public void Constructor_SetsDefaultBaseUrl_WhenBaseUrlIsEmpty()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = string.Empty;

        new SimpleMQClient(requestAdapter, Substitute.For<ILoggerFactory>());

        Assert.Equal("https://simplemq.tk1b.api.sacloud.jp/v1", requestAdapter.BaseUrl);
    }

    /// <summary>
    /// コンストラクタが既存のBaseUrlを保持することを確認
    /// </summary>
    [Fact]
    public void Constructor_PreservesExistingBaseUrl_WhenBaseUrlIsNotEmpty()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var customBaseUrl = "https://custom.api.example.com";
        requestAdapter.BaseUrl = customBaseUrl;

        new SimpleMQClient(requestAdapter, Substitute.For<ILoggerFactory>());

        Assert.Equal(customBaseUrl, requestAdapter.BaseUrl);
    }

    /// <summary>
    /// Queuesプロパティが正しくQueuesRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Queues_ReturnsQueuesRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";

        var client = new SimpleMQClient(requestAdapter, Substitute.For<ILoggerFactory>());

        Assert.NotNull(client.Queues);
        Assert.IsType<QueuesRequestBuilder>(client.Queues);
    }

    /// <summary>
    /// Queuesプロパティが毎回新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Queues_ReturnsNewInstance_OnEachAccess()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";

        var client = new SimpleMQClient(requestAdapter, Substitute.For<ILoggerFactory>());

        var queues1 = client.Queues;
        var queues2 = client.Queues;

        Assert.NotSame(queues1, queues2);
    }
}
