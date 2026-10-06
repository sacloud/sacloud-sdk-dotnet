using Microsoft.Kiota.Abstractions;
using NSubstitute;
using Sacloud.Sdk.AddOn.Analytics.Etl.Status.Item.Item;
using Sacloud.Sdk.AddOn.Analytics.Etl.Status.Item;
using Microsoft.Extensions.Logging;

namespace Sacloud.Sdk.AddOn.Tests.Analytics.Etl.Status.Item;

/// <summary>
/// WithResourceGroupNameItemRequestBuilderのテスト
/// </summary>
public class WithResourceGroupNameItemRequestBuilderTests
{
    private readonly IRequestAdapter _mockRequestAdapter;
    private readonly ILoggerFactory _mockLoggerFactory;

    public WithResourceGroupNameItemRequestBuilderTests()
    {
        _mockRequestAdapter = Substitute.For<IRequestAdapter>();
        _mockRequestAdapter.BaseUrl = "https://api.example.com";
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
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
            { "resourceGroupName", "test-resourceGroupName" },
        };

        var builder = new WithResourceGroupNameItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（RawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_InitializesSuccessfully()
    {
        var rawUrl = "https://api.example.com/analytics/etl/status/test-resourceGroupName";

        var builder = new WithResourceGroupNameItemRequestBuilder(rawUrl, _mockRequestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// インデクサーが正しいWithDeploymentNameItemRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_ReturnsWithDeploymentNameItemRequestBuilder()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "resourceGroupName", "test-resourceGroupName" },
        };
        var builder = new WithResourceGroupNameItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var itemBuilder = builder["test-deploymentName"];

        Assert.NotNull(itemBuilder);
        Assert.IsType<WithDeploymentNameItemRequestBuilder>(itemBuilder);
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
            { "resourceGroupName", "test-resourceGroupName" },
        };
        var builder = new WithResourceGroupNameItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var itemBuilder1 = builder["test-deploymentName"];
        var itemBuilder2 = builder["test-deploymentName"];

        Assert.NotSame(itemBuilder1, itemBuilder2);
    }
}
