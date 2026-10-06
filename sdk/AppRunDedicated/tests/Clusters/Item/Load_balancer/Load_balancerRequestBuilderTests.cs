using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.AppRunDedicated.Clusters.Item.Load_balancer;
using Sacloud.Sdk.AppRunDedicated.Models;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.AppRunDedicated.Tests.Clusters.Item.Load_balancer;

/// <summary>
/// Load_balancerRequestBuilderのテスト
/// </summary>
public class Load_balancerRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<Load_balancerRequestBuilder> _mockLogger;
    private readonly Guid _clusterId = Guid.NewGuid();

    private static void AssertApiErrorMapping(Dictionary<string, ParsableFactory<IParsable>> mapping)
    {
        Assert.Single(mapping);
        Assert.True(mapping.ContainsKey("XXX"));
        Assert.Equal(ApiError.CreateFromDiscriminatorValue, mapping["XXX"]);
    }

    public Load_balancerRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<Load_balancerRequestBuilder>>();
        _mockLoggerFactory.CreateLogger(Arg.Any<string>()).Returns(_mockLogger);
    }

    /// <summary>
    /// コンストラクタとWithUrlが正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void ConstructorsAndWithUrl_InitializeSuccessfully()
    {
        var adapter = Substitute.For<IRequestAdapter>();
        var pathParameters = ValidPathParameters();

        Assert.NotNull(new Load_balancerRequestBuilder(pathParameters, adapter, _mockLoggerFactory));
        Assert.NotNull(new Load_balancerRequestBuilder("https://example.com/clusters/id/load_balancer", adapter, _mockLoggerFactory));
        Assert.NotNull(new Load_balancerRequestBuilder(pathParameters, adapter, _mockLoggerFactory).WithUrl("https://example.com/clusters/id/load_balancer"));
    }

    /// <summary>
    /// ToGetRequestInformationがGETリクエストを作成することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_CreatesGetRequest()
    {
        var request = CreateBuilder(Substitute.For<IRequestAdapter>()).ToGetRequestInformation();

        Assert.Equal(Method.GET, request.HttpMethod);
        Assert.Contains("application/json", request.Headers["Accept"]);
        Assert.Contains("load_balancer", request.UrlTemplate);
    }

    /// <summary>
    /// ToPatchRequestInformationがマージパッチリクエストを作成することを確認
    /// </summary>
    [Fact]
    public void ToPatchRequestInformation_CreatesMergePatchRequest()
    {
        var request = CreateBuilder(Substitute.For<IRequestAdapter>()).ToPatchRequestInformation(ValidBody());

        Assert.Equal(Method.PATCH, request.HttpMethod);
        Assert.Contains("application/json", request.Headers["Accept"]);
        Assert.Contains("application/merge-patch+json", request.Headers["Content-Type"]);
    }

    /// <summary>
    /// 不正なクラスタIDでGetAsyncを呼び出すとバリデーション例外が発生することを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "value", "クラスタIDを入力してください。")]
    [InlineData("clusterID", "invalid", "クラスタIDはGUID形式で入力してください。")]
    public async Task GetAsync_WithInvalidClusterId_ThrowsValidationException(string key, string value, string message)
    {
        var builder = new Load_balancerRequestBuilder(new Dictionary<string, object> { ["baseurl"] = "https://example.com", [key] = value }, Substitute.For<IRequestAdapter>(), _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync());

        Assert.Equal(message, exception.Message);
        _mockLogger.Received(1).LogWarning(message);
    }

    /// <summary>
    /// nullのリクエストボディでPatchAsyncを呼び出すとバリデーション例外が発生することを確認
    /// </summary>
    [Fact]
    public async Task PatchAsync_WithNullBody_ThrowsValidationException()
    {
        var exception = await Assert.ThrowsAsync<ValidationException>(() => CreateBuilder(Substitute.For<IRequestAdapter>()).PatchAsync(null!));

        Assert.Equal("リクエストボディは必須です。", exception.Message);
        _mockLogger.Received(1).LogWarning("リクエストボディは必須です。");
    }

    /// <summary>
    /// 不正なリクエストボディでPatchAsyncを呼び出すとバリデーション例外が発生することを確認
    /// </summary>
    [Theory]
    [InlineData("invalid-email", "LetsEncryptEmailの形式が正しくありません。")]
    [InlineData("six-ports", "Portsの要素数は5以下にしてください。")]
    [InlineData("null-port", "Ports.Portが入力されていません。")]
    [InlineData("out-of-range", "Ports.Portの値は1以上65535以下で入力してください。")]
    [InlineData("denied-port", "Ports.Portには許可された値を入力してください。")]
    [InlineData("invalid-protocol", "Ports.ProtocolはEnumに含まれる値を入力してください。")]
    public async Task PatchAsync_WithInvalidBody_ThrowsValidationException(string invalidCase, string message)
    {
        var body = ValidBody();
        switch (invalidCase)
        {
            case "invalid-email": body.LetsEncryptEmail = "invalid-email"; break;
            case "six-ports": body.Ports = Enumerable.Range(0, 6).Select(_ => ValidPort()).ToList(); break;
            case "null-port": body.Ports = new List<ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly> { new() { Protocol = ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly_protocol.Http } }; break;
            case "out-of-range": body.Ports = new List<ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly> { new() { Port = 65536, Protocol = ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly_protocol.Http } }; break;
            case "denied-port": body.Ports = new List<ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly> { new() { Port = 5950, Protocol = ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly_protocol.Http } }; break;
            case "invalid-protocol": body.Ports = new List<ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly> { new() { Port = 8080, Protocol = (ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly_protocol)99 } }; break;
        }

        var exception = await Assert.ThrowsAsync<ValidationException>(() => CreateBuilder(Substitute.For<IRequestAdapter>()).PatchAsync(body));

        Assert.Equal(message, exception.Message);
        _mockLogger.Received(1).LogWarning(message);
    }

    /// <summary>
    /// GetAsyncがレスポンスを返し、成功ログを出力することを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_ReturnsResponseAndLogsSuccess()
    {
        var adapter = Substitute.For<IRequestAdapter>();
        var expected = new ReadClusterLoadBalancerResponse();
        Dictionary<string, ParsableFactory<IParsable>>? errorMapping = null;
        adapter.SendAsync<ReadClusterLoadBalancerResponse>(Arg.Any<RequestInformation>(), Arg.Any<ParsableFactory<ReadClusterLoadBalancerResponse>>(), Arg.Do<Dictionary<string, ParsableFactory<IParsable>>>(mapping => errorMapping = mapping), Arg.Any<CancellationToken>()).Returns(expected);

        var result = await CreateBuilder(adapter).GetAsync();

        Assert.Same(expected, result);
        Assert.NotNull(errorMapping);
        AssertApiErrorMapping(errorMapping);
        AssertInformationLogged("クラスタのロードバランサ設定の取得を開始します。");
        AssertInformationLogged("クラスタのロードバランサ設定の取得が完了しました。");
    }

    /// <summary>
    /// PatchAsyncがリクエストアダプターを呼び出し、成功ログを出力することを確認
    /// </summary>
    [Fact]
    public async Task PatchAsync_CallsAdapterAndLogsSuccess()
    {
        var adapter = Substitute.For<IRequestAdapter>();
        Dictionary<string, ParsableFactory<IParsable>>? errorMapping = null;
        adapter.SendNoContentAsync(Arg.Any<RequestInformation>(), Arg.Do<Dictionary<string, ParsableFactory<IParsable>>>(mapping => errorMapping = mapping), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        await CreateBuilder(adapter).PatchAsync(ValidBody());

        await adapter.Received(1).SendNoContentAsync(Arg.Any<RequestInformation>(), Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(), Arg.Any<CancellationToken>());
        Assert.NotNull(errorMapping);
        AssertApiErrorMapping(errorMapping);
        AssertInformationLogged("クラスタのロードバランサ設定の更新を開始します。");
        AssertInformationLogged("クラスタのロードバランサ設定の更新が完了しました。");
    }

    /// <summary>
    /// GetAsyncでAPIエラーが発生した場合にエラーログを出力して再スローすることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithApiError_LogsAndRethrows()
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.SendAsync<ReadClusterLoadBalancerResponse>(Arg.Any<RequestInformation>(), Arg.Any<ParsableFactory<ReadClusterLoadBalancerResponse>>(), Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(), Arg.Any<CancellationToken>()).ThrowsAsync(new ApiError { ResponseStatusCode = 401 });

        await Assert.ThrowsAsync<ApiError>(() => CreateBuilder(adapter).GetAsync());

        AssertErrorLogged("クラスタのロードバランサ設定取得のAPIエラーが発生しました。");
    }

    /// <summary>
    /// GetAsyncでその他のエラーが発生した場合にエラーログを出力して再スローすることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithOtherError_LogsAndRethrows()
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.SendAsync<ReadClusterLoadBalancerResponse>(Arg.Any<RequestInformation>(), Arg.Any<ParsableFactory<ReadClusterLoadBalancerResponse>>(), Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(), Arg.Any<CancellationToken>()).ThrowsAsync(new InvalidOperationException());

        await Assert.ThrowsAsync<InvalidOperationException>(() => CreateBuilder(adapter).GetAsync());

        AssertErrorLogged("クラスタのロードバランサ設定の取得中にエラーが発生しました。");
    }

    /// <summary>
    /// PatchAsyncでAPIエラーが発生した場合にエラーログを出力して再スローすることを確認
    /// </summary>
    [Fact]
    public async Task PatchAsync_WithApiError_LogsAndRethrows()
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.SendNoContentAsync(Arg.Any<RequestInformation>(), Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(), Arg.Any<CancellationToken>()).ThrowsAsync(new ApiError { ResponseStatusCode = 400 });

        await Assert.ThrowsAsync<ApiError>(() => CreateBuilder(adapter).PatchAsync(ValidBody()));

        AssertErrorLogged("クラスタのロードバランサ設定更新のAPIエラーが発生しました。");
    }

    /// <summary>
    /// PatchAsyncでその他のエラーが発生した場合にエラーログを出力して再スローすることを確認
    /// </summary>
    [Fact]
    public async Task PatchAsync_WithOtherError_LogsAndRethrows()
    {
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.SendNoContentAsync(Arg.Any<RequestInformation>(), Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(), Arg.Any<CancellationToken>()).ThrowsAsync(new InvalidOperationException());

        await Assert.ThrowsAsync<InvalidOperationException>(() => CreateBuilder(adapter).PatchAsync(ValidBody()));

        AssertErrorLogged("クラスタのロードバランサ設定の更新中にエラーが発生しました。");
    }

    private Load_balancerRequestBuilder CreateBuilder(IRequestAdapter adapter) => new(ValidPathParameters(), adapter, _mockLoggerFactory);
    private Dictionary<string, object> ValidPathParameters() => new() { ["baseurl"] = "https://example.com", ["clusterID"] = _clusterId };
    private static PatchClusterLoadBalancerRequest ValidBody() => new() { LetsEncryptEmail = "test@example.com", Ports = new List<ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly> { ValidPort() } };
    private static ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly ValidPort() => new() { Port = 8080, Protocol = ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly_protocol.Http };

    private void AssertInformationLogged(string messagePrefix) => _mockLogger.Received().Log(LogLevel.Information, Arg.Any<EventId>(), Arg.Is<object>(state => state.ToString()!.StartsWith(messagePrefix)), null, Arg.Any<Func<object, Exception?, string>>());
    private void AssertErrorLogged(string messagePrefix) => _mockLogger.Received().Log(LogLevel.Error, Arg.Any<EventId>(), Arg.Is<object>(state => state.ToString()!.StartsWith(messagePrefix)), Arg.Any<Exception>(), Arg.Any<Func<object, Exception?, string>>());
}
