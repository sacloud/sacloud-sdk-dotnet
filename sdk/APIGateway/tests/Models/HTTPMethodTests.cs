using System.Reflection;
using System.Runtime.Serialization;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// HTTPMethodのテスト
/// </summary>
public class HTTPMethodTests
{
    /// <summary>
    /// Enum値が正しく定義されていることを確認
    /// </summary>
    [Fact]
    public void EnumValues_AreDefinedCorrectly()
    {
        Assert.Equal(HTTPMethod.GET, HTTPMethod.GET);
        Assert.Equal(HTTPMethod.POST, HTTPMethod.POST);
        Assert.Equal(HTTPMethod.PUT, HTTPMethod.PUT);
        Assert.Equal(HTTPMethod.DELETE, HTTPMethod.DELETE);
        Assert.Equal(HTTPMethod.PATCH, HTTPMethod.PATCH);
        Assert.Equal(HTTPMethod.OPTIONS, HTTPMethod.OPTIONS);
        Assert.Equal(HTTPMethod.HEAD, HTTPMethod.HEAD);
        Assert.Equal(HTTPMethod.CONNECT, HTTPMethod.CONNECT);
        Assert.Equal(HTTPMethod.TRACE, HTTPMethod.TRACE);
    }

    /// <summary>
    /// Enum値の数が正しいことを確認
    /// </summary>
    [Fact]
    public void EnumValues_HasCorrectCount()
    {
        var values = Enum.GetValues<HTTPMethod>();

        Assert.Equal(9, values.Length);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: GET
    /// </summary>
    [Fact]
    public void HTTPMethod_GET_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(HTTPMethod).GetField(nameof(HTTPMethod.GET));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("GET", attribute.Value);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: POST
    /// </summary>
    [Fact]
    public void HTTPMethod_POST_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(HTTPMethod).GetField(nameof(HTTPMethod.POST));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("POST", attribute.Value);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: PUT
    /// </summary>
    [Fact]
    public void HTTPMethod_PUT_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(HTTPMethod).GetField(nameof(HTTPMethod.PUT));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("PUT", attribute.Value);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: DELETE
    /// </summary>
    [Fact]
    public void HTTPMethod_DELETE_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(HTTPMethod).GetField(nameof(HTTPMethod.DELETE));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("DELETE", attribute.Value);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: PATCH
    /// </summary>
    [Fact]
    public void HTTPMethod_PATCH_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(HTTPMethod).GetField(nameof(HTTPMethod.PATCH));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("PATCH", attribute.Value);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: OPTIONS
    /// </summary>
    [Fact]
    public void HTTPMethod_OPTIONS_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(HTTPMethod).GetField(nameof(HTTPMethod.OPTIONS));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("OPTIONS", attribute.Value);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: HEAD
    /// </summary>
    [Fact]
    public void HTTPMethod_HEAD_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(HTTPMethod).GetField(nameof(HTTPMethod.HEAD));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("HEAD", attribute.Value);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: CONNECT
    /// </summary>
    [Fact]
    public void HTTPMethod_CONNECT_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(HTTPMethod).GetField(nameof(HTTPMethod.CONNECT));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("CONNECT", attribute.Value);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: TRACE
    /// </summary>
    [Fact]
    public void HTTPMethod_TRACE_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(HTTPMethod).GetField(nameof(HTTPMethod.TRACE));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("TRACE", attribute.Value);
    }
}
