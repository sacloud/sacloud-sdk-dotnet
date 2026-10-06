using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.IAM.MoveFolders;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Sacloud.Sdk.Shared.Models;

namespace Sacloud.Sdk.IAM.Tests.MoveFolders;

/// <summary>
/// MoveFoldersRequestBuilderのテスト
/// </summary>
public class MoveFoldersRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<MoveFoldersRequestBuilder> _mockLogger;

    public MoveFoldersRequestBuilderTests()
    {
        _mockLogger = Substitute.For<ILogger<MoveFoldersRequestBuilder>>();
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLoggerFactory.CreateLogger<MoveFoldersRequestBuilder>().Returns(_mockLogger);
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
            { "baseurl", "https://api.example.com" }
        };

        Assert.Throws<ArgumentNullException>(() => new MoveFoldersRequestBuilder(pathParameters, requestAdapter, null));
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）の初期化でLoggerFactoryがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrlAndNullLoggerFactory_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var rawUrl = "https://api.example.com/move-folders";

        Assert.Throws<ArgumentNullException>(() => new MoveFoldersRequestBuilder(rawUrl, requestAdapter, null));
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

        var builder = new MoveFoldersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var rawUrl = "https://api.example.com/move-folders";

        var builder = new MoveFoldersRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new MoveFoldersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new IAM.Models.MoveFolders
        {
            FolderIds = [1, 2, 3],
            ParentId = 100
        };

        var requestInfo = builder.ToPostRequestInformation(body);

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.POST, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToPostRequestInformationが正しいAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_SetsAcceptHeader()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new MoveFoldersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new IAM.Models.MoveFolders
        {
            FolderIds = [1, 2, 3],
            ParentId = 100
        };

        var requestInfo = builder.ToPostRequestInformation(body);

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// ToPostRequestInformationが正しいURLテンプレートを使用することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_UsesCorrectUrlTemplate()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new MoveFoldersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new IAM.Models.MoveFolders
        {
            FolderIds = [1, 2, 3],
            ParentId = 100
        };

        var requestInfo = builder.ToPostRequestInformation(body);

        Assert.NotNull(requestInfo.UrlTemplate);
        Assert.Equal("{+baseurl}/move-folders", requestInfo.UrlTemplate);
    }

    /// <summary>
    /// PostAsyncがnullボディで例外をスローすることを確認
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
        var builder = new MoveFoldersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await Assert.ThrowsAsync<ValidationException>(() => 
            builder.PostAsync(null));
    }

    /// <summary>
    /// PostAsyncが正しくRequestAdapterを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithValidBody_CallsRequestAdapter()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new MoveFoldersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new IAM.Models.MoveFolders
        {
            FolderIds = [1, 2, 3],
            ParentId = 100
        };

        await builder.PostAsync(body);

        await requestAdapter.Received(1).SendNoContentAsync(
            Arg.Is<RequestInformation>(r => r.HttpMethod == Method.POST),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^フォルダの移動を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^フォルダの移動が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 204\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncがエラーマッピングを正しく設定することを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_ConfiguresErrorMapping()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new MoveFoldersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new IAM.Models.MoveFolders
        {
            FolderIds = [1, 2, 3],
            ParentId = 100
        };

        await builder.PostAsync(body);

        await requestAdapter.Received(1).SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Is<Dictionary<string, ParsableFactory<IParsable>>>(em => em.ContainsKey("XXX")),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// PostAsyncがCancellationTokenを正しく伝達することを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_PassesCancellationToken()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new MoveFoldersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new IAM.Models.MoveFolders
        {
            FolderIds = [1, 2, 3],
            ParentId = 100
        };
        var cancellationToken = new CancellationToken();

        await builder.PostAsync(body, cancellationToken: cancellationToken);

        await requestAdapter.Received(1).SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            cancellationToken);
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
        var builder = new MoveFoldersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new IAM.Models.MoveFolders
        {
            FolderIds = [1, 2, 3],
            ParentId = 100
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
            await builder.PostAsync(body);
        });

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^フォルダ移動のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
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
        var builder = new MoveFoldersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new IAM.Models.MoveFolders
        {
            FolderIds = [1, 2, 3],
            ParentId = 100
        };
        var expectedException = new Exception("Unexpected error");

        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
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
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^フォルダの移動中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncにnullが含まれるリクエストボディが渡された場合にValidationExceptionをスローすることを確認
    /// </summary>
    [Theory]
    [InlineData("FolderIds")]
    [InlineData("ParentId")]
    public async Task PostAsync_WithNullBodyParameter_ThrowsValidationException(string nullParam)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new MoveFoldersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new IAM.Models.MoveFolders
        {
            FolderIds = nullParam == "FolderIds" ? null : [1, 2, 3],
            ParentId = nullParam == "ParentId" ? null : (long?)100
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));
        Assert.Equal($"{nullParam}が入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{nullParam}が入力されていません。");
    }

    /// <summary>
    /// WithUrlが正しいRawUrlで新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewInstanceWithRawUrl()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new MoveFoldersRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var newRawUrl = "https://api2.example.com/move-folders";

        var newBuilder = builder.WithUrl(newRawUrl);

        Assert.NotNull(newBuilder);
        Assert.NotSame(builder, newBuilder);
        Assert.IsType<MoveFoldersRequestBuilder>(newBuilder);
    }
}
