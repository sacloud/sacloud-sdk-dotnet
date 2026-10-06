using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;
using Sacloud.Sdk.Shared.Helpers;
using Sacloud.Sdk.Shared.Models;
using System.Text.RegularExpressions;

namespace Sacloud.Sdk.Shared.Tests.Helpers;

public class ApiErrorLoggerTests
{
    private const string DefaultOperationName = "テスト操作";
    private readonly ILogger _mockLogger;

    public ApiErrorLoggerTests()
    {
        _mockLogger = Substitute.For<ILogger>();
    }

    private static ApiError CreateApiError(int statusCode = 400) => new() { ResponseStatusCode = statusCode };

    [Fact]
    public void LogApiException_WithNullException_DoesNotWriteLog()
    {
        ApiErrorLogger.LogApiException(null, _mockLogger, DefaultOperationName);

        _mockLogger.DidNotReceive().Log(
            Arg.Any<LogLevel>(),
            Arg.Any<EventId>(),
            Arg.Any<object>(),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    [Fact]
    public void LogApiException_WithNullLogger_DoesNotThrow()
    {
        var exception = new ApiException("request failed") { ResponseStatusCode = 500 };

        var recordedException = Record.Exception(() => ApiErrorLogger.LogApiException(exception, null, DefaultOperationName));

        Assert.Null(recordedException);
    }

    [Fact]
    public void LogApiException_WithApiException_WritesStructuredLog()
    {
        var exception = new ApiException("request failed") { ResponseStatusCode = 500 };

        ApiErrorLogger.LogApiException(exception, _mockLogger, DefaultOperationName);

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                Regex.IsMatch(
                    state.ToString()!,
                    @$"^{DefaultOperationName}のAPIエラーが発生しました。 \d{{4}}-\d{{2}}-\d{{2}}T\d{{2}}:\d{{2}}:\d{{2}}\.\d+Z \[StatusCode, 500\]$")),
            exception,
            Arg.Any<Func<object, Exception?, string>>());
    }

    [Fact]
    public void LogApiException_WithApiError_FlattensNestedDictionary()
    {
        var apiError = CreateApiError();
        apiError.ResponseBody["error"] = new Dictionary<string, object>
        {
            ["message"] = "invalid request",
            ["details"] = new Dictionary<string, object>
            {
                ["field"] = "name",
                ["reason"] = "required"
            }
        };
        apiError.ResponseBody["trackingId"] = "abc123";

        ApiErrorLogger.LogApiException(apiError, _mockLogger, "アプリケーション作成");

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                Regex.IsMatch(
                    state.ToString()!,
                    @"^アプリケーション作成のAPIエラーが発生しました。 \d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z.*\[StatusCode, 400\].*\[error\.message, invalid request\].*\[error\.details\.field, name\].*\[error\.details\.reason, required\].*\[trackingId, abc123\]$")),
            apiError,
            Arg.Any<Func<object, Exception?, string>>());
    }

    [Fact]
    public void LogApiException_WithApiError_EmptyResponseBody()
    {
        var apiError = CreateApiError(404);

        ApiErrorLogger.LogApiException(apiError, _mockLogger, DefaultOperationName);

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                Regex.IsMatch(
                    state.ToString()!,
                    @$"^{DefaultOperationName}のAPIエラーが発生しました。 \d{{4}}-\d{{2}}-\d{{2}}T\d{{2}}:\d{{2}}:\d{{2}}\.\d+Z \[StatusCode, 404\]$")),
            apiError,
            Arg.Any<Func<object, Exception?, string>>());
    }

    [Theory]
    [InlineData("message", "UntypedString", "エラーメッセージ", @"\[message, エラーメッセージ\]")]
    [InlineData("code", "UntypedInteger", 12345, @"\[code, 12345\]")]
    [InlineData("timestamp", "UntypedLong", 1234567890123L, @"\[timestamp, 1234567890123\]")]
    [InlineData("percentage", "UntypedDouble", 99.99, @"\[percentage, 99\.99\]")]
    [InlineData("ratio", "UntypedFloat", 0.5f, @"\[ratio, 0\.5\]")]
    [InlineData("isActive", "UntypedBoolean", true, @"\[isActive, True\]")]
    public void LogApiException_WithApiError_UntypedPrimitiveValues(string fieldName, string typeName, object value, string expectedPattern)
    {
        var apiError = CreateApiError();

        UntypedNode untypedValue = typeName switch
        {
            "UntypedString" => new UntypedString((string)value),
            "UntypedInteger" => new UntypedInteger((int)value),
            "UntypedLong" => new UntypedLong((long)value),
            "UntypedDouble" => new UntypedDouble((double)value),
            "UntypedFloat" => new UntypedFloat((float)value),
            "UntypedBoolean" => new UntypedBoolean((bool)value),
            _ => throw new ArgumentException($"Unknown type: {typeName}")
        };

        apiError.ResponseBody[fieldName] = untypedValue;

        ApiErrorLogger.LogApiException(apiError, _mockLogger, DefaultOperationName);

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                Regex.IsMatch(
                    state.ToString()!,
                    @$"^{DefaultOperationName}のAPIエラーが発生しました。 \d{{4}}-\d{{2}}-\d{{2}}T\d{{2}}:\d{{2}}:\d{{2}}\.\d+Z.*\[StatusCode, 400\].*{expectedPattern}$")),
            apiError,
            Arg.Any<Func<object, Exception?, string>>());
    }

    [Fact]
    public void LogApiException_WithApiError_UntypedNullValue()
    {
        var apiError = CreateApiError();
        apiError.ResponseBody["nullField"] = new UntypedNull();

        ApiErrorLogger.LogApiException(apiError, _mockLogger, DefaultOperationName);

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => state.ToString()!.Contains("[StatusCode, 400]")),
            apiError,
            Arg.Any<Func<object, Exception?, string>>());
    }

    [Fact]
    public void LogApiException_WithApiError_UntypedObject()
    {
        var apiError = CreateApiError();
        var nestedDict = new Dictionary<string, UntypedNode>
        {
            ["field1"] = new UntypedString("value1"),
            ["field2"] = new UntypedInteger(42)
        };
        apiError.ResponseBody["nested"] = new UntypedObject(nestedDict);

        ApiErrorLogger.LogApiException(apiError, _mockLogger, DefaultOperationName);

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                Regex.IsMatch(
                    state.ToString()!,
                    @$"^{DefaultOperationName}のAPIエラーが発生しました。 \d{{4}}-\d{{2}}-\d{{2}}T\d{{2}}:\d{{2}}:\d{{2}}\.\d+Z.*\[StatusCode, 400\].*\[nested\.field1, value1\].*\[nested\.field2, 42\]$")),
            apiError,
            Arg.Any<Func<object, Exception?, string>>());
    }

    [Fact]
    public void LogApiException_WithApiError_UntypedObjectWithNull()
    {
        var apiError = CreateApiError();
        apiError.ResponseBody["nullObject"] = new UntypedObject(null!);

        ApiErrorLogger.LogApiException(apiError, _mockLogger, DefaultOperationName);

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => state.ToString()!.Contains("[StatusCode, 400]")),
            apiError,
            Arg.Any<Func<object, Exception?, string>>());
    }

    [Fact]
    public void LogApiException_WithApiError_UntypedArray()
    {
        var apiError = CreateApiError();
        var arrayItems = new List<UntypedNode>
        {
            new UntypedString("item1"),
            new UntypedString("item2"),
            new UntypedInteger(123)
        };
        apiError.ResponseBody["items"] = new UntypedArray(arrayItems);

        ApiErrorLogger.LogApiException(apiError, _mockLogger, DefaultOperationName);

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                Regex.IsMatch(
                    state.ToString()!,
                    @$"^{DefaultOperationName}のAPIエラーが発生しました。 \d{{4}}-\d{{2}}-\d{{2}}T\d{{2}}:\d{{2}}:\d{{2}}\.\d+Z.*\[StatusCode, 400\].*\[items\[0\], item1\].*\[items\[1\], item2\].*\[items\[2\], 123\]$")),
            apiError,
            Arg.Any<Func<object, Exception?, string>>());
    }

    [Fact]
    public void LogApiException_WithApiError_UntypedArrayWithNull()
    {
        var apiError = CreateApiError();
        apiError.ResponseBody["emptyArray"] = new UntypedArray(null!);

        ApiErrorLogger.LogApiException(apiError, _mockLogger, DefaultOperationName);

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state => state.ToString()!.Contains("[StatusCode, 400]")),
            apiError,
            Arg.Any<Func<object, Exception?, string>>());
    }

    [Fact]
    public void LogApiException_WithApiError_NestedUntypedArrayInObject()
    {
        var apiError = CreateApiError();
        var nestedArray = new List<UntypedNode>
        {
            new UntypedString("nested1"),
            new UntypedString("nested2")
        };
        var nestedDict = new Dictionary<string, UntypedNode>
        {
            ["arrayField"] = new UntypedArray(nestedArray)
        };
        apiError.ResponseBody["container"] = new UntypedObject(nestedDict);

        ApiErrorLogger.LogApiException(apiError, _mockLogger, DefaultOperationName);

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                Regex.IsMatch(
                    state.ToString()!,
                    @$"^{DefaultOperationName}のAPIエラーが発生しました。 \d{{4}}-\d{{2}}-\d{{2}}T\d{{2}}:\d{{2}}:\d{{2}}\.\d+Z.*\[StatusCode, 400\].*\[container\.arrayField\[0\], nested1\].*\[container\.arrayField\[1\], nested2\]$")),
            apiError,
            Arg.Any<Func<object, Exception?, string>>());
    }

    [Fact]
    public void LogApiException_WithApiError_ComplexNestedStructure()
    {
        var apiError = CreateApiError();
        var level3Dict = new Dictionary<string, object>
        {
            ["deepField"] = "deepValue"
        };
        var level2Dict = new Dictionary<string, object>
        {
            ["level3"] = level3Dict,
            ["simpleField"] = "simpleValue"
        };
        apiError.ResponseBody["level1"] = level2Dict;

        ApiErrorLogger.LogApiException(apiError, _mockLogger, DefaultOperationName);

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                Regex.IsMatch(
                    state.ToString()!,
                    @$"^{DefaultOperationName}のAPIエラーが発生しました。 \d{{4}}-\d{{2}}-\d{{2}}T\d{{2}}:\d{{2}}:\d{{2}}\.\d+Z.*\[StatusCode, 400\].*\[level1\.level3\.deepField, deepValue\].*\[level1\.simpleField, simpleValue\]$")),
            apiError,
            Arg.Any<Func<object, Exception?, string>>());
    }

    [Fact]
    public void LogApiException_WithApiError_MixedPrimitiveTypes()
    {
        var apiError = CreateApiError();
        apiError.ResponseBody["stringValue"] = "test";
        apiError.ResponseBody["intValue"] = 42;
        apiError.ResponseBody["boolValue"] = true;
        apiError.ResponseBody["nullValue"] = null;

        ApiErrorLogger.LogApiException(apiError, _mockLogger, DefaultOperationName);

        _mockLogger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(state =>
                state.ToString()!.Contains("[StatusCode, 400]") &&
                state.ToString()!.Contains("[stringValue, test]") &&
                state.ToString()!.Contains("[intValue, 42]") &&
                state.ToString()!.Contains("[boolValue, True]")),
            apiError,
            Arg.Any<Func<object, Exception?, string>>());
    }
}
