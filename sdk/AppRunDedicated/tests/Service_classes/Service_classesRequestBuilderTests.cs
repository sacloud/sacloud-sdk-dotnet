using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using NSubstitute;
using Sacloud.Sdk.AppRunDedicated.Service_classes;
using Sacloud.Sdk.AppRunDedicated.Service_classes.Lb;
using Sacloud.Sdk.AppRunDedicated.Service_classes.Worker;

namespace Sacloud.Sdk.AppRunDedicated.Tests.Service_classes;

/// <summary>
/// Service_classesRequestBuilderのテスト
/// </summary>
public class Service_classesRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;

    public Service_classesRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
    }

    /// <summary>
    /// コンストラクタ（PathParameters版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_InitializesSuccessfully()
    {
        var pathParameters = new Dictionary<string, object>();
        var requestAdapter = Substitute.For<IRequestAdapter>();

        var builder = new Service_classesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_InitializesSuccessfully()
    {
        var rawUrl = "https://example.com/service_classes";
        var requestAdapter = Substitute.For<IRequestAdapter>();

        var builder = new Service_classesRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// Lbプロパティが正しいLbRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Lb_ReturnsLbRequestBuilder()
    {
        var pathParameters = new Dictionary<string, object>();
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new Service_classesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var lbBuilder = builder.Lb;

        Assert.NotNull(lbBuilder);
        Assert.IsType<LbRequestBuilder>(lbBuilder);
    }

    /// <summary>
    /// Lbプロパティが毎回新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Lb_ReturnsNewInstance_OnEachCall()
    {
        var pathParameters = new Dictionary<string, object>();
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new Service_classesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var lbBuilder1 = builder.Lb;
        var lbBuilder2 = builder.Lb;

        Assert.NotSame(lbBuilder1, lbBuilder2);
    }

    /// <summary>
    /// Workerプロパティが正しいWorkerRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Worker_ReturnsWorkerRequestBuilder()
    {
        var pathParameters = new Dictionary<string, object>();
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new Service_classesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var workerBuilder = builder.Worker;

        Assert.NotNull(workerBuilder);
        Assert.IsType<WorkerRequestBuilder>(workerBuilder);
    }

    /// <summary>
    /// Workerプロパティが毎回新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Worker_ReturnsNewInstance_OnEachCall()
    {
        var pathParameters = new Dictionary<string, object>();
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new Service_classesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var workerBuilder1 = builder.Worker;
        var workerBuilder2 = builder.Worker;

        Assert.NotSame(workerBuilder1, workerBuilder2);
    }
}
