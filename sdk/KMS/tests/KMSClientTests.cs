using System.Collections.Generic;
using Microsoft.Kiota.Abstractions;
using NSubstitute;
using Sacloud.Sdk.KMS.Kms;

namespace Sacloud.Sdk.KMS.Tests;

/// <summary>
/// KMSClientのテスト
/// </summary>
public class KMSClientTests
{
    /// <summary>
    /// コンストラクタが正常に初期化されることを確認
    /// </summary>
    [Fact]
    public void Constructor_InitializesSuccessfully()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();

        var client = new KMSClient(requestAdapter);

        Assert.NotNull(client);
    }

    /// <summary>
    /// コンストラクタがBaseUrlを設定することを確認
    /// </summary>
    [Fact]
    public void Constructor_SetsDefaultBaseUrl_WhenBaseUrlIsEmpty()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = string.Empty;

        _ = new KMSClient(requestAdapter);

        Assert.Equal("https://secure.sakura.ad.jp/cloud/zone/is1a/api/cloud/1.1", requestAdapter.BaseUrl);
    }

    /// <summary>
    /// コンストラクタが既存のBaseUrlを上書きしないことを確認
    /// </summary>
    [Fact]
    public void Constructor_DoesNotOverrideBaseUrl_WhenAlreadySet()
    {
        var existingBaseUrl = "https://api.example.com";
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = existingBaseUrl;

        var client = new TestKMSClient(requestAdapter);

        Assert.Equal(existingBaseUrl, requestAdapter.BaseUrl);
        Assert.True(client.ExposedPathParameters.TryGetValue("baseurl", out var baseUrlValue));
        Assert.Equal(existingBaseUrl, baseUrlValue);
    }

    /// <summary>
    /// Kmsプロパティが正しいKmsRequestBuilderを返すことを確認
    /// </summary>
    [Fact]
    public void Kms_ReturnsKmsRequestBuilder()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new KMSClient(requestAdapter);

        var kmsBuilder = client.Kms;

        Assert.NotNull(kmsBuilder);
        Assert.IsType<KmsRequestBuilder>(kmsBuilder);
    }

    /// <summary>
    /// ゾーン指定のコンストラクタがBaseUrlとPathParametersを正しく設定することを確認
    /// </summary>
    [Theory]
    [InlineData(KMSClient.SakuraCloudZone.Is1a, "https://secure.sakura.ad.jp/cloud/zone/is1a/api/cloud/1.1")]
    [InlineData(KMSClient.SakuraCloudZone.Is1b, "https://secure.sakura.ad.jp/cloud/zone/is1b/api/cloud/1.1")]
    [InlineData(KMSClient.SakuraCloudZone.Tk1a, "https://secure.sakura.ad.jp/cloud/zone/tk1a/api/cloud/1.1")]
    [InlineData(KMSClient.SakuraCloudZone.Tk1b, "https://secure.sakura.ad.jp/cloud/zone/tk1b/api/cloud/1.1")]
    public void Constructor_WithZone_SetsBaseUrlAndPathParameters(KMSClient.SakuraCloudZone zone, string expectedUrl)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = string.Empty;
        var client = new TestKMSClient(requestAdapter, zone);

        Assert.Equal(expectedUrl, requestAdapter.BaseUrl);
        Assert.NotNull(client.ExposedPathParameters);
        Assert.True(client.ExposedPathParameters.TryGetValue("baseurl", out var baseUrlValue));
        Assert.Equal(expectedUrl, baseUrlValue?.ToString());
    }

    /// <summary>
    /// ゾーン指定のコンストラクタが既存のBaseUrlをゾーンのURLで上書きすることを確認
    /// </summary>
    [Theory]
    [InlineData(KMSClient.SakuraCloudZone.Is1a, "https://secure.sakura.ad.jp/cloud/zone/is1a/api/cloud/1.1")]
    [InlineData(KMSClient.SakuraCloudZone.Is1b, "https://secure.sakura.ad.jp/cloud/zone/is1b/api/cloud/1.1")]
    [InlineData(KMSClient.SakuraCloudZone.Tk1a, "https://secure.sakura.ad.jp/cloud/zone/tk1a/api/cloud/1.1")]
    [InlineData(KMSClient.SakuraCloudZone.Tk1b, "https://secure.sakura.ad.jp/cloud/zone/tk1b/api/cloud/1.1")]
    public void Constructor_WithZone_OverridesExistingBaseUrl(KMSClient.SakuraCloudZone zone, string expectedUrl)
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();
        requestAdapter.BaseUrl = "https://api.example.com";
        var client = new TestKMSClient(requestAdapter, zone);

        Assert.Equal(expectedUrl, requestAdapter.BaseUrl);
        Assert.True(client.ExposedPathParameters.TryGetValue("baseurl", out var baseUrlValue));
        Assert.Equal(expectedUrl, baseUrlValue);
    }

    /// <summary>
    /// 未対応のゾーン指定で例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithUnsupportedZone_ThrowsArgumentOutOfRangeException()
    {
        var requestAdapter = Substitute.For<IRequestAdapter>();

        Assert.Throws<ArgumentOutOfRangeException>(() => new KMSClient(requestAdapter, (KMSClient.SakuraCloudZone)999));
    }

    /// <summary>
    /// ゾーン指定コンストラクタでnullのrequestAdapterが渡された場合に例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithZoneNullRequestAdapter_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new KMSClient(null!, KMSClient.SakuraCloudZone.Is1a));
    }

    private sealed class TestKMSClient : KMSClient
    {
        public TestKMSClient(IRequestAdapter requestAdapter)
            : base(requestAdapter)
        {
        }

        public TestKMSClient(IRequestAdapter requestAdapter, SakuraCloudZone zone)
            : base(requestAdapter, zone)
        {
        }

        public IDictionary<string, object> ExposedPathParameters => PathParameters;
    }
}
