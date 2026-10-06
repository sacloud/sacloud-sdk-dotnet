using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.KMS.Kms.Keys.Item.ScheduleDestruction;
using Sacloud.Sdk.KMS.Models;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.KMS.Tests.Kms.Keys.Item.ScheduleDestruction;

/// <summary>
/// ScheduleDestructionRequestBuilderのテスト
/// </summary>
public class ScheduleDestructionRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<ScheduleDestructionRequestBuilder> _mockLogger;

    public ScheduleDestructionRequestBuilderTests()
    {
        _mockLogger = Substitute.For<ILogger<ScheduleDestructionRequestBuilder>>();
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLoggerFactory.CreateLogger(Arg.Any<string>()).Returns(_mockLogger);
    }

    /// <summary>
    /// コンストラクタ（PathParameters版）の初期化でLoggerFactoryがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParametersAndNullLoggerFactory_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "resource_id", "test-resource-id" }
        };

        Assert.Throws<ArgumentNullException>(() => new ScheduleDestructionRequestBuilder(pathParameters, requestAdapter, null!));
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）の初期化でLoggerFactoryがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrlAndNullLoggerFactory_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var rawUrl = "https://api.example.com/kms/keys/test-resource-id/schedule-destruction";

        Assert.Throws<ArgumentNullException>(() => new ScheduleDestructionRequestBuilder(rawUrl, requestAdapter, null!));
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
            { "resource_id", "test-resource-id" }
        };

        var builder = new ScheduleDestructionRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var rawUrl = "https://api.example.com/kms/keys/test-resource-id/schedule-destruction";

        var builder = new ScheduleDestructionRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// ToPostRequestInformationがPOSTメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_CreatesRequestInformationWithPostMethod()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "resource_id", "test-resource-id" }
        };
        var builder = new ScheduleDestructionRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new WrappedScheduleDestructionKey();

        var requestInfo = builder.ToPostRequestInformation(body);

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.POST, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToPostRequestInformationでリクエストボディがnullの場合にArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_WithNullBody_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "resource_id", "test-resource-id" }
        };
        var builder = new ScheduleDestructionRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.Throws<ArgumentNullException>(() => builder.ToPostRequestInformation(null!));
    }

    /// <summary>
    /// WithUrlが新しいScheduleDestructionRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewScheduleDestructionRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "resource_id", "test-resource-id" }
        };
        var builder = new ScheduleDestructionRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var newBuilder = builder.WithUrl("https://api.example.com/kms/keys/new-resource-id/schedule-destruction");

        Assert.NotNull(newBuilder);
        Assert.IsType<ScheduleDestructionRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }

    /// <summary>
    /// PostAsyncが結果を返すことを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_ReturnsResult()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "resource_id", "test-resource-id" }
        };
        var builder = new ScheduleDestructionRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = CreateValidWrappedScheduleDestructionKey();
        var expectedResult = new MemoryStream();
        requestAdapter.SendPrimitiveAsync<Stream>(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(expectedResult);

        var result = await builder.PostAsync(body);

        Assert.NotNull(result);
        Assert.Same(expectedResult, result);
    }

    /// <summary>
    /// PostAsyncが開始・成功ログを出力することを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_LogsStartAndSuccess()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "resource_id", "test-resource-id" }
        };
        var builder = new ScheduleDestructionRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = CreateValidWrappedScheduleDestructionKey();
        var expectedResult = new MemoryStream();
        requestAdapter.SendPrimitiveAsync<Stream>(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(expectedResult);

        await builder.PostAsync(body);

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^キーの削除予約を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^キーの削除予約が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncでAPIエラーが発生した場合にエラーログを出力することを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithApiError_LogsError()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "resource_id", "test-resource-id" }
        };
        var builder = new ScheduleDestructionRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = CreateValidWrappedScheduleDestructionKey();
        var expectedException = new ApiError { ResponseStatusCode = 400 };

        requestAdapter.SendPrimitiveAsync<Stream>(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(async () => await builder.PostAsync(body));

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => state.ToString()!.Contains("キー削除予約のAPIエラーが発生しました。")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncでその他のエラーが発生した場合にエラーログを出力することを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithOtherError_LogsError()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "resource_id", "test-resource-id" }
        };
        var builder = new ScheduleDestructionRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = CreateValidWrappedScheduleDestructionKey();
        var expectedException = new Exception("Test error");

        requestAdapter.SendPrimitiveAsync<Stream>(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<Exception>(async () => await builder.PostAsync(body));

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^キーの削除予約中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncのパスパラメータにresource_idがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("resource_id", "")]
    [InlineData("resource_id", " ")]
    [InlineData("resource_id", "　")]
    public async Task PostAsync_WithInvalidPathParameters_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { pathParameterKey, pathParameterValue }
        };
        var builder = new ScheduleDestructionRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new WrappedScheduleDestructionKey();

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        Assert.Equal("リソースIDを入力してください。", exception.Message);
        _mockLogger.Received(1).Log(
            LogLevel.Warning,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => state.ToString() == "リソースIDを入力してください。"),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncでリクエストボディがnullの場合にValidationExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithNullBody_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "resource_id", "test-resource-id" }
        };
        var builder = new ScheduleDestructionRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(null!);
        });

        Assert.Equal("リクエストボディは必須です。", exception.Message);
        _mockLogger.Received(1).Log(
            LogLevel.Warning,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => state.ToString() == "リクエストボディは必須です。"),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncのリクエストボディのKeyパラメータがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithNullKeyParameter_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "resource_id", "test-resource-id" }
        };
        var builder = new ScheduleDestructionRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new WrappedScheduleDestructionKey
        {
            Key = null
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        Assert.Equal("Keyが入力されていません。", exception.Message);
    }

    /// <summary>
    /// PostAsyncのリクエストボディのKey.PendingDaysパラメータがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithNullPendingDaysParameter_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "resource_id", "test-resource-id" }
        };
        var builder = new ScheduleDestructionRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new WrappedScheduleDestructionKey
        {
            Key = new ScheduleDestructionKey
            {
                PendingDays = null
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PostAsync(body);
        });

        Assert.Equal("PendingDaysが入力されていません。", exception.Message);
    }

    /// <summary>
    /// バリデーションを通過する有効なWrappedScheduleDestructionKeyを作成するヘルパーメソッド
    /// </summary>
    private static WrappedScheduleDestructionKey CreateValidWrappedScheduleDestructionKey()
    {
        return new WrappedScheduleDestructionKey
        {
            Key = new ScheduleDestructionKey
            {
                PendingDays = 7
            }
        };
    }
}
