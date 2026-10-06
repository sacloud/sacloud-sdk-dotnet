using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using User_otpModel = Sacloud.Sdk.IAM.Models.User_otp;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// User_otpのテスト
/// </summary>
public class User_otpTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = User_otpModel.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<User_otpModel>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Sacloud.Sdk.IAM.Models.User_otp.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピング（"has_recovery_code", "status"）を返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var otp = new User_otpModel();

        var deserializers = otp.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(2, deserializers.Count);
        Assert.True(deserializers.ContainsKey("has_recovery_code"));
        Assert.True(deserializers.ContainsKey("status"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var otp = new User_otpModel();

        Assert.Throws<ArgumentNullException>(() => otp.Serialize(null));
    }

    /// <summary>
    /// Serializeが正しく値を書き込むことを検証
    /// </summary>
    [Fact]
    public void Serialize_WritesValues()
    {
        var otp = new User_otpModel
        {
            HasRecoveryCode = true,
            Status = global::Sacloud.Sdk.IAM.Models.User_otp_status.Activated
        };
        var writer = Substitute.For<ISerializationWriter>();

        otp.Serialize(writer);

        writer.Received(1).WriteBoolValue("has_recovery_code", true);
        writer.Received(1).WriteEnumValue<IAM.Models.User_otp_status>("status", global::Sacloud.Sdk.IAM.Models.User_otp_status.Activated);
        writer.Received(1).WriteAdditionalData(Arg.Any<IDictionary<string, object>>());
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクション（"has_recovery_code", "status"）が正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var otp = new User_otpModel();
        var deserializers = otp.GetFieldDeserializers();
        
        var parseNodeHasRecoveryCode = Substitute.For<IParseNode>();
        parseNodeHasRecoveryCode.GetBoolValue().Returns(true);
        deserializers["has_recovery_code"](parseNodeHasRecoveryCode);
        
        var parseNodeStatus = Substitute.For<IParseNode>();
        parseNodeStatus.GetEnumValue<IAM.Models.User_otp_status>().Returns(global::Sacloud.Sdk.IAM.Models.User_otp_status.Activated);
        deserializers["status"](parseNodeStatus);

        Assert.True(otp.HasRecoveryCode);
        Assert.Equal(global::Sacloud.Sdk.IAM.Models.User_otp_status.Activated, otp.Status);
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task User_otp_CanSerializeAndDeserializeWithAllProperties()
    {
        var originalModel = new User_otpModel
        {
            HasRecoveryCode = true,
            Status = global::Sacloud.Sdk.IAM.Models.User_otp_status.Activated
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Sacloud.Sdk.IAM.Models.User_otp.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.HasRecoveryCode, deserializedModel.HasRecoveryCode);
        Assert.Equal(originalModel.Status, deserializedModel.Status);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task User_otp_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new User_otpModel
        {
            HasRecoveryCode = true
        };
        originalModel.AdditionalData.Add("customField", "customValue");

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(Sacloud.Sdk.IAM.Models.User_otp.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.HasRecoveryCode, deserializedModel.HasRecoveryCode);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
