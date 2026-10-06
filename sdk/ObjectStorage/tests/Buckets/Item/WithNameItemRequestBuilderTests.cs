using Microsoft.Kiota.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Buckets.Item;
using Sacloud.Sdk.ObjectStorage.Buckets.Item.Encryption;
using Sacloud.Sdk.ObjectStorage.Buckets.Item.Penalty;
using Sacloud.Sdk.ObjectStorage.Buckets.Item.Plan;
using Sacloud.Sdk.ObjectStorage.Buckets.Item.Quota;
using Sacloud.Sdk.ObjectStorage.Buckets.Item.ReplicableTargets;
using Sacloud.Sdk.ObjectStorage.Buckets.Item.Replication;
using Sacloud.Sdk.ObjectStorage.Buckets.Item.Usage;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.ObjectStorage.Tests.Buckets.Item;

/// <summary>
/// WithNameItemRequestBuilderのテスト
/// </summary>
public class WithNameItemRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<WithNameItemRequestBuilder> _mockLogger;

    public WithNameItemRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<WithNameItemRequestBuilder>>();
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

        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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

        var builder = new WithNameItemRequestBuilder("https://api.example.com/buckets/mybucket", requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// Encryptionプロパティが正しいEncryptionRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Encryption_ReturnsEncryptionRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder.Encryption);
        Assert.IsType<EncryptionRequestBuilder>(builder.Encryption);
    }

    /// <summary>
    /// Penaltyプロパティが正しいPenaltyRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Penalty_ReturnsPenaltyRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder.Penalty);
        Assert.IsType<PenaltyRequestBuilder>(builder.Penalty);
    }

    /// <summary>
    /// Planプロパティが正しいPlanRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Plan_ReturnsPlanRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder.Plan);
        Assert.IsType<PlanRequestBuilder>(builder.Plan);
    }

    /// <summary>
    /// Quotaプロパティが正しいQuotaRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Quota_ReturnsQuotaRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder.Quota);
        Assert.IsType<QuotaRequestBuilder>(builder.Quota);
    }

    /// <summary>
    /// ReplicableTargetsプロパティが正しいReplicableTargetsRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void ReplicableTargets_ReturnsReplicableTargetsRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder.ReplicableTargets);
        Assert.IsType<ReplicableTargetsRequestBuilder>(builder.ReplicableTargets);
    }

    /// <summary>
    /// Replicationプロパティが正しいReplicationRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Replication_ReturnsReplicationRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder.Replication);
        Assert.IsType<ReplicationRequestBuilder>(builder.Replication);
    }

    /// <summary>
    /// Usageプロパティが正しいUsageRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Usage_ReturnsUsageRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder.Usage);
        Assert.IsType<UsageRequestBuilder>(builder.Usage);
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
            { "name", "mybucket" }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
            { "name", "mybucket" }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await builder.DeleteAsync();

        await requestAdapter.Received(1).SendNoContentAsync(
            Arg.Any<RequestInformation>(),
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
            { "name", "mybucket" }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = CreateValidPutBucketReqBody();

        var requestInfo = builder.ToPutRequestInformation(body);

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.PUT, requestInfo.HttpMethod);
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
            { "name", "mybucket" }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(null));
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
            { "name", "mybucket" }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = CreateValidPutBucketReqBody();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ObjectStorage.Models.Handler.CreateBucketResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(new ObjectStorage.Models.Handler.CreateBucketResponse());

        var result = await builder.PutAsync(body);

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ObjectStorage.Models.Handler.CreateBucketResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// WithUrlが正しいWithNameItemRequestBuilderを返すことを確認
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
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var newBuilder = builder.WithUrl("https://api.example.com/buckets/otherbucket");

        Assert.NotNull(newBuilder);
        Assert.IsType<WithNameItemRequestBuilder>(newBuilder);
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
            { "name", "mybucket" }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
            { "name", "mybucket" }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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
            { "name", "mybucket" }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new InvalidOperationException("test exception");
        requestAdapter.SendNoContentAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException(expectedException));

        await Assert.ThrowsAsync<InvalidOperationException>(() => builder.DeleteAsync());
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
            { "name", "mybucket" }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = CreateValidPutBucketReqBody();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ObjectStorage.Models.Handler.CreateBucketResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(new ObjectStorage.Models.Handler.CreateBucketResponse());

        await builder.PutAsync(body);

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ObjectStorage.Models.Handler.CreateBucketResponse>>(),
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
            { "name", "mybucket" }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = CreateValidPutBucketReqBody();
        var expectedException = new ApiError();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ObjectStorage.Models.Handler.CreateBucketResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<ObjectStorage.Models.Handler.CreateBucketResponse?>(expectedException));

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
            { "name", "mybucket" }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = CreateValidPutBucketReqBody();
        var expectedException = new InvalidOperationException("test exception");
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ObjectStorage.Models.Handler.CreateBucketResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<ObjectStorage.Models.Handler.CreateBucketResponse?>(expectedException));

        await Assert.ThrowsAsync<InvalidOperationException>(() => builder.PutAsync(body));
    }

    /// <summary>
    /// DeleteAsyncが成功した場合に開始・完了ログが出力されることを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_Success_LogsInformation()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await builder.DeleteAsync();

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^バケットの削除を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^バケットの削除が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, \d+\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PutAsyncが成功した場合に開始・完了ログが出力されることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_Success_LogsInformation()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = CreateValidPutBucketReqBody();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ObjectStorage.Models.Handler.CreateBucketResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(new ObjectStorage.Models.Handler.CreateBucketResponse());

        await builder.PutAsync(body);

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^バケットの作成を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^バケットの作成が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, \d+\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// DeleteAsyncでApiErrorがスローされた場合にAPIエラーログが出力されることを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_WithApiError_LogsError()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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
            Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^バケット削除のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z .*$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// DeleteAsyncでその他の例外がスローされた場合にエラーログが出力されることを確認
    /// </summary>
    [Fact]
    public async Task DeleteAsync_WithOtherException_LogsError()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
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
            Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^バケットの削除中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PutAsyncでApiErrorがスローされた場合にAPIエラーログが出力されることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithApiError_LogsError()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = CreateValidPutBucketReqBody();
        var expectedException = new ApiError();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ObjectStorage.Models.Handler.CreateBucketResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<ObjectStorage.Models.Handler.CreateBucketResponse?>(expectedException));

        var exception = await Assert.ThrowsAsync<ApiError>(() => builder.PutAsync(body));
        Assert.Same(expectedException, exception);

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^バケット作成のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z .*$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PutAsyncでその他の例外がスローされた場合にエラーログが出力されることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithOtherException_LogsError()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = CreateValidPutBucketReqBody();
        var expectedException = new InvalidOperationException("test exception");
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ObjectStorage.Models.Handler.CreateBucketResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<ObjectStorage.Models.Handler.CreateBucketResponse?>(expectedException));

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => builder.PutAsync(body));
        Assert.Same(expectedException, exception);

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^バケットの作成中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// DeleteAsyncで無効なパスパラメーターを指定した場合にValidationExceptionがスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("　")]
    public async Task DeleteAsync_WithNullOrEmptyOrWhitespacePathParameter_ThrowsValidationException(string? invalidName)
    {
        var pathParameters = new Dictionary<string, object?>
        {
            { "baseurl", "https://api.example.com" },
            { "name", invalidName }
        };
        var expectedMessage = "バケット名を入力してください。";
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.DeleteAsync());

        Assert.Equal(expectedMessage, exception.Message);
        _mockLogger.Received(1).LogWarning(expectedMessage);
    }

    /// <summary>
    /// PutAsyncで無効なパスパラメーターを指定した場合にValidationExceptionがスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("　")]
    public async Task PutAsync_WithNullOrEmptyOrWhitespacePathParameter_ThrowsValidationException(string? invalidName)
    {
        var pathParameters = new Dictionary<string, object?>
        {
            { "baseurl", "https://api.example.com" },
            { "name", invalidName }
        };
        var expectedMessage = "バケット名を入力してください。";
        var requestAdapter = CreateRequestAdapter();
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = CreateValidPutBucketReqBody();

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal(expectedMessage, exception.Message);
        _mockLogger.Received(1).LogWarning(expectedMessage);
    }

    /// <summary>
    /// PutAsyncでClusterIdがnull、空文字、または空白文字の場合にValidationExceptionがスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("　")]
    public async Task PutAsync_WithNullOrEmptyOrWhitespaceClusterId_ThrowsValidationException(string? invalidClusterId)
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ObjectStorage.Models.Handler.CreateBucketRequestBody
        {
            ClusterId = invalidClusterId
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal("ClusterIdが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("ClusterIdが入力されていません。");
    }

    /// <summary>
    /// PutAsyncでPlan.TypeがEnumに含まれない値の場合にValidationExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithInvalidPlanType_ThrowsValidationException()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ObjectStorage.Models.Handler.CreateBucketRequestBody
        {
            ClusterId = "is1a",
            Plan = new ObjectStorage.Models.Handler.CreateBucketRequestBody_plan
            {
                Type = (ObjectStorage.Models.Model.PlanType)999
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal("Plan.TypeはEnumに含まれる値を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("Plan.TypeはEnumに含まれる値を入力してください。");
    }

    /// <summary>
    /// PutAsyncでPlan.TypeがEnumに含まれる値の場合に正常に処理されることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithValidPlanType_Succeeds()
    {
        var requestAdapter = CreateRequestAdapter();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ObjectStorage.Models.Handler.CreateBucketResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(new ObjectStorage.Models.Handler.CreateBucketResponse());

        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new WithNameItemRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ObjectStorage.Models.Handler.CreateBucketRequestBody
        {
            ClusterId = "is1a",
            Plan = new ObjectStorage.Models.Handler.CreateBucketRequestBody_plan
            {
                Type = ObjectStorage.Models.Model.PlanType.Standard
            }
        };

        var result = await builder.PutAsync(body);

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ObjectStorage.Models.Handler.CreateBucketResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());
    }

    private static IRequestAdapter CreateRequestAdapter()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var serializationWriter = Substitute.For<ISerializationWriter>();
        serializationWriter.GetSerializedContent().Returns(new MemoryStream());
        var serializationWriterFactory = Substitute.For<ISerializationWriterFactory>();
        serializationWriterFactory.GetSerializationWriter(Arg.Any<string>()).Returns(serializationWriter);
        requestAdapter.SerializationWriterFactory.Returns(serializationWriterFactory);
        return requestAdapter;
    }

    private static ObjectStorage.Models.Handler.CreateBucketRequestBody CreateValidPutBucketReqBody()
    {
        return new ObjectStorage.Models.Handler.CreateBucketRequestBody
        {
            ClusterId = "is1a"
        };
    }

    private static bool HasApiErrorMapping(Dictionary<string, ParsableFactory<IParsable>> errorMapping)
    {
        return errorMapping.TryGetValue("XXX", out var handler)
            && handler == ApiError.CreateFromDiscriminatorValue;
    }
}
