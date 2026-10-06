using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// PaginatedAlertProjectListモデルのテスト
/// </summary>
public class PaginatedAlertProjectListTests
{
    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void PaginatedAlertProjectList_CreateFromDiscriminatorValue_ReturnsInstance()
    {
        var parseNode = Substitute.For<IParseNode>();
        var result = PaginatedAlertProjectList.CreateFromDiscriminatorValue(parseNode);
        Assert.NotNull(result);
        Assert.IsType<PaginatedAlertProjectList>(result);
    }

    /// <summary>
    /// nullパースノードでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void PaginatedAlertProjectList_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => PaginatedAlertProjectList.CreateFromDiscriminatorValue(null!));
    }

    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void PaginatedAlertProjectList_GetFieldDeserializers_ReturnsCorrectMappings()
    {
        var d = new PaginatedAlertProjectList().GetFieldDeserializers();
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
    public void PaginatedAlertProjectList_Serialize_WithNullWriter_ThrowsArgumentNullException()
    {
        var model = new PaginatedAlertProjectList();
        Assert.Throws<ArgumentNullException>(() => model.Serialize(null!));
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void PaginatedAlertProjectList_Serialize_WritesExpectedFields()
    {
        var model = new PaginatedAlertProjectList { Count = 1, From = 0, IsOk = true, Total = 1 };
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
        writer.Received(1).WriteLongValue("count", 1L);
        writer.Received(1).WriteLongValue("from", 0L);
        writer.Received(1).WriteBoolValue("is_ok", true);
        writer.Received(1).WriteLongValue("total", 1L);
        writer.Received(1).WriteAdditionalData(model.AdditionalData);
    }

    /// <summary>
    /// コンストラクタがAdditionalDataを初期化することを確認
    /// </summary>
    [Fact]
    public void PaginatedAlertProjectList_Constructor_InitializesAdditionalData()
    {
        var model = new PaginatedAlertProjectList();
        Assert.NotNull(model.AdditionalData);
    }

    /// <summary>
    /// GetFieldDeserializersのすべてのアクションが正しく動作することを確認
    /// </summary>
    [Fact]
    public void PaginatedAlertProjectList_GetFieldDeserializers_AllActionsWorkCorrectly()
    {
        var model = new PaginatedAlertProjectList();
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
}
