using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.AppRunDedicated.Clusters.Item.Asg;
using Sacloud.Sdk.AppRunDedicated.Clusters.Item.Asg.Item;
using Sacloud.Sdk.AppRunDedicated.Models;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.AppRunDedicated.Tests.Clusters.Item.Asg;

/// <summary>
/// AsgRequestBuilderの単体テスト
/// </summary>
public class AsgRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<AsgRequestBuilder> _mockLogger;
    private readonly Guid _validClusterID;

    public AsgRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<AsgRequestBuilder>>();
        _mockLoggerFactory.CreateLogger(Arg.Any<string>()).Returns(_mockLogger);
        _validClusterID = Guid.NewGuid();
    }

    /// <summary>
    /// PathParametersを使用したコンストラクタが正しく動作することを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_SetsProperties()
    {
        var pathParameters = new Dictionary<string, object>();
        var requestAdapter = Substitute.For<IRequestAdapter>();

        var builder = new AsgRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// rawUrlを使用したコンストラクタが正しく動作することを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_SetsProperties()
    {
        var rawUrl = "https://example.com/clusters/123/asg";
        var requestAdapter = Substitute.For<IRequestAdapter>();

        var builder = new AsgRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// Guidインデクサーが正しくWithAutoScalingGroupItemRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_WithGuid_ReturnsWithAutoScalingGroupItemRequestBuilder()
    {
        var pathParameters = new Dictionary<string, object>();
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new AsgRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var autoScalingGroupId = Guid.NewGuid();

        var itemBuilder = builder[autoScalingGroupId];

        Assert.NotNull(itemBuilder);
        Assert.IsType<WithAutoScalingGroupItemRequestBuilder>(itemBuilder);
    }

    /// <summary>
    /// ToGetRequestInformationが正しいRequestInformationを生成することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_ReturnsValidRequestInformation()
    {
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://example.com" }, { "clusterID", _validClusterID } };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new AsgRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.GET, requestInfo.HttpMethod);
        Assert.Contains("asg", requestInfo.UrlTemplate);
    }

    /// <summary>
    /// ToGetRequestInformationがQueryParametersを正しく設定することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_WithQueryParameters_SetsQueryParameters()
    {
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://example.com" }, { "clusterID", _validClusterID } };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new AsgRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var cursor = Guid.NewGuid();
        var lastAutoScalingGroupId = Guid.NewGuid();

        var requestInfo = builder.ToGetRequestInformation(config =>
        {
            config.QueryParameters.Cursor = cursor;
            config.QueryParameters.LastAutoScalingGroupID = lastAutoScalingGroupId;
            config.QueryParameters.MaxItems = 10;
        });

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.GET, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToPostRequestInformationが正しいRequestInformationを生成することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_ReturnsValidRequestInformation()
    {
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://example.com" }, { "clusterID", _validClusterID } };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new AsgRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var createAutoScalingGroup = new CreateAutoScalingGroupRequest
        {
            Name = "test-asg",
            MinNodes = 1,
            MaxNodes = 5,
            Zone = "is1a"
        };

        var requestInfo = builder.ToPostRequestInformation(createAutoScalingGroup);

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.POST, requestInfo.HttpMethod);
        Assert.Contains("asg", requestInfo.UrlTemplate);
    }

    /// <summary>
    /// GetAsyncのパスパラメータにclusterIDがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("clusterID", "")]
    [InlineData("clusterID", " ")]
    [InlineData("clusterID", "　")]
    public async Task GetAsync_WithNullOrEmptyPathParameters_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { pathParameterKey, pathParameterValue }
        };
        var builder = new AsgRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new ListAutoScalingGroupsResponse();

        requestAdapter.SendAsync<ListAutoScalingGroupsResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListAutoScalingGroupsResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

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
    public async Task GetAsync_WithInvalidFormatPathParameters_ThrowsValidationException(string invalidPathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", invalidPathParameterValue }
        };
        var builder = new AsgRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new ListAutoScalingGroupsResponse();

        requestAdapter.SendAsync<ListAutoScalingGroupsResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListAutoScalingGroupsResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync();
        });

        Assert.Equal("クラスタIDはGUID形式で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("クラスタIDはGUID形式で入力してください。");
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
            { "clusterID", _validClusterID }
        };
        var builder = new AsgRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
    [InlineData(0)]
    [InlineData(31)]
    public async Task GetAsync_WithInvalidRangeMaxItems_ThrowsValidationException(int invalidMaxItemsValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validClusterID }
        };
        var builder = new AsgRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync(config =>
            {
                config.QueryParameters.MaxItems = invalidMaxItemsValue;
            });
        });

        Assert.Equal("MaxItemsの値は1以上30以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("MaxItemsの値は1以上30以下で入力してください。");
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
            { "clusterID", _validClusterID }
        };
        var builder = new AsgRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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

        requestAdapter.SendAsync<ListAutoScalingGroupsResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListAutoScalingGroupsResponse>>(),
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
                @"^Auto Scaling Groupの一覧取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
            { "clusterID", _validClusterID }
        };
        var builder = new AsgRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync<ListAutoScalingGroupsResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListAutoScalingGroupsResponse>>(),
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
                @"^Auto Scaling Groupの一覧の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncがListAutoScalingGroupsResponseを返すことを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_ReturnsListAutoScalingGroupResponse()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://example.com" },
            { "clusterID", _validClusterID }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var expectedResponse = new ListAutoScalingGroupsResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListAutoScalingGroupsResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);
        var builder = new AsgRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var response = await builder.GetAsync();

        Assert.NotNull(response);
        Assert.IsType<ListAutoScalingGroupsResponse>(response);

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^Auto Scaling Groupの一覧の取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^Auto Scaling Groupの一覧の取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
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
    public async Task PostAsync_WithNullOrEmptyPathParameters_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { pathParameterKey, pathParameterValue }
        };
        var builder = new AsgRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateAutoScalingGroupRequest
        {
            Name = "test-asg",
            Zone = "is1a",
            WorkerServiceClassPath = "cloud/apprun/dedicated/worker/1vcpu_2gb",
            MinNodes = 1,
            MaxNodes = 5,
            Interfaces = new List<AutoScalingGroupNodeInterface>
            {
                new AutoScalingGroupNodeInterface
                {
                    InterfaceIndex = 0,
                    Upstream = "shared",
                    ConnectsToLB = false
                }
            }
        };
        var expectedResponse = new CreateAutoScalingGroupResponse();

        requestAdapter.SendAsync<CreateAutoScalingGroupResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreateAutoScalingGroupResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

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
    public async Task PostAsync_WithInvalidFormatPathParameters_ThrowsValidationException(string invalidPathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", invalidPathParameterValue }
        };
        var builder = new AsgRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateAutoScalingGroupRequest
        {
            Name = "test-asg",
            Zone = "is1a",
            WorkerServiceClassPath = "cloud/apprun/dedicated/worker/1vcpu_2gb",
            MinNodes = 1,
            MaxNodes = 5,
            Interfaces = new List<AutoScalingGroupNodeInterface>
            {
                new AutoScalingGroupNodeInterface
                {
                    InterfaceIndex = 0,
                    Upstream = "shared",
                    ConnectsToLB = false
                }
            }
        };
        var expectedResponse = new ListAutoScalingGroupsResponse();

        requestAdapter.SendAsync<ListAutoScalingGroupsResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListAutoScalingGroupsResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        Assert.Equal("クラスタIDはGUID形式で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("クラスタIDはGUID形式で入力してください。");
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
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validClusterID }
        };
        var builder = new AsgRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
    [InlineData("Zone")]
    [InlineData("WorkerServiceClassPath")]
    [InlineData("MinNodes")]
    [InlineData("MaxNodes")]
    [InlineData("Interfaces")]
    [InlineData("Interfaces.InterfaceIndex")]
    [InlineData("Interfaces.Upstream")]
    [InlineData("Interfaces.ConnectsToLB")]
    [InlineData("Interfaces.IpPool.Start")]
    [InlineData("Interfaces.IpPool.End")]
    public async Task PostAsync_WithNullRequestBodyParameter_ThrowsValidationException(string invalidParameterName)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validClusterID }
        };
        var builder = new AsgRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateAutoScalingGroupRequest
        {
            Name = invalidParameterName == "Name" ? null : "test-name",
            Zone = invalidParameterName == "Zone" ? null : "test-zone",
            WorkerServiceClassPath = invalidParameterName == "WorkerServiceClassPath" ? null : "cloud/apprun/dedicated/worker/1vcpu_2gb",
            MinNodes = invalidParameterName == "MinNodes" ? null : 1,
            MaxNodes = invalidParameterName == "MaxNodes" ? null : 1,
            Interfaces = invalidParameterName == "Interfaces" ? null : new List<AutoScalingGroupNodeInterface>
            {
                new AutoScalingGroupNodeInterface
                {
                    InterfaceIndex = invalidParameterName == "Interfaces.InterfaceIndex" ? null : 0,
                    Upstream = invalidParameterName == "Interfaces.Upstream" ? null : "switch",
                    IpPool = new List<IpRange>
                    {
                        new IpRange
                        {
                            Start = invalidParameterName == "Interfaces.IpPool.Start" ? null : "127.0.0.1",
                            End = invalidParameterName == "Interfaces.IpPool.End" ? null : "127.0.0.1"
                        }
                    },
                    ConnectsToLB = invalidParameterName == "Interfaces.ConnectsToLB" ? null : false
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
    [InlineData("WorkerServiceClassPath" ,255)]
    public async Task PostAsync_WithInvalidMaxLengthRequestBodyParameter_ThrowsValidationException(string invalidParameterName, int validMaxLength)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validClusterID }
        };
        var builder = new AsgRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateAutoScalingGroupRequest
        {
            Name = invalidParameterName == "Name" ? "123456789012345678901" : "test-name",
            Zone = "is1a",
            WorkerServiceClassPath = invalidParameterName == "WorkerServiceClassPath" ? new string('a', 256) : "cloud/apprun/dedicated/worker/1vcpu_2gb",
            MinNodes = 1,
            MaxNodes = 1,
            Interfaces = new List<AutoScalingGroupNodeInterface>
            {
                new AutoScalingGroupNodeInterface
                {
                    InterfaceIndex = 0,
                    Upstream = "shared",
                    ConnectsToLB = false
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
    [InlineData("MinNodes", 0, 1, 10)]
    [InlineData("MinNodes", 11, 1, 10)]
    [InlineData("MaxNodes", 0, 1, 10)]
    [InlineData("MaxNodes", 11, 1, 10)]
    [InlineData("Interfaces.NetmaskLen", 7, 8, 29)]
    [InlineData("Interfaces.NetmaskLen", 30, 8, 29)]
    public async Task PostAsync_WithInvalidRangeRequestBodyParameter_ThrowsValidationException(string invalidParameterName, int invalidParameterValue, int validMinimumValue, int validMaximumValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validClusterID }
        };
        var builder = new AsgRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateAutoScalingGroupRequest
        {
            Name = "test-asg",
            Zone = "is1a",
            WorkerServiceClassPath = "cloud/apprun/dedicated/worker/1vcpu_2gb",
            MinNodes = invalidParameterName == "MinNodes" ? invalidParameterValue : 1,
            MaxNodes = invalidParameterName == "MaxNodes" ? invalidParameterValue : 5,
            Interfaces = new List<AutoScalingGroupNodeInterface>
            {
                new AutoScalingGroupNodeInterface
                {
                    InterfaceIndex = 0,
                    Upstream = "switch",
                    NetmaskLen = invalidParameterName == "Interfaces.NetmaskLen" ? invalidParameterValue : 24,
                    ConnectsToLB = false
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

        Assert.Equal($"{invalidParameterName}の値は{validMinimumValue}以上{validMaximumValue}以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}の値は{validMinimumValue}以上{validMaximumValue}以下で入力してください。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディパラメータの文字数が正規表現パターンに一致しなかった場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("Name", "invalid+name")]
    [InlineData("Name", "無効なName")]
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
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validClusterID }
        };
        var builder = new AsgRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateAutoScalingGroupRequest
        {
            Name = invalidParameterName == "Name" ? invalidParameterValue : "test-name",
            Zone = "is1a",
            WorkerServiceClassPath = "cloud/apprun/dedicated/worker/1vcpu_2gb",
            MinNodes = 1,
            MaxNodes = 1,
            Interfaces = new List<AutoScalingGroupNodeInterface>
            {
                new AutoScalingGroupNodeInterface
                {
                    InterfaceIndex = 0,
                    Upstream = "switch",
                    IpPool = new List<IpRange>
                    {
                        new IpRange
                        {
                            Start = invalidParameterName == "Interfaces.IpPool.Start" ? invalidParameterValue : "127.0.0.1",
                            End = invalidParameterName == "Interfaces.IpPool.End" ? invalidParameterValue : "127.0.0.1"
                        }
                    },
                    ConnectsToLB = false
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
    /// PostAsyncのリクエストボディパラメータの配列要素の文字数が正規表現パターンに一致しなかった場合に例外がスローされることを確認
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
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validClusterID }
        };
        var builder = new AsgRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateAutoScalingGroupRequest
        {
            Name = "test-name",
            Zone = "is1a",
            WorkerServiceClassPath = "cloud/apprun/dedicated/worker/1vcpu_2gb",
            MinNodes = 1,
            MaxNodes = 1,
            Interfaces = new List<AutoScalingGroupNodeInterface>
            {
                new AutoScalingGroupNodeInterface
                {
                    InterfaceIndex = 0,
                    Upstream = "switch",
                    IpPool = new List<IpRange>
                    {
                        new IpRange
                        {
                            Start = "127.0.0.1",
                            End = "127.0.0.1"
                        }
                    },
                    ConnectsToLB = false
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
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validClusterID }
        };
        var builder = new AsgRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateAutoScalingGroupRequest
        {
            Name = "test-name",
            Zone = "is1a",
            NameServers = invalidParameterName == "NameServers"
                ? Enumerable.Range(0, 4).Select(_ => "127.0.0.1").ToList()
                : new List<string> { "127.0.0.1" },
            WorkerServiceClassPath = "cloud/apprun/dedicated/worker/1vcpu_2gb",
            MinNodes = 1,
            MaxNodes = 1,
            Interfaces = invalidParameterName == "Interfaces"
                ? Enumerable.Range(0, 6).Select(_ => new AutoScalingGroupNodeInterface { InterfaceIndex = 0, Upstream = "shared", ConnectsToLB = false }).ToList()
                : new List<AutoScalingGroupNodeInterface>
                {
                    new AutoScalingGroupNodeInterface
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
                            },
                        ConnectsToLB = false }
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
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validClusterID }
        };
        var builder = new AsgRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateAutoScalingGroupRequest
        {
            Name = "test-name",
            Zone = "is1a",
            NameServers = invalidParameterName == "NameServers" ? new List<string>() : new List<string> { "127.0.0.1" },
            WorkerServiceClassPath = "cloud/apprun/dedicated/worker/1vcpu_2gb",
            MinNodes = 1,
            MaxNodes = 1,
            Interfaces = invalidParameterName == "Interfaces" ? new List<AutoScalingGroupNodeInterface>() : new List<AutoScalingGroupNodeInterface>
            {
                new AutoScalingGroupNodeInterface
                {
                    InterfaceIndex = 0,
                    Upstream = "shared",
                    ConnectsToLB = false
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
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validClusterID }
        };
        var builder = new AsgRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateAutoScalingGroupRequest
        {
            Name = "123456789012345678901",
            Zone = "is1a",
            NameServers = new List<string>(),
            WorkerServiceClassPath = null,
            MinNodes = 0,
            MaxNodes = 11,
            Interfaces = new List<AutoScalingGroupNodeInterface>
            {
                new AutoScalingGroupNodeInterface
                {
                    InterfaceIndex = 0,
                    Upstream = "switch",
                    IpPool = new List<IpRange>
                    {
                        new IpRange
                        {
                            Start = "invalid-ip",
                            End = null,
                        }
                    },
                    ConnectsToLB = false
                }
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        var expectedMessage = """
            MaxNodesの値は1以上10以下で入力してください。
            MinNodesの値は1以上10以下で入力してください。
            Nameは1文字以上20文字以下で入力してください。
            NameServersが入力されていません。
            WorkerServiceClassPathが入力されていません。
            Interfaces.IpPool.Endが入力されていません。
            Interfaces.IpPool.Startの形式が正しくありません。
            """;
        Assert.Equal(expectedMessage, exception.Message);
        _mockLogger.Received(1).LogWarning("MaxNodesの値は1以上10以下で入力してください。");
        _mockLogger.Received(1).LogWarning("MinNodesの値は1以上10以下で入力してください。");
        _mockLogger.Received(1).LogWarning("Nameは1文字以上20文字以下で入力してください。");
        _mockLogger.Received(1).LogWarning("NameServersが入力されていません。");
        _mockLogger.Received(1).LogWarning("WorkerServiceClassPathが入力されていません。");
        _mockLogger.Received(1).LogWarning("Interfaces.IpPool.Endが入力されていません。");
        _mockLogger.Received(1).LogWarning("Interfaces.IpPool.Startの形式が正しくありません。");
    }

    /// <summary>
    /// PostAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithApiError_ThrowsApiException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validClusterID }
        };
        var builder = new AsgRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateAutoScalingGroupRequest
        {
            Name = "test-asg",
            Zone = "is1a",
            WorkerServiceClassPath = "cloud/apprun/dedicated/worker/1vcpu_2gb",
            MinNodes = 1,
            MaxNodes = 5,
            Interfaces = new List<AutoScalingGroupNodeInterface>
            {
                new AutoScalingGroupNodeInterface
                {
                    InterfaceIndex = 0,
                    Upstream = "shared",
                    ConnectsToLB = false
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

        requestAdapter.SendAsync<CreateAutoScalingGroupResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreateAutoScalingGroupResponse>>(),
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
                @"^Auto Scaling Group作成のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncでその他のエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithOtherError_ThrowsException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validClusterID }
        };
        var builder = new AsgRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateAutoScalingGroupRequest
        {
            Name = "test-asg",
            Zone = "is1a",
            WorkerServiceClassPath = "cloud/apprun/dedicated/worker/1vcpu_2gb",
            MinNodes = 1,
            MaxNodes = 5,
            Interfaces = new List<AutoScalingGroupNodeInterface>
            {
                new AutoScalingGroupNodeInterface
                {
                    InterfaceIndex = 0,
                    Upstream = "shared",
                    ConnectsToLB = false
                }
            }
        };
        var expectedException = new Exception();

        requestAdapter.SendAsync<CreateAutoScalingGroupResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreateAutoScalingGroupResponse>>(),
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
                @"^Auto Scaling Groupの作成中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncがCreateAutoScalingGroupResponseを返すことを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_ReturnsCreateAutoScalingGroupResponse()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://example.com" },
            { "clusterID", _validClusterID }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var expectedResponse = new CreateAutoScalingGroupResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreateAutoScalingGroupResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);
        var builder = new AsgRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var createAutoScalingGroup = new CreateAutoScalingGroupRequest
        {
            Name = "test-asg",
            Zone = "is1a",
            WorkerServiceClassPath = "cloud/apprun/dedicated/worker/1vcpu_2gb",
            MinNodes = 1,
            MaxNodes = 5,
            Interfaces = new List<AutoScalingGroupNodeInterface>
            {
                new AutoScalingGroupNodeInterface
                {
                    InterfaceIndex = 0,
                    Upstream = "shared",
                    ConnectsToLB = false
                }
            }
        };

        var response = await builder.PostAsync(createAutoScalingGroup);

        Assert.NotNull(response);
        Assert.IsType<CreateAutoScalingGroupResponse>(response);

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^Auto Scaling Groupの作成を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^Auto Scaling Groupの作成が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// WithUrlが新しいAsgRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewBuilder()
    {
        var pathParameters = new Dictionary<string, object>();
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new AsgRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var rawUrl = "https://example.com/clusters/123/asg";

        var newBuilder = builder.WithUrl(rawUrl);

        Assert.NotNull(newBuilder);
        Assert.IsType<AsgRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }

    /// <summary>
    /// QueryParametersのCursorプロパティが正しく設定されることを確認
    /// </summary>
    [Fact]
    public void AsgRequestBuilderGetQueryParameters_Cursor_SetAndGet()
    {
        var queryParameters = new AsgRequestBuilder.AsgRequestBuilderGetQueryParameters();
        var cursor = Guid.NewGuid();

        queryParameters.Cursor = cursor;

        Assert.Equal(cursor, queryParameters.Cursor);
    }

    /// <summary>
    /// QueryParametersのLastAutoScalingGroupIDプロパティが正しく設定されることを確認
    /// </summary>
    [Fact]
    public void AsgRequestBuilderGetQueryParameters_LastAutoScalingGroupID_SetAndGet()
    {
        var queryParameters = new AsgRequestBuilder.AsgRequestBuilderGetQueryParameters();
        var lastAutoScalingGroupId = Guid.NewGuid();

        queryParameters.LastAutoScalingGroupID = lastAutoScalingGroupId;

        Assert.Equal(lastAutoScalingGroupId, queryParameters.LastAutoScalingGroupID);
    }

    /// <summary>
    /// QueryParametersのMaxItemsプロパティが正しく設定されることを確認
    /// </summary>
    [Fact]
    public void AsgRequestBuilderGetQueryParameters_MaxItems_SetAndGet()
    {
        var queryParameters = new AsgRequestBuilder.AsgRequestBuilderGetQueryParameters
        {
            MaxItems = 10
        };

        Assert.Equal(10, queryParameters.MaxItems);
    }
}
