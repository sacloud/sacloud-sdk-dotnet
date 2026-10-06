using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.IAM.ScimConfigurations.Item.RegenerateToken;
using Sacloud.Sdk.Shared.Models;

namespace Sacloud.Sdk.IAM.Tests.ScimConfigurations.Item.RegenerateToken;

/// <summary>
/// RegenerateTokenRequestBuilderのテスト
/// </summary>
public class RegenerateTokenRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<RegenerateTokenRequestBuilder> _mockLogger;

    public RegenerateTokenRequestBuilderTests()
    {
        _mockLogger = Substitute.For<ILogger<RegenerateTokenRequestBuilder>>();
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLoggerFactory.CreateLogger<RegenerateTokenRequestBuilder>().Returns(_mockLogger);
    }
    /// <summary>
    /// コンストラクタ（PathParameters版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void コンストラクタ_PathParameters版で正常に初期化される()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "12345678-1234-1234-1234-123456789012" }
        };

        var builder = new RegenerateTokenRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void コンストラクタ_rawUrl版で正常に初期化される()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var rawUrl = "https://api.example.com/scim-configurations/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx/regenerate-token";

        var builder = new RegenerateTokenRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// ToPostRequestInformationがPOSTメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_POSTメソッドのRequestInformationを返す()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "12345678-1234-1234-1234-123456789012" }
        };
        var builder = new RegenerateTokenRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToPostRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.POST, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToPostRequestInformationが正しいURLテンプレートを設定することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_URLテンプレートにregenerate_tokenを含む()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "12345678-1234-1234-1234-123456789012" }
        };
        var builder = new RegenerateTokenRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToPostRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Contains("regenerate-token", requestInfo.UrlTemplate);
    }

    /// <summary>
    /// ToPostRequestInformationがAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_Acceptヘッダーを設定する()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "12345678-1234-1234-1234-123456789012" }
        };
        var builder = new RegenerateTokenRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToPostRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
    }

    /// <summary>
    /// PostAsyncがrequestConfigurationを使用することを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_requestConfigurationを使用する()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<RegenerateTokenPostResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(new RegenerateTokenPostResponse());
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "12345678-1234-1234-1234-123456789012" }
        };
        var builder = new RegenerateTokenRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var result = await builder.PostAsync(config => { });

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<RegenerateTokenPostResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// PostAsyncがCancellationTokenを使用することを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_CancellationTokenを使用する()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var expectedResponse = new RegenerateTokenPostResponse();
        var cancellationToken = new CancellationToken();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<RegenerateTokenPostResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            cancellationToken
        ).Returns(expectedResponse);
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "12345678-1234-1234-1234-123456789012" }
        };
        var builder = new RegenerateTokenRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var result = await builder.PostAsync(cancellationToken: cancellationToken);

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<RegenerateTokenPostResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            cancellationToken
        );
    }

    /// <summary>
    /// PostAsyncがnull戻り値を処理できることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_nullのレスポンスでnullを返す()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<RegenerateTokenPostResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns((RegenerateTokenPostResponse?)null);
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "12345678-1234-1234-1234-123456789012" }
        };
        var builder = new RegenerateTokenRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var result = await builder.PostAsync();

        Assert.Null(result);
    }

    /// <summary>
    /// WithUrlが新しいRegenerateTokenRequestBuilderのインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_正しいインスタンスを返す()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "12345678-1234-1234-1234-123456789012" }
        };
        var builder = new RegenerateTokenRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var newBuilder = builder.WithUrl("https://api.example.com/scim-configurations/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx/regenerate-token");

        Assert.NotNull(newBuilder);
        Assert.IsType<RegenerateTokenRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }

    /// <summary>
    /// コンストラクタ（PathParameters版）にnullのLoggerFactoryを渡すとArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void コンストラクタ_PathParameters版でnullのLoggerFactoryはArgumentNullExceptionをスロー()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "12345678-1234-1234-1234-123456789012" }
        };

        Assert.Throws<ArgumentNullException>(() =>
            new RegenerateTokenRequestBuilder(pathParameters, requestAdapter, null));
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）にnullのLoggerFactoryを渡すとArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void コンストラクタ_rawUrl版でnullのLoggerFactoryはArgumentNullExceptionをスロー()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var rawUrl = "https://api.example.com/scim-configurations/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx/regenerate-token";

        Assert.Throws<ArgumentNullException>(() =>
            new RegenerateTokenRequestBuilder(rawUrl, requestAdapter, null));
    }

    /// <summary>
    /// PostAsyncのパスパラメータにidがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("id", "")]
    [InlineData("id", " ")]
    [InlineData("id", "　")]
    public async Task PostAsync_無効なパスパラメータでValidationExceptionをスロー(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { pathParameterKey, pathParameterValue }
        };
        var builder = new RegenerateTokenRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync());

        Assert.Equal("ユーザープロビジョニングIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("ユーザープロビジョニングIDを入力してください。");
    }

    /// <summary>
    /// PostAsyncのパスパラメータidがGUID形式でない場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("test-value")]
    [InlineData("1234-567890ab-cdef-1234-567890abcdef")]
    [InlineData("12345678-90ab-cdef-1234-567890ghijkl")]
    public async Task PostAsync_GUID形式でないidでValidationExceptionをスロー(string invalidId)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", invalidId }
        };
        var builder = new RegenerateTokenRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync());

        Assert.Equal("ユーザープロビジョニングIDはGUID形式で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("ユーザープロビジョニングIDはGUID形式で入力してください。");
    }

    /// <summary>
    /// PostAsyncが開始ログと完了ログを出力することを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_開始ログと完了ログを出力する()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var expectedResponse = new RegenerateTokenPostResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<RegenerateTokenPostResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "12345678-1234-1234-1234-123456789012" }
        };
        var builder = new RegenerateTokenRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await builder.PostAsync();

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^ユーザープロビジョニングのシークレットトークン再発行を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^ユーザープロビジョニングのシークレットトークン再発行が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncでAPIエラーが発生した場合にエラーログを出力することを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_APIエラー時にエラーログを出力する()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "12345678-1234-1234-1234-123456789012" }
        };
        var builder = new RegenerateTokenRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError { ResponseStatusCode = 401 };
        ((IAdditionalDataHolder)expectedException).AdditionalData = new Dictionary<string, object>
        {
            { "error_code", "unauthorized" },
            { "error_msg", "error-unauthorized" },
            { "is_fatal", true },
            { "serial", "1234567890" },
            { "status", "401 Unauthorized" }
        };
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<RegenerateTokenPostResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(() => builder.PostAsync());

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^ユーザープロビジョニングシークレットトークン再発行のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncでその他のエラーが発生した場合にエラーログを出力することを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_その他エラー時にエラーログを出力する()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "12345678-1234-1234-1234-123456789012" }
        };
        var builder = new RegenerateTokenRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<RegenerateTokenPostResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(new Exception());

        await Assert.ThrowsAsync<Exception>(() => builder.PostAsync());

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^ユーザープロビジョニングのシークレットトークン再発行中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }
}


