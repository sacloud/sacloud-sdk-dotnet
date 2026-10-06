using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.Shared.Models;
using Sacloud.Sdk.SimpleNotification.Commonserviceitem.Item;
using Sacloud.Sdk.SimpleNotification.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.SimpleNotification.Tests.Commonserviceitem.Item;

/// <summary>
/// CommonserviceitemItemRequestBuilderのテスト
/// </summary>
public class CommonserviceitemItemRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<CommonserviceitemItemRequestBuilder> _mockLogger;

    public CommonserviceitemItemRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<CommonserviceitemItemRequestBuilder>>();
        _mockLoggerFactory.CreateLogger<CommonserviceitemItemRequestBuilder>().Returns(_mockLogger);
    }

    /// <summary>
    /// コンストラクタ（PathParameters版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "123456789012" }
        };

        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();

        var builder = new CommonserviceitemItemRequestBuilder("https://api.example.com/commonserviceitem/123456789012", requestAdapter);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// Simplenotificationプロパティが正しい型を返すことを確認
    /// </summary>
    [Fact]
    public void Simplenotification_ReturnsSimplenotificationRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "123456789012" }
        };
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter);

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
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "123456789012" }
        };
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.GET, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToPutRequestInformationが正しいRequestInformationを生成することを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_ReturnsCorrectRequestInformation()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var serializationWriter = Substitute.For<ISerializationWriter>();
        serializationWriter.GetSerializedContent().Returns(new MemoryStream());
        var serializationWriterFactory = Substitute.For<ISerializationWriterFactory>();
        serializationWriterFactory.GetSerializationWriter(Arg.Any<string>()).Returns(serializationWriter);
        requestAdapter.SerializationWriterFactory.Returns(serializationWriterFactory);
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "123456789012" }
        };
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter);
        var body = new PutCommonServiceItemRequest
        {
            CommonServiceItem = new PutCommonServiceItemRequest_CommonServiceItem
            {
                Name = "test-resource",
                Description = "test description",
                Settings = new CommonServiceItemSettings
                {
                    DestinationSettings = new DestinationSettings
                    {
                        Type = DestinationSettings_Type.Email,
                        Value = "test@example.com"
                    }
                },
                Icon = new CommonServiceItemIcon
                {
                    ID = "112233445566"
                },
                Tags = new List<string> { "tag1" }
            }
        };

        var requestInfo = builder.ToPutRequestInformation(body);

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.PUT, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToPutRequestInformationがnullボディで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_WithNullBody_ThrowsArgumentNullException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "123456789012" }
        };
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter);

        Assert.Throws<ArgumentNullException>(() => builder.ToPutRequestInformation(null!));
    }

    /// <summary>
    /// ToDeleteRequestInformationが正しいRequestInformationを生成することを確認
    /// </summary>
    [Fact]
    public void ToDeleteRequestInformation_ReturnsCorrectRequestInformation()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "123456789012" }
        };
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter);

        var requestInfo = builder.ToDeleteRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.DELETE, requestInfo.HttpMethod);
    }

    /// <summary>
    /// DeleteAsyncがRequestAdapter.SendAsyncを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_CallsRequestAdapterSendAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "123456789012" }
        };
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter);
        var expectedResponse = new CommonserviceitemDeleteResponse();

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CommonserviceitemDeleteResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(expectedResponse);

        var result = await builder.DeleteAsync();

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CommonserviceitemDeleteResponse>>(),
            Arg.Is<Dictionary<string, ParsableFactory<IParsable>>>(x => HasApiErrorMapping(x)),
            Arg.Any<CancellationToken>());
        Assert.Equal(expectedResponse, result);
    }

    /// <summary>
    /// DeleteAsyncでApiErrorがスローされた場合にそのまま再スローされることを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_WithApiError_ThrowsApiError()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = CreateValidPathParameters();
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter);
        var expectedException = new ApiError();

        requestAdapter.SendAsync<CommonserviceitemDeleteResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CommonserviceitemDeleteResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<CommonserviceitemDeleteResponse?>(expectedException));

        await Assert.ThrowsAsync<ApiError>(() => builder.DeleteAsync());
    }

    /// <summary>
    /// DeleteAsyncでその他の例外がスローされた場合にそのまま再スローされることを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_WithOtherException_ThrowsException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = CreateValidPathParameters();
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter);
        var expectedException = new InvalidOperationException("test exception");

        requestAdapter.SendAsync<CommonserviceitemDeleteResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CommonserviceitemDeleteResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<CommonserviceitemDeleteResponse?>(expectedException));

        await Assert.ThrowsAsync<InvalidOperationException>(() => builder.DeleteAsync());
    }

    /// <summary>
    /// GetAsyncがRequestAdapter.SendAsyncを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_CallsRequestAdapterSendAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "123456789012" }
        };
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter);
        var expectedResponse = new CommonserviceitemGetResponse();

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CommonserviceitemGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(expectedResponse);

        var result = await builder.GetAsync();

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CommonserviceitemGetResponse>>(),
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
        var pathParameters = CreateValidPathParameters();
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter);
        var expectedException = new ApiError();

        requestAdapter.SendAsync<CommonserviceitemGetResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CommonserviceitemGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<CommonserviceitemGetResponse?>(expectedException));

        await Assert.ThrowsAsync<ApiError>(() => builder.GetAsync());
    }

    /// <summary>
    /// GetAsyncでその他の例外がスローされた場合にそのまま再スローされることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithOtherException_ThrowsException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = CreateValidPathParameters();
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter);
        var expectedException = new InvalidOperationException("test exception");

        requestAdapter.SendAsync<CommonserviceitemGetResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CommonserviceitemGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<CommonserviceitemGetResponse?>(expectedException));

        await Assert.ThrowsAsync<InvalidOperationException>(() => builder.GetAsync());
    }

    /// <summary>
    /// PutAsyncがRequestAdapter.SendAsyncを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_CallsRequestAdapterSendAsync()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = CreateValidPathParameters();
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter);
        var body = CreateValidPutRequestBody();
        var expectedResponse = new CommonserviceitemPutResponse();

        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CommonserviceitemPutResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(expectedResponse);

        var result = await builder.PutAsync(body);

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CommonserviceitemPutResponse>>(),
            Arg.Is<Dictionary<string, ParsableFactory<IParsable>>>(x => HasApiErrorMapping(x)),
            Arg.Any<CancellationToken>());
        Assert.Equal(expectedResponse, result);
    }

    /// <summary>
    /// PutAsyncがnullボディでバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithNullBody_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "123456789012" }
        };
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter);

        await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(null!));
    }

    /// <summary>
    /// PutAsyncのリクエストボディの必須パラメータがnullの場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Theory]
    [InlineData("CommonServiceItem")]
    [InlineData("CommonServiceItem.Description")]
    [InlineData("CommonServiceItem.Icon.ID")]
    [InlineData("CommonServiceItem.Name")]
    [InlineData("CommonServiceItem.Settings.DestinationSettings.Type")]
    [InlineData("CommonServiceItem.Settings.DestinationSettings.Value")]
    [InlineData("CommonServiceItem.Settings.GroupSettings.Destinations")]
    [InlineData("CommonServiceItem.Settings.RoutingSettings.MatchLabels")]
    [InlineData("CommonServiceItem.Settings.RoutingSettings.MatchLabels.Name")]
    [InlineData("CommonServiceItem.Settings.RoutingSettings.MatchLabels.Value")]
    [InlineData("CommonServiceItem.Settings.RoutingSettings.SourceID")]
    [InlineData("CommonServiceItem.Settings.RoutingSettings.TargetGroupID")]
    public async Task PutAsync_WithNullRequestBodyParameter_ThrowsValidationException(string invalidParameterName)
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = CreateValidPathParameters();
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = CreatePutRequestBodyWithNullParameter(invalidParameterName);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal($"{invalidParameterName}が入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}が入力されていません。");
    }

    /// <summary>
    /// PutAsyncのリクエストボディのEnumパラメータがEnum定義外値の場合にバリデーション例外をスローすることを確認
    /// </summary>
    [Theory]
    [InlineData("CommonServiceItem.Settings.DestinationSettings.Type")]
    public async Task PutAsync_WithInvalidEnumRequestBodyParameter_ThrowsValidationException(string invalidParameterName)
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = CreateValidPathParameters();
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = CreatePutRequestBodyWithInvalidEnumParameter(invalidParameterName);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal($"{invalidParameterName}はEnumに含まれる値を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}はEnumに含まれる値を入力してください。");
    }

    /// <summary>
    /// PutAsyncのリクエストボディの正規表現パターンに一致しなかった場合に例外がスローされることを確認
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
    public async Task PutAsync_WithInvalidRegexPatternRequestBodyParameter_ThrowsValidationException(string invalidParameterName, string invalidParameterValue)
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = CreateValidPathParameters();
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = CreatePutRequestBodyWithInvalidRegExPatternParameter(invalidParameterName, invalidParameterValue);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal($"{invalidParameterName}の形式が正しくありません。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}の形式が正しくありません。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディの配列要素のうち正規表現パターンに一致しないものがあった場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithInvalidArrayElementRegularExpressionRequestBodyParameter_ThrowsValidationException   ()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = CreateValidPathParameters();
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PutCommonServiceItemRequest
        {
            CommonServiceItem = new PutCommonServiceItemRequest_CommonServiceItem
            {
                Name = "test-resource",
                Description = "test-description",
                Settings = new CommonServiceItemSettings
                {
                    GroupSettings = new GroupSettings
                    {
                        Destinations = new List<string> { "invalid-destination" }
                    }
                }
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal("CommonServiceItem.Settings.GroupSettings.Destinationsの要素の形式が正しくありません。", exception.Message);
        _mockLogger.Received(1).LogWarning("CommonServiceItem.Settings.GroupSettings.Destinationsの要素の形式が正しくありません。");
    }

    /// <summary>
    /// PutAsyncでApiErrorがスローされた場合にそのまま再スローされることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithApiError_ThrowsApiError()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = CreateValidPathParameters();
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter);
        var body = CreateValidPutRequestBody();
        var expectedException = new ApiError();

        requestAdapter.SendAsync<CommonserviceitemPutResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CommonserviceitemPutResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<CommonserviceitemPutResponse?>(expectedException));

        await Assert.ThrowsAsync<ApiError>(() => builder.PutAsync(body));
    }

    /// <summary>
    /// PutAsyncでその他の例外がスローされた場合にそのまま再スローされることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithOtherException_ThrowsException()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = CreateValidPathParameters();
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter);
        var body = CreateValidPutRequestBody();
        var expectedException = new InvalidOperationException("test exception");

        requestAdapter.SendAsync<CommonserviceitemPutResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CommonserviceitemPutResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<CommonserviceitemPutResponse?>(expectedException));

        await Assert.ThrowsAsync<InvalidOperationException>(() => builder.PutAsync(body));
    }

    /// <summary>
    /// GetAsyncが不正なid形式でバリデーション例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithInvalidId_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "123" }
        };
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync());

        Assert.Contains("リソースID", exception.Message);
    }

    private static bool HasApiErrorMapping(Dictionary<string, ParsableFactory<IParsable>> errorMapping)
    {
        return errorMapping.TryGetValue("XXX", out var handler)
            && handler == ApiError.CreateFromDiscriminatorValue;
    }

    private static Dictionary<string, object> CreateValidPathParameters()
    {
        return new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "123456789012" }
        };
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

    private static PutCommonServiceItemRequest CreateValidPutRequestBody()
    {
        return new PutCommonServiceItemRequest
        {
            CommonServiceItem = new PutCommonServiceItemRequest_CommonServiceItem
            {
                Name = "test-resource",
                Description = "test description",
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

    private static PutCommonServiceItemRequest CreatePutRequestBodyWithNullParameter(string invalidParameterName)
    {
        var body = CreateValidPutRequestBody();

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

    private static PutCommonServiceItemRequest CreatePutRequestBodyWithInvalidEnumParameter(string invalidParameterName)
    {
        var body = CreateValidPutRequestBody();

        switch (invalidParameterName)
        {
            case "CommonServiceItem.Settings.DestinationSettings.Type":
                body.CommonServiceItem!.Settings!.DestinationSettings.Type = (DestinationSettings_Type)999;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(invalidParameterName), invalidParameterName, null);
        }

        return body;
    }

    private static PutCommonServiceItemRequest CreatePutRequestBodyWithInvalidRegExPatternParameter(string invalidParameterName, string invalidParameterValue)
    {
        var body = CreateValidPutRequestBody();

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
    /// WithUrlが正しい型を返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "123456789012" }
        };
        var builder = new CommonserviceitemItemRequestBuilder(pathParameters, requestAdapter);

        var result = builder.WithUrl("https://api.example.com/commonserviceitem/123456789012");

        Assert.NotNull(result);
        Assert.IsType<CommonserviceitemItemRequestBuilder>(result);
    }
}
