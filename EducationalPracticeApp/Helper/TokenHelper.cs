using EducationalPracticeApp.Properties;

namespace EducationalPracticeApp.Helper;

public class TokenHelper
{
    public static void SaveRefreshToken(string refreshToken)
    {
        var encryptedToken = TokenEncryptor.Encrypt(refreshToken);
        Settings.Default.RefreshToken = encryptedToken;
        Settings.Default.Save();
    }

    public static string? LoadRefreshToken()
    {
        if (string.IsNullOrEmpty(Settings.Default.RefreshToken))
            return null;

        try
        {
            return TokenEncryptor.Decrypt(Settings.Default.RefreshToken);
        }
        catch
        {
            return null;
        }
    }
}