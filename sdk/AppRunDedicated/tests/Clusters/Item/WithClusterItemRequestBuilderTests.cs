using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.AppRunDedicated.Clusters.Item;
using Sacloud.Sdk.AppRunDedicated.Clusters.Item.Asg;
using Sacloud.Sdk.AppRunDedicated.Clusters.Item.Certificates;
using Sacloud.Sdk.AppRunDedicated.Clusters.Item.Load_balancer;
using Sacloud.Sdk.AppRunDedicated.Models;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.AppRunDedicated.Tests.Clusters.Item;

/// <summary>
/// WithClusterItemRequestBuilderの単体テスト
/// </summary>
public class WithClusterItemRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<WithClusterItemRequestBuilder> _mockLogger;
    private readonly Guid _validClusterID;

    public WithClusterItemRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<WithClusterItemRequestBuilder>>();
        _mockLoggerFactory.CreateLogger<WithClusterItemRequestBuilder>().Returns(_mockLogger);
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

        var builder = new WithClusterItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// rawUrlを使用したコンストラクタが正しく動作することを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_SetsProperties()
    {
        var rawUrl = "https://example.com/clusters/123";
        var requestAdapter = Substitute.For<IRequestAdapter>();

        var builder = new WithClusterItemRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// Asgプロパティが正しくAsgRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Asg_ReturnsAsgRequestBuilder()
    {
        var pathParameters = new Dictionary<string, object>();
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WithClusterItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var asgBuilder = builder.Asg;

        Assert.NotNull(asgBuilder);
        Assert.IsType<AsgRequestBuilder>(asgBuilder);
    }

    /// <summary>
    /// Certificatesプロパティが正しくCertificatesRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Certificates_ReturnsCertificatesRequestBuilder()
    {
        var pathParameters = new Dictionary<string, object>();
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WithClusterItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var certificatesBuilder = builder.Certificates;

        Assert.NotNull(certificatesBuilder);
        Assert.IsType<CertificatesRequestBuilder>(certificatesBuilder);
    }

    /// <summary>
    /// Load_balancerプロパティが正しくLoad_balancerRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Load_balancer_ReturnsLoad_balancerRequestBuilder()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://example.com" },
            { "clusterID", _validClusterID },
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WithClusterItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var loadBalancerBuilder = builder.Load_balancer;

        Assert.NotNull(loadBalancerBuilder);
        Assert.IsType<Load_balancerRequestBuilder>(loadBalancerBuilder);
    }

    /// <summary>
    /// ToDeleteRequestInformationが正しいRequestInformationを生成することを確認
    /// </summary>
    [Fact]
    public void ToDeleteRequestInformation_ReturnsValidRequestInformation()
    {
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://example.com" }, { "clusterID", _validClusterID } };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WithClusterItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToDeleteRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.DELETE, requestInfo.HttpMethod);
        Assert.Contains("clusters", requestInfo.UrlTemplate);
    }

    /// <summary>
    /// ToGetRequestInformationが正しいRequestInformationを生成することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_ReturnsValidRequestInformation()
    {
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://example.com" }, { "clusterID", _validClusterID } };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WithClusterItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.GET, requestInfo.HttpMethod);
        Assert.Contains("clusters", requestInfo.UrlTemplate);
    }

    /// <summary>
    /// ToPutRequestInformationが正しいRequestInformationを生成することを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_ReturnsValidRequestInformation()
    {
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://example.com" }, { "clusterID", _validClusterID } };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WithClusterItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var updateCluster = new UpdateClusterRequest
        {
            ServicePrincipalID = "updated-principal"
        };

        var requestInfo = builder.ToPutRequestInformation(updateCluster);

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.PUT, requestInfo.HttpMethod);
        Assert.Contains("clusters", requestInfo.UrlTemplate);
    }

    /// <summary>
    /// DeleteAsyncのパスパラメータにclusterIDがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("clusterID", "")]
    [InlineData("clusterID", " ")]
    [InlineData("clusterID", "　")]
    public async Task DeleteAsync_WithNullOrEmptyPathParameters_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { pathParameterKey, pathParameterValue }
        };
        var builder = new WithClusterItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
    public async Task DeleteAsync_WithInvalidFormatPathParameters_ThrowsValidationException(string invalidPathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", invalidPathParameterValue }
        };
        var builder = new WithClusterItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.DeleteAsync();
        });

        Assert.Equal("クラスタIDはGUID形式で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("クラスタIDはGUID形式で入力してください。");
    }

    /// <summary>
    /// DeleteAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_WithApiError_ThrowsApiException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validClusterID }
        };
        var builder = new WithClusterItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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
                @"^クラスタ削除のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// DeleteAsyncでその他のエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_WithOtherError_ThrowsException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validClusterID }
        };
        var builder = new WithClusterItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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
                @"^クラスタの削除中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// DeleteAsyncが正常に完了することを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_CompletesSuccessfully()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://example.com" },
            { "clusterID", _validClusterID }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(Task.CompletedTask);
        var builder = new WithClusterItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await builder.DeleteAsync();

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
                    @"^クラスタの削除を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^クラスタの削除が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 204\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
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
        var builder = new WithClusterItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new ReadClusterResponse();

        requestAdapter.SendAsync<ReadClusterResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ReadClusterResponse>>(),
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
        var builder = new WithClusterItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new ReadClusterResponse();

        requestAdapter.SendAsync<ReadClusterResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ReadClusterResponse>>(),
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
        var builder = new WithClusterItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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

        requestAdapter.SendAsync<ReadClusterResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ReadClusterResponse>>(),
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
                @"^クラスタの詳細取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var builder = new WithClusterItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync<ReadClusterResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ReadClusterResponse>>(),
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
                @"^クラスタの詳細の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncがReadClusterResponseを返すことを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_ReturnsGetClusterResponse()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://example.com" },
            { "clusterID", _validClusterID }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var expectedResponse = new ReadClusterResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ReadClusterResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);
        var builder = new WithClusterItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var response = await builder.GetAsync();

        Assert.NotNull(response);
        Assert.IsType<ReadClusterResponse>(response);

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^クラスタの詳細の取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^クラスタの詳細の取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PutAsyncのパスパラメータにclusterIDがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("clusterID", "")]
    [InlineData("clusterID", " ")]
    [InlineData("clusterID", "　")]
    public async Task PutAsync_WithNullOrEmptyPathParameters_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { pathParameterKey, pathParameterValue }
        };
        var builder = new WithClusterItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateClusterRequest
        {
            ServicePrincipalID = "123456789012"
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
    public async Task PutAsync_WithInvalidFormatPathParameters_ThrowsValidationException(string invalidPathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", invalidPathParameterValue }
        };
        var builder = new WithClusterItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateClusterRequest
        {
            ServicePrincipalID = "123456789012"
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(body);
        });

        Assert.Equal("クラスタIDはGUID形式で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("クラスタIDはGUID形式で入力してください。");
    }

    /// <summary>
    /// PutAsyncのリクエストボディがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithNullRequestBody_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validClusterID }
        };
        var builder = new WithClusterItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(null);
        });

        Assert.Equal("リクエストボディは必須です。", exception.Message);
        _mockLogger.Received(1).LogWarning("リクエストボディは必須です。");
    }

    /// <summary>
    /// PutAsyncのリクエストボディのパラメータがnullの場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("ServicePrincipalID")]
    [InlineData("Ports.Port")]
    [InlineData("Ports.Protocol")]
    public async Task PutAsync_WithNullRequestBodyParameter_ThrowsValidationException(string invalidParameterName)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validClusterID }
        };
        var builder = new WithClusterItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateClusterRequest
        {
            ServicePrincipalID = invalidParameterName == "ServicePrincipalID" ? null : "123456789012",
            Ports = new List<ClusterLoadBalancerPort>
            {
                new ClusterLoadBalancerPort
                {
                    Port = invalidParameterName == "Ports.Port" ? null : 80,
                    Protocol = invalidParameterName == "Ports.Protocol" ? null : ClusterLoadBalancerPort_protocol.Http
                }
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(body);
        });

        Assert.Equal($"{invalidParameterName}が入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}が入力されていません。");
    }

    /// <summary>
    /// PutAsyncのパスパラメータの文字数が正規表現パターンに一致しなかった場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalid.email")]
    [InlineData("@invalid.email")]
    [InlineData("invalid@email")]
    [InlineData("invalid&email@example.com")]
    [InlineData("invalid-email@example%.com")]
    [InlineData("invalid-email@example.123")]
    [InlineData("invalid-email@example.c")]
    public async Task PutAsync_WithInvalidRegexPatternRequestBodyParameter_ThrowsValidationException(string invalidParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validClusterID }
        };
        var builder = new WithClusterItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateClusterRequest
        {
            LetsEncryptEmail = invalidParameterValue,
            ServicePrincipalID = "123456789012"
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(body);
        });

        Assert.Equal("LetsEncryptEmailの形式が正しくありません。", exception.Message);
        _mockLogger.Received(1).LogWarning("LetsEncryptEmailの形式が正しくありません。");
    }

    /// <summary>
    /// PutAsyncのPortsリストが5件を超える場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithTooManyPortsInRequestBody_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validClusterID }
        };
        var builder = new WithClusterItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateClusterRequest
        {
            ServicePrincipalID = "123456789012",
            Ports = new List<ClusterLoadBalancerPort>
            {
                new ClusterLoadBalancerPort { Port = 80, Protocol = ClusterLoadBalancerPort_protocol.Http },
                new ClusterLoadBalancerPort { Port = 443, Protocol = ClusterLoadBalancerPort_protocol.Https },
                new ClusterLoadBalancerPort { Port = 8080, Protocol = ClusterLoadBalancerPort_protocol.Http },
                new ClusterLoadBalancerPort { Port = 8443, Protocol = ClusterLoadBalancerPort_protocol.Https },
                new ClusterLoadBalancerPort { Port = 9000, Protocol = ClusterLoadBalancerPort_protocol.Tcp },
                new ClusterLoadBalancerPort { Port = 9001, Protocol = ClusterLoadBalancerPort_protocol.Tcp }
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(body);
        });

        Assert.Equal("Portsの要素数は5以下にしてください。", exception.Message);
        _mockLogger.Received(1).LogWarning("Portsの要素数は5以下にしてください。");
    }

    /// <summary>
    /// PutAsyncのPortsリスト内のPortが範囲外の場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(65536)]
    public async Task PutAsync_WithPortOutOfRangeInRequestBodyPortsItem_ThrowsValidationException(int invalidParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validClusterID }
        };
        var builder = new WithClusterItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateClusterRequest
        {
            ServicePrincipalID = "123456789012",
            Ports = new List<ClusterLoadBalancerPort>
            {
                new ClusterLoadBalancerPort { Port = invalidParameterValue, Protocol = ClusterLoadBalancerPort_protocol.Http }
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(body);
        });

        Assert.Equal("Ports.Portの値は1以上65535以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("Ports.Portの値は1以上65535以下で入力してください。");
    }

    /// <summary>
    /// PutAsyncのPortsリスト内のPortが予約済みポート番号（5950-5959）の場合に例外がスローされることを確認
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
    public async Task PutAsync_WithReservedPortInRequestBodyPortsItem_ThrowsValidationException(int reservedPort)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validClusterID }
        };
        var builder = new WithClusterItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateClusterRequest
        {
            ServicePrincipalID = "123456789012",
            Ports = new List<ClusterLoadBalancerPort>
            {
                new ClusterLoadBalancerPort { Port = reservedPort, Protocol = ClusterLoadBalancerPort_protocol.Http }
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(body);
        });

        Assert.Equal("Ports.Portには許可された値を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("Ports.Portには許可された値を入力してください。");
    }

    /// <summary>
    /// PutAsyncのリクエストボディのパラメータの値がEnumに含まれていない場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithOutOfEnumRequestBodyParameter_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "clusterID", _validClusterID }
        };
        var builder = new WithClusterItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateClusterRequest
        {
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
            await builder.PutAsync(body);
        });
        Assert.Equal("Ports.ProtocolはEnumに含まれる値を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("Ports.ProtocolはEnumに含まれる値を入力してください。");
    }

    /// <summary>
    /// PutAsyncのリクエストボディの複数パラメータが無効な値の場合に例外がスローされることを確認
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
        var builder = new WithClusterItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateClusterRequest
        {
            LetsEncryptEmail = "invalid-email",
            ServicePrincipalID = null
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(body);
        });

        var expectedMessage = """
            LetsEncryptEmailの形式が正しくありません。
            ServicePrincipalIDが入力されていません。
            """;
        Assert.Equal(expectedMessage, exception.Message);
        _mockLogger.Received(1).LogWarning("LetsEncryptEmailの形式が正しくありません。");
        _mockLogger.Received(1).LogWarning("ServicePrincipalIDが入力されていません。");
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
            { "clusterID", _validClusterID }
        };
        var builder = new WithClusterItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateClusterRequest
        {
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
                @"^クラスタ更新のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
            { "clusterID", _validClusterID }
        };
        var builder = new WithClusterItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateClusterRequest
        {
            ServicePrincipalID = "123456789012"
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
                @"^クラスタの更新中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
            { "clusterID", _validClusterID }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(Task.CompletedTask);
        var builder = new WithClusterItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var updateCluster = new UpdateClusterRequest
        {
            ServicePrincipalID = "123456789012"
        };

        await builder.PutAsync(updateCluster);

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
                    @"^クラスタの更新を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^クラスタの更新が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 204\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// WithUrlが新しいWithClusterItemRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewBuilder()
    {
        var pathParameters = new Dictionary<string, object>();
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var builder = new WithClusterItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var rawUrl = "https://example.com/clusters/123";

        var newBuilder = builder.WithUrl(rawUrl);

        Assert.NotNull(newBuilder);
        Assert.IsType<WithClusterItemRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }
}
