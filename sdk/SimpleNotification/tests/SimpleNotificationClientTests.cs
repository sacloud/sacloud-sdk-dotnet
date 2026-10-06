using Microsoft.Kiota.Abstractions;
using NSubstitute;

namespace Sacloud.Sdk.SimpleNotification.Tests;

/// <summary>
/// SimpleNotificationClientのテスト
/// </summary>
public class SimpleNotificationClientTests
{
    /// <summary>
    /// コンストラクタが正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();

        var client = new SimpleNotificationClient(requestAdapter);

        Assert.NotNull(client);
    }

    /// <summary>
    /// Commonserviceitemプロパティが非nullを返すことを確認
    /// </summary>
    [Fact]
    public void Commonserviceitem_ReturnsRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var client = new SimpleNotificationClient(requestAdapter);

        var result = client.Commonserviceitem;

        Assert.NotNull(result);
    }
}
