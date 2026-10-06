using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using System.ComponentModel.DataAnnotations;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.CloudHSM.Cloudhsm.Cloudhsms.Item.Clients.Item;
using Sacloud.Sdk.CloudHSM.Cloudhsm.Cloudhsms.Item.Clients;
using Sacloud.Sdk.CloudHSM.Models;
using Sacloud.Sdk.Shared.Models;

namespace Sacloud.Sdk.CloudHSM.Tests.Cloudhsm.Cloudhsms.Item.Clients;

/// <summary>
/// ClientsRequestBuilderのテスト
/// </summary>
public class ClientsRequestBuilderTests
{
    private readonly IRequestAdapter _mockRequestAdapter;
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger _mockLogger;

    public ClientsRequestBuilderTests()
    {
        _mockRequestAdapter = Substitute.For<IRequestAdapter>();
        _mockRequestAdapter.BaseUrl = "https://api.example.com";
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger>();
        _mockLoggerFactory.CreateLogger(Arg.Any<string>()).Returns(_mockLogger);
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
            { "cloudhsm_resource_id", "test-cloudhsm-resource-id" },
        };

        var builder = new ClientsRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（RawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_InitializesSuccessfully()
    {
        var rawUrl = "https://api.example.com/cloudhsm/cloudhsms/test-cloudhsm-resource-id/clients";

        var builder = new ClientsRequestBuilder(rawUrl, _mockRequestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// インデクサーが正しいClientsItemRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_ReturnsClientsItemRequestBuilder()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "cloudhsm_resource_id", "test-cloudhsm-resource-id" },
        };
        var builder = new ClientsRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var itemBuilder = builder["test-id"];

        Assert.NotNull(itemBuilder);
        Assert.IsType<ClientsItemRequestBuilder>(itemBuilder);
    }

    /// <summary>
    /// インデクサーが毎回新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_ReturnsNewInstance_OnEachCall()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "cloudhsm_resource_id", "test-cloudhsm-resource-id" },
        };
        var builder = new ClientsRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var itemBuilder1 = builder["test-id"];
        var itemBuilder2 = builder["test-id"];

        Assert.NotSame(itemBuilder1, itemBuilder2);
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
            { "cloudhsm_resource_id", "test-cloudhsm-resource-id" },
        };
        var builder = new ClientsRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

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
            { "cloudhsm_resource_id", "test-cloudhsm-resource-id" },
        };
        var builder = new ClientsRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

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
            { "cloudhsm_resource_id", "test-cloudhsm-resource-id" },
        };
        var builder = new ClientsRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.True(requestInfo.PathParameters.ContainsKey("cloudhsm_resource_id"));
        Assert.Equal("test-cloudhsm-resource-id", requestInfo.PathParameters["cloudhsm_resource_id"]);
    }

    /// <summary>
    /// ToPostRequestInformationがPOSTメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_CreatesRequestInformationWithPostMethod()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "cloudhsm_resource_id", "test-cloudhsm-resource-id" },
        };
        var builder = new ClientsRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToPostRequestInformation(new WrappedCreateCloudHSMClient());

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.POST, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToPostRequestInformationが正しいAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_SetsAcceptHeader()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "cloudhsm_resource_id", "test-cloudhsm-resource-id" },
        };
        var builder = new ClientsRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToPostRequestInformation(new WrappedCreateCloudHSMClient());

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// ToPostRequestInformationが正しいContent-Typeヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_SetsContentTypeHeader()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "cloudhsm_resource_id", "test-cloudhsm-resource-id" },
        };
        var builder = new ClientsRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToPostRequestInformation(new WrappedCreateCloudHSMClient());

        Assert.True(requestInfo.Headers.ContainsKey("Content-Type"));
        Assert.Contains("application/json", requestInfo.Headers["Content-Type"]);
    }

    /// <summary>
    /// ToPostRequestInformationが正しいPathParametersを設定することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_SetsPathParameters()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "cloudhsm_resource_id", "test-cloudhsm-resource-id" },
        };
        var builder = new ClientsRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToPostRequestInformation(new WrappedCreateCloudHSMClient());

        Assert.True(requestInfo.PathParameters.ContainsKey("cloudhsm_resource_id"));
        Assert.Equal("test-cloudhsm-resource-id", requestInfo.PathParameters["cloudhsm_resource_id"]);
    }

    /// <summary>
    /// GetAsync が期待されるSendAsyncを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_CallsRequestAdapterSendAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "cloudhsm_resource_id", "test-cloudhsm-resource-id" },
        };
        var builder = new ClientsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var mockResponse = new PaginatedCloudHSMClientList();
        requestAdapter.SendAsync<PaginatedCloudHSMClientList>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PaginatedCloudHSMClientList>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(mockResponse);

        var result = await builder.GetAsync();

        Assert.NotNull(result);
        Assert.IsType<PaginatedCloudHSMClientList>(result);

        await requestAdapter.Received(1).SendAsync<PaginatedCloudHSMClientList>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PaginatedCloudHSMClientList>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());

        _mockLogger.Received(1).Log(LogLevel.Information, Arg.Any<EventId>(), Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^CloudHSMクライアント一覧の取得を開始します。 \d{4}-\d{2}-\d{2}T.*Z$")), null, Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(LogLevel.Information, Arg.Any<EventId>(), Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^CloudHSMクライアント一覧の取得が完了しました。 \d{4}-\d{2}-\d{2}T.*Z \[StatusCode, 200\]$")), null, Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsync が期待されるSendAsyncを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_CallsRequestAdapterSendAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "cloudhsm_resource_id", "test-cloudhsm-resource-id" },
        };
        var builder = new ClientsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var mockResponse = new WrappedCreateCloudHSMClient();
        requestAdapter.SendAsync<WrappedCreateCloudHSMClient>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedCreateCloudHSMClient>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(mockResponse);

        var result = await builder.PostAsync(new WrappedCreateCloudHSMClient
        {
            Client = new CreateCloudHSMClient
            {
                Certificate = "test-certificate",
                Name = "test-client"
            }
        });

        Assert.NotNull(result);
        Assert.IsType<WrappedCreateCloudHSMClient>(result);

        await requestAdapter.Received(1).SendAsync<WrappedCreateCloudHSMClient>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedCreateCloudHSMClient>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());

        _mockLogger.Received(1).Log(LogLevel.Information, Arg.Any<EventId>(), Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^CloudHSMクライアントの作成を開始します。 \d{4}-\d{2}-\d{2}T.*Z$")), null, Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(LogLevel.Information, Arg.Any<EventId>(), Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^CloudHSMクライアントの作成が完了しました。 \d{4}-\d{2}-\d{2}T.*Z \[StatusCode, 201\]$")), null, Arg.Any<Func<object, Exception?, string>>());
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
            { "cloudhsm_resource_id", "test-cloudhsm-resource-id" },
        };
        var builder = new ClientsRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);
        var newUrl = "https://newapi.example.com/cloudhsm/cloudhsms/new-cloudhsm-resource-%2Did/clients";

        var newBuilder = builder.WithUrl(newUrl);

        Assert.NotNull(newBuilder);
        Assert.NotSame(builder, newBuilder);
        Assert.IsType<ClientsRequestBuilder>(newBuilder);
    }

    /// <summary>
    /// コンストラクタ（PathParameters版）でLoggerFactoryがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParametersAndNullLoggerFactory_ThrowsArgumentNullException()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "cloudhsm_resource_id", "test-cloudhsm-resource-%2Did" },
        };

        Assert.Throws<ArgumentNullException>(() => new ClientsRequestBuilder(pathParameters, _mockRequestAdapter, null!));
    }

    /// <summary>
    /// コンストラクタ（RawUrl版）でLoggerFactoryがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrlAndNullLoggerFactory_ThrowsArgumentNullException()
    {
        var rawUrl = "https://api.example.com/cloudhsm/cloudhsms/test-cloudhsm-resource-%2Did/clients";

        Assert.Throws<ArgumentNullException>(() => new ClientsRequestBuilder(rawUrl, _mockRequestAdapter, null!));
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
            { "cloudhsm_resource_id", "test-id" }
        };
        var builder = new ClientsRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);
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

        _mockRequestAdapter.SendAsync<PaginatedCloudHSMClientList>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PaginatedCloudHSMClientList>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(async () => await builder.GetAsync());

        _mockLogger.Received(1).Log(LogLevel.Error, Arg.Any<EventId>(), Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^CloudHSMクライアント一覧取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T.*Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")), Arg.Any<ApiException>(), Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithApiError_ThrowsApiException()
    {
        var builder = new ClientsRequestBuilder(new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }, { "cloudhsm_resource_id", "test-hsm-id" }
        }, _mockRequestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError { ResponseStatusCode = 400 };
        ((IAdditionalDataHolder)expectedException).AdditionalData = new Dictionary<string, object>
        {
            { "error_code", "bad_request" }, { "error_msg", "invalid client" }, { "is_fatal", false }, { "serial", "9876543210" }, { "status", "400 Bad Request" }
        };
        _mockRequestAdapter.SendAsync<WrappedCreateCloudHSMClient>(Arg.Any<RequestInformation>(), Arg.Any<ParsableFactory<WrappedCreateCloudHSMClient>>(), Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(), Arg.Any<CancellationToken>()).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(() => builder.PostAsync(new WrappedCreateCloudHSMClient { Client = new CreateCloudHSMClient { Certificate = "test-certificate", Name = "test-client" } }));

        _mockLogger.Received(1).Log(LogLevel.Error, Arg.Any<EventId>(), Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^CloudHSMクライアント作成のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 400\], \[error_code, bad_request\], \[error_msg, invalid client\], \[is_fatal, False\], \[serial, 9876543210\], \[status, 400 Bad Request\]$")), Arg.Any<ApiException>(), Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncのリクエストボディに必須プロパティがない場合にバリデーションエラーと警告ログが出力されることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithMissingClient_ThrowsValidationExceptionAndLogsWarning()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "cloudhsm_resource_id", "test-cloudhsm-resource-id" },
        };
        var builder = new ClientsRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(new WrappedCreateCloudHSMClient()));

        Assert.Equal("Clientが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("Clientが入力されていません。");
        await _mockRequestAdapter.DidNotReceive().SendAsync<WrappedCreateCloudHSMClient>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedCreateCloudHSMClient>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// GetAsyncで予期しないエラーが発生した場合に例外を再スローし、エラーログが出力されることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithOtherError_ThrowsExceptionAndLogsError()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "cloudhsm_resource_id", "test-cloudhsm-resource-id" },
        };
        var builder = new ClientsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var mockException = new Exception("Test exception");
        requestAdapter.SendAsync<PaginatedCloudHSMClientList>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PaginatedCloudHSMClientList>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .ThrowsAsync(mockException);

        var exception = await Assert.ThrowsAsync<Exception>(async () => await builder.GetAsync());

        Assert.Equal("Test exception", exception.Message);

        _mockLogger.Received(1).Log(LogLevel.Error, Arg.Any<EventId>(), Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^CloudHSMクライアント一覧の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T.*Z$")), Arg.Any<Exception>(), Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncで予期しないエラーが発生した場合に例外を再スローし、エラーログが出力されることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithOtherError_ThrowsExceptionAndLogsError()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "cloudhsm_resource_id", "test-cloudhsm-resource-id" },
        };
        var builder = new ClientsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var mockException = new Exception("Test exception");
        requestAdapter.SendAsync<WrappedCreateCloudHSMClient>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<WrappedCreateCloudHSMClient>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .ThrowsAsync(mockException);

        var exception = await Assert.ThrowsAsync<Exception>(async () => await builder.PostAsync(new WrappedCreateCloudHSMClient
        {
            Client = new CreateCloudHSMClient
            {
                Certificate = "test-certificate",
                Name = "test-client"
            }
        }));

        Assert.Equal("Test exception", exception.Message);

        _mockLogger.Received(1).Log(LogLevel.Error, Arg.Any<EventId>(), Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^CloudHSMクライアントの作成中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T.*Z$")), Arg.Any<Exception>(), Arg.Any<Func<object, Exception?, string>>());
    }
}
