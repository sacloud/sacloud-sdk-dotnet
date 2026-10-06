using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.AddOn.Models;
using Sacloud.Sdk.AddOn.Security.Waf;
using Sacloud.Sdk.AddOn.Security.Waf.Item;
using Sacloud.Sdk.AddOn.Security.Waf.Status;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Sacloud.Sdk.AddOn.Tests.Security.Waf;

/// <summary>
/// WafRequestBuilderのテスト
/// </summary>
public class WafRequestBuilderTests
{
    private readonly IRequestAdapter _mockRequestAdapter;
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<WafRequestBuilder> _mockLogger;

    public WafRequestBuilderTests()
    {
        _mockRequestAdapter = Substitute.For<IRequestAdapter>();
        _mockRequestAdapter.BaseUrl = "https://api.example.com";
        _mockLogger = Substitute.For<ILogger<WafRequestBuilder>>();
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLoggerFactory.CreateLogger<WafRequestBuilder>().Returns(_mockLogger);
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
        };

        var builder = new WafRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（RawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithRawUrl_InitializesSuccessfully()
    {
        var rawUrl = "https://api.example.com/security/waf";

        var builder = new WafRequestBuilder(rawUrl, _mockRequestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// インデクサーが正しいWithResourceGroupNameItemRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_ReturnsWithResourceGroupNameItemRequestBuilder()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new WafRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var itemBuilder = builder["test-resourceGroupName"];

        Assert.NotNull(itemBuilder);
        Assert.IsType<WithResourceGroupNameItemRequestBuilder>(itemBuilder);
    }

    /// <summary>
    /// インデクサーが毎回新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void Indexer_ReturnsNewInstance_OnEachCall()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new WafRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var itemBuilder1 = builder["test-resourceGroupName"];
        var itemBuilder2 = builder["test-resourceGroupName"];

        Assert.NotSame(itemBuilder1, itemBuilder2);
    }

    /// <summary>
    /// Statusプロパティが正しくStatusRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Status_ReturnsStatusRequestBuilder()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new WafRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var result = builder.Status;

        Assert.NotNull(result);
        Assert.IsType<StatusRequestBuilder>(result);
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
        };
        var builder = new WafRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

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
        };
        var builder = new WafRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// ToPostRequestInformationがPOSTメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_CreatesRequestInformationWithPostMethod()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new WafRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToPostRequestInformation(new WafRequestBody());

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.POST, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToPostRequestInformationが正しいAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_SetsAcceptHeader()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new WafRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToPostRequestInformation(new WafRequestBody());

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
        Assert.Contains("application/json", requestInfo.Headers["Accept"]);
    }

    /// <summary>
    /// ToPostRequestInformationが正しいContent-Typeヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_SetsContentTypeHeader()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new WafRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToPostRequestInformation(new WafRequestBody());

        Assert.True(requestInfo.Headers.ContainsKey("Content-Type"));
        Assert.Contains("application/json", requestInfo.Headers["Content-Type"]);
    }

    /// <summary>
    /// GetAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithApiError_ThrowsApiException()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new WafRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);
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

        _mockRequestAdapter.SendAsync<ListResourcesResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListResourcesResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(async () =>
        {
            await builder.GetAsync();
        });

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                Regex.IsMatch(state.ToString()!,
                @"^WAFサービスリソース一覧取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncでその他のエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithOtherError_ThrowsException()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new WafRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        _mockRequestAdapter.SendAsync<ListResourcesResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListResourcesResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<Exception>(async () =>
        {
            await builder.GetAsync();
        });

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                Regex.IsMatch(state.ToString()!,
                @"^WAFサービスリソース一覧の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsync が期待されるSendAsyncを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_CallsRequestAdapterSendAsync()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new WafRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var expectedResponse = new ListResourcesResponse();
        _mockRequestAdapter.SendAsync<ListResourcesResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListResourcesResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(expectedResponse);

        var result = await builder.GetAsync();

        await _mockRequestAdapter.Received(1).SendAsync<ListResourcesResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ListResourcesResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());

        Assert.Equal(expectedResponse, result);

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                Regex.IsMatch(state.ToString()!,
                @"^WAFサービスリソース一覧の取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                Regex.IsMatch(state.ToString()!,
                @"^WAFサービスリソース一覧の取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncのリクエストボディがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithNullRequestBody_ThrowsValidationException()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new WafRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(null));

        Assert.Equal("リクエストボディは必須です。", exception.Message);
        _mockLogger.Received(1).LogWarning("リクエストボディは必須です。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディパラメータが未入力の場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("Location")]
    [InlineData("Endpoint")]
    [InlineData("Endpoint.Route")]
    [InlineData("Endpoint.Route.Patterns")]
    [InlineData("Endpoint.Route.OriginGroup")]
    [InlineData("Endpoint.Route.OriginGroup.Origin")]
    [InlineData("Endpoint.Route.OriginGroup.Origin.HostName")]
    [InlineData("Endpoint.Route.OriginGroup.Origin.HostHeader")]
    [InlineData("Profile")]
    [InlineData("Profile.Level")]
    public async Task PostAsync_WithNullParameter_ThrowsValidationException(string invalidParameterName)
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new WafRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);
        var body = new WafRequestBody
        {
            Location = invalidParameterName == "Location" ? null : WafRequestBody_location.Japaneast,
            Endpoint = invalidParameterName == "Endpoint" ? null : new FrontDoorEndpoint
            {
                Route = invalidParameterName == "Endpoint.Route" ? null : new FrontDoorRoute
                {
                    Patterns = invalidParameterName == "Endpoint.Route.Patterns" ? null : new List<string> { "/path1/*", "/path2/*" },
                    OriginGroup = invalidParameterName == "Endpoint.Route.OriginGroup" ? null : new FrontDoorOriginGroup
                    {
                        Origin = invalidParameterName == "Endpoint.Route.OriginGroup.Origin" ? null : new FrontDoorOrigin
                        {
                            HostName = invalidParameterName == "Endpoint.Route.OriginGroup.Origin.HostName" ? null : "example.com",
                            HostHeader = invalidParameterName == "Endpoint.Route.OriginGroup.Origin.HostHeader" ? null : "example.com"
                        }
                    }
                }
            },
            Profile = invalidParameterName == "Profile" ? null : new FrontDoorProfile
            {
                Level = invalidParameterName == "Profile.Level" ? null : 1
            },
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal($"{invalidParameterName}が入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}が入力されていません。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディパラメータが最大文字数を超えた場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("Endpoint.Route.OriginGroup.Origin.HostName", 101, 100)]
    [InlineData("Endpoint.Route.OriginGroup.Origin.HostHeader", 101, 100)]
    public async Task PostAsync_WithInvalidMaxLengthRequestBodyParameter_ThrowsValidationException(string invalidParameterName, int invalidParameterValueLength, int validMaxLength)
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new WafRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);
        var body = new WafRequestBody
        {
            Location = WafRequestBody_location.Japaneast,
            Endpoint = new FrontDoorEndpoint
            {
                Route = new FrontDoorRoute
                {
                    Patterns = new List<string> { "/path1/*", "/path2/*" },
                    OriginGroup = new FrontDoorOriginGroup
                    {
                        Origin = new FrontDoorOrigin
                        {
                            HostName = invalidParameterName == "Endpoint.Route.OriginGroup.Origin.HostName" ? new string('a', invalidParameterValueLength) : "example.com",
                            HostHeader = invalidParameterName == "Endpoint.Route.OriginGroup.Origin.HostHeader" ? new string('a', invalidParameterValueLength) : "example.com"
                        }
                    }
                }
            },
            Profile = new FrontDoorProfile
            {
                Level = 1
            },
        };
        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));
        Assert.Equal($"{invalidParameterName}は{validMaxLength}文字以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning($"{invalidParameterName}は{validMaxLength}文字以下で入力してください。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディパラメータがEnum範囲外の値の場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithOutOfEnumRequestBodyParameter_ThrowsValidationException()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new WafRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);
        var body = new WafRequestBody
        {
            Location = (WafRequestBody_location)99,
            Endpoint = new FrontDoorEndpoint
            {
                Route = new FrontDoorRoute
                {
                    Patterns = new List<string> { "/path1/*", "/path2/*" },
                    OriginGroup = new FrontDoorOriginGroup
                    {
                        Origin = new FrontDoorOrigin
                        {
                            HostName = "example.com",
                            HostHeader = "example.com"
                        }
                    }
                }
            },
            Profile = new FrontDoorProfile
            {
                Level = 1
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal("LocationはEnumに含まれる値を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("LocationはEnumに含まれる値を入力してください。");
    }

    /// <summary>
    /// PostAsyncのリクエストボディパラメータが許可されていない値の場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(3)]
    public async Task PostAsync_WithNotAllowedRequestBodyParameter_ThrowsValidationException(int invalidParameterValue)
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new WafRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);
        var body = new WafRequestBody
        {
            Location = WafRequestBody_location.Japaneast,
            Endpoint = new FrontDoorEndpoint
            {
                Route = new FrontDoorRoute
                {
                    Patterns = new List<string> { "/path1/*", "/path2/*" },
                    OriginGroup = new FrontDoorOriginGroup
                    {
                        Origin = new FrontDoorOrigin
                        {
                            HostName = "example.com",
                            HostHeader = "example.com"
                        }
                    }
                }
            },
            Profile = new FrontDoorProfile
            {
                Level = invalidParameterValue
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal("Profile.Levelには許可された値（1, 2）を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("Profile.Levelには許可された値（1, 2）を入力してください。");
    }

    /// <summary>
    /// PostAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithApiError_ThrowsApiException()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new WafRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);
        var body = new WafRequestBody
        {
            Location = WafRequestBody_location.Japaneast,
            Endpoint = new FrontDoorEndpoint
            {
                Route = new FrontDoorRoute
                {
                    Patterns = new List<string> { "/path1/*", "/path2/*" },
                    OriginGroup = new FrontDoorOriginGroup
                    {
                        Origin = new FrontDoorOrigin
                        {
                            HostName = "example.com",
                            HostHeader = "example.com"
                        }
                    }
                }
            },
            Profile = new FrontDoorProfile
            {
                Level = 1
            }
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

        _mockRequestAdapter.SendAsync<PostDeploymentResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PostDeploymentResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(async () =>
        {
            await builder.PostAsync(body);
        });

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                Regex.IsMatch(state.ToString()!,
                @"^WAFサービスリソース作成のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncでその他のエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_WithOtherError_ThrowsException()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new WafRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);
        var body = new WafRequestBody
        {
            Location = WafRequestBody_location.Japaneast,
            Endpoint = new FrontDoorEndpoint
            {
                Route = new FrontDoorRoute
                {
                    Patterns = new List<string> { "/path1/*", "/path2/*" },
                    OriginGroup = new FrontDoorOriginGroup
                    {
                        Origin = new FrontDoorOrigin
                        {
                            HostName = "example.com",
                            HostHeader = "example.com"
                        }
                    }
                }
            },
            Profile = new FrontDoorProfile
            {
                Level = 1
            }
        };
        var expectedException = new Exception();

        _mockRequestAdapter.SendAsync<PostDeploymentResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PostDeploymentResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<Exception>(async () =>
        {
            await builder.PostAsync(body);
        });

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                Regex.IsMatch(state.ToString()!,
                @"^WAFサービスリソースの作成中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsync が期待されるSendAsyncを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_CallsRequestAdapterSendAsync()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
        };
        var builder = new WafRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);
        var body = new WafRequestBody
        {
            Location = WafRequestBody_location.Japaneast,
            Endpoint = new FrontDoorEndpoint
            {
                Route = new FrontDoorRoute
                {
                    Patterns = new List<string> { "/path1/*", "/path2/*" },
                    OriginGroup = new FrontDoorOriginGroup
                    {
                        Origin = new FrontDoorOrigin
                        {
                            HostName = "example.com",
                            HostHeader = "example.com"
                        }
                    }
                }
            },
            Profile = new FrontDoorProfile
            {
                Level = 1
            }
        };

        var expectedResponse = new PostDeploymentResponse();
        _mockRequestAdapter.SendAsync<PostDeploymentResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PostDeploymentResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(expectedResponse);

        var result = await builder.PostAsync(body);

        await _mockRequestAdapter.Received(1).SendAsync<PostDeploymentResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PostDeploymentResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());

        Assert.Equal(expectedResponse, result);

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                Regex.IsMatch(state.ToString()!,
                @"^WAFサービスリソースの作成を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                Regex.IsMatch(state.ToString()!,
                @"^WAFサービスリソースの作成が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 202\]$")),
            null,
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
        };
        var builder = new WafRequestBuilder(pathParameters, _mockRequestAdapter, _mockLoggerFactory);
        var newUrl = "https://newapi.example.com/security/waf";

        var newBuilder = builder.WithUrl(newUrl);

        Assert.NotNull(newBuilder);
        Assert.NotSame(builder, newBuilder);
        Assert.IsType<WafRequestBuilder>(newBuilder);
    }
}
