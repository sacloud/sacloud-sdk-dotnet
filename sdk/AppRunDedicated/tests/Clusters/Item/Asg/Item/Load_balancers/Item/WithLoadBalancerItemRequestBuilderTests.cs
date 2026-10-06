using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.AppRunDedicated.Clusters.Item.Asg.Item.Load_balancers.Item;
using Sacloud.Sdk.AppRunDedicated.Models;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.AppRunDedicated.Tests.Clusters.Item.Asg.Item.Load_balancers.Item;

/// <summary>
/// WithLoadBalancerItemRequestBuilderのテスト
/// </summary>
public class WithLoadBalancerItemRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<WithLoadBalancerItemRequestBuilder> _mockLogger;
    private readonly Guid _validGuid;

    public WithLoadBalancerItemRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<WithLoadBalancerItemRequestBuilder>>();
        _mockLoggerFactory.CreateLogger<WithLoadBalancerItemRequestBuilder>().Returns(_mockLogger);
        _validGuid = Guid.NewGuid();
    }

    /// <summary>
    /// コンストラクタ（PathParameters）が正常に動作することを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_SetsProperties()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid },
            { "loadBalancerID", _validGuid }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();

        var builder = new WithLoadBalancerItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（rawUrl）が正常に動作することを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_SetsProperties()
    {
        var rawUrl = "https://example.com/clusters/00000000-0000-0000-0000-000000000000/asg/00000000-0000-0000-0000-000000000000/load_balancers/00000000-0000-0000-0000-000000000000";
        var requestAdapter = Substitute.For<IRequestAdapter>();

        var builder = new WithLoadBalancerItemRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// Load_balancer_nodesプロパティが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Load_balancer_nodes_ReturnsRequestBuilder()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid },
            { "loadBalancerID", _validGuid }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WithLoadBalancerItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var nodesBuilder = builder.Load_balancer_nodes;

        Assert.NotNull(nodesBuilder);
    }

    /// <summary>
    /// ToGetRequestInformationが正しいRequestInformationを返すことを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_ReturnsRequestInformation()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid },
            { "loadBalancerID", _validGuid }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WithLoadBalancerItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.GET, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToDeleteRequestInformationが正しいRequestInformationを返すことを確認
    /// </summary>
    [Fact]
    public void ToDeleteRequestInformation_ReturnsRequestInformation()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid },
            { "loadBalancerID", _validGuid }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WithLoadBalancerItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToDeleteRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.DELETE, requestInfo.HttpMethod);
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
            { "autoScalingGroupID", _validGuid },
            { "loadBalancerID", _validGuid }
        };
        var builder = new WithLoadBalancerItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
            { "autoScalingGroupID", _validGuid },
            { "loadBalancerID", _validGuid }
        };
        var builder = new WithLoadBalancerItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
            { pathParameterKey, pathParameterValue },
            { "loadBalancerID", _validGuid }
        };
        var builder = new WithLoadBalancerItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
            { "autoScalingGroupID", invalidPathParameterValue },
            { "loadBalancerID", _validGuid }
        };
        var builder = new WithLoadBalancerItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync();
        });

        Assert.Equal("オートスケーリンググループIDはGUID形式で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("オートスケーリンググループIDはGUID形式で入力してください。");
    }

    /// <summary>
    /// GetAsyncのパスパラメータにloadBalancerIDがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("loadBalancerID", "")]
    [InlineData("loadBalancerID", " ")]
    [InlineData("loadBalancerID", "　")]
    public async Task GetAsync_WithEmptyLoadBalancerID_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid },
            { pathParameterKey, pathParameterValue }
        };
        var builder = new WithLoadBalancerItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync();
        });

        Assert.Equal("ロードバランサIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("ロードバランサIDを入力してください。");
    }

    /// <summary>
    /// GetAsyncのパスパラメータが有効なGUID形式の値でない場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("test-value")]
    [InlineData("1234-567890ab-cdef-1234-567890abcdef")]
    [InlineData("12345678-90ab-cdef-1234-567890ghijkl")]
    public async Task GetAsync_WithInvalidFormatLoadBalancerID_ThrowsValidationException(string invalidPathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid },
            { "loadBalancerID", invalidPathParameterValue }
        };
        var builder = new WithLoadBalancerItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync();
        });

        Assert.Equal("ロードバランサIDはGUID形式で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("ロードバランサIDはGUID形式で入力してください。");
    }

    /// <summary>
    /// GetAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithApiError_ThrowsApiException()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid },
            { "loadBalancerID", _validGuid }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WithLoadBalancerItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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

        requestAdapter.SendAsync<ReadLoadBalancerResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ReadLoadBalancerResponse>>(),
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
                @"^ロードバランサの詳細情報取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid },
            { "loadBalancerID", _validGuid }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WithLoadBalancerItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync<ReadLoadBalancerResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ReadLoadBalancerResponse>>(),
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
                @"^ロードバランサの詳細情報の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncが正しく呼び出されることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_CallsRequestAdapter()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid },
            { "loadBalancerID", _validGuid }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WithLoadBalancerItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new ReadLoadBalancerResponse();

        requestAdapter.SendAsync<ReadLoadBalancerResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ReadLoadBalancerResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.GetAsync();

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ReadLoadBalancerResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());

        Assert.Equal(expectedResponse, result);
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^ロードバランサの詳細情報の取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^ロードバランサの詳細情報の取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// DeleteAsyncのパスパラメータにclusterIDがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("clusterID", "")]
    [InlineData("clusterID", " ")]
    [InlineData("clusterID", "　")]
    public async Task DeleteAsync_WithEmptyClusterID_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { pathParameterKey, pathParameterValue },
            { "autoScalingGroupID", _validGuid },
            { "loadBalancerID", _validGuid }
        };
        var builder = new WithLoadBalancerItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.DeleteAsync();
        });

        Assert.Equal("クラスタIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("クラスタIDを入力してください。");
    }

    /// <summary>
    /// DeleteAsyncのパスパラメータが有効なGUID形式の値でない場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("test-value")]
    [InlineData("1234-567890ab-cdef-1234-567890abcdef")]
    [InlineData("12345678-90ab-cdef-1234-567890ghijkl")]
    public async Task DeleteAsync_WithInvalidFormatClusterID_ThrowsValidationException(string invalidPathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "clusterID", invalidPathParameterValue },
            { "autoScalingGroupID", _validGuid },
            { "loadBalancerID", _validGuid }
        };
        var builder = new WithLoadBalancerItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.DeleteAsync();
        });

        Assert.Equal("クラスタIDはGUID形式で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("クラスタIDはGUID形式で入力してください。");
    }

    /// <summary>
    /// DeleteAsyncのパスパラメータにautoScalingGroupIDがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("autoScalingGroupID", "")]
    [InlineData("autoScalingGroupID", " ")]
    [InlineData("autoScalingGroupID", "　")]
    public async Task DeleteAsync_WithEmptyAutoScalingGroupID_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "clusterID", _validGuid },
            { pathParameterKey, pathParameterValue },
            { "loadBalancerID", _validGuid }
        };
        var builder = new WithLoadBalancerItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.DeleteAsync();
        });

        Assert.Equal("オートスケーリンググループIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("オートスケーリンググループIDを入力してください。");
    }

    /// <summary>
    /// DeleteAsyncのパスパラメータが有効なGUID形式の値でない場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("test-value")]
    [InlineData("1234-567890ab-cdef-1234-567890abcdef")]
    [InlineData("12345678-90ab-cdef-1234-567890ghijkl")]
    public async Task DeleteAsync_WithInvalidFormatAutoScalingGroupID_ThrowsValidationException(string invalidPathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "clusterID", _validGuid },
            { "autoScalingGroupID", invalidPathParameterValue },
            { "loadBalancerID", _validGuid }
        };
        var builder = new WithLoadBalancerItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.DeleteAsync();
        });

        Assert.Equal("オートスケーリンググループIDはGUID形式で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("オートスケーリンググループIDはGUID形式で入力してください。");
    }

    /// <summary>
    /// DeleteAsyncのパスパラメータにloadBalancerIDがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("loadBalancerID", "")]
    [InlineData("loadBalancerID", " ")]
    [InlineData("loadBalancerID", "　")]
    public async Task DeleteAsync_WithEmptyLoadBalancerID_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid },
            { pathParameterKey, pathParameterValue }
        };
        var builder = new WithLoadBalancerItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.DeleteAsync();
        });

        Assert.Equal("ロードバランサIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("ロードバランサIDを入力してください。");
    }

    /// <summary>
    /// DeleteAsyncのパスパラメータが有効なGUID形式の値でない場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("test-value")]
    [InlineData("1234-567890ab-cdef-1234-567890abcdef")]
    [InlineData("12345678-90ab-cdef-1234-567890ghijkl")]
    public async Task DeleteAsync_WithInvalidFormatLoadBalancerID_ThrowsValidationException(string invalidPathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid },
            { "loadBalancerID", invalidPathParameterValue }
        };
        var builder = new WithLoadBalancerItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.DeleteAsync();
        });

        Assert.Equal("ロードバランサIDはGUID形式で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("ロードバランサIDはGUID形式で入力してください。");
    }

    /// <summary>
    /// DeleteAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_WithApiError_ThrowsApiException()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid },
            { "loadBalancerID", _validGuid }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WithLoadBalancerItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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
            await builder.DeleteAsync();
        });

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^ロードバランサ削除のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// DeleteAsyncでその他のエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_WithOtherError_ThrowsException()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid },
            { "loadBalancerID", _validGuid }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WithLoadBalancerItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<Exception>(async () =>
        {
            await builder.DeleteAsync();
        });

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^ロードバランサの削除中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// DeleteAsyncが正しく呼び出されることを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_CallsRequestAdapter()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid },
            { "loadBalancerID", _validGuid }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WithLoadBalancerItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await builder.DeleteAsync();

        await requestAdapter.Received(1).SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^ロードバランサの削除を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^ロードバランサの削除が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 204\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// WithUrlが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewInstance()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid },
            { "loadBalancerID", _validGuid }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WithLoadBalancerItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var rawUrl = "https://example.com/clusters/00000000-0000-0000-0000-000000000000/asg/00000000-0000-0000-0000-000000000000/load_balancers/00000000-0000-0000-0000-000000000000";

        var newBuilder = builder.WithUrl(rawUrl);

        Assert.NotNull(newBuilder);
        Assert.IsType<WithLoadBalancerItemRequestBuilder>(newBuilder);
    }
}
