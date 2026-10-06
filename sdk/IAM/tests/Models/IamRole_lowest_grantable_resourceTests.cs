using System.Reflection;
using System.Runtime.Serialization;
using Sacloud.Sdk.IAM.Models;

namespace Sacloud.Sdk.IAM.Tests.Models;

/// <summary>
/// IamRole_lowest_grantable_resource の単体テスト
/// </summary>
public class IamRole_lowest_grantable_resourceTests
{
    /// <summary>
    /// Enum 値が正しく定義されていることを確認
    /// </summary>
    [Fact]
    public void EnumValues_AreDefinedCorrectly()
    {
        var org = IamRole_lowest_grantable_resource.Organization;

        Assert.Equal(IamRole_lowest_grantable_resource.Organization, org);
    }

    /// <summary>
    /// Enum 値の数が正しいことを確認
    /// </summary>
    [Fact]
    public void EnumValues_HasCorrectCount()
    {
        var values = Enum.GetValues(typeof(IamRole_lowest_grantable_resource));

        Assert.Equal(3, values.Length);
    }

    /// <summary>
    /// EnumMember 属性が正しく設定されていることを確認: Organization
    /// </summary>
    [Fact]
    public void IamRole_lowest_grantable_resource_Organization_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(IamRole_lowest_grantable_resource).GetField(nameof(IamRole_lowest_grantable_resource.Organization));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("organization", attribute.Value);
    }

    /// <summary>
    /// EnumMember 属性が正しく設定されていることを確認: Folder
    /// </summary>
    [Fact]
    public void IamRole_lowest_grantable_resource_Folder_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(IamRole_lowest_grantable_resource).GetField(nameof(IamRole_lowest_grantable_resource.Folder));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("folder", attribute.Value);
    }

    /// <summary>
    /// EnumMember 属性が正しく設定されていることを確認: Project
    /// </summary>
    [Fact]
    public void IamRole_lowest_grantable_resource_Project_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(IamRole_lowest_grantable_resource).GetField(nameof(IamRole_lowest_grantable_resource.Project));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("project", attribute.Value);
    }
}
