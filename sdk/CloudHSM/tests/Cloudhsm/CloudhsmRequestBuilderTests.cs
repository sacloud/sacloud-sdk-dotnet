using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using NSubstitute;
using Sacloud.Sdk.CloudHSM.Cloudhsm.Cloudhsms;
using Sacloud.Sdk.CloudHSM.Cloudhsm.Licenses;
using Sacloud.Sdk.CloudHSM.Cloudhsm;

namespace Sacloud.Sdk.CloudHSM.Tests.Cloudhsm;

/// <summary>
/// CloudhsmRequestBuilderのテスト
/// </summary>
public class CloudhsmRequestBuilderTests
{
    private readonly IRequestAdapter _mockRequestAdapter;
    private readonly ILoggerFactory _mockLoggerFactory;

    public CloudhsmRequestBuilderTests()
    {
        _mockRequestAdapter = Substitute.For<IRequestAdapter>();
        _mockRequestAdapter.BaseUrl = "https://api.example.com";
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
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

        var builder = new CloudhsmRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（RawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_InitializesSuccessfully()
    {
        var rawUrl = "https://api.example.com/cloudhsm";

        var builder = new CloudhsmRequestBuilder(rawUrl, _mockRequestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// Cloudhsmsプロパティが正しくCloudhsmsRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Cloudhsms_ReturnsCloudhsmsRequestBuilder()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new CloudhsmRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var result = builder.Cloudhsms;

        Assert.NotNull(result);
        Assert.IsType<CloudhsmsRequestBuilder>(result);
    }

    /// <summary>
    /// Licensesプロパティが正しくLicensesRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Licenses_ReturnsLicensesRequestBuilder()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new CloudhsmRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var result = builder.Licenses;

        Assert.NotNull(result);
        Assert.IsType<LicensesRequestBuilder>(result);
    }

    /// <summary>
    /// コンストラクタ（PathParameters版）でLoggerFactoryがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParametersAndNullLoggerFactory_ThrowsArgumentNullException()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };

        Assert.Throws<ArgumentNullException>(() => new CloudhsmRequestBuilder(pathParameters, _mockRequestAdapter, null!));
    }

    /// <summary>
    /// コンストラクタ（RawUrl版）でLoggerFactoryがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrlAndNullLoggerFactory_ThrowsArgumentNullException()
    {
        var rawUrl = "https://api.example.com/cloudhsm";

        Assert.Throws<ArgumentNullException>(() => new CloudhsmRequestBuilder(rawUrl, _mockRequestAdapter, null!));
    }
}
