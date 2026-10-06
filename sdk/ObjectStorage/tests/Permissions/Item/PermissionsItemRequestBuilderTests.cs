using Microsoft.Kiota.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Models;
using Sacloud.Sdk.ObjectStorage.Permissions.Item;
using Sacloud.Sdk.ObjectStorage.Permissions.Item.Keys;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.ObjectStorage.Tests.Permissions.Item;

/// <summary>
/// PermissionsItemRequestBuilderのテスト
/// </summary>
public class PermissionsItemRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<PermissionsItemRequestBuilder> _mockLogger;

    public PermissionsItemRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<PermissionsItemRequestBuilder>>();
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

        var builder = new PermissionsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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

        var builder = new PermissionsItemRequestBuilder("https://api.example.com/permissions/permission-id-1", requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// Keysプロパティが正しいKeysRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Keys_ReturnsKeysRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", 1234 }
        };
        var builder = new PermissionsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder.Keys);
        Assert.IsType<KeysRequestBuilder>(builder.Keys);
    }

    /// <summary>
    /// ToDeleteRequestInformationが正しいRequestInformationを返すことを確認
    /// </summary>
    [Fact]
    public void ToDeleteRequestInformation_ReturnsRequestInformation()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", 1234 }
        };
        var builder = new PermissionsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToDeleteRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.DELETE, requestInfo.HttpMethod);
    }

    /// <summary>
    /// DeleteAsyncがRequestAdapterを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_CallsRequestAdapter()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", 1234 }
        };
        var builder = new PermissionsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await builder.DeleteAsync();

        await requestAdapter.Received(1).SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
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
        var builder = new PermissionsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<Permission>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(new Permission());

        var result = await builder.GetAsync();

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<Permission>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// ToPutRequestInformationが正しいRequestInformationを返すことを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_ReturnsRequestInformation()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", 1234 }
        };
        var builder = new PermissionsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PermissionBucketControlsBody
        {
            DisplayName = "valid-permission-name",
            BucketControls =
            [
                new()
                {
                    BucketName = "valid-bucket",
                    CanRead = true,
                    CanWrite = true
                }
            ]
        };

        var requestInfo = builder.ToPutRequestInformation(body);

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.PUT, requestInfo.HttpMethod);
    }

    /// <summary>
    /// PutAsyncがRequestAdapterを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_CallsRequestAdapter()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", 1234 }
        };
        var builder = new PermissionsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PermissionBucketControlsBody
        {
            DisplayName = "valid-permission-name",
            BucketControls =
            [
                new()
                {
                    BucketName = "valid-bucket",
                    CanRead = true,
                    CanWrite = true
                }
            ]
        };
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<Permission>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(new Permission());

        var result = await builder.PutAsync(body);

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<Permission>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// WithUrlが正しいPermissionsItemRequestBuilderを返すことを確認
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
        var builder = new PermissionsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var newBuilder = builder.WithUrl("https://api.example.com/permissions/other-id");

        Assert.NotNull(newBuilder);
        Assert.IsType<PermissionsItemRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }

    /// <summary>
    /// DeleteAsyncが"XXX"キーにApiErrorのエラーマッピングを使用することを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_SendsWithXxxErrorMapping()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", 1234 }
        };
        var builder = new PermissionsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await builder.DeleteAsync();

        await requestAdapter.Received(1).SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Is<Dictionary<string, ParsableFactory<IParsable>>>(x => HasApiErrorMapping(x)),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// DeleteAsyncでApiErrorがスローされた場合に再スローされることを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_WithApiError_ThrowsApiError()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", 1234 }
        };
        var builder = new PermissionsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError();
        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException(expectedException));

        await Assert.ThrowsAsync<ApiError>(() => builder.DeleteAsync());
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
            { "id", 1234 }
        };
        var builder = new PermissionsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new InvalidOperationException("test exception");
        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException(expectedException));

        await Assert.ThrowsAsync<InvalidOperationException>(() => builder.DeleteAsync());
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
        var builder = new PermissionsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<Permission>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(new Permission());

        await builder.GetAsync();

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<Permission>>(),
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
        var builder = new PermissionsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<Permission>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<Permission?>(expectedException));

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
        var builder = new PermissionsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new InvalidOperationException("test exception");
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<Permission>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<Permission?>(expectedException));

        await Assert.ThrowsAsync<InvalidOperationException>(() => builder.GetAsync());
    }

    /// <summary>
    /// PutAsyncが"XXX"キーにApiErrorのエラーマッピングを使用することを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_SendsWithXxxErrorMapping()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", 1234 }
        };
        var builder = new PermissionsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PermissionBucketControlsBody
        {
            DisplayName = "valid-permission-name",
            BucketControls =
            [
                new()
                {
                    BucketName = "valid-bucket",
                    CanRead = true,
                    CanWrite = true
                }
            ]
        };
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<Permission>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(new Permission());

        await builder.PutAsync(body);

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<Permission>>(),
            Arg.Is<Dictionary<string, ParsableFactory<IParsable>>>(x => HasApiErrorMapping(x)),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// PutAsyncでApiErrorがスローされた場合に再スローされることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithApiError_ThrowsApiError()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", 1234 }
        };
        var builder = new PermissionsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PermissionBucketControlsBody
        {
            DisplayName = "valid-permission-name",
            BucketControls =
            [
                new()
                {
                    BucketName = "valid-bucket",
                    CanRead = true,
                    CanWrite = true
                }
            ]
        };
        var expectedException = new ApiError();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<Permission>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<Permission?>(expectedException));

        await Assert.ThrowsAsync<ApiError>(() => builder.PutAsync(body));
    }

    /// <summary>
    /// PutAsyncでその他の例外がスローされた場合に再スローされることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithOtherException_RethrowsException()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", 1234 }
        };
        var builder = new PermissionsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PermissionBucketControlsBody
        {
            DisplayName = "valid-permission-name",
            BucketControls =
            [
                new()
                {
                    BucketName = "valid-bucket",
                    CanRead = true,
                    CanWrite = true
                }
            ]
        };
        var expectedException = new InvalidOperationException("test exception");
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<Permission>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<Permission?>(expectedException));

        await Assert.ThrowsAsync<InvalidOperationException>(() => builder.PutAsync(body));
    }

    /// <summary>
    /// DeleteAsyncでパーミッションIDが無効な場合にValidationExceptionがスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("　")]
    [InlineData(null)]
    public async Task DeleteAsync_WithInvalidPathParameters_ThrowsValidationException(string? invalidPermissionId)
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", invalidPermissionId }
        };
        var expectedMessage = "パーミッションIDを入力してください。";
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new PermissionsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.DeleteAsync());

        Assert.Equal(expectedMessage, exception.Message);
        _mockLogger.Received(1).LogWarning(expectedMessage);
    }

    /// <summary>
    /// GetAsyncでパーミッションIDが無効な場合にValidationExceptionがスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("　")]
    [InlineData(null)]
    public async Task GetAsync_WithInvalidPathParameters_ThrowsValidationException(string? invalidPermissionId)
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", invalidPermissionId }
        };
        var expectedMessage = "パーミッションIDを入力してください。";
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new PermissionsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync());

        Assert.Equal(expectedMessage, exception.Message);
        _mockLogger.Received(1).LogWarning(expectedMessage);
    }

    /// <summary>
    /// PutAsyncでパーミッションIDが無効な場合にValidationExceptionがスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("　")]
    [InlineData(null)]
    public async Task PutAsync_WithInvalidPermissionId_ThrowsValidationException(string? invalidPermissionId)
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", invalidPermissionId }
        };
        var expectedMessage = "パーミッションIDを入力してください。";
        var body = new PermissionBucketControlsBody
        {
            DisplayName = "valid-permission-name"
        };
        var builder = new PermissionsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal(expectedMessage, exception.Message);
        _mockLogger.Received(1).LogWarning(expectedMessage);
    }

    /// <summary>
    /// PutAsyncでDisplayNameがnull、空文字、または空白文字の場合にValidationExceptionがスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("　")]
    public async Task PutAsync_WithNullOrEmptyOrWhitespaceDisplayName_ThrowsValidationException(string? invalidDisplayName)
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", 1234 }
        };
        var builder = new PermissionsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PermissionBucketControlsBody
        {
            DisplayName = invalidDisplayName,
            BucketControls =
            [
                new()
                {
                    BucketName = "valid-bucket",
                    CanRead = true,
                    CanWrite = true
                }
            ]
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal("DisplayNameが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("DisplayNameが入力されていません。");
    }

    /// <summary>
    /// PutAsyncでDisplayNameが正規表現パターンに合致しない場合にValidationExceptionがスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("valid\nname")]
    [InlineData("\nname")]
    [InlineData("name\n")]
    public async Task PutAsync_WithInvalidDisplayNameFormat_ThrowsFormatValidationException(string invalidDisplayName)
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", 1234 }
        };
        var builder = new PermissionsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PermissionBucketControlsBody
        {
            DisplayName = invalidDisplayName,
            BucketControls =
            [
                new()
                {
                    BucketName = "valid-bucket",
                    CanRead = true,
                    CanWrite = true
                }
            ]
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal("DisplayNameの形式が正しくありません。", exception.Message);
        _mockLogger.Received(1).LogWarning("DisplayNameの形式が正しくありません。");
    }

    /// <summary>
    /// PutAsyncでBucketNameがnull、空文字、または空白文字の場合にValidationExceptionがスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("　")]
    public async Task PutAsync_WithNullOrEmptyOrWhitespaceBucketName_ThrowsRequiredValidationException(string? invalidBucketName)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", 1234 }
        };
        var builder = new PermissionsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PermissionBucketControlsBody
        {
            DisplayName = "valid-permission-name",
            BucketControls =
            [
                new()
                {
                    BucketName = invalidBucketName,
                    CanRead = true,
                    CanWrite = true,
                }
            ]
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal("BucketControlsBodyItem.BucketNameが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("BucketControlsBodyItem.BucketNameが入力されていません。");
    }

    /// <summary>
    /// PutAsyncでBucketNameの文字数が範囲外の場合に文字数エラーのValidationExceptionがスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(2)]
    [InlineData(65)]
    public async Task PutAsync_WithBucketNameLengthOutOfRange_ThrowsLengthValidationException(int invalidBucketNameLength)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", 1234 }
        };
        var builder = new PermissionsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PermissionBucketControlsBody
        {
            DisplayName = "valid-permission-name",
            BucketControls =
            [
                new()
                {
                    BucketName = new string('a', invalidBucketNameLength),
                    CanRead = true,
                    CanWrite = true,
                }
            ]
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal("BucketControlsBodyItem.BucketNameは3文字以上64文字以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("BucketControlsBodyItem.BucketNameは3文字以上64文字以下で入力してください。");
    }

    /// <summary>
    /// PutAsyncでBucketNameが正規表現パターンに合致しない場合に形式エラーのValidationExceptionがスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("INVALID")]
    [InlineData("-invalid")]
    [InlineData("invalid-")]
    [InlineData("in_valid")]
    public async Task PutAsync_WithInvalidBucketNameFormat_ThrowsFormatValidationException(string invalidBucketName)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", 1234 }
        };
        var builder = new PermissionsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PermissionBucketControlsBody
        {
            DisplayName = "valid-permission-name",
            BucketControls =
            [
                new()
                {
                    BucketName = invalidBucketName,
                    CanRead = true,
                    CanWrite = true,
                }
            ]
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal("BucketControlsBodyItem.BucketNameの形式が正しくありません。", exception.Message);
        _mockLogger.Received(1).LogWarning("BucketControlsBodyItem.BucketNameの形式が正しくありません。");
    }

    /// <summary>
    /// PutAsyncでnullボディの場合にValidationExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithNullBody_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "permission-id-1" }
        };
        var builder = new PermissionsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(null));
    }

    /// <summary>
    /// PutAsyncでBucketControlsがnullの場合にValidationExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithNullBucketControls_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "permission-id-1" }
        };
        var builder = new PermissionsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PermissionBucketControlsBody
        {
            DisplayName = "valid-permission-name",
            BucketControls = null
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal("BucketControlsが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("BucketControlsが入力されていません。");
    }

    /// <summary>
    /// PutAsyncでCanReadがnullの場合にValidationExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithNullCanRead_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "permission-id-1" }
        };
        var builder = new PermissionsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PermissionBucketControlsBody
        {
            DisplayName = "valid-permission-name",
            BucketControls =
            [
                new()
                {
                    BucketName = "valid-bucket",
                    CanRead = true,
                    CanWrite = true
                }
            ]
        };
        body.BucketControls[0].CanRead = null;

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal("BucketControlsBodyItem.CanReadが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("BucketControlsBodyItem.CanReadが入力されていません。");
    }

    /// <summary>
    /// PutAsyncでCanWriteがnullの場合にValidationExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithNullCanWrite_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "permission-id-1" }
        };
        var builder = new PermissionsItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PermissionBucketControlsBody
        {
            DisplayName = "valid-permission-name",
            BucketControls =
            [
                new()
                {
                    BucketName = "valid-bucket",
                    CanRead = true,
                    CanWrite = true
                }
            ]
        };
        body.BucketControls[0].CanWrite = null;

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal("BucketControlsBodyItem.CanWriteが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("BucketControlsBodyItem.CanWriteが入力されていません。");
    }

    private static IRequestAdapter CreateRequestAdapter()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var serializationWriterFactory = Substitute.For<ISerializationWriterFactory>();
        var serializationWriter = Substitute.For<ISerializationWriter>();
        serializationWriter.GetSerializedContent().Returns(new MemoryStream());
        serializationWriterFactory.GetSerializationWriter(Arg.Any<string>()).Returns(serializationWriter);
        requestAdapter.SerializationWriterFactory.Returns(serializationWriterFactory);
        return requestAdapter;
    }

    private static bool HasApiErrorMapping(Dictionary<string, ParsableFactory<IParsable>> errorMapping)
    {
        return errorMapping.TryGetValue("XXX", out var handler)
            && handler == ApiError.CreateFromDiscriminatorValue;
    }
}
