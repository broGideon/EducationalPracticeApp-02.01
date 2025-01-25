using System.Security.Cryptography;
using System.Text;

namespace EducationalPracticeApp.Helper;

public class TokenEncryptor
{
    private static readonly byte[] entropy = Encoding.UTF8.GetBytes("");

    public static string Encrypt(string input)
    {
        var data = Encoding.UTF8.GetBytes(input);
        var encryptedData = ProtectedData.Protect(data, entropy, DataProtectionScope.CurrentUser);
        return Convert.ToBase64String(encryptedData);
    }

    public static string Decrypt(string encryptedInput)
    {
        var encryptedData = Convert.FromBase64String(encryptedInput);
        var decryptedData = ProtectedData.Unprotect(encryptedData, entropy, DataProtectionScope.CurrentUser);
        return Encoding.UTF8.GetString(decryptedData);
    }
}