using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using NSubstitute;
using Sacloud.Sdk.EventBus.Commonserviceitem.Item.Eventbus;
using Sacloud.Sdk.EventBus.Commonserviceitem.Item.Eventbus.Processconfiguration;

namespace Sacloud.Sdk.EventBus.Tests.Commonserviceitem.Item.Eventbus;

/// <summary>
/// EventbusRequestBuilderのテスト
/// </summary>
public class EventbusRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    
    public EventbusRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
    }

    /// <summary>
    /// コンストラクタ（PathParameters版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };

        var builder = new EventbusRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var rawUrl = "https://api.example.com/commonserviceitem/test-id/eventbus";

        var builder = new EventbusRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// Processconfigurationプロパティが正しくProcessconfigurationRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Processconfiguration_ReturnsProcessconfigurationRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new EventbusRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var processconfiguration = builder.Processconfiguration;

        Assert.NotNull(processconfiguration);
        Assert.IsType<ProcessconfigurationRequestBuilder>(processconfiguration);
    }

    /// <summary>
    /// Processconfigurationプロパティが毎回新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Processconfiguration_ReturnsNewInstance_OnEachAccess()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new EventbusRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var processconfiguration1 = builder.Processconfiguration;
        var processconfiguration2 = builder.Processconfiguration;

        Assert.NotSame(processconfiguration1, processconfiguration2);
    }
}
