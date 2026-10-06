using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.AppRunDedicated.Clusters.Item.Asg.Item.Worker_nodes.Item.Draining;
using Sacloud.Sdk.AppRunDedicated.Models;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.AppRunDedicated.Tests.Clusters.Item.Asg.Item.Worker_nodes.Item.Draining;

/// <summary>
/// DrainingRequestBuilderの単体テスト
/// </summary>
public class DrainingRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<DrainingRequestBuilder> _mockLogger;
    private readonly Guid _validGuid;

    public DrainingRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<DrainingRequestBuilder>>();
        _mockLoggerFactory.CreateLogger<DrainingRequestBuilder>().Returns(_mockLogger);
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

        var builder = new DrainingRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// rawUrlを使用したコンストラクタが正しく動作することを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_SetsProperties()
    {
        var rawUrl = "https://example.com/clusters/123/asg/456/worker_nodes/789/draining";
        var requestAdapter = Substitute.For<IRequestAdapter>();

        var builder = new DrainingRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// ToPutRequestInformationが正しいRequestInformationを生成することを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_ReturnsValidRequestInformation()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://example.com" },
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid },
            { "workerNodeID", _validGuid }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new DrainingRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var updateRequest = new UpdateWorkerNodeDrainingRequest
        {
            Draining = true
        };

        var requestInfo = builder.ToPutRequestInformation(updateRequest);

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.PUT, requestInfo.HttpMethod);
        Assert.Contains("draining", requestInfo.UrlTemplate);
    }

    /// <summary>
    /// PutAsyncのパスパラメータにclusterIDがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("clusterID", "")]
    [InlineData("clusterID", " ")]
    [InlineData("clusterID", "　")]
    public async Task PutAsync_WithEmptyClusterID_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { pathParameterKey, pathParameterValue },
            { "autoScalingGroupID", _validGuid },
            { "workerNodeID", _validGuid }
        };
        var builder = new DrainingRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateWorkerNodeDrainingRequest
        {
            Draining = true
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(body);
        });

        Assert.Equal("クラスタIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("クラスタIDを入力してください。");
    }

    /// <summary>
    /// PutAsyncのパスパラメータが有効なGUID形式の値でない場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("test-value")]
    [InlineData("1234-567890ab-cdef-1234-567890abcdef")]
    [InlineData("12345678-90ab-cdef-1234-567890ghijkl")]
    public async Task PutAsync_WithInvalidFormatClusterID_ThrowsValidationException(string invalidPathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "clusterID", invalidPathParameterValue },
            { "autoScalingGroupID", _validGuid },
            { "workerNodeID", _validGuid }
        };
        var builder = new DrainingRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateWorkerNodeDrainingRequest
        {
            Draining = true
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(body);
        });

        Assert.Equal("クラスタIDはGUID形式で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("クラスタIDはGUID形式で入力してください。");
    }

    /// <summary>
    /// PutAsyncのパスパラメータにautoScalingGroupIDがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("autoScalingGroupID", "")]
    [InlineData("autoScalingGroupID", " ")]
    [InlineData("autoScalingGroupID", "　")]
    public async Task PutAsync_WithEmptyAutoScalingGroupID_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "clusterID", _validGuid },
            { pathParameterKey, pathParameterValue },
            { "workerNodeID", _validGuid }
        };
        var builder = new DrainingRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateWorkerNodeDrainingRequest
        {
            Draining = true
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(body);
        });

        Assert.Equal("オートスケーリンググループIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("オートスケーリンググループIDを入力してください。");
    }

    /// <summary>
    /// PutAsyncのパスパラメータが有効なGUID形式の値でない場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("test-value")]
    [InlineData("1234-567890ab-cdef-1234-567890abcdef")]
    [InlineData("12345678-90ab-cdef-1234-567890ghijkl")]
    public async Task PutAsync_WithInvalidFormatAutoScalingGroupID_ThrowsValidationException(string invalidPathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "clusterID", _validGuid },
            { "autoScalingGroupID", invalidPathParameterValue },
            { "workerNodeID", _validGuid }
        };
        var builder = new DrainingRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateWorkerNodeDrainingRequest
        {
            Draining = true
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(body);
        });

        Assert.Equal("オートスケーリンググループIDはGUID形式で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("オートスケーリンググループIDはGUID形式で入力してください。");
    }

    /// <summary>
    /// PutAsyncのパスパラメータにworkerNodeIDがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("workerNodeID", "")]
    [InlineData("workerNodeID", " ")]
    [InlineData("workerNodeID", "　")]
    public async Task PutAsync_WithEmptyWorkerNodeID_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid },
            { pathParameterKey, pathParameterValue }
        };
        var builder = new DrainingRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateWorkerNodeDrainingRequest
        {
            Draining = true
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(body);
        });

        Assert.Equal("ワーカノードIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("ワーカノードIDを入力してください。");
    }

    /// <summary>
    /// PutAsyncのパスパラメータが有効なGUID形式の値でない場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("test-value")]
    [InlineData("1234-567890ab-cdef-1234-567890abcdef")]
    [InlineData("12345678-90ab-cdef-1234-567890ghijkl")]
    public async Task PutAsync_WithInvalidFormatWorkerNodeID_ThrowsValidationException(string invalidPathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid },
            { "workerNodeID", invalidPathParameterValue }
        };
        var builder = new DrainingRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateWorkerNodeDrainingRequest
        {
            Draining = true
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(body);
        });

        Assert.Equal("ワーカノードIDはGUID形式で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("ワーカノードIDはGUID形式で入力してください。");
    }

    /// <summary>
    /// PutAsyncのリクエストボディがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithNullBody_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid },
            { "workerNodeID", _validGuid }
        };
        var builder = new DrainingRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(null);
        });

        Assert.Equal("リクエストボディは必須です。", exception.Message);
        _mockLogger.Received(1).LogWarning("リクエストボディは必須です。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのパラメータがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithNullRequestBodyParameter_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid },
            { "workerNodeID", _validGuid }
        };
        var builder = new DrainingRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateWorkerNodeDrainingRequest
        {
            Draining = null
        };
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(body);
        });
        Assert.Equal("Drainingが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("Drainingが入力されていません。");
    }

    /// <summary>
    /// PutAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithApiError_ThrowsApiException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid },
            { "workerNodeID", _validGuid }
        };
        var builder = new DrainingRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateWorkerNodeDrainingRequest
        {
            Draining = true
        };
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

        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(async () =>
        {
            await builder.PutAsync(body);
        });

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^ワーカノードのdraining状態変更のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PutAsyncでその他のエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithOtherError_ThrowsException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid },
            { "workerNodeID", _validGuid }
        };
        var builder = new DrainingRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateWorkerNodeDrainingRequest
        {
            Draining = true
        };
        var expectedException = new Exception();

        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<Exception>(async () =>
        {
            await builder.PutAsync(body);
        });

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^ワーカノードのdraining状態の変更中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PutAsyncが正常に完了することを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_CompletesSuccessfully()
    {
        var pathParameters = new Dictionary<string, object> 
        { 
            { "baseurl", "https://example.com" }, 
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid },
            { "workerNodeID", _validGuid }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(Task.CompletedTask);
        var builder = new DrainingRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var updateRequest = new UpdateWorkerNodeDrainingRequest
        {
            Draining = true
        };

        await builder.PutAsync(updateRequest);

        await requestAdapter.Received(1).SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^ワーカノードのdraining状態の変更を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^ワーカノードのdraining状態の変更が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 204\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// WithUrlが新しいDrainingRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewBuilder()
    {
        var pathParameters = new Dictionary<string, object>();
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new DrainingRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var rawUrl = "https://example.com/clusters/123/asg/456/worker_nodes/789/draining";

        var newBuilder = builder.WithUrl(rawUrl);

        Assert.NotNull(newBuilder);
        Assert.IsType<DrainingRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }
}
