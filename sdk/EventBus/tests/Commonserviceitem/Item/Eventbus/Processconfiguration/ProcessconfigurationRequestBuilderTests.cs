using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using NSubstitute;
using Sacloud.Sdk.EventBus.Commonserviceitem.Item.Eventbus.Processconfiguration;
using Sacloud.Sdk.EventBus.Commonserviceitem.Item.Eventbus.Processconfiguration.SetSecret;

namespace Sacloud.Sdk.EventBus.Tests.Commonserviceitem.Item.Eventbus.Processconfiguration;

/// <summary>
/// ProcessconfigurationRequestBuilderのテスト
/// </summary>
public class ProcessconfigurationRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;

    public ProcessconfigurationRequestBuilderTests()
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

        var builder = new ProcessconfigurationRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var rawUrl = "https://api.example.com/commonserviceitem/test-id/eventbus/processconfiguration";

        var builder = new ProcessconfigurationRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// SetSecretプロパティが正しくSetSecretRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void SetSecret_ReturnsSetSecretRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new ProcessconfigurationRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var setSecret = builder.SetSecret;

        Assert.NotNull(setSecret);
        Assert.IsType<SetSecretRequestBuilder>(setSecret);
    }

    /// <summary>
    /// SetSecretプロパティが毎回新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void SetSecret_ReturnsNewInstance_OnEachAccess()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new ProcessconfigurationRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var setSecret1 = builder.SetSecret;
        var setSecret2 = builder.SetSecret;

        Assert.NotSame(setSecret1, setSecret2);
    }
}
