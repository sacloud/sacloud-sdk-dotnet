using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Sacloud.Sdk.Shared.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Sacloud.Sdk.Shared.Helpers
{
    /// <summary>
    /// API例外のログ出力を共通化するヘルパークラスです。
    /// </summary>
    public static class ApiErrorLogger
    {
        /// <summary>
        /// ApiExceptionの詳細情報を構造化ログとして出力します。
        /// </summary>
        /// <param name="ex">API例外</param>
        /// <param name="logger">ロガー</param>
        /// <param name="operationName">実行していた操作名（例: "アプリケーション一覧取得"）</param>
        public static void LogApiException(ApiException ex, ILogger logger, string operationName)
        {
            if (ex == null || logger == null)
            {
                return;
            }

            var errorDetails = BuildErrorDetails(ex);

            logger.LogError(ex,
                "{OperationName}のAPIエラーが発生しました。 {EndTime} {@ErrorDetails}",
                operationName,
                DateTime.UtcNow.ToString("o"),
                errorDetails);
        }

        /// <summary>
        /// ApiExceptionから構造化されたエラー詳細情報を構築します。
        /// </summary>
        /// <param name="ex">API例外</param>
        /// <returns>エラー詳細情報のディクショナリ</returns>
        private static Dictionary<string, object> BuildErrorDetails(ApiException ex)
        {
            var errorDetails = new Dictionary<string, object>
            {
                ["StatusCode"] = ex.ResponseStatusCode
            };

            if (ex is ApiError apiError)
            {
                var errorResponseBody = apiError.ResponseBody;

                var flattenedBody = FlattenResponseBody(errorResponseBody);
                foreach (var kvp in flattenedBody)
                {
                    errorDetails[kvp.Key] = kvp.Value;
                }
            }

            return errorDetails;
        }

        /// <summary>
        /// UntypedNodeまたはその他のオブジェクトを文字列に変換します。
        /// </summary>
        /// <param name="value">変換する値</param>
        /// <returns>文字列表現。nullの場合はnull</returns>
        private static string ConvertToString(object value)
        {
            if (value == null)
            {
                return null;
            }

            // UntypedNodeの各派生型から実際の値を取得
            switch (value)
            {
                case Microsoft.Kiota.Abstractions.Serialization.UntypedString untypedString:
                    return untypedString.GetValue();
                case Microsoft.Kiota.Abstractions.Serialization.UntypedInteger untypedInteger:
                    return untypedInteger.GetValue().ToString(CultureInfo.InvariantCulture);
                case Microsoft.Kiota.Abstractions.Serialization.UntypedLong untypedLong:
                    return untypedLong.GetValue().ToString(CultureInfo.InvariantCulture);
                case Microsoft.Kiota.Abstractions.Serialization.UntypedDouble untypedDouble:
                    return untypedDouble.GetValue().ToString(CultureInfo.InvariantCulture);
                case Microsoft.Kiota.Abstractions.Serialization.UntypedFloat untypedFloat:
                    return untypedFloat.GetValue().ToString(CultureInfo.InvariantCulture);
                case Microsoft.Kiota.Abstractions.Serialization.UntypedBoolean untypedBoolean:
                    return untypedBoolean.GetValue().ToString(CultureInfo.InvariantCulture);
                case Microsoft.Kiota.Abstractions.Serialization.UntypedNull _:
                    return null;
                default:
                    return value.ToString();
            }
        }

        /// <summary>
        /// エラーレスポンスボディをフラット化してログ出力可能な形式に変換します。
        /// UntypedObjectやUntypedArrayを含むすべてのキーバリューを展開します。
        /// </summary>
        /// <param name="errorResponseBody">APIから返却されたエラーレスポンスボディ</param>
        /// <returns>フラット化されたディクショナリ</returns>
        private static Dictionary<string, object> FlattenResponseBody(IDictionary<string, object> errorResponseBody)
        {
            var result = new Dictionary<string, object>();

            if (errorResponseBody == null || !errorResponseBody.Any())
            {
                return result;
            }

            FlattenDictionary(errorResponseBody, string.Empty, result);
            return result;
        }

        /// <summary>
        /// ディクショナリを再帰的にフラット化します。
        /// </summary>
        /// <param name="dictionary">フラット化するディクショナリ</param>
        /// <param name="prefix">キーのプレフィックス（ネスト階層を表現）</param>
        /// <param name="result">フラット化結果を格納するディクショナリ</param>
        private static void FlattenDictionary(IDictionary<string, object> dictionary, string prefix, Dictionary<string, object> result)
        {
            foreach (var kvp in dictionary)
            {
                var key = string.IsNullOrEmpty(prefix) ? kvp.Key : $"{prefix}.{kvp.Key}";
                FlattenValue(kvp.Value, key, result);
            }
        }

        /// <summary>
        /// UntypedNodeディクショナリを再帰的にフラット化します。
        /// </summary>
        /// <param name="dictionary">フラット化するUntypedNodeディクショナリ</param>
        /// <param name="prefix">キーのプレフィックス（ネスト階層を表現）</param>
        /// <param name="result">フラット化結果を格納するディクショナリ</param>
        private static void FlattenUntypedDictionary(
            IDictionary<string, Microsoft.Kiota.Abstractions.Serialization.UntypedNode> dictionary,
            string prefix,
            Dictionary<string, object> result)
        {
            foreach (var kvp in dictionary)
            {
                var key = string.IsNullOrEmpty(prefix) ? kvp.Key : $"{prefix}.{kvp.Key}";
                FlattenValue(kvp.Value, key, result);
            }
        }

        /// <summary>
        /// 値を型に応じてフラット化します。
        /// </summary>
        /// <param name="value">フラット化する値</param>
        /// <param name="key">キー</param>
        /// <param name="result">フラット化結果を格納するディクショナリ</param>
        private static void FlattenValue(object value, string key, Dictionary<string, object> result)
        {
            switch (value)
            {
                case Microsoft.Kiota.Abstractions.Serialization.UntypedObject untypedObject:
                    var nestedDict = untypedObject.GetValue();
                    if (nestedDict != null)
                    {
                        FlattenUntypedDictionary(nestedDict, key, result);
                    }
                    break;
                case IDictionary<string, object> dictionary:
                    FlattenDictionary(dictionary, key, result);
                    break;
                case Microsoft.Kiota.Abstractions.Serialization.UntypedArray untypedArray:
                    FlattenUntypedArray(untypedArray, key, result);
                    break;
                default:
                    result[key] = ConvertToString(value);
                    break;
            }
        }

        /// <summary>
        /// UntypedArrayをフラット化します。
        /// </summary>
        /// <param name="untypedArray">フラット化するUntypedArray</param>
        /// <param name="key">キー</param>
        /// <param name="result">フラット化結果を格納するディクショナリ</param>
        private static void FlattenUntypedArray(
            Microsoft.Kiota.Abstractions.Serialization.UntypedArray untypedArray,
            string key,
            Dictionary<string, object> result)
        {
            var items = untypedArray.GetValue();
            if (items == null)
            {
                return;
            }

            var index = 0;
            foreach (var item in items)
            {
                var arrayKey = $"{key}[{index}]";
                FlattenValue(item, arrayKey, result);
                index++;
            }
        }
    }
}