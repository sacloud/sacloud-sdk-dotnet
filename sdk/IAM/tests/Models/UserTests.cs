using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;
using UserModel = Sacloud.Sdk.IAM.Models.User;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// Userのテスト
/// </summary>
public class UserTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueがインスタンスを返すことを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();

        var result = UserModel.CreateFromDiscriminatorValue(parseNode);

        Assert.NotNull(result);
        Assert.IsType<UserModel>(result);
    }

    /// <summary>
    /// CreateFromDiscriminatorValueがnull ParseNodeで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => UserModel.CreateFromDiscriminatorValue(null));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいマッピングを返すことを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_ReturnsCorrectMapping()
    {
        var model = new UserModel();

        var deserializers = model.GetFieldDeserializers();

        Assert.NotNull(deserializers);
        Assert.Equal(12, deserializers.Count);
        Assert.True(deserializers.ContainsKey("code"));
        Assert.True(deserializers.ContainsKey("created_at"));
        Assert.True(deserializers.ContainsKey("description"));
        Assert.True(deserializers.ContainsKey("email"));
        Assert.True(deserializers.ContainsKey("id"));
        Assert.True(deserializers.ContainsKey("is_passwordless"));
        Assert.True(deserializers.ContainsKey("is_security_key_registered"));
        Assert.True(deserializers.ContainsKey("member"));
        Assert.True(deserializers.ContainsKey("name"));
        Assert.True(deserializers.ContainsKey("otp"));
        Assert.True(deserializers.ContainsKey("status"));
        Assert.True(deserializers.ContainsKey("updated_at"));
    }

    /// <summary>
    /// Serializeがnull writerで例外をスローすることを検証
    /// </summary>
    [Fact]
    public void Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new UserModel();

        Assert.Throws<ArgumentNullException>(() => model.Serialize(null));
    }

    /// <summary>
    /// Serializeが正しく値を書き込むことを検証
    /// </summary>
    [Fact]
    public void Serialize_WritesValues()
    {
        var mockOtp = Substitute.For<IAM.Models.User_otp>();
        var model = new UserModel
        {
            Description = "Test Description",
            IsSecurityKeyRegistered = true,
            Name = "Test User",
            Otp = mockOtp
        };
        var writer = Substitute.For<ISerializationWriter>();

        model.Serialize(writer);

        writer.Received(1).WriteStringValue("description", "Test Description");
        writer.Received(1).WriteBoolValue("is_security_key_registered", true);
        writer.Received(1).WriteStringValue("name", "Test User");
        writer.Received(1).WriteObjectValue("otp", mockOtp);
        writer.Received(1).WriteAdditionalData(Arg.Any<IDictionary<string, object>>());
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを検証
    /// </summary>
    [Fact]
    public void GetFieldDeserializers_AllActions_WorkCorrectly()
    {
        var model = new UserModel();
        var deserializers = model.GetFieldDeserializers();
        
        var parseNodeCode = Substitute.For<IParseNode>();
        parseNodeCode.GetStringValue().Returns("USER001");
        deserializers["code"](parseNodeCode);
        
        var parseNodeCreatedAt = Substitute.For<IParseNode>();
        parseNodeCreatedAt.GetStringValue().Returns("2024-01-01T00:00:00Z");
        deserializers["created_at"](parseNodeCreatedAt);
        
        var parseNodeDescription = Substitute.For<IParseNode>();
        parseNodeDescription.GetStringValue().Returns("Test Description");
        deserializers["description"](parseNodeDescription);
        
        var parseNodeEmail = Substitute.For<IParseNode>();
        parseNodeEmail.GetStringValue().Returns("test@example.com");
        deserializers["email"](parseNodeEmail);
        
        var parseNodeId = Substitute.For<IParseNode>();
        parseNodeId.GetLongValue().Returns(123456L);
        deserializers["id"](parseNodeId);
        
        var parseNodeIsPasswordless = Substitute.For<IParseNode>();
        parseNodeIsPasswordless.GetBoolValue().Returns(true);
        deserializers["is_passwordless"](parseNodeIsPasswordless);
        
        var parseNodeIsSecurityKeyRegistered = Substitute.For<IParseNode>();
        parseNodeIsSecurityKeyRegistered.GetBoolValue().Returns(false);
        deserializers["is_security_key_registered"](parseNodeIsSecurityKeyRegistered);
        
        var mockMember = Substitute.For<IAM.Models.User_member>();
        var parseNodeMember = Substitute.For<IParseNode>();
        parseNodeMember.GetObjectValue(Arg.Any<ParsableFactory<IAM.Models.User_member>>())
            .Returns(mockMember);
        deserializers["member"](parseNodeMember);
        
        var parseNodeName = Substitute.For<IParseNode>();
        parseNodeName.GetStringValue().Returns("Test User");
        deserializers["name"](parseNodeName);
        
        var mockOtp = Substitute.For<IAM.Models.User_otp>();
        var parseNodeOtp = Substitute.For<IParseNode>();
        parseNodeOtp.GetObjectValue(Arg.Any<ParsableFactory<IAM.Models.User_otp>>())
            .Returns(mockOtp);
        deserializers["otp"](parseNodeOtp);
        
        var parseNodeStatus = Substitute.For<IParseNode>();
        parseNodeStatus.GetEnumValue<IAM.Models.User_status>()
            .Returns(Sacloud.Sdk.IAM.Models.User_status.Available);
        deserializers["status"](parseNodeStatus);
        
        var parseNodeUpdatedAt = Substitute.For<IParseNode>();
        parseNodeUpdatedAt.GetStringValue().Returns("2024-01-02T00:00:00Z");
        deserializers["updated_at"](parseNodeUpdatedAt);

        Assert.Equal("USER001", model.Code);
        Assert.Equal("2024-01-01T00:00:00Z", model.CreatedAt);
        Assert.Equal("Test Description", model.Description);
        Assert.Equal("test@example.com", model.Email);
        Assert.Equal(123456L, model.Id);
        Assert.True(model.IsPasswordless);
        Assert.False(model.IsSecurityKeyRegistered);
        Assert.NotNull(model.Member);
        Assert.Equal("Test User", model.Name);
        Assert.NotNull(model.Otp);
        Assert.Equal(Sacloud.Sdk.IAM.Models.User_status.Available, model.Status);
        Assert.Equal("2024-01-02T00:00:00Z", model.UpdatedAt);
    }

    /// <summary>
    /// シリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task User_CanSerializeAndDeserializeWithAllProperties()
    {
        var mockOtp = new IAM.Models.User_otp
        {
            HasRecoveryCode = true,
            Status = Sacloud.Sdk.IAM.Models.User_otp_status.Activated
        };
        var originalModel = new UserModel
        {
            Description = "Test Description",
            IsSecurityKeyRegistered = false,
            Name = "Test User",
            Otp = mockOtp
        };

        using var serializeStream = new MemoryStream();
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, originalModel);
        var content = writer.GetSerializedContent();
        content.CopyTo(serializeStream);

        serializeStream.Position = 0;
        var parseNodeFactory = new JsonParseNodeFactory();
        var parseNode = await parseNodeFactory.GetRootParseNodeAsync("application/json", serializeStream);
        var deserializedModel = parseNode.GetObjectValue(UserModel.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Description, deserializedModel.Description);
        Assert.Equal(originalModel.IsSecurityKeyRegistered, deserializedModel.IsSecurityKeyRegistered);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.NotNull(deserializedModel.Otp);
    }

    /// <summary>
    /// AdditionalDataのシリアライズとデシリアライズが可能なことを検証
    /// </summary>
    [Fact]
    public async Task User_CanSerializeAndDeserializeWithAdditionalData()
    {
        var originalModel = new UserModel
        {
            Name = "Test User"
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
        var deserializedModel = parseNode.GetObjectValue(UserModel.CreateFromDiscriminatorValue);

        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.True(deserializedModel.AdditionalData.ContainsKey("customField"));
    }
}
