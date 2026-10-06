using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.AddOn.Ai.Status.Item.Item;
using Sacloud.Sdk.AddOn.Models;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.AddOn.Tests.Ai.Status.Item.Item;

/// <summary>
/// WithDeploymentNameItemRequestBuilderのテスト
/// </summary>
public class WithDeploymentNameItemRequestBuilderTests
{
    private readonly IRequestAdapter _mockRequestAdapter;
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<WithDeploymentNameItemRequestBuilder> _mockLogger;

    public WithDeploymentNameItemRequestBuilderTests()
    {
        _mockRequestAdapter = Substitute.For<IRequestAdapter>();
        _mockRequestAdapter.BaseUrl = "https://api.example.com";
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<WithDeploymentNameItemRequestBuilder>>();
        _mockLoggerFactory.CreateLogger<WithDeploymentNameItemRequestBuilder>().Returns(_mockLogger);
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
            { "resourceGroupName", "test-resourceGroupName" },
            { "deploymentName", "test-deploymentName" },
        };

        var builder = new WithDeploymentNameItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（RawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_InitializesSuccessfully()
    {
        var rawUrl = "https://api.example.com/ai/status/test-resourceGroupName/test-deploymentName";

        var builder = new WithDeploymentNameItemRequestBuilder(rawUrl, _mockRequestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// ToGetRequestInformationがGETメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_CreatesRequestInformationWithGetMethod()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "resourceGroupName", "test-resourceGroupName" },
            { "deploymentName", "test-deploymentName" },
        };
        var builder = new WithDeploymentNameItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.GET, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToGetRequestInformationが正しいAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_SetsAcceptHeader()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "resourceGroupName", "test-resourceGroupName" },
            { "deploymentName", "test-deploymentName" },
        };
        var builder = new WithDeploymentNameItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// ToGetRequestInformationが正しいPathParametersを設定することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_SetsPathParameters()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "resourceGroupName", "test-resourceGroupName" },
            { "deploymentName", "test-deploymentName" },
        };
        var builder = new WithDeploymentNameItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.True(requestInfo.PathParameters.ContainsKey("resourceGroupName"));
        Assert.Equal("test-resourceGroupName", requestInfo.PathParameters["resourceGroupName"]);
        Assert.True(requestInfo.PathParameters.ContainsKey("deploymentName"));
        Assert.Equal("test-deploymentName", requestInfo.PathParameters["deploymentName"]);
    }

    /// <summary>
    /// GetAsyncのパスパラメータにresourceGroupNameがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("resourceGroupName", "")]
    [InlineData("resourceGroupName", " ")]
    [InlineData("resourceGroupName", "　")]
    public async Task GetAsync_WithInvalidResourceGroupNameParameter_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { pathParameterKey, pathParameterValue },
            { "deploymentName", "test-deployment-name" }
        };
        var builder = new WithDeploymentNameItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        _mockRequestAdapter.SendAsync<DeploymentStatus>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<DeploymentStatus>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(new DeploymentStatus());

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync();
        });

        Assert.Equal("リソースグループ名を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("リソースグループ名を入力してください。");
    }

    /// <summary>
    /// GetAsyncのパスパラメータにdeploymentNameがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("deploymentName", "")]
    [InlineData("deploymentName", " ")]
    [InlineData("deploymentName", "　")]
    public async Task GetAsync_WithInvalidDeploymentNameParameter_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "resourceGroupName", "test-resource-group-name" },
            { pathParameterKey, pathParameterValue }
        };
        var builder = new WithDeploymentNameItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        _mockRequestAdapter.SendAsync<DeploymentStatus>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<DeploymentStatus>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(new DeploymentStatus());

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync();
        });

        Assert.Equal("デプロイメント名を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("デプロイメント名を入力してください。");
    }

    /// <summary>
    /// GetAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithApiError_ThrowsApiException()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "resourceGroupName", "test-resource-group-name" },
            { "deploymentName", "test-deployment-name" }
        };
        var builder = new WithDeploymentNameItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError
        {
            ResponseStatusCode = 401
        };
        ((IAdditionalDataHolder)expectedException).AdditionalData = new Dictionary<string, object>
        {
            { "error_code", "unauthorized" },
            { "error_msg", "error-unauthorized" },
            { "is_fatal", true },
            { "serial", "1234567890" },
            { "status", "401 Unauthorized" }
        };

        _mockRequestAdapter.SendAsync<DeploymentStatus>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<DeploymentStatus>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(async () =>
        {
            await builder.GetAsync();
        });

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^指定されたAI機能リソースのステータス取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncでその他のエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithOtherError_ThrowsException()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "resourceGroupName", "test-resource-group-name" },
            { "deploymentName", "test-deployment-name" }
        };
        var builder = new WithDeploymentNameItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        _mockRequestAdapter.SendAsync<DeploymentStatus>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<DeploymentStatus>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<Exception>(async () =>
        {
            await builder.GetAsync();
        });

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^指定されたAI機能リソースのステータスの取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsync が期待されるSendAsyncを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_CallsRequestAdapterSendAsync()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "resourceGroupName", "test-resourceGroupName" },
            { "deploymentName", "test-deploymentName" },
        };
        var builder = new WithDeploymentNameItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var expectedResponse = new DeploymentStatus();
        _mockRequestAdapter.SendAsync<DeploymentStatus>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<DeploymentStatus>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(expectedResponse);

        var result = await builder.GetAsync();

        await _mockRequestAdapter.Received(1).SendAsync<DeploymentStatus>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<DeploymentStatus>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());

        Assert.Equal(expectedResponse, result);

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^指定されたAI機能リソースのステータスの取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^指定されたAI機能リソースのステータスの取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// WithUrlが新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewInstance()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "resourceGroupName", "test-resourceGroupName" },
            { "deploymentName", "test-deploymentName" },
        };
        var builder = new WithDeploymentNameItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);
        var newUrl = "https://newapi.example.com/ai/status/new-resourceGroupName/new-deploymentName";

        var newBuilder = builder.WithUrl(newUrl);

        Assert.NotNull(newBuilder);
        Assert.NotSame(builder, newBuilder);
        Assert.IsType<WithDeploymentNameItemRequestBuilder>(newBuilder);
    }
}
