using Microsoft.Kiota.Abstractions;
using NSubstitute;
using Sacloud.Sdk.SimpleNotification.Commonserviceitem.Item.Simplenotification;
using Sacloud.Sdk.SimpleNotification.Commonserviceitem.Item.Simplenotification.Message;
using Sacloud.Sdk.SimpleNotification.Commonserviceitem.Item.Simplenotification.Status;

namespace Sacloud.Sdk.SimpleNotification.Tests.Commonserviceitem.Item.Simplenotification;

/// <summary>
/// Commonserviceitem/Item/Simplenotification配下のSimplenotificationRequestBuilderのテスト
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
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "123456789012" }
        };

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

        var builder = new SimplenotificationRequestBuilder("https://api.example.com/commonserviceitem/123456789012/simplenotification", requestAdapter);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// Messageプロパティが正しい型を返すことを確認
    /// </summary>
    [Fact]
    public void Message_ReturnsMessageRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "123456789012" }
        };
        var builder = new SimplenotificationRequestBuilder(pathParameters, requestAdapter);

        var result = builder.Message;

        Assert.NotNull(result);
        Assert.IsType<MessageRequestBuilder>(result);
    }

    /// <summary>
    /// Statusプロパティが正しい型を返すことを確認
    /// </summary>
    [Fact]
    public void Status_ReturnsStatusRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "123456789012" }
        };
        var builder = new SimplenotificationRequestBuilder(pathParameters, requestAdapter);

        var result = builder.Status;

        Assert.NotNull(result);
        Assert.IsType<StatusRequestBuilder>(result);
    }
}
