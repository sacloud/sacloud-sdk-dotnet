using Microsoft.Kiota.Abstractions;
using NSubstitute;
using Sacloud.Sdk.EventBus.Commonserviceitem;

namespace Sacloud.Sdk.EventBus.Tests;

/// <summary>
/// EventBusClientのテスト
/// </summary>
public class EventBusClientTests
{
    /// <summary>
    /// コンストラクタがIRequestAdapterを受け取って正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithValidRequestAdapter_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";

        var client = new EventBusClient(requestAdapter);

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

        new EventBusClient(requestAdapter);

        Assert.Equal("https://secure.sakura.ad.jp/cloud/zone/is1a/api/cloud/1.1", requestAdapter.BaseUrl);
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

        new EventBusClient(requestAdapter);

        Assert.Equal(customBaseUrl, requestAdapter.BaseUrl);
    }

    /// <summary>
    /// Commonserviceitemプロパティが正しくCommonserviceitemRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Commonserviceitem_ReturnsCommonserviceitemRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";

        var client = new EventBusClient(requestAdapter);

        Assert.NotNull(client.Commonserviceitem);
        Assert.IsType<CommonserviceitemRequestBuilder>(client.Commonserviceitem);
    }

    /// <summary>
    /// Commonserviceitemプロパティが毎回新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Commonserviceitem_ReturnsNewInstance_OnEachAccess()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";

        var client = new EventBusClient(requestAdapter);

        var commonserviceitem1 = client.Commonserviceitem;
        var commonserviceitem2 = client.Commonserviceitem;

        Assert.NotSame(commonserviceitem1, commonserviceitem2);
    }
}
