using Microsoft.Kiota.Abstractions;
using NSubstitute;
using Sacloud.Sdk.SimpleNotification.Commonserviceitem.Simplenotification.Routing;
using Sacloud.Sdk.SimpleNotification.Commonserviceitem.Simplenotification.Routing.Reorder;

namespace Sacloud.Sdk.SimpleNotification.Tests.Commonserviceitem.Simplenotification.Routing;

/// <summary>
/// RoutingRequestBuilderのテスト
/// </summary>
public class RoutingRequestBuilderTests
{
    /// <summary>
    /// コンストラクタ（PathParameters版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };

        var builder = new RoutingRequestBuilder(pathParameters, requestAdapter);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();

        var builder = new RoutingRequestBuilder("https://api.example.com/commonserviceitem/simplenotification/routing", requestAdapter);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// Reorderプロパティが正しい型を返すことを確認
    /// </summary>
    [Fact]
    public void Reorder_ReturnsReorderRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new RoutingRequestBuilder(pathParameters, requestAdapter);

        var result = builder.Reorder;

        Assert.NotNull(result);
        Assert.IsType<ReorderRequestBuilder>(result);
    }
}
