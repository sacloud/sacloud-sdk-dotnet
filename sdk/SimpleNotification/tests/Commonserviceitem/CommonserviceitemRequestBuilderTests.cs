using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sacloud.Sdk.SimpleNotification.Commonserviceitem;
using Sacloud.Sdk.SimpleNotification.Models;
using Sacloud.Sdk.Shared.Models;
using Sacloud.Sdk.Shared.Validation;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.SimpleNotification.Tests.Commonserviceitem;

/// <summary>
/// CommonserviceitemRequestBuilderのテスト
/// </summary>
public class CommonserviceitemRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<CommonserviceitemRequestBuilder> _mockLogger;

    public CommonserviceitemRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<CommonserviceitemRequestBuilder>>();
        _mockLoggerFactory.CreateLogger<CommonserviceitemRequestBuilder>().Returns(_mockLogger);
    }

    /// <summary>
    /// コンストラクタ（PathParameters版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };

        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();

        var builder = new CommonserviceitemRequestBuilder("https://api.example.com/commonserviceitem", requestAdapter);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// インデクサーが正しいCommonserviceitemItemRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_ReturnsCommonserviceitemItemRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter);

        var itemBuilder = builder["123456789012"];

        Assert.NotNull(itemBuilder);
    }

    /// <summary>
    /// Simplenotificationプロパティが正しい型を返すことを確認
    /// </summary>
    [Fact]
    public void Simplenotification_ReturnsSimplenotificationRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter);

        var result = builder.Simplenotification;

        Assert.NotNull(result);
    }

    /// <summary>
    /// ToGetRequestInformationが正しいRequestInformationを生成することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_ReturnsCorrectRequestInformation()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.GET, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToPostRequestInformationが正しいRequestInformationを生成することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_ReturnsCorrectRequestInformation()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var serializationWriter = Substitute.For<ISerializationWriter>();
        serializationWriter.GetSerializedContent().Returns(new MemoryStream());
        var serializationWriterFactory = Substitute.For<ISerializationWriterFactory>();
        serializationWriterFactory.GetSerializationWriter(Arg.Any<string>()).Returns(serializationWriter);
        requestAdapter.SerializationWriterFactory.Returns(serializationWriterFactory);
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter);
        var body = new PostCommonServiceItemRequest
        {
            CommonServiceItem = new PostCommonServiceItemRequest_CommonServiceItem
            {
                Name = "test-resource",
                Provider = new CommonServiceItemProvider
                {
                    Class = CommonServiceItemProvider_Class.Saknoticedestination
                },
                Settings = new CommonServiceItemSettings
                {
                    DestinationSettings = new DestinationSettings
                    {
                        Type = DestinationSettings_Type.Email,
                        Value = "test@example.com"
                    }
                }
            }
        };

        var requestInfo = builder.ToPostRequestInformation(body);

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.POST, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToPostRequestInformationがnullボディで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_WithNullBody_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter);

        Assert.Throws<ArgumentNullException>(() => builder.ToPostRequestInformation(null!));
    }

    /// <summary>
    /// GetAsyncがRequestAdapter.SendAsyncを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_CallsRequestAdapterSendAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter);
        var expectedResponse = new ListCommonServiceItemsResponse();

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListCommonServiceItemsResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(expectedResponse);

        var result = await builder.GetAsync();

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListCommonServiceItemsResponse>>(),
            Arg.Is<Dictionary<string, ParsableFactory<IParsable>>>(x => HasApiErrorMapping(x)),
            Arg.Any<CancellationToken>());
        Assert.Equal(expectedResponse, result);
    }

    /// <summary>
    /// GetAsyncでApiErrorがスローされた場合にそのまま再スローされることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithApiError_ThrowsApiError()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter);
        var expectedException = new ApiError();

        requestAdapter.SendAsync<ListCommonServiceItemsResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListCommonServiceItemsResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<ListCommonServiceItemsResponse?>(expectedException));

        await Assert.ThrowsAsync<ApiError>(() => builder.GetAsync());
    }

    /// <summary>
    /// GetAsyncでその他の例外がスローされた場合にそのまま再スローされることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithOtherException_ThrowsException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter);
        var expectedException = new InvalidOperationException("test exception");

        requestAdapter.SendAsync<ListCommonServiceItemsResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListCommonServiceItemsResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<ListCommonServiceItemsResponse?>(expectedException));

        await Assert.ThrowsAsync<InvalidOperationException>(() => builder.GetAsync());
    }

    /// <summary>
    /// RequestParameterValidator.ValidateRequestBodyがネスト必須項目未設定のボディでValidationExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void ValidateRequestBody_WithMissingNestedFields_ThrowsValidationException()
    {
        var body = new PostCommonServiceItemRequest
        {
            CommonServiceItem = new PostCommonServiceItemRequest_CommonServiceItem()
        };

        var exception = Assert.Throws<ValidationException>(() => RequestParameterValidator.ValidateRequestBody(body, NullLogger.Instance));

        Assert.Contains("CommonServiceItem.Name", exception.Message);
    }

    /// <summary>
    /// PostAsyncがRequestAdapter.SendAsyncを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_CallsRequestAdapterSendAsync()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter);
        var body = CreateValidPostRequestBody();
        var expectedResponse = new CommonserviceitemPostResponse();

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CommonserviceitemPostResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(expectedResponse);

        var result = await builder.PostAsync(body);

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CommonserviceitemPostResponse>>(),
            Arg.Is<Dictionary<string, ParsableFactory<IParsable>>>(x => HasApiErrorMapping(x)),
            Arg.Any<CancellationToken>());
        Assert.Equal(expectedResponse, result);
    }

    /// <summary>
    /// PostAsyncがnullボディでバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithNullBody_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter);

        await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(null!));
    }

    /// <summary>
    /// PostAsyncのリクエストボディの必須パラメータがnullの場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Theory]
    [InlineData("CommonServiceItem")]
    [InlineData("CommonServiceItem.Description")]
    [InlineData("CommonServiceItem.Icon.ID")]
    [InlineData("CommonServiceItem.Name")]
    [InlineData("CommonServiceItem.Provider")]
    [InlineData("CommonServiceItem.Provider.Class")]
    [InlineData("CommonServiceItem.ServiceClass")]
    [InlineData("CommonServiceItem.Settings")]
    [InlineData("CommonServiceItem.Settings.DestinationSettings.Type")]
    [InlineData("CommonServiceItem.Settings.DestinationSettings.Value")]
    [InlineData("CommonServiceItem.Settings.GroupSettings.Destinations")]
    [InlineData("CommonServiceItem.Settings.RoutingSettings.MatchLabels")]
    [InlineData("CommonServiceItem.Settings.RoutingSettings.MatchLabels.Name")]
    [InlineData("CommonServiceItem.Settings.RoutingSettings.MatchLabels.Value")]
    [InlineData("CommonServiceItem.Settings.RoutingSettings.SourceID")]
    [InlineData("CommonServiceItem.Settings.RoutingSettings.TargetGroupID")]
    public async Task PostAsync_WithNullRequestBodyParameter_ThrowsValidationException(string invalidParameterName)
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = CreatePostRequestBodyWithNullParameter(invalidParameterName);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal($"{invalidParameterName}が入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}が入力されていません。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディのEnumパラメータがEnum定義外値の場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Theory]
    [InlineData("CommonServiceItem.Provider.Class")]
    [InlineData("CommonServiceItem.ServiceClass")]
    [InlineData("CommonServiceItem.Settings.DestinationSettings.Type")]
    public async Task PostAsync_WithInvalidEnumRequestBodyParameter_ThrowsValidationException(string invalidParameterName)
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = CreatePostRequestBodyWithInvalidEnumParameter(invalidParameterName);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal($"{invalidParameterName}はEnumに含まれる値を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}はEnumに含まれる値を入力してください。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディの正規表現パターンに一致しなかった場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("CommonServiceItem.Settings.RoutingSettings.MatchLabels.Name", "&name")]
    [InlineData("CommonServiceItem.Settings.RoutingSettings.MatchLabels.Name", "無効なName")]
    [InlineData("CommonServiceItem.Settings.RoutingSettings.MatchLabels.Value", "無効なValue")]
    [InlineData("CommonServiceItem.Settings.RoutingSettings.MatchLabels.Value", "abc\tdef")]
    [InlineData("CommonServiceItem.Settings.RoutingSettings.MatchLabels.Value", "abc\ndef")]
    [InlineData("CommonServiceItem.Settings.RoutingSettings.SourceID", "abcdef")]
    [InlineData("CommonServiceItem.Settings.RoutingSettings.SourceID", "ABCDEF")]
    [InlineData("CommonServiceItem.Settings.RoutingSettings.SourceID", "無効なソースID")]
    [InlineData("CommonServiceItem.Settings.RoutingSettings.TargetGroupID", "abcdef")]
    [InlineData("CommonServiceItem.Settings.RoutingSettings.TargetGroupID", "ABCDEF")]
    [InlineData("CommonServiceItem.Settings.RoutingSettings.TargetGroupID", "無効なターゲットグループID")]
    public async Task PostAsync_WithInvalidRegexPatternRequestBodyParameter_ThrowsValidationException(string invalidParameterName, string invalidParameterValue)
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = CreatePostRequestBodyWithInvalidRegExPatternParameter(invalidParameterName, invalidParameterValue);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal($"{invalidParameterName}の形式が正しくありません。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}の形式が正しくありません。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディの配列要素のうち正規表現パターンに一致しないものがあった場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithInvalidArrayElementRegularExpressionRequestBodyParameter_ThrowsValidationException()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PostCommonServiceItemRequest
        {
            CommonServiceItem = new PostCommonServiceItemRequest_CommonServiceItem
            {
                Name = "test-resource",
                Description = "test-description",
                Provider = new CommonServiceItemProvider
                {
                    Class = CommonServiceItemProvider_Class.Saknoticegroup
                },
                Settings = new CommonServiceItemSettings
                {
                    GroupSettings = new GroupSettings
                    {
                        Destinations = new List<string> { "invalid-destination" }
                    }
                },
                ServiceClass = CommonServiceItemServiceClass.CloudSaknoticegroup2
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal("CommonServiceItem.Settings.GroupSettings.Destinationsの要素の形式が正しくありません。", exception.Message);
        _mockLogger.Received(1).LogWarning("CommonServiceItem.Settings.GroupSettings.Destinationsの要素の形式が正しくありません。");
    }

    /// <summary>
    /// PostAsyncでApiErrorがスローされた場合にそのまま再スローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithApiError_ThrowsApiError()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter);
        var body = CreateValidPostRequestBody();
        var expectedException = new ApiError();

        requestAdapter.SendAsync<CommonserviceitemPostResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CommonserviceitemPostResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<CommonserviceitemPostResponse?>(expectedException));

        await Assert.ThrowsAsync<ApiError>(() => builder.PostAsync(body));
    }

    /// <summary>
    /// PostAsyncでその他の例外がスローされた場合にそのまま再スローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithOtherException_ThrowsException()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter);
        var body = CreateValidPostRequestBody();
        var expectedException = new InvalidOperationException("test exception");

        requestAdapter.SendAsync<CommonserviceitemPostResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CommonserviceitemPostResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<CommonserviceitemPostResponse?>(expectedException));

        await Assert.ThrowsAsync<InvalidOperationException>(() => builder.PostAsync(body));
    }

    private static bool HasApiErrorMapping(Dictionary<string, ParsableFactory<IParsable>> errorMapping)
    {
        return errorMapping.TryGetValue("XXX", out var handler)
            && handler == ApiError.CreateFromDiscriminatorValue;
    }

    private static IRequestAdapter CreateRequestAdapter()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var serializationWriter = Substitute.For<ISerializationWriter>();
        serializationWriter.GetSerializedContent().Returns(new MemoryStream());
        var serializationWriterFactory = Substitute.For<ISerializationWriterFactory>();
        serializationWriterFactory.GetSerializationWriter(Arg.Any<string>()).Returns(serializationWriter);
        requestAdapter.SerializationWriterFactory.Returns(serializationWriterFactory);
        return requestAdapter;
    }

    private static PostCommonServiceItemRequest CreateValidPostRequestBody()
    {
        return new PostCommonServiceItemRequest
        {
            CommonServiceItem = new PostCommonServiceItemRequest_CommonServiceItem
            {
                Name = "test-resource",
                Description = "test description",
                Provider = new CommonServiceItemProvider
                {
                    Class = CommonServiceItemProvider_Class.Saknoticedestination
                },
                ServiceClass = CommonServiceItemServiceClass.CloudSaknoticedestination2,
                Settings = new CommonServiceItemSettings
                {
                    DestinationSettings = new DestinationSettings
                    {
                        Type = DestinationSettings_Type.Email,
                        Value = "test@example.com"
                    },
                    GroupSettings = new GroupSettings
                    {
                        Destinations = new List<string> { "123456789012" }
                    },
                    RoutingSettings = new RoutingSettings
                    {
                        MatchLabels = new List<RoutingSettings_MatchLabels>
                        {
                            new RoutingSettings_MatchLabels { Name = "test-name", Value = "test-value" }
                        },
                        SourceID = "123456",
                        TargetGroupID = "654321"
                    }
                },
                Icon = new CommonServiceItemIcon
                {
                    ID = "112233445566"
                },
                Tags = new List<string> { "tag1" }
            }
        };
    }

    private static PostCommonServiceItemRequest CreatePostRequestBodyWithNullParameter(string invalidParameterName)
    {
        var body = CreateValidPostRequestBody();

        switch (invalidParameterName)
        {
            case "CommonServiceItem":
                body.CommonServiceItem = null;
                break;
            case "CommonServiceItem.Description":
                body.CommonServiceItem!.Description = null;
                break;
            case "CommonServiceItem.Icon.ID":
                body.CommonServiceItem!.Icon!.ID = null;
                break;
            case "CommonServiceItem.Name":
                body.CommonServiceItem!.Name = null;
                break;
            case "CommonServiceItem.Provider":
                body.CommonServiceItem!.Provider = null;
                break;
            case "CommonServiceItem.Provider.Class":
                body.CommonServiceItem!.Provider!.Class = null;
                break;
            case "CommonServiceItem.ServiceClass":
                body.CommonServiceItem!.ServiceClass = null;
                break;
            case "CommonServiceItem.Settings":
                body.CommonServiceItem!.Settings = null;
                break;
            case "CommonServiceItem.Settings.DestinationSettings.Type":
                body.CommonServiceItem!.Settings!.DestinationSettings.Type = null;
                break;
            case "CommonServiceItem.Settings.DestinationSettings.Value":
                body.CommonServiceItem!.Settings!.DestinationSettings.Value = null;
                break;
            case "CommonServiceItem.Settings.GroupSettings.Destinations":
                body.CommonServiceItem!.Settings!.GroupSettings.Destinations = null;
                break;
            case "CommonServiceItem.Settings.RoutingSettings.MatchLabels":
                body.CommonServiceItem!.Settings!.RoutingSettings.MatchLabels = null;
                break;
            case "CommonServiceItem.Settings.RoutingSettings.MatchLabels.Name":
                body.CommonServiceItem!.Settings!.RoutingSettings.MatchLabels.ForEach(label => label.Name = null);
                break;
            case "CommonServiceItem.Settings.RoutingSettings.MatchLabels.Value":
                body.CommonServiceItem!.Settings!.RoutingSettings.MatchLabels.ForEach(label => label.Value = null);
                break;
            case "CommonServiceItem.Settings.RoutingSettings.SourceID":
                body.CommonServiceItem!.Settings!.RoutingSettings.SourceID = null;
                break;
            case "CommonServiceItem.Settings.RoutingSettings.TargetGroupID":
                body.CommonServiceItem!.Settings!.RoutingSettings.TargetGroupID = null;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(invalidParameterName), invalidParameterName, null);
        }

        return body;
    }

    private static PostCommonServiceItemRequest CreatePostRequestBodyWithInvalidEnumParameter(string invalidParameterName)
    {
        var body = CreateValidPostRequestBody();

        switch (invalidParameterName)
        {
            case "CommonServiceItem.Provider.Class":
                body.CommonServiceItem!.Provider!.Class = (CommonServiceItemProvider_Class)999;
                break;
            case "CommonServiceItem.ServiceClass":
                body.CommonServiceItem!.ServiceClass = (CommonServiceItemServiceClass)999;
                break;
            case "CommonServiceItem.Settings.DestinationSettings.Type":
                body.CommonServiceItem!.Settings!.DestinationSettings.Type = (DestinationSettings_Type)999;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(invalidParameterName), invalidParameterName, null);
        }

        return body;
    }

    private static PostCommonServiceItemRequest CreatePostRequestBodyWithInvalidRegExPatternParameter(string invalidParameterName, string invalidParameterValue)
    {
        var body = CreateValidPostRequestBody();

        switch (invalidParameterName)
        {
            case "CommonServiceItem.Settings.RoutingSettings.MatchLabels.Name":
                body.CommonServiceItem!.Settings!.RoutingSettings.MatchLabels.ForEach(label => label.Name = invalidParameterValue);
                break;
            case "CommonServiceItem.Settings.RoutingSettings.MatchLabels.Value":
                body.CommonServiceItem!.Settings!.RoutingSettings.MatchLabels.ForEach(label => label.Value = invalidParameterValue);
                break;
            case "CommonServiceItem.Settings.RoutingSettings.SourceID":
                body.CommonServiceItem!.Settings!.RoutingSettings.SourceID = invalidParameterValue;
                break;
            case "CommonServiceItem.Settings.RoutingSettings.TargetGroupID":
                body.CommonServiceItem!.Settings!.RoutingSettings.TargetGroupID = invalidParameterValue;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(invalidParameterName), invalidParameterName, null);
        }

        return body;
    }

    /// <summary>
    /// ToGetRequestInformationがProviderClass指定なしでFilterクエリを含まないRequestInformationを返すことを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_WithoutProviderClass_DoesNotContainFilterQuery()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.GET, requestInfo.HttpMethod);
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
        Assert.DoesNotContain("Filter", requestInfo.URI.ToString());
    }

    /// <summary>
    /// ToGetRequestInformationが無効なProviderClass値を指定した場合にArgumentExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_WithInvalidProviderClass_ThrowsArgumentException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter);

        var exception = Assert.Throws<ArgumentException>(() =>
        {
            builder.ToGetRequestInformation(config =>
            {
                config.QueryParameters = new CommonserviceitemRequestBuilder.CommonserviceitemRequestBuilderGetQueryParameters
                {
                    ProviderClass = (CommonServiceItemProvider_Class)999
                };
            });
        });

        Assert.Contains("Invalid provider class", exception.Message);
    }

    /// <summary>
    /// GetAsyncがProviderClassフィルタを使用して正しく実行されることを確認
    /// </summary>
    [Theory]
    [InlineData(CommonServiceItemProvider_Class.Saknoticedestination, "saknoticedestination")]
    [InlineData(CommonServiceItemProvider_Class.Saknoticegroup, "saknoticegroup")]
    [InlineData(CommonServiceItemProvider_Class.Saknoticerouting, "saknoticerouting")]
    public async Task GetAsync_WithProviderClassFilter_ExecutesSuccessfully(CommonServiceItemProvider_Class providerClass, string expectedClassName)
    {
        var requestAdapter = CreateRequestAdapter();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter);
        var expectedResponse = new ListCommonServiceItemsResponse();
        requestAdapter.SendAsync<ListCommonServiceItemsResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListCommonServiceItemsResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var response = await builder.GetAsync(config =>
        {
            config.QueryParameters = new CommonserviceitemRequestBuilder.CommonserviceitemRequestBuilderGetQueryParameters
            {
                ProviderClass = providerClass
            };
        });

        Assert.NotNull(response);
        Assert.Equal(expectedResponse, response);

        var expectedFilterJson = $"{{\"Filter\":{{\"Provider.Class\":\"{expectedClassName}\"}}}}";
        await requestAdapter.Received(1).SendAsync<ListCommonServiceItemsResponse>(
            Arg.Is<RequestInformation>(ri =>
                ri.URI.ToString().Contains("/commonserviceitem?") &&
                Uri.UnescapeDataString(ri.URI.ToString()).Contains(expectedFilterJson)),
            Arg.Any<ParsableFactory<ListCommonServiceItemsResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// GetAsyncがProviderClassフィルタ指定時にカスタムヘッダーがRequestInformationに反映されることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithProviderClassFilterAndCustomHeader_ReflectsCustomHeaderInRequestInformation()
    {
        var requestAdapter = CreateRequestAdapter();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter);
        var expectedResponse = new ListCommonServiceItemsResponse();
        requestAdapter.SendAsync<ListCommonServiceItemsResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListCommonServiceItemsResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        await builder.GetAsync(config =>
        {
            config.QueryParameters = new CommonserviceitemRequestBuilder.CommonserviceitemRequestBuilderGetQueryParameters
            {
                ProviderClass = CommonServiceItemProvider_Class.Saknoticedestination
            };
            config.Headers.Add("X-Custom-Header", "custom-value");
        });

        await requestAdapter.Received(1).SendAsync<ListCommonServiceItemsResponse>(
            Arg.Is<RequestInformation>(ri =>
                ri.Headers.ContainsKey("X-Custom-Header") &&
                ri.Headers["X-Custom-Header"].Contains("custom-value")),
            Arg.Any<ParsableFactory<ListCommonServiceItemsResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// WithUrlが正しい型を返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object> { { "baseurl", "https://api.example.com" } };
        var builder = new CommonserviceitemRequestBuilder(pathParameters, requestAdapter);

        var result = builder.WithUrl("https://api.example.com/commonserviceitem");

        Assert.NotNull(result);
        Assert.IsType<CommonserviceitemRequestBuilder>(result);
    }
}
