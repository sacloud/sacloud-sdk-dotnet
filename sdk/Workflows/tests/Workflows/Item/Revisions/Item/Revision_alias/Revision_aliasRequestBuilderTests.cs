using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.Shared.Models;
using Sacloud.Sdk.Workflows.Models;
using Sacloud.Sdk.Workflows.Workflows.Item.Revisions;
using Sacloud.Sdk.Workflows.Workflows.Item.Revisions.Item.Revision_alias;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.Workflows.Tests.Workflows.Item.Revisions.Item.Revision_alias;

/// <summary>
/// Revision_aliasRequestBuilderのテスト
/// </summary>
public class Revision_aliasRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<Revision_aliasRequestBuilder> _mockLogger;

    public Revision_aliasRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<Revision_aliasRequestBuilder>>();
        _mockLoggerFactory.CreateLogger<Revision_aliasRequestBuilder>().Returns(_mockLogger);
    }

    /// <summary>
    /// コンストラクタ（PathParameters版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithPathParameters_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "workflow-123" },
            { "revisionId", 1 }
        };

        var builder = new Revision_aliasRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var rawUrl = "https://api.example.com/workflows/workflow-123/revisions/1/revision_alias";

        var builder = new Revision_aliasRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// ToDeleteRequestInformationがDELETEメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToDeleteRequestInformation_CreatesRequestInformationWithDeleteMethod()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "workflow-123" },
            { "revisionId", 1 }
        };
        var builder = new Revision_aliasRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToDeleteRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.DELETE, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToPutRequestInformationがPUTメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_CreatesRequestInformationWithPutMethod()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "workflow-123" },
            { "revisionId", 1 }
        };
        var builder = new Revision_aliasRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateWorkflowRevisionAlias();

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
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "workflow-123" },
            { "revisionId", 1 }
        };
        var builder = new Revision_aliasRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.Throws<ArgumentNullException>(() => builder.ToPutRequestInformation(null!));
    }

    /// <summary>
    /// ToDeleteRequestInformationが正しいAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToDeleteRequestInformation_SetsAcceptHeader()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "workflow-123" },
            { "revisionId", 1 }
        };
        var builder = new Revision_aliasRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToDeleteRequestInformation();

        Assert.True(requestInfo.Headers.TryGetValue("Accept", out var acceptHeader));
        Assert.Contains("application/json", acceptHeader);
    }

    /// <summary>
    /// ToPutRequestInformationが正しいAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToPutRequestInformation_SetsAcceptHeader()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "workflow-123" },
            { "revisionId", 1 }
        };
        var builder = new Revision_aliasRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateWorkflowRevisionAlias();

        var requestInfo = builder.ToPutRequestInformation(body);

        Assert.True(requestInfo.Headers.TryGetValue("Accept", out var acceptHeader));
        Assert.Contains("application/json", acceptHeader);
    }

    /// <summary>
    /// WithUrlが正しい新しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_ReturnsNewInstance()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "workflow-123" },
            { "revisionId", 1 }
        };
        var builder = new Revision_aliasRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var rawUrl = "https://api.example.com/workflows/workflow-123/revisions/1/revision_alias";

        var newBuilder = builder.WithUrl(rawUrl);

        Assert.NotNull(newBuilder);
        Assert.IsType<Revision_aliasRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }

    /// <summary>
    /// DeleteAsyncのパスパラメータにidがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("id", "")]
    [InlineData("id", " ")]
    [InlineData("id", "　")]
    public async Task DeleteAsync_WithEmptyId_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { pathParameterKey, pathParameterValue },
            { "revisionId", 1 }
        };
        var builder = new Revision_aliasRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new DeleteWorkflowRevisionAliasResponse();

        requestAdapter.SendAsync<DeleteWorkflowRevisionAliasResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<DeleteWorkflowRevisionAliasResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.DeleteAsync();
        });

        Assert.Equal("ワークフローIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("ワークフローIDを入力してください。");
    }

    /// <summary>
    /// DeleteAsyncのパスパラメータに無効な文字数の値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_WithInvalidLengthId_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "1234567890123456789012345678901234567" },
            { "revisionId", 1 }
        };
        var builder = new Revision_aliasRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new DeleteWorkflowRevisionAliasResponse();

        requestAdapter.SendAsync<DeleteWorkflowRevisionAliasResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<DeleteWorkflowRevisionAliasResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.DeleteAsync();
        });

        Assert.Equal("ワークフローIDは36文字以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("ワークフローIDは36文字以下で入力してください。");
    }

    /// <summary>
    /// DeleteAsyncのパスパラメータにrevisionIdがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("revisionId", "")]
    [InlineData("revisionId", " ")]
    [InlineData("revisionId", "　")]
    public async Task DeleteAsync_WithEmptyRevisionId_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "workflow-123" },
            { pathParameterKey, pathParameterValue }
        };
        var builder = new Revision_aliasRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new DeleteWorkflowRevisionAliasResponse();

        requestAdapter.SendAsync<DeleteWorkflowRevisionAliasResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<DeleteWorkflowRevisionAliasResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.DeleteAsync();
        });

        Assert.Equal("リビジョンIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("リビジョンIDを入力してください。");
    }

    /// <summary>
    /// DeleteAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_WithApiError_ThrowsApiException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "workflow-123" },
            { "revisionId", 1 }
        };
        var builder = new Revision_aliasRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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

        requestAdapter.SendAsync<DeleteWorkflowRevisionAliasResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<DeleteWorkflowRevisionAliasResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(async () => await builder.DeleteAsync());

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^ワークフローのリビジョンエイリアス削除のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// DeleteAsyncでその他のエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_WithOtherError_ThrowsException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "workflow-123" },
            { "revisionId", 1 }
        };
        var builder = new Revision_aliasRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new Exception();

        requestAdapter.SendAsync<DeleteWorkflowRevisionAliasResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<DeleteWorkflowRevisionAliasResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<Exception>(async () => await builder.DeleteAsync());

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^ワークフローのリビジョンエイリアスの削除中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// DeleteAsyncがRequestAdapter.SendAsyncを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_CallsRequestAdapterSendAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "workflow-123" },
            { "revisionId", 1 }
        };
        var builder = new Revision_aliasRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedResponse = new DeleteWorkflowRevisionAliasResponse();

        requestAdapter.SendAsync<DeleteWorkflowRevisionAliasResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<DeleteWorkflowRevisionAliasResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(expectedResponse);

        var result = await builder.DeleteAsync();

        await requestAdapter.Received(1).SendAsync<DeleteWorkflowRevisionAliasResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<DeleteWorkflowRevisionAliasResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());
        Assert.Equal(expectedResponse, result);

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^ワークフローのリビジョンエイリアスの削除を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^ワークフローのリビジョンエイリアスの削除が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PutAsyncのパスパラメータにidがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("id", "")]
    [InlineData("id", " ")]
    [InlineData("id", "　")]
    public async Task PutAsync_WithEmptyId_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { pathParameterKey, pathParameterValue },
            { "revisionId", 1 }
        };
        var builder = new Revision_aliasRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateWorkflowRevisionAlias();
        var expectedResponse = new UpdateWorkflowRevisionAliasResponse();

        requestAdapter.SendAsync<UpdateWorkflowRevisionAliasResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<UpdateWorkflowRevisionAliasResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(body);
        });

        Assert.Equal("ワークフローIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("ワークフローIDを入力してください。");
    }

    /// <summary>
    /// PutAsyncのパスパラメータに無効な文字数の値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithInvalidLengthId_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "1234567890123456789012345678901234567" },
            { "revisionId", 1 }
        };
        var builder = new Revision_aliasRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateWorkflowRevisionAlias();
        var expectedResponse = new UpdateWorkflowRevisionAliasResponse();

        requestAdapter.SendAsync<UpdateWorkflowRevisionAliasResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<UpdateWorkflowRevisionAliasResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(body);
        });

        Assert.Equal("ワークフローIDは36文字以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("ワークフローIDは36文字以下で入力してください。");
    }

    /// <summary>
    /// PutAsyncのパスパラメータにrevisionIdがないか無効な値を入力した場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalidKey", "test-value")]
    [InlineData("revisionId", "")]
    [InlineData("revisionId", " ")]
    [InlineData("revisionId", "　")]
    public async Task PutAsync_WithEmptyRevisionId_ThrowsValidationException(string pathParameterKey, string pathParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "workflow-123" },
            { pathParameterKey, pathParameterValue }
        };
        var builder = new Revision_aliasRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateWorkflowRevisionAlias();
        var expectedResponse = new UpdateWorkflowRevisionAliasResponse();

        requestAdapter.SendAsync<UpdateWorkflowRevisionAliasResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<UpdateWorkflowRevisionAliasResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(body);
        });

        Assert.Equal("リビジョンIDを入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("リビジョンIDを入力してください。");
    }

    /// <summary>
    /// PutAsyncのリクエストボディがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithNullBody_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "workflow-123" },
            { "revisionId", 1 }
        };
        var builder = new Revision_aliasRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(null);
        });

        Assert.Equal("リクエストボディは必須です。", exception.Message);
        _mockLogger.Received(1).LogWarning("リクエストボディは必須です。");
    }

    /// <summary>
    /// PutAsyncのリクエストボディのパラメータがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithNullParameter_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "workflow-123" },
            { "revisionId", 1 }
        };
        var builder = new Revision_aliasRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateWorkflowRevisionAlias
        {
            RevisionAlias = null
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(body);
        });

        Assert.Equal("RevisionAliasが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("RevisionAliasが入力されていません。");
    }

    /// <summary>
    /// PutAsyncのリクエストボディのパラメータの値が有効な最大文字数を超過した場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithInvalidLengthParameter_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "workflow-123" },
            { "revisionId", 1 }
        };
        var builder = new Revision_aliasRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateWorkflowRevisionAlias
        {
            RevisionAlias = new string('a', 65)
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(body);
        });

        Assert.Equal("RevisionAliasは64文字以下で入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("RevisionAliasは64文字以下で入力してください。");
    }

    /// <summary>
    /// PutAsyncのパスパラメータの文字数が正規表現パターンに一致しなかった場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("invalid+revision-alias")]
    [InlineData("invalid&revision-alias")]
    public async Task PutAsync_WithInvalidRegexPatternRequestBodyParameter_ThrowsValidationException(string invalidParameterValue)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "workflow-123" },
            { "revisionId", 1 }
        };
        var builder = new Revision_aliasRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateWorkflowRevisionAlias
        {
            RevisionAlias = invalidParameterValue
        };
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(body);
        });
        Assert.Equal("RevisionAliasの形式が正しくありません。", exception.Message);
        _mockLogger.Received(1).LogWarning("RevisionAliasの形式が正しくありません。");
    }

    /// <summary>
    /// PutAsyncのリクエストボディのパラメータが複数のバリデーションエラーになる場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithMultipleInvalidRequestBodyParameters_ThrowsValidationException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "workflow-123" },
            { "revisionId", 1 }
        };
        var builder = new Revision_aliasRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateWorkflowRevisionAlias
        {
            RevisionAlias = new string('@', 65)
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await builder.PutAsync(body);
        });

        var expectedMessage = """
            RevisionAliasは64文字以下で入力してください。
            RevisionAliasの形式が正しくありません。
            """;
        Assert.Equal(expectedMessage, exception.Message);
        _mockLogger.Received(1).LogWarning("RevisionAliasは64文字以下で入力してください。");
        _mockLogger.Received(1).LogWarning("RevisionAliasの形式が正しくありません。");
    }

    /// <summary>
    /// PutAsyncでAPIエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithApiError_ThrowsApiException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "workflow-123" },
            { "revisionId", 1 }
        };
        var builder = new Revision_aliasRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateWorkflowRevisionAlias
        {
            RevisionAlias = "test-alias"
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

        requestAdapter.SendAsync<UpdateWorkflowRevisionAliasResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<UpdateWorkflowRevisionAliasResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(async () => await builder.PutAsync(body));

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^ワークフローのリビジョンエイリアス更新のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PutAsyncでその他のエラーが発生した場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithOtherError_ThrowsException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "workflow-123" },
            { "revisionId", 1 }
        };
        var builder = new Revision_aliasRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateWorkflowRevisionAlias
        {
            RevisionAlias = "test-alias"
        };
        var expectedException = new Exception();

        requestAdapter.SendAsync<UpdateWorkflowRevisionAliasResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<UpdateWorkflowRevisionAliasResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<Exception>(async () => await builder.PutAsync(body));

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                state.ToString()!,
                @"^ワークフローのリビジョンエイリアスの更新中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PutAsyncがRequestAdapter.SendAsyncを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_CallsRequestAdapterSendAsync()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "id", "workflow-123" },
            { "revisionId", 1 }
        };
        var builder = new Revision_aliasRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new UpdateWorkflowRevisionAlias
        {
            RevisionAlias = "test-alias"
        };
        var expectedResponse = new UpdateWorkflowRevisionAliasResponse();

        requestAdapter.SendAsync<UpdateWorkflowRevisionAliasResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<UpdateWorkflowRevisionAliasResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(expectedResponse);

        var result = await builder.PutAsync(body);

        await requestAdapter.Received(1).SendAsync<UpdateWorkflowRevisionAliasResponse>(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<UpdateWorkflowRevisionAliasResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());
        Assert.Equal(expectedResponse, result);

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^ワークフローのリビジョンエイリアスの更新を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    state.ToString()!,
                    @"^ワークフローのリビジョンエイリアスの更新が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }
}
