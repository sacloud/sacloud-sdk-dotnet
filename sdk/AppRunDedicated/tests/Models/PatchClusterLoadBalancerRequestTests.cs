using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.AppRunDedicated.Models;

namespace Sacloud.Sdk.AppRunDedicated.Tests.Models;

/// <summary>
/// PatchClusterLoadBalancerRequestモデルのテスト
/// </summary>
public class PatchClusterLoadBalancerRequestTests
{

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void PatchClusterLoadBalancerRequest_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var model = new global::Sacloud.Sdk.AppRunDedicated.Models.PatchClusterLoadBalancerRequest();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("letsEncryptEmail"));
        Assert.True(deserializers.ContainsKey("ports"));
    }

    /// <summary>
    /// Serializeがnullライターで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void PatchClusterLoadBalancerRequest_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new global::Sacloud.Sdk.AppRunDedicated.Models.PatchClusterLoadBalancerRequest();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// LetsEncryptEmailがnullの場合にJSONのnull値としてシリアライズされることを確認
    /// </summary>
    [Fact]
    public void PatchClusterLoadBalancerRequest_Serialize_WithNullLetsEncryptEmail_WritesNullValue()
    {
        var model = new global::Sacloud.Sdk.AppRunDedicated.Models.PatchClusterLoadBalancerRequest
        {
            LetsEncryptEmail = null,
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, model);
        using var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        var serializedJson = Encoding.UTF8.GetString(serializeStream.ToArray());

        Assert.Contains("\"letsEncryptEmail\":null", serializedJson);
    }

    /// <summary>
    /// LetsEncryptEmailがメールアドレスの場合にJSON文字列としてシリアライズされることを確認
    /// </summary>
    [Fact]
    public void PatchClusterLoadBalancerRequest_Serialize_WithLetsEncryptEmail_WritesStringValue()
    {
        var model = new global::Sacloud.Sdk.AppRunDedicated.Models.PatchClusterLoadBalancerRequest
        {
            LetsEncryptEmail = "test@example.com",
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, model);
        using var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        var serializedJson = Encoding.UTF8.GetString(serializeStream.ToArray());

        Assert.Contains("\"letsEncryptEmail\":\"test@example.com\"", serializedJson);
    }

    /// <summary>
    /// LetsEncryptEmailが未設定（デフォルト値のnull）の場合も、
    /// WriteNullableStringValueの仕様によりletsEncryptEmailキーがnullとして出力されることを確認。
    /// 本モデルはKiotaの標準シリアライズを手動修正しているため「未設定」と「明示的なnull」を区別できず、
    /// どちらの場合も常にnull値が送信される。
    /// </summary>
    [Fact]
    public void PatchClusterLoadBalancerRequest_Serialize_WithUnsetLetsEncryptEmail_WritesNullValue()
    {
        var model = new global::Sacloud.Sdk.AppRunDedicated.Models.PatchClusterLoadBalancerRequest
        {
            Ports = [],
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, model);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        var serializedJson = Encoding.UTF8.GetString(serializeStream.ToArray());

        Assert.Contains("\"letsEncryptEmail\":null", serializedJson);
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void PatchClusterLoadBalancerRequest_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new global::Sacloud.Sdk.AppRunDedicated.Models.PatchClusterLoadBalancerRequest();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        Assert.True(deserializers.ContainsKey("letsEncryptEmail"));

        parseNode.GetStringValue().Returns("sample");

        deserializers["letsEncryptEmail"](parseNode);
        Assert.NotNull(model.LetsEncryptEmail);

        Assert.True(deserializers.ContainsKey("ports"));

        parseNode.GetCollectionOfObjectValues<Sacloud.Sdk.AppRunDedicated.Models.ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly>(Arg.Any<ParsableFactory<Sacloud.Sdk.AppRunDedicated.Models.ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly>>()).Returns(new List<Sacloud.Sdk.AppRunDedicated.Models.ClusterLoadBalancerPortMergePatchCreateOrUpdateReplaceOnly>());

        deserializers["ports"](parseNode);
        Assert.NotNull(model.Ports);
    }

    /// <summary>
    /// すべてのプロパティでシリアライズとデシリアライズができることを確認
    /// </summary>
    [Fact]
    public async Task PatchClusterLoadBalancerRequest_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new global::Sacloud.Sdk.AppRunDedicated.Models.PatchClusterLoadBalancerRequest();
        originalModel.LetsEncryptEmail = "sample";
        originalModel.Ports = [];

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue<global::Sacloud.Sdk.AppRunDedicated.Models.PatchClusterLoadBalancerRequest>(global::Sacloud.Sdk.AppRunDedicated.Models.PatchClusterLoadBalancerRequest.CreateFromDiscriminatorValue);
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.LetsEncryptEmail, deserializedModel!.LetsEncryptEmail);
        Assert.NotNull(deserializedModel!.Ports);
    }

    /// <summary>
    /// AdditionalDataが往復シリアライズで保持されることを確認
    /// </summary>
    [Fact]
    public async Task PatchClusterLoadBalancerRequest_AdditionalData_RoundTrip_PreservesSerializedKeys()
    {
        var model = new global::Sacloud.Sdk.AppRunDedicated.Models.PatchClusterLoadBalancerRequest
        {
            AdditionalData = new Dictionary<string, object>
            {
                { "extra_key", "extra-value" },
            },
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, model);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue<global::Sacloud.Sdk.AppRunDedicated.Models.PatchClusterLoadBalancerRequest>(global::Sacloud.Sdk.AppRunDedicated.Models.PatchClusterLoadBalancerRequest.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel!.AdditionalData.ContainsKey("extra_key"));
    }
}
