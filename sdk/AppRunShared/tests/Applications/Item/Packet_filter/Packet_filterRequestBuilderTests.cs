using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.AppRunShared.Applications.Item.Packet_filter;
using Sacloud.Sdk.AppRunShared.Models;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;
using Sacloud.Sdk.AppRunShared.Models.Handler;

namespace Sacloud.Sdk.AppRunShared.Tests.Applications.Item.Packet_filter;

/// <summary>
/// Packet_filterRequestBuilderのテスト
/// </summary>
public class Packet_filterRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<Packet_filterRequestBuilder> _mockLogger;

    public Packet_filterRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<Packet_filterRequestBuilder>>();
        _mockLoggerFactory.CreateLogger<Packet_filterRequestBuilder>().Returns(_mockLogger);
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

        var builder = new Packet_filterRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var rawUrl = "https://api.example.com/applications/test-id/packet_filter";

        var builder = new Packet_filterRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

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
        var builder = new Packet_filterRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var builder = new Packet_filterRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// ToPatchRequestInformationがPATCHメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToPatchRequestInformation_CreatesRequestInformationWithPatchMethod()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new Packet_filterRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PatchPacketFilterBody();

        var requestInfo = builder.ToPatchRequestInformation(body);

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.PATCH, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToPatchRequestInformationがnullボディで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ToPatchRequestInformation_WithNullBody_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new Packet_filterRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.Throws<ArgumentNullException>(() => builder.ToPatchRequestInformation(null!));
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
        var builder = new Packet_filterRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new ReadPacketFilter();

        requestAdapter.SendAsync<ReadPacketFilter>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ReadPacketFilter>>(),
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
        var builder = new Packet_filterRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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

        requestAdapter.SendAsync<ReadPacketFilter>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ReadPacketFilter>>(),
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
                @"^パケットフィルタ取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var builder = new Packet_filterRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync<ReadPacketFilter>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ReadPacketFilter>>(),
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
                @"^パケットフィルタの取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var builder = new Packet_filterRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new ReadPacketFilter();

        requestAdapter.SendAsync<ReadPacketFilter>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ReadPacketFilter>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.GetAsync();

        await requestAdapter.Received(1).SendAsync<ReadPacketFilter>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ReadPacketFilter>>(),
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
                    @"^パケットフィルタの取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^パケットフィルタの取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PatchAsyncのパスパラメータにidがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("id", "")]
    [InlineData("id", " ")]
    [InlineData("id", "　")]
    public async Task PatchAsync_WithInvalidPathParameters_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { pathParameterKey, pathParameterValue }
        };
        var builder = new Packet_filterRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PatchPacketFilterBody();

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PatchAsync(body);
        });

        Assert.Equal("アプリケーションIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("アプリケーションIDを入力してください。");
    }

    /// <summary>
    /// PatchAsyncのリクエストボディのパラメータがnullの場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("Settings.FromIp")]
    [InlineData("Settings.FromIpPrefixLength")]
    public async Task PatchAsync_WithNullParameter_ThrowsValidationException(string invalidParameterName)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new Packet_filterRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PatchPacketFilterBody
        {
            IsEnabled = true,
            Settings = new List<PatchPacketFilterBody_settings>()
            {
                new PatchPacketFilterBody_settings()
                {
                    FromIp = invalidParameterName == "Settings.FromIp" ? null : "127.0.0.1",
                    FromIpPrefixLength = invalidParameterName == "Settings.FromIpPrefixLength" ? null : 24
                }
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PatchAsync(body);
        });

        Assert.Equal($"{invalidParameterName}が入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}が入力されていません。");
    }

    /// <summary>
    /// PatchAsyncのリクエストボディのパラメータが有効な要素数でない値の場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PatchAsync_WithInvalidElementCountParameter_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new Packet_filterRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PatchPacketFilterBody
        {
            IsEnabled = true,
            Settings = Enumerable.Range(0, 11)
                .Select(_ => new PatchPacketFilterBody_settings { FromIp = "127.0.0.1", FromIpPrefixLength = 24 })
                .ToList()
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PatchAsync(body);
        });
        Assert.Equal("Settingsの要素数は10以下にしてください。", exception.Message);
        _mockLogger.Received(1).LogWarning("Settingsの要素数は10以下にしてください。");
    }

    /// <summary>
    /// PatchAsyncのリクエストボディのパラメータが有効なIPv4アドレス形式でない値の場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("127.0.1")]
    [InlineData("192.168.001.001")]
    [InlineData("256.1.1.1")]
    [InlineData("localhost")]
    [InlineData("2001:0db8:0000:0000:0000:0000:0000:0001")]
    [InlineData("::1")]
    public async Task PatchAsync_WithInvalidIPv4AddressParameter_ThrowsValidationException(string invalidParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new Packet_filterRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PatchPacketFilterBody
        {
            IsEnabled = true,
            Settings = new List<PatchPacketFilterBody_settings>()
            {
                new PatchPacketFilterBody_settings { FromIp = invalidParameterValue, FromIpPrefixLength = 24 }
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PatchAsync(body);
        });
        Assert.Equal("Settings.FromIpには有効なIPv4アドレス形式を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("Settings.FromIpには有効なIPv4アドレス形式を入力してください。");
    }

    /// <summary>
    /// PatchAsyncのリクエストボディのパラメータが有効な数値範囲でない値の場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(33)]
    public async Task PatchAsync_WithOutOfRangeParameter_ThrowsValidationException(int invalidParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new Packet_filterRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PatchPacketFilterBody
        {
            IsEnabled = true,
            Settings = new List<PatchPacketFilterBody_settings>()
            {
                new PatchPacketFilterBody_settings { FromIp = "127.0.0.1", FromIpPrefixLength = invalidParameterValue }
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PatchAsync(body);
        });
        Assert.Equal("Settings.FromIpPrefixLengthの値は1以上32以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("Settings.FromIpPrefixLengthの値は1以上32以下で入力してください。");
    }

    /// <summary>
    /// PatchAsyncのリクエストボディの複数パラメータが無効な値の場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PatchAsync_WithMultipleInvalidParams_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new Packet_filterRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PatchPacketFilterBody
        {
            IsEnabled = true,
            Settings = new List<PatchPacketFilterBody_settings>()
            {
                new PatchPacketFilterBody_settings()
                {
                    FromIp = null,
                    FromIpPrefixLength = null
                }
            },
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PatchAsync(body);
        });

        var expectedMessage = """
            Settings.FromIpが入力されていません。
            Settings.FromIpPrefixLengthが入力されていません。
            """;
        Assert.Equal(expectedMessage.ReplaceLineEndings(), exception.Message.ReplaceLineEndings());
        _mockLogger.Received(1).LogWarning("Settings.FromIpが入力されていません。");
        _mockLogger.Received(1).LogWarning("Settings.FromIpPrefixLengthが入力されていません。");
    }

    /// <summary>
    /// PatchAsyncでCloudctrlErrorによるAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PatchAsync_WithCloudctrlError_ThrowsApiException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new Packet_filterRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PatchPacketFilterBody
        {
            IsEnabled = true,
            Settings = new List<PatchPacketFilterBody_settings>()
            {
                new PatchPacketFilterBody_settings()
                {
                    FromIp = "127.0.0.1",
                    FromIpPrefixLength = 24
                }
            },
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

        requestAdapter.SendAsync<PatchPacketFilter>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PatchPacketFilter>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(async () =>
        {
            await builder.PatchAsync(body);
        });

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^パケットフィルタ変更のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PatchAsyncでその他のエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PatchAsync_WithOtherError_ThrowsException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new Packet_filterRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PatchPacketFilterBody
        {
            IsEnabled = true,
            Settings = new List<PatchPacketFilterBody_settings>()
            {
                new PatchPacketFilterBody_settings()
                {
                    FromIp = "127.0.0.1",
                    FromIpPrefixLength = 24
                }
            },
        };
        var expectedException = new Exception();

        requestAdapter.SendAsync<PatchPacketFilter>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PatchPacketFilter>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<Exception>(async () =>
        {
            await builder.PatchAsync(body);
        });

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^パケットフィルタの変更中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PatchAsyncがRequestAdapter.SendAsyncを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task PatchAsync_CallsRequestAdapterSendAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new Packet_filterRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PatchPacketFilterBody
        {
            IsEnabled = true,
            Settings = new List<PatchPacketFilterBody_settings>()
            {
                new PatchPacketFilterBody_settings()
                {
                    FromIp = "127.0.0.1",
                    FromIpPrefixLength = 24
                }
            },
        };
        var expectedResponse = new PatchPacketFilter();

        requestAdapter.SendAsync<PatchPacketFilter>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PatchPacketFilter>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.PatchAsync(body);

        await requestAdapter.Received(1).SendAsync<PatchPacketFilter>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PatchPacketFilter>>(),
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
                    @"^パケットフィルタの変更を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^パケットフィルタの変更が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PatchAsyncがnullボディで例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PatchAsync_WithNullBody_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new Packet_filterRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await Assert.ThrowsAsync<ValidationException>(() => builder.PatchAsync(null!));
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
        var builder = new Packet_filterRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var newUrl = "https://newapi.example.com/applications/new-id/packet_filter";

        var newBuilder = builder.WithUrl(newUrl);

        Assert.NotNull(newBuilder);
        Assert.NotSame(builder, newBuilder);
        Assert.IsType<Packet_filterRequestBuilder>(newBuilder);
    }
}
