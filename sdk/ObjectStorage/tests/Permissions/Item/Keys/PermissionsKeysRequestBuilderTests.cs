using Microsoft.Kiota.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Models.Handler;
using Sacloud.Sdk.ObjectStorage.Permissions.Item.Keys;
using Sacloud.Sdk.ObjectStorage.Permissions.Item.Keys.Item;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.ObjectStorage.Tests.Permissions.Item.Keys;

/// <summary>
/// KeysRequestBuilder（Permissions.Item）のテスト
/// </summary>
public class PermissionsKeysRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<KeysRequestBuilder> _mockLogger;

    public PermissionsKeysRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<KeysRequestBuilder>>();
        _mockLoggerFactory.CreateLogger(Arg.Any<string>()).Returns(_mockLogger);
    }

    /// <summary>
    /// コンストラクタ（pathParameters版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", 1234 }
        };

        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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

        var builder = new KeysRequestBuilder("https://api.example.com/permissions/permission-id-1/keys", requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// インデクサーが正しいWithKey_ItemRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_WithKeyId_ReturnsWithKey_ItemRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", 1234 }
        };
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var item = builder["KEYID1234"];

        Assert.NotNull(item);
        Assert.IsType<WithKey_ItemRequestBuilder>(item);
    }

    /// <summary>
    /// インデクサーが毎回新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_ReturnsNewInstanceOnEachCall()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", 1234 }
        };
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var item1 = builder["KEYID1234"];
        var item2 = builder["KEYID1234"];

        Assert.NotSame(item1, item2);
    }

    /// <summary>
    /// ToGetRequestInformation が HTTP メソッド GET の RequestInformation を返すことを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_ReturnsRequestInformation()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", 1234 }
        };
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.GET, requestInfo.HttpMethod);
    }

    /// <summary>
    /// GetAsyncがRequestAdapterを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_CallsRequestAdapter()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", 1234 }
        };
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListPermissionKeysResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(new ListPermissionKeysResponse());

        var result = await builder.GetAsync();

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListPermissionKeysResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// ToPostRequestInformationが正しいRequestInformationを返すことを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_ReturnsRequestInformation()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", 1234 }
        };
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToPostRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.POST, requestInfo.HttpMethod);
    }

    /// <summary>
    /// PostAsyncがRequestAdapterを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_CallsRequestAdapter()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", 1234 }
        };
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreatePermissionKeyResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(new CreatePermissionKeyResponse());

        var result = await builder.PostAsync();

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreatePermissionKeyResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// WithUrlが正しいKeysRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", 1234 }
        };
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var newBuilder = builder.WithUrl("https://api.example.com/permissions/other-id/keys");

        Assert.NotNull(newBuilder);
        Assert.IsType<KeysRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }

    /// <summary>
    /// GetAsyncが"XXX"キーにApiErrorのエラーマッピングを使用することを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_SendsWithXxxErrorMapping()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", 1234 }
        };
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListPermissionKeysResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(new ListPermissionKeysResponse());

        await builder.GetAsync();

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListPermissionKeysResponse>>(),
            Arg.Is<Dictionary<string, ParsableFactory<IParsable>>>(x => HasApiErrorMapping(x)),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// GetAsyncでApiErrorがスローされた場合に再スローされることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithApiError_ThrowsApiError()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", 1234 }
        };
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListPermissionKeysResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<ListPermissionKeysResponse?>(expectedException));

        await Assert.ThrowsAsync<ApiError>(() => builder.GetAsync());
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
            { "id", 1234 }
        };
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new InvalidOperationException("test exception");
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListPermissionKeysResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<ListPermissionKeysResponse?>(expectedException));

        await Assert.ThrowsAsync<InvalidOperationException>(() => builder.GetAsync());
    }

    /// <summary>
    /// PostAsyncが"XXX"キーにApiErrorのエラーマッピングを使用することを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_SendsWithXxxErrorMapping()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", 1234 }
        };
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreatePermissionKeyResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(new CreatePermissionKeyResponse());

        await builder.PostAsync();

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreatePermissionKeyResponse>>(),
            Arg.Is<Dictionary<string, ParsableFactory<IParsable>>>(x => HasApiErrorMapping(x)),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// PostAsyncでApiErrorがスローされた場合に再スローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithApiError_ThrowsApiError()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", 1234 }
        };
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreatePermissionKeyResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<CreatePermissionKeyResponse?>(expectedException));

        await Assert.ThrowsAsync<ApiError>(() => builder.PostAsync());
    }

    /// <summary>
    /// PostAsyncでその他の例外がスローされた場合に再スローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithOtherException_RethrowsException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", 1234 }
        };
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new InvalidOperationException("test exception");
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CreatePermissionKeyResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<CreatePermissionKeyResponse?>(expectedException));

        await Assert.ThrowsAsync<InvalidOperationException>(() => builder.PostAsync());
    }

    /// <summary>
    /// GetAsyncで無効なパスパラメーターを指定した場合にValidationExceptionがスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("　")]
    public async Task GetAsync_WithNullOrEmptyOrWhitespacePathParameter_ThrowsValidationException(string? invalidId)
    {
        var pathParameters = new Dictionary<string, object?>
        {
            { "baseurl", "https://api.example.com" },
            { "id", invalidId }
        };
        var expectedMessage = "パーミッションIDを入力してください。";
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync());

        Assert.Equal(expectedMessage, exception.Message);
        _mockLogger.Received(1).LogWarning(expectedMessage);
    }

    /// <summary>
    /// PostAsyncで無効なパスパラメーターを指定した場合にValidationExceptionがスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("　")]
    public async Task PostAsync_WithNullOrEmptyOrWhitespacePathParameter_ThrowsValidationException(string? invalidId)
    {
        var pathParameters = new Dictionary<string, object?>
        {
            { "baseurl", "https://api.example.com" },
            { "id", invalidId }
        };
        var expectedMessage = "パーミッションIDを入力してください。";
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new KeysRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync());

        Assert.Equal(expectedMessage, exception.Message);
        _mockLogger.Received(1).LogWarning(expectedMessage);
    }

    private static bool HasApiErrorMapping(Dictionary<string, ParsableFactory<IParsable>> errorMapping)
    {
        return errorMapping.TryGetValue("XXX", out var handler)
            && handler == ApiError.CreateFromDiscriminatorValue;
    }
}
