using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Kiota.Abstractions;
using NSubstitute;
using Sacloud.Sdk.CloudHSM;
using System.Reflection;

namespace Sacloud.Sdk.CloudHSM.Tests;

/// <summary>
/// CloudHSMClientのテスト
/// </summary>
public class CloudHSMClientTests
{

    /// <summary>
    /// コンストラクタがIRequestAdapterを受け取って正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithValidRequestAdapter_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";

        var client = new CloudHSMClient(requestAdapter);

        Assert.NotNull(client);
    }

    /// <summary>
    /// コンストラクタがBaseUrlを設定することを確認
    /// </summary>
    [Fact]
    public void Constructor_SetsDefaultBaseUrl_WhenBaseUrlIsEmpty()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = string.Empty;

        _ = new CloudHSMClient(requestAdapter);

        Assert.Equal("https://secure.sakura.ad.jp/cloud/zone/is1a/api/cloud/1.1", requestAdapter.BaseUrl);
    }

    /// <summary>
    /// コンストラクタが既存のBaseUrlを保持することを確認
    /// </summary>
    [Fact]
    public void Constructor_PreservesExistingBaseUrl_WhenBaseUrlIsNotEmpty()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var customBaseUrl = "https://custom.api.example.com";
        requestAdapter.BaseUrl = customBaseUrl;

        _ = new CloudHSMClient(requestAdapter);

        Assert.Equal(customBaseUrl, requestAdapter.BaseUrl);
    }

    /// <summary>
    /// Cloudhsmプロパティが正しくCloudhsmRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Cloudhsm_ReturnsCloudhsmRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";

        var client = new CloudHSMClient(requestAdapter);

        var builder = client.Cloudhsm;

        Assert.NotNull(builder);
        Assert.IsType<global::Sacloud.Sdk.CloudHSM.Cloudhsm.CloudhsmRequestBuilder>(builder);
    }

    /// <summary>
    /// Cloudhsmプロパティが毎回新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Cloudhsm_ReturnsNewInstance_OnEachAccess()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";

        var client = new CloudHSMClient(requestAdapter);

        var builder1 = client.Cloudhsm;
        var builder2 = client.Cloudhsm;

        Assert.NotSame(builder1, builder2);
    }

    /// <summary>
    /// コンストラクタがloggerFactory未指定でも正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithNullLoggerFactory_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";

        var client = new CloudHSMClient(requestAdapter, null);

        Assert.NotNull(client);
        Assert.NotNull(client.Cloudhsm);
    }

    /// <summary>
    /// コンストラクタが明示的なloggerFactoryを受け取って正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithExplicitLoggerFactory_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";

        var client = new CloudHSMClient(requestAdapter, NullLoggerFactory.Instance);

        Assert.NotNull(client);
        Assert.NotNull(client.Cloudhsm);
    }

    /// <summary>
    /// クライアントに指定したLoggerFactoryがすべての子RequestBuilderで使用されることを確認
    /// </summary>
    [Fact]
    public void RequestBuilderNavigation_PropagatesLoggerFactoryToAllChildBuilders()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var loggerFactory = Substitute.For<ILoggerFactory>();
        loggerFactory.CreateLogger(Arg.Any<string>()).Returns(Substitute.For<ILogger>());
        var client = new CloudHSMClient(requestAdapter, loggerFactory);

        _ = client.Cloudhsm.Cloudhsms["cloudhsm-resource-id"].Clients["client-id"];
        _ = client.Cloudhsm.Cloudhsms["cloudhsm-resource-id"].Peers["peer-id"];
        _ = client.Cloudhsm.Licenses["resource-id"];

        loggerFactory.Received(13).CreateLogger(Arg.Any<string>());
        loggerFactory.Received().CreateLogger(Arg.Is<string>(category => category.Contains("ClientsItemRequestBuilder")));
        loggerFactory.Received().CreateLogger(Arg.Is<string>(category => category.Contains("WithPeer_ItemRequestBuilder")));
        loggerFactory.Received().CreateLogger(Arg.Is<string>(category => category.Contains("WithResource_ItemRequestBuilder")));
    }

    /// <summary>
    /// すべてのRequestBuilderがPathParameters版とRawUrl版でLoggerFactoryを必須とすることを確認
    /// </summary>
    [Fact]
    public void RequestBuilderConstructors_WithNullLoggerFactory_ThrowArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var requestBuilderTypes = new[]
        {
            typeof(global::Sacloud.Sdk.CloudHSM.Cloudhsm.CloudhsmRequestBuilder),
            typeof(global::Sacloud.Sdk.CloudHSM.Cloudhsm.Cloudhsms.CloudhsmsRequestBuilder),
            typeof(global::Sacloud.Sdk.CloudHSM.Cloudhsm.Cloudhsms.Item.Cloudhsm_resource_ItemRequestBuilder),
            typeof(global::Sacloud.Sdk.CloudHSM.Cloudhsm.Cloudhsms.Item.Clients.ClientsRequestBuilder),
            typeof(global::Sacloud.Sdk.CloudHSM.Cloudhsm.Cloudhsms.Item.Clients.Item.ClientsItemRequestBuilder),
            typeof(global::Sacloud.Sdk.CloudHSM.Cloudhsm.Cloudhsms.Item.Peers.PeersRequestBuilder),
            typeof(global::Sacloud.Sdk.CloudHSM.Cloudhsm.Cloudhsms.Item.Peers.Item.WithPeer_ItemRequestBuilder),
            typeof(global::Sacloud.Sdk.CloudHSM.Cloudhsm.Licenses.LicensesRequestBuilder),
            typeof(global::Sacloud.Sdk.CloudHSM.Cloudhsm.Licenses.Item.WithResource_ItemRequestBuilder),
        };

        foreach (var requestBuilderType in requestBuilderTypes)
        {
            var constructors = requestBuilderType.GetConstructors();
            Assert.Equal(2, constructors.Length);

            foreach (var constructor in constructors)
            {
                var firstParameter = constructor.GetParameters()[0];
                var firstArgument = firstParameter.ParameterType == typeof(string)
                    ? "https://api.example.com"
                    : Activator.CreateInstance(firstParameter.ParameterType)!;

                var exception = Assert.Throws<TargetInvocationException>(() =>
                    constructor.Invoke(new object?[] { firstArgument, requestAdapter, null }));

                Assert.IsType<ArgumentNullException>(exception.InnerException);
            }
        }
    }
}
