using Sacloud.Sdk.Shared.Validation;
using System;

namespace Sacloud.Sdk.Shared.Tests.Validation;

/// <summary>
/// PathParameterValidationOptionsのテスト
/// </summary>
public class PathParameterValidationOptionsTests
{
    /// <summary>
    /// コンストラクタが有効なdisplayNameで正常にインスタンスを作成することを確認
    /// </summary>
    [Fact]
    public void Constructor_WithValidDisplayName_CreatesInstance()
    {
        var displayName = "テストID";

        var options = new PathParameterValidationOptions(displayName);

        Assert.NotNull(options);
        Assert.Equal(displayName, options.DisplayName);
    }

    /// <summary>
    /// コンストラクタがnullのdisplayNameでArgumentNullExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void Constructor_WithNullDisplayName_ThrowsArgumentNullException()
    {
        string displayName = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => 
            new PathParameterValidationOptions(displayName));

        Assert.Equal("displayName", exception.ParamName);
    }

    /// <summary>
    /// MinLengthプロパティが初期値nullであることを確認
    /// </summary>
    [Fact]
    public void MinLength_DefaultValue_IsNull()
    {
        var options = new PathParameterValidationOptions("テストID");

        Assert.Null(options.MinLength);
    }

    /// <summary>
    /// MinLengthプロパティに値を設定できることを確認
    /// </summary>
    [Fact]
    public void MinLength_CanBeSet()
    {
        var options = new PathParameterValidationOptions("テストID");
        
        options.MinLength = 5;

        Assert.Equal(5, options.MinLength);
    }

    /// <summary>
    /// MinLengthプロパティにnullを設定できることを確認
    /// </summary>
    [Fact]
    public void MinLength_CanBeSetToNull()
    {
        var options = new PathParameterValidationOptions("テストID")
        {
            MinLength = 5
        };

        options.MinLength = null;

        Assert.Null(options.MinLength);
    }

    /// <summary>
    /// MaxLengthプロパティが初期値nullであることを確認
    /// </summary>
    [Fact]
    public void MaxLength_DefaultValue_IsNull()
    {
        var options = new PathParameterValidationOptions("テストID");

        Assert.Null(options.MaxLength);
    }

    /// <summary>
    /// MaxLengthプロパティに値を設定できることを確認
    /// </summary>
    [Fact]
    public void MaxLength_CanBeSet()
    {
        var options = new PathParameterValidationOptions("テストID");

        options.MaxLength = 10;

        Assert.Equal(10, options.MaxLength);
    }

    /// <summary>
    /// MaxLengthプロパティにnullを設定できることを確認
    /// </summary>
    [Fact]
    public void MaxLength_CanBeSetToNull()
    {
        var options = new PathParameterValidationOptions("テストID")
        {
            MaxLength = 10
        };

        options.MaxLength = null;

        Assert.Null(options.MaxLength);
    }

    /// <summary>
    /// Lengthプロパティが初期値nullであることを確認
    /// </summary>
    [Fact]
    public void Length_DefaultValue_IsNull()
    {
        var options = new PathParameterValidationOptions("テストID");

        Assert.Null(options.Length);
    }

    /// <summary>
    /// Lengthプロパティに値を設定できることを確認
    /// </summary>
    [Fact]
    public void Length_CanBeSet()
    {
        var options = new PathParameterValidationOptions("テストID");

        options.Length = 12;

        Assert.Equal(12, options.Length);
    }

    /// <summary>
    /// Lengthプロパティにnullを設定できることを確認
    /// </summary>
    [Fact]
    public void Length_CanBeSetToNull()
    {
        var options = new PathParameterValidationOptions("テストID")
        {
            Length = 12
        };

        options.Length = null;

        Assert.Null(options.Length);
    }

    /// <summary>
    /// RegexPatternプロパティが初期値nullであることを確認
    /// </summary>
    [Fact]
    public void RegexPattern_DefaultValue_IsNull()
    {
        var options = new PathParameterValidationOptions("テストID");

        Assert.Null(options.RegexPattern);
    }

    /// <summary>
    /// RegexPatternプロパティに値を設定できることを確認
    /// </summary>
    [Fact]
    public void RegexPattern_CanBeSet()
    {
        var options = new PathParameterValidationOptions("テストID");
        var pattern = "^[a-z0-9]+$";

        options.RegexPattern = pattern;

        Assert.Equal(pattern, options.RegexPattern);
    }

    /// <summary>
    /// RegexPatternプロパティにnullを設定できることを確認
    /// </summary>
    [Fact]
    public void RegexPattern_CanBeSetToNull()
    {
        var options = new PathParameterValidationOptions("テストID")
        {
            RegexPattern = "^[a-z0-9]+$"
        };

        options.RegexPattern = null!;

        Assert.Null(options.RegexPattern);
    }

    /// <summary>
    /// オブジェクト初期化子で全てのプロパティを設定できることを確認
    /// </summary>
    [Fact]
    public void ObjectInitializer_WithAllProperties_SetsAllValues()
    {
        var options = new PathParameterValidationOptions("テストID")
        {
            MinLength = 5,
            MaxLength = 20,
            Length = 12,
            RegexPattern = "^[0-9]+$"
        };

        Assert.Equal("テストID", options.DisplayName);
        Assert.Equal(5, options.MinLength);
        Assert.Equal(20, options.MaxLength);
        Assert.Equal(12, options.Length);
        Assert.Equal("^[0-9]+$", options.RegexPattern);
    }

    /// <summary>
    /// MinLengthとMaxLengthの両方を設定できることを確認
    /// </summary>
    [Fact]
    public void MinLengthAndMaxLength_CanBeSetTogether()
    {
        var options = new PathParameterValidationOptions("テストID")
        {
            MinLength = 5,
            MaxLength = 10
        };

        Assert.Equal(5, options.MinLength);
        Assert.Equal(10, options.MaxLength);
    }

    /// <summary>
    /// LengthとMinLength/MaxLengthの両方を設定できることを確認（Lengthが優先されることを想定）
    /// </summary>
    [Fact]
    public void Length_CanBeSetWithMinMaxLength()
    {
        var options = new PathParameterValidationOptions("テストID")
        {
            MinLength = 5,
            MaxLength = 10,
            Length = 8
        };

        Assert.Equal(5, options.MinLength);
        Assert.Equal(10, options.MaxLength);
        Assert.Equal(8, options.Length);
    }

    /// <summary>
    /// 複数のバリデーションオプションを組み合わせて設定できることを確認
    /// </summary>
    [Fact]
    public void MultipleValidationOptions_CanBeSetTogether()
    {
        var options = new PathParameterValidationOptions("テストID")
        {
            MinLength = 3,
            MaxLength = 50,
            RegexPattern = "^app-[a-z0-9]+$"
        };

        Assert.Equal("テストID", options.DisplayName);
        Assert.Equal(3, options.MinLength);
        Assert.Equal(50, options.MaxLength);
        Assert.Equal("^app-[a-z0-9]+$", options.RegexPattern);
        Assert.Null(options.Length);
    }

    /// <summary>
    /// MaxValueプロパティが初期値nullであることを確認
    /// </summary>
    [Fact]
    public void MaxValue_DefaultValue_IsNull()
    {
        var options = new PathParameterValidationOptions("テストID");

        Assert.Null(options.MaxValue);
    }

    /// <summary>
    /// MaxValueプロパティに値を設定できることを確認
    /// </summary>
    [Fact]
    public void MaxValue_CanBeSet()
    {
        var options = new PathParameterValidationOptions("テストID");

        options.MaxValue = 100;

        Assert.Equal(100, options.MaxValue);
    }

    /// <summary>
    /// MaxValueプロパティにnullを設定できることを確認
    /// </summary>
    [Fact]
    public void MaxValue_CanBeSetToNull()
    {
        var options = new PathParameterValidationOptions("テストID")
        {
            MaxValue = 100
        };

        options.MaxValue = null;

        Assert.Null(options.MaxValue);
    }

    /// <summary>
    /// MinValueプロパティが初期値nullであることを確認
    /// </summary>
    [Fact]
    public void MinValue_DefaultValue_IsNull()
    {
        var options = new PathParameterValidationOptions("テストID");

        Assert.Null(options.MinValue);
    }

    /// <summary>
    /// MinValueプロパティに値を設定できることを確認
    /// </summary>
    [Fact]
    public void MinValue_CanBeSet()
    {
        var options = new PathParameterValidationOptions("テストID");

        options.MinValue = 10;

        Assert.Equal(10, options.MinValue);
    }

    /// <summary>
    /// MinValueプロパティにnullを設定できることを確認
    /// </summary>
    [Fact]
    public void MinValue_CanBeSetToNull()
    {
        var options = new PathParameterValidationOptions("テストID")
        {
            MinValue = 10
        };

        options.MinValue = null;

        Assert.Null(options.MinValue);
    }

    /// <summary>
    /// IsGuidプロパティが初期値falseであることを確認
    /// </summary>
    [Fact]
    public void IsGuid_DefaultValue_IsFalse()
    {
        var options = new PathParameterValidationOptions("テストID");

        Assert.False(options.IsGuid);
    }

    /// <summary>
    /// IsGuidプロパティに値を設定できることを確認
    /// </summary>
    [Fact]
    public void IsGuid_CanBeSet()
    {
        var options = new PathParameterValidationOptions("テストID");

        options.IsGuid = true;

        Assert.True(options.IsGuid);
    }
}
