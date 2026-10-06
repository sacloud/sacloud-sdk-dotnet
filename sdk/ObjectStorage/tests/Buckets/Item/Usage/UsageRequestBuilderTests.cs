using Microsoft.Kiota.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Buckets.Item.Usage;
using Sacloud.Sdk.ObjectStorage.Models;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.ObjectStorage.Tests.Buckets.Item.Usage;

/// <summary>
/// UsageRequestBuilderのテスト
/// </summary>
public class UsageRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<UsageRequestBuilder> _mockLogger;

    public UsageRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<UsageRequestBuilder>>();
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
            { "name", "mybucket" }
        };

        var builder = new UsageRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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

        var builder = new UsageRequestBuilder("https://api.example.com/buckets/mybucket/usage", requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
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
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new UsageRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
            { "name", "mybucket" }
        };
        var builder = new UsageRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<BucketUsage>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(new BucketUsage());

        var result = await builder.GetAsync();

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<BucketUsage>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// GetAsyncでハイフンを含むバケット名が受け入れられることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithHyphenatedBucketName_CallsRequestAdapter()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "myb-ucket" }
        };
        var builder = new UsageRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<BucketUsage>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(new BucketUsage());

        var result = await builder.GetAsync();

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<BucketUsage>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// WithUrlが正しいUsageRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new UsageRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var newBuilder = builder.WithUrl("https://api.example.com/buckets/otherbucket/usage");

        Assert.NotNull(newBuilder);
        Assert.IsType<UsageRequestBuilder>(newBuilder);
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
            { "name", "mybucket" }
        };
        var builder = new UsageRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<BucketUsage>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(new BucketUsage());

        await builder.GetAsync();

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<BucketUsage>>(),
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
            { "name", "mybucket" }
        };
        var builder = new UsageRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<BucketUsage>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<BucketUsage?>(expectedException));

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
            { "name", "mybucket" }
        };
        var builder = new UsageRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new InvalidOperationException("test exception");
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<BucketUsage>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<BucketUsage?>(expectedException));

        await Assert.ThrowsAsync<InvalidOperationException>(() => builder.GetAsync());
    }

    /// <summary>
    /// GetAsyncでバケット名の長さが許容範囲外の場合にValidationExceptionがスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(2, "バケット名は3文字以上で入力してください。")]
    [InlineData(65, "バケット名は64文字以下で入力してください。")]
    public async Task GetAsync_WithBucketNameOutsideLengthRange_ThrowsValidationException(int nameLength, string expectedMessage)
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", new string('a', nameLength) }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new UsageRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync());

        Assert.Equal(expectedMessage, exception.Message);
        _mockLogger.Received(1).LogWarning(expectedMessage);
    }

    /// <summary>
    /// GetAsyncで3文字または64文字のバケット名が受け入れられることを確認
    /// </summary>
    [Theory]
    [InlineData(3)]
    [InlineData(64)]
    public async Task GetAsync_WithBoundaryLengthBucketName_CallsRequestAdapter(int nameLength)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<BucketUsage>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(new BucketUsage());
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", new string('a', nameLength) }
        };
        var builder = new UsageRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var result = await builder.GetAsync();

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<BucketUsage>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// <summary>
    /// GetAsyncでパスパラメータが不正な場合にValidationExceptionがスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("　")]
    [InlineData(null)]
    public async Task GetAsync_WithNullOrWhitespacePathParameters_ThrowsValidationException(string? invalidName)
    {
        var pathParameters = new Dictionary<string, object?>
        {
            { "baseurl", "https://api.example.com" },
            { "name", invalidName }
        };
        var expectedMessage = "バケット名を入力してください。";
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new UsageRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync());

        Assert.Equal(expectedMessage, exception.Message);
        _mockLogger.Received(1).LogWarning(expectedMessage);
    }
    
    /// <summary>
    /// GetAsyncでパスパラメータが不正な場合にValidationExceptionがスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("-ab")]
    [InlineData("Ａbc")]
    [InlineData("あab")]
    [InlineData(" ab")]
    [InlineData("　ab")]
    public async Task GetAsync_WithInvalidPathParameters_ThrowsValidationException_AdditionalCases(string invalidName)
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", invalidName }
        };
        var expectedMessage = "バケット名の形式が正しくありません。";
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new UsageRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync());

        Assert.Equal(expectedMessage, exception.Message);
        _mockLogger.Received(1).LogWarning(expectedMessage);
    }

    private static bool HasApiErrorMapping(Dictionary<string, ParsableFactory<IParsable>> errorMapping)
    {
        return errorMapping.TryGetValue("XXX", out var handler)
            && handler == ApiError.CreateFromDiscriminatorValue;
    }
}
