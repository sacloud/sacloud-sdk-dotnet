using Microsoft.Kiota.Abstractions;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Account;
using Sacloud.Sdk.ObjectStorage.Buckets;
using Sacloud.Sdk.ObjectStorage.Clusters;
using Sacloud.Sdk.ObjectStorage.Metering;
using Sacloud.Sdk.ObjectStorage.Permissions;
using Sacloud.Sdk.ObjectStorage.Plans;
using Sacloud.Sdk.ObjectStorage.Quota;
using Sacloud.Sdk.ObjectStorage.Status;

namespace Sacloud.Sdk.ObjectStorage.Tests;

/// <summary>
/// ObjectStorageClientのテスト
/// </summary>
public class ObjectStorageClientTests
{
    private readonly string siteId = "isk01";

    /// <summary>
    /// requestAdapterがnullの場合にArgumentNullExceptionが発生することを確認
    /// </summary>
    [Fact]
    public void Constructor_WithNullRequestAdapter_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new ObjectStorageClient(null, siteId));
    }

    /// <summary>
    /// siteIDがnullの場合にArgumentExceptionが発生することを確認
    /// </summary>
    [Fact]
    public void Constructor_WithNullSiteId_ThrowsArgumentException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();

        Assert.Throws<ArgumentException>(() => new ObjectStorageClient(requestAdapter, null));
    }

    /// <summary>
    /// siteIDが空白の場合にArgumentExceptionが発生することを確認
    /// </summary>
    [Fact]
    public void Constructor_WithWhitespaceSiteId_ThrowsArgumentException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();

        Assert.Throws<ArgumentException>(() => new ObjectStorageClient(requestAdapter, "  "));
    }

    /// <summary>
    /// コンストラクタが正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";

        var client = new ObjectStorageClient(requestAdapter, siteId);

        Assert.NotNull(client);
    }

    /// <summary>
    /// BaseUrlが未設定の場合にデフォルト値が設定されることを確認
    /// </summary>
    [Fact]
    public void Constructor_SetsDefaultBaseUrl_WhenBaseUrlIsEmpty()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = string.Empty;

        var client = new ObjectStorageClient(requestAdapter, siteId);

        requestAdapter.Received().BaseUrl = Arg.Is<string>(s => !string.IsNullOrEmpty(s));
    }

    /// <summary>
    /// Accountプロパティが正しい型のインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Account_ReturnsAccountRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new ObjectStorageClient(requestAdapter, siteId);

        var result = client.Account;

        Assert.NotNull(result);
        Assert.IsType<AccountRequestBuilder>(result);
    }

    /// <summary>
    /// Bucketsプロパティが正しい型のインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Buckets_ReturnsBucketsRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new ObjectStorageClient(requestAdapter, siteId);

        var result = client.Buckets;

        Assert.NotNull(result);
        Assert.IsType<BucketsRequestBuilder>(result);
    }

    /// <summary>
    /// Clustersプロパティが正しい型のインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Clusters_ReturnsClustersRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new ObjectStorageClient(requestAdapter, siteId);

        var result = client.Clusters;

        Assert.NotNull(result);
        Assert.IsType<ClustersRequestBuilder>(result);
    }

    /// <summary>
    /// Meteringプロパティが正しい型のインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Metering_ReturnsMeteringRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new ObjectStorageClient(requestAdapter, siteId);

        var result = client.Metering;

        Assert.NotNull(result);
        Assert.IsType<MeteringRequestBuilder>(result);
    }

    /// <summary>
    /// Permissionsプロパティが正しい型のインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Permissions_ReturnsPermissionsRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new ObjectStorageClient(requestAdapter, siteId);

        var result = client.Permissions;

        Assert.NotNull(result);
        Assert.IsType<PermissionsRequestBuilder>(result);
    }

    /// <summary>
    /// Plansプロパティが正しい型のインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Plans_ReturnsPlansRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new ObjectStorageClient(requestAdapter, siteId);

        var result = client.Plans;

        Assert.NotNull(result);
        Assert.IsType<PlansRequestBuilder>(result);
    }

    /// <summary>
    /// Quotaプロパティが正しい型のインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Quota_ReturnsQuotaRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new ObjectStorageClient(requestAdapter, siteId);

        var result = client.Quota;

        Assert.NotNull(result);
        Assert.IsType<QuotaRequestBuilder>(result);
    }

    /// <summary>
    /// Statusプロパティが正しい型のインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Status_ReturnsStatusRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new ObjectStorageClient(requestAdapter, siteId);

        var result = client.Status;

        Assert.NotNull(result);
        Assert.IsType<StatusRequestBuilder>(result);
    }
}
