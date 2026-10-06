using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sacloud.Sdk.Shared.Validation
{
    public static class RequestParameterValidator
    {
        /// <summary>
        /// パスパラメータのバリデーションを実行します。
        /// </summary>
        /// <param name="pathParameters">パスパラメータ名とその値のリスト</param>
        /// <param name="validationOptions">バリデーション対象のパスパラメータ名とそのバリデーションオプションのリスト</param>
        /// <param name="logger">ロガー</param>
        public static void ValidatePathParameters(
            Dictionary<string, object> pathParameters,
            Dictionary<string, PathParameterValidationOptions> validationOptions,
            ILogger logger)
        {
            var validationResults = new List<ValidationResult>();

            foreach (var option in validationOptions)
            {
                var parameterName = option.Key;
                var validationOption = option.Value;
                var displayName = validationOption.DisplayName;

                // パスパラメータに対象のパラメータ名が存在することをチェック
                if (!pathParameters.TryGetValue(parameterName, out var parameterValue))
                {
                    var errorMessage = $"{displayName}を入力してください。";
                    validationResults.Add(new ValidationResult(errorMessage, new[] { parameterName }));
                    continue; // 値が存在しない場合、以降のバリデーションをスキップ
                }

                // パラメータの値がnullや空文字、空白でないことをチェック
                var parameterStringValue = parameterValue?.ToString();
                if (string.IsNullOrWhiteSpace(parameterStringValue))
                {
                    var errorMessage = $"{displayName}を入力してください。";
                    validationResults.Add(new ValidationResult(errorMessage, new[] { parameterName }));
                    continue; // 値が空の場合、以降のバリデーションをスキップ
                }

                // 文字数のチェック
                // Lengthが指定されている場合は特定の文字数チェックを行い、指定されていない場合はMinLength/MaxLengthの範囲チェックを行う
                if (validationOption.Length.HasValue)
                {
                    // 特定の文字数チェック
                    if (parameterStringValue.Length != validationOption.Length.Value)
                    {
                        var errorMessage = $"{displayName}は{validationOption.Length.Value}文字で入力してください。";
                        validationResults.Add(new ValidationResult(errorMessage, new[] { parameterName }));
                    }
                }
                else
                {
                    // 最小文字数のチェック
                    if (validationOption.MinLength.HasValue && parameterStringValue.Length < validationOption.MinLength.Value)
                    {
                        var errorMessage = $"{displayName}は{validationOption.MinLength.Value}文字以上で入力してください。";
                        validationResults.Add(new ValidationResult(errorMessage, new[] { parameterName }));
                    }

                    // 最大文字数のチェック
                    if (validationOption.MaxLength.HasValue && parameterStringValue.Length > validationOption.MaxLength.Value)
                    {
                        var errorMessage = $"{displayName}は{validationOption.MaxLength.Value}文字以下で入力してください。";
                        validationResults.Add(new ValidationResult(errorMessage, new[] { parameterName }));
                    }
                }

                // 正規表現パターンのチェック
                if (!string.IsNullOrEmpty(validationOption.RegexPattern))
                {
                    var regex = new Regex(validationOption.RegexPattern, RegexOptions.Compiled);
                    if (!regex.IsMatch(parameterStringValue))
                    {
                        var errorMessage = $"{displayName}の形式が正しくありません。";
                        validationResults.Add(new ValidationResult(errorMessage, new[] { parameterName }));
                    }
                }

                // 最小値のチェック
                // パラメータの値がint型にパースできる場合のみチェックする
                if (validationOption.MinValue.HasValue && int.TryParse(parameterStringValue, out var intValue) && intValue < validationOption.MinValue.Value)
                {
                    var errorMessage = $"{displayName}は{validationOption.MinValue.Value}以上で入力してください。";
                    validationResults.Add(new ValidationResult(errorMessage, new[] { parameterName }));
                }

                // 最大値のチェック
                // パラメータの値がlong型にパースできる場合のみチェックする
                if (validationOption.MaxValue.HasValue && long.TryParse(parameterStringValue, out var longValue) && longValue > validationOption.MaxValue.Value)
                {
                    var errorMessage = $"{displayName}は{validationOption.MaxValue.Value}以下で入力してください。";
                    validationResults.Add(new ValidationResult(errorMessage, new[] { parameterName }));
                }

                // GUID形式のチェック
                if (validationOption.IsGuid && !Guid.TryParse(parameterStringValue, out _))
                {
                    var errorMessage = $"{displayName}はGUID形式で入力してください。";
                    validationResults.Add(new ValidationResult(errorMessage, new[] { parameterName }));
                }
            }

            // バリデーションエラーがあればログ出力し、例外をスロー
            if (validationResults.Count > 0)
            {
                LogAndThrow(validationResults, logger);
            }
        }

        /// <summary>
        /// パスパラメータのバリデーションを実行します(後方互換性のため)。
        /// </summary>
        /// <param name="pathParameters">パスパラメータ名とその値のリスト</param>
        /// <param name="targetPathParametersInfo">バリデーション対象のパスパラメータ名とその表示名のリスト</param>
        /// <param name="logger">ロガー</param>
        public static void ValidatePathParameters(
            Dictionary<string, object> pathParameters,
            Dictionary<string, string> targetPathParametersInfo,
            ILogger logger)
        {
            // 既存の呼び出しを新しいオーバーロードに変換
            var validationOptions = new Dictionary<string, PathParameterValidationOptions>();
            foreach (var info in targetPathParametersInfo)
            {
                validationOptions[info.Key] = new PathParameterValidationOptions(info.Value);
            }
            ValidatePathParameters(pathParameters, validationOptions, logger);
        }

        /// <summary>
        /// クエリパラメータをバリデーションを実行します。
        /// </summary>
        /// <param name="queryParameters">クエリパラメータ</param>
        /// <param name="logger">ロガー</param>
        public static void ValidateQueryParameters(object queryParameters, ILogger logger)
        {
            // クエリパラメータがnullの場合はバリデーションをスキップ
            if (queryParameters == null)
            {
                return;
            }

            // バリデーションを実行
            var validationResults = new List<ValidationResult>();
            var isValid = ExecuteRecursiveValidation(queryParameters, validationResults, logger);
            if (!isValid) LogAndThrow(validationResults, logger);
        }

        /// <summary>
        /// リクエストボディのバリデーションを実行します。
        /// </summary>
        /// <param name="requestBody">リクエストボディ</param>
        /// <param name="logger">ロガー</param>
        /// <exception cref="ValidationException"></exception>
        public static void ValidateRequestBody(object requestBody, ILogger logger)
        {
            // リクエストボディがnullの場合は例外をスロー
            if (requestBody == null)
            {
                var message = "リクエストボディは必須です。";
                logger.LogWarning(message);
                throw new ValidationException(message);
            }

            // リクエストボディが空配列の場合は例外をスロー
            if (requestBody is IEnumerable enumerable)
            {
                var hasElements = false;
                foreach (var _ in enumerable)
                {
                    hasElements = true;
                    break;
                }
                if (!hasElements)
                {
                    var message = "リクエストボディが空配列です。";
                    logger.LogWarning(message);
                    throw new ValidationException(message);
                }
            }

            // 再帰的にバリデーションを実行
            var validationResults = new List<ValidationResult>();
            var isValid = ExecuteRecursiveValidation(requestBody, validationResults, logger);

            // バリデーションエラーがあれば警告ログに出力し、例外をスロー
            if (!isValid) LogAndThrow(validationResults, logger);
        }

        /// <summary>
        /// ネストしたプロパティを含むオブジェクトに対して再帰的にバリデーションを実行します。
        /// </summary>
        /// <param name="input">バリデーション対象のオブジェクト</param>
        /// <param name="validationResults">バリデーション結果</param>
        /// <returns>オブジェクトの有効無効フラグ</returns>
        private static bool ExecuteRecursiveValidation(object input, List<ValidationResult> validationResults, ILogger logger)
        {
            // inputがnullの場合は何もしない
            if (input == null)
            {
                return true;
            }

            // IEnumerableの場合は各要素に対してバリデーションを実行
            if (input is System.Collections.IEnumerable enumerable)
            {
                var isValid = true;
                foreach (var item in enumerable)
                {
                    if (item != null)
                    {
                        var itemIsValid = ExecuteRecursiveValidation(item, validationResults, logger);
                        isValid &= itemIsValid;
                    }
                }
                return isValid;
            }

            var validationContext = new ValidationContext(input);
            var isObjectValid = Validator.TryValidateObject(input, validationContext, validationResults, true);

            // ネストしたプロパティのみを事前にフィルタリング
            var nestedProperties = input.GetType().GetProperties()
                .Where(prop => prop.CanRead
                    && prop.GetIndexParameters().Length == 0 // インデクサープロパティを除外
                    && IsNestedType(prop.PropertyType)); // ネストされた型のみを取得

            // ネストしたプロパティがあれば再帰的にバリデーションを実行
            var nestedPropertyValues = nestedProperties
                .Select(prop => prop.GetValue(input))
                .Where(value => value != null);

            foreach (var value in nestedPropertyValues)
            {
                var nestedIsValid = ExecuteRecursiveValidation(value, validationResults, logger);
                isObjectValid &= nestedIsValid;
            }
            return isObjectValid;
        }

        /// <summary>
        /// 指定された型がネストされた型（プリミティブ型や基本的な値型ではない型）かどうかを判定します。
        /// </summary>
        /// <param name="type">判定する型</param>
        /// <returns>ネストされた型の場合true</returns>
        private static bool IsNestedType(Type type)
        {
            // Nullable型の場合は基底型をチェック
            var underlyingType = Nullable.GetUnderlyingType(type) ?? type;

            // プリミティブ型や基本的な値型の場合はfalseを返す（ネストされていない）
            return !(underlyingType.IsPrimitive
                || underlyingType == typeof(string)
                || underlyingType == typeof(decimal)
                || underlyingType == typeof(DateTime)
                || underlyingType == typeof(DateTimeOffset)
                || underlyingType == typeof(Guid));
        }

        /// <summary>
        /// バリデーションエラーをログに出力し、ValidationExceptionをスローします。
        /// </summary>
        /// <param name="validationResults">バリデーション結果</param>
        /// <param name="logger">ロガー</param>
        /// <exception cref="ValidationException"></exception>
        private static void LogAndThrow(List<ValidationResult> validationResults, ILogger logger)
        {
            // 各バリデーションエラーメッセージを警告ログに出力
            foreach (var validationResult in validationResults)
            {
                logger.LogWarning(validationResult.ErrorMessage);
            }
            // すべてのバリデーションエラーメッセージを内包したValidationExceptionをスロー
            throw new ValidationException(string.Join(Environment.NewLine, validationResults.Select(vr => vr.ErrorMessage)));
        }
    }
}
