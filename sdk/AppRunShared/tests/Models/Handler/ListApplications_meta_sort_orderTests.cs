using System.Reflection;
using System.Runtime.Serialization;
using Sacloud.Sdk.AppRunShared.Models.Handler;

namespace Sacloud.Sdk.AppRunShared.Tests.Models.Handler;

/// <summary>
/// ListApplications_meta_sort_orderモデルのテスト
/// </summary>
public class ListApplications_meta_sort_orderTests
{
    /// <summary>
    /// Enum ListApplications_meta_sort_orderが期待される値を持つことを確認
    /// </summary>
    [Fact]
    public void ListApplications_meta_sort_order_HasExpectedValues()
    {
        var values = Enum.GetValues<ListApplications_meta_sort_order>();
        Assert.Contains(ListApplications_meta_sort_order.Asc, values);
        Assert.Contains(ListApplications_meta_sort_order.Desc, values);
    }

    /// <summary>
    /// Ascが正しいEnumMember属性を持つことを確認
    /// </summary>
    [Fact]
    public void ListApplications_meta_sort_order_Asc_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(ListApplications_meta_sort_order).GetField(nameof(ListApplications_meta_sort_order.Asc));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("asc", attribute.Value);
    }

    /// <summary>
    /// Descが正しいEnumMember属性を持つことを確認
    /// </summary>
    [Fact]
    public void ListApplications_meta_sort_order_Desc_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(ListApplications_meta_sort_order).GetField(nameof(ListApplications_meta_sort_order.Desc));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("desc", attribute.Value);
    }
}
