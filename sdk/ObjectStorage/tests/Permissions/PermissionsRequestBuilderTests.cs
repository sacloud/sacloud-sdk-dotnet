using Microsoft.Kiota.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Permissions;
using Sacloud.Sdk.ObjectStorage.Permissions.Item;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.ObjectStorage.Tests.Permissions;

/// <summary>
/// PermissionsRequestBuilderのテスト
/// </summary>
public class PermissionsRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<PermissionsRequestBuilder> _mockLogger;

    public PermissionsRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<PermissionsRequestBuilder>>();
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
            { "baseurl", "https://api.example.com" }
        };

        var builder = new PermissionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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

        var builder = new PermissionsRequestBuilder("https://api.example.com/permissions", requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// インデクサーが正しいPermissionsItemRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_WithId_ReturnsPermissionsItemRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new PermissionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var item = builder[1234];

        Assert.NotNull(item);
        Assert.IsType<PermissionsItemRequestBuilder>(item);
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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new PermissionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var item1 = builder[1234];
        var item2 = builder[1234];

        Assert.NotSame(item1, item2);
    }

    /// <summary>
    /// ToGetRequestInformationが正しいRequestInformationを返すことを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_ReturnsRequestInformation()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new PermissionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new PermissionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ObjectStorage.Models.Permissions>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(new ObjectStorage.Models.Permissions());

        var result = await builder.GetAsync();

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ObjectStorage.Models.Permissions>>(),
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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new PermissionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ObjectStorage.Models.PermissionBucketControlsBody
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

        var requestInfo = builder.ToPostRequestInformation(body);

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.POST, requestInfo.HttpMethod);
    }

    /// <summary>
    /// PostAsyncでnullボディの場合にValidationExceptionがスローされることを確認
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
        var builder = new PermissionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(null));
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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new PermissionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ObjectStorage.Models.PermissionBucketControlsBody
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
            Arg.Any<ParsableFactory<ObjectStorage.Models.Permission>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(new ObjectStorage.Models.Permission());

        var result = await builder.PostAsync(body);

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ObjectStorage.Models.Permission>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// WithUrlが正しいPermissionsRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new PermissionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var newBuilder = builder.WithUrl("https://api.example.com/permissions");

        Assert.NotNull(newBuilder);
        Assert.IsType<PermissionsRequestBuilder>(newBuilder);
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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new PermissionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ObjectStorage.Models.Permissions>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(new ObjectStorage.Models.Permissions());

        await builder.GetAsync();

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ObjectStorage.Models.Permissions>>(),
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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new PermissionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ObjectStorage.Models.Permissions>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<ObjectStorage.Models.Permissions?>(expectedException));

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
            { "baseurl", "https://api.example.com" }
        };
        var builder = new PermissionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new InvalidOperationException("test exception");
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ObjectStorage.Models.Permissions>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<ObjectStorage.Models.Permissions?>(expectedException));

        await Assert.ThrowsAsync<InvalidOperationException>(() => builder.GetAsync());
    }

    /// <summary>
    /// PostAsyncが"XXX"キーにApiErrorのエラーマッピングを使用することを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_SendsWithXxxErrorMapping()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new PermissionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ObjectStorage.Models.PermissionBucketControlsBody
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
            Arg.Any<ParsableFactory<ObjectStorage.Models.Permission>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(new ObjectStorage.Models.Permission());

        await builder.PostAsync(body);

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ObjectStorage.Models.Permission>>(),
            Arg.Is<Dictionary<string, ParsableFactory<IParsable>>>(x => HasApiErrorMapping(x)),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// PostAsyncでApiErrorがスローされた場合に再スローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithApiError_ThrowsApiError()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new PermissionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ObjectStorage.Models.PermissionBucketControlsBody
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
            Arg.Any<ParsableFactory<ObjectStorage.Models.Permission>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<ObjectStorage.Models.Permission?>(expectedException));

        await Assert.ThrowsAsync<ApiError>(() => builder.PostAsync(body));
    }

    /// <summary>
    /// PostAsyncでその他の例外がスローされた場合に再スローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithOtherException_RethrowsException()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new PermissionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ObjectStorage.Models.PermissionBucketControlsBody
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
            Arg.Any<ParsableFactory<ObjectStorage.Models.Permission>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<ObjectStorage.Models.Permission?>(expectedException));

        await Assert.ThrowsAsync<InvalidOperationException>(() => builder.PostAsync(body));
    }

    /// <summary>
    /// PostAsyncでDisplayNameがnull、空文字、または空白文字の場合にValidationExceptionがスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("　")]
    public async Task PostAsync_WithNullOrEmptyOrWhitespaceDisplayName_ThrowsValidationException(string? invalidDisplayName)
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new PermissionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ObjectStorage.Models.PermissionBucketControlsBody
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

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal("DisplayNameが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("DisplayNameが入力されていません。");
    }

    /// <summary>
    /// PostAsyncでDisplayNameが正規表現パターンに合致しない場合にValidationExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithInvalidDisplayNameFormat_ThrowsValidationException()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new PermissionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ObjectStorage.Models.PermissionBucketControlsBody
        {
            DisplayName = "valid\nname",
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

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal("DisplayNameの形式が正しくありません。", exception.Message);
        _mockLogger.Received(1).LogWarning("DisplayNameの形式が正しくありません。");
    }

    /// <summary>
    /// PostAsyncでBucketNameがnull、空文字、または空白文字の場合にValidationExceptionがスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("　")]
    public async Task PostAsync_WithNullOrEmptyOrWhitespaceBucketName_ThrowsRequiredValidationException(string? invalidBucketName)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new PermissionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ObjectStorage.Models.PermissionBucketControlsBody
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

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal("BucketControlsBodyItem.BucketNameが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("BucketControlsBodyItem.BucketNameが入力されていません。");
    }

    /// <summary>
    /// PostAsyncでBucketNameの文字数が範囲外の場合に文字数エラーのValidationExceptionがスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(2)]
    [InlineData(65)]
    public async Task PostAsync_WithBucketNameLengthOutOfRange_ThrowsLengthValidationException(int invalidBucketNameLength)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new PermissionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ObjectStorage.Models.PermissionBucketControlsBody
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

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal("BucketControlsBodyItem.BucketNameは3文字以上64文字以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("BucketControlsBodyItem.BucketNameは3文字以上64文字以下で入力してください。");
    }

    /// <summary>
    /// PostAsyncでBucketNameが正規表現パターンに合致しない場合に形式エラーのValidationExceptionがスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("INVALID")]
    [InlineData("-invalid")]
    [InlineData("invalid-")]
    [InlineData("in_valid")]
    public async Task PostAsync_WithInvalidBucketNameFormat_ThrowsFormatValidationException(string invalidBucketName)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new PermissionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ObjectStorage.Models.PermissionBucketControlsBody
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

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal("BucketControlsBodyItem.BucketNameの形式が正しくありません。", exception.Message);
        _mockLogger.Received(1).LogWarning("BucketControlsBodyItem.BucketNameの形式が正しくありません。");
    }

    /// <summary>
    /// PostAsyncでBucketControlsがnullの場合にValidationExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithNullBucketControls_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new PermissionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ObjectStorage.Models.PermissionBucketControlsBody
        {
            DisplayName = "valid-permission-name",
            BucketControls = null
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal("BucketControlsが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("BucketControlsが入力されていません。");
    }

    /// <summary>
    /// PostAsyncでCanReadがnullの場合にValidationExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithNullCanRead_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new PermissionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ObjectStorage.Models.PermissionBucketControlsBody
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

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal("BucketControlsBodyItem.CanReadが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("BucketControlsBodyItem.CanReadが入力されていません。");
    }

    /// <summary>
    /// PostAsyncでCanWriteがnullの場合にValidationExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithNullCanWrite_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new PermissionsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ObjectStorage.Models.PermissionBucketControlsBody
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

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

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
