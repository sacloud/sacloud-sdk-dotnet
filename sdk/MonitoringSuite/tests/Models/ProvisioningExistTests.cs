using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// ProvisioningExistモデルのテスト
/// </summary>
public class ProvisioningExistTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void ProvisioningExist_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<ProvisioningExist>(ProvisioningExist.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));

    /// <summary>
    /// nullパースノードでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void ProvisioningExist_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => ProvisioningExist.CreateFromDiscriminatorValue(null!));

    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void ProvisioningExist_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new ProvisioningExist().GetFieldDeserializers();
        Assert.Equal(2, d.Count);
        Assert.True(d.ContainsKey("system_exist"));
        Assert.True(d.ContainsKey("user_exist"));
    }

    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void ProvisioningExist_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new ProvisioningExist().Serialize(null!));

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void ProvisioningExist_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new ProvisioningExist();
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
    public void ProvisioningExist_Serialize_WritesExpectedFields()
    {
        var model = new ProvisioningExist { SystemExist = true, UserExist = true };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteBoolValue("system_exist", true);
        writer.Received(1).WriteBoolValue("user_exist", true);
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
