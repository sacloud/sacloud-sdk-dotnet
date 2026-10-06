using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using NSubstitute;
using Sacloud.Sdk.NetworkingSuite.NetworkingSuite;
using Sacloud.Sdk.NetworkingSuite.NetworkingSuite.Addresses;
using Sacloud.Sdk.NetworkingSuite.NetworkingSuite.InterfaceConnections;
using Sacloud.Sdk.NetworkingSuite.NetworkingSuite.SubnetGroups;
using Sacloud.Sdk.NetworkingSuite.NetworkingSuite.Subnets;

namespace Sacloud.Sdk.NetworkingSuite.Tests.NetworkingSuite;

/// <summary>
/// NetworkingSuiteRequestBuilderのテスト
/// </summary>
public class NetworkingSuiteRequestBuilderTests
{
    private readonly IRequestAdapter _mockRequestAdapter;
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<AddressesRequestBuilder> _mockLogger;
    private readonly ILogger<InterfaceConnectionsRequestBuilder> _mockInterfaceConnectionsLogger;
    private readonly ILogger<SubnetGroupsRequestBuilder> _mockSubnetGroupsLogger;
    private readonly ILogger<SubnetsRequestBuilder> _mockSubnetsLogger;

    public NetworkingSuiteRequestBuilderTests()
    {
        _mockRequestAdapter = Substitute.For<IRequestAdapter>();
        _mockRequestAdapter.BaseUrl = "https://api.example.com";
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<AddressesRequestBuilder>>();
        _mockInterfaceConnectionsLogger = Substitute.For<ILogger<InterfaceConnectionsRequestBuilder>>();
        _mockSubnetGroupsLogger = Substitute.For<ILogger<SubnetGroupsRequestBuilder>>();
        _mockSubnetsLogger = Substitute.For<ILogger<SubnetsRequestBuilder>>();
        _mockLoggerFactory.CreateLogger<AddressesRequestBuilder>().Returns(_mockLogger);
        _mockLoggerFactory.CreateLogger<InterfaceConnectionsRequestBuilder>().Returns(_mockInterfaceConnectionsLogger);
        _mockLoggerFactory.CreateLogger<SubnetGroupsRequestBuilder>().Returns(_mockSubnetGroupsLogger);
        _mockLoggerFactory.CreateLogger<SubnetsRequestBuilder>().Returns(_mockSubnetsLogger);
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
        };

        var builder = new NetworkingSuiteRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（PathParameters版）がnullのLoggerFactoryで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_WithNullLoggerFactory_ThrowsArgumentNullException()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };

        Assert.Throws<ArgumentNullException>(() => new NetworkingSuiteRequestBuilder(pathParameters, _mockRequestAdapter, null));
    }

    /// <summary>
    /// コンストラクタ（RawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_InitializesSuccessfully()
    {
        var rawUrl = "https://api.example.com/networking-suite";

        var builder = new NetworkingSuiteRequestBuilder(rawUrl, _mockRequestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（RawUrl版）がnullのLoggerFactoryで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_WithNullLoggerFactory_ThrowsArgumentNullException()
    {
        var rawUrl = "https://api.example.com/networking-suite";

        Assert.Throws<ArgumentNullException>(() => new NetworkingSuiteRequestBuilder(rawUrl, _mockRequestAdapter, null));
    }

    /// <summary>
    /// Addressesプロパティが正しくAddressesRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Addresses_ReturnsAddressesRequestBuilder()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new NetworkingSuiteRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var result = builder.Addresses;

        Assert.NotNull(result);
        Assert.IsType<AddressesRequestBuilder>(result);
    }

    /// <summary>
    /// InterfaceConnectionsプロパティが正しくInterfaceConnectionsRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void InterfaceConnections_ReturnsInterfaceConnectionsRequestBuilder()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new NetworkingSuiteRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var result = builder.InterfaceConnections;

        Assert.NotNull(result);
        Assert.IsType<InterfaceConnectionsRequestBuilder>(result);
    }

    /// <summary>
    /// SubnetGroupsプロパティが正しくSubnetGroupsRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void SubnetGroups_ReturnsSubnetGroupsRequestBuilder()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new NetworkingSuiteRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var result = builder.SubnetGroups;

        Assert.NotNull(result);
        Assert.IsType<SubnetGroupsRequestBuilder>(result);
    }

    /// <summary>
    /// Subnetsプロパティが正しくSubnetsRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Subnets_ReturnsSubnetsRequestBuilder()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new NetworkingSuiteRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var result = builder.Subnets;

        Assert.NotNull(result);
        Assert.IsType<SubnetsRequestBuilder>(result);
    }

    /// <summary>
    /// AddressesプロパティがLoggerFactoryをAddressesRequestBuilderへ引き渡すことを確認
    /// </summary>
    [Fact]
    public void Addresses_PropagatesLoggerFactory()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new NetworkingSuiteRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var result = builder.Addresses;

        Assert.NotNull(result);
        _mockLoggerFactory.Received(1).CreateLogger<AddressesRequestBuilder>();
    }

    /// <summary>
    /// InterfaceConnectionsプロパティがLoggerFactoryをInterfaceConnectionsRequestBuilderへ引き渡すことを確認
    /// </summary>
    [Fact]
    public void InterfaceConnections_PropagatesLoggerFactory()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new NetworkingSuiteRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var result = builder.InterfaceConnections;

        Assert.NotNull(result);
        _mockLoggerFactory.Received(1).CreateLogger<InterfaceConnectionsRequestBuilder>();
    }

    /// <summary>
    /// SubnetGroupsプロパティがLoggerFactoryをSubnetGroupsRequestBuilderへ引き渡すことを確認
    /// </summary>
    [Fact]
    public void SubnetGroups_PropagatesLoggerFactory()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new NetworkingSuiteRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var result = builder.SubnetGroups;

        Assert.NotNull(result);
        _mockLoggerFactory.Received(1).CreateLogger<SubnetGroupsRequestBuilder>();
    }

    /// <summary>
    /// SubnetsプロパティがLoggerFactoryをSubnetsRequestBuilderへ引き渡すことを確認
    /// </summary>
    [Fact]
    public void Subnets_PropagatesLoggerFactory()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new NetworkingSuiteRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var result = builder.Subnets;

        Assert.NotNull(result);
        _mockLoggerFactory.Received(1).CreateLogger<SubnetsRequestBuilder>();
    }
}
