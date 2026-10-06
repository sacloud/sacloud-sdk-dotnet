using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using Sacloud.Sdk.IAM.Auth.Context;

namespace Sacloud.Sdk.IAM.Tests.Auth.Context;

/// <summary>
/// ContextGetResponseのテスト
/// </summary>
public class ContextGetResponseTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = ContextGetResponse.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<ContextGetResponse>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueにnullのParseNodeを渡した場合にArgumentNullExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ContextGetResponse.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいデシリアライザのマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new ContextGetResponse();
        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(4, deserializers.Count);
        Assert.True(deserializers.ContainsKey("auth_type"));
        Assert.True(deserializers.ContainsKey("limited_to_project_id"));
        Assert.True(deserializers.ContainsKey("member_code"));
        Assert.True(deserializers.ContainsKey("resource_id"));
    }

    /// <summary>
    /// Serializeにnullのwriterを渡した場合にArgumentNullExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new ContextGetResponse();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new ContextGetResponse();
        var deserializers = model.GetFieldDeserializers();

        var parseNodeAuth = Substitute.For<IParseNode>();
        parseNodeAuth.GetEnumValue<ContextGetResponse_auth_type>().Returns(ContextGetResponse_auth_type.Apikey);
        deserializers["auth_type"](parseNodeAuth);

        var parseNodeLimited = Substitute.For<IParseNode>();
        parseNodeLimited.GetLongValue().Returns(123L);
        deserializers["limited_to_project_id"](parseNodeLimited);

        var parseNodeMemberCode = Substitute.For<IParseNode>();
        parseNodeMemberCode.GetStringValue().Returns("member-code");
        deserializers["member_code"](parseNodeMemberCode);

        var parseNodeResource = Substitute.For<IParseNode>();
        parseNodeResource.GetLongValue().Returns(456L);
        deserializers["resource_id"](parseNodeResource);

        Assert.Equal(ContextGetResponse_auth_type.Apikey, model.AuthType);
        Assert.Equal(123L, model.LimitedToProjectId);
        Assert.Equal("member-code", model.MemberCode);
        Assert.Equal(456L, model.ResourceId);
    }

    /// <summary>
    /// ContextGetResponseがすべてのプロパティをシリアライズおよびデシリアライズできることを確認
    /// </summary>
    [Fact]
    public async Task ContextGetResponse_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new ContextGetResponse
        {
            AuthType = ContextGetResponse_auth_type.Service_principal,
            LimitedToProjectId = 111L,
            MemberCode = "member-code",
            ResourceId = 222L
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;

        var factory = new JsonParseNodeFactory();
        var parseNode = await factory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(ContextGetResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(ContextGetResponse_auth_type.Service_principal, deserializedModel.AuthType);
        Assert.Equal(111L, deserializedModel.LimitedToProjectId);
        Assert.Equal("member-code", deserializedModel.MemberCode);
        Assert.Equal(222L, deserializedModel.ResourceId);
    }

    /// <summary>
    /// ContextGetResponseがAdditionalDataを含めてシリアライズおよびデシリアライズできることを確認
    /// </summary>
    [Fact]
    public async Task ContextGetResponse_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new ContextGetResponse
        {
            AuthType = ContextGetResponse_auth_type.Service_principal,
            LimitedToProjectId = 111L,
            MemberCode = "member-code",
            ResourceId = 222L
        };
        originalModel.AdditionalData["custom"] = "value";

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);
        serializeStream.Position = 0;

        var parseNode = await new JsonParseNodeFactory().GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(ContextGetResponse.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("custom"));
    }

    /// <summary>
    /// コンストラクタでAdditionalDataが初期化されることを確認
    /// </summary>
    [Fact]
    public void ContextGetResponse_Constructor_InitializesAdditionalData()
    {
        var model = new ContextGetResponse();
        Assert.NotNull(model.AdditionalData);
        Assert.Empty(model.AdditionalData);
    }

    /// <summary>
    /// Serializeがすべてのプロパティ値を書き込むことを確認
    /// </summary>
    [Fact]
    public void ContextGetResponse_Serialize_WritesAllPropertyValues()
    {
        var model = new ContextGetResponse
        {
            AuthType = ContextGetResponse_auth_type.Apikey,
            LimitedToProjectId = 10L,
            MemberCode = "member-code",
            ResourceId = 20L
        };

        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);

        writer.Received(1).WriteEnumValue<ContextGetResponse_auth_type>("auth_type", ContextGetResponse_auth_type.Apikey);
        writer.Received(1).WriteLongValue("limited_to_project_id", 10L);
        writer.Received(1).WriteStringValue("member_code", "member-code");
        writer.Received(1).WriteLongValue("resource_id", 20L);
    }

    /// <summary>
    /// SerializeがAdditionalDataを書き込むことを確認
    /// </summary>
    [Fact]
    public void ContextGetResponse_Serialize_WritesAdditionalData()
    {
        var model = new ContextGetResponse();
        model.AdditionalData["k"] = "v";
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
