using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.AppRunShared.Applications.Item.Traffics;
using Sacloud.Sdk.AppRunShared.Models;
using Sacloud.Sdk.AppRunShared.Models.Handler;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.AppRunShared.Tests.Applications.Item.Traffics;

/// <summary>
/// TrafficsRequestBuilderのテスト
/// </summary>
public class TrafficsRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<TrafficsRequestBuilder> _mockLogger;

    public TrafficsRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<TrafficsRequestBuilder>>();
        _mockLoggerFactory.CreateLogger<TrafficsRequestBuilder>().Returns(_mockLogger);
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

        var builder = new TrafficsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var rawUrl = "https://api.example.com/applications/test-id/traffics";

        var builder = new TrafficsRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// ToGetRequestInformationがGETメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_CreatesRequestInformationWithGetMethod()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new TrafficsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new TrafficsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// ToPutRequestInformationがPUTメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_CreatesRequestInformationWithPutMethod()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new TrafficsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new List<TrafficsRequestBuilder.UpdateTrafficBody>
        {
            new TrafficsRequestBuilder.UpdateTrafficBody
            {
                UpdateTrafficBodyMember1 = new UpdateTrafficBodyMember1 { IsLatestVersion = true, Percent = 100 }
            }
        };

        var requestInfo = builder.ToPutRequestInformation(body);

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.PUT, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToPutRequestInformationが正しいAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_SetsAcceptHeader()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new TrafficsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new List<TrafficsRequestBuilder.UpdateTrafficBody>
        {
            new TrafficsRequestBuilder.UpdateTrafficBody
            {
                UpdateTrafficBodyMember1 = new UpdateTrafficBodyMember1 { IsLatestVersion = true, Percent = 100 }
            }
        };

        var requestInfo = builder.ToPutRequestInformation(body);

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// ToPutRequestInformationがnullボディで例外をスローすることを確認
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
        var builder = new TrafficsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.Throws<ArgumentNullException>(() => builder.ToPutRequestInformation(null!));
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
        var builder = new TrafficsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new ListTraffic();

        requestAdapter.SendAsync<ListTraffic>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListTraffic>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.GetAsync();
        });

        Assert.Equal("アプリケーションIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("アプリケーションIDを入力してください。");
    }

    /// <summary>
    /// GetAsyncでCloudctrlErrorによるAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithCloudctrlError_ThrowsApiException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new TrafficsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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

        requestAdapter.SendAsync<ListTraffic>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListTraffic>>(),
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
                @"^アプリケーショントラフィック分散取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var builder = new TrafficsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync<ListTraffic>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListTraffic>>(),
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
                @"^アプリケーショントラフィック分散の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncがRequestAdapter.SendAsyncを呼び出すことを確認
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
        var builder = new TrafficsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new ListTraffic();

        requestAdapter.SendAsync<ListTraffic>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListTraffic>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.GetAsync();

        await requestAdapter.Received(1).SendAsync<ListTraffic>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListTraffic>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
        Assert.Equal(expectedResponse, result);
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^アプリケーショントラフィック分散の取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^アプリケーショントラフィック分散の取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PutAsyncのリクエストボディのパラメータがnullの場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("IsLatestVersion")]
    [InlineData("Percent")]
    [InlineData("VersionName")]
    public async Task PutAsync_WithNullParameter_ThrowsValidationException(string invalidParameterName)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new TrafficsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new List<TrafficsRequestBuilder.UpdateTrafficBody>
        {
            new TrafficsRequestBuilder.UpdateTrafficBody
            {
                UpdateTrafficBodyMember1 = new UpdateTrafficBodyMember1 { IsLatestVersion = invalidParameterName == "IsLatestVersion" ? null : true, Percent = invalidParameterName == "Percent" ? null : 100 },
            },
            new TrafficsRequestBuilder.UpdateTrafficBody
            {
                UpdateTrafficBodyMember2 = new UpdateTrafficBodyMember2 { VersionName = invalidParameterName == "VersionName" ? null : "test-version-name", Percent =  100 },
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
    /// PutAsyncのリクエストボディの排他的なパラメータが共存する場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithMultipleExclusiveParameters_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new TrafficsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new List<TrafficsRequestBuilder.UpdateTrafficBody>
        {
            new TrafficsRequestBuilder.UpdateTrafficBody
            {
                UpdateTrafficBodyMember1 = new UpdateTrafficBodyMember1 { IsLatestVersion = true, Percent = 80 },
                UpdateTrafficBodyMember2 = new UpdateTrafficBodyMember2 { VersionName = "test-version-name", Percent =  20 },
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(body);
        });

        Assert.Equal("UpdateTrafficBodyMember1またはUpdateTrafficBodyMember2のいずれか1つのみを設定してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("UpdateTrafficBodyMember1またはUpdateTrafficBodyMember2のいずれか1つのみを設定してください。");
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
        var builder = new TrafficsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new List<TrafficsRequestBuilder.UpdateTrafficBody>
        {
            new TrafficsRequestBuilder.UpdateTrafficBody
            {
                UpdateTrafficBodyMember1 = new UpdateTrafficBodyMember1 { IsLatestVersion = null, Percent = null }
            },
            new TrafficsRequestBuilder.UpdateTrafficBody
            {
                UpdateTrafficBodyMember2 = new UpdateTrafficBodyMember2 { VersionName = null, Percent = 100 }
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(body);
        });

        var expectedMessage = """
            IsLatestVersionが入力されていません。
            Percentが入力されていません。
            VersionNameが入力されていません。
            """;
        Assert.Equal(expectedMessage.ReplaceLineEndings(), exception.Message.ReplaceLineEndings());
        _mockLogger.Received(1).LogWarning("IsLatestVersionが入力されていません。");
        _mockLogger.Received(1).LogWarning("Percentが入力されていません。");
        _mockLogger.Received(1).LogWarning("VersionNameが入力されていません。");
    }

    /// <summary>
    /// PutAsyncでCloudctrlErrorによるAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithCloudctrlError_ThrowsApiException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new TrafficsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new List<TrafficsRequestBuilder.UpdateTrafficBody>
        {
            new TrafficsRequestBuilder.UpdateTrafficBody
            {
                UpdateTrafficBodyMember1 = new UpdateTrafficBodyMember1 { IsLatestVersion = true, Percent = 100 }
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

        requestAdapter.SendAsync<UpdateTraffic>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<UpdateTraffic>>(),
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
                @"^アプリケーショントラフィック分散変更のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var builder = new TrafficsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new List<TrafficsRequestBuilder.UpdateTrafficBody>
        {
            new TrafficsRequestBuilder.UpdateTrafficBody
            {
                UpdateTrafficBodyMember1 = new UpdateTrafficBodyMember1 { IsLatestVersion = true, Percent = 100 }
            }
        };
        var expectedException = new Exception();

        requestAdapter.SendAsync<UpdateTraffic>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<UpdateTraffic>>(),
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
                @"^アプリケーショントラフィック分散の変更中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PutAsyncがRequestAdapter.SendAsyncを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_CallsRequestAdapterSendAsync()
    {

        var serializationWriterFactory = new JsonSerializationWriterFactory();

        // RequestAdapterのモック作成
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        requestAdapter.SerializationWriterFactory.Returns(serializationWriterFactory);
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new TrafficsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new List<TrafficsRequestBuilder.UpdateTrafficBody>
        {
            new TrafficsRequestBuilder.UpdateTrafficBody
            {
                UpdateTrafficBodyMember1 = new UpdateTrafficBodyMember1 { IsLatestVersion = true, Percent = 100 }
            }
        };
        var expectedResponse = new UpdateTraffic();

        requestAdapter.SendAsync<UpdateTraffic>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<UpdateTraffic>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.PutAsync(body);

        await requestAdapter.Received(1).SendAsync<UpdateTraffic>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<UpdateTraffic>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
        Assert.Equal(expectedResponse, result);
        _mockLogger.Received(1).Log(
    LogLevel.Information,
    Arg.Any<EventId>(),
    Arg.Is<object>(state =>
        System.Text.RegularExpressions.Regex.IsMatch(
            state.ToString()!,
            @"^アプリケーショントラフィック分散の変更を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
    null,
    Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^アプリケーショントラフィック分散の変更が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PutAsyncがnullのリクエストボディで例外をスローすることを確認
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
        var builder = new TrafficsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(null!));

        Assert.Equal("リクエストボディは必須です。", exception.Message);
    }

    /// <summary>
    /// PutAsyncが空配列のリクエストボディで例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithEmptyArrayBody_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new TrafficsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(new List<TrafficsRequestBuilder.UpdateTrafficBody>()));

        Assert.Equal("リクエストボディが空配列です。", exception.Message);
    }

    /// <summary>
    /// WithUrlが新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new TrafficsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var newUrl = "https://newapi.example.com/applications/new-id/traffics";

        var newBuilder = builder.WithUrl(newUrl);

        Assert.NotNull(newBuilder);
        Assert.NotSame(builder, newBuilder);
        Assert.IsType<TrafficsRequestBuilder>(newBuilder);
    }

    /// <summary>
    /// UpdateTrafficBodyがUpdateTrafficBodyMember1プロパティを設定できることを確認
    /// </summary>
    [Fact]
    public void UpdateTrafficBody_CanSetUpdateTrafficBodyMember1()
    {
        var body = new TrafficsRequestBuilder.UpdateTrafficBody();
        var member1 = new UpdateTrafficBodyMember1 { IsLatestVersion = true, Percent = 100 };

        body.UpdateTrafficBodyMember1 = member1;

        Assert.NotNull(body.UpdateTrafficBodyMember1);
        Assert.True(body.UpdateTrafficBodyMember1.IsLatestVersion);
        Assert.Equal(100, body.UpdateTrafficBodyMember1.Percent);
    }

    /// <summary>
    /// UpdateTrafficBodyがUpdateTrafficBodyMember2プロパティを設定できることを確認
    /// </summary>
    [Fact]
    public void UpdateTrafficBody_CanSetUpdateTrafficBodyMember2()
    {
        var body = new TrafficsRequestBuilder.UpdateTrafficBody();
        var member2 = new UpdateTrafficBodyMember2 { VersionName = "version-1", Percent = 50 };

        body.UpdateTrafficBodyMember2 = member2;

        Assert.NotNull(body.UpdateTrafficBodyMember2);
        Assert.Equal("version-1", body.UpdateTrafficBodyMember2.VersionName);
        Assert.Equal(50, body.UpdateTrafficBodyMember2.Percent);
    }

    /// <summary>
    /// UpdateTrafficBody.CreateFromDiscriminatorValueがnullパラメータで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void UpdateTrafficBody_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => 
            TrafficsRequestBuilder.UpdateTrafficBody.CreateFromDiscriminatorValue(null!));
    }

    /// <summary>
    /// UpdateTrafficBody.CreateFromDiscriminatorValueが新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void UpdateTrafficBody_CreateFromDiscriminatorValue_ReturnsNewInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var childNode = Substitute.For<IParseNode>();
        childNode.GetStringValue().Returns(string.Empty);
        parseNode.GetChildNode("").Returns(childNode);

        var result = TrafficsRequestBuilder.UpdateTrafficBody.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<TrafficsRequestBuilder.UpdateTrafficBody>(result);
    }

    /// <summary>
    /// UpdateTrafficBody.CreateFromDiscriminatorValueがis_latest_version指定時にMember1を解釈できることを確認
    /// </summary>
    [Fact]
    public void UpdateTrafficBody_CreateFromDiscriminatorValue_WithIsLatestVersionBody_ResolvesMember1()
    {
        var parseNode = Substitute.For<IParseNode>();
        var childNode = Substitute.For<IParseNode>();
        childNode.GetStringValue().Returns("isLatestVersion");
        parseNode.GetChildNode("").Returns(childNode);

        var result = TrafficsRequestBuilder.UpdateTrafficBody.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.NotNull(result.UpdateTrafficBodyMember1);
        Assert.Null(result.UpdateTrafficBodyMember2);
    }

    /// <summary>
    /// UpdateTrafficBody.CreateFromDiscriminatorValueがversion_name指定時にMember2を解釈できることを確認
    /// </summary>
    [Fact]
    public void UpdateTrafficBody_CreateFromDiscriminatorValue_WithVersionNameBody_ResolvesMember2()
    {
        var parseNode = Substitute.For<IParseNode>();
        var childNode = Substitute.For<IParseNode>();
        childNode.GetStringValue().Returns("versionName");
        parseNode.GetChildNode("").Returns(childNode);

        var result = TrafficsRequestBuilder.UpdateTrafficBody.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.Null(result.UpdateTrafficBodyMember1);
        Assert.NotNull(result.UpdateTrafficBodyMember2);
    }

    /// <summary>
    /// UpdateTrafficBody.GetFieldDeserializersがUpdateTrafficBodyMember1のデシリアライザーを返すことを確認
    /// </summary>
    [Fact]
    public void UpdateTrafficBody_GetFieldDeserializers_WithMember1_ReturnsMember1Deserializers()
    {
        var body = new TrafficsRequestBuilder.UpdateTrafficBody
        {
            UpdateTrafficBodyMember1 = new UpdateTrafficBodyMember1()
        };

        var deserializers = body.GetFieldDeserializers();

        Assert.NotNull(deserializers);
    }

    /// <summary>
    /// UpdateTrafficBody.GetFieldDeserializersがUpdateTrafficBodyMember2のデシリアライザーを返すことを確認
    /// </summary>
    [Fact]
    public void UpdateTrafficBody_GetFieldDeserializers_WithMember2_ReturnsMember2Deserializers()
    {
        var body = new TrafficsRequestBuilder.UpdateTrafficBody
        {
            UpdateTrafficBodyMember2 = new UpdateTrafficBodyMember2()
        };

        var deserializers = body.GetFieldDeserializers();

        Assert.NotNull(deserializers);
    }

    /// <summary>
    /// UpdateTrafficBody.GetFieldDeserializersが空のメンバーで空のディクショナリを返すことを確認
    /// </summary>
    [Fact]
    public void UpdateTrafficBody_GetFieldDeserializers_WithNoMembers_ReturnsEmptyDictionary()
    {
        var body = new TrafficsRequestBuilder.UpdateTrafficBody();

        var deserializers = body.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Empty(deserializers);
    }

    /// <summary>
    /// UpdateTrafficBody.Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void UpdateTrafficBody_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var body = new TrafficsRequestBuilder.UpdateTrafficBody();

        Assert.Throws<ArgumentNullException>(() => body.Serialize(null!));
    }

    /// <summary>
    /// UpdateTrafficBody.SerializeがUpdateTrafficBodyMember1を正しくシリアライズすることを確認
    /// </summary>
    [Fact]
    public void UpdateTrafficBody_Serialize_WithMember1_SerializesCorrectly()
    {
        var body = new TrafficsRequestBuilder.UpdateTrafficBody
        {
            UpdateTrafficBodyMember1 = new UpdateTrafficBodyMember1 { IsLatestVersion = true, Percent = 100 }
        };
        var writer = Substitute.For<ISerializationWriter>();

        body.Serialize(writer);

        writer.Received(1).WriteObjectValue<UpdateTrafficBodyMember1>(null, body.UpdateTrafficBodyMember1);
    }

    /// <summary>
    /// UpdateTrafficBody.SerializeがUpdateTrafficBodyMember2を正しくシリアライズすることを確認
    /// </summary>
    [Fact]
    public void UpdateTrafficBody_Serialize_WithMember2_SerializesCorrectly()
    {
        var body = new TrafficsRequestBuilder.UpdateTrafficBody
        {
            UpdateTrafficBodyMember2 = new UpdateTrafficBodyMember2 { VersionName = "version-1", Percent = 50 }
        };
        var writer = Substitute.For<ISerializationWriter>();

        body.Serialize(writer);

        writer.Received(1).WriteObjectValue<UpdateTrafficBodyMember2>(null, body.UpdateTrafficBodyMember2);
    }

    /// <summary>
    /// ToPutRequestInformationがContentを正しく設定することを確認
    /// </summary>
    [Fact]
    public async Task ToPutRequestInformation_SetsContent()
    {
        var serializationWriterFactory = new JsonSerializationWriterFactory();

        // RequestAdapterのモック作成
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        requestAdapter.SerializationWriterFactory.Returns(serializationWriterFactory);

        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
            var builder = new TrafficsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
            var body = new List<TrafficsRequestBuilder.UpdateTrafficBody>
        {
            new TrafficsRequestBuilder.UpdateTrafficBody
            {
                UpdateTrafficBodyMember1 = new UpdateTrafficBodyMember1 { IsLatestVersion = true, Percent = 50 }
            },
            new TrafficsRequestBuilder.UpdateTrafficBody
            {
                UpdateTrafficBodyMember2 = new UpdateTrafficBodyMember2 { VersionName = "version1.0", Percent = 50 }
            }
        };

        var requestInfo = builder.ToPutRequestInformation(body);

        // ContentがStreamとして設定されていることを確認
        Assert.NotNull(requestInfo.Content);
        Assert.True(requestInfo.Content.CanRead);

        // Contentの内容を読み取ってJSON形式であることを確認
        using var memoryStream = new MemoryStream();
        await requestInfo.Content.CopyToAsync(memoryStream);
        memoryStream.Position = 0;

        using var reader = new StreamReader(memoryStream);
        var jsonContent = await reader.ReadToEndAsync();

        // JSONの検証
        Assert.NotEmpty(jsonContent);
        Assert.Contains("is_latest_version", jsonContent);
        Assert.Contains("version_name", jsonContent);
        Assert.Contains("\"percent\":50", jsonContent);

        // JSONとしてパース可能か確認
        var jsonDocument = System.Text.Json.JsonDocument.Parse(jsonContent);
        var jsonArray = jsonDocument.RootElement;
        Assert.Equal(System.Text.Json.JsonValueKind.Array, jsonArray.ValueKind);
        Assert.Equal(2, jsonArray.GetArrayLength());

        // 1つ目の要素がMember1（is_latest_versionを持つ）
        var firstElement = jsonArray[0];
        Assert.True(firstElement.TryGetProperty("is_latest_version", out var isLatestVersion));
        Assert.True(isLatestVersion.GetBoolean());
        Assert.True(firstElement.TryGetProperty("percent", out var percent1));
        Assert.Equal(50, percent1.GetInt32());

        // 2つ目の要素がMember2（version_nameを持つ）
        var secondElement = jsonArray[1];
        Assert.True(secondElement.TryGetProperty("version_name", out var versionName));
        Assert.Equal("version1.0", versionName.GetString());
        Assert.True(secondElement.TryGetProperty("percent", out var percent2));
        Assert.Equal(50, percent2.GetInt32());

        // Content-Typeヘッダーが設定されていることを確認
        Assert.True(requestInfo.Headers.ContainsKey("Content-Type"));
        Assert.Contains("application/json", requestInfo.Headers["Content-Type"]);
    }
}
