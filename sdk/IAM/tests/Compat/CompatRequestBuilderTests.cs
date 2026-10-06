using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using NSubstitute;
using Sacloud.Sdk.IAM.Compat;
using Sacloud.Sdk.IAM.Compat.ApiKeys;
using Sacloud.Sdk.IAM.Compat.Users;

namespace Sacloud.Sdk.IAM.Tests.Compat;

/// <summary>
/// CompatRequestBuilderのテスト
/// </summary>
public class CompatRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<CompatRequestBuilder> _mockLogger;

    public CompatRequestBuilderTests()
    {
        _mockLogger = Substitute.For<ILogger<CompatRequestBuilder>>();
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLoggerFactory.CreateLogger<CompatRequestBuilder>().Returns(_mockLogger);
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

        var builder = new CompatRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var rawUrl = "https://api.example.com/compat";

        var builder = new CompatRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// ApiKeysプロパティが正しいApiKeysRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void ApiKeys_ReturnsApiKeysRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new CompatRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var apiKeysBuilder = builder.ApiKeys;

        Assert.NotNull(apiKeysBuilder);
        Assert.IsType<ApiKeysRequestBuilder>(apiKeysBuilder);
    }

    /// <summary>
    /// ApiKeysプロパティが毎回新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void ApiKeys_ReturnsNewInstance_OnEachCall()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new CompatRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var apiKeysBuilder1 = builder.ApiKeys;
        var apiKeysBuilder2 = builder.ApiKeys;

        Assert.NotSame(apiKeysBuilder1, apiKeysBuilder2);
    }

    /// <summary>
    /// Usersプロパティが正しいUsersRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Users_ReturnsUsersRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new CompatRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var usersBuilder = builder.Users;

        Assert.NotNull(usersBuilder);
        Assert.IsType<UsersRequestBuilder>(usersBuilder);
    }

    /// <summary>
    /// Usersプロパティが毎回新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Users_ReturnsNewInstance_OnEachCall()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new CompatRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var usersBuilder1 = builder.Users;
        var usersBuilder2 = builder.Users;

        Assert.NotSame(usersBuilder1, usersBuilder2);
    }
}
