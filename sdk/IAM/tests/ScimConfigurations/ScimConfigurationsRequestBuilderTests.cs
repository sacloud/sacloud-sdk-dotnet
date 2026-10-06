using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sacloud.Sdk.IAM.Models;
using Sacloud.Sdk.IAM.ScimConfigurations;
using Sacloud.Sdk.IAM.ScimConfigurations.Item;
using Sacloud.Sdk.Shared.Models;

namespace Sacloud.Sdk.IAM.Tests.ScimConfigurations;

/// <summary>
/// ScimConfigurationsRequestBuilderのテスト
/// </summary>
public class ScimConfigurationsRequestBuilderTests
{
    private readonly ILoggerFactory _mockLoggerFactory;
    private readonly ILogger<ScimConfigurationsRequestBuilder> _mockLogger;

    public ScimConfigurationsRequestBuilderTests()
    {
        _mockLogger = Substitute.For<ILogger<ScimConfigurationsRequestBuilder>>();
        _mockLoggerFactory = Substitute.For<ILoggerFactory>();
        _mockLoggerFactory.CreateLogger<ScimConfigurationsRequestBuilder>().Returns(_mockLogger);
    }
    /// <summary>
    /// コンストラクタ（PathParameters版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void コンストラクタ_PathParameters版で正常に初期化される()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };

        var builder = new ScimConfigurationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）が正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void コンストラクタ_rawUrl版で正常に初期化される()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var rawUrl = "https://api.example.com/scim-configurations";

        var builder = new ScimConfigurationsRequestBuilder(rawUrl, requestAdapter, _mockLoggerFactory);

        Assert.NotNull(builder);
    }

    /// <summary>
    /// インデクサーが正しいScimConfigurationsItemRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void インデクサー_ScimConfigurationsItemRequestBuilderを返す()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ScimConfigurationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var itemBuilder = builder[Guid.NewGuid()];

        Assert.NotNull(itemBuilder);
        Assert.IsType<ScimConfigurationsItemRequestBuilder>(itemBuilder);
    }

    /// <summary>
    /// インデクサーが異なるIDで異なるインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void インデクサー_異なるIDで異なるインスタンスを返す()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ScimConfigurationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var itemBuilder1 = builder[Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")];
        var itemBuilder2 = builder[Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb")];

        Assert.NotNull(itemBuilder1);
        Assert.NotNull(itemBuilder2);
        Assert.NotSame(itemBuilder1, itemBuilder2);
    }

    /// <summary>
    /// ToGetRequestInformationがGETメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_GETメソッドのRequestInformationを返す()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ScimConfigurationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.GET, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToGetRequestInformationが正しいURLテンプレートを設定することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_URLテンプレートにscim_configurationsを含む()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ScimConfigurationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.Contains("scim-configurations", requestInfo.UrlTemplate);
    }

    /// <summary>
    /// ToGetRequestInformationがAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToGetRequestInformation_Acceptヘッダーを設定する()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ScimConfigurationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation();

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
    }

    /// <summary>
    /// ToGetRequestInformationがクエリパラメータを設定することを確認
    /// </summary>
    [Theory]
    [InlineData(1, 10)]
    [InlineData(2, 20)]
    public void ToGetRequestInformation_クエリパラメータを設定できる(int page, int perPage)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ScimConfigurationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var requestInfo = builder.ToGetRequestInformation(config =>
        {
            config.QueryParameters.Page = page;
            config.QueryParameters.PerPage = perPage;
        });

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.GET, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToPostRequestInformationがPOSTメソッドで正しいRequestInformationを作成することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_POSTメソッドのRequestInformationを返す()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var serializationWriterFactory = Substitute.For<ISerializationWriterFactory>();
        var serializationWriter = Substitute.For<ISerializationWriter>();
        serializationWriterFactory.GetSerializationWriter(Arg.Any<string>()).Returns(serializationWriter);
        serializationWriter.GetSerializedContent().Returns(new MemoryStream());
        requestAdapter.SerializationWriterFactory.Returns(serializationWriterFactory);
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ScimConfigurationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ScimConfigurationsPostRequestBody { Name = "Test SCIM Configuration" };

        var requestInfo = builder.ToPostRequestInformation(body);

        Assert.NotNull(requestInfo);
        Assert.Equal(Method.POST, requestInfo.HttpMethod);
    }

    /// <summary>
    /// ToPostRequestInformationが正しいURLテンプレートを設定することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_URLテンプレートにscim_configurationsを含む()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var serializationWriterFactory = Substitute.For<ISerializationWriterFactory>();
        var serializationWriter = Substitute.For<ISerializationWriter>();
        serializationWriterFactory.GetSerializationWriter(Arg.Any<string>()).Returns(serializationWriter);
        serializationWriter.GetSerializedContent().Returns(new MemoryStream());
        requestAdapter.SerializationWriterFactory.Returns(serializationWriterFactory);
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ScimConfigurationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ScimConfigurationsPostRequestBody { Name = "Test SCIM Configuration" };

        var requestInfo = builder.ToPostRequestInformation(body);

        Assert.Contains("scim-configurations", requestInfo.UrlTemplate);
    }

    /// <summary>
    /// ToPostRequestInformationがAcceptヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_Acceptヘッダーを設定する()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var serializationWriterFactory = Substitute.For<ISerializationWriterFactory>();
        var serializationWriter = Substitute.For<ISerializationWriter>();
        serializationWriterFactory.GetSerializationWriter(Arg.Any<string>()).Returns(serializationWriter);
        serializationWriter.GetSerializedContent().Returns(new MemoryStream());
        requestAdapter.SerializationWriterFactory.Returns(serializationWriterFactory);
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ScimConfigurationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ScimConfigurationsPostRequestBody { Name = "Test SCIM Configuration" };

        var requestInfo = builder.ToPostRequestInformation(body);

        Assert.True(requestInfo.Headers.ContainsKey("Accept"));
    }

    /// <summary>
    /// ToPostRequestInformationがContent-Typeヘッダーを設定することを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_ContentTypeヘッダーを設定する()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var serializationWriterFactory = Substitute.For<ISerializationWriterFactory>();
        var serializationWriter = Substitute.For<ISerializationWriter>();
        serializationWriterFactory.GetSerializationWriter(Arg.Any<string>()).Returns(serializationWriter);
        serializationWriter.GetSerializedContent().Returns(new MemoryStream());
        requestAdapter.SerializationWriterFactory.Returns(serializationWriterFactory);
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ScimConfigurationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ScimConfigurationsPostRequestBody { Name = "Test SCIM Configuration" };

        var requestInfo = builder.ToPostRequestInformation(body);

        Assert.True(requestInfo.Headers.ContainsKey("Content-Type"));
    }

    /// <summary>
    /// GetAsyncがrequestConfigurationを使用することを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_requestConfigurationを使用する()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ScimConfigurationsGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(new ScimConfigurationsGetResponse());
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ScimConfigurationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var result = await builder.GetAsync(config =>
        {
            config.QueryParameters.Page = 1;
            config.QueryParameters.PerPage = 10;
        });

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ScimConfigurationsGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// GetAsyncがCancellationTokenを使用することを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_CancellationTokenを使用する()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var cancellationToken = new CancellationToken();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ScimConfigurationsGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            cancellationToken
        ).Returns(new ScimConfigurationsGetResponse());
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ScimConfigurationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var result = await builder.GetAsync(cancellationToken: cancellationToken);

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ScimConfigurationsGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            cancellationToken
        );
    }

    /// <summary>
    /// GetAsyncがnull戻り値を処理できることを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_nullのレスポンスでnullを返す()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ScimConfigurationsGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns((ScimConfigurationsGetResponse?)null);
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ScimConfigurationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var result = await builder.GetAsync();

        Assert.Null(result);
    }

    /// <summary>
    /// PostAsyncがrequestConfigurationを使用することを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_requestConfigurationを使用する()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var serializationWriterFactory = Substitute.For<ISerializationWriterFactory>();
        var serializationWriter = Substitute.For<ISerializationWriter>();
        serializationWriterFactory.GetSerializationWriter(Arg.Any<string>()).Returns(serializationWriter);
        serializationWriter.GetSerializedContent().Returns(new MemoryStream());
        requestAdapter.SerializationWriterFactory.Returns(serializationWriterFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ScimConfiguration>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(new ScimConfiguration());
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ScimConfigurationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ScimConfigurationsPostRequestBody { Name = "Test SCIM Configuration" };

        var result = await builder.PostAsync(body, config => { });

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ScimConfiguration>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// PostAsyncがCancellationTokenを使用することを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_CancellationTokenを使用する()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var serializationWriterFactory = Substitute.For<ISerializationWriterFactory>();
        var serializationWriter = Substitute.For<ISerializationWriter>();
        serializationWriterFactory.GetSerializationWriter(Arg.Any<string>()).Returns(serializationWriter);
        serializationWriter.GetSerializedContent().Returns(new MemoryStream());
        requestAdapter.SerializationWriterFactory.Returns(serializationWriterFactory);
        var cancellationToken = new CancellationToken();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ScimConfiguration>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            cancellationToken
        ).Returns(new ScimConfiguration());
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ScimConfigurationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ScimConfigurationsPostRequestBody { Name = "Test SCIM Configuration" };

        var result = await builder.PostAsync(body, cancellationToken: cancellationToken);

        Assert.NotNull(result);
        await requestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ScimConfiguration>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            cancellationToken
        );
    }

    /// <summary>
    /// PostAsyncがnull戻り値を処理できることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_nullのレスポンスでnullを返す()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var serializationWriterFactory = Substitute.For<ISerializationWriterFactory>();
        var serializationWriter = Substitute.For<ISerializationWriter>();
        serializationWriterFactory.GetSerializationWriter(Arg.Any<string>()).Returns(serializationWriter);
        serializationWriter.GetSerializedContent().Returns(new MemoryStream());
        requestAdapter.SerializationWriterFactory.Returns(serializationWriterFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ScimConfiguration>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns((ScimConfiguration?)null);
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ScimConfigurationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ScimConfigurationsPostRequestBody { Name = "Test SCIM Configuration" };

        var result = await builder.PostAsync(body);

        Assert.Null(result);
    }

    /// <summary>
    /// ToPostRequestInformationにnullのbodyを渡すとArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void ToPostRequestInformation_nullのbodyでArgumentNullExceptionをスロー()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ScimConfigurationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        Assert.Throws<ArgumentNullException>(() => builder.ToPostRequestInformation(null));
    }

    /// <summary>
    /// PostAsyncにnullのbodyを渡すとValidationExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_nullのbodyでValidationExceptionをスロー()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ScimConfigurationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await Assert.ThrowsAsync<ValidationException>(async () => await builder.PostAsync(null));
    }

    /// <summary>
    /// WithUrlが新しいScimConfigurationsRequestBuilderのインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void WithUrl_正しいインスタンスを返す()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ScimConfigurationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        var newBuilder = builder.WithUrl("https://api.example.com/scim-configurations");

        Assert.NotNull(newBuilder);
        Assert.IsType<ScimConfigurationsRequestBuilder>(newBuilder);
        Assert.NotSame(builder, newBuilder);
    }

    /// <summary>
    /// QueryParametersのPageプロパティが設定できることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_Pageが設定できる()
    {
        var queryParams = new ScimConfigurationsRequestBuilder.ScimConfigurationsRequestBuilderGetQueryParameters
        {
            Page = 1
        };

        Assert.Equal(1, queryParams.Page);
    }

    /// <summary>
    /// QueryParametersのPerPageプロパティが設定できることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_PerPageが設定できる()
    {
        var queryParams = new ScimConfigurationsRequestBuilder.ScimConfigurationsRequestBuilderGetQueryParameters
        {
            PerPage = 20
        };

        Assert.Equal(20, queryParams.PerPage);
    }

    /// <summary>
    /// QueryParametersのPageプロパティにNullを設定できることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_PageがNullを設定できる()
    {
        var queryParams = new ScimConfigurationsRequestBuilder.ScimConfigurationsRequestBuilderGetQueryParameters
        {
            Page = null
        };

        Assert.Null(queryParams.Page);
    }

    /// <summary>
    /// QueryParametersのPerPageプロパティにNullを設定できることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_PerPageがNullを設定できる()
    {
        var queryParams = new ScimConfigurationsRequestBuilder.ScimConfigurationsRequestBuilderGetQueryParameters
        {
            PerPage = null
        };

        Assert.Null(queryParams.PerPage);
    }

    /// <summary>
    /// QueryParametersのPageプロパティがQueryParameter属性を持つことを確認
    /// </summary>
    [Fact]
    public void QueryParameters_PageがQueryParameter属性を持つ()
    {
        var property = typeof(ScimConfigurationsRequestBuilder.ScimConfigurationsRequestBuilderGetQueryParameters)
            .GetProperty(nameof(ScimConfigurationsRequestBuilder.ScimConfigurationsRequestBuilderGetQueryParameters.Page));
        var attribute = property?.GetCustomAttributes(typeof(QueryParameterAttribute), false).FirstOrDefault();

        Assert.NotNull(attribute);
    }

    /// <summary>
    /// QueryParametersのPerPageプロパティがQueryParameter属性を持つことを確認
    /// </summary>
    [Fact]
    public void QueryParameters_PerPageがQueryParameter属性を持つ()
    {
        var property = typeof(ScimConfigurationsRequestBuilder.ScimConfigurationsRequestBuilderGetQueryParameters)
            .GetProperty(nameof(ScimConfigurationsRequestBuilder.ScimConfigurationsRequestBuilderGetQueryParameters.PerPage));
        var attribute = property?.GetCustomAttributes(typeof(QueryParameterAttribute), false).FirstOrDefault();

        Assert.NotNull(attribute);
    }

    /// <summary>
    /// QueryParametersの全プロパティが設定できることを確認
    /// </summary>
    [Fact]
    public void QueryParameters_全プロパティが設定できる()
    {
        var queryParams = new ScimConfigurationsRequestBuilder.ScimConfigurationsRequestBuilderGetQueryParameters
        {
            Page = 2,
            PerPage = 50
        };

        Assert.Equal(2, queryParams.Page);
        Assert.Equal(50, queryParams.PerPage);
    }

    /// <summary>
    /// コンストラクタ（PathParameters版）にnullのLoggerFactoryを渡すとArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void コンストラクタ_PathParameters版でnullのLoggerFactoryはArgumentNullExceptionをスロー()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };

        Assert.Throws<ArgumentNullException>(() =>
            new ScimConfigurationsRequestBuilder(pathParameters, requestAdapter, null));
    }

    /// <summary>
    /// コンストラクタ（rawUrl版）にnullのLoggerFactoryを渡すとArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void コンストラクタ_rawUrl版でnullのLoggerFactoryはArgumentNullExceptionをスロー()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var rawUrl = "https://api.example.com/scim-configurations";

        Assert.Throws<ArgumentNullException>(() =>
            new ScimConfigurationsRequestBuilder(rawUrl, requestAdapter, null));
    }

    /// <summary>
    /// GetAsyncが開始ログと完了ログを出力することを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_開始ログと完了ログを出力する()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var expectedResponse = new ScimConfigurationsGetResponse();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ScimConfigurationsGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ScimConfigurationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);

        await builder.GetAsync();

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^ユーザープロビジョニング一覧の取得を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^ユーザープロビジョニング一覧の取得が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 200\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncでAPIエラーが発生した場合にエラーログを出力することを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_APIエラー時にエラーログを出力する()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ScimConfigurationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var expectedException = new ApiError { ResponseStatusCode = 401 };
        ((IAdditionalDataHolder)expectedException).AdditionalData = new Dictionary<string, object>
        {
            { "error_code", "unauthorized" },
            { "error_msg", "error-unauthorized" },
            { "is_fatal", true },
            { "serial", "1234567890" },
            { "status", "401 Unauthorized" }
        };
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ScimConfigurationsGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(() => builder.GetAsync());

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^ユーザープロビジョニング一覧取得のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 401\], \[error_code, unauthorized\], \[error_msg, error\-unauthorized\], \[is_fatal, True\], \[serial, 1234567890\], \[status, 401 Unauthorized\]$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// GetAsyncでその他のエラーが発生した場合にエラーログを出力することを確認
    /// </summary>
    [Fact]
    public async Task GetAsync_その他エラー時にエラーログを出力する()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ScimConfigurationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ScimConfigurationsGetResponse>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(new Exception());

        await Assert.ThrowsAsync<Exception>(() => builder.GetAsync());

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^ユーザープロビジョニング一覧の取得中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncが開始ログと完了ログを出力することを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_開始ログと完了ログを出力する()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var serializationWriterFactory = Substitute.For<ISerializationWriterFactory>();
        var serializationWriter = Substitute.For<ISerializationWriter>();
        serializationWriterFactory.GetSerializationWriter(Arg.Any<string>()).Returns(serializationWriter);
        serializationWriter.GetSerializedContent().Returns(new MemoryStream());
        requestAdapter.SerializationWriterFactory.Returns(serializationWriterFactory);
        var expectedResponse = new ScimConfiguration();
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ScimConfiguration>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedResponse);
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ScimConfigurationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ScimConfigurationsPostRequestBody { Name = "Test SCIM Configuration" };

        await builder.PostAsync(body);

        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^ユーザープロビジョニングの作成を開始します。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        _mockLogger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^ユーザープロビジョニングの作成が完了しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 201\]$")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncでAPIエラーが発生した場合にエラーログを出力することを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_APIエラー時にエラーログを出力する()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var serializationWriterFactory = Substitute.For<ISerializationWriterFactory>();
        var serializationWriter = Substitute.For<ISerializationWriter>();
        serializationWriterFactory.GetSerializationWriter(Arg.Any<string>()).Returns(serializationWriter);
        serializationWriter.GetSerializedContent().Returns(new MemoryStream());
        requestAdapter.SerializationWriterFactory.Returns(serializationWriterFactory);
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ScimConfigurationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ScimConfigurationsPostRequestBody { Name = "Test SCIM Configuration" };
        var expectedException = new ApiError { ResponseStatusCode = 409 };
        ((IAdditionalDataHolder)expectedException).AdditionalData = new Dictionary<string, object>
        {
            { "error_code", "conflict" },
            { "error_msg", "error-conflict" },
            { "is_fatal", false },
            { "serial", "1234567890" },
            { "status", "409 Conflict" }
        };
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ScimConfiguration>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(expectedException);

        await Assert.ThrowsAsync<ApiError>(() => builder.PostAsync(body));

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^ユーザープロビジョニング作成のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z \[StatusCode, 409\], \[error_code, conflict\], \[error_msg, error\-conflict\], \[is_fatal, False\], \[serial, 1234567890\], \[status, 409 Conflict\]$")),
            Arg.Any<ApiException>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncでその他のエラーが発生した場合にエラーログを出力することを確認
    /// </summary>
    [Fact]
    public async Task PostAsync_その他エラー時にエラーログを出力する()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var serializationWriterFactory = Substitute.For<ISerializationWriterFactory>();
        var serializationWriter = Substitute.For<ISerializationWriter>();
        serializationWriterFactory.GetSerializationWriter(Arg.Any<string>()).Returns(serializationWriter);
        serializationWriter.GetSerializedContent().Returns(new MemoryStream());
        requestAdapter.SerializationWriterFactory.Returns(serializationWriterFactory);
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ScimConfigurationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ScimConfigurationsPostRequestBody { Name = "Test SCIM Configuration" };
        requestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ScimConfiguration>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>()
        ).ThrowsAsync(new Exception());

        await Assert.ThrowsAsync<Exception>(() => builder.PostAsync(body));

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => Regex.IsMatch(state.ToString(),
                @"^ユーザープロビジョニングの作成中にエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// PostAsyncにNameが無効なbodyを渡すとValidationExceptionがスローされることを確認
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task PostAsync_無効なNameでValidationExceptionをスロー(string? name)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var pathParameters = new Dictionary<string, object>
        {
            { "baseurl", "https://api.example.com" }
        };
        var builder = new ScimConfigurationsRequestBuilder(pathParameters, requestAdapter, _mockLoggerFactory);
        var body = new ScimConfigurationsPostRequestBody { Name = name };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => builder.PostAsync(body));

        Assert.Equal("nameが入力されていません。", exception.Message);
        _mockLogger.Received(1).LogWarning("nameが入力されていません。");
    }
}


