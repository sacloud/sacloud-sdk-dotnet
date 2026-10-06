using System.Reflection;
using System.Runtime.Serialization;
using Sacloud.Sdk.ObjectStorage.Models.Model;

namespace Sacloud.Sdk.ObjectStorage.Tests.Models.Model;

/// <summary>
/// Replication_config_status Enumのテスト
/// </summary>
public class Replication_config_statusTests
{

    /// <summary>
    /// すべてのEnum値が定義されていることを確認
    /// </summary>
    [Fact]
    public void Replication_config_status_HasExpectedValues()
    {
        var values = Enum.GetValues<Replication_config_status>();
        Assert.Contains(Replication_config_status.Creating, values);
        Assert.Contains(Replication_config_status.Created, values);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Creating
    /// </summary>
    [Fact]
    public void Replication_config_status_Creating_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(Replication_config_status).GetField(nameof(Replication_config_status.Creating));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("creating", attribute?.Value);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: Created
    /// </summary>
    [Fact]
    public void Replication_config_status_Created_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(Replication_config_status).GetField(nameof(Replication_config_status.Created));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("created", attribute?.Value);
    }
}
