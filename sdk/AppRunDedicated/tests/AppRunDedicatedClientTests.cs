using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using NSubstitute;
using Sacloud.Sdk.AppRunDedicated.Applications;
using Sacloud.Sdk.AppRunDedicated.Clusters;
using Sacloud.Sdk.AppRunDedicated.Limits;
using Sacloud.Sdk.AppRunDedicated.Service_classes;

namespace Sacloud.Sdk.AppRunDedicated.Tests;

/// <summary>
/// AppRunDedicatedClientのテスト
/// </summary>
public class AppRunDedicatedClientTests
{
    /// <summary>
    /// コンストラクタがIRequestAdapterを受け取って正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithValidRequestAdapter_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = string.Empty;

        var client = new AppRunDedicatedClient(requestAdapter, Substitute.For<ILoggerFactory>());

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

        new AppRunDedicatedClient(requestAdapter, Substitute.For<ILoggerFactory>());

        Assert.Equal("https://secure.sakura.ad.jp/cloud/api/apprun-dedicated/1.0", requestAdapter.BaseUrl);
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

        new AppRunDedicatedClient(requestAdapter, Substitute.For<ILoggerFactory>());

        Assert.Equal(customBaseUrl, requestAdapter.BaseUrl);
    }

    /// <summary>
    /// Applicationsプロパティが正しくApplicationsRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Applications_ReturnsApplicationsRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";

        var client = new AppRunDedicatedClient(requestAdapter, Substitute.For<ILoggerFactory>());

        Assert.NotNull(client.Applications);
        Assert.IsType<ApplicationsRequestBuilder>(client.Applications);
    }

    /// <summary>
    /// Clustersプロパティが正しくClustersRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Clusters_ReturnsClustersRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";

        var client = new AppRunDedicatedClient(requestAdapter, Substitute.For<ILoggerFactory>());

        Assert.NotNull(client.Clusters);
        Assert.IsType<ClustersRequestBuilder>(client.Clusters);
    }

    /// <summary>
    /// Limitsプロパティが正しくLimitsRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Limits_ReturnsLimitsRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";

        var client = new AppRunDedicatedClient(requestAdapter, Substitute.For<ILoggerFactory>());

        Assert.NotNull(client.Limits);
        Assert.IsType<LimitsRequestBuilder>(client.Limits);
    }

    /// <summary>
    /// Service_classesプロパティが正しくService_classesRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Service_classes_ReturnsService_classesRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";

        var client = new AppRunDedicatedClient(requestAdapter, Substitute.For<ILoggerFactory>());

        Assert.NotNull(client.Service_classes);
        Assert.IsType<Service_classesRequestBuilder>(client.Service_classes);
    }
}
