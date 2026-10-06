using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.NetworkingSuite.Models;
using Sacloud.Sdk.NetworkingSuite.NetworkingSuite.SubnetGroups.Item;
using Sacloud.Sdk.Shared.Models;

namespace Sacloud.Sdk.NetworkingSuite.Tests.NetworkingSuite.SubnetGroups.Item;

/// <summary>
/// WithSubnetgroupItemRequestBuilderのテスト
/// </summary>
public class WithSubnetgroupItemRequestBuilderTests
{
    private readonly IRequestAdapter _mockRequestAdapter;
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<WithSubnetgroupItemRequestBuilder> _mockLogger;

    public WithSubnetgroupItemRequestBuilderTests()
    {
        _mockRequestAdapter = Substitute.For<IRequestAdapter>();
        _mockRequestAdapter.BaseUrl = "https://api.example.com";
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<WithSubnetgroupItemRequestBuilder>>();
        _mockLoggerFactory.CreateLogger<WithSubnetgroupItemRequestBuilder>().Returns(_mockLogger);
    }

    /// <summary>
    /// コンストラクタ（PathParameters版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_InitializesSuccessfully()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "subnetgroupid", "test-subnetgroupid" },
        };

        var builder = new WithSubnetgroupItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（PathParameters版）がnullのLoggerFactoryで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_WithNullLoggerFactory_ThrowsArgumentNullException()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "subnetgroupid", "test-subnetgroupid" },
        };

        Assert.Throws<ArgumentNullException>(() => new WithSubnetgroupItemRequestBuilder(pathParameters, _mockRequestAdapter, null));
    }

    /// <summary>
    /// コンストラクタ（RawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_InitializesSuccessfully()
    {
        var rawUrl = "https://api.example.com/networking-suite/subnet-groups/test-subnetgroupid";

        var builder = new WithSubnetgroupItemRequestBuilder(rawUrl, _mockRequestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（RawUrl版）がnullのLoggerFactoryで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_WithNullLoggerFactory_ThrowsArgumentNullException()
    {
        var rawUrl = "https://api.example.com/networking-suite/subnet-groups/test-subnetgroupid";

        Assert.Throws<ArgumentNullException>(() => new WithSubnetgroupItemRequestBuilder(rawUrl, _mockRequestAdapter, null));
    }

    /// <summary>
    /// ToDeleteRequestInformationがDELETEメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToDeleteRequestInformation_CreatesRequestInformationWithDeleteMethod()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "subnetgroupid", "test-subnetgroupid" },
        };
        var builder = new WithSubnetgroupItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToDeleteRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.DELETE, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToDeleteRequestInformationが正しいAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToDeleteRequestInformation_SetsAcceptHeader()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "subnetgroupid", "test-subnetgroupid" },
        };
        var builder = new WithSubnetgroupItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToDeleteRequestInformation();

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// ToDeleteRequestInformationが正しいPathParametersを設定することを確認
    /// </summary>
    [Fact]
    public void ToDeleteRequestInformation_SetsPathParameters()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "subnetgroupid", "test-subnetgroupid" },
        };
        var builder = new WithSubnetgroupItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToDeleteRequestInformation();

        Assert.True(requestInfo.PathParameters.ContainsKey("subnetgroupid"));
        Assert.Equal("test-subnetgroupid", requestInfo.PathParameters["subnetgroupid"]);
    }

    /// <summary>
    /// ToGetRequestInformationがGETメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_CreatesRequestInformationWithGetMethod()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "subnetgroupid", "test-subnetgroupid" },
        };
        var builder = new WithSubnetgroupItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

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
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "subnetgroupid", "test-subnetgroupid" },
        };
        var builder = new WithSubnetgroupItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// ToGetRequestInformationが正しいPathParametersを設定することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_SetsPathParameters()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "subnetgroupid", "test-subnetgroupid" },
        };
        var builder = new WithSubnetgroupItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.True(requestInfo.PathParameters.ContainsKey("subnetgroupid"));
        Assert.Equal("test-subnetgroupid", requestInfo.PathParameters["subnetgroupid"]);
    }

    /// <summary>
    /// ToPutRequestInformationがPUTメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_CreatesRequestInformationWithPutMethod()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "subnetgroupid", "test-subnetgroupid" },
        };
        var builder = new WithSubnetgroupItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var body = new UpdateSubnetGroup
        {
            Description = "updated-description",
            Name = "updated-subnetgroup",
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
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "subnetgroupid", "test-subnetgroupid" },
        };
        var builder = new WithSubnetgroupItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var body = new UpdateSubnetGroup
        {
            Description = "updated-description",
            Name = "updated-subnetgroup",
        };

        var requestInfo = builder.ToPutRequestInformation(body);

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// ToPutRequestInformationが正しいContent-Typeヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_SetsContentTypeHeader()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "subnetgroupid", "test-subnetgroupid" },
        };
        var builder = new WithSubnetgroupItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var body = new UpdateSubnetGroup
        {
            Description = "updated-description",
            Name = "updated-subnetgroup",
        };

        var requestInfo = builder.ToPutRequestInformation(body);

        Assert.True(requestInfo.Headers.ContainsKey("Content-Type"));
        Assert.Contains("application/json", requestInfo.Headers["Content-Type"]);
    }

    /// <summary>
    /// ToPutRequestInformationが正しいPathParametersを設定することを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_SetsPathParameters()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "subnetgroupid", "test-subnetgroupid" },
        };
        var builder = new WithSubnetgroupItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var body = new UpdateSubnetGroup
        {
            Description = "updated-description",
            Name = "updated-subnetgroup",
        };

        var requestInfo = builder.ToPutRequestInformation(body);

        Assert.True(requestInfo.PathParameters.ContainsKey("subnetgroupid"));
        Assert.Equal("test-subnetgroupid", requestInfo.PathParameters["subnetgroupid"]);
    }

    /// <summary>
    /// DeleteAsync が期待されるSendNoContentAsyncを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_CallsRequestAdapterSendNoContentAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "subnetgroupid", "test-subnetgroupid" },
        };
        var builder = new WithSubnetgroupItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        await builder.DeleteAsync();

        await requestAdapter.Received(1).SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => string.Concat(state).Contains("サブネットグループの削除を開始します。")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => string.Concat(state).Contains("サブネットグループの削除が完了しました。")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// DeleteAsyncで無効なサブネットグループIDを指定した場合にValidationExceptionがスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("　")]
    public async Task DeleteAsync_WithNullOrEmptyOrWhitespaceSubnetGroupId_ThrowsValidationException(string? invalidSubnetGroupId)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object?>
        {
            { "baseurl", "https://api.example.com" },
            { "subnetgroupid", invalidSubnetGroupId },
        };
        var builder = new WithSubnetgroupItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.DeleteAsync());

        Assert.Equal("サブネットグループIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("サブネットグループIDを入力してください。");
    }

    /// <summary>
    /// GetAsync が期待されるSendAsyncを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_CallsRequestAdapterSendAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "subnetgroupid", "test-subnetgroupid" },
        };
        var builder = new WithSubnetgroupItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var mockResponse = new SubnetGroupResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<SubnetGroupResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(mockResponse);

        var result = await builder.GetAsync();

        Assert.NotNull(result);
        Assert.IsType<SubnetGroupResponse>(result);

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<SubnetGroupResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => string.Concat(state).Contains("サブネットグループ詳細の取得を開始します。")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => string.Concat(state).Contains("サブネットグループ詳細の取得が完了しました。")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncで無効なサブネットグループIDを指定した場合にValidationExceptionがスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("　")]
    public async Task GetAsync_WithNullOrEmptyOrWhitespaceSubnetGroupId_ThrowsValidationException(string? invalidSubnetGroupId)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object?>
        {
            { "baseurl", "https://api.example.com" },
            { "subnetgroupid", invalidSubnetGroupId },
        };
        var builder = new WithSubnetgroupItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync());

        Assert.Equal("サブネットグループIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("サブネットグループIDを入力してください。");
    }

    /// <summary>
    /// PutAsync が期待されるSendAsyncを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_CallsRequestAdapterSendAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "subnetgroupid", "test-subnetgroupid" },
        };
        var builder = new WithSubnetgroupItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var body = new UpdateSubnetGroup
        {
            Description = "updated-description",
            Name = "updated-subnetgroup",
        };

        var mockResponse = new SubnetGroupResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<SubnetGroupResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(mockResponse);

        var result = await builder.PutAsync(body);

        Assert.NotNull(result);
        Assert.IsType<SubnetGroupResponse>(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<SubnetGroupResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => string.Concat(state).Contains("サブネットグループ詳細の更新を開始します。")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => string.Concat(state).Contains("サブネットグループ詳細の更新が完了しました。")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PutAsyncがnullボディで例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithNullBody_ThrowsValidationException()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "subnetgroupid", "test-subnetgroupid" },
        };
        var builder = new WithSubnetgroupItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(null));

        Assert.Equal("リクエストボディは必須です。", exception.Message);
        _mockLogger.Received(1).LogWarning("リクエストボディは必須です。");
    }

    /// <summary>
    /// PutAsyncのリクエストボディのDescriptionがnull、空文字列、空白文字の場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("　")]
    public async Task PutAsync_WithNullOrEmptyOrWhitespaceDescription_ThrowsValidationException(string? invalidDescription)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "subnetgroupid", "test-subnetgroupid" },
        };
        var builder = new WithSubnetgroupItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateSubnetGroup
        {
            Description = invalidDescription,
            Name = "updated-subnetgroup",
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal("Descriptionが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("Descriptionが入力されていません。");
    }

    /// <summary>
    /// PutAsyncのリクエストボディのNameがnull、空文字列、空白文字の場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("　")]
    public async Task PutAsync_WithNullOrEmptyOrWhitespaceName_ThrowsValidationException(string? invalidName)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "subnetgroupid", "test-subnetgroupid" },
        };
        var builder = new WithSubnetgroupItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateSubnetGroup
        {
            Description = "updated-description",
            Name = invalidName,
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal("Nameが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("Nameが入力されていません。");
    }

    /// <summary>
    /// PutAsyncで無効なサブネットグループIDを指定した場合にValidationExceptionがスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("　")]
    public async Task PutAsync_WithNullOrEmptyOrWhitespaceSubnetGroupId_ThrowsValidationException(string? invalidSubnetGroupId)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object?>
        {
            { "baseurl", "https://api.example.com" },
            { "subnetgroupid", invalidSubnetGroupId },
        };
        var builder = new WithSubnetgroupItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var body = new UpdateSubnetGroup
        {
            Description = "updated-description",
            Name = "updated-subnetgroup",
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal("サブネットグループIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("サブネットグループIDを入力してください。");
    }

    /// <summary>
    /// DeleteAsyncが"XXX"キーに汎用ApiErrorのエラーマッピングを使用することを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_SendsWithXxxErrorMapping()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "subnetgroupid", "test-subnetgroupid" },
        };
        var builder = new WithSubnetgroupItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await builder.DeleteAsync();

        await requestAdapter.Received(1).SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Is<Dictionary<string, ParsableFactory<IParsable>>>(x =>
                x.ContainsKey("XXX")
                && x["XXX"] == ApiError.CreateFromDiscriminatorValue),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// DeleteAsyncで汎用ApiErrorがスローされた場合に再スローされることを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_WithApiError_ThrowsApiError()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "subnetgroupid", "test-subnetgroupid" },
        };
        var builder = new WithSubnetgroupItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError();
        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException(expectedException));

        var exception = await Assert.ThrowsAsync<ApiError>(() => builder.DeleteAsync());

        Assert.Same(expectedException, exception);
        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => string.Concat(state).Contains("サブネットグループ削除のAPIエラーが発生しました。")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// DeleteAsyncでその他の例外がスローされた場合に再スローされることを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_WithOtherException_RethrowsException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "subnetgroupid", "test-subnetgroupid" },
        };
        var builder = new WithSubnetgroupItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new InvalidOperationException("test exception");
        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException(expectedException));

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => builder.DeleteAsync());

        Assert.Same(expectedException, exception);
        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => string.Concat(state).Contains("サブネットグループの削除中にエラーが発生しました。")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncが"XXX"キーに汎用ApiErrorのエラーマッピングを使用することを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_SendsWithXxxErrorMapping()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "subnetgroupid", "test-subnetgroupid" },
        };
        var builder = new WithSubnetgroupItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<SubnetGroupResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(new SubnetGroupResponse());

        await builder.GetAsync();

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<SubnetGroupResponse>>(),
            Arg.Is<Dictionary<string, ParsableFactory<IParsable>>>(x =>
                x.ContainsKey("XXX")
                && x["XXX"] == ApiError.CreateFromDiscriminatorValue),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// GetAsyncで汎用ApiErrorがスローされた場合に再スローされることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithApiError_ThrowsApiError()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "subnetgroupid", "test-subnetgroupid" },
        };
        var builder = new WithSubnetgroupItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<SubnetGroupResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<SubnetGroupResponse?>(expectedException));

        var exception = await Assert.ThrowsAsync<ApiError>(() => builder.GetAsync());

        Assert.Same(expectedException, exception);
        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => string.Concat(state).Contains("サブネットグループ詳細取得のAPIエラーが発生しました。")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncでその他の例外がスローされた場合に再スローされることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithOtherException_RethrowsException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "subnetgroupid", "test-subnetgroupid" },
        };
        var builder = new WithSubnetgroupItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new InvalidOperationException("test exception");
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<SubnetGroupResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<SubnetGroupResponse?>(expectedException));

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => builder.GetAsync());

        Assert.Same(expectedException, exception);
        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => string.Concat(state).Contains("サブネットグループ詳細の取得中にエラーが発生しました。")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PutAsyncが"XXX"キーに汎用ApiErrorのエラーマッピングを使用することを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_SendsWithXxxErrorMapping()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "subnetgroupid", "test-subnetgroupid" },
        };
        var builder = new WithSubnetgroupItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateSubnetGroup
        {
            Description = "updated-description",
            Name = "updated-subnetgroup",
        };

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<SubnetGroupResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(new SubnetGroupResponse());

        await builder.PutAsync(body);

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<SubnetGroupResponse>>(),
            Arg.Is<Dictionary<string, ParsableFactory<IParsable>>>(x =>
                x.ContainsKey("XXX")
                && x["XXX"] == ApiError.CreateFromDiscriminatorValue),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// PutAsyncで汎用ApiErrorがスローされた場合に再スローされることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithApiError_ThrowsApiError()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "subnetgroupid", "test-subnetgroupid" },
        };
        var builder = new WithSubnetgroupItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError();
        var body = new UpdateSubnetGroup
        {
            Description = "updated-description",
            Name = "updated-subnetgroup",
        };

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<SubnetGroupResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<SubnetGroupResponse?>(expectedException));

        var exception = await Assert.ThrowsAsync<ApiError>(() => builder.PutAsync(body));

        Assert.Same(expectedException, exception);
        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => string.Concat(state).Contains("サブネットグループ詳細更新のAPIエラーが発生しました。")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PutAsyncでその他の例外がスローされた場合に再スローされることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithOtherException_RethrowsException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "subnetgroupid", "test-subnetgroupid" },
        };
        var builder = new WithSubnetgroupItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new InvalidOperationException("test exception");
        var body = new UpdateSubnetGroup
        {
            Description = "updated-description",
            Name = "updated-subnetgroup",
        };

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<SubnetGroupResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<SubnetGroupResponse?>(expectedException));

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => builder.PutAsync(body));

        Assert.Same(expectedException, exception);
        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => string.Concat(state).Contains("サブネットグループ詳細の更新中にエラーが発生しました。")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// WithUrlが新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewInstance()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "subnetgroupid", "test-subnetgroupid" },
        };
        var builder = new WithSubnetgroupItemRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);
        var newUrl = "https://newapi.example.com/networking-suite/subnet-groups/new-subnetgroupid";

        var newBuilder = builder.WithUrl(newUrl);

        Assert.NotNull(newBuilder);
        Assert.NotSame(builder, newBuilder);
        Assert.IsType<WithSubnetgroupItemRequestBuilder>(newBuilder);
    }
}
