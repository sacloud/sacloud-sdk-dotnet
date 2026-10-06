using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.MonitoringSuite.Management.Provisioning.Initialize;
using Sacloud.Sdk.MonitoringSuite.Models;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Management.Provisioning.Initialize;

/// <summary>
/// InitializeRequestBuilderのテスト
/// </summary>
public class InitializeRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<InitializeRequestBuilder> _mockLogger;

    public InitializeRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<InitializeRequestBuilder>>();
        _mockLoggerFactory.CreateLogger<InitializeRequestBuilder>().Returns(_mockLogger);
    }

    /// <summary>
    /// コンストラクタ（PathParameters版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };

        var builder = new InitializeRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";

        var builder = new InitializeRequestBuilder("https://api.example.com/management/provisioning/initialize", requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// ToPostRequestInformationが正しいAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_SetsAcceptHeader()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        requestAdapter.SerializationWriterFactory.Returns(
            new JsonSerializationWriterFactory());
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new InitializeRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ProvisioningCreateRequest();

        var requestInfo = builder.ToPostRequestInformation(body);

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
        Assert.True(requestInfo.Headers.ContainsKey("X-Sakura-Bigint-As-Int"));
        Assert.Contains("1", requestInfo.Headers["X-Sakura-Bigint-As-Int"]);
    }

    /// <summary>
    /// ToPostRequestInformationにnullボディを渡すと例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_WithNullBody_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new InitializeRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.Throws<ArgumentNullException>(() => builder.ToPostRequestInformation(null!));
    }

    /// <summary>
    /// WithUrlが新しいInitializeRequestBuilderインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new InitializeRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var newBuilder = builder.WithUrl("https://other.example.com/management/provisioning/initialize");

        Assert.NotNull(newBuilder);
        Assert.IsType<InitializeRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }

    /// <summary>
    /// PostAsyncでアダプターがnullを返した場合にnullを返すことを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_ReturnsNull_WhenAdapterReturnsNull()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        requestAdapter.SendAsync<global::Sacloud.Sdk.MonitoringSuite.Models.Provisioning>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<global::Sacloud.Sdk.MonitoringSuite.Models.Provisioning>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<global::Sacloud.Sdk.MonitoringSuite.Models.Provisioning?>(null));
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new InitializeRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var result = await builder.PostAsync(new ProvisioningCreateRequest());

        Assert.Null(result);
    }

    /// <summary>
    /// PostAsyncのリクエストボディパラメータにnullを入力した場合に例外をスローすることを確認
    /// </summary>
    [Theory]
    [InlineData("SystemExist")]
    [InlineData("UserExist")]
    public async Task PostAsync_WithNullRequestBodyParameter_ThrowsValidationException(string invalidParameterName)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new InitializeRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ProvisioningCreateRequest
        {
            Logs = new ProvisioningExistRequest
            {
                SystemExist = invalidParameterName == "SystemExist" ? null : false,
                UserExist = invalidParameterName == "UserExist" ? null : false
            },
            Metrics = new ProvisioningExistRequest
            {
                SystemExist = invalidParameterName == "SystemExist" ? null : false,
                UserExist = invalidParameterName == "UserExist" ? null : false
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));
        var expectedErrorMessage = string.Join(
            Environment.NewLine,
            $"{invalidParameterName}が入力されていません。",
            $"{invalidParameterName}が入力されていません。");
        Assert.Equal(expectedErrorMessage, exception.Message);
        _mockLogger.Received(2).LogWarning($"{invalidParameterName}が入力されていません。");
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
        var builder = new InitializeRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ProvisioningCreateRequest();
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

        requestAdapter.SendAsync<MonitoringSuite.Models.Provisioning>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<MonitoringSuite.Models.Provisioning>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(async () => await builder.PostAsync(body));

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^リソースプロビジョニングのAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var builder = new InitializeRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ProvisioningCreateRequest();
        var expectedException = new Exception();

        requestAdapter.SendAsync<MonitoringSuite.Models.Provisioning>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<MonitoringSuite.Models.Provisioning>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<Exception>(async () => await builder.PostAsync(body));

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^リソースのプロビジョニング中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncがステータスコード200で適切なログを出力することを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithStatusCode200_LogsCancellationResourceCreation()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new InitializeRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ProvisioningCreateRequest();
        var expectedResponse = new MonitoringSuite.Models.Provisioning();

        requestAdapter.SendAsync<MonitoringSuite.Models.Provisioning>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<MonitoringSuite.Models.Provisioning>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(callInfo =>
            {
                var requestInfo = callInfo.ArgAt<RequestInformation>(0);
                var responseHandlerOption = requestInfo.GetRequestOption<ResponseHandlerOption>();

                if (responseHandlerOption?.ResponseHandler is NativeResponseHandler handler)
                {
                    handler.Value = new HttpResponseMessage(HttpStatusCode.OK);
                }

                return expectedResponse;
            });

        var result = await builder.PostAsync(body);

        Assert.Equal(expectedResponse, result);

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^リソースのプロビジョニングを開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^既存のリソースが存在するためリソースの新規作成は行われませんでした。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncがステータスコード201で適切なログを出力することを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithStatusCode201_LogsQueueRegistrationCompletion()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new InitializeRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ProvisioningCreateRequest();
        var expectedResponse = new MonitoringSuite.Models.Provisioning();

        requestAdapter.SendAsync<MonitoringSuite.Models.Provisioning>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<MonitoringSuite.Models.Provisioning>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(callInfo =>
            {
                var requestInfo = callInfo.ArgAt<RequestInformation>(0);
                var responseHandlerOption = requestInfo.GetRequestOption<ResponseHandlerOption>();

                if (responseHandlerOption?.ResponseHandler is NativeResponseHandler handler)
                {
                    handler.Value = new HttpResponseMessage(HttpStatusCode.Created);
                }

                return expectedResponse;
            });

        var result = await builder.PostAsync(body);

        Assert.Equal(expectedResponse, result);

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^リソースのプロビジョニングを開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^新規リソースを作成しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 201\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncが正常時に200または202以外のステータスコードを受け取ったときに警告ログを出力することを確認
    /// </summary>
    [Theory]
    [InlineData(HttpStatusCode.Accepted)]
    [InlineData(HttpStatusCode.NotModified)]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async Task PostAsync_WithUnexpectedStatusCode_LogsWarning(HttpStatusCode statusCode)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new InitializeRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ProvisioningCreateRequest();
        var expectedResponse = new MonitoringSuite.Models.Provisioning();

        requestAdapter.SendAsync<MonitoringSuite.Models.Provisioning>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<MonitoringSuite.Models.Provisioning>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(callInfo =>
            {
                var requestInfo = callInfo.ArgAt<RequestInformation>(0);
                var responseHandlerOption = requestInfo.GetRequestOption<ResponseHandlerOption>();

                if (responseHandlerOption?.ResponseHandler is NativeResponseHandler handler)
                {
                    handler.Value = new HttpResponseMessage(statusCode);
                }

                return expectedResponse;
            });

        var result = await builder.PostAsync(body);

        Assert.Equal(expectedResponse, result);

        // 警告ログの検証（予期しないステータスコード）
        _mockLogger.Received(1).Log(
            LogLevel.Warning,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    $@"^リソースのプロビジョニングで予期しないステータスコードを受信しました。 \d{{4}}-\d{{2}}-\d{{2}}T\d{{2}}:\d{{2}}:\d{{2}}\.\d+Z \[StatusCode, {(int)statusCode}\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }
}
