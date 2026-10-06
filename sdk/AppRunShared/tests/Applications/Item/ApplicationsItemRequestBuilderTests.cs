using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.AppRunShared.Applications;
using Sacloud.Sdk.AppRunShared.Applications.Item;
using Sacloud.Sdk.AppRunShared.Applications.Item.Packet_filter;
using Sacloud.Sdk.AppRunShared.Applications.Item.Status;
using Sacloud.Sdk.AppRunShared.Applications.Item.Traffics;
using Sacloud.Sdk.AppRunShared.Applications.Item.Versions;
using Sacloud.Sdk.AppRunShared.Models;
using Sacloud.Sdk.AppRunShared.Models.Handler;
using Sacloud.Sdk.AppRunShared.Models.Request;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.AppRunShared.Tests.Applications.Item;

/// <summary>
/// ApplicationsItemRequestBuilderのテスト
/// </summary>
public class ApplicationsItemRequestBuilderTests
{
    private readonly ILogger<ApplicationsItemRequestBuilder> _mockLogger;
    private readonly ILoggerFactory _mockLoggerFactory;

    public ApplicationsItemRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<ApplicationsItemRequestBuilder>>();
        _mockLoggerFactory.CreateLogger(Arg.Any<string>()).Returns(_mockLogger);
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

        var builder = new ApplicationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var rawUrl = "https://api.example.com/applications/test-id";

        var builder = new ApplicationsItemRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// Packet_filterプロパティが正しくPacket_filterRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void PacketFilter_ReturnsPacketFilterRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new ApplicationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var packetFilterBuilder = builder.Packet_filter;

        Assert.NotNull(packetFilterBuilder);
        Assert.IsType<Packet_filterRequestBuilder>(packetFilterBuilder);
    }

    /// <summary>
    /// Statusプロパティが正しくStatusRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Status_ReturnsStatusRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new ApplicationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var statusBuilder = builder.Status;

        Assert.NotNull(statusBuilder);
        Assert.IsType<StatusRequestBuilder>(statusBuilder);
    }

    /// <summary>
    /// Trafficsプロパティが正しくTrafficsRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Traffics_ReturnsTrafficsRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new ApplicationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var trafficsBuilder = builder.Traffics;

        Assert.NotNull(trafficsBuilder);
        Assert.IsType<TrafficsRequestBuilder>(trafficsBuilder);
    }

    /// <summary>
    /// Versionsプロパティが正しくVersionsRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Versions_ReturnsVersionsRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new ApplicationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var versionsBuilder = builder.Versions;

        Assert.NotNull(versionsBuilder);
        Assert.IsType<VersionsRequestBuilder>(versionsBuilder);
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
        var builder = new ApplicationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.GET, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToDeleteRequestInformationがDELETEメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToDeleteRequestInformation_CreatesRequestInformationWithDeleteMethod()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new ApplicationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToDeleteRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.DELETE, requestInfo.HttpMethod);
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
        var builder = new ApplicationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PatchApplicationBody();

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
        var builder = new ApplicationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var builder = new ApplicationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new ReadApplication();

        requestAdapter.SendAsync<ReadApplication>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ReadApplication>>(),
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
        var builder = new ApplicationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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

        requestAdapter.SendAsync<ReadApplication>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ReadApplication>>(),
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
                @"^アプリケーション詳細取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var builder = new ApplicationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync<ReadApplication>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ReadApplication>>(),
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
                @"^アプリケーション詳細の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
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
        var builder = new ApplicationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new ReadApplication();

        requestAdapter.SendAsync<ReadApplication>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ReadApplication>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.GetAsync();

        await requestAdapter.Received(1).SendAsync<ReadApplication>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ReadApplication>>(),
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
                    @"^アプリケーション詳細の取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^アプリケーション詳細の取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
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
        var builder = new ApplicationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.DeleteAsync();
        });

        Assert.Equal("アプリケーションIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("アプリケーションIDを入力してください。");
    }

    /// <summary>
    /// DeleteAsyncでCloudctrlErrorによるAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_WithCloudctrlError_ThrowsApiException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new ApplicationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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
                @"^アプリケーション削除のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var builder = new ApplicationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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
                @"^アプリケーションの削除中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// DeleteAsyncがRequestAdapter.SendNoContentAsyncを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_CallsRequestAdapterSendNoContentAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new ApplicationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
                    @"^アプリケーションの削除を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^アプリケーションの削除が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 204\]$")),
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
        var builder = new ApplicationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PatchApplicationBody();

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
    [InlineData("Components.Name")]
    [InlineData("Components.MaxCpu")]
    [InlineData("Components.MaxMemory")]
    [InlineData("Components.DeploySource")]
    [InlineData("Components.DeploySource.ContainerRegistry.Image")]
    [InlineData("Components.Env.Key")]
    [InlineData("Components.Env.Value")]
    [InlineData("Components.Secret.Key")]
    [InlineData("Components.Probe.HttpGet.Path")]
    [InlineData("Components.Probe.HttpGet.Port")]
    public async Task PatchAsync_WithNullParameter_ThrowsValidationException(string invalidParameterName)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new ApplicationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var components = new List<PatchApplicationBody_components>()
        {
            new PatchApplicationBody_components()
            {
                DeploySource = invalidParameterName == "Components.DeploySource" ? null : new PatchApplicationBody_components_deploy_source()
                {
                    ContainerRegistry = new PatchApplicationBody_components_deploy_source_container_registry()
                    {
                        Image = invalidParameterName == "Components.DeploySource.ContainerRegistry.Image" ? null : "test-image"
                    }
                },
                Env = new List<EnvItem>()
                {
                    new EnvItem()
                    {
                        Key = invalidParameterName == "Components.Env.Key" ? null : "test-env-key",
                        Value = invalidParameterName == "Components.Env.Value" ? null : "test-env-value"
                    }
                },
                Secret = new List<PatchApplicationBody_components_secret>()
                {
                    new PatchApplicationBody_components_secret()
                    {
                        Key = invalidParameterName == "Components.Secret.Key" ? null : "test-secret-key"
                    }
                },
                MaxCpu = invalidParameterName == "Components.MaxCpu" ? null : new PatchApplicationBody_components_max_cpu(),
                MaxMemory = invalidParameterName == "Components.MaxMemory" ? null : new PatchApplicationBody_components_max_memory(),
                Name = invalidParameterName == "Components.Name" ? null : "test-component-name",
                Probe = new PatchApplicationBody_components_probe()
                {
                    HttpGet = new PatchApplicationBody_components_probe_http_get()
                    {
                        Path = invalidParameterName == "Components.Probe.HttpGet.Path" ? null : "/health",
                        Port = invalidParameterName == "Components.Probe.HttpGet.Port" ? null : 1234
                    }
                }
            }
        };
        var body = new PatchApplicationBody
        {
            Components = components,
            MaxScale = 10,
            MinScale = 0,
            Port = 1234,
            ScaleTargetConcurrency = 100,
            TimeoutSeconds = 60
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PatchAsync(body);
        });

        Assert.Equal($"{invalidParameterName}が入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}が入力されていません。");
    }

    /// <summary>
    /// PatchAsyncのリクエストボディのパラメータが有効な数値範囲でない値の場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("Components.Probe.HttpGet.Port", -1, 1, 65535)]
    [InlineData("Components.Probe.HttpGet.Port", 65536, 1, 65535)]
    [InlineData("MaxScale", 0, 1, 10)]
    [InlineData("MaxScale", 11, 1, 10)]
    [InlineData("MinScale", -1, 0, 10)]
    [InlineData("MinScale", 11, 0, 10)]
    [InlineData("Port", -1, 1, 65535)]
    [InlineData("Port", 65536, 1, 65535)]
    [InlineData("ScaleTargetConcurrency", 49, 50, 200)]
    [InlineData("ScaleTargetConcurrency", 201, 50, 200)]
    [InlineData("TimeoutSeconds", 0, 1, 300)]
    [InlineData("TimeoutSeconds", 301, 1, 300)]
    public async Task PatchAsync_WithOutOfRangeParameter_ThrowsValidationException(string invalidParameterName, int invalidParameterValue, int validMinimumValue, int validMaximumValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new ApplicationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var components = new List<PatchApplicationBody_components>()
        {
            new PatchApplicationBody_components()
            {
                DeploySource = new PatchApplicationBody_components_deploy_source()
                {
                    ContainerRegistry = new PatchApplicationBody_components_deploy_source_container_registry()
                    {
                        Image = "test-image"
                    }
                },
                MaxCpu = new PatchApplicationBody_components_max_cpu(),
                MaxMemory = new PatchApplicationBody_components_max_memory(),
                Name = "test-component-name",
                Probe = new PatchApplicationBody_components_probe()
                {
                    HttpGet = new PatchApplicationBody_components_probe_http_get()
                    {
                        Path = "/health",
                        Port = invalidParameterName == "Components.Probe.HttpGet.Port" ? invalidParameterValue : 1234
                    }
                }
            }
        };
        var body = new PatchApplicationBody
        {
            Components = components,
            MaxScale = invalidParameterName == "MaxScale" ? invalidParameterValue : 10,
            MinScale = invalidParameterName == "MinScale" ? invalidParameterValue : 0,
            Port = invalidParameterName == "Port" ? invalidParameterValue : 1234,
            ScaleTargetConcurrency = invalidParameterName == "ScaleTargetConcurrency" ? invalidParameterValue : 100,
            TimeoutSeconds = invalidParameterName == "TimeoutSeconds" ? invalidParameterValue : 60
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PatchAsync(body);
        });

        Assert.Equal($"{invalidParameterName}の値は{validMinimumValue}以上{validMaximumValue}以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}の値は{validMinimumValue}以上{validMaximumValue}以下で入力してください。");
    }

    /// <summary>
    /// PatchAsyncのリクエストボディのパラメータの値がEnumに含まれていない場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("Components.DeploySource.ContainerRegistry.Action")]
    [InlineData("Components.MaxCpu")]
    [InlineData("Components.MaxMemory")]
    public async Task PatchAsync_WithOutOfEnumParameter_ThrowsValidationException(string invalidParameterName)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new ApplicationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var components = new List<PatchApplicationBody_components>()
        {
            new PatchApplicationBody_components()
            {
                DeploySource = new PatchApplicationBody_components_deploy_source()
                {
                    ContainerRegistry = new PatchApplicationBody_components_deploy_source_container_registry()
                    {
                        Action = invalidParameterName == "Components.DeploySource.ContainerRegistry.Action" ? (ContainerRegistryAction)999 : ContainerRegistryAction.New,
                        Image = "test-image"
                    }
                },
                MaxCpu = invalidParameterName == "Components.MaxCpu" ? (PatchApplicationBody_components_max_cpu)999 : new PatchApplicationBody_components_max_cpu(),
                MaxMemory = invalidParameterName == "Components.MaxMemory" ? (PatchApplicationBody_components_max_memory)999 : new PatchApplicationBody_components_max_memory(),
                Name = "test-component-name",
                Probe = new PatchApplicationBody_components_probe()
                {
                    HttpGet = new PatchApplicationBody_components_probe_http_get()
                    {
                        Path = "/health",
                        Port = 1234
                    }
                }
            }
        };
        var body = new PatchApplicationBody
        {
            Components = components,
            MaxScale = 10,
            MinScale = 0,
            Port = 1234,
            ScaleTargetConcurrency = 100,
            TimeoutSeconds = 60
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PatchAsync(body);
        });

        Assert.Equal($"{invalidParameterName}はEnumに含まれる値を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}はEnumに含まれる値を入力してください。");
    }

    /// <summary>
    /// PatchAsyncのリクエストボディのパラメータが有効な文字数範囲でない値の場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("Components.DeploySource.ContainerRegistry.Image", 129, 1, 128)]
    [InlineData("Components.DeploySource.ContainerRegistry.Password", "", 1, 63)]
    [InlineData("Components.DeploySource.ContainerRegistry.Password", 64, 1, 63)]
    [InlineData("Components.DeploySource.ContainerRegistry.Server", "", 1, 128)]
    [InlineData("Components.DeploySource.ContainerRegistry.Server", 129, 1, 128)]
    [InlineData("Components.DeploySource.ContainerRegistry.Username", "", 1, 63)]
    [InlineData("Components.DeploySource.ContainerRegistry.Username", 64, 1, 63)]
    [InlineData("Components.Name", 256, 1, 255)]
    public async Task PatchAsync_WithInvalidLengthParameter_ThrowsValidationException(string invalidParameterName, object invalidParameterValue, int validMinLengthValue, int validMaxLengthValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new ApplicationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var invalidStringValue = invalidParameterValue is int invalidLength
            ? new string('a', invalidLength)
            : (string)invalidParameterValue;
        var components = new List<PatchApplicationBody_components>()
        {
            new PatchApplicationBody_components()
            {
                DeploySource = new PatchApplicationBody_components_deploy_source()
                {
                    ContainerRegistry = new PatchApplicationBody_components_deploy_source_container_registry()
                    {
                        Image = invalidParameterName == "Components.DeploySource.ContainerRegistry.Image" ? invalidStringValue : "test-image",
                        Password = invalidParameterName == "Components.DeploySource.ContainerRegistry.Password" ? invalidStringValue : null,
                        Server = invalidParameterName == "Components.DeploySource.ContainerRegistry.Server" ? invalidStringValue : null,
                        Username = invalidParameterName == "Components.DeploySource.ContainerRegistry.Username" ? invalidStringValue : null
                    }
                },
                MaxCpu = new PatchApplicationBody_components_max_cpu(),
                MaxMemory = new PatchApplicationBody_components_max_memory(),
                Name = invalidParameterName == "Components.Name" ? invalidStringValue : "test-name",
                Probe = new PatchApplicationBody_components_probe()
                {
                    HttpGet = new PatchApplicationBody_components_probe_http_get()
                    {
                        Path = "/health",
                        Port = 1234
                    }
                }
            }
        };
        var body = new PatchApplicationBody
        {
            Components = components,
            MaxScale = 10,
            MinScale = 0,
            Port = 1234,
            ScaleTargetConcurrency = 100,
            TimeoutSeconds = 60
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PatchAsync(body);
        });

        var expectedMessage = $"{invalidParameterName}は{validMinLengthValue}文字以上{validMaxLengthValue}文字以下で入力してください。";
        Assert.Equal(expectedMessage, exception.Message);
        _mockLogger.Received(1).LogWarning(expectedMessage);
    }

    /// <summary>
    /// PatchAsyncのリクエストボディのパラメータの値が許可されていない場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("Port", 8012)]
    [InlineData("Port", 8013)]
    [InlineData("Port", 8022)]
    [InlineData("Port", 9090)]
    [InlineData("Port", 9091)]
    [InlineData("Port", 8008)]
    [InlineData("Components.Env.Key", "K_SERVICE")]
    [InlineData("Components.Env.Key", "K_CONFIGURATION")]
    [InlineData("Components.Env.Key", "K_REVISION")]
    [InlineData("Components.Env.Key", "PORT")]
    [InlineData("Components.Secret.Key", "K_SERVICE")]
    [InlineData("Components.Secret.Key", "K_CONFIGURATION")]
    [InlineData("Components.Secret.Key", "K_REVISION")]
    [InlineData("Components.Secret.Key", "PORT")]
    [InlineData("Components.Probe.HttpGet.Port", 8012)]
    [InlineData("Components.Probe.HttpGet.Port", 8013)]
    [InlineData("Components.Probe.HttpGet.Port", 8022)]
    [InlineData("Components.Probe.HttpGet.Port", 9090)]
    [InlineData("Components.Probe.HttpGet.Port", 9091)]
    [InlineData("Components.Probe.HttpGet.Port", 8008)]
    public async Task PatchAsync_WithDeniedValue_ThrowsValidationException(string invalidParameterName, object invalidParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "test-id" }
        };
        var builder = new ApplicationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var components = new List<PatchApplicationBody_components>()
        {
            new PatchApplicationBody_components()
            {
                DeploySource = new PatchApplicationBody_components_deploy_source()
                {
                    ContainerRegistry = new PatchApplicationBody_components_deploy_source_container_registry()
                    {
                        Image = "test-image"
                    }
                },
                Env = new List<EnvItem>()
                {
                    new EnvItem()
                    {
                        Key = invalidParameterName == "Components.Env.Key" ? (string)invalidParameterValue : "test-env-key",
                        Value = "test-env-value"
                    }
                },
                Secret = new List<PatchApplicationBody_components_secret>()
                {
                    new PatchApplicationBody_components_secret()
                    {
                        Key = invalidParameterName == "Components.Secret.Key" ? (string)invalidParameterValue : "test-secret-key"
                    }
                },
                MaxCpu = new PatchApplicationBody_components_max_cpu(),
                MaxMemory = new PatchApplicationBody_components_max_memory(),
                Name = "test-name",
                Probe = new PatchApplicationBody_components_probe()
                {
                    HttpGet = new PatchApplicationBody_components_probe_http_get()
                    {
                        Path = "/health",
                        Port = invalidParameterName == "Components.Probe.HttpGet.Port" ? (int)invalidParameterValue : 1234
                    }
                }
            }
        };
        var body = new PatchApplicationBody
        {
            Components = components,
            MaxScale = 10,
            MinScale = 0,
            Port = invalidParameterName == "Port" ? (int)invalidParameterValue : 1234,
            ScaleTargetConcurrency = 100,
            TimeoutSeconds = 60
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PatchAsync(body);
        });

        Assert.Equal($"{invalidParameterName}には許可された値を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}には許可された値を入力してください。");
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
        var builder = new ApplicationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PatchApplicationBody
        {
            Components = new List<PatchApplicationBody_components>()
            {
                new PatchApplicationBody_components()
                {
                    DeploySource = null,
                    MaxCpu = null,
                    MaxMemory = null,
                    Name = null
                }
            },
            MaxScale = 0,
            MinScale = 11,
            Port = 65536,
            ScaleTargetConcurrency = 49,
            TimeoutSeconds = 301
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PatchAsync(body);
        });

        var expectedMessage = """
            MaxScaleの値は1以上10以下で入力してください。
            MinScaleの値は0以上10以下で入力してください。
            Portの値は1以上65535以下で入力してください。
            ScaleTargetConcurrencyの値は50以上200以下で入力してください。
            TimeoutSecondsの値は1以上300以下で入力してください。
            Components.DeploySourceが入力されていません。
            Components.MaxCpuが入力されていません。
            Components.MaxMemoryが入力されていません。
            Components.Nameが入力されていません。
            """;
        Assert.Equal(expectedMessage.ReplaceLineEndings(), exception.Message.ReplaceLineEndings());
        _mockLogger.Received(1).LogWarning("MaxScaleの値は1以上10以下で入力してください。");
        _mockLogger.Received(1).LogWarning("MinScaleの値は0以上10以下で入力してください。");
        _mockLogger.Received(1).LogWarning("Portの値は1以上65535以下で入力してください。");
        _mockLogger.Received(1).LogWarning("ScaleTargetConcurrencyの値は50以上200以下で入力してください。");
        _mockLogger.Received(1).LogWarning("TimeoutSecondsの値は1以上300以下で入力してください。");
        _mockLogger.Received(1).LogWarning("Components.DeploySourceが入力されていません。");
        _mockLogger.Received(1).LogWarning("Components.MaxCpuが入力されていません。");
        _mockLogger.Received(1).LogWarning("Components.MaxMemoryが入力されていません。");
        _mockLogger.Received(1).LogWarning("Components.Nameが入力されていません。");
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
        var builder = new ApplicationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PatchApplicationBody
        {
            Components = null,
            MaxScale = 10,
            MinScale = 1,
            Port = 1234,
            ScaleTargetConcurrency = 50,
            TimeoutSeconds = 60
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

        requestAdapter.SendAsync<PatchApplication>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PatchApplication>>(),
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
                @"^アプリケーション変更のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var builder = new ApplicationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PatchApplicationBody
        {
            Components = null,
            MaxScale = 10,
            MinScale = 1,
            Port = 1234,
            ScaleTargetConcurrency = 50,
            TimeoutSeconds = 60
        };
        var expectedException = new Exception();

        requestAdapter.SendAsync<PatchApplication>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PatchApplication>>(),
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
                @"^アプリケーションの変更中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PatchAsyncがRequestAdapter.SendAsyncを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task PatchAsync_CallsRequestAdapterSendAsync()
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
        var builder = new ApplicationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PatchApplicationBody();
        var expectedResponse = new PatchApplication();

        requestAdapter.SendAsync<PatchApplication>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PatchApplication>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var result = await builder.PatchAsync(body);

        await requestAdapter.Received(1).SendAsync<PatchApplication>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PatchApplication>>(),
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
                    @"^アプリケーションの変更を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^アプリケーションの変更が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
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
        var builder = new ApplicationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PatchAsync(null));

        Assert.Equal("リクエストボディは必須です。", exception.Message);
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
        var builder = new ApplicationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var newUrl = "https://newapi.example.com/applications/new-id";

        var newBuilder = builder.WithUrl(newUrl);

        Assert.NotNull(newBuilder);
        Assert.NotSame(builder, newBuilder);
        Assert.IsType<ApplicationsItemRequestBuilder>(newBuilder);
    }
}
