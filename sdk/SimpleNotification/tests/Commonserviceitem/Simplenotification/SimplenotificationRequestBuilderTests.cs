using Microsoft.Kiota.Abstractions;
using NSubstitute;
using Sacloud.Sdk.SimpleNotification.Commonserviceitem.Simplenotification;
using Sacloud.Sdk.SimpleNotification.Commonserviceitem.Simplenotification.History;
using Sacloud.Sdk.SimpleNotification.Commonserviceitem.Simplenotification.Routing;
using Sacloud.Sdk.SimpleNotification.Commonserviceitem.Simplenotification.Sources;

namespace Sacloud.Sdk.SimpleNotification.Tests.Commonserviceitem.Simplenotification;

/// <summary>
/// Commonserviceitem/Simplenotification配下のSimplenotificationRequestBuilderのテスト
/// </summary>
public class SimplenotificationRequestBuilderTests
{
    /// <summary>
    /// コンストラクタ（PathParameters版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };

        var builder = new SimplenotificationRequestBuilder(pathParameters, requestAdapter);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();

        var builder = new SimplenotificationRequestBuilder("https://api.example.com/commonserviceitem/simplenotification", requestAdapter);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// Historyプロパティが正しい型を返すことを確認
    /// </summary>
    [Fact]
    public void History_ReturnsHistoryRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new SimplenotificationRequestBuilder(pathParameters, requestAdapter);

        var result = builder.History;

        Assert.NotNull(result);
        Assert.IsType<HistoryRequestBuilder>(result);
    }

    /// <summary>
    /// Routingプロパティが正しい型を返すことを確認
    /// </summary>
    [Fact]
    public void Routing_ReturnsRoutingRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new SimplenotificationRequestBuilder(pathParameters, requestAdapter);

        var result = builder.Routing;

        Assert.NotNull(result);
        Assert.IsType<RoutingRequestBuilder>(result);
    }

    /// <summary>
    /// Sourcesプロパティが正しい型を返すことを確認
    /// </summary>
    [Fact]
    public void Sources_ReturnsSourcesRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new SimplenotificationRequestBuilder(pathParameters, requestAdapter);

        var result = builder.Sources;

        Assert.NotNull(result);
        Assert.IsType<SourcesRequestBuilder>(result);
    }
}
