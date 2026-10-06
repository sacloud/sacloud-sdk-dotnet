using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sacloud.Sdk.IAM.Models;
using Sacloud.Sdk.Shared.Validation;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// AuthConditionsのテスト
/// </summary>
public class AuthConditionsTests
{
    private readonly ILogger _mockLogger = Substitute.For<ILogger>();

    /// <summary>
    /// CreateFromDiscriminatorValueが正しくインスタンスを生成することを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = AuthConditions.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<AuthConditions>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() =>
            AuthConditions.CreateFromDiscriminatorValue(parseNode));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var authConditions = new AuthConditions();

        var deserializers = authConditions.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(3, deserializers.Count);
        Assert.True(deserializers.ContainsKey("datetime_restriction"));
        Assert.True(deserializers.ContainsKey("ip_restriction"));
        Assert.True(deserializers.ContainsKey("require_two_factor_auth"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var authConditions = new AuthConditions();

        Assert.Throws<ArgumentNullException>(() => authConditions.Serialize(null));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var authConditions = new AuthConditions();
        var deserializers = authConditions.GetFieldDeserializers();

        var parseNodeDatetime = Substitute.For<IParseNode>();
        var datetimeRestriction = new AuthConditions_datetime_restriction();
        parseNodeDatetime.GetObjectValue(
            Arg.Any<ParsableFactory<AuthConditions_datetime_restriction>>()).Returns(datetimeRestriction);
        deserializers["datetime_restriction"](parseNodeDatetime);

        var parseNodeIp = Substitute.For<IParseNode>();
        var ipRestriction = new AuthConditions.AuthConditions_ip_restriction();
        parseNodeIp.GetObjectValue(
            Arg.Any<ParsableFactory<AuthConditions.AuthConditions_ip_restriction>>()).Returns(ipRestriction);
        deserializers["ip_restriction"](parseNodeIp);

        var parseNodeTwoFactor = Substitute.For<IParseNode>();
        var twoFactorAuth = new AuthConditions_require_two_factor_auth();
        parseNodeTwoFactor.GetObjectValue(
            Arg.Any<ParsableFactory<AuthConditions_require_two_factor_auth>>()).Returns(twoFactorAuth);
        deserializers["require_two_factor_auth"](parseNodeTwoFactor);

        Assert.Same(datetimeRestriction, authConditions.DatetimeRestriction);
        Assert.Same(ipRestriction, authConditions.IpRestriction);
        Assert.Same(twoFactorAuth, authConditions.RequireTwoFactorAuth);
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを確認
    /// </summary>
    [Fact]
    public async Task AuthConditions_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new AuthConditions
        {
            DatetimeRestriction = new AuthConditions_datetime_restriction
            {
                After = DateTimeOffset.UtcNow,
                Before = DateTimeOffset.UtcNow.AddDays(1)
            },
            RequireTwoFactorAuth = new AuthConditions_require_two_factor_auth
            {
                Enabled = true
            }
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(AuthConditions.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.NotNull(deserializedModel.DatetimeRestriction);
        Assert.NotNull(deserializedModel.RequireTwoFactorAuth);
        Assert.True(deserializedModel.RequireTwoFactorAuth.Enabled);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを確認
    /// </summary>
    [Fact]
    public async Task AuthConditions_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new AuthConditions();
        originalModel.AdditionalData.Add("customField", "customValue");

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(AuthConditions.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }

    /// <summary>
    /// oneOfの候補が1つだけ設定されている場合に検証をパスすることを確認
    /// </summary>
    [Fact]
    public void ValidateRequestBody_WithExactlyOneIpRestrictionMember_PassesValidation()
    {
        var body = new AuthConditions
        {
            DatetimeRestriction = new AuthConditions_datetime_restriction(),
            IpRestriction = new AuthConditions.AuthConditions_ip_restriction
            {
                AuthConditionsIpRestrictionMember1 = new AuthConditions_ip_restrictionMember1
                {
                    Mode = AuthConditions_ip_restrictionMember1_mode.Allow_all
                }
            },
            RequireTwoFactorAuth = new AuthConditions_require_two_factor_auth
            {
                Enabled = false
            }
        };

        var exception = Record.Exception(() => RequestParameterValidator.ValidateRequestBody(body, _mockLogger));

        Assert.Null(exception);
    }

    /// <summary>
    /// Member1が選択されていてModeが未設定の場合に検証エラーとなることを確認
    /// </summary>
    [Fact]
    public void ValidateRequestBody_WithMember1AndNullMode_ThrowsValidationException()
    {
        var body = new AuthConditions
        {
            DatetimeRestriction = new AuthConditions_datetime_restriction(),
            IpRestriction = new AuthConditions.AuthConditions_ip_restriction
            {
                AuthConditionsIpRestrictionMember1 = new AuthConditions_ip_restrictionMember1()
            },
            RequireTwoFactorAuth = new AuthConditions_require_two_factor_auth
            {
                Enabled = false
            }
        };

        var exception = Assert.Throws<ValidationException>(() => RequestParameterValidator.ValidateRequestBody(body, _mockLogger));

        Assert.Equal("Modeが入力されていません。", exception.Message);
    }

    /// <summary>
    /// Member2が選択されていてModeが未設定の場合に検証エラーとなることを確認
    /// </summary>
    [Fact]
    public void ValidateRequestBody_WithMember2AndNullMode_ThrowsValidationException()
    {
        var body = new AuthConditions
        {
            DatetimeRestriction = new AuthConditions_datetime_restriction(),
            IpRestriction = new AuthConditions.AuthConditions_ip_restriction
            {
                AuthConditionsIpRestrictionMember2 = new AuthConditions_ip_restrictionMember2
                {
                    SourceNetwork = ["192.168.1.0/24"]
                }
            },
            RequireTwoFactorAuth = new AuthConditions_require_two_factor_auth
            {
                Enabled = false
            }
        };

        var exception = Assert.Throws<ValidationException>(() => RequestParameterValidator.ValidateRequestBody(body, _mockLogger));

        Assert.Equal("Modeが入力されていません。", exception.Message);
    }

    /// <summary>
    /// oneOfの候補が未設定の場合に検証エラーとなることを確認
    /// </summary>
    [Fact]
    public void ValidateRequestBody_WithNoIpRestrictionMember_ThrowsValidationException()
    {
        var body = new AuthConditions
        {
            DatetimeRestriction = new AuthConditions_datetime_restriction(),
            IpRestriction = new AuthConditions.AuthConditions_ip_restriction(),
            RequireTwoFactorAuth = new AuthConditions_require_two_factor_auth
            {
                Enabled = false
            }
        };

        var exception = Assert.Throws<ValidationException>(() => RequestParameterValidator.ValidateRequestBody(body, _mockLogger));

        Assert.Equal("AuthConditionsIpRestrictionMember1またはAuthConditionsIpRestrictionMember2のいずれか1つのみを設定してください。", exception.Message);
    }

    /// <summary>
    /// oneOfの候補が複数設定されている場合に検証エラーとなることを確認
    /// </summary>
    [Fact]
    public void ValidateRequestBody_WithMultipleIpRestrictionMembers_ThrowsValidationException()
    {
        var body = new AuthConditions
        {
            DatetimeRestriction = new AuthConditions_datetime_restriction(),
            IpRestriction = new AuthConditions.AuthConditions_ip_restriction
            {
                AuthConditionsIpRestrictionMember1 = new AuthConditions_ip_restrictionMember1
                {
                    Mode = AuthConditions_ip_restrictionMember1_mode.Allow_all
                },
                AuthConditionsIpRestrictionMember2 = new AuthConditions_ip_restrictionMember2
                {
                    Mode = AuthConditions_ip_restrictionMember2_mode.Allow_list,
                    SourceNetwork = ["192.168.1.0/24"]
                }
            },
            RequireTwoFactorAuth = new AuthConditions_require_two_factor_auth
            {
                Enabled = false
            }
        };

        var exception = Assert.Throws<ValidationException>(() => RequestParameterValidator.ValidateRequestBody(body, _mockLogger));

        Assert.Equal("AuthConditionsIpRestrictionMember1またはAuthConditionsIpRestrictionMember2のいずれか1つのみを設定してください。", exception.Message);
    }

    /// <summary>
    /// ip_restrictionのCreateFromDiscriminatorValueがallow_allでMember1を生成することを確認
    /// </summary>
    [Fact]
    public void IpRestrictionCreateFromDiscriminatorValue_WithAllowAll_ReturnsMember1()
    {
        var parseNode = Substitute.For<IParseNode>();
        var modeNode = Substitute.For<IParseNode>();
        parseNode.GetChildNode("mode").Returns(modeNode);
        modeNode.GetStringValue().Returns("allow_all");

        var result = AuthConditions.AuthConditions_ip_restriction.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.NotNull(result.AuthConditionsIpRestrictionMember1);
        Assert.Null(result.AuthConditionsIpRestrictionMember2);
    }

    /// <summary>
    /// ip_restrictionのCreateFromDiscriminatorValueがallow_listでMember2を生成することを確認
    /// </summary>
    [Fact]
    public void IpRestrictionCreateFromDiscriminatorValue_WithAllowList_ReturnsMember2()
    {
        var parseNode = Substitute.For<IParseNode>();
        var modeNode = Substitute.For<IParseNode>();
        parseNode.GetChildNode("mode").Returns(modeNode);
        modeNode.GetStringValue().Returns("allow_list");

        var result = AuthConditions.AuthConditions_ip_restriction.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.Null(result.AuthConditionsIpRestrictionMember1);
        Assert.NotNull(result.AuthConditionsIpRestrictionMember2);
    }

    /// <summary>
    /// ip_restrictionのCreateFromDiscriminatorValueが未知のmodeで空のwrapperを返すことを確認
    /// </summary>
    [Fact]
    public void IpRestrictionCreateFromDiscriminatorValue_WithUnknownMode_ReturnsEmptyWrapper()
    {
        var parseNode = Substitute.For<IParseNode>();
        var modeNode = Substitute.For<IParseNode>();
        parseNode.GetChildNode("mode").Returns(modeNode);
        modeNode.GetStringValue().Returns("unknown");

        var result = AuthConditions.AuthConditions_ip_restriction.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.Null(result.AuthConditionsIpRestrictionMember1);
        Assert.Null(result.AuthConditionsIpRestrictionMember2);
    }

    /// <summary>
    /// ip_restrictionのCreateFromDiscriminatorValueがnullのparseNodeで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void IpRestrictionCreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        IParseNode? parseNode = null;

        Assert.Throws<ArgumentNullException>(() =>
            AuthConditions.AuthConditions_ip_restriction.CreateFromDiscriminatorValue(parseNode));
    }

    /// <summary>
    /// ip_restrictionのGetFieldDeserializersがMember1のデシリアライザを返すことを確認
    /// </summary>
    [Fact]
    public void IpRestrictionGetFieldDeserializers_WithMember1_ReturnsMember1Deserializers()
    {
        var wrapper = new AuthConditions.AuthConditions_ip_restriction
        {
            AuthConditionsIpRestrictionMember1 = new AuthConditions_ip_restrictionMember1()
        };

        var deserializers = wrapper.GetFieldDeserializers();

        Assert.Single(deserializers);
        Assert.True(deserializers.ContainsKey("mode"));
    }

    /// <summary>
    /// ip_restrictionのGetFieldDeserializersがMember2のデシリアライザを返すことを確認
    /// </summary>
    [Fact]
    public void IpRestrictionGetFieldDeserializers_WithMember2_ReturnsMember2Deserializers()
    {
        var wrapper = new AuthConditions.AuthConditions_ip_restriction
        {
            AuthConditionsIpRestrictionMember2 = new AuthConditions_ip_restrictionMember2()
        };

        var deserializers = wrapper.GetFieldDeserializers();

        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("mode"));
        Assert.True(deserializers.ContainsKey("source_network"));
    }

    /// <summary>
    /// ip_restrictionのGetFieldDeserializersが候補未設定で空を返すことを確認
    /// </summary>
    [Fact]
    public void IpRestrictionGetFieldDeserializers_WithoutMembers_ReturnsEmptyDictionary()
    {
        var wrapper = new AuthConditions.AuthConditions_ip_restriction();

        var deserializers = wrapper.GetFieldDeserializers();

        Assert.Empty(deserializers);
    }

    /// <summary>
    /// ip_restrictionのSerializeがnull writerで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void IpRestrictionSerialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var wrapper = new AuthConditions.AuthConditions_ip_restriction();

        Assert.Throws<ArgumentNullException>(() => wrapper.Serialize(null));
    }

    /// <summary>
    /// ip_restrictionのSerializeがMember1をシリアライズすることを確認
    /// </summary>
    [Fact]
    public void IpRestrictionSerialize_WithMember1_WritesMember1()
    {
        var writer = Substitute.For<ISerializationWriter>();
        var member1 = new AuthConditions_ip_restrictionMember1
        {
            Mode = AuthConditions_ip_restrictionMember1_mode.Allow_all
        };
        var wrapper = new AuthConditions.AuthConditions_ip_restriction
        {
            AuthConditionsIpRestrictionMember1 = member1
        };

        wrapper.Serialize(writer);

        writer.Received(1).WriteObjectValue(null, member1);
        writer.DidNotReceive().WriteObjectValue(Arg.Any<string>(), Arg.Any<AuthConditions_ip_restrictionMember2>());
    }

    /// <summary>
    /// ip_restrictionのSerializeがMember2をシリアライズすることを確認
    /// </summary>
    [Fact]
    public void IpRestrictionSerialize_WithMember2_WritesMember2()
    {
        var writer = Substitute.For<ISerializationWriter>();
        var member2 = new AuthConditions_ip_restrictionMember2
        {
            Mode = AuthConditions_ip_restrictionMember2_mode.Allow_list,
            SourceNetwork = ["192.168.1.0/24"]
        };
        var wrapper = new AuthConditions.AuthConditions_ip_restriction
        {
            AuthConditionsIpRestrictionMember2 = member2
        };

        wrapper.Serialize(writer);

        writer.Received(1).WriteObjectValue(null, member2);
        writer.DidNotReceive().WriteObjectValue(Arg.Any<string>(), Arg.Any<AuthConditions_ip_restrictionMember1>());
    }
}
