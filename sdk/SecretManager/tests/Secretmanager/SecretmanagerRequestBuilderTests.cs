using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using NSubstitute;
using Sacloud.Sdk.SecretManager.Secretmanager;
using Sacloud.Sdk.SecretManager.Secretmanager.Vaults;

namespace Sacloud.Sdk.SecretManager.Tests.Secretmanager;

/// <summary>
/// SecretmanagerRequestBuilderのテスト
/// </summary>
public class SecretmanagerRequestBuilderTests
{
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
        var loggerFactory = Substitute.For<ILoggerFactory>();

        var builder = new SecretmanagerRequestBuilder(pathParameters, requestAdapter, loggerFactory);

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
        var rawUrl = "https://api.example.com/secretmanager";
        var loggerFactory = Substitute.For<ILoggerFactory>();

        var builder = new SecretmanagerRequestBuilder(rawUrl, requestAdapter, loggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// Vaultsプロパティが正しいVaultsRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Vaults_ReturnsVaultsRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var loggerFactory = Substitute.For<ILoggerFactory>();
        var builder = new SecretmanagerRequestBuilder(pathParameters, requestAdapter, loggerFactory);

        var vaults = builder.Vaults;

        Assert.NotNull(vaults);
        Assert.IsType<VaultsRequestBuilder>(vaults);
    }

    /// <summary>
    /// Vaultsプロパティが毎回新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Vaults_ReturnsNewInstance_OnEachCall()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var loggerFactory = Substitute.For<ILoggerFactory>();
        var builder = new SecretmanagerRequestBuilder(pathParameters, requestAdapter, loggerFactory);

        var vaults1 = builder.Vaults;
        var vaults2 = builder.Vaults;

        Assert.NotSame(vaults1, vaults2);
    }
}
