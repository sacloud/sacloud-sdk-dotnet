using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.AppRunDedicated.Clusters;
using Sacloud.Sdk.AppRunDedicated.Clusters.Item;
using Sacloud.Sdk.AppRunDedicated.Models;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.AppRunDedicated.Tests.Clusters;

/// <summary>
/// ClustersRequestBuilderの単体テスト
/// </summary>
public class ClustersRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<ClustersRequestBuilder> _mockLogger;

    public ClustersRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<ClustersRequestBuilder>>();
        _mockLoggerFactory.CreateLogger(Arg.Any<string>()).Returns(_mockLogger);
    }

    /// <summary>
    /// コンストラクタ（PathParameters版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_InitializesSuccessfully()
    {
        var pathParameters = new Dictionary<string, object>();
        var requestAdapter = Substitute.For<IRequestAdapter>();

        var builder = new ClustersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_InitializesSuccessfully()
    {
        var rawUrl = "https://example.com/clusters";
        var requestAdapter = Substitute.For<IRequestAdapter>();

        var builder = new ClustersRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// インデクサー（Guid版）が正しいWithClusterItemRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_WithGuid_ReturnsWithClusterItemRequestBuilder()
    {
        var pathParameters = new Dictionary<string, object>();
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new ClustersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var clusterId = Guid.NewGuid();

        var itemBuilder = builder[clusterId];

        Assert.NotNull(itemBuilder);
        Assert.IsType<WithClusterItemRequestBuilder>(itemBuilder);
    }

    /// <summary>
    /// ToGetRequestInformationがGETメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_CreatesRequestInformationWithGetMethod()
    {
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://example.com" } };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new ClustersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.GET, requestInfo.HttpMethod);
        Assert.Contains("clusters", requestInfo.UrlTemplate);
    }

    /// <summary>
    /// ToGetRequestInformationがクエリパラメータを正しく処理することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_WithQueryParameters_ConfiguresCorrectly()
    {
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://example.com" } };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new ClustersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var cursor = Guid.NewGuid();
        var lastClusterId = Guid.NewGuid();

        var requestInfo = builder.ToGetRequestInformation(config =>
        {
            config.QueryParameters.Cursor = cursor;
            config.QueryParameters.LastClusterID = lastClusterId;
            config.QueryParameters.MaxItems = 10;
        });

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.GET, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToPostRequestInformationがPOSTメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_CreatesRequestInformationWithPostMethod()
    {
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://example.com" } };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new ClustersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var createCluster = new CreateClusterRequest
        {
            Name = "test-cluster",
            ServicePrincipalID = "test-principal"
        };

        var requestInfo = builder.ToPostRequestInformation(createCluster);

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.POST, requestInfo.HttpMethod);
        Assert.Contains("clusters", requestInfo.UrlTemplate);
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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ClustersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
    [InlineData(4)]
    [InlineData(31)]
    public async Task GetAsync_WithInvalidRangeMaxItems_ThrowsValidationException(int invalidMaxItemsValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ClustersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync(config =>
            {
                config.QueryParameters.MaxItems = invalidMaxItemsValue;
            });
        });

        Assert.Equal("MaxItemsの値は5以上30以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("MaxItemsの値は5以上30以下で入力してください。");
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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ClustersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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

        requestAdapter.SendAsync<ListClustersResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListClustersResponse>>(),
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
                @"^クラスタの一覧取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ClustersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync<ListClustersResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListClustersResponse>>(),
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
                @"^クラスタの一覧の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncがListClustersResponseを正常に返すことを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_ReturnsListClusterResponse()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://example.com" }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var expectedResponse = new ListClustersResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListClustersResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);
        var builder = new ClustersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var response = await builder.GetAsync();

        Assert.NotNull(response);
        Assert.IsType<ListClustersResponse>(response);

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^クラスタの一覧の取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^クラスタの一覧の取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ClustersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(null!);
        });

        Assert.Equal("リクエストボディは必須です。", exception.Message);
        _mockLogger.Received(1).LogWarning("リクエストボディは必須です。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのパラメータがnullの場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("Name")]
    [InlineData("Ports")]
    [InlineData("Ports.Port")]
    [InlineData("Ports.Protocol")]
    [InlineData("ServicePrincipalID")]
    public async Task PostAsync_WithNullRequestBodyParameter_ThrowsValidationException(string invalidParameterName)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ClustersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateClusterRequest
        {
            Name = invalidParameterName == "Name" ? null : "test-app",
            Ports = invalidParameterName == "Ports" ? null : new List<ClusterLoadBalancerPort>
            {
                new ClusterLoadBalancerPort
                {
                    Port = invalidParameterName == "Ports.Port" ? null : 1234,
                    Protocol = invalidParameterName == "Ports.Protocol" ? null : ClusterLoadBalancerPort_protocol.Http
                }
            },
            ServicePrincipalID = invalidParameterName == "ServicePrincipalID" ? null : "123456789012"
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
    [Fact]
    public async Task PostAsync_WithInvalidMaxLengthRequestBodyParameter_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ClustersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateClusterRequest
        {
            Name = "123456789012345678901",
            Ports = new List<ClusterLoadBalancerPort>
            {
                new ClusterLoadBalancerPort
                {
                    Port = 1234,
                    Protocol = ClusterLoadBalancerPort_protocol.Http
                }
            },
            ServicePrincipalID = "123456789012"
        };
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });
        Assert.Equal("Nameは1文字以上20文字以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("Nameは1文字以上20文字以下で入力してください。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディパラメータの文字列が正規表現パターンに一致しなかった場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("Name", "invalid+name")]
    [InlineData("Name", "無効なName")]
    [InlineData("LetsEncryptEmail", "invalid.email")]
    [InlineData("LetsEncryptEmail", "@invalid.email")]
    [InlineData("LetsEncryptEmail", "invalid@email")]
    [InlineData("LetsEncryptEmail", "invalid&email@example.com")]
    [InlineData("LetsEncryptEmail", "invalid-email@example%.com")]
    [InlineData("LetsEncryptEmail", "invalid-email@example.123")]
    [InlineData("LetsEncryptEmail", "invalid-email@example.c")]
    [InlineData("LetsEncryptEmail", "無効なLetsEncryptEmail")]
    public async Task PostAsync_WithInvalidRegexPatternRequestBodyParameter_ThrowsValidationException(string invalidParameterName, string invalidParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ClustersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateClusterRequest
        {
            Name = invalidParameterName == "Name" ? invalidParameterValue : "test-name",
            LetsEncryptEmail = invalidParameterName == "LetsEncryptEmail" ? invalidParameterValue : "test@example.com",
            Ports = new List<ClusterLoadBalancerPort>
            {
                new ClusterLoadBalancerPort
                {
                    Port = 1234,
                    Protocol = ClusterLoadBalancerPort_protocol.Http
                }
            },
            ServicePrincipalID = "123456789012"
        };
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });
        Assert.Equal($"{invalidParameterName}の形式が正しくありません。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}の形式が正しくありません。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのパラメータが有効な数値範囲でない値の場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(65536)]
    public async Task PostAsync_WithInvalidRangeRequestBodyParameter_ThrowsValidationException(int invalidParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ClustersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateClusterRequest
        {
            Name = "test-name",
            Ports = new List<ClusterLoadBalancerPort>
            {
                new ClusterLoadBalancerPort
                {
                    Port = invalidParameterValue,
                    Protocol = ClusterLoadBalancerPort_protocol.Http
                }
            },
            ServicePrincipalID = "123456789012"
        };
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });
        Assert.Equal("Ports.Portの値は1以上65535以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("Ports.Portの値は1以上65535以下で入力してください。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのパラメータが有効な要素数でない値の場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithInvalidElementCountRequestBodyParameter_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ClustersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateClusterRequest
        {
            Name = "test-name",
            Ports = Enumerable.Range(0, 6).Select(_ => new ClusterLoadBalancerPort { Port = 1234, Protocol = ClusterLoadBalancerPort_protocol.Http }).ToList(),
            ServicePrincipalID = "123456789012"
        };
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });
        Assert.Equal("Portsの要素数は5以下にしてください。", exception.Message);
        _mockLogger.Received(1).LogWarning("Portsの要素数は5以下にしてください。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのパラメータの値がEnumに含まれていない場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithOutOfEnumRequestBodyParameter_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ClustersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateClusterRequest
        {
            Name = "test-name",
            Ports = new List<ClusterLoadBalancerPort>
            {
                new ClusterLoadBalancerPort
                {
                    Port = 1234,
                    Protocol = (ClusterLoadBalancerPort_protocol)99
                }
            },
            ServicePrincipalID = "123456789012"
        };
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });
        Assert.Equal("Ports.ProtocolはEnumに含まれる値を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("Ports.ProtocolはEnumに含まれる値を入力してください。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのパラメータの値が特定の文字数でない場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("12345678901")]
    [InlineData("1234567890123")]
    public async Task PostAsync_WithInvalidLengthRequestBodyParameter_ThrowsValidationException(string invalidParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ClustersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateClusterRequest
        {
            Name = "test-name",
            Ports = new List<ClusterLoadBalancerPort>
            {
                new ClusterLoadBalancerPort
                {
                    Port = 1234,
                    Protocol = ClusterLoadBalancerPort_protocol.Http
                }
            },
            ServicePrincipalID = invalidParameterValue
        };
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });
        Assert.Equal("ServicePrincipalIDは12文字で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("ServicePrincipalIDは12文字で入力してください。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのパラメータの値が指定不可の値と一致する場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(5950)]
    [InlineData(5951)]
    [InlineData(5952)]
    [InlineData(5953)]
    [InlineData(5954)]
    [InlineData(5955)]
    [InlineData(5956)]
    [InlineData(5957)]
    [InlineData(5958)]
    [InlineData(5959)]
    public async Task PostAsync_WithDeniedValueRequestBodyParameter_ThrowsValidationException(int invalidParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ClustersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateClusterRequest
        {
            Name = "test-name",
            Ports = new List<ClusterLoadBalancerPort>
            {
                new ClusterLoadBalancerPort
                {
                    Port = invalidParameterValue,
                    Protocol = ClusterLoadBalancerPort_protocol.Http
                }
            },
            ServicePrincipalID = "123456789012"
        };
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });
        Assert.Equal("Ports.Portには許可された値を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("Ports.Portには許可された値を入力してください。");
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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ClustersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateClusterRequest
        {
            Name = "123456789012345678901",
            LetsEncryptEmail = "invalid-email",
            Ports = new List<ClusterLoadBalancerPort>
            {
                new ClusterLoadBalancerPort
                {
                    Port = 5950,
                    Protocol = (ClusterLoadBalancerPort_protocol)99
                }
            },
            ServicePrincipalID = null
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        var expectedMessage = """
            LetsEncryptEmailの形式が正しくありません。
            Nameは1文字以上20文字以下で入力してください。
            ServicePrincipalIDが入力されていません。
            Ports.Portには許可された値を入力してください。
            Ports.ProtocolはEnumに含まれる値を入力してください。
            """;
        Assert.Equal(expectedMessage, exception.Message);
        _mockLogger.Received(1).LogWarning("LetsEncryptEmailの形式が正しくありません。");
        _mockLogger.Received(1).LogWarning("Nameは1文字以上20文字以下で入力してください。");
        _mockLogger.Received(1).LogWarning("ServicePrincipalIDが入力されていません。");
        _mockLogger.Received(1).LogWarning("Ports.Portには許可された値を入力してください。");
        _mockLogger.Received(1).LogWarning("Ports.ProtocolはEnumに含まれる値を入力してください。");
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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ClustersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateClusterRequest
        {
            Name = "test-name",
            Ports = new List<ClusterLoadBalancerPort>
            {
                new ClusterLoadBalancerPort
                {
                    Port = 1234,
                    Protocol = ClusterLoadBalancerPort_protocol.Http
                }
            },
            ServicePrincipalID = "123456789012"
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

        requestAdapter.SendAsync<CreateClusterResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreateClusterResponse>>(),
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
                @"^クラスタ作成のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ClustersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateClusterRequest
        {
            Name = "test-name",
            Ports = new List<ClusterLoadBalancerPort>
            {
                new ClusterLoadBalancerPort
                {
                    Port = 1234,
                    Protocol = ClusterLoadBalancerPort_protocol.Http
                }
            },
            ServicePrincipalID = "123456789012"
        };
        var expectedException = new Exception();

        requestAdapter.SendAsync<CreateClusterResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreateClusterResponse>>(),
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
                @"^クラスタの作成中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncがCreateClusterResponseを正常に返すことを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_ReturnsCreateClusterResponse()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://example.com" }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var expectedResponse = new CreateClusterResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreateClusterResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);
        var builder = new ClustersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var createCluster = new CreateClusterRequest
        {
            Name = "test-name",
            Ports = new List<ClusterLoadBalancerPort>
            {
                new ClusterLoadBalancerPort
                {
                    Port = 1234,
                    Protocol = ClusterLoadBalancerPort_protocol.Http
                }
            },
            ServicePrincipalID = "123456789012"
        };

        var response = await builder.PostAsync(createCluster);

        Assert.NotNull(response);
        Assert.IsType<CreateClusterResponse>(response);

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^クラスタの作成を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^クラスタの作成が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// WithUrlが新しいClustersRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewBuilder()
    {
        var pathParameters = new Dictionary<string, object>();
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new ClustersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var rawUrl = "https://example.com/clusters";

        var newBuilder = builder.WithUrl(rawUrl);

        Assert.NotNull(newBuilder);
        Assert.IsType<ClustersRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }

    /// <summary>
    /// QueryParametersのCursorプロパティが正しく設定・取得されることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Cursor_SetsAndGetsCorrectly()
    {
        var queryParameters = new ClustersRequestBuilder.ClustersRequestBuilderGetQueryParameters();
        var cursor = Guid.NewGuid();

        queryParameters.Cursor = cursor;

        Assert.Equal(cursor, queryParameters.Cursor);
    }

    /// <summary>
    /// QueryParametersのLastClusterIDプロパティが正しく設定・取得されることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_LastClusterID_SetsAndGetsCorrectly()
    {
        var queryParameters = new ClustersRequestBuilder.ClustersRequestBuilderGetQueryParameters();
        var lastClusterId = Guid.NewGuid();

        queryParameters.LastClusterID = lastClusterId;

        Assert.Equal(lastClusterId, queryParameters.LastClusterID);
    }

    /// <summary>
    /// QueryParametersのMaxItemsプロパティが正しく設定・取得されることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_MaxItems_SetsAndGetsCorrectly()
    {
        var queryParameters = new ClustersRequestBuilder.ClustersRequestBuilderGetQueryParameters
        {
            MaxItems = 10
        };

        Assert.Equal(10, queryParameters.MaxItems);
    }
}

