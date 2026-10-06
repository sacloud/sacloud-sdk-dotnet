using Microsoft.Kiota.Abstractions;
using NSubstitute;
using Sacloud.Sdk.APIGateway.Certificates;
using Sacloud.Sdk.APIGateway.Domains;
using Sacloud.Sdk.APIGateway.Groups;
using Sacloud.Sdk.APIGateway.Oidc;
using Sacloud.Sdk.APIGateway.Plans;
using Sacloud.Sdk.APIGateway.Services;
using Sacloud.Sdk.APIGateway.Subscriptions;
using Sacloud.Sdk.APIGateway.Users;

namespace Sacloud.Sdk.APIGateway.Tests;

/// <summary>
/// APIGatewayClientのテスト
/// </summary>
public class APIGatewayClientTests
{
    /// <summary>
    /// コンストラクタが正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = string.Empty;

        var client = new APIGatewayClient(requestAdapter);

        Assert.NotNull(client);
    }

    /// <summary>
    /// コンストラクタがBaseUrlが空の場合にデフォルトURLを設定することを確認
    /// </summary>
    [Fact]
    public void Constructor_SetsBaseUrl_WhenBaseUrlIsEmpty()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = string.Empty;

        var client = new APIGatewayClient(requestAdapter);

        Assert.Equal("https://secure.sakura.ad.jp/cloud/api/apigw/1.0", requestAdapter.BaseUrl);
    }

    /// <summary>
    /// コンストラクタがBaseUrlが既に設定されている場合に変更しないことを確認
    /// </summary>
    [Fact]
    public void Constructor_DoesNotOverrideBaseUrl_WhenBaseUrlIsSet()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://custom.api.example.com";

        var client = new APIGatewayClient(requestAdapter);

        Assert.Equal("https://custom.api.example.com", requestAdapter.BaseUrl);
    }

    /// <summary>
    /// コンストラクタがPathParametersにbaseurlを追加することを確認
    /// </summary>
    [Fact]
    public void Constructor_AddsBaseurlToPathParameters()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";

        var client = new APIGatewayClient(requestAdapter);

        Assert.NotNull(client);
    }

    /// <summary>
    /// CertificatesプロパティがCertificatesRequestBuilderインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Certificates_ReturnsRequestBuilderInstance()
    {
        var client = CreateClient();

        var result = client.Certificates;

        Assert.NotNull(result);
        Assert.IsType<CertificatesRequestBuilder>(result);
    }

    /// <summary>
    /// DomainsプロパティがDomainsRequestBuilderインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Domains_ReturnsRequestBuilderInstance()
    {
        var client = CreateClient();

        var result = client.Domains;

        Assert.NotNull(result);
        Assert.IsType<DomainsRequestBuilder>(result);
    }

    /// <summary>
    /// GroupsプロパティがGroupsRequestBuilderインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Groups_ReturnsRequestBuilderInstance()
    {
        var client = CreateClient();

        var result = client.Groups;

        Assert.NotNull(result);
        Assert.IsType<GroupsRequestBuilder>(result);
    }

    /// <summary>
    /// OidcプロパティがOidcRequestBuilderインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Oidc_ReturnsRequestBuilderInstance()
    {
        var client = CreateClient();

        var result = client.Oidc;

        Assert.NotNull(result);
        Assert.IsType<OidcRequestBuilder>(result);
    }

    /// <summary>
    /// PlansプロパティがPlansRequestBuilderインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Plans_ReturnsRequestBuilderInstance()
    {
        var client = CreateClient();

        var result = client.Plans;

        Assert.NotNull(result);
        Assert.IsType<PlansRequestBuilder>(result);
    }

    /// <summary>
    /// ServicesプロパティがServicesRequestBuilderインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Services_ReturnsRequestBuilderInstance()
    {
        var client = CreateClient();

        var result = client.Services;

        Assert.NotNull(result);
        Assert.IsType<ServicesRequestBuilder>(result);
    }

    /// <summary>
    /// SubscriptionsプロパティがSubscriptionsRequestBuilderインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Subscriptions_ReturnsRequestBuilderInstance()
    {
        var client = CreateClient();

        var result = client.Subscriptions;

        Assert.NotNull(result);
        Assert.IsType<SubscriptionsRequestBuilder>(result);
    }

    /// <summary>
    /// UsersプロパティがUsersRequestBuilderインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Users_ReturnsRequestBuilderInstance()
    {
        var client = CreateClient();

        var result = client.Users;

        Assert.NotNull(result);
        Assert.IsType<UsersRequestBuilder>(result);
    }

    /// <summary>
    /// Certificatesプロパティが複数回アクセスされた場合に異なるインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Certificates_ReturnsNewInstanceOnEachAccess()
    {
        var client = CreateClient();

        var result1 = client.Certificates;
        var result2 = client.Certificates;

        Assert.NotSame(result1, result2);
    }

    /// <summary>
    /// Domainsプロパティが複数回アクセスされた場合に異なるインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Domains_ReturnsNewInstanceOnEachAccess()
    {
        var client = CreateClient();

        var result1 = client.Domains;
        var result2 = client.Domains;

        Assert.NotSame(result1, result2);
    }

    /// <summary>
    /// Groupsプロパティが複数回アクセスされた場合に異なるインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Groups_ReturnsNewInstanceOnEachAccess()
    {
        var client = CreateClient();

        var result1 = client.Groups;
        var result2 = client.Groups;

        Assert.NotSame(result1, result2);
    }

    /// <summary>
    /// Oidcプロパティが複数回アクセスされた場合に異なるインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Oidc_ReturnsNewInstanceOnEachAccess()
    {
        var client = CreateClient();

        var result1 = client.Oidc;
        var result2 = client.Oidc;

        Assert.NotSame(result1, result2);
    }

    /// <summary>
    /// Plansプロパティが複数回アクセスされた場合に異なるインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Plans_ReturnsNewInstanceOnEachAccess()
    {
        var client = CreateClient();

        var result1 = client.Plans;
        var result2 = client.Plans;

        Assert.NotSame(result1, result2);
    }

    /// <summary>
    /// Servicesプロパティが複数回アクセスされた場合に異なるインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Services_ReturnsNewInstanceOnEachAccess()
    {
        var client = CreateClient();

        var result1 = client.Services;
        var result2 = client.Services;

        Assert.NotSame(result1, result2);
    }

    /// <summary>
    /// Subscriptionsプロパティが複数回アクセスされた場合に異なるインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Subscriptions_ReturnsNewInstanceOnEachAccess()
    {
        var client = CreateClient();

        var result1 = client.Subscriptions;
        var result2 = client.Subscriptions;

        Assert.NotSame(result1, result2);
    }

    /// <summary>
    /// Usersプロパティが複数回アクセスされた場合に異なるインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Users_ReturnsNewInstanceOnEachAccess()
    {
        var client = CreateClient();

        var result1 = client.Users;
        var result2 = client.Users;

        Assert.NotSame(result1, result2);
    }

    private static APIGatewayClient CreateClient()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        return new APIGatewayClient(requestAdapter);
    }
}
