using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using NSubstitute;
using Sacloud.Sdk.Workflows.Plans;
using Sacloud.Sdk.Workflows.Subscriptions;
using Sacloud.Sdk.Workflows.Workflows;

namespace Sacloud.Sdk.Workflows.Tests;

/// <summary>
/// WorkflowsClientのテスト
/// </summary>
public class WorkflowsClientTests
{
    private readonly ILoggerFactory _mockLoggerFactory;

    public WorkflowsClientTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
    }

    /// <summary>
    /// コンストラクタが正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();

        var client = new WorkflowsClient(requestAdapter, _mockLoggerFactory);

        Assert.NotNull(client);
    }

    /// <summary>
    /// コンストラクタがBaseUrlを設定することを確認
    /// </summary>
    [Fact]
    public void Constructor_SetsBaseUrl_WhenEmpty()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = string.Empty;

        _ = new WorkflowsClient(requestAdapter, _mockLoggerFactory);

        Assert.Equal("https://secure.sakura.ad.jp/cloud/zone/tk1b/api/workflow/1.0", requestAdapter.BaseUrl);
    }

    /// <summary>
    /// コンストラクタが既存のBaseUrlを保持することを確認
    /// </summary>
    [Fact]
    public void Constructor_PreservesExistingBaseUrl()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://custom.example.com";

        _ = new WorkflowsClient(requestAdapter, _mockLoggerFactory);

        Assert.Equal("https://custom.example.com", requestAdapter.BaseUrl);
    }

    /// <summary>
    /// Plansプロパティが正しいPlansRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Plans_ReturnsPlansRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new WorkflowsClient(requestAdapter, _mockLoggerFactory);

        var plansBuilder = client.Plans;

        Assert.NotNull(plansBuilder);
        Assert.IsType<PlansRequestBuilder>(plansBuilder);
    }

    /// <summary>
    /// Plansプロパティが毎回新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Plans_ReturnsNewInstance_OnEachCall()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new WorkflowsClient(requestAdapter, _mockLoggerFactory);

        var plansBuilder1 = client.Plans;
        var plansBuilder2 = client.Plans;

        Assert.NotSame(plansBuilder1, plansBuilder2);
    }

    /// <summary>
    /// Subscriptionsプロパティが正しいSubscriptionsRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Subscriptions_ReturnsSubscriptionsRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new WorkflowsClient(requestAdapter, _mockLoggerFactory);

        var subscriptionsBuilder = client.Subscriptions;

        Assert.NotNull(subscriptionsBuilder);
        Assert.IsType<SubscriptionsRequestBuilder>(subscriptionsBuilder);
    }

    /// <summary>
    /// Subscriptionsプロパティが毎回新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Subscriptions_ReturnsNewInstance_OnEachCall()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new WorkflowsClient(requestAdapter, _mockLoggerFactory);

        var subscriptionsBuilder1 = client.Subscriptions;
        var subscriptionsBuilder2 = client.Subscriptions;

        Assert.NotSame(subscriptionsBuilder1, subscriptionsBuilder2);
    }

    /// <summary>
    /// Workflowsプロパティが正しいWorkflowsRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Workflows_ReturnsWorkflowsRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new WorkflowsClient(requestAdapter, _mockLoggerFactory);

        var workflowsBuilder = client.Workflows;

        Assert.NotNull(workflowsBuilder);
        Assert.IsType<WorkflowsRequestBuilder>(workflowsBuilder);
    }

    /// <summary>
    /// Workflowsプロパティが毎回新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Workflows_ReturnsNewInstance_OnEachCall()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new WorkflowsClient(requestAdapter, _mockLoggerFactory);

        var workflowsBuilder1 = client.Workflows;
        var workflowsBuilder2 = client.Workflows;

        Assert.NotSame(workflowsBuilder1, workflowsBuilder2);
    }
}
