using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.AppRunDedicated.Clusters.Item.Asg.Item.Load_balancers;
using Sacloud.Sdk.AppRunDedicated.Models;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.AppRunDedicated.Tests.Clusters.Item.Asg.Item.Load_balancers;

/// <summary>
/// Load_balancersRequestBuilderのテスト
/// </summary>
public class Load_balancersRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<Load_balancersRequestBuilder> _mockLogger;
    private readonly Guid _validGuid;

    public Load_balancersRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<Load_balancersRequestBuilder>>();
        _mockLoggerFactory.CreateLogger(Arg.Any<string>()).Returns(_mockLogger);
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
            { "autoScalingGroupID", _validGuid }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();

        var builder = new Load_balancersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（rawUrl）が正常に動作することを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_SetsProperties()
    {
        var rawUrl = "https://example.com/clusters/00000000-0000-0000-0000-000000000000/asg/00000000-0000-0000-0000-000000000000/load_balancers";
        var requestAdapter = Substitute.For<IRequestAdapter>();

        var builder = new Load_balancersRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// インデクサー（Guid）が正しいWithLoadBalancerItemRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_WithGuid_ReturnsItemRequestBuilder()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new Load_balancersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var loadBalancerID = Guid.NewGuid();

        var itemBuilder = builder[loadBalancerID];

        Assert.NotNull(itemBuilder);
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
            { "autoScalingGroupID", _validGuid }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new Load_balancersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.GET, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToPostRequestInformationが正しいRequestInformationを返すことを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_WithBody_ReturnsRequestInformation()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new Load_balancersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateLoadBalancerRequest
        {
            Name = "test-lb",
            ServiceClassPath = "/service/load_balancer/standard"
        };

        var requestInfo = builder.ToPostRequestInformation(body);

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.POST, requestInfo.HttpMethod);
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
            { "baseurl", "https://api.example.com" },
            { pathParameterKey, pathParameterValue },
            { "autoScalingGroupID", _validGuid }
        };
        var builder = new Load_balancersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        
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
            { "baseurl", "https://api.example.com" },
            { "clusterID", invalidPathParameterValue },
            { "autoScalingGroupID", _validGuid }
        };
        var builder = new Load_balancersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        
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
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validGuid },
            { pathParameterKey, pathParameterValue }
        };
        var builder = new Load_balancersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        
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
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validGuid },
            { "autoScalingGroupID", invalidPathParameterValue }
        };
        var builder = new Load_balancersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        
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
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid }
        };
        var builder = new Load_balancersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        
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
    [InlineData(31)]
    public async Task GetAsync_WithInvalidRangeMaxItems_ThrowsValidationException(int invalidMaxItemsValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid }
        };
        var builder = new Load_balancersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync(config =>
            {
                config.QueryParameters.MaxItems = invalidMaxItemsValue;
            });
        });

        Assert.Equal("MaxItemsの値は2以上30以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("MaxItemsの値は2以上30以下で入力してください。");
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
            { "autoScalingGroupID", _validGuid }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new Load_balancersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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

        requestAdapter.SendAsync<ListLoadBalancersResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListLoadBalancersResponse>>(),
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
                @"^ロードバランサの一覧取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
            { "autoScalingGroupID", _validGuid }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new Load_balancersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync<ListLoadBalancersResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListLoadBalancersResponse>>(),
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
                @"^ロードバランサの一覧の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
            { "autoScalingGroupID", _validGuid }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new Load_balancersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new ListLoadBalancersResponse();

        requestAdapter.SendAsync<ListLoadBalancersResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListLoadBalancersResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.GetAsync();

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListLoadBalancersResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());

        Assert.Equal(expectedResponse, result);
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^ロードバランサの一覧の取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^ロードバランサの一覧の取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncのパスパラメータにclusterIDがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("clusterID", "")]
    [InlineData("clusterID", " ")]
    [InlineData("clusterID", "　")]
    public async Task PostAsync_WithEmptyClusterID_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { pathParameterKey, pathParameterValue },
            { "autoScalingGroupID", _validGuid }
        };
        var builder = new Load_balancersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateLoadBalancerRequest
        {
            Name = "test-lb",
            ServiceClassPath = "cloud/apprun/dedicated/lb/1vcpu_2gb_1",
            Interfaces = new List<LoadBalancerInterface>
            {
                new LoadBalancerInterface
                {
                    InterfaceIndex = 0,
                    Upstream = "shared"
                }
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        Assert.Equal("クラスタIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("クラスタIDを入力してください。");
    }

    /// <summary>
    /// PostAsyncのパスパラメータが有効なGUID形式の値でない場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("test-value")]
    [InlineData("1234-567890ab-cdef-1234-567890abcdef")]
    [InlineData("12345678-90ab-cdef-1234-567890ghijkl")]
    public async Task PostAsync_WithInvalidFormatClusterID_ThrowsValidationException(string invalidPathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", invalidPathParameterValue },
            { "autoScalingGroupID", _validGuid }
        };
        var builder = new Load_balancersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateLoadBalancerRequest
        {
            Name = "test-lb",
            ServiceClassPath = "cloud/apprun/dedicated/lb/1vcpu_2gb_1",
            Interfaces = new List<LoadBalancerInterface>
            {
                new LoadBalancerInterface
                {
                    InterfaceIndex = 0,
                    Upstream = "shared"
                }
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        Assert.Equal("クラスタIDはGUID形式で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("クラスタIDはGUID形式で入力してください。");
    }

    /// <summary>
    /// PostAsyncのパスパラメータにautoScalingGroupIDがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("autoScalingGroupID", "")]
    [InlineData("autoScalingGroupID", " ")]
    [InlineData("autoScalingGroupID", "　")]
    public async Task PostAsync_WithEmptyAutoScalingGroupID_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validGuid },
            { pathParameterKey, pathParameterValue }
        };
        var builder = new Load_balancersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateLoadBalancerRequest
        {
            Name = "test-lb",
            ServiceClassPath = "cloud/apprun/dedicated/lb/1vcpu_2gb_1",
            Interfaces = new List<LoadBalancerInterface>
            {
                new LoadBalancerInterface
                {
                    InterfaceIndex = 0,
                    Upstream = "shared"
                }
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        Assert.Equal("オートスケーリンググループIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("オートスケーリンググループIDを入力してください。");
    }

    /// <summary>
    /// PostAsyncのパスパラメータが有効なGUID形式の値でない場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("test-value")]
    [InlineData("1234-567890ab-cdef-1234-567890abcdef")]
    [InlineData("12345678-90ab-cdef-1234-567890ghijkl")]
    public async Task PostAsync_WithInvalidFormatAutoScalingGroupID_ThrowsValidationException(string invalidPathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validGuid },
            { "autoScalingGroupID", invalidPathParameterValue }
        };
        var builder = new Load_balancersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateLoadBalancerRequest
        {
            Name = "test-lb",
            ServiceClassPath = "cloud/apprun/dedicated/lb/1vcpu_2gb_1",
            Interfaces = new List<LoadBalancerInterface>
            {
                new LoadBalancerInterface
                {
                    InterfaceIndex = 0,
                    Upstream = "shared"
                }
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        Assert.Equal("オートスケーリンググループIDはGUID形式で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("オートスケーリンググループIDはGUID形式で入力してください。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithNullBody_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid }
        };
        var builder = new Load_balancersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(null);
        });

        Assert.Equal("リクエストボディは必須です。", exception.Message);
        _mockLogger.Received(1).LogWarning("リクエストボディは必須です。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのパラメータがnullの場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("Name")]
    [InlineData("ServiceClassPath")]
    [InlineData("Interfaces")]
    [InlineData("Interfaces.InterfaceIndex")]
    [InlineData("Interfaces.Upstream")]
    [InlineData("Interfaces.IpPool.Start")]
    [InlineData("Interfaces.IpPool.End")]
    public async Task PostAsync_WithNullRequestBodyParameter_ThrowsValidationException(string invalidParameterName)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid }
        };
        var builder = new Load_balancersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateLoadBalancerRequest
        {
            Name = invalidParameterName == "Name" ? null : "test-lb",
            ServiceClassPath = invalidParameterName == "ServiceClassPath" ? null : "cloud/apprun/dedicated/lb/1vcpu_2gb_1",
            Interfaces = invalidParameterName == "Interfaces" ? null : new List<LoadBalancerInterface>
            {
                new LoadBalancerInterface
                {
                    InterfaceIndex = invalidParameterName == "Interfaces.InterfaceIndex" ? null : 0,
                    Upstream = invalidParameterName == "Interfaces.Upstream" ? null : "shared",
                    IpPool = new List<IpRange>
                    {
                        new IpRange
                        {
                            Start = invalidParameterName == "Interfaces.IpPool.Start" ? null : "127.0.0.1",
                            End = invalidParameterName == "Interfaces.IpPool.End" ? null : "127.0.0.1"
                        }
                    }
                }
            }
        };
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });
        Assert.Equal($"{invalidParameterName}が入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}が入力されていません。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのパラメータの値が有効な最大文字数を超過した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("Name", 20)]
    [InlineData("ServiceClassPath", 255)]
    public async Task PostAsync_WithInvalidMaxLengthRequestBodyParameter_ThrowsValidationException(string invalidParameterName, int validMaxLength)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid }
        };
        var builder = new Load_balancersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateLoadBalancerRequest
        {
            Name = invalidParameterName == "Name" ? "123456789012345678901" : "test-lb",
            ServiceClassPath = invalidParameterName == "ServiceClassPath" ? new string('a', 256) : "cloud/apprun/dedicated/lb/1vcpu_2gb_1",
            Interfaces = new List<LoadBalancerInterface>
            {
                new LoadBalancerInterface
                {
                    InterfaceIndex = 0,
                    Upstream = "shared"
                }
            }
        };
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });
        Assert.Equal($"{invalidParameterName}は1文字以上{validMaxLength}文字以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}は1文字以上{validMaxLength}文字以下で入力してください。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのパラメータが有効な数値範囲でない値の場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(7)]
    [InlineData(30)]
    public async Task PostAsync_WithInvalidRangeRequestBodyParameter_ThrowsValidationException(int invalidParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid }
        };
        var builder = new Load_balancersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateLoadBalancerRequest
        {
            Name = "test-asg",
            ServiceClassPath = "cloud/apprun/dedicated/lb/1vcpu_2gb_1",
            Interfaces = new List<LoadBalancerInterface>
            {
                new LoadBalancerInterface
                {
                    InterfaceIndex = 0,
                    Upstream = "switch",
                    NetmaskLen = invalidParameterValue
                }
            }
        };
        var expectedResponse = new CreateApplicationVersionResponse();
        requestAdapter.SendAsync<CreateApplicationVersionResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreateApplicationVersionResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        Assert.Equal("Interfaces.NetmaskLenの値は8以上29以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("Interfaces.NetmaskLenの値は8以上29以下で入力してください。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディパラメータの文字列が正規表現パターンに一致しなかった場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("Name", "invalid+name")]
    [InlineData("Name", "無効なName")]
    [InlineData("Interfaces.Vip", "localhost")]
    [InlineData("Interfaces.Vip", "127.0.1")]
    [InlineData("Interfaces.Vip", "127.0.0.0.1")]
    [InlineData("Interfaces.Vip", "2001:0db8:0000:0000:0000:0000:0000:0001")]
    [InlineData("Interfaces.Vip", "::1")]
    [InlineData("Interfaces.IpPool.Start", "localhost")]
    [InlineData("Interfaces.IpPool.Start", "127.0.1")]
    [InlineData("Interfaces.IpPool.Start", "127.0.0.0.1")]
    [InlineData("Interfaces.IpPool.Start", "2001:0db8:0000:0000:0000:0000:0000:0001")]
    [InlineData("Interfaces.IpPool.Start", "::1")]
    [InlineData("Interfaces.IpPool.End", "localhost")]
    [InlineData("Interfaces.IpPool.End", "127.0.1")]
    [InlineData("Interfaces.IpPool.End", "127.0.0.0.1")]
    [InlineData("Interfaces.IpPool.End", "2001:0db8:0000:0000:0000:0000:0000:0001")]
    [InlineData("Interfaces.IpPool.End", "::1")]
    public async Task PostAsync_WithInvalidRegexPatternRequestBodyParameter_ThrowsValidationException(string invalidParameterName, string invalidParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid }
        };
        var builder = new Load_balancersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateLoadBalancerRequest
        {
            Name = invalidParameterName == "Name" ? invalidParameterValue : "test-lb",
            ServiceClassPath = "cloud/apprun/dedicated/lb/1vcpu_2gb_1",
            Interfaces = new List<LoadBalancerInterface>
            {
                new LoadBalancerInterface
                {
                    InterfaceIndex = 0,
                    Upstream = "shared",
                    IpPool = new List<IpRange>
                    {
                        new IpRange
                        {
                            Start = invalidParameterName == "Interfaces.IpPool.Start" ? invalidParameterValue : "172.0.0.1",
                            End = invalidParameterName == "Interfaces.IpPool.End" ? invalidParameterValue : "127.0.0.1"
                        }
                    },
                    Vip = invalidParameterName == "Interfaces.Vip" ? invalidParameterValue : "127.0.0.1"
                }
            }
        };
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });
        Assert.Equal($"{invalidParameterName}の形式が正しくありません。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}の形式が正しくありません。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディパラメータの配列要素の文字列が正規表現パターンに一致しなかった場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("localhost")]
    [InlineData("127.0.1")]
    [InlineData("127.0.0.0.1")]
    [InlineData("2001:0db8:0000:0000:0000:0000:0000:0001")]
    [InlineData("::1")]
    public async Task PostAsync_WithInvalidArrayElementRegexPatternRequestBodyParameter_ThrowsValidationException(string invalidParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid }
        };
        var builder = new Load_balancersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateLoadBalancerRequest
        {
            Name = "test-lb",
            ServiceClassPath = "cloud/apprun/dedicated/lb/1vcpu_2gb_1",
            Interfaces = new List<LoadBalancerInterface>
            {
                new LoadBalancerInterface
                {
                    InterfaceIndex = 0,
                    Upstream = "shared",
                    IpPool = new List<IpRange>
                    {
                        new IpRange
                        {
                            Start = "172.0.0.1",
                            End = "127.0.0.1"
                        }
                    },
                    Vip = "127.0.0.1"
                }
            },
            NameServers = new List<string> { invalidParameterValue }
        };
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });
        Assert.Equal("NameServersの要素の形式が正しくありません。", exception.Message);
        _mockLogger.Received(1).LogWarning("NameServersの要素の形式が正しくありません。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのパラメータが有効な要素数でない値の場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("NameServers", 3)]
    [InlineData("Interfaces", 5)]
    [InlineData("Interfaces.IpPool", 20)]
    public async Task PostAsync_WithInvalidElementCountRequestBodyParameter_ThrowsValidationException(string invalidParameterName, int validMaxElementsCount)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid }
        };
        var builder = new Load_balancersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateLoadBalancerRequest
        {
            Name = "test-name",
            ServiceClassPath = "cloud/apprun/dedicated/lb/1vcpu_2gb_1",
            NameServers = invalidParameterName == "NameServers"
                ? Enumerable.Range(0, 4).Select(_ => "127.0.0.1").ToList()
                : new List<string> { "127.0.0.1" },
            Interfaces = invalidParameterName == "Interfaces"
                ? Enumerable.Range(0, 6).Select(_ => new LoadBalancerInterface { InterfaceIndex = 0, Upstream = "shared" }).ToList()
                : new List<LoadBalancerInterface>
                {
                    new LoadBalancerInterface
                    {
                        InterfaceIndex = 0,
                        Upstream = "switch",
                        IpPool = invalidParameterName == "Interfaces.IpPool"
                            ? Enumerable.Range(0, 21).Select(_ => new IpRange { Start = "127.0.0.1", End = "127.0.0.1" }).ToList()
                            : new List<IpRange>
                            {
                                new IpRange
                                {
                                    Start = "127.0.0.1",
                                    End = "127.0.0.1"
                                }
                            }
                    }
                }
        };
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });
        Assert.Equal($"{invalidParameterName}の要素数は{validMaxElementsCount}以下にしてください。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}の要素数は{validMaxElementsCount}以下にしてください。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのパラメータの値が空配列の場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("NameServers")]
    [InlineData("Interfaces")]
    public async Task PostAsync_WithEmptyArrayBodyParameter_ThrowsValidationException(string invalidParameterName)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid }
        };
        var builder = new Load_balancersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateLoadBalancerRequest
        {
            Name = "test-lb",
            ServiceClassPath = "cloud/apprun/dedicated/lb/1vcpu_2gb_1",
            NameServers = invalidParameterName == "NameServers" ? new List<string>() : new List<string> { "127.0.0.1" },
            Interfaces = invalidParameterName == "Interfaces" ? new List<LoadBalancerInterface>() : new List<LoadBalancerInterface>
            {
                new LoadBalancerInterface
                {
                    InterfaceIndex = 0,
                    Upstream = "shared"
                }
            }
        };
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });
        Assert.Equal($"{invalidParameterName}が入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}が入力されていません。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディの複数パラメータが無効な値の場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithMultipleInvalidRequestBodyParameters_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid }
        };
        var builder = new Load_balancersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateLoadBalancerRequest
        {
            Name = "123456789012345678901",
            ServiceClassPath = null,
            Interfaces = new List<LoadBalancerInterface>()
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        var expectedMessage = """
            Interfacesが入力されていません。
            Nameは1文字以上20文字以下で入力してください。
            ServiceClassPathが入力されていません。
            """;
        Assert.Equal(expectedMessage, exception.Message);
        _mockLogger.Received(1).LogWarning("Interfacesが入力されていません。");
        _mockLogger.Received(1).LogWarning("Nameは1文字以上20文字以下で入力してください。");
        _mockLogger.Received(1).LogWarning("ServiceClassPathが入力されていません。");
    }

    /// <summary>
    /// PostAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithApiError_ThrowsApiException()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new Load_balancersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateLoadBalancerRequest
        {
            Name = "test-lb",
            ServiceClassPath = "cloud/apprun/dedicated/lb/1vcpu_2gb_1",
            Interfaces = new List<LoadBalancerInterface>
            {
                new LoadBalancerInterface
                {
                    InterfaceIndex = 0,
                    Upstream = "shared"
                }
            }
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

        requestAdapter.SendAsync<CreateLoadBalancerResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreateLoadBalancerResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(async () =>
        {
            await builder.PostAsync(body);
        });

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^ロードバランサ作成のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncでその他のエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithOtherError_ThrowsException()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new Load_balancersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateLoadBalancerRequest
        {
            Name = "test-lb",
            ServiceClassPath = "cloud/apprun/dedicated/lb/1vcpu_2gb_1",
            Interfaces = new List<LoadBalancerInterface>
            {
                new LoadBalancerInterface
                {
                    InterfaceIndex = 0,
                    Upstream = "shared"
                }
            }
        };
        var expectedException = new Exception();

        requestAdapter.SendAsync<CreateLoadBalancerResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreateLoadBalancerResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<Exception>(async () =>
        {
            await builder.PostAsync(body);
        });

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^ロードバランサの作成中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncが正しく呼び出されることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithBody_CallsRequestAdapter()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "clusterID", _validGuid },
            { "autoScalingGroupID", _validGuid }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new Load_balancersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateLoadBalancerRequest
        {
            Name = "test-lb",
            ServiceClassPath = "cloud/apprun/dedicated/lb/1vcpu_2gb_1",
            Interfaces = new List<LoadBalancerInterface>
            {
                new LoadBalancerInterface
                {
                    InterfaceIndex = 0,
                    Upstream = "shared"
                }
            }
        };
        var expectedResponse = new CreateLoadBalancerResponse();

        requestAdapter.SendAsync<CreateLoadBalancerResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreateLoadBalancerResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.PostAsync(body);

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreateLoadBalancerResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());

        Assert.Equal(expectedResponse, result);

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^ロードバランサの作成を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^ロードバランサの作成が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
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
            { "autoScalingGroupID", _validGuid }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new Load_balancersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var rawUrl = "https://example.com/clusters/00000000-0000-0000-0000-000000000000/asg/00000000-0000-0000-0000-000000000000/load_balancers";

        var newBuilder = builder.WithUrl(rawUrl);

        Assert.NotNull(newBuilder);
        Assert.IsType<Load_balancersRequestBuilder>(newBuilder);
    }

    /// <summary>
    /// QueryParameters.Cursorが設定できることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Cursor_CanBeSet()
    {
        var queryParams = new Load_balancersRequestBuilder.Load_balancersRequestBuilderGetQueryParameters();
        var cursor = Guid.NewGuid();

        queryParams.Cursor = cursor;

        Assert.Equal(cursor, queryParams.Cursor);
    }

    /// <summary>
    /// QueryParameters.MaxItemsが設定できることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_MaxItems_CanBeSet()
    {
        var queryParams = new Load_balancersRequestBuilder.Load_balancersRequestBuilderGetQueryParameters();
        long maxItems = 50;

        queryParams.MaxItems = maxItems;

        Assert.Equal(maxItems, queryParams.MaxItems);
    }
}
