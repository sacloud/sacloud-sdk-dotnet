using System.Reflection;
using System.Runtime.Serialization;
using Sacloud.Sdk.APIGateway.Models;

namespace Sacloud.Sdk.APIGateway.Tests.Models;

/// <summary>
/// Jwt_algorithmのテスト
/// </summary>
public class Jwt_algorithmTests
{
    /// <summary>
    /// Enum値が正しく定義されていることを確認
    /// </summary>
    [Fact]
    public void EnumValues_AreDefinedCorrectly()
    {
        Assert.Equal(Jwt_algorithm.HS256, Jwt_algorithm.HS256);
        Assert.Equal(Jwt_algorithm.HS384, Jwt_algorithm.HS384);
        Assert.Equal(Jwt_algorithm.HS512, Jwt_algorithm.HS512);
    }

    /// <summary>
    /// Enum値の数が正しいことを確認
    /// </summary>
    [Fact]
    public void EnumValues_HasCorrectCount()
    {
        var values = Enum.GetValues<Jwt_algorithm>();

        Assert.Equal(3, values.Length);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: HS256
    /// </summary>
    [Fact]
    public void Jwt_algorithm_HS256_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(Jwt_algorithm).GetField(nameof(Jwt_algorithm.HS256));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("HS256", attribute.Value);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: HS384
    /// </summary>
    [Fact]
    public void Jwt_algorithm_HS384_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(Jwt_algorithm).GetField(nameof(Jwt_algorithm.HS384));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("HS384", attribute.Value);
    }

    /// <summary>
    /// EnumMember属性が正しく設定されていることを確認: HS512
    /// </summary>
    [Fact]
    public void Jwt_algorithm_HS512_HasCorrectEnumMemberAttribute()
    {
        var field = typeof(Jwt_algorithm).GetField(nameof(Jwt_algorithm.HS512));
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("HS512", attribute.Value);
    }
}
