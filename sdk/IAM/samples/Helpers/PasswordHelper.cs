namespace Sacloud.Sdk.IAM.Samples.Helpers;

public static class PasswordHelper
{
    /// <summary>
    /// 英数字とASCII標準文字における記号を含むランダムなパスワードを生成します。
    /// 英字と数字を必ず含めます。
    /// </summary>
    /// <returns>生成されたパスワード</returns>
    public static string GeneratePassword()
    {
        var random = Random.Shared;
        const string letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string digits = "0123456789";
        const string symbols = "!\"#$%&'()*+,-./:;<=>?@[]^_`{|}~";
        const string allChars = letters + digits + symbols;

        var password = new char[16];

        // 必ず英字と数字を含める
        password[0] = letters[random.Next(letters.Length)];
        password[1] = digits[random.Next(digits.Length)];

        // 残りをランダムに埋める
        for (int i = 2; i < password.Length; i++)
        {
            password[i] = allChars[random.Next(allChars.Length)];
        }

        // シャッフル（Fisher-Yates）
        for (int i = password.Length - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            (password[i], password[j]) = (password[j], password[i]);
        }

        return new string(password);
    }
}
