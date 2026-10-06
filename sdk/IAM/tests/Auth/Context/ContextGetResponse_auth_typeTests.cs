using System.Reflection;
using System.Runtime.Serialization;
using Sacloud.Sdk.IAM.Auth.Context;

namespace Sacloud.Sdk.IAM.Tests.Auth.Context;

/// <summary>
/// ContextGetResponse_auth_type の単体テスト
/// </summary>
public class ContextGetResponse_auth_typeTests
{
    /// <summary>
    /// Enum 値が正しく定義されていることを確認
    /// </summary>
    [Fact]
    public void EnumValues_AreDefinedCorrectly()
    {
        var apikey = ContextGetResponse_auth_type.Apikey;

        Assert.Equal(ContextGetResponse_auth_type.Apikey, apikey);
    }

    /// <summary>
    /// Enum 値の数が正しいことを確認
    /// </summary>
    [Fact]
    public void EnumValues_HasCorrectCount()
    {
        var values = Enum.GetValues(typeof(ContextGetResponse_auth_type));

        Assert.Equal(2, values.Length);
    }

    /// <summary>
    /// EnumMember 属性が正しく設定されていることを確認: Apikey
    /// </summary>
    [Fact]
    public void ContextGetResponse_auth_type_Apikey_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(ContextGetResponse_auth_type).GetField(nameof(ContextGetResponse_auth_type.Apikey));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("apikey", attribute.Value);
    }

    /// <summary>
    /// EnumMember 属性が正しく設定されていることを確認: Service_principal
    /// </summary>
    [Fact]
    public void ContextGetResponse_auth_type_Service_principal_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(ContextGetResponse_auth_type).GetField(nameof(ContextGetResponse_auth_type.Service_principal));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("service_principal", attribute.Value);
    }
}
