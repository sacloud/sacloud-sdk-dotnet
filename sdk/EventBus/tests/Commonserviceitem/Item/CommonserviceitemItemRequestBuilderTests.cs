using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.EventBus.Commonserviceitem.Item;
using Sacloud.Sdk.EventBus.Commonserviceitem.Item.Eventbus;
using Sacloud.Sdk.EventBus.Models;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.EventBus.Tests.Commonserviceitem.Item;

/// <summary>
/// CommonserviceitemItemRequestBuilderのテスト
/// </summary>
public class CommonserviceitemItemRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<CommonserviceitemItemRequestBuilder> _mockLogger;

    public CommonserviceitemItemRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<CommonserviceitemItemRequestBuilder>>();
        _mockLoggerFactory.CreateLogger<CommonserviceitemItemRequestBuilder>().Returns(_mockLogger);
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
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };

        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var rawUrl = "https://api.example.com/commonserviceitem/test-id";

        var builder = new CommonserviceitemItemRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// Eventbusプロパティが正しくEventbusRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Eventbus_ReturnsEventbusRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var eventbus = builder.Eventbus;

        Assert.NotNull(eventbus);
        Assert.IsType<EventbusRequestBuilder>(eventbus);
    }

    /// <summary>
    /// Eventbusプロパティが毎回新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Eventbus_ReturnsNewInstance_OnEachAccess()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var eventbus1 = builder.Eventbus;
        var eventbus2 = builder.Eventbus;

        Assert.NotSame(eventbus1, eventbus2);
    }

    /// <summary>
    /// ToDeleteRequestInformationが正しいRequestInformationを返すことを確認
    /// </summary>
    [Fact]
    public void ToDeleteRequestInformation_ReturnsValidRequestInformation()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToDeleteRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.DELETE, requestInfo.HttpMethod);
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// ToGetRequestInformationが正しいRequestInformationを返すことを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_ReturnsValidRequestInformation()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.GET, requestInfo.HttpMethod);
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// ToPutRequestInformationが正しいRequestInformationを返すことを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_ReturnsValidRequestInformation()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateCommonServiceItemRequest();

        var requestInfo = builder.ToPutRequestInformation(body);

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.PUT, requestInfo.HttpMethod);
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// ToPutRequestInformationがnullのbodyで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_WithNullBody_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.Throws<ArgumentNullException>(() => builder.ToPutRequestInformation(null!));
    }

    /// <summary>
    /// DeleteAsyncのパスパラメータにidがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("id", "")]
    [InlineData("id", " ")]
    [InlineData("id", "　")]
    public async Task DeleteAsync_WithInvalidPathParameters_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { pathParameterKey, pathParameterValue }
        };
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new CommonserviceitemDeleteResponse();

        requestAdapter.SendAsync<CommonserviceitemDeleteResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CommonserviceitemDeleteResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.DeleteAsync();
        });

        Assert.Equal("リソースIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("リソースIDを入力してください。");
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
            { "id", "test-id" }
        };
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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

        requestAdapter.SendAsync<CommonserviceitemDeleteResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CommonserviceitemDeleteResponse>>(),
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
                @"^実行設定・スケジュール・トリガー削除のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
            { "id", "test-id" }

        };
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync<CommonserviceitemDeleteResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CommonserviceitemDeleteResponse>>(),
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
                @"^実行設定・スケジュール・トリガーの削除中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// DeleteAsyncが正しくRequestAdapterを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_CallsRequestAdapterSendAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new CommonserviceitemDeleteResponse();
        requestAdapter.SendAsync<CommonserviceitemDeleteResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CommonserviceitemDeleteResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var response = await builder.DeleteAsync();

        Assert.NotNull(response);
        await requestAdapter.Received(1).SendAsync<CommonserviceitemDeleteResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CommonserviceitemDeleteResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
        Assert.Equal(expectedResponse, response);
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^実行設定・スケジュール・トリガーの削除を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^実行設定・スケジュール・トリガーの削除が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncのパスパラメータにidがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("id", "")]
    [InlineData("id", " ")]
    [InlineData("id", "　")]
    public async Task GetAsync_WithInvalidPathParameters_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { pathParameterKey, pathParameterValue }
        };
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new CommonserviceitemGetResponse();

        requestAdapter.SendAsync<CommonserviceitemGetResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CommonserviceitemGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync();
        });

        Assert.Equal("リソースIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("リソースIDを入力してください。");
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
            { "id", "test-id" }
        };
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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

        requestAdapter.SendAsync<CommonserviceitemGetResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CommonserviceitemGetResponse>>(),
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
                @"^実行設定・スケジュール・トリガー取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
            { "id", "test-id" }

        };
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync<CommonserviceitemGetResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CommonserviceitemGetResponse>>(),
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
                @"^実行設定・スケジュール・トリガーの取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncが正しくRequestAdapterを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_CallsRequestAdapterSendAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new CommonserviceitemGetResponse();
        requestAdapter.SendAsync<CommonserviceitemGetResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CommonserviceitemGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var response = await builder.GetAsync();

        Assert.NotNull(response);
        await requestAdapter.Received(1).SendAsync<CommonserviceitemGetResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CommonserviceitemGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
        Assert.Equal(expectedResponse, response);
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^実行設定・スケジュール・トリガーの取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^実行設定・スケジュール・トリガーの取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PutAsyncのパスパラメータにidがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("id", "")]
    [InlineData("id", " ")]
    [InlineData("id", "　")]
    public async Task PutAsync_WithInvalidPathParameters_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { pathParameterKey, pathParameterValue }
        };
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateCommonServiceItemRequest();
        var expectedResponse = new CommonserviceitemPutResponse();

        requestAdapter.SendAsync<CommonserviceitemPutResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CommonserviceitemPutResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(body);
        });

        Assert.Equal("リソースIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("リソースIDを入力してください。");
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
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
    [InlineData("CommonServiceItem")]
    [InlineData("CommonServiceItem.Settings.Destination")]
    [InlineData("CommonServiceItem.Settings.Parameters")]
    [InlineData("CommonServiceItem.Settings.ProcessConfigurationID")]
    [InlineData("CommonServiceItem.Settings.StartsAt")]
    [InlineData("CommonServiceItem.Settings.Source")]
    [InlineData("CommonServiceItem.Settings.Conditions.Key")]
    [InlineData("CommonServiceItem.Settings.Conditions.Op")]
    [InlineData("CommonServiceItem.Settings.Conditions.Values")]
    [InlineData("CommonServiceItem.Provider.Class")]
    public async Task PutAsync_WithNullParameter_ThrowsValidationException(string invalidParameterName)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateCommonServiceItemRequest
        {
            CommonServiceItem = invalidParameterName == "CommonServiceItem" ? null : new UpdateCommonServiceItemRequest_CommonServiceItem
            {
                Settings = new SettingsRequest
                {
                    ProcessConfigurationSettings = new ProcessConfigurationSettings
                    {
                        Destination = invalidParameterName == "CommonServiceItem.Settings.Destination" ? null : ProcessConfigurationSettings_Destination.Simplenotification,
                        Parameters = invalidParameterName == "CommonServiceItem.Settings.Parameters" ? null : "test-parameters"
                    },
                    ScheduleSettingsRequest = new ScheduleSettingsRequest
                    {
                        ProcessConfigurationID = invalidParameterName == "CommonServiceItem.Settings.ProcessConfigurationID" ? null : "test-process-configuration-id",
                        StartsAt = invalidParameterName == "CommonServiceItem.Settings.StartsAt" ? null : DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                    },
                    TriggerSettings = new TriggerSettings
                    {
                        Source = invalidParameterName == "CommonServiceItem.Settings.Source" ? null : "test-source",
                        Conditions = new List<TriggerSettings.TriggerSettings_Conditions>
                        {
                            new TriggerSettings.TriggerSettings_Conditions
                            {
                                TriggerConditionEq = new TriggerConditionEq
                                {
                                    Key = invalidParameterName == "CommonServiceItem.Settings.Conditions.Key" ? null : "testkey",
                                    Op = invalidParameterName == "CommonServiceItem.Settings.Conditions.Op" ? null : TriggerConditionEq_Op.Eq,
                                    Values = invalidParameterName == "CommonServiceItem.Settings.Conditions.Values" ? null : new List<string> { "test-value1" }
                                }
                            }
                        },
                        ProcessConfigurationID = "test-process-configuration-id"
                    }
                },
                Provider = new Provider
                {
                    Class = invalidParameterName == "CommonServiceItem.Provider.Class" ? null : Provider_Class.Eventbusprocessconfiguration
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
    /// PutAsyncのリクエストボディのパラメータの値がEnumに含まれていない場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("CommonServiceItem.Settings.Destination")]
    [InlineData("CommonServiceItem.Settings.RecurringUnit")]
    [InlineData("CommonServiceItem.Settings.Conditions.Op")]
    [InlineData("CommonServiceItem.Provider.Class")]
    public async Task PutAsync_WithOutOfEnumRequestBodyParameter_ThrowsValidationException(string invalidParameterName)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateCommonServiceItemRequest
        {
            CommonServiceItem = new UpdateCommonServiceItemRequest_CommonServiceItem
            {
                Name = "test-name",
                Settings = new SettingsRequest
                {
                    ProcessConfigurationSettings = new ProcessConfigurationSettings
                    {
                        Destination = invalidParameterName == "CommonServiceItem.Settings.Destination" ? (ProcessConfigurationSettings_Destination)999 : ProcessConfigurationSettings_Destination.Simplenotification,
                        Parameters = "test-parameters"
                    },
                    ScheduleSettingsRequest = new ScheduleSettingsRequest
                    {
                        ProcessConfigurationID = "test-process-configuration-id",
                        RecurringUnit = invalidParameterName == "CommonServiceItem.Settings.RecurringUnit" ? (ScheduleSettingsRequest_RecurringUnit)999 : ScheduleSettingsRequest_RecurringUnit.Day,
                        StartsAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                    },
                    TriggerSettings = new TriggerSettings
                    {
                        Source = "test-source",
                        Conditions = new List<TriggerSettings.TriggerSettings_Conditions>
                        {
                            new TriggerSettings.TriggerSettings_Conditions
                            {
                                TriggerConditionEq = new TriggerConditionEq
                                {
                                    Key = "testkey",
                                    Op = invalidParameterName == "CommonServiceItem.Settings.Conditions.Op" ? (TriggerConditionEq_Op)999 : TriggerConditionEq_Op.Eq,
                                    Values = new List<string> { "test-value" }
                                }
                            }
                        },
                        ProcessConfigurationID = "test-process-configuration-id"
                    }
                },
                Provider = new Provider
                {
                    Class = invalidParameterName == "CommonServiceItem.Provider.Class" ? (Provider_Class)999 : Provider_Class.Eventbusprocessconfiguration
                }
            }
        };
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(body);
        });
        Assert.Equal($"{invalidParameterName}はEnumに含まれる値を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}はEnumに含まれる値を入力してください。");
    }

    /// <summary>
    /// PutAsyncのパスパラメータの文字数が正規表現パターンに一致しなかった場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("data")]
    [InlineData("InvalidKey")]
    [InlineData("無効なキー")]
    [InlineData("invalid-key")]
    [InlineData("invalid_key")]
    [InlineData("123456789012345678901")]
    public async Task PutAsync_WithInvalidRegexPatternRequestBodyParameter_ThrowsValidationException(string invalidParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateCommonServiceItemRequest
        {
            CommonServiceItem = new UpdateCommonServiceItemRequest_CommonServiceItem
            {
                Name = "test-name",
                Settings = new SettingsRequest
                {
                    TriggerSettings = new TriggerSettings
                    {
                        Source = "test-source",
                        Conditions = new List<TriggerSettings.TriggerSettings_Conditions>
                        {
                            new TriggerSettings.TriggerSettings_Conditions
                            {
                                TriggerConditionEq = new TriggerConditionEq
                                {
                                    Key = invalidParameterValue,
                                    Op = TriggerConditionEq_Op.Eq,
                                    Values = new List<string> { "test-value1" }
                                }
                            }
                        },
                        ProcessConfigurationID = "test-process-configuration-id"
                    }
                },
                Provider = new Provider
                {
                    Class = Provider_Class.Eventbustrigger
                }
            }
        };
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(body);
        });
        Assert.Equal("CommonServiceItem.Settings.Conditions.Keyの形式が正しくありません。", exception.Message);
        _mockLogger.Received(1).LogWarning("CommonServiceItem.Settings.Conditions.Keyの形式が正しくありません。");
    }

    /// <summary>
    /// PutAsyncのリクエストボディのパラメータが特定の要素数でない配列の場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    public async Task PutAsync_WithInvalidElementCountRequestBodyParameter_ThrowsValidationException(int invalidElementCount)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateCommonServiceItemRequest
        {
            CommonServiceItem = new UpdateCommonServiceItemRequest_CommonServiceItem
            {
                Name = "test-name",
                Settings = new SettingsRequest
                {
                    TriggerSettings = new TriggerSettings
                    {
                        Source = "test-source",
                        Conditions = new List<TriggerSettings.TriggerSettings_Conditions>
                        {
                            new TriggerSettings.TriggerSettings_Conditions
                            {
                                TriggerConditionEq = new TriggerConditionEq
                                {
                                    Key = "testkey",
                                    Op = TriggerConditionEq_Op.Eq,
                                    Values = Enumerable.Range(0, invalidElementCount).Select(_ => "test-value").ToList()
                                }
                            }
                        },
                        ProcessConfigurationID = "test-process-configuration-id"
                    }
                },
                Provider = new Provider
                {
                    Class = Provider_Class.Eventbustrigger
                }
            }
        };
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(body);
        });
        Assert.Equal("CommonServiceItem.Settings.Conditions.Valuesの要素数は1にしてください。", exception.Message);
        _mockLogger.Received(1).LogWarning("CommonServiceItem.Settings.Conditions.Valuesの要素数は1にしてください。");
    }

    /// <summary>
    /// PutAsyncのリクエストボディのパラメータが空配列の場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithEmptyArrayRequestBodyParameter_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateCommonServiceItemRequest
        {
            CommonServiceItem = new UpdateCommonServiceItemRequest_CommonServiceItem
            {
                Name = "test-name",
                Settings = new SettingsRequest
                {
                    TriggerSettings = new TriggerSettings
                    {
                        Source = "test-source",
                        Conditions = new List<TriggerSettings.TriggerSettings_Conditions>
                        {
                            new TriggerSettings.TriggerSettings_Conditions
                            {
                                TriggerConditionIn = new TriggerConditionIn
                                {
                                    Key = "testkey",
                                    Op = TriggerConditionIn_Op.In,
                                    Values = new List<string>()
                                }
                            }
                        },
                        ProcessConfigurationID = "test-process-configuration-id"
                    }
                },
                Provider = new Provider
                {
                    Class = Provider_Class.Eventbustrigger
                }
            }
        };
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(body);
        });
        Assert.Equal("CommonServiceItem.Settings.Conditions.Valuesが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("CommonServiceItem.Settings.Conditions.Valuesが入力されていません。");
    }

    /// <summary>
    /// PutAsyncのリクエストボディの複数パラメータが無効な値の場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithMultipleInvalidParams_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateCommonServiceItemRequest
        {
            CommonServiceItem = new UpdateCommonServiceItemRequest_CommonServiceItem
            {
                Settings = new SettingsRequest
                {
                    TriggerSettings = new TriggerSettings
                    {
                        Source = "",
                        Conditions = new List<TriggerSettings.TriggerSettings_Conditions>
                        {
                            new TriggerSettings.TriggerSettings_Conditions
                            {
                                TriggerConditionEq = new TriggerConditionEq
                                {
                                    Key = "invalid-key",
                                    Op = (TriggerConditionEq_Op)999,
                                    Values = new List<string>()
                                }
                            }
                        },
                        ProcessConfigurationID = string.Empty
                    }
                },
                Provider = new Provider
                {
                    Class = (Provider_Class)999
                }
            }
        };
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(body);
        });

        var expectedMessage = """
            CommonServiceItem.Provider.ClassはEnumに含まれる値を入力してください。
            CommonServiceItem.Settings.ProcessConfigurationIDが入力されていません。
            CommonServiceItem.Settings.Sourceが入力されていません。
            CommonServiceItem.Settings.Conditions.Keyの形式が正しくありません。
            CommonServiceItem.Settings.Conditions.OpはEnumに含まれる値を入力してください。
            CommonServiceItem.Settings.Conditions.Valuesの要素数は1にしてください。
            """;
        Assert.Equal(expectedMessage.ReplaceLineEndings(), exception.Message.ReplaceLineEndings());
        _mockLogger.Received(1).LogWarning("CommonServiceItem.Provider.ClassはEnumに含まれる値を入力してください。");
        _mockLogger.Received(1).LogWarning("CommonServiceItem.Settings.ProcessConfigurationIDが入力されていません。");
        _mockLogger.Received(1).LogWarning("CommonServiceItem.Settings.Sourceが入力されていません。");
        _mockLogger.Received(1).LogWarning("CommonServiceItem.Settings.Conditions.Keyの形式が正しくありません。");
        _mockLogger.Received(1).LogWarning("CommonServiceItem.Settings.Conditions.OpはEnumに含まれる値を入力してください。");
        _mockLogger.Received(1).LogWarning("CommonServiceItem.Settings.Conditions.Valuesの要素数は1にしてください。");
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
            { "id", "test-id" }
        };
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateCommonServiceItemRequest
        {
            CommonServiceItem = new UpdateCommonServiceItemRequest_CommonServiceItem
            {
                Name = "test-name",
                Settings = new SettingsRequest
                {
                    ScheduleSettingsRequest = new ScheduleSettingsRequest
                    {
                        ProcessConfigurationID = "test-process-configuration-id",
                        Crontab = "0 0 * * *",
                        StartsAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                    }
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

        requestAdapter.SendAsync<CommonserviceitemPutResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CommonserviceitemPutResponse>>(),
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
                @"^実行設定・スケジュール・トリガー更新のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
            { "id", "test-id" }
        };
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateCommonServiceItemRequest
        {
            CommonServiceItem = new UpdateCommonServiceItemRequest_CommonServiceItem
            {
                Name = "test-name",
                Settings = new SettingsRequest
                {
                    ScheduleSettingsRequest = new ScheduleSettingsRequest
                    {
                        ProcessConfigurationID = "test-process-configuration-id",
                        Crontab = "0 0 * * *",
                        StartsAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                    }
                }
            }
        };
        var expectedException = new Exception();

        requestAdapter.SendAsync<CommonserviceitemPutResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CommonserviceitemPutResponse>>(),
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
                @"^実行設定・スケジュール・トリガーの更新中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PutAsyncが正しくRequestAdapterを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_CallsRequestAdapterSendAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateCommonServiceItemRequest
        {
            CommonServiceItem = new UpdateCommonServiceItemRequest_CommonServiceItem
            {
                Name = "test-name",
                Settings = new SettingsRequest
                {
                    ScheduleSettingsRequest = new ScheduleSettingsRequest
                    {
                        ProcessConfigurationID = "test-process-configuration-id",
                        Crontab = "0 0 * * *",
                        StartsAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                    }
                }
            }
        };
        var expectedResponse = new CommonserviceitemPutResponse();
        requestAdapter.SendAsync<CommonserviceitemPutResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CommonserviceitemPutResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var response = await builder.PutAsync(body);

        Assert.NotNull(response);
        await requestAdapter.Received(1).SendAsync<CommonserviceitemPutResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CommonserviceitemPutResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
        Assert.Equal(expectedResponse, response);
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^実行設定・スケジュール・トリガーの更新を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^実行設定・スケジュール・トリガーの更新が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// WithUrlが新しいCommonserviceitemItemRequestBuilderインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewCommonserviceitemItemRequestBuilderInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var rawUrl = "https://api.example.com/commonserviceitem/new-id";

        var newBuilder = builder.WithUrl(rawUrl);

        Assert.NotNull(newBuilder);
        Assert.IsType<CommonserviceitemItemRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }

    /// <summary>
    /// WithUrlが指定されたrawUrlを使用してRequestBuilderを生成することを確認
    /// </summary>
    [Fact]
    public void WithUrl_CreatesBuilderWithSpecifiedRawUrl()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var rawUrl = "https://api.example.com/commonserviceitem/custom-id";

        var newBuilder = builder.WithUrl(rawUrl);

        Assert.NotNull(newBuilder);
        var requestInfo = newBuilder.ToGetRequestInformation();
        Assert.NotNull(requestInfo);
    }

    /// <summary>
    /// WithUrlが元のRequestAdapterを保持することを確認
    /// </summary>
    [Fact]
    public void WithUrl_RetainsOriginalRequestAdapter()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var rawUrl = "https://api.example.com/commonserviceitem/new-id";

        var newBuilder = builder.WithUrl(rawUrl);

        var requestInfo = newBuilder.ToGetRequestInformation();
        Assert.NotNull(requestInfo);
    }
}
