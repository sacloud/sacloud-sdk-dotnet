using System.Reflection;
using System.Runtime.Serialization;
using Sacloud.Sdk.AppRunShared.Models.Handler;

namespace Sacloud.Sdk.AppRunShared.Tests.Models.Handler;

/// <summary>
/// ListVersions_meta_sort_orderモデルのテスト
/// </summary>
public class ListVersions_meta_sort_orderTests
{
    /// <summary>
    /// Enum ListVersions_meta_sort_orderが期待される値を持つことを確認
    /// </summary>
    [Fact]
    public void ListVersions_meta_sort_order_HasExpectedValues()
    {
        var values = Enum.GetValues<ListVersions_meta_sort_order>();
        Assert.Contains(ListVersions_meta_sort_order.Asc, values);
        Assert.Contains(ListVersions_meta_sort_order.Desc, values);
    }

    /// <summary>
    /// Ascが正しいEnumMember属性を持つことを確認
    /// </summary>
    [Fact]
    public void ListVersions_meta_sort_order_Asc_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(ListVersions_meta_sort_order).GetField(nameof(ListVersions_meta_sort_order.Asc));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("asc", attribute.Value);
    }

    /// <summary>
    /// Descが正しいEnumMember属性を持つことを確認
    /// </summary>
    [Fact]
    public void ListVersions_meta_sort_order_Desc_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(ListVersions_meta_sort_order).GetField(nameof(ListVersions_meta_sort_order.Desc));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("desc", attribute.Value);
    }
}
