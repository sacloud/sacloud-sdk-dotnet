using Microsoft.Extensions.Logging;
using NSubstitute;
using Sacloud.Sdk.Shared.Validation;
using System.ComponentModel.DataAnnotations;

namespace Sacloud.Sdk.Shared.Tests.Validation;

/// <summary>
/// RequestParameterValidatorのテスト
/// </summary>
public class RequestParameterValidatorTests
{
    private readonly ILogger _mockLogger = Substitute.For<ILogger>();

    /// <summary>
    /// ValidatePathParametersが有効なパラメータで検証をパスすることを確認
    /// </summary>
    [Fact]
    public void ValidatePathParameters_WithValidParameters_PassesValidation()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "projectId", "123456789012" },
            { "applicationId", "app-001" }
        };
        var targetPathParametersInfo = new Dictionary<string, string>
        {
            { "projectId", "プロジェクトID" },
            { "applicationId", "アプリケーションID" }
        };

        var exception = Record.Exception(() => 
            RequestParameterValidator.ValidatePathParameters(pathParameters, targetPathParametersInfo, _mockLogger));

        Assert.Null(exception);
    }

    /// <summary>
    /// ValidatePathParametersがパラメータ名が存在しない場合にValidationExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void ValidatePathParameters_WithMissingParameterName_ThrowsValidationException()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "projectId", "123456789012" }
        };
        var targetPathParametersInfo = new Dictionary<string, string>
        {
            { "projectId", "プロジェクトID" },
            { "applicationId", "アプリケーションID" }
        };

        var exception = Assert.Throws<ValidationException>(() =>
            RequestParameterValidator.ValidatePathParameters(pathParameters, targetPathParametersInfo, _mockLogger));

        Assert.Equal("アプリケーションIDを入力してください。", exception.Message);
    }

    /// <summary>
    /// ValidatePathParametersがnull値のパラメータでValidationExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void ValidatePathParameters_WithNullParameterValue_ThrowsValidationException()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "projectId", null! }
        };
        var targetPathParametersInfo = new Dictionary<string, string>
        {
            { "projectId", "プロジェクトID" }
        };

        var exception = Assert.Throws<ValidationException>(() =>
            RequestParameterValidator.ValidatePathParameters(pathParameters, targetPathParametersInfo, _mockLogger));

        Assert.Equal("プロジェクトIDを入力してください。", exception.Message);
    }

    /// <summary>
    /// ValidatePathParametersが空文字列のパラメータでValidationExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void ValidatePathParameters_WithEmptyParameterValue_ThrowsValidationException()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "projectId", string.Empty }
        };
        var targetPathParametersInfo = new Dictionary<string, string>
        {
            { "projectId", "プロジェクトID" }
        };

        var exception = Assert.Throws<ValidationException>(() =>
            RequestParameterValidator.ValidatePathParameters(pathParameters, targetPathParametersInfo, _mockLogger));

        Assert.Equal("プロジェクトIDを入力してください。", exception.Message);
    }

    /// <summary>
    /// ValidatePathParametersが空白文字のみのパラメータでValidationExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void ValidatePathParameters_WithWhitespaceParameterValue_ThrowsValidationException()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "projectId", "   " }
        };
        var targetPathParametersInfo = new Dictionary<string, string>
        {
            { "projectId", "プロジェクトID" }
        };

        var exception = Assert.Throws<ValidationException>(() =>
            RequestParameterValidator.ValidatePathParameters(pathParameters, targetPathParametersInfo, _mockLogger));

        Assert.Equal("プロジェクトIDを入力してください。", exception.Message);
    }

    /// <summary>
    /// ValidatePathParametersがタブ文字のみのパラメータでValidationExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void ValidatePathParameters_WithTabParameterValue_ThrowsValidationException()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "projectId", "\t" }
        };
        var targetPathParametersInfo = new Dictionary<string, string>
        {
            { "projectId", "プロジェクトID" }
        };

        var exception = Assert.Throws<ValidationException>(() =>
            RequestParameterValidator.ValidatePathParameters(pathParameters, targetPathParametersInfo, _mockLogger));

        Assert.Equal("プロジェクトIDを入力してください。", exception.Message);
    }

    /// <summary>
    /// ValidatePathParametersが改行文字のみのパラメータでValidationExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void ValidatePathParameters_WithNewlineParameterValue_ThrowsValidationException()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "projectId", "\n" }
        };
        var targetPathParametersInfo = new Dictionary<string, string>
        {
            { "projectId", "プロジェクトID" }
        };

        var exception = Assert.Throws<ValidationException>(() =>
            RequestParameterValidator.ValidatePathParameters(pathParameters, targetPathParametersInfo, _mockLogger));

        Assert.Equal("プロジェクトIDを入力してください。", exception.Message);
    }

    /// <summary>
    /// ValidatePathParametersが複数の有効なパラメータで検証をパスすることを確認
    /// </summary>
    [Fact]
    public void ValidatePathParameters_WithMultipleValidParameters_PassesValidation()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "projectId", "123456789012" },
            { "applicationId", "app-001" },
            { "versionId", "v1.0.0" }
        };
        var targetPathParametersInfo = new Dictionary<string, string>
        {
            { "projectId", "プロジェクトID" },
            { "applicationId", "アプリケーションID" },
            { "versionId", "バージョンID" }
        };

        var exception = Record.Exception(() =>
            RequestParameterValidator.ValidatePathParameters(pathParameters, targetPathParametersInfo, _mockLogger));

        Assert.Null(exception);
    }

    /// <summary>
    /// ValidatePathParametersが複数パラメータの一部が無効な場合に最初のエラーで例外をスローすることを確認
    /// </summary>
    [Fact]
    public void ValidatePathParameters_WithPartiallyInvalidParameters_ThrowsValidationExceptionForFirstError()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "projectId", "123456789012" },
            { "applicationId", "" },
            { "versionId", "v1.0.0" }
        };
        var targetPathParametersInfo = new Dictionary<string, string>
        {
            { "projectId", "プロジェクトID" },
            { "applicationId", "アプリケーションID" },
            { "versionId", "バージョンID" }
        };

        var exception = Assert.Throws<ValidationException>(() =>
            RequestParameterValidator.ValidatePathParameters(pathParameters, targetPathParametersInfo, _mockLogger));

        Assert.Equal("アプリケーションIDを入力してください。", exception.Message);
    }

    /// <summary>
    /// ValidatePathParametersがtargetPathParametersInfoが空の場合に例外をスローしないことを確認
    /// </summary>
    [Fact]
    public void ValidatePathParameters_WithEmptyTargetInfo_PassesValidation()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "projectId", "123456789012" }
        };
        var targetPathParametersInfo = new Dictionary<string, string>();

        var exception = Record.Exception(() =>
            RequestParameterValidator.ValidatePathParameters(pathParameters, targetPathParametersInfo, _mockLogger));

        Assert.Null(exception);
    }

    /// <summary>
    /// ValidatePathParametersがpathParametersに余分なパラメータが存在する場合に例外をスローしないことを確認
    /// </summary>
    [Fact]
    public void ValidatePathParameters_WithExtraParameters_PassesValidation()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "projectId", "123456789012" },
            { "applicationId", "app-001" },
            { "extraParam", "extra-value" }
        };
        var targetPathParametersInfo = new Dictionary<string, string>
        {
            { "projectId", "プロジェクトID" }
        };

        var exception = Record.Exception(() =>
            RequestParameterValidator.ValidatePathParameters(pathParameters, targetPathParametersInfo, _mockLogger));

        Assert.Null(exception);
    }

    /// <summary>
    /// ValidatePathParametersがValidationException時にWarningログを出力することを確認
    /// </summary>
    [Fact]
    public void ValidatePathParameters_OnValidationException_LogsWarning()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "projectId", "" }
        };
        var targetPathParametersInfo = new Dictionary<string, string>
        {
            { "projectId", "プロジェクトID" }
        };

        Assert.Throws<ValidationException>(() =>
            RequestParameterValidator.ValidatePathParameters(pathParameters, targetPathParametersInfo, _mockLogger));

        _mockLogger.Received(1).Log(
            LogLevel.Warning,
            Arg.Any<EventId>(),
            Arg.Is<object>(o => o.ToString() == "プロジェクトIDを入力してください。"),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// ValidatePathParametersがパラメータ不存在時にWarningログを出力することを確認
    /// </summary>
    [Fact]
    public void ValidatePathParameters_OnMissingParameter_LogsWarning()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "projectId", "123456789012" }
        };
        var targetPathParametersInfo = new Dictionary<string, string>
        {
            { "applicationId", "アプリケーションID" }
        };

        Assert.Throws<ValidationException>(() =>
            RequestParameterValidator.ValidatePathParameters(pathParameters, targetPathParametersInfo, _mockLogger));

        _mockLogger.Received(1).Log(
            LogLevel.Warning,
            Arg.Any<EventId>(),
            Arg.Is<object>(o => o.ToString() == "アプリケーションIDを入力してください。"),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// ValidatePathParameters(PathParameterValidationOptions版)がLength指定時に正確な文字数チェックを行うことを確認
    /// </summary>
    [Fact]
    public void ValidatePathParameters_WithLengthOption_ValidatesExactLength()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "projectId", "123456789012" } // 12文字
        };
        var validationOptions = new Dictionary<string, PathParameterValidationOptions>
        {
            { "projectId", new PathParameterValidationOptions("プロジェクトID") { Length = 12 } }
        };

        var exception = Record.Exception(() =>
            RequestParameterValidator.ValidatePathParameters(pathParameters, validationOptions, _mockLogger));

        Assert.Null(exception);
    }

    /// <summary>
    /// ValidatePathParameters(PathParameterValidationOptions版)がLength指定時に文字数が異なる場合にValidationExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void ValidatePathParameters_WithLengthOption_ThrowsWhenLengthMismatch()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "projectId", "12345" } // 5文字
        };
        var validationOptions = new Dictionary<string, PathParameterValidationOptions>
        {
            { "projectId", new PathParameterValidationOptions("プロジェクトID") { Length = 12 } }
        };

        var exception = Assert.Throws<ValidationException>(() =>
            RequestParameterValidator.ValidatePathParameters(pathParameters, validationOptions, _mockLogger));

        Assert.Equal("プロジェクトIDは12文字で入力してください。", exception.Message);
    }

    /// <summary>
    /// ValidatePathParameters(PathParameterValidationOptions版)がMinLength指定時に最小文字数チェックを行うことを確認
    /// </summary>
    [Fact]
    public void ValidatePathParameters_WithMinLengthOption_ValidatesMinimumLength()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "projectId", "12345" } // 5文字
        };
        var validationOptions = new Dictionary<string, PathParameterValidationOptions>
        {
            { "projectId", new PathParameterValidationOptions("プロジェクトID") { MinLength = 5 } }
        };

        var exception = Record.Exception(() =>
            RequestParameterValidator.ValidatePathParameters(pathParameters, validationOptions, _mockLogger));

        Assert.Null(exception);
    }

    /// <summary>
    /// ValidatePathParameters(PathParameterValidationOptions版)がMinLength違反時にValidationExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void ValidatePathParameters_WithMinLengthOption_ThrowsWhenTooShort()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "projectId", "123" } // 3文字
        };
        var validationOptions = new Dictionary<string, PathParameterValidationOptions>
        {
            { "projectId", new PathParameterValidationOptions("プロジェクトID") { MinLength = 5 } }
        };

        var exception = Assert.Throws<ValidationException>(() =>
            RequestParameterValidator.ValidatePathParameters(pathParameters, validationOptions, _mockLogger));

        Assert.Equal("プロジェクトIDは5文字以上で入力してください。", exception.Message);
    }

    /// <summary>
    /// ValidatePathParameters(PathParameterValidationOptions版)がMaxLength指定時に最大文字数チェックを行うことを確認
    /// </summary>
    [Fact]
    public void ValidatePathParameters_WithMaxLengthOption_ValidatesMaximumLength()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "projectId", "12345" } // 5文字
        };
        var validationOptions = new Dictionary<string, PathParameterValidationOptions>
        {
            { "projectId", new PathParameterValidationOptions("プロジェクトID") { MaxLength = 10 } }
        };

        var exception = Record.Exception(() =>
            RequestParameterValidator.ValidatePathParameters(pathParameters, validationOptions, _mockLogger));

        Assert.Null(exception);
    }

    /// <summary>
    /// ValidatePathParameters(PathParameterValidationOptions版)がMaxLength違反時にValidationExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void ValidatePathParameters_WithMaxLengthOption_ThrowsWhenTooLong()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "projectId", "12345678901234567890" } // 20文字
        };
        var validationOptions = new Dictionary<string, PathParameterValidationOptions>
        {
            { "projectId", new PathParameterValidationOptions("プロジェクトID") { MaxLength = 10 } }
        };

        var exception = Assert.Throws<ValidationException>(() =>
            RequestParameterValidator.ValidatePathParameters(pathParameters, validationOptions, _mockLogger));

        Assert.Equal("プロジェクトIDは10文字以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// ValidatePathParameters(PathParameterValidationOptions版)がMinLengthとMaxLengthの範囲内で検証をパスすることを確認
    /// </summary>
    [Fact]
    public void ValidatePathParameters_WithMinMaxLengthOptions_ValidatesRange()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "projectId", "1234567" } // 7文字
        };
        var validationOptions = new Dictionary<string, PathParameterValidationOptions>
        {
            { "projectId", new PathParameterValidationOptions("プロジェクトID") { MinLength = 5, MaxLength = 10 } }
        };

        var exception = Record.Exception(() =>
            RequestParameterValidator.ValidatePathParameters(pathParameters, validationOptions, _mockLogger));

        Assert.Null(exception);
    }

    /// <summary>
    /// ValidatePathParameters(PathParameterValidationOptions版)がRegexPattern指定時にパターンマッチングを行うことを確認
    /// </summary>
    [Fact]
    public void ValidatePathParameters_WithRegexPattern_ValidatesPattern()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "projectId", "123456789012" } // 数字のみ12文字
        };
        var validationOptions = new Dictionary<string, PathParameterValidationOptions>
        {
            { "projectId", new PathParameterValidationOptions("プロジェクトID") { RegexPattern = @"^\d{12}$" } }
        };

        var exception = Record.Exception(() =>
            RequestParameterValidator.ValidatePathParameters(pathParameters, validationOptions, _mockLogger));

        Assert.Null(exception);
    }

    /// <summary>
    /// ValidatePathParameters(PathParameterValidationOptions版)がRegexPattern違反時にValidationExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void ValidatePathParameters_WithRegexPattern_ThrowsWhenPatternMismatch()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "projectId", "abc123" } // 数字以外を含む
        };
        var validationOptions = new Dictionary<string, PathParameterValidationOptions>
        {
            { "projectId", new PathParameterValidationOptions("プロジェクトID") { RegexPattern = @"^\d+$" } }
        };

        var exception = Assert.Throws<ValidationException>(() =>
            RequestParameterValidator.ValidatePathParameters(pathParameters, validationOptions, _mockLogger));

        Assert.Equal("プロジェクトIDの形式が正しくありません。", exception.Message);
    }

    /// <summary>
    /// ValidatePathParameters(PathParameterValidationOptions版)が複数のバリデーションオプションを組み合わせて検証することを確認
    /// </summary>
    [Fact]
    public void ValidatePathParameters_WithMultipleValidationOptions_ValidatesAll()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "projectId", "123456789012" } // 数字のみ12文字
        };
        var validationOptions = new Dictionary<string, PathParameterValidationOptions>
        {
            {
                "projectId",
                new PathParameterValidationOptions("プロジェクトID")
                {
                    Length = 12,
                    RegexPattern = @"^\d+$"
                }
            }
        };

        var exception = Record.Exception(() =>
            RequestParameterValidator.ValidatePathParameters(pathParameters, validationOptions, _mockLogger));

        Assert.Null(exception);
    }

    /// <summary>
    /// ValidatePathParameters(PathParameterValidationOptions版)が複数のバリデーションエラーを結合して例外メッセージに含めることを確認
    /// </summary>
    [Fact]
    public void ValidatePathParameters_WithMultipleValidationErrors_CombinesErrorMessages()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "projectId", "abc" }, // 長さ不足&パターン不一致
            { "applicationId", "" } // 空文字
        };
        var validationOptions = new Dictionary<string, PathParameterValidationOptions>
        {
            { "projectId", new PathParameterValidationOptions("プロジェクトID") { MinLength = 12, RegexPattern = @"^\d+$" } },
            { "applicationId", new PathParameterValidationOptions("アプリケーションID") }
        };

        var exception = Assert.Throws<ValidationException>(() =>
            RequestParameterValidator.ValidatePathParameters(pathParameters, validationOptions, _mockLogger));

        Assert.Contains("プロジェクトIDは12文字以上で入力してください。", exception.Message);
        Assert.Contains("プロジェクトIDの形式が正しくありません。", exception.Message);
        Assert.Contains("アプリケーションIDを入力してください。", exception.Message);
    }

    /// <summary>
    /// ValidatePathParameters(PathParameterValidationOptions版)がIsGuidオプションで有効なGUID形式の検証をパスすることを確認
    /// </summary>
    [Fact]
    public void ValidatePathParameters_WithIsGuidOption_ValidatesValidGuid()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "resourceId", "550e8400-e29b-41d4-a716-446655440000" }
        };
        var validationOptions = new Dictionary<string, PathParameterValidationOptions>
        {
            { "resourceId", new PathParameterValidationOptions("リソースID") { IsGuid = true } }
        };

        var exception = Record.Exception(() =>
            RequestParameterValidator.ValidatePathParameters(pathParameters, validationOptions, _mockLogger));

        Assert.Null(exception);
    }

    /// <summary>
    /// ValidatePathParameters(PathParameterValidationOptions版)がIsGuidオプションで無効なGUID形式でValidationExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void ValidatePathParameters_WithIsGuidOption_ThrowsWhenInvalidGuid()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "resourceId", "invalid-guid-format" }
        };
        var validationOptions = new Dictionary<string, PathParameterValidationOptions>
        {
            { "resourceId", new PathParameterValidationOptions("リソースID") { IsGuid = true } }
        };

        var exception = Assert.Throws<ValidationException>(() =>
            RequestParameterValidator.ValidatePathParameters(pathParameters, validationOptions, _mockLogger));

        Assert.Equal("リソースIDはGUID形式で入力してください。", exception.Message);
    }

    /// <summary>
    /// ValidatePathParameters(PathParameterValidationOptions版)がIsGuidオプションと他のバリデーションを組み合わせて検証することを確認
    /// </summary>
    [Fact]
    public void ValidatePathParameters_WithIsGuidAndOtherOptions_ValidatesAll()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "resourceId", "550e8400-e29b-41d4-a716-446655440000" }
        };
        var validationOptions = new Dictionary<string, PathParameterValidationOptions>
        {
            {
                "resourceId",
                new PathParameterValidationOptions("リソースID")
                {
                    IsGuid = true,
                    Length = 36
                }
            }
        };

        var exception = Record.Exception(() =>
            RequestParameterValidator.ValidatePathParameters(pathParameters, validationOptions, _mockLogger));

        Assert.Null(exception);
    }

    /// <summary>
    /// ValidatePathParameters(PathParameterValidationOptions版)がMaxValueオプションで有効な値の検証をパスすることを確認
    /// </summary>
    [Fact]
    public void ValidatePathParameters_WithMaxValueOption_ValidatesValidValue()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "page", "10" }
        };
        var validationOptions = new Dictionary<string, PathParameterValidationOptions>
        {
            { "page", new PathParameterValidationOptions("ページ番号") { MaxValue = 20 } }
        };

        var exception = Record.Exception(() =>
            RequestParameterValidator.ValidatePathParameters(pathParameters, validationOptions, _mockLogger));

        Assert.Null(exception);
    }

    /// <summary>
    /// ValidatePathParameters(PathParameterValidationOptions版)がMaxValue違反時にValidationExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void ValidatePathParameters_WithMaxValueOption_ThrowsWhenAboveMaximum()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "page", "1000000000000" }
        };
        var validationOptions = new Dictionary<string, PathParameterValidationOptions>
        {
            { "page", new PathParameterValidationOptions("ページ番号") { MaxValue = 999999999999 } }
        };

        var exception = Assert.Throws<ValidationException>(() =>
            RequestParameterValidator.ValidatePathParameters(pathParameters, validationOptions, _mockLogger));

        Assert.Equal("ページ番号は999999999999以下で入力してください。", exception.Message);
    }

    /// <summary>
    /// ValidatePathParameters(PathParameterValidationOptions版)がMaxValueオプションで境界値（最大値と等しい）の検証をパスすることを確認
    /// </summary>
    [Fact]
    public void ValidatePathParameters_WithMaxValueOption_ValidatesBoundaryValue()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "page", "999999999999" }
        };
        var validationOptions = new Dictionary<string, PathParameterValidationOptions>
        {
            { "page", new PathParameterValidationOptions("ページ番号") { MaxValue = 999999999999 } }
        };

        var exception = Record.Exception(() =>
            RequestParameterValidator.ValidatePathParameters(pathParameters, validationOptions, _mockLogger));

        Assert.Null(exception);
    }

    /// <summary>
    /// ValidatePathParameters(PathParameterValidationOptions版)がMinValueとMaxValueの範囲内で検証をパスすることを確認
    /// </summary>
    [Fact]
    public void ValidatePathParameters_WithMinAndMaxValueOptions_ValidatesRange()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "resourceId", "999999999999" }
        };
        var validationOptions = new Dictionary<string, PathParameterValidationOptions>
        {
            { "resourceId", new PathParameterValidationOptions("リソースID") { MinValue = 0, MaxValue = 999999999999 } }
        };

        var exception = Record.Exception(() =>
            RequestParameterValidator.ValidatePathParameters(pathParameters, validationOptions, _mockLogger));

        Assert.Null(exception);
    }

    /// <summary>
    /// ValidatePathParameters(PathParameterValidationOptions版)がMaxValueオプションで数値に変換できない値をスキップすることを確認
    /// </summary>
    [Fact]
    public void ValidatePathParameters_WithMaxValueOption_SkipsNonNumericValue()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "id", "abc" }
        };
        var validationOptions = new Dictionary<string, PathParameterValidationOptions>
        {
            { "id", new PathParameterValidationOptions("ID") { MaxValue = 999999999999 } }
        };

        var exception = Record.Exception(() =>
            RequestParameterValidator.ValidatePathParameters(pathParameters, validationOptions, _mockLogger));

        Assert.Null(exception);
    }

    /// <summary>
    /// ValidatePathParameters(PathParameterValidationOptions版)がMinValueオプションで有効な値の検証をパスすることを確認
    /// </summary>
    [Fact]
    public void ValidatePathParameters_WithMinValueOption_ValidatesValidValue()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "page", "10" }
        };
        var validationOptions = new Dictionary<string, PathParameterValidationOptions>
        {
            { "page", new PathParameterValidationOptions("ページ番号") { MinValue = 1 } }
        };

        var exception = Record.Exception(() =>
            RequestParameterValidator.ValidatePathParameters(pathParameters, validationOptions, _mockLogger));

        Assert.Null(exception);
    }

    /// <summary>
    /// ValidatePathParameters(PathParameterValidationOptions版)がMinValue違反時にValidationExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void ValidatePathParameters_WithMinValueOption_ThrowsWhenBelowMinimum()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "page", "0" }
        };
        var validationOptions = new Dictionary<string, PathParameterValidationOptions>
        {
            { "page", new PathParameterValidationOptions("ページ番号") { MinValue = 1 } }
        };

        var exception = Assert.Throws<ValidationException>(() =>
            RequestParameterValidator.ValidatePathParameters(pathParameters, validationOptions, _mockLogger));

        Assert.Equal("ページ番号は1以上で入力してください。", exception.Message);
    }

    /// <summary>
    /// ValidatePathParameters(PathParameterValidationOptions版)がMinValueオプションで境界値（最小値と等しい）の検証をパスすることを確認
    /// </summary>
    [Fact]
    public void ValidatePathParameters_WithMinValueOption_ValidatesBoundaryValue()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "page", "1" }
        };
        var validationOptions = new Dictionary<string, PathParameterValidationOptions>
        {
            { "page", new PathParameterValidationOptions("ページ番号") { MinValue = 1 } }
        };

        var exception = Record.Exception(() =>
            RequestParameterValidator.ValidatePathParameters(pathParameters, validationOptions, _mockLogger));

        Assert.Null(exception);
    }

    /// <summary>
    /// ValidatePathParameters(PathParameterValidationOptions版)がMinValueオプションで数値に変換できない値をスキップすることを確認
    /// </summary>
    [Fact]
    public void ValidatePathParameters_WithMinValueOption_SkipsNonNumericValue()
    {
        var pathParameters = new Dictionary<string, object>
        {
            { "id", "abc" }
        };
        var validationOptions = new Dictionary<string, PathParameterValidationOptions>
        {
            { "id", new PathParameterValidationOptions("ID") { MinValue = 1 } }
        };

        var exception = Record.Exception(() =>
            RequestParameterValidator.ValidatePathParameters(pathParameters, validationOptions, _mockLogger));

        Assert.Null(exception);
    }

    /// <summary>
    /// ValidateQueryParametersがnullのqueryParametersでバリデーションをスキップすることを確認
    /// </summary>
    [Fact]
    public void ValidateQueryParameters_WithNullQueryParameters_SkipsValidation()
    {
        object? queryParameters = null;

        var exception = Record.Exception(() =>
            RequestParameterValidator.ValidateQueryParameters(queryParameters, _mockLogger));

        Assert.Null(exception);
    }

    /// <summary>
    /// ValidateQueryParametersが有効なqueryParametersで検証をパスすることを確認
    /// </summary>
    [Fact]
    public void ValidateQueryParameters_WithValidQueryParameters_PassesValidation()
    {
        var queryParameters = new ValidTestModel
        {
            Name = "TestName",
            Age = 25
        };

        var exception = Record.Exception(() =>
            RequestParameterValidator.ValidateQueryParameters(queryParameters, _mockLogger));

        Assert.Null(exception);
    }

    /// <summary>
    /// ValidateQueryParametersがRequired属性違反でValidationExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void ValidateQueryParameters_WithRequiredViolation_ThrowsValidationException()
    {
        var queryParameters = new ValidTestModel
        {
            Name = null,
            Age = 25
        };

        var exception = Assert.Throws<ValidationException>(() =>
            RequestParameterValidator.ValidateQueryParameters(queryParameters, _mockLogger));

        Assert.Equal("The Name field is required.", exception.Message);
    }

    /// <summary>
    /// ValidateQueryParametersがRange属性違反でValidationExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void ValidateQueryParameters_WithRangeViolation_ThrowsValidationException()
    {
        var queryParameters = new ValidTestModel
        {
            Name = "TestName",
            Age = 150
        };

        var exception = Assert.Throws<ValidationException>(() =>
            RequestParameterValidator.ValidateQueryParameters(queryParameters, _mockLogger));

        Assert.Equal("The field Age must be between 1 and 100.", exception.Message);
    }

    /// <summary>
    /// ValidateQueryParametersがStringLength属性違反でValidationExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void ValidateQueryParameters_WithStringLengthViolation_ThrowsValidationException()
    {
        var queryParameters = new ValidTestModel
        {
            Name = "ThisIsAVeryLongName",
            Age = 25
        };

        var exception = Assert.Throws<ValidationException>(() =>
            RequestParameterValidator.ValidateQueryParameters(queryParameters, _mockLogger));

        Assert.Equal("The field Name must be a string with a maximum length of 10.", exception.Message);
    }

    /// <summary>
    /// ValidateQueryParametersがネストしたプロパティが全て有効な場合に検証をパスすることを確認
    /// </summary>
    [Fact]
    public void ValidateQueryParameters_WithValidNestedProperties_PassesValidation()
    {
        var queryParameters = new NestedTestModel
        {
            OuterProperty = "OuterValue",
            Nested = new ValidTestModel
            {
                Name = "NestedName",
                Age = 30
            }
        };

        var exception = Record.Exception(() =>
            RequestParameterValidator.ValidateQueryParameters(queryParameters, _mockLogger));

        Assert.Null(exception);
    }

    /// <summary>
    /// ValidateQueryParametersがネストしたプロパティの一部が無効な場合にValidationExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void ValidateQueryParameters_WithInvalidNestedProperty_ThrowsValidationException()
    {
        var queryParameters = new NestedTestModel
        {
            OuterProperty = "OuterValue",
            Nested = new ValidTestModel
            {
                Name = null,
                Age = 30
            }
        };

        var exception = Assert.Throws<ValidationException>(() =>
            RequestParameterValidator.ValidateQueryParameters(queryParameters, _mockLogger));

        Assert.Equal("The Name field is required.", exception.Message);
    }

    /// <summary>
    /// ValidateQueryParametersが3階層のネストしたプロパティでバリデーションを実行することを確認
    /// </summary>
    [Fact]
    public void ValidateQueryParameters_WithThreeLevelNesting_ValidatesAllLevels()
    {
        var queryParameters = new Level1Model
        {
            Level1Property = "Level1",
            Level2 = new Level2Model
            {
                Level2Property = "Level2",
                Level3 = new Level3Model
                {
                    Level3Property = null
                }
            }
        };

        var exception = Assert.Throws<ValidationException>(() =>
            RequestParameterValidator.ValidateQueryParameters(queryParameters, _mockLogger));

        Assert.Equal("The Level3Property field is required.", exception.Message);
    }

    /// <summary>
    /// ValidateQueryParametersがIEnumerable内の有効な要素で検証をパスすることを確認
    /// </summary>
    [Fact]
    public void ValidateQueryParameters_WithValidIEnumerableItems_PassesValidation()
    {
        var queryParameters = new CollectionTestModel
        {
            OuterProperty = "OuterValue",
            Items = new List<ValidTestModel?>
            {
                new ValidTestModel { Name = "Item1", Age = 25 },
                new ValidTestModel { Name = "Item2", Age = 30 }
            }
        };

        var exception = Record.Exception(() =>
            RequestParameterValidator.ValidateQueryParameters(queryParameters, _mockLogger));

        Assert.Null(exception);
    }

    /// <summary>
    /// ValidateQueryParametersがIEnumerable内の無効な要素でValidationExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void ValidateQueryParameters_WithInvalidIEnumerableItem_ThrowsValidationException()
    {
        var queryParameters = new CollectionTestModel
        {
            OuterProperty = "OuterValue",
            Items = new List<ValidTestModel?>
            {
                new ValidTestModel { Name = "Item1", Age = 25 },
                new ValidTestModel { Name = null, Age = 30 }
            }
        };

        var exception = Assert.Throws<ValidationException>(() =>
            RequestParameterValidator.ValidateQueryParameters(queryParameters, _mockLogger));

        Assert.Equal("The Name field is required.", exception.Message);
    }

    /// <summary>
    /// ValidateQueryParametersが基本型プロパティを再帰対象外とすることを確認
    /// </summary>
    [Fact]
    public void ValidateQueryParameters_WithPrimitiveTypeProperties_DoesNotRecurse()
    {
        var queryParameters = new PrimitiveTypesTestModel
        {
            IntValue = 42,
            StringValue = "test",
            DecimalValue = 3.14m,
            DateTimeValue = DateTime.Now,
            DateTimeOffsetValue = DateTimeOffset.Now,
            GuidValue = Guid.NewGuid(),
            NullableIntValue = 10
        };

        var exception = Record.Exception(() =>
            RequestParameterValidator.ValidateQueryParameters(queryParameters, _mockLogger));

        Assert.Null(exception);
    }

    /// <summary>
    /// ValidateQueryParametersがインデクサープロパティを除外することを確認
    /// </summary>
    [Fact]
    public void ValidateQueryParameters_WithIndexerProperty_ExcludesIndexer()
    {
        var queryParameters = new IndexerTestModel();
        queryParameters["key1"] = "value1";

        var exception = Record.Exception(() =>
            RequestParameterValidator.ValidateQueryParameters(queryParameters, _mockLogger));

        Assert.Null(exception);
    }

    /// <summary>
    /// ValidateQueryParametersがnull要素を含むIEnumerableでnull要素をスキップすることを確認
    /// </summary>
    [Fact]
    public void ValidateQueryParameters_WithNullItemsInCollection_SkipsNullItems()
    {
        var queryParameters = new CollectionTestModel
        {
            OuterProperty = "OuterValue",
            Items = new List<ValidTestModel?>
            {
                new ValidTestModel { Name = "Item1", Age = 25 },
                null,
                new ValidTestModel { Name = "Item2", Age = 30 }
            }
        };

        var exception = Record.Exception(() =>
            RequestParameterValidator.ValidateQueryParameters(queryParameters, _mockLogger));

        Assert.Null(exception);
    }

    /// <summary>
    /// ValidateQueryParametersがIEnumerableそのものをルートとして渡された場合に各要素を検証することを確認
    /// </summary>
    [Fact]
    public void ValidateQueryParameters_WithRootLevelIEnumerable_ValidatesAllItems()
    {
        var queryParameters = new List<ValidTestModel>
        {
            new ValidTestModel { Name = "Item1", Age = 25 },
            new ValidTestModel { Name = "Item2", Age = 30 }
        };

        var exception = Record.Exception(() =>
            RequestParameterValidator.ValidateQueryParameters(queryParameters, _mockLogger));

        Assert.Null(exception);
    }

    /// <summary>
    /// ValidateQueryParametersがIEnumerableそのものをルートとして渡され、無効な要素がある場合にValidationExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void ValidateQueryParameters_WithRootLevelIEnumerableAndInvalidItem_ThrowsValidationException()
    {
        var queryParameters = new List<ValidTestModel>
        {
            new ValidTestModel { Name = "Item1", Age = 25 },
            new ValidTestModel { Name = null, Age = 30 }
        };

        var exception = Assert.Throws<ValidationException>(() =>
            RequestParameterValidator.ValidateQueryParameters(queryParameters, _mockLogger));

        Assert.Equal("The Name field is required.", exception.Message);
    }

    /// <summary>
    /// ValidateRequestBodyがnullのrequestBodyでValidationExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void ValidateRequestBody_WithNullRequestBody_ThrowsValidationException()
    {
        object? requestBody = null;

        var exception = Assert.Throws<ValidationException>(() =>
            RequestParameterValidator.ValidateRequestBody(requestBody!, _mockLogger));

        Assert.Equal("リクエストボディは必須です。", exception.Message);
    }

    /// <summary>
    /// ValidateRequestBodyがnullのrequestBody時にWarningログを出力することを確認
    /// </summary>
    [Fact]
    public void ValidateRequestBody_WithNullRequestBody_LogsWarning()
    {
        object? requestBody = null;

        Assert.Throws<ValidationException>(() =>
            RequestParameterValidator.ValidateRequestBody(requestBody!, _mockLogger));

        _mockLogger.Received(1).Log(
            LogLevel.Warning,
            Arg.Any<EventId>(),
            Arg.Is<object>(o => o.ToString() == "リクエストボディは必須です。"),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// ValidateRequestBodyが有効なrequestBodyで検証をパスすることを確認
    /// </summary>
    [Fact]
    public void ValidateRequestBody_WithValidRequestBody_PassesValidation()
    {
        var requestBody = new ValidTestModel
        {
            Name = "TestName",
            Age = 25
        };

        var exception = Record.Exception(() =>
            RequestParameterValidator.ValidateRequestBody(requestBody, _mockLogger));

        Assert.Null(exception);
    }

    /// <summary>
    /// ValidateRequestBodyがRequired属性違反でValidationExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void ValidateRequestBody_WithRequiredViolation_ThrowsValidationException()
    {
        var requestBody = new ValidTestModel
        {
            Name = null,
            Age = 25
        };

        var exception = Assert.Throws<ValidationException>(() =>
            RequestParameterValidator.ValidateRequestBody(requestBody, _mockLogger));

        Assert.Equal("The Name field is required.", exception.Message);
    }

    /// <summary>
    /// ValidateRequestBodyがRange属性違反でValidationExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void ValidateRequestBody_WithRangeViolation_ThrowsValidationException()
    {
        var requestBody = new ValidTestModel
        {
            Name = "TestName",
            Age = 150
        };

        var exception = Assert.Throws<ValidationException>(() =>
            RequestParameterValidator.ValidateRequestBody(requestBody, _mockLogger));

        Assert.Equal("The field Age must be between 1 and 100.", exception.Message);
    }

    /// <summary>
    /// ValidateRequestBodyがStringLength属性違反でValidationExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void ValidateRequestBody_WithStringLengthViolation_ThrowsValidationException()
    {
        var requestBody = new ValidTestModel
        {
            Name = "ThisIsAVeryLongName",
            Age = 25
        };

        var exception = Assert.Throws<ValidationException>(() =>
            RequestParameterValidator.ValidateRequestBody(requestBody, _mockLogger));

        Assert.Equal("The field Name must be a string with a maximum length of 10.", exception.Message);
    }

    /// <summary>
    /// ValidateRequestBodyがネストしたプロパティが全て有効な場合に検証をパスすることを確認
    /// </summary>
    [Fact]
    public void ValidateRequestBody_WithValidNestedProperties_PassesValidation()
    {
        var requestBody = new NestedTestModel
        {
            OuterProperty = "OuterValue",
            Nested = new ValidTestModel
            {
                Name = "NestedName",
                Age = 30
            }
        };

        var exception = Record.Exception(() =>
            RequestParameterValidator.ValidateRequestBody(requestBody, _mockLogger));

        Assert.Null(exception);
    }

    /// <summary>
    /// ValidateRequestBodyがネストしたプロパティの一部が無効な場合にValidationExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void ValidateRequestBody_WithInvalidNestedProperty_ThrowsValidationException()
    {
        var requestBody = new NestedTestModel
        {
            OuterProperty = "OuterValue",
            Nested = new ValidTestModel
            {
                Name = null,
                Age = 30
            }
        };

        var exception = Assert.Throws<ValidationException>(() =>
            RequestParameterValidator.ValidateRequestBody(requestBody, _mockLogger));

        Assert.Equal("The Name field is required.", exception.Message);
    }

    /// <summary>
    /// ValidateRequestBodyが3階層のネストしたプロパティでバリデーションを実行することを確認
    /// </summary>
    [Fact]
    public void ValidateRequestBody_WithThreeLevelNesting_ValidatesAllLevels()
    {
        var requestBody = new Level1Model
        {
            Level1Property = "Level1",
            Level2 = new Level2Model
            {
                Level2Property = "Level2",
                Level3 = new Level3Model
                {
                    Level3Property = null
                }
            }
        };

        var exception = Assert.Throws<ValidationException>(() =>
            RequestParameterValidator.ValidateRequestBody(requestBody, _mockLogger));

        Assert.Equal("The Level3Property field is required.", exception.Message);
    }

    /// <summary>
    /// ValidateRequestBodyがIEnumerable内の有効な要素で検証をパスすることを確認
    /// </summary>
    [Fact]
    public void ValidateRequestBody_WithValidIEnumerableItems_PassesValidation()
    {
        var requestBody = new CollectionTestModel
        {
            OuterProperty = "OuterValue",
            Items = new List<ValidTestModel?>
            {
                new ValidTestModel { Name = "Item1", Age = 25 },
                new ValidTestModel { Name = "Item2", Age = 30 }
            }
        };

        var exception = Record.Exception(() =>
            RequestParameterValidator.ValidateRequestBody(requestBody, _mockLogger));

        Assert.Null(exception);
    }

    /// <summary>
    /// ValidateRequestBodyがIEnumerable内の無効な要素でValidationExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void ValidateRequestBody_WithInvalidIEnumerableItem_ThrowsValidationException()
    {
        var requestBody = new CollectionTestModel
        {
            OuterProperty = "OuterValue",
            Items = new List<ValidTestModel?>
            {
                new ValidTestModel { Name = "Item1", Age = 25 },
                new ValidTestModel { Name = null, Age = 30 }
            }
        };

        var exception = Assert.Throws<ValidationException>(() =>
            RequestParameterValidator.ValidateRequestBody(requestBody, _mockLogger));

        Assert.Equal("The Name field is required.", exception.Message);
    }

    /// <summary>
    /// ValidateRequestBodyが複数のバリデーションエラーを結合して例外メッセージに含めることを確認
    /// </summary>
    [Fact]
    public void ValidateRequestBody_WithMultipleErrors_CombinesErrorMessages()
    {
        var requestBody = new InvalidTestModel
        {
            RequiredProperty = null,
            RangeProperty = 5,
            StringProperty = "TooLongString"
        };

        var exception = Assert.Throws<ValidationException>(() =>
            RequestParameterValidator.ValidateRequestBody(requestBody, _mockLogger));

        Assert.Contains("The RequiredProperty field is required.", exception.Message);
        Assert.Contains("RangeProperty must be between 10 and 20", exception.Message);
        Assert.Contains("The field StringProperty must be a string with a maximum length of 5.", exception.Message);
    }

    /// <summary>
    /// ValidateRequestBodyが複数のバリデーションエラー時に各エラーをログ出力することを確認
    /// </summary>
    [Fact]
    public void ValidateRequestBody_WithMultipleErrors_LogsAllErrors()
    {
        var requestBody = new InvalidTestModel
        {
            RequiredProperty = null,
            RangeProperty = 5,
            StringProperty = "TooLongString"
        };

        Assert.Throws<ValidationException>(() =>
            RequestParameterValidator.ValidateRequestBody(requestBody, _mockLogger));

        _mockLogger.Received(1).Log(
            LogLevel.Warning,
            Arg.Any<EventId>(),
            Arg.Is<object>(o => o.ToString() == "The RequiredProperty field is required."),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());

        _mockLogger.Received(1).Log(
            LogLevel.Warning,
            Arg.Any<EventId>(),
            Arg.Is<object>(o => o.ToString() == "The field RangeProperty must be between 10 and 20."),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());

        _mockLogger.Received(1).Log(
            LogLevel.Warning,
            Arg.Any<EventId>(),
            Arg.Is<object>(o => o.ToString() == "The field StringProperty must be a string with a maximum length of 5."),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// ValidateRequestBodyが空配列でValidationExceptionをスローすることを確認
    /// </summary>
    [Fact]
    public void ValidateRequestBody_WithEmptyArray_ThrowsValidationException()
    {
        var requestBody = new List<ValidTestModel>();

        var exception = Assert.Throws<ValidationException>(() =>
            RequestParameterValidator.ValidateRequestBody(requestBody, _mockLogger));

        Assert.Equal("リクエストボディが空配列です。", exception.Message);
    }

    /// <summary>
    /// ValidateRequestBodyが空配列時にWarningログを出力することを確認
    /// </summary>
    [Fact]
    public void ValidateRequestBody_WithEmptyArray_LogsWarning()
    {
        var requestBody = new List<ValidTestModel>();

        Assert.Throws<ValidationException>(() =>
            RequestParameterValidator.ValidateRequestBody(requestBody, _mockLogger));

        _mockLogger.Received(1).Log(
            LogLevel.Warning,
            Arg.Any<EventId>(),
            Arg.Is<object>(o => o.ToString() == "リクエストボディが空配列です。"),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// ValidateRequestBodyが要素を含む配列で検証をパスすることを確認
    /// </summary>
    [Fact]
    public void ValidateRequestBody_WithNonEmptyArray_PassesValidation()
    {
        var requestBody = new List<ValidTestModel>
        {
            new ValidTestModel { Name = "Test", Age = 25 }
        };

        var exception = Record.Exception(() =>
            RequestParameterValidator.ValidateRequestBody(requestBody, _mockLogger));

        Assert.Null(exception);
    }

    /// <summary>
    /// ValidateRequestBodyがnull要素を含むIEnumerableでnull要素をスキップすることを確認
    /// </summary>
    [Fact]
    public void ValidateRequestBody_WithNullItemsInCollection_SkipsNullItems()
    {
        var requestBody = new CollectionTestModel
        {
            OuterProperty = "OuterValue",
            Items = new List<ValidTestModel?>
            {
                new ValidTestModel { Name = "Item1", Age = 25 },
                null,
                new ValidTestModel { Name = "Item2", Age = 30 }
            }
        };

        var exception = Record.Exception(() =>
            RequestParameterValidator.ValidateRequestBody(requestBody, _mockLogger));

        Assert.Null(exception);
    }

    /// <summary>
    /// バリデーション属性付きテスト用モデルクラス
    /// </summary>
    private class ValidTestModel
    {
        [Required]
        [StringLength(10)]
        public string? Name { get; set; }

        [Range(1, 100)]
        public int Age { get; set; }
    }

    /// <summary>
    /// ネストプロパティテスト用モデルクラス
    /// </summary>
    private class NestedTestModel
    {
        [Required]
        public string? OuterProperty { get; set; }

        [Required]
        public ValidTestModel? Nested { get; set; }
    }

    /// <summary>
    /// オプショナルネストプロパティテスト用モデルクラス
    /// </summary>
    private class OptionalNestedTestModel
    {
        [Required]
        public string? OuterProperty { get; set; }

        public ValidTestModel? OptionalNested { get; set; }
    }

    /// <summary>
    /// 複数バリデーションエラーテスト用モデルクラス
    /// </summary>
    private class InvalidTestModel
    {
        [Required]
        public string? RequiredProperty { get; set; }

        [Range(10, 20)]
        public int RangeProperty { get; set; }

        [StringLength(5)]
        public string? StringProperty { get; set; }
    }

    /// <summary>
    /// IEnumerableテスト用モデルクラス
    /// </summary>
    private class CollectionTestModel
    {
        [Required]
        public string? OuterProperty { get; set; }

        public List<ValidTestModel?>? Items { get; set; }
    }

    /// <summary>
    /// 基本型プロパティテスト用モデルクラス
    /// </summary>
    private class PrimitiveTypesTestModel
    {
        public int IntValue { get; set; }
        public string? StringValue { get; set; }
        public decimal DecimalValue { get; set; }
        public DateTime DateTimeValue { get; set; }
        public DateTimeOffset DateTimeOffsetValue { get; set; }
        public Guid GuidValue { get; set; }
        public int? NullableIntValue { get; set; }
    }

    /// <summary>
    /// インデクサープロパティテスト用モデルクラス
    /// </summary>
    private class IndexerTestModel
    {
        private readonly Dictionary<string, string> _data = new();

        public string this[string key]
        {
            get => _data.TryGetValue(key, out var value) ? value : string.Empty;
            set => _data[key] = value;
        }
    }

    /// <summary>
    /// 3階層ネストテスト用モデルクラス（Level1）
    /// </summary>
    private class Level1Model
    {
        [Required]
        public string? Level1Property { get; set; }

        [Required]
        public Level2Model? Level2 { get; set; }
    }

    /// <summary>
    /// 3階層ネストテスト用モデルクラス（Level2）
    /// </summary>
    private class Level2Model
    {
        [Required]
        public string? Level2Property { get; set; }

        [Required]
        public Level3Model? Level3 { get; set; }
    }

    /// <summary>
    /// 3階層ネストテスト用モデルクラス（Level3）
    /// </summary>
    private class Level3Model
    {
        [Required]
        public string? Level3Property { get; set; }
    }
}
