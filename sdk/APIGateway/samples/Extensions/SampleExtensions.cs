namespace Sacloud.Sdk.APIGateway.Samples.Extensions;

/// <summary>
/// サンプルコード用の拡張メソッドを提供します。
/// </summary>
public static class SampleExtensions
{
    /// <summary>
    /// Console.WriteLine の拡張メソッド。指定されたインデントレベルでメッセージを出力します。
    /// </summary>
    /// <param name="message">出力するメッセージ。</param>
    /// <param name="indentLevel">インデントレベル（スペース数）。デフォルトは 0。</param>
    extension(Console)
    {
        public static void WriteIndentedLine(string message, int indentLevel = 0)
        {
            string indent = new(' ', indentLevel);
            Console.WriteLine($"{indent}{message}");
        }
    }

    /// <summary>
    /// null 値を文字列に変換します。null の場合は "(null)" を返します。
    /// </summary>
    /// <param name="value">変換する値。</param>
    /// <returns>値を文字列に変換した結果。null の場合は "(null)"。</returns>
    public static string FormatNullable(this object? value)
    {
        return value?.ToString() ?? "(null)";
    }
}
