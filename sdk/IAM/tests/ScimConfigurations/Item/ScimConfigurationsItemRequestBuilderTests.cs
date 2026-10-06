using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.IAM.Models;
using Sacloud.Sdk.IAM.ScimConfigurations.Item;
using Sacloud.Sdk.IAM.ScimConfigurations.Item.RegenerateToken;
using Sacloud.Sdk.Shared.Models;

namespace Sacloud.Sdk.IAM.Tests.ScimConfigurations.Item;

/// <summary>
/// ScimConfigurationsItemRequestBuilderのテスト
/// </summary>
public class ScimConfigurationsItemRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<ScimConfigurationsItemRequestBuilder> _mockLogger;

    public ScimConfigurationsItemRequestBuilderTests()
    {
        _mockLogger = Substitute.For<ILogger<ScimConfigurationsItemRequestBuilder>>();
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLoggerFactory.CreateLogger<ScimConfigurationsItemRequestBuilder>().Returns(_mockLogger);
    }
    private static IRequestAdapter CreateRequestAdapter()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        return requestAdapter;
    }

    private static Dictionary<string, object> CreatePathParameters()
    {
        return new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "12345678-1234-1234-1234-123456789012" }
        };
    }

    /// <summary>
    /// コンストラクタ（PathParameters版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void コンストラクタ_PathParameters版で正常に初期化される()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = CreatePathParameters();

        var builder = new ScimConfigurationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void コンストラクタ_rawUrl版で正常に初期化される()
    {
        var requestAdapter = CreateRequestAdapter();
        var rawUrl = "https://api.example.com/scim-configurations/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";

        var builder = new ScimConfigurationsItemRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// RegenerateTokenプロパティが正しいRegenerateTokenRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void RegenerateTokenプロパティ_RegenerateTokenRequestBuilderを返す()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = CreatePathParameters();
        var builder = new ScimConfigurationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var regenerateTokenBuilder = builder.RegenerateToken;

        Assert.NotNull(regenerateTokenBuilder);
        Assert.IsType<RegenerateTokenRequestBuilder>(regenerateTokenBuilder);
    }

    /// <summary>
    /// ToGetRequestInformationがGETメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_GETメソッドのRequestInformationを返す()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = CreatePathParameters();
        var builder = new ScimConfigurationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.GET, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToGetRequestInformationが正しいURLテンプレートを設定することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_URLテンプレートにscim_configurationsを含む()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = CreatePathParameters();
        var builder = new ScimConfigurationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Contains("scim-configurations", requestInfo.UrlTemplate);
    }

    /// <summary>
    /// ToGetRequestInformationがAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_Acceptヘッダーを設定する()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = CreatePathParameters();
        var builder = new ScimConfigurationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
    }

    /// <summary>
    /// ToPutRequestInformationがPUTメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_PUTメソッドのRequestInformationを返す()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = CreatePathParameters();
        var builder = new ScimConfigurationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ScimConfigurationsPutRequestBody { Name = "テスト" };

        var requestInfo = builder.ToPutRequestInformation(body);

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.PUT, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToPutRequestInformationが正しいURLテンプレートを設定することを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_URLテンプレートにscim_configurationsを含む()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = CreatePathParameters();
        var builder = new ScimConfigurationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ScimConfigurationsPutRequestBody { Name = "テスト" };

        var requestInfo = builder.ToPutRequestInformation(body);

        Assert.Contains("scim-configurations", requestInfo.UrlTemplate);
    }

    /// <summary>
    /// ToPutRequestInformationがAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_Acceptヘッダーを設定する()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = CreatePathParameters();
        var builder = new ScimConfigurationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ScimConfigurationsPutRequestBody { Name = "テスト" };

        var requestInfo = builder.ToPutRequestInformation(body);

        Assert.NotNull(requestInfo);
        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
    }

    /// <summary>
    /// ToPutRequestInformationがContent-Typeヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_ContentTypeヘッダーを設定する()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = CreatePathParameters();
        var builder = new ScimConfigurationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ScimConfigurationsPutRequestBody { Name = "テスト" };

        var requestInfo = builder.ToPutRequestInformation(body);

        Assert.NotNull(requestInfo);
        Assert.True(requestInfo.Headers.ContainsKey("Content-Type"));
    }

    /// <summary>
    /// ToDeleteRequestInformationがDELETEメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToDeleteRequestInformation_DELETEメソッドのRequestInformationを返す()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = CreatePathParameters();
        var builder = new ScimConfigurationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToDeleteRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.DELETE, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToDeleteRequestInformationが正しいURLテンプレートを設定することを確認
    /// </summary>
    [Fact]
    public void ToDeleteRequestInformation_URLテンプレートにscim_configurationsを含む()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = CreatePathParameters();
        var builder = new ScimConfigurationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToDeleteRequestInformation();

        Assert.Contains("scim-configurations", requestInfo.UrlTemplate);
    }

    /// <summary>
    /// ToDeleteRequestInformationがAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToDeleteRequestInformation_Acceptヘッダーを設定する()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = CreatePathParameters();
        var builder = new ScimConfigurationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToDeleteRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
    }

    /// <summary>
    /// GetAsyncがrequestConfigurationを使用することを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_requestConfigurationを使用する()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = CreatePathParameters();
        var builder = new ScimConfigurationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ScimConfigurationPublic>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(new ScimConfigurationPublic());

        var result = await builder.GetAsync(config => { });

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ScimConfigurationPublic>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// GetAsyncがCancellationTokenを使用することを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_CancellationTokenを使用する()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = CreatePathParameters();
        var builder = new ScimConfigurationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new ScimConfigurationPublic();
        var cancellationToken = new CancellationToken();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ScimConfigurationPublic>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            cancellationToken
        ).Returns(expectedResponse);

        var result = await builder.GetAsync(cancellationToken: cancellationToken);

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ScimConfigurationPublic>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            cancellationToken
        );
    }

    /// <summary>
    /// GetAsyncがnull戻り値を処理できることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_nullのレスポンスでnullを返す()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = CreatePathParameters();
        var builder = new ScimConfigurationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ScimConfigurationPublic>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns((ScimConfigurationPublic?)null);

        var result = await builder.GetAsync();

        Assert.Null(result);
    }

    /// <summary>
    /// PutAsyncがrequestConfigurationを使用することを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_requestConfigurationを使用する()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = CreatePathParameters();
        var builder = new ScimConfigurationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ScimConfigurationsPutRequestBody { Name = "テスト" };
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ScimConfigurationPublic>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(new ScimConfigurationPublic());

        var result = await builder.PutAsync(body, config => { });

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ScimConfigurationPublic>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// PutAsyncがCancellationTokenを使用することを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_CancellationTokenを使用する()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = CreatePathParameters();
        var builder = new ScimConfigurationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ScimConfigurationsPutRequestBody { Name = "テスト" };
        var expectedResponse = new ScimConfigurationPublic();
        var cancellationToken = new CancellationToken();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ScimConfigurationPublic>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            cancellationToken
        ).Returns(expectedResponse);

        var result = await builder.PutAsync(body, cancellationToken: cancellationToken);

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ScimConfigurationPublic>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            cancellationToken
        );
    }

    /// <summary>
    /// PutAsyncがnull戻り値を処理できることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_nullのレスポンスでnullを返す()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = CreatePathParameters();
        var builder = new ScimConfigurationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ScimConfigurationsPutRequestBody { Name = "テスト" };
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ScimConfigurationPublic>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns((ScimConfigurationPublic?)null);

        var result = await builder.PutAsync(body);

        Assert.Null(result);
    }

    /// <summary>
    /// DeleteAsyncがrequestConfigurationを使用することを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_requestConfigurationを使用する()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = CreatePathParameters();
        var builder = new ScimConfigurationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await builder.DeleteAsync(config => { });

        await requestAdapter.Received(1).SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// DeleteAsyncがCancellationTokenを使用することを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_CancellationTokenを使用する()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = CreatePathParameters();
        var builder = new ScimConfigurationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var cancellationToken = new CancellationToken();

        await builder.DeleteAsync(cancellationToken: cancellationToken);

        await requestAdapter.Received(1).SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            cancellationToken
        );
    }

    /// <summary>
    /// ToPutRequestInformationにnullのbodyを渡すとArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_nullのbodyでArgumentNullExceptionをスロー()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = CreatePathParameters();
        var builder = new ScimConfigurationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.Throws<ArgumentNullException>(() => builder.ToPutRequestInformation(null));
    }

    /// <summary>
    /// PutAsyncにnullのbodyを渡すとValidationExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_nullのbodyでValidationExceptionをスロー()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = CreatePathParameters();
        var builder = new ScimConfigurationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await Assert.ThrowsAsync<ValidationException>(async () => await builder.PutAsync(null));
    }

    /// <summary>
    /// WithUrlが新しいScimConfigurationsItemRequestBuilderのインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_正しいインスタンスを返す()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = CreatePathParameters();
        var builder = new ScimConfigurationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var newBuilder = builder.WithUrl("https://api.example.com/scim-configurations/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");

        Assert.NotNull(newBuilder);
        Assert.IsType<ScimConfigurationsItemRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }

    /// <summary>
    /// コンストラクタ（PathParameters版）にnullのLoggerFactoryを渡すとArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void コンストラクタ_PathParameters版でnullのLoggerFactoryはArgumentNullExceptionをスロー()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = CreatePathParameters();

        Assert.Throws<ArgumentNullException>(() =>
            new ScimConfigurationsItemRequestBuilder(pathParameters, requestAdapter, null));
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）にnullのLoggerFactoryを渡すとArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void コンストラクタ_rawUrl版でnullのLoggerFactoryはArgumentNullExceptionをスロー()
    {
        var requestAdapter = CreateRequestAdapter();
        var rawUrl = "https://api.example.com/scim-configurations/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";

        Assert.Throws<ArgumentNullException>(() =>
            new ScimConfigurationsItemRequestBuilder(rawUrl, requestAdapter, null));
    }

    /// <summary>
    /// GetAsyncのパスパラメータにidがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("id", "")]
    [InlineData("id", " ")]
    [InlineData("id", "　")]
    public async Task GetAsync_無効なパスパラメータでValidationExceptionをスロー(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { pathParameterKey, pathParameterValue }
        };
        var builder = new ScimConfigurationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync());

        Assert.Equal("ユーザープロビジョニングIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("ユーザープロビジョニングIDを入力してください。");
    }

    /// <summary>
    /// GetAsyncのパスパラメータidがGUID形式でない場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("test-value")]
    [InlineData("1234-567890ab-cdef-1234-567890abcdef")]
    [InlineData("12345678-90ab-cdef-1234-567890ghijkl")]
    public async Task GetAsync_GUID形式でないidでValidationExceptionをスロー(string invalidId)
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", invalidId }
        };
        var builder = new ScimConfigurationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync());

        Assert.Equal("ユーザープロビジョニングIDはGUID形式で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("ユーザープロビジョニングIDはGUID形式で入力してください。");
    }

    /// <summary>
    /// PutAsyncのパスパラメータにidがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("id", "")]
    [InlineData("id", " ")]
    [InlineData("id", "　")]
    public async Task PutAsync_無効なパスパラメータでValidationExceptionをスロー(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { pathParameterKey, pathParameterValue }
        };
        var builder = new ScimConfigurationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ScimConfigurationsPutRequestBody { Name = "テスト" };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal("ユーザープロビジョニングIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("ユーザープロビジョニングIDを入力してください。");
    }

    /// <summary>
    /// PutAsyncのパスパラメータidがGUID形式でない場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("test-value")]
    [InlineData("1234-567890ab-cdef-1234-567890abcdef")]
    [InlineData("12345678-90ab-cdef-1234-567890ghijkl")]
    public async Task PutAsync_GUID形式でないidでValidationExceptionをスロー(string invalidId)
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", invalidId }
        };
        var builder = new ScimConfigurationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ScimConfigurationsPutRequestBody { Name = "テスト" };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal("ユーザープロビジョニングIDはGUID形式で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("ユーザープロビジョニングIDはGUID形式で入力してください。");
    }

    /// <summary>
    /// DeleteAsyncのパスパラメータにidがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("id", "")]
    [InlineData("id", " ")]
    [InlineData("id", "　")]
    public async Task DeleteAsync_無効なパスパラメータでValidationExceptionをスロー(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { pathParameterKey, pathParameterValue }
        };
        var builder = new ScimConfigurationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.DeleteAsync());

        Assert.Equal("ユーザープロビジョニングIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("ユーザープロビジョニングIDを入力してください。");
    }

    /// <summary>
    /// DeleteAsyncのパスパラメータidがGUID形式でない場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("test-value")]
    [InlineData("1234-567890ab-cdef-1234-567890abcdef")]
    [InlineData("12345678-90ab-cdef-1234-567890ghijkl")]
    public async Task DeleteAsync_GUID形式でないidでValidationExceptionをスロー(string invalidId)
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", invalidId }
        };
        var builder = new ScimConfigurationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.DeleteAsync());

        Assert.Equal("ユーザープロビジョニングIDはGUID形式で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("ユーザープロビジョニングIDはGUID形式で入力してください。");
    }

    /// <summary>
    /// PutAsyncにNameが無効なbodyを渡すとValidationExceptionがスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task PutAsync_無効なNameでValidationExceptionをスロー(string? name)
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = CreatePathParameters();
        var builder = new ScimConfigurationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ScimConfigurationsPutRequestBody { Name = name };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal("nameが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("nameが入力されていません。");
    }

    /// <summary>
    /// GetAsyncが開始ログと完了ログを出力することを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_開始ログと完了ログを出力する()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = CreatePathParameters();
        var builder = new ScimConfigurationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new ScimConfigurationPublic();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ScimConfigurationPublic>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        await builder.GetAsync();

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^ユーザープロビジョニングの取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^ユーザープロビジョニングの取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncでAPIエラーが発生した場合にエラーログを出力することを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_APIエラー時にエラーログを出力する()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = CreatePathParameters();
        var builder = new ScimConfigurationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError { ResponseStatusCode = 404 };
        ((IAdditionalDataHolder)expectedException).AdditionalData = new Dictionary<string, object>
        {
            { "error_code", "not_found" },
            { "error_msg", "error-not-found" },
            { "is_fatal", false },
            { "serial", "1234567890" },
            { "status", "404 Not Found" }
        };
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ScimConfigurationPublic>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(() => builder.GetAsync());

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^ユーザープロビジョニング取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 404\], \[error_code, not_found\], \[error_msg, error\-not\-found\], \[is_fatal, False\], \[serial, 1234567890\], \[status, 404 Not Found\]$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncでその他のエラーが発生した場合にエラーログを出力することを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_その他エラー時にエラーログを出力する()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = CreatePathParameters();
        var builder = new ScimConfigurationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ScimConfigurationPublic>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(new Exception());

        await Assert.ThrowsAsync<Exception>(() => builder.GetAsync());

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^ユーザープロビジョニングの取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PutAsyncが開始ログと完了ログを出力することを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_開始ログと完了ログを出力する()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = CreatePathParameters();
        var builder = new ScimConfigurationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ScimConfigurationsPutRequestBody { Name = "テスト" };
        var expectedResponse = new ScimConfigurationPublic();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ScimConfigurationPublic>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        await builder.PutAsync(body);

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^ユーザープロビジョニングの更新を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^ユーザープロビジョニングの更新が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PutAsyncでAPIエラーが発生した場合にエラーログを出力することを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_APIエラー時にエラーログを出力する()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = CreatePathParameters();
        var builder = new ScimConfigurationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ScimConfigurationsPutRequestBody { Name = "テスト" };
        var expectedException = new ApiError { ResponseStatusCode = 400 };
        ((IAdditionalDataHolder)expectedException).AdditionalData = new Dictionary<string, object>
        {
            { "error_code", "bad_request" },
            { "error_msg", "error-bad-request" },
            { "is_fatal", false },
            { "serial", "1234567890" },
            { "status", "400 Bad Request" }
        };
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ScimConfigurationPublic>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(() => builder.PutAsync(body));

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^ユーザープロビジョニング更新のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 400\], \[error_code, bad_request\], \[error_msg, error\-bad\-request\], \[is_fatal, False\], \[serial, 1234567890\], \[status, 400 Bad Request\]$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PutAsyncでその他のエラーが発生した場合にエラーログを出力することを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_その他エラー時にエラーログを出力する()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = CreatePathParameters();
        var builder = new ScimConfigurationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ScimConfigurationsPutRequestBody { Name = "テスト" };
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ScimConfigurationPublic>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(new Exception());

        await Assert.ThrowsAsync<Exception>(() => builder.PutAsync(body));

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^ユーザープロビジョニングの更新中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// DeleteAsyncが開始ログと完了ログを出力することを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_開始ログと完了ログを出力する()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = CreatePathParameters();
        var builder = new ScimConfigurationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await builder.DeleteAsync();

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^ユーザープロビジョニングの削除を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^ユーザープロビジョニングの削除が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 204\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// DeleteAsyncでAPIエラーが発生した場合にエラーログを出力することを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_APIエラー時にエラーログを出力する()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = CreatePathParameters();
        var builder = new ScimConfigurationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError { ResponseStatusCode = 404 };
        ((IAdditionalDataHolder)expectedException).AdditionalData = new Dictionary<string, object>
        {
            { "error_code", "not_found" },
            { "error_msg", "error-not-found" },
            { "is_fatal", false },
            { "serial", "1234567890" },
            { "status", "404 Not Found" }
        };
        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(() => builder.DeleteAsync());

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^ユーザープロビジョニング削除のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 404\], \[error_code, not_found\], \[error_msg, error\-not\-found\], \[is_fatal, False\], \[serial, 1234567890\], \[status, 404 Not Found\]$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// DeleteAsyncでその他のエラーが発生した場合にエラーログを出力することを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_その他エラー時にエラーログを出力する()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = CreatePathParameters();
        var builder = new ScimConfigurationsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(new Exception());

        await Assert.ThrowsAsync<Exception>(() => builder.DeleteAsync());

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^ユーザープロビジョニングの削除中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }
}



