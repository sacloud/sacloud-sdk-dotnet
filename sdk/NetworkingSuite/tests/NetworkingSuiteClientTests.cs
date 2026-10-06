using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using NSubstitute;
using Sacloud.Sdk.NetworkingSuite.NetworkingSuite.Addresses;

namespace Sacloud.Sdk.NetworkingSuite.Tests;

/// <summary>
/// NetworkingSuiteClientのテスト
/// </summary>
public class NetworkingSuiteClientTests
{
    private sealed class TestNetworkingSuiteClient(IRequestAdapter requestAdapter, string zone) : NetworkingSuiteClient(requestAdapter, zone)
    {
        public Dictionary<string, object> ExposedPathParameters => PathParameters;
    }

    /// <summary>
    /// コンストラクタがIRequestAdapterを受け取って正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithValidRequestAdapter_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";

        var client = new NetworkingSuiteClient(requestAdapter, "is1c");

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

        _ = new NetworkingSuiteClient(requestAdapter, "is1c");

        Assert.Equal("https://secure.sakura.ad.jp/cloud/zone/is1c/api/cloud/1.1", requestAdapter.BaseUrl);
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

        _ = new NetworkingSuiteClient(requestAdapter, "is1c");

        Assert.Equal(customBaseUrl, requestAdapter.BaseUrl);
    }

    /// <summary>
    /// ゾーン指定のコンストラクタがBaseUrl、PathParameters、RequestInformationのURIを正しく設定することを確認
    /// </summary>
    [Theory]
    [InlineData("is1a", "https://secure.sakura.ad.jp/cloud/zone/is1a/api/cloud/1.1")]
    [InlineData("is1b", "https://secure.sakura.ad.jp/cloud/zone/is1b/api/cloud/1.1")]
    [InlineData("is1c", "https://secure.sakura.ad.jp/cloud/zone/is1c/api/cloud/1.1")]
    [InlineData("tk1a", "https://secure.sakura.ad.jp/cloud/zone/tk1a/api/cloud/1.1")]
    public void Constructor_WithZone_SetsEndpoint(string zone, string expectedUrl)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = string.Empty;

        var client = new TestNetworkingSuiteClient(requestAdapter, zone);

        Assert.Equal(expectedUrl, requestAdapter.BaseUrl);
        Assert.True(client.ExposedPathParameters.TryGetValue("baseurl", out var baseUrlValue));
        Assert.Equal(expectedUrl, baseUrlValue?.ToString());

        var requestInfo = client.NetworkingSuite.SubnetGroups.ToGetRequestInformation();

        Assert.Equal(
            $"{expectedUrl}/networking-suite/subnet-groups",
            requestInfo.URI.ToString());
    }

    /// <summary>
    /// ゾーン指定のコンストラクタが空のゾーンを拒否することを確認
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("　")]
    public void Constructor_WithZone_ThrowsArgumentException_WhenZoneIsEmpty(string? zone)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();

        Assert.Throws<ArgumentException>(() => new NetworkingSuiteClient(requestAdapter, zone));
    }

    /// <summary>
    /// ゾーン指定のコンストラクタがnullのrequestAdapterを拒否することを確認
    /// </summary>
    [Fact]
    public void Constructor_WithZone_ThrowsArgumentNullException_WhenRequestAdapterIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new NetworkingSuiteClient(null, "is1c"));
    }

    /// <summary>
    /// NetworkingSuiteプロパティが正しくNetworkingSuiteRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void NetworkingSuite_ReturnsNetworkingSuiteRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";

        var client = new NetworkingSuiteClient(requestAdapter, "is1c");

        var builder = client.NetworkingSuite;

        Assert.NotNull(builder);
        Assert.IsType<Sdk.NetworkingSuite.NetworkingSuite.NetworkingSuiteRequestBuilder>(builder);
    }

    /// <summary>
    /// NetworkingSuiteプロパティが毎回新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void NetworkingSuite_ReturnsNewInstance_OnEachAccess()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";

        var client = new NetworkingSuiteClient(requestAdapter, "is1c");

        var builder1 = client.NetworkingSuite;
        var builder2 = client.NetworkingSuite;

        Assert.NotSame(builder1, builder2);
    }

    /// <summary>
    /// NetworkingSuiteプロパティがLoggerFactoryをAddressesRequestBuilderへ引き渡すことを確認
    /// </summary>
    [Fact]
    public void NetworkingSuite_PropagatesLoggerFactory()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var loggerFactory = Substitute.For<ILoggerFactory>();
        var logger = Substitute.For<ILogger<AddressesRequestBuilder>>();
        loggerFactory.CreateLogger<AddressesRequestBuilder>().Returns(logger);
        var client = new NetworkingSuiteClient(requestAdapter, "is1c", loggerFactory);

        var addresses = client.NetworkingSuite.Addresses;

        Assert.NotNull(addresses);
        loggerFactory.Received(1).CreateLogger<AddressesRequestBuilder>();
    }
}
