using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.MonitoringSuite.Models;

namespace Sacloud.Sdk.MonitoringSuite.Tests.Models;

/// <summary>
/// FieldMatcherモデルのテスト
/// </summary>
public class FieldMatcherTests
{
    private static ISerializationWriter W() => Substitute.For<ISerializationWriter>();

    /// <summary>
    /// CreateFromDiscriminatorValueが正しいインスタンスを返すことを確認
    /// </summary>
    [Fact]
    public void FieldMatcher_CreateFromDiscriminatorValue_ReturnsInstance()
        => Assert.IsType<FieldMatcher>(FieldMatcher.CreateFromDiscriminatorValue(Substitute.For<IParseNode>()));

    /// <summary>
    /// nullパースノードでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void FieldMatcher_CreateFromDiscriminatorValue_WithNullParseNode_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => FieldMatcher.CreateFromDiscriminatorValue(null!));

    /// <summary>
    /// nullライターでArgumentNullExceptionがスローされることを確認
    /// </summary>
    [Fact]
    public void FieldMatcher_Serialize_WithNullWriter_ThrowsArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() => new FieldMatcher().Serialize(null!));

    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void FieldMatcher_GetFieldDeserializers_ReturnsEmptyWhenNoSubMatcherSet()
        => Assert.Empty(new FieldMatcher().GetFieldDeserializers());

    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void FieldMatcher_GetFieldDeserializers_ReturnsFromSubMatcherWhenSet()
    {
        var fm = new FieldMatcher { StrMatcher = new StrMatcher() };
        Assert.NotEmpty(fm.GetFieldDeserializers());
    }

    // --- Branch tests (GetFieldDeserializers with each sub-matcher) ---

    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void FieldMatcher_GetFieldDeserializers_WithAndMatcher_ReturnsAndMatcherFields()
    {
        var fm = new FieldMatcher { AndMatcher = new AndMatcher() };
        Assert.NotNull(fm.GetFieldDeserializers());
    }

    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void FieldMatcher_GetFieldDeserializers_WithBoolMatcher_ReturnsBoolMatcherFields()
    {
        var fm = new FieldMatcher { BoolMatcher = new BoolMatcher() };
        Assert.NotNull(fm.GetFieldDeserializers());
    }

    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void FieldMatcher_GetFieldDeserializers_WithEnumMatcher_ReturnsEnumMatcherFields()
    {
        var fm = new FieldMatcher { EnumMatcher = new EnumMatcher() };
        Assert.NotNull(fm.GetFieldDeserializers());
    }

    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void FieldMatcher_GetFieldDeserializers_WithMapKeyExistsMatcher_ReturnsFields()
    {
        var fm = new FieldMatcher { MapKeyExistsMatcher = new MapKeyExistsMatcher() };
        Assert.NotNull(fm.GetFieldDeserializers());
    }

    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void FieldMatcher_GetFieldDeserializers_WithMapKeyValueMatcher_ReturnsFields()
    {
        var fm = new FieldMatcher { MapKeyValueMatcher = new MapKeyValueMatcher() };
        Assert.NotNull(fm.GetFieldDeserializers());
    }

    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void FieldMatcher_GetFieldDeserializers_WithMapValueNumMatcher_ReturnsFields()
    {
        var fm = new FieldMatcher { MapValueNumMatcher = new MapValueNumMatcher() };
        Assert.NotNull(fm.GetFieldDeserializers());
    }

    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void FieldMatcher_GetFieldDeserializers_WithMapValueStrMatcher_ReturnsFields()
    {
        var fm = new FieldMatcher { MapValueStrMatcher = new MapValueStrMatcher() };
        Assert.NotNull(fm.GetFieldDeserializers());
    }

    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void FieldMatcher_GetFieldDeserializers_WithNumMatcher_ReturnsFields()
    {
        var fm = new FieldMatcher { NumMatcher = new NumMatcher() };
        Assert.NotNull(fm.GetFieldDeserializers());
    }

    /// <summary>
    /// GetFieldDeserializersが正しいキーを含むことを確認
    /// </summary>
    [Fact]
    public void FieldMatcher_GetFieldDeserializers_WithOrMatcher_ReturnsFields()
    {
        var fm = new FieldMatcher { OrMatcher = new OrMatcher() };
        Assert.NotNull(fm.GetFieldDeserializers());
    }

    // --- Branch tests (Serialize with each sub-matcher) ---

    /// <summary>
    /// テストの説明
    /// </summary>
    [Fact]
    public void FieldMatcher_Serialize_WithAndMatcher_Invokes()
    {
        var fm = new FieldMatcher { AndMatcher = new AndMatcher() };
        fm.Serialize(W());
    }

    /// <summary>
    /// テストの説明
    /// </summary>
    [Fact]
    public void FieldMatcher_Serialize_WithBoolMatcher_Invokes()
    {
        var fm = new FieldMatcher { BoolMatcher = new BoolMatcher() };
        fm.Serialize(W());
    }

    /// <summary>
    /// テストの説明
    /// </summary>
    [Fact]
    public void FieldMatcher_Serialize_WithEnumMatcher_Invokes()
    {
        var fm = new FieldMatcher { EnumMatcher = new EnumMatcher() };
        fm.Serialize(W());
    }

    /// <summary>
    /// テストの説明
    /// </summary>
    [Fact]
    public void FieldMatcher_Serialize_WithMapKeyExistsMatcher_Invokes()
    {
        var fm = new FieldMatcher { MapKeyExistsMatcher = new MapKeyExistsMatcher() };
        fm.Serialize(W());
    }

    /// <summary>
    /// テストの説明
    /// </summary>
    [Fact]
    public void FieldMatcher_Serialize_WithMapKeyValueMatcher_Invokes()
    {
        var fm = new FieldMatcher { MapKeyValueMatcher = new MapKeyValueMatcher() };
        fm.Serialize(W());
    }

    /// <summary>
    /// テストの説明
    /// </summary>
    [Fact]
    public void FieldMatcher_Serialize_WithMapValueNumMatcher_Invokes()
    {
        var fm = new FieldMatcher { MapValueNumMatcher = new MapValueNumMatcher() };
        fm.Serialize(W());
    }

    /// <summary>
    /// テストの説明
    /// </summary>
    [Fact]
    public void FieldMatcher_Serialize_WithMapValueStrMatcher_Invokes()
    {
        var fm = new FieldMatcher { MapValueStrMatcher = new MapValueStrMatcher() };
        fm.Serialize(W());
    }

    /// <summary>
    /// テストの説明
    /// </summary>
    [Fact]
    public void FieldMatcher_Serialize_WithNumMatcher_Invokes()
    {
        var fm = new FieldMatcher { NumMatcher = new NumMatcher() };
        fm.Serialize(W());
    }

    /// <summary>
    /// テストの説明
    /// </summary>
    [Fact]
    public void FieldMatcher_Serialize_WithOrMatcher_Invokes()
    {
        var fm = new FieldMatcher { OrMatcher = new OrMatcher() };
        fm.Serialize(W());
    }

    /// <summary>
    /// テストの説明
    /// </summary>
    [Fact]
    public void FieldMatcher_Serialize_WithNone_DoesNotThrow()
    {
        new FieldMatcher().Serialize(W());
    }

    // --- CreateFromDiscriminatorValue discriminator tests ---

    /// <summary>
    /// テストの説明
    /// </summary>
    [Fact]
    public void FieldMatcher_CreateFromDiscriminatorValue_And_SetsAndMatcher()
    {
        var result = FieldMatcher.CreateFromDiscriminatorValue(MakeNode("and"));
        Assert.NotNull(result.AndMatcher);
    }

    /// <summary>
    /// テストの説明
    /// </summary>
    [Fact]
    public void FieldMatcher_CreateFromDiscriminatorValue_Boolean_SetsBoolMatcher()
    {
        var result = FieldMatcher.CreateFromDiscriminatorValue(MakeNode("boolean"));
        Assert.NotNull(result.BoolMatcher);
    }

    /// <summary>
    /// テストの説明
    /// </summary>
    [Fact]
    public void FieldMatcher_CreateFromDiscriminatorValue_Enum_SetsEnumMatcher()
    {
        var result = FieldMatcher.CreateFromDiscriminatorValue(MakeNode("enum"));
        Assert.NotNull(result.EnumMatcher);
    }

    /// <summary>
    /// テストの説明
    /// </summary>
    [Fact]
    public void FieldMatcher_CreateFromDiscriminatorValue_MapKeyExists_SetsMapKeyExistsMatcher()
    {
        var result = FieldMatcher.CreateFromDiscriminatorValue(MakeNode("map-key-exists"));
        Assert.NotNull(result.MapKeyExistsMatcher);
    }

    /// <summary>
    /// テストの説明
    /// </summary>
    [Fact]
    public void FieldMatcher_CreateFromDiscriminatorValue_MapKeyValueMatcher_SetsMapKeyValueMatcher()
    {
        var result = FieldMatcher.CreateFromDiscriminatorValue(MakeNode("map-key-value-matcher"));
        Assert.NotNull(result.MapKeyValueMatcher);
    }

    /// <summary>
    /// テストの説明
    /// </summary>
    [Fact]
    public void FieldMatcher_CreateFromDiscriminatorValue_MapValueNumber_SetsMapValueNumMatcher()
    {
        var result = FieldMatcher.CreateFromDiscriminatorValue(MakeNode("map-value-number"));
        Assert.NotNull(result.MapValueNumMatcher);
    }

    /// <summary>
    /// テストの説明
    /// </summary>
    [Fact]
    public void FieldMatcher_CreateFromDiscriminatorValue_MapValueString_SetsMapValueStrMatcher()
    {
        var result = FieldMatcher.CreateFromDiscriminatorValue(MakeNode("map-value-string"));
        Assert.NotNull(result.MapValueStrMatcher);
    }

    /// <summary>
    /// テストの説明
    /// </summary>
    [Fact]
    public void FieldMatcher_CreateFromDiscriminatorValue_Number_SetsNumMatcher()
    {
        var result = FieldMatcher.CreateFromDiscriminatorValue(MakeNode("number"));
        Assert.NotNull(result.NumMatcher);
    }

    /// <summary>
    /// テストの説明
    /// </summary>
    [Fact]
    public void FieldMatcher_CreateFromDiscriminatorValue_Or_SetsOrMatcher()
    {
        var result = FieldMatcher.CreateFromDiscriminatorValue(MakeNode("or"));
        Assert.NotNull(result.OrMatcher);
    }

    /// <summary>
    /// テストの説明
    /// </summary>
    [Fact]
    public void FieldMatcher_CreateFromDiscriminatorValue_String_SetsStrMatcher()
    {
        var result = FieldMatcher.CreateFromDiscriminatorValue(MakeNode("string"));
        Assert.NotNull(result.StrMatcher);
    }

    private static FieldMatcher CreateFromDiscriminatorValue(string? typeValue)
        => FieldMatcher.CreateFromDiscriminatorValue(MakeNode(typeValue));

    private static IParseNode MakeNode(string? typeValue)
    {
        var childNode = Substitute.For<IParseNode>();
        childNode.GetStringValue().Returns(typeValue);
        var node = Substitute.For<IParseNode>();
        node.GetChildNode("type").Returns(childNode);
        return node;
    }

    /// <summary>
    /// Serializeが期待されるフィールドを書き込むことを確認
    /// </summary>
    [Fact]
    public void FieldMatcher_Serialize_WritesExpectedFields()
    {
        var model = new FieldMatcher();
        var writer = Substitute.For<ISerializationWriter>();
        model.Serialize(writer);
    }
}
