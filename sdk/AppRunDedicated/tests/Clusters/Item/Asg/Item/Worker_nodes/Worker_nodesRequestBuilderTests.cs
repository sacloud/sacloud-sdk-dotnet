using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.AppRunDedicated.Clusters.Item.Asg.Item.Worker_nodes;
using Sacloud.Sdk.AppRunDedicated.Clusters.Item.Asg.Item.Worker_nodes.Item;
using Sacloud.Sdk.AppRunDedicated.Models;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.AppRunDedicated.Tests.Clusters.Item.Asg.Item.Worker_nodes;

/// <summary>
/// Worker_nodesRequestBuilderの単体テスト
/// </summary>
public class Worker_nodesRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<Worker_nodesRequestBuilder> _mockLogger;
    private readonly Guid _validGuid;

    public Worker_nodesRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<Worker_nodesRequestBuilder>>();
        _mockLoggerFactory.CreateLogger(Arg.Any<string>()).Returns(_mockLogger);
        _validGuid = Guid.NewGuid();
    }

    /// <summary>
    /// PathParametersを使用したコンストラクタが正しく動作することを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_SetsProperties()
    {
        var pathParameters = new Dictionary<string, object>();
        var requestAdapter = Substitute.For<IRequestAdapter>();

        var builder = new Worker_nodesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// rawUrlを使用したコンストラクタが正しく動作することを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_SetsProperties()
    {
        var rawUrl = "https://example.com/clusters/123/asg/456/worker_nodes";
        var requestAdapter = Substitute.For<IRequestAdapter>();

        var builder = new Worker_nodesRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// Guidインデクサーが正しくWithWorkerNodeItemRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_WithGuid_ReturnsWithWorkerNodeItemRequestBuilder()
    {
        var pathParameters = new Dictionary<string, object>();
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new Worker_nodesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var workerNodeId = Guid.NewGuid();

        var itemBuilder = builder[workerNodeId];

        Assert.NotNull(itemBuilder);
        Assert.IsType<WithWorkerNodeItemRequestBuilder>(itemBuilder);
    }

    /// <summary>
    /// ToGetRequestInformationが正しいRequestInformationを生成することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_ReturnsValidRequestInformation()
    {
        var pathParameters = new Dictionary<string, object> 
        { 
            { "baseurl", "https://example.com" }, 
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new Worker_nodesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.GET, requestInfo.HttpMethod);
        Assert.Contains("worker_nodes", requestInfo.UrlTemplate);
    }

    /// <summary>
    /// ToGetRequestInformationがQueryParametersを正しく設定することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_WithQueryParameters_SetsQueryParameters()
    {
        var pathParameters = new Dictionary<string, object> 
        { 
            { "baseurl", "https://example.com" }, 
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new Worker_nodesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var cursor = Guid.NewGuid();

        var requestInfo = builder.ToGetRequestInformation(config =>
        {
            config.QueryParameters.Cursor = cursor;
            config.QueryParameters.MaxItems = 10;
        });

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.GET, requestInfo.HttpMethod);
    }

    /// <summary>
    /// GetAsyncのパスパラメータにclusterIDがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("clusterID", "")]
    [InlineData("clusterID", " ")]
    [InlineData("clusterID", "　")]
    public async Task GetAsync_WithEmptyClusterID_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { pathParameterKey, pathParameterValue },
            { "autoScalingGroupID", _validGuid }
        };
        var builder = new Worker_nodesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync();
        });

        Assert.Equal("クラスタIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("クラスタIDを入力してください。");
    }

    /// <summary>
    /// GetAsyncのパスパラメータが有効なGUID形式の値でない場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("test-value")]
    [InlineData("1234-567890ab-cdef-1234-567890abcdef")]
    [InlineData("12345678-90ab-cdef-1234-567890ghijkl")]
    public async Task GetAsync_WithInvalidFormatClusterID_ThrowsValidationException(string invalidPathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "clusterID", invalidPathParameterValue },
            { "autoScalingGroupID", _validGuid }
        };
        var builder = new Worker_nodesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync();
        });

        Assert.Equal("クラスタIDはGUID形式で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("クラスタIDはGUID形式で入力してください。");
    }

    /// <summary>
    /// GetAsyncのパスパラメータにautoScalingGroupIDがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("autoScalingGroupID", "")]
    [InlineData("autoScalingGroupID", " ")]
    [InlineData("autoScalingGroupID", "　")]
    public async Task GetAsync_WithEmptyAutoScalingGroupID_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "clusterID", _validGuid },
            { pathParameterKey, pathParameterValue }
        };
        var builder = new Worker_nodesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync();
        });

        Assert.Equal("オートスケーリンググループIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("オートスケーリンググループIDを入力してください。");
    }

    /// <summary>
    /// GetAsyncのパスパラメータが有効なGUID形式の値でない場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("test-value")]
    [InlineData("1234-567890ab-cdef-1234-567890abcdef")]
    [InlineData("12345678-90ab-cdef-1234-567890ghijkl")]
    public async Task GetAsync_WithInvalidFormatAutoScalingGroupID_ThrowsValidationException(string invalidPathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "clusterID", _validGuid },
            { "autoScalingGroupID", invalidPathParameterValue }
        };
        var builder = new Worker_nodesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync();
        });

        Assert.Equal("オートスケーリンググループIDはGUID形式で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("オートスケーリンググループIDはGUID形式で入力してください。");
    }

    /// <summary>
    /// GetAsyncのリクエストパラメータのMaxItemsがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithNullMaxItems_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid }
        };
        var builder = new Worker_nodesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync(config =>
            {
                config.QueryParameters.MaxItems = null;
            });
        });

        Assert.Equal("MaxItemsが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("MaxItemsが入力されていません。");
    }

    /// <summary>
    /// GetAsyncのリクエストパラメータのMaxItemsが有効な数値範囲でない値の場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(1)]
    [InlineData(101)]
    public async Task GetAsync_WithInvalidRangeMaxItems_ThrowsValidationException(int invalidMaxItemsValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid }
        };
        var builder = new Worker_nodesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync(config =>
            {
                config.QueryParameters.MaxItems = invalidMaxItemsValue;
            });
        });

        Assert.Equal("MaxItemsの値は2以上100以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("MaxItemsの値は2以上100以下で入力してください。");
    }

    /// <summary>
    /// GetAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithApiError_ThrowsApiException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid }
        };
        var builder = new Worker_nodesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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

        requestAdapter.SendAsync<ListWorkerNodesResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListWorkerNodesResponse>>(),
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
                @"^Auto Scaling Group に所属するワーカノード一覧取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncでその他のエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithOtherError_ThrowsException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid }
        };
        var builder = new Worker_nodesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync<ListWorkerNodesResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListWorkerNodesResponse>>(),
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
                @"^Auto Scaling Group に所属するワーカノード一覧の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncがListWorkerNodesResponseを返すことを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_ReturnsListWorkerNodesResponse()
    {
        var pathParameters = new Dictionary<string, object> 
        { 
            { "baseurl", "https://example.com" }, 
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var expectedResponse = new ListWorkerNodesResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListWorkerNodesResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);
        var builder = new Worker_nodesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var response = await builder.GetAsync();

        Assert.NotNull(response);
        Assert.IsType<ListWorkerNodesResponse>(response);

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^Auto Scaling Group に所属するワーカノード一覧の取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^Auto Scaling Group に所属するワーカノード一覧の取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// WithUrlが新しいWorker_nodesRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewBuilder()
    {
        var pathParameters = new Dictionary<string, object>();
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new Worker_nodesRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var rawUrl = "https://example.com/clusters/123/asg/456/worker_nodes";

        var newBuilder = builder.WithUrl(rawUrl);

        Assert.NotNull(newBuilder);
        Assert.IsType<Worker_nodesRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }

    /// <summary>
    /// QueryParametersのCursorプロパティが正しく設定されることを確認
    /// </summary>
    [Fact]
    public void Worker_nodesRequestBuilderGetQueryParameters_Cursor_SetAndGet()
    {
        var queryParameters = new Worker_nodesRequestBuilder.Worker_nodesRequestBuilderGetQueryParameters();
        var cursor = Guid.NewGuid();

        queryParameters.Cursor = cursor;

        Assert.Equal(cursor, queryParameters.Cursor);
    }

    /// <summary>
    /// QueryParametersのMaxItemsプロパティが正しく設定されることを確認
    /// </summary>
    [Fact]
    public void Worker_nodesRequestBuilderGetQueryParameters_MaxItems_SetAndGet()
    {
        var queryParameters = new Worker_nodesRequestBuilder.Worker_nodesRequestBuilderGetQueryParameters
        {
            MaxItems = 10
        };

        Assert.Equal(10, queryParameters.MaxItems);
    }
}
