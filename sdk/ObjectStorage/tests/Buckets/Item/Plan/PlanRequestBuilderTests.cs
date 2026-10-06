using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.ObjectStorage.Buckets.Item.Plan;
using Sacloud.Sdk.ObjectStorage.Models;
using Sacloud.Sdk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.ObjectStorage.Tests.Buckets.Item.Plan;

/// <summary>
/// PlanRequestBuilderのテスト
/// </summary>
public class PlanRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<PlanRequestBuilder> _mockLogger;

    public PlanRequestBuilderTests()
    {
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLogger = Substitute.For<ILogger<PlanRequestBuilder>>();
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

        var builder = new PlanRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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

        var builder = new PlanRequestBuilder("https://api.example.com/buckets/mybucket/plan", requestAdapter, _mockLoggerFactory);

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
        var builder = new PlanRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
        var builder = new PlanRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ReadBucketPlanResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(new ReadBucketPlanResponse());

        var result = await builder.GetAsync();

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ReadBucketPlanResponse>>(),
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
        var builder = new PlanRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ReadBucketPlanResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(new ReadBucketPlanResponse());

        var result = await builder.GetAsync();

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ReadBucketPlanResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// PutAsyncがRequestAdapterを呼び出すことを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_CallsRequestAdapter()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new PlanRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PlanChangeReqBody
        {
            NewPlan = new PlanChangeReqBody_new_plan
            {
                Type = ObjectStorage.Models.Model.PlanType.Standard,
                ServiceClassPath = "is1a/is1b"
            },
            PreviousContract = new PlanChangeReqBody_previous_contract
            {
                ResourceId = "123"
            }
        };
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<UpdateBucketPlanResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(new UpdateBucketPlanResponse());

        var result = await builder.PutAsync(body);

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<UpdateBucketPlanResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// PutAsyncでハイフンを含むバケット名が受け入れられることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithHyphenatedBucketName_CallsRequestAdapter()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "myb-ucket" }
        };
        var builder = new PlanRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PlanChangeReqBody
        {
            NewPlan = new PlanChangeReqBody_new_plan
            {
                Type = ObjectStorage.Models.Model.PlanType.Standard,
                ServiceClassPath = "is1a/is1b"
            },
            PreviousContract = new PlanChangeReqBody_previous_contract
            {
                ResourceId = "123"
            }
        };
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<UpdateBucketPlanResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(new UpdateBucketPlanResponse());

        var result = await builder.PutAsync(body);

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<UpdateBucketPlanResponse>>(),
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
        var builder = new PlanRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PlanChangeReqBody();

        var requestInfo = builder.ToPutRequestInformation(body);

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.PUT, requestInfo.HttpMethod);
    }

    /// <summary>
    /// WithUrlが正しいPlanRequestBuilderを返すことを確認
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
        var builder = new PlanRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var newBuilder = builder.WithUrl("https://api.example.com/buckets/otherbucket/plan");

        Assert.NotNull(newBuilder);
        Assert.IsType<PlanRequestBuilder>(newBuilder);
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
        var builder = new PlanRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ReadBucketPlanResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(new ReadBucketPlanResponse());

        await builder.GetAsync();

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ReadBucketPlanResponse>>(),
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
        var builder = new PlanRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ReadBucketPlanResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<ReadBucketPlanResponse?>(expectedException));

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
        var builder = new PlanRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new InvalidOperationException("test exception");
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ReadBucketPlanResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<ReadBucketPlanResponse?>(expectedException));

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
            { "name", "mybucket" }
        };
        var builder = new PlanRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PlanChangeReqBody
        {
            NewPlan = new PlanChangeReqBody_new_plan
            {
                Type = ObjectStorage.Models.Model.PlanType.Standard,
                ServiceClassPath = "is1a/is1b"
            },
            PreviousContract = new PlanChangeReqBody_previous_contract
            {
                ResourceId = "123"
            }
        };
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<UpdateBucketPlanResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(new UpdateBucketPlanResponse());

        await builder.PutAsync(body);

        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<UpdateBucketPlanResponse>>(),
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
        var builder = new PlanRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PlanChangeReqBody
        {
            NewPlan = new PlanChangeReqBody_new_plan
            {
                Type = ObjectStorage.Models.Model.PlanType.Standard,
                ServiceClassPath = "is1a/is1b"
            },
            PreviousContract = new PlanChangeReqBody_previous_contract
            {
                ResourceId = "123"
            }
        };
        var expectedException = new ApiError();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<UpdateBucketPlanResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<UpdateBucketPlanResponse?>(expectedException));

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
        var builder = new PlanRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PlanChangeReqBody
        {
            NewPlan = new PlanChangeReqBody_new_plan
            {
                Type = ObjectStorage.Models.Model.PlanType.Standard,
                ServiceClassPath = "is1a/is1b"
            },
            PreviousContract = new PlanChangeReqBody_previous_contract
            {
                ResourceId = "123"
            }
        };
        var expectedException = new InvalidOperationException("test exception");
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<UpdateBucketPlanResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<UpdateBucketPlanResponse?>(expectedException));

        await Assert.ThrowsAsync<InvalidOperationException>(() => builder.PutAsync(body));
    }

    /// <summary>
    /// PutAsyncのリクエストボディのnew_planがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithNullNewPlan_ThrowsValidationException()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new PlanRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PlanChangeReqBody
        {
            NewPlan = null,
            PreviousContract = new PlanChangeReqBody_previous_contract { ResourceId = "123" }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal("NewPlanが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("NewPlanが入力されていません。");
    }

    /// <summary>
    /// PutAsyncのリクエストボディのnew_plan.service_class_pathがnull、空文字、または空白文字の場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("　")]
    public async Task PutAsync_WithNullOrEmptyOrWhitespaceNewPlanServiceClassPath_ThrowsValidationException(string? invalidServiceClassPath)
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new PlanRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PlanChangeReqBody
        {
            NewPlan = new PlanChangeReqBody_new_plan
            {
                Type = ObjectStorage.Models.Model.PlanType.Standard,
                ServiceClassPath = invalidServiceClassPath
            },
            PreviousContract = new PlanChangeReqBody_previous_contract { ResourceId = "123" }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal("NewPlan.ServiceClassPathが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("NewPlan.ServiceClassPathが入力されていません。");
    }

    /// <summary>
    /// PutAsyncのリクエストボディのnew_plan.typeがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithNullNewPlanType_ThrowsValidationException()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new PlanRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PlanChangeReqBody
        {
            NewPlan = new PlanChangeReqBody_new_plan
            {
                Type = null,
                ServiceClassPath = "is1a/is1b"
            },
            PreviousContract = new PlanChangeReqBody_previous_contract { ResourceId = "123" }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal("NewPlan.Typeが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("NewPlan.Typeが入力されていません。");
    }

    /// <summary>
    /// PutAsyncのリクエストボディのnew_plan.typeがEnumに含まれない値の場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithInvalidNewPlanType_ThrowsValidationException()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new PlanRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PlanChangeReqBody
        {
            NewPlan = new PlanChangeReqBody_new_plan
            {
                Type = (ObjectStorage.Models.Model.PlanType)999,
                ServiceClassPath = "is1a/is1b"
            },
            PreviousContract = new PlanChangeReqBody_previous_contract { ResourceId = "123" }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal("NewPlan.TypeはEnumに含まれる値を入力してください。", exception.Message);
        _mockLogger.Received(1).LogWarning("NewPlan.TypeはEnumに含まれる値を入力してください。");
    }
    
    /// <summary>
    /// PutAsyncのリクエストボディのprevious_contractがnullの場合に例外がスローされることを確認
    /// </summary>
    [Fact]
    public async Task PutAsync_WithNullPreviousContract_ThrowsValidationException()
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new PlanRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PlanChangeReqBody
        {
            NewPlan = new PlanChangeReqBody_new_plan
            {
                Type = ObjectStorage.Models.Model.PlanType.Standard,
                ServiceClassPath = "is1a/is1b"
            },
            PreviousContract = null
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal("PreviousContractが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("PreviousContractが入力されていません。");
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
        var builder = new PlanRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(null));
    }
    
    /// <summary>
    /// PutAsyncのリクエストボディのprevious_contract.resource_idが不正な場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("abc")]
    [InlineData("1234567890123")]
    public async Task PutAsync_WithInvalidPreviousContractResourceId_ThrowsValidationException(string invalidResourceId)
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new PlanRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PlanChangeReqBody
        {
            NewPlan = new PlanChangeReqBody_new_plan
            {
                Type = ObjectStorage.Models.Model.PlanType.Standard,
                ServiceClassPath = "is1a/is1b"
            },
            PreviousContract = new PlanChangeReqBody_previous_contract { ResourceId = invalidResourceId }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal("PreviousContract.ResourceIdの形式が正しくありません。", exception.Message);
        _mockLogger.Received(1).LogWarning("PreviousContract.ResourceIdの形式が正しくありません。");
    }

    /// <summary>
    /// PutAsyncのリクエストボディのprevious_contract.resource_idがnull、空文字、または空白文字の場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("　")]
    public async Task PutAsync_WithNullOrEmptyOrWhitespacePreviousContractResourceId_ThrowsValidationException(string? invalidResourceId)
    {
        var requestAdapter = CreateRequestAdapter();
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new PlanRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PlanChangeReqBody
        {
            NewPlan = new PlanChangeReqBody_new_plan
            {
                Type = ObjectStorage.Models.Model.PlanType.Standard,
                ServiceClassPath = "is1a/is1b"
            },
            PreviousContract = new PlanChangeReqBody_previous_contract { ResourceId = invalidResourceId }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal("PreviousContract.ResourceIdが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("PreviousContract.ResourceIdが入力されていません。");
    }

    /// <summary>
    /// PutAsyncでprevious_contract.resource_idが1桁または12桁の場合に正常に処理されることを確認
    /// </summary>
    [Theory]
    [InlineData("1")]
    [InlineData("123456789012")]
    public async Task PutAsync_WithBoundaryPreviousContractResourceId_Succeeds(string resourceId)
    {
        var requestAdapter = CreateRequestAdapter();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<UpdateBucketPlanResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(new UpdateBucketPlanResponse());
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new PlanRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PlanChangeReqBody
        {
            NewPlan = new PlanChangeReqBody_new_plan
            {
                Type = ObjectStorage.Models.Model.PlanType.Standard,
                ServiceClassPath = "is1a/is1b"
            },
            PreviousContract = new PlanChangeReqBody_previous_contract { ResourceId = resourceId }
        };

        var result = await builder.PutAsync(body);

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<UpdateBucketPlanResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// GetAsyncが成功した場合に開始・完了ログが出力されることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_Success_LogsInformation()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new PlanRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ReadBucketPlanResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(new ReadBucketPlanResponse());

        await builder.GetAsync();

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^バケットプランの取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^バケットプランの取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, \d+\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncでApiErrorがスローされた場合にエラーログが出力されることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithApiError_LogsError()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new PlanRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ReadBucketPlanResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<ReadBucketPlanResponse?>(expectedException));

        await Assert.ThrowsAsync<ApiError>(() => builder.GetAsync());

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^バケットプラン取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z .*$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncでその他例外がスローされた場合にエラーログが出力されることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_WithOtherException_LogsError()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", "mybucket" }
        };
        var builder = new PlanRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new InvalidOperationException("test exception");
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ReadBucketPlanResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<ReadBucketPlanResponse?>(expectedException));

        await Assert.ThrowsAsync<InvalidOperationException>(() => builder.GetAsync());

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => System.Text.RegularExpressions.Regex.IsMatch(state.ToString()!, @"^バケットプランの取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncでバケット名が許容される文字数の範囲外の場合にValidationExceptionがスローされることを確認
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
        var builder = new PlanRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

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
            Arg.Any<ParsableFactory<ReadBucketPlanResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(new ReadBucketPlanResponse());
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", new string('a', nameLength) }
        };
        var builder = new PlanRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var result = await builder.GetAsync();

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ReadBucketPlanResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// GetAsyncのリクエストパスパラメータがnull、空文字、または空白文字の場合にValidationExceptionがスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("　")]
    [InlineData(null)]
    public async Task GetAsync_WithNullPathParameters_ThrowsValidationException(string? invalidName)
    {
        var pathParameters = new Dictionary<string, object?>
        {
            { "baseurl", "https://api.example.com" },
            { "name", invalidName }
        };
        var expectedMessage = "バケット名を入力してください。";
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new PlanRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync());

        Assert.Equal(expectedMessage, exception.Message);
        _mockLogger.Received(1).LogWarning(expectedMessage);
    }

    /// <summary>
    /// GetAsyncのリクエストパスパラメータのnameが不正な場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("-ab")]
    [InlineData("Ａbc")]
    [InlineData("あab")]
    [InlineData(" ab")]
    [InlineData("　ab")]
    public async Task GetAsync_WithInvalidPathParameters_ThrowsValidationException(string invalidName)
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", invalidName }
        };
        var expectedMessage = "バケット名の形式が正しくありません。";
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new PlanRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.GetAsync());

        Assert.Equal(expectedMessage, exception.Message);
        _mockLogger.Received(1).LogWarning(expectedMessage);
    }


    /// <summary>
    /// PutAsyncのリクエストパスパラメータがnull、空文字、または空白文字の場合にValidationExceptionがスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("　")]
    [InlineData(null)]
    public async Task PutAsync_WithNullPathParameters_ThrowsValidationException(string? invalidName)
    {
        var pathParameters = new Dictionary<string, object?>
        {
            { "baseurl", "https://api.example.com" },
            { "name", invalidName }
        };
        var expectedMessage = "バケット名を入力してください。";
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new PlanRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PlanChangeReqBody
        {
            NewPlan = new PlanChangeReqBody_new_plan
            {
                Type = ObjectStorage.Models.Model.PlanType.Standard,
                ServiceClassPath = "is1a/is1b"
            },
            PreviousContract = new PlanChangeReqBody_previous_contract
            {
                ResourceId = "123"
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal(expectedMessage, exception.Message);
        _mockLogger.Received(1).LogWarning(expectedMessage);
    }

    /// <summary>
    /// PutAsyncのリクエストパスパラメータのnameが不正な場合に例外がスローされることを確認
    /// </summary>
    [Theory]
    [InlineData("-ab")]
    [InlineData("Ａbc")]
    [InlineData("あab")]
    [InlineData(" ab")]
    [InlineData("　ab")]
    public async Task PutAsync_WithInvalidPathParameters_ThrowsValidationException(string invalidName)
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", invalidName }
        };
        var expectedMessage = "バケット名の形式が正しくありません。";
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new PlanRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PlanChangeReqBody
        {
            NewPlan = new PlanChangeReqBody_new_plan
            {
                Type = ObjectStorage.Models.Model.PlanType.Standard,
                ServiceClassPath = "is1a/is1b"
            },
            PreviousContract = new PlanChangeReqBody_previous_contract
            {
                ResourceId = "123"
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal(expectedMessage, exception.Message);
        _mockLogger.Received(1).LogWarning(expectedMessage);
    }

    /// <summary>
    /// PutAsyncでバケット名が許容される文字数の範囲外の場合にValidationExceptionがスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(2, "バケット名は3文字以上で入力してください。")]
    [InlineData(65, "バケット名は64文字以下で入力してください。")]
    public async Task PutAsync_WithBucketNameOutsideLengthRange_ThrowsValidationException(int nameLength, string expectedMessage)
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", new string('a', nameLength) }
        };
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var builder = new PlanRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new PlanChangeReqBody
        {
            NewPlan = new PlanChangeReqBody_new_plan
            {
                Type = ObjectStorage.Models.Model.PlanType.Standard,
                ServiceClassPath = "is1a/is1b"
            },
            PreviousContract = new PlanChangeReqBody_previous_contract
            {
                ResourceId = "123"
            }
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PutAsync(body));

        Assert.Equal(expectedMessage, exception.Message);
        _mockLogger.Received(1).LogWarning(expectedMessage);
    }

    /// <summary>
    /// PutAsyncで3文字または64文字のバケット名が受け入れられることを確認
    /// </summary>
    [Theory]
    [InlineData(3)]
    [InlineData(64)]
    public async Task PutAsync_WithBoundaryLengthBucketName_CallsRequestAdapter(int nameLength)
    {
        var requestAdapter = CreateRequestAdapter();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<UpdateBucketPlanResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()).Returns(new UpdateBucketPlanResponse());
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" },
            { "name", new string('a', nameLength) }
        };
        var builder = new PlanRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var result = await builder.PutAsync(new PlanChangeReqBody
        {
            NewPlan = new PlanChangeReqBody_new_plan
            {
                Type = ObjectStorage.Models.Model.PlanType.Standard,
                ServiceClassPath = "is1a/is1b"
            },
            PreviousContract = new PlanChangeReqBody_previous_contract
            {
                ResourceId = "123"
            }
        });

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<UpdateBucketPlanResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>());
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
