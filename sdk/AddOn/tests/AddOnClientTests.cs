using Microsoft.Kiota.Abstractions;
using NSubstitute;

namespace Sacloud.Sdk.AddOn.Tests;

/// <summary>
/// AddOnClientのテスト
/// </summary>
public class AddOnClientTests
{

    /// <summary>
    /// コンストラクタがIRequestAdapterを受け取って正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithValidRequestAdapter_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";

        var client = new AddOnClient(requestAdapter);

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

        _ = new AddOnClient(requestAdapter);

        Assert.Equal("https://secure.sakura.ad.jp/cloud/api/addon/1.0", requestAdapter.BaseUrl);
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

        _ = new AddOnClient(requestAdapter);

        Assert.Equal(customBaseUrl, requestAdapter.BaseUrl);
    }

    /// <summary>
    /// Aiプロパティが正しくAiRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Ai_ReturnsAiRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";

        var client = new AddOnClient(requestAdapter);

        var builder = client.Ai;

        Assert.NotNull(builder);
        Assert.IsType<global::Sacloud.Sdk.AddOn.Ai.AiRequestBuilder>(builder);
    }

    /// <summary>
    /// Aiプロパティが毎回新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Ai_ReturnsNewInstance_OnEachAccess()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";

        var client = new AddOnClient(requestAdapter);

        var builder1 = client.Ai;
        var builder2 = client.Ai;

        Assert.NotSame(builder1, builder2);
    }

    /// <summary>
    /// Analyticsプロパティが正しくAnalyticsRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Analytics_ReturnsAnalyticsRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";

        var client = new AddOnClient(requestAdapter);

        var builder = client.Analytics;

        Assert.NotNull(builder);
        Assert.IsType<global::Sacloud.Sdk.AddOn.Analytics.AnalyticsRequestBuilder>(builder);
    }

    /// <summary>
    /// Analyticsプロパティが毎回新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Analytics_ReturnsNewInstance_OnEachAccess()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";

        var client = new AddOnClient(requestAdapter);

        var builder1 = client.Analytics;
        var builder2 = client.Analytics;

        Assert.NotSame(builder1, builder2);
    }

    /// <summary>
    /// Cdnプロパティが正しくCdnRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Cdn_ReturnsCdnRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";

        var client = new AddOnClient(requestAdapter);

        var builder = client.Cdn;

        Assert.NotNull(builder);
        Assert.IsType<global::Sacloud.Sdk.AddOn.Cdn.CdnRequestBuilder>(builder);
    }

    /// <summary>
    /// Cdnプロパティが毎回新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Cdn_ReturnsNewInstance_OnEachAccess()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";

        var client = new AddOnClient(requestAdapter);

        var builder1 = client.Cdn;
        var builder2 = client.Cdn;

        Assert.NotSame(builder1, builder2);
    }

    /// <summary>
    /// Securityプロパティが正しくSecurityRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Security_ReturnsSecurityRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";

        var client = new AddOnClient(requestAdapter);

        var builder = client.Security;

        Assert.NotNull(builder);
        Assert.IsType<global::Sacloud.Sdk.AddOn.Security.SecurityRequestBuilder>(builder);
    }

    /// <summary>
    /// Securityプロパティが毎回新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Security_ReturnsNewInstance_OnEachAccess()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";

        var client = new AddOnClient(requestAdapter);

        var builder1 = client.Security;
        var builder2 = client.Security;

        Assert.NotSame(builder1, builder2);
    }
}
