using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Management.Provisioning;
using Sacloud.Sdk.MonitoringSuite.Management.Provisioning.Initialize;
using Sacloud.Sdk.MonitoringSuite.Management.Provisioning.State;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Management.Provisioning;

/// <summary>
/// ProvisioningRequestBuilderのテスト
/// </summary>
public class ProvisioningRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;

    public ProvisioningRequestBuilderTests()
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
            { "baseurl", "https://api.example.com" }
        };

        var builder = new ProvisioningRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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

        var builder = new ProvisioningRequestBuilder("https://api.example.com/management/provisioning", requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// InitializeプロパティがInitializeRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Initialize_ReturnsInitializeRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ProvisioningRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder.Initialize);
        Assert.IsType<InitializeRequestBuilder>(builder.Initialize);
    }

    /// <summary>
    /// StateプロパティがStateRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void State_ReturnsStateRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ProvisioningRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder.State);
        Assert.IsType<StateRequestBuilder>(builder.State);
    }
}
