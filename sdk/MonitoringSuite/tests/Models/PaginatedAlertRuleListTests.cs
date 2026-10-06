using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// PaginatedAlertRuleListモデルのテスト
/// </summary>
public class PaginatedAlertRuleListTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void PaginatedAlertRuleList_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = PaginatedAlertRuleList.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<PaginatedAlertRuleList>(result);
    }

    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void PaginatedAlertRuleList_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new PaginatedAlertRuleList().GetFieldDeserializers();
        Assert.Equal(5, d.Count);
        Assert.True(d.ContainsKey("count"));
        Assert.True(d.ContainsKey("from"));
        Assert.True(d.ContainsKey("is_ok"));
        Assert.True(d.ContainsKey("results"));
        Assert.True(d.ContainsKey("total"));
    }

    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void PaginatedAlertRuleList_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new PaginatedAlertRuleList().Serialize(null!));
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void PaginatedAlertRuleList_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new PaginatedAlertRuleList();
        var deserializers = model.GetFieldDeserializers();
        var parseNode = Substitute.For<IParseNode>();

        parseNode.GetLongValue().Returns(1L);
        deserializers["count"](parseNode);
        Assert.Equal(1L, model.Count);

        parseNode.GetLongValue().Returns(1L);
        deserializers["from"](parseNode);
        Assert.Equal(1L, model.From);

        parseNode.GetBoolValue().Returns(true);
        deserializers["is_ok"](parseNode);
        Assert.Equal(true, model.IsOk);

        deserializers["results"](parseNode);

        parseNode.GetLongValue().Returns(1L);
        deserializers["total"](parseNode);
        Assert.Equal(1L, model.Total);
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void PaginatedAlertRuleList_Serialize_WritesExpectedFields()
    {
        var model = new PaginatedAlertRuleList { Count = 1L, From = 1L, IsOk = true, Total = 1L };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteLongValue("count", 1L);
        writer.Received(1).WriteLongValue("from", 1L);
        writer.Received(1).WriteBoolValue("is_ok", true);
        writer.Received(1).WriteLongValue("total", 1L);
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }
}
