namespace EducationalPracticeApp.Helper;

public class TokenHelper
{
    public static void SaveRefreshToken(string refreshToken)
    {
        string encryptedToken = TokenEncryptor.Encrypt(refreshToken);
        Properties.Settings.Default.RefreshToken = encryptedToken;
        Properties.Settings.Default.Save();
    }
     
    public static string? LoadRefreshToken()
    {
        if (string.IsNullOrEmpty(Properties.Settings.Default.RefreshToken))
            return null;

        try
        {
            return TokenEncryptor.Decrypt(Properties.Settings.Default.RefreshToken);
        }
        catch
        {
            return null;
        }
    }
}