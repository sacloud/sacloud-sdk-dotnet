using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// ProvisioningExistRequestモデルのテスト
/// </summary>
public class ProvisioningExistRequestTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void ProvisioningExistRequest_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<ProvisioningExistRequest>(ProvisioningExistRequest.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));

    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void ProvisioningExistRequest_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new ProvisioningExistRequest().Serialize(null!));

    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void ProvisioningExistRequest_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new ProvisioningExistRequest().GetFieldDeserializers();
        Assert.Equal(2, d.Count);
        Assert.True(d.ContainsKey("system_exist"));
        Assert.True(d.ContainsKey("user_exist"));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void ProvisioningExistRequest_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new ProvisioningExistRequest();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetBoolValue().Returns(true);
        deserializers["system_exist"](parseNode);
        Assert.Equal(true, model.SystemExist);

        parseNode.GetBoolValue().Returns(true);
        deserializers["user_exist"](parseNode);
        Assert.Equal(true, model.UserExist);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void ProvisioningExistRequest_Serialize_WritesExpectedFields()
    {
        var model = new ProvisioningExistRequest { SystemExist = true, UserExist = true };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteBoolValue("system_exist", true);
        writer.Received(1).WriteBoolValue("user_exist", true);
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
