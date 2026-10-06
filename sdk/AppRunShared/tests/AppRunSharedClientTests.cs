using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using NSubstitute;
using Sacloud.Sdk.AppRunShared.Applications;
using Sacloud.Sdk.AppRunShared.User;

namespace Sacloud.Sdk.AppRunShared.Tests;

/// <summary>
/// AppRunSharedClientのテスト
/// </summary>
public class AppRunSharedClientTests
{
    /// <summary>
    /// コンストラクタがIRequestAdapterを受け取って正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithValidRequestAdapter_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";

        var client = new AppRunSharedClient(requestAdapter, Substitute.For<ILoggerFactory>());

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

        var client = new AppRunSharedClient(requestAdapter, Substitute.For<ILoggerFactory>());

        Assert.Equal("https://secure.sakura.ad.jp/cloud/api/apprun/1.0/apprun/api", requestAdapter.BaseUrl);
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

        var client = new AppRunSharedClient(requestAdapter, Substitute.For<ILoggerFactory>());

        Assert.Equal(customBaseUrl, requestAdapter.BaseUrl);
    }

    /// <summary>
    /// Applicationsプロパティが正しくApplicationsRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Applications_ReturnsApplicationsRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";

        var client = new AppRunSharedClient(requestAdapter, Substitute.For<ILoggerFactory>());

        Assert.NotNull(client.Applications);
        Assert.IsType<ApplicationsRequestBuilder>(client.Applications);
    }

    /// <summary>
    /// Applicationsプロパティが毎回新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Applications_ReturnsNewInstance_OnEachAccess()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";

        var client = new AppRunSharedClient(requestAdapter, Substitute.For<ILoggerFactory>());

        var applications1 = client.Applications;
        var applications2 = client.Applications;

        Assert.NotSame(applications1, applications2);
    }

    /// <summary>
    /// Userプロパティが正しくUserRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void User_ReturnsUserRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";

        var client = new AppRunSharedClient(requestAdapter, Substitute.For<ILoggerFactory>());

        Assert.NotNull(client.User);
        Assert.IsType<UserRequestBuilder>(client.User);
    }

    /// <summary>
    /// Userプロパティが毎回新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void User_ReturnsNewInstance_OnEachAccess()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";

        var client = new AppRunSharedClient(requestAdapter, Substitute.For<ILoggerFactory>());

        var user1 = client.User;
        var user2 = client.User;

        Assert.NotSame(user1, user2);
    }
}
