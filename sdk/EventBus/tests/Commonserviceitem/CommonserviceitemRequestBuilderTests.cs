using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.EventBus.Commonserviceitem;
using Sacloud.Sdk.EventBus.Commonserviceitem.Item;
using Sacloud.Sdk.EventBus.Models;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.EventBus.Tests.Commonserviceitem;

/// <summary>
/// CommonserviceitemRequestBuilderのテスト
/// </summary>
public class CommonserviceitemRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<CommonserviceitemRequestBuilder> _mockLogger;

    public CommonserviceitemRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<CommonserviceitemRequestBuilder>>();
        _mockLoggerFactory.CreateLogger<CommonserviceitemRequestBuilder>().Returns(_mockLogger);
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

        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var rawUrl = "https://api.example.com/commonserviceitem";

        var builder = new CommonserviceitemRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// インデクサーが正しいCommonserviceitemItemRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_ReturnsCommonserviceitemItemRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var itemBuilder = builder["test-id"];

        Assert.NotNull(itemBuilder);
        Assert.IsType<CommonserviceitemItemRequestBuilder>(itemBuilder);
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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.GET, requestInfo.HttpMethod);
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// ToPostRequestInformationが正しいRequestInformationを返すことを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_ReturnsValidRequestInformation()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateCommonServiceItemRequest();

        var requestInfo = builder.ToPostRequestInformation(body);

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.POST, requestInfo.HttpMethod);
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// ToPostRequestInformationがnullのbodyで例外をスローすることを確認
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
        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.Throws<ArgumentNullException>(() => builder.ToPostRequestInformation(null!));
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
        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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

        requestAdapter.SendAsync<EventBus.Commonserviceitem.CommonserviceitemGetResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<EventBus.Commonserviceitem.CommonserviceitemGetResponse>>(),
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
                @"^実行設定・スケジュール・トリガー一覧取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync<EventBus.Commonserviceitem.CommonserviceitemGetResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<EventBus.Commonserviceitem.CommonserviceitemGetResponse>>(),
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
                @"^実行設定・スケジュール・トリガーの一覧取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new EventBus.Commonserviceitem.CommonserviceitemGetResponse();
        requestAdapter.SendAsync<EventBus.Commonserviceitem.CommonserviceitemGetResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<EventBus.Commonserviceitem.CommonserviceitemGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var response = await builder.GetAsync();

        Assert.NotNull(response);
        await requestAdapter.Received(1).SendAsync<EventBus.Commonserviceitem.CommonserviceitemGetResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<EventBus.Commonserviceitem.CommonserviceitemGetResponse>>(),
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
                    @"^実行設定・スケジュール・トリガーの一覧取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^実行設定・スケジュール・トリガーの一覧取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
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
        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
    [InlineData("CommonServiceItem")]
    [InlineData("CommonServiceItem.Name")]
    [InlineData("CommonServiceItem.Settings")]
    [InlineData("CommonServiceItem.Settings.Destination")]
    [InlineData("CommonServiceItem.Settings.Parameters")]
    [InlineData("CommonServiceItem.Settings.ProcessConfigurationID")]
    [InlineData("CommonServiceItem.Settings.StartsAt")]
    [InlineData("CommonServiceItem.Settings.Source")]
    [InlineData("CommonServiceItem.Settings.Conditions.Key")]
    [InlineData("CommonServiceItem.Settings.Conditions.Op")]
    [InlineData("CommonServiceItem.Settings.Conditions.Values")]
    [InlineData("CommonServiceItem.Provider")]
    [InlineData("CommonServiceItem.Provider.Class")]
    public async Task PostAsync_WithNullParameter_ThrowsValidationException(string invalidParameterName)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateCommonServiceItemRequest
        {
            CommonServiceItem = invalidParameterName == "CommonServiceItem" ? null : new CreateCommonServiceItemRequest_CommonServiceItem
            {
                Name = invalidParameterName == "CommonServiceItem.Name" ? null : "test-name",
                Settings = invalidParameterName == "CommonServiceItem.Settings" ? null : new SettingsRequest
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
                Provider = invalidParameterName == "CommonServiceItem.Provider" ? null : new Provider
                {
                    Class = invalidParameterName == "CommonServiceItem.Provider.Class" ? null : Provider_Class.Eventbusprocessconfiguration
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
    /// PostAsyncのリクエストボディのパラメータの値がEnumに含まれていない場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("CommonServiceItem.Settings.Destination")]
    [InlineData("CommonServiceItem.Settings.RecurringUnit")]
    [InlineData("CommonServiceItem.Settings.Conditions.Op")]
    [InlineData("CommonServiceItem.Provider.Class")]
    public async Task PostAsync_WithOutOfEnumRequestBodyParameter_ThrowsValidationException(string invalidParameterName)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateCommonServiceItemRequest
        {
            CommonServiceItem = new CreateCommonServiceItemRequest_CommonServiceItem
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
            await builder.PostAsync(body);
        });
        Assert.Equal($"{invalidParameterName}はEnumに含まれる値を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}はEnumに含まれる値を入力してください。");
    }

    /// <summary>
    /// PostAsyncのパスパラメータの文字数が正規表現パターンに一致しなかった場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("data")]
    [InlineData("InvalidKey")]
    [InlineData("無効なキー")]
    [InlineData("invalid-key")]
    [InlineData("invalid_key")]
    [InlineData("123456789012345678901")]
    public async Task PostAsync_WithInvalidRegexPatternRequestBodyParameter_ThrowsValidationException(string invalidParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateCommonServiceItemRequest
        {
            CommonServiceItem = new CreateCommonServiceItemRequest_CommonServiceItem
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
            await builder.PostAsync(body);
        });
        Assert.Equal("CommonServiceItem.Settings.Conditions.Keyの形式が正しくありません。", exception.Message);
        _mockLogger.Received(1).LogWarning("CommonServiceItem.Settings.Conditions.Keyの形式が正しくありません。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのパラメータが特定の要素数でない配列の場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    public async Task PostAsync_WithInvalidElementCountRequestBodyParameter_ThrowsValidationException(int invalidElementCount)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateCommonServiceItemRequest
        {
            CommonServiceItem = new CreateCommonServiceItemRequest_CommonServiceItem
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
            await builder.PostAsync(body);
        });
        Assert.Equal("CommonServiceItem.Settings.Conditions.Valuesの要素数は1にしてください。", exception.Message);
        _mockLogger.Received(1).LogWarning("CommonServiceItem.Settings.Conditions.Valuesの要素数は1にしてください。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのパラメータが空配列の場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithEmptyArrayRequestBodyParameter_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateCommonServiceItemRequest
        {
            CommonServiceItem = new CreateCommonServiceItemRequest_CommonServiceItem
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
            await builder.PostAsync(body);
        });
        Assert.Equal("CommonServiceItem.Settings.Conditions.Valuesが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("CommonServiceItem.Settings.Conditions.Valuesが入力されていません。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディの複数パラメータが無効な値の場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithMultipleInvalidParams_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateCommonServiceItemRequest
        {
            CommonServiceItem = new CreateCommonServiceItemRequest_CommonServiceItem
            {
                Name = null,
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
            await builder.PostAsync(body);
        });

        var expectedMessage = """
            CommonServiceItem.Nameが入力されていません。
            CommonServiceItem.Provider.ClassはEnumに含まれる値を入力してください。
            CommonServiceItem.Settings.ProcessConfigurationIDが入力されていません。
            CommonServiceItem.Settings.Sourceが入力されていません。
            CommonServiceItem.Settings.Conditions.Keyの形式が正しくありません。
            CommonServiceItem.Settings.Conditions.OpはEnumに含まれる値を入力してください。
            CommonServiceItem.Settings.Conditions.Valuesの要素数は1にしてください。
            """;
        Assert.Equal(expectedMessage.ReplaceLineEndings(), exception.Message.ReplaceLineEndings());
        _mockLogger.Received(1).LogWarning("CommonServiceItem.Nameが入力されていません。");
        _mockLogger.Received(1).LogWarning("CommonServiceItem.Provider.ClassはEnumに含まれる値を入力してください。");
        _mockLogger.Received(1).LogWarning("CommonServiceItem.Settings.ProcessConfigurationIDが入力されていません。");
        _mockLogger.Received(1).LogWarning("CommonServiceItem.Settings.Sourceが入力されていません。");
        _mockLogger.Received(1).LogWarning("CommonServiceItem.Settings.Conditions.Keyの形式が正しくありません。");
        _mockLogger.Received(1).LogWarning("CommonServiceItem.Settings.Conditions.OpはEnumに含まれる値を入力してください。");
        _mockLogger.Received(1).LogWarning("CommonServiceItem.Settings.Conditions.Valuesの要素数は1にしてください。");
    }

    /// <summary>
    /// PostAsyncでApiErrorによるAPIエラーが発生した場合に例外をスローすることを確認
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
        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateCommonServiceItemRequest
        {
            CommonServiceItem = new CreateCommonServiceItemRequest_CommonServiceItem
            {
                Name = "test-name",
                Settings = new SettingsRequest
                {
                    ProcessConfigurationSettings = new ProcessConfigurationSettings
                    {
                        Destination = ProcessConfigurationSettings_Destination.Simplenotification,
                        Parameters = "test-parameters"
                    }
                },
                Provider = new Provider
                {
                    Class = Provider_Class.Eventbusprocessconfiguration
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

        requestAdapter.SendAsync<CommonserviceitemPostResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CommonserviceitemPostResponse>>(),
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
                @"^実行設定・スケジュール・トリガー作成のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateCommonServiceItemRequest
        {
            CommonServiceItem = new CreateCommonServiceItemRequest_CommonServiceItem
            {
                Name = "test-name",
                Settings = new SettingsRequest
                {
                    ProcessConfigurationSettings = new ProcessConfigurationSettings
                    {
                        Destination = ProcessConfigurationSettings_Destination.Simplenotification,
                        Parameters = "test-parameters"
                    }
                },
                Provider = new Provider
                {
                    Class = Provider_Class.Eventbusprocessconfiguration
                }
            }
        };
        var expectedException = new Exception();

        requestAdapter.SendAsync<CommonserviceitemPostResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CommonserviceitemPostResponse>>(),
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
                @"^実行設定・スケジュール・トリガーの作成中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncが正しくRequestAdapterを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_CallsRequestAdapterSendAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new CreateCommonServiceItemRequest
        {
            CommonServiceItem = new CreateCommonServiceItemRequest_CommonServiceItem
            {
                Name = "test-name",
                Settings = new SettingsRequest
                {
                    ProcessConfigurationSettings = new ProcessConfigurationSettings
                    {
                        Destination = ProcessConfigurationSettings_Destination.Simplenotification,
                        Parameters = "test-parameters"
                    }
                },
                Provider = new Provider
                {
                    Class = Provider_Class.Eventbusprocessconfiguration
                }
            }
        };
        var expectedResponse = new CommonserviceitemPostResponse();
        requestAdapter.SendAsync<CommonserviceitemPostResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CommonserviceitemPostResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var response = await builder.PostAsync(body);

        Assert.NotNull(response);
        await requestAdapter.Received(1).SendAsync<CommonserviceitemPostResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CommonserviceitemPostResponse>>(),
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
                    @"^実行設定・スケジュール・トリガーの作成を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^実行設定・スケジュール・トリガーの作成が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 201\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// WithUrlが新しいCommonserviceitemRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewCommonserviceitemRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var newUrl = "https://api.example.com/commonserviceitem/new";

        var newBuilder = builder.WithUrl(newUrl);

        Assert.NotNull(newBuilder);
        Assert.IsType<CommonserviceitemRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }

    /// <summary>
    /// ToGetRequestInformationがProviderClass指定なしで正しいRequestInformationを返すことを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_WithoutProviderClass_ReturnsValidRequestInformation()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.GET, requestInfo.HttpMethod);
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
        Assert.DoesNotContain("Filter", requestInfo.URI.ToString());
    }

    /// <summary>
    /// ToGetRequestInformationが無効なProviderClass値を指定した場合にArgumentExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_WithInvalidProviderClass_ThrowsArgumentException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = Assert.Throws<ArgumentException>(() =>
        {
            builder.ToGetRequestInformation(config =>
            {
                config.QueryParameters = new CommonserviceitemRequestBuilder.CommonserviceitemRequestBuilderGetQueryParameters
                {
                    ProviderClass = (Provider_Class)999
                };
            });
        });

        Assert.Contains("Invalid provider class", exception.Message);
    }

    /// <summary>
    /// GetAsyncがProviderClassフィルタを使用して正しく実行されることを確認
    /// </summary>
    [Theory]
    [InlineData(Provider_Class.Eventbusschedule, "eventbusschedule")]
    [InlineData(Provider_Class.Eventbustrigger, "eventbustrigger")]
    [InlineData(Provider_Class.Eventbusprocessconfiguration, "eventbusprocessconfiguration")]
    public async Task GetAsync_WithProviderClassFilter_ExecutesSuccessfully(Provider_Class providerClass, string expectedClassName)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new EventBus.Commonserviceitem.CommonserviceitemGetResponse();
        requestAdapter.SendAsync<EventBus.Commonserviceitem.CommonserviceitemGetResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<EventBus.Commonserviceitem.CommonserviceitemGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var response = await builder.GetAsync(config =>
        {
            config.QueryParameters = new CommonserviceitemRequestBuilder.CommonserviceitemRequestBuilderGetQueryParameters
            {
                ProviderClass = providerClass
            };
        });

        Assert.NotNull(response);
        Assert.Equal(expectedResponse, response);

        // RequestInformationのURIに期待されるフィルタ全体が含まれていることを確認
        var expectedFilterJson = $"{{\"Filter\":{{\"Provider.Class\":\"{expectedClassName}\"}}}}";
        await requestAdapter.Received(1).SendAsync<EventBus.Commonserviceitem.CommonserviceitemGetResponse>(
            Arg.Is<RequestInformation>(ri =>
                ri.URI.ToString().Contains("/commonserviceitem?") &&
                Uri.UnescapeDataString(ri.URI.ToString()).Contains(expectedFilterJson)),
            Arg.Any<ParsableFactory<EventBus.Commonserviceitem.CommonserviceitemGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// GetAsyncがProviderClassフィルタ指定時にカスタムヘッダーがRequestInformationに反映されることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithProviderClassFilterAndCustomHeader_ReflectsCustomHeaderInRequestInformation()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new EventBus.Commonserviceitem.CommonserviceitemGetResponse();
        requestAdapter.SendAsync<EventBus.Commonserviceitem.CommonserviceitemGetResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<EventBus.Commonserviceitem.CommonserviceitemGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        await builder.GetAsync(config =>
        {
            config.QueryParameters = new CommonserviceitemRequestBuilder.CommonserviceitemRequestBuilderGetQueryParameters
            {
                ProviderClass = Provider_Class.Eventbusschedule
            };
            config.Headers.Add("X-Custom-Header", "custom-value");
        });

        await requestAdapter.Received(1).SendAsync<EventBus.Commonserviceitem.CommonserviceitemGetResponse>(
            Arg.Is<RequestInformation>(ri =>
                ri.Headers.ContainsKey("X-Custom-Header") &&
                ri.Headers["X-Custom-Header"].Contains("custom-value")),
            Arg.Any<ParsableFactory<EventBus.Commonserviceitem.CommonserviceitemGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
    }
}
