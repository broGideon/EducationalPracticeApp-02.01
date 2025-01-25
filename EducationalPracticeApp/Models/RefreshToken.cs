namespace EducationalPracticeApp.Models;

public class RefreshToken
{
    public string Token { get; set; }

    public RefreshToken(string token)
    {
        Token = token;
    }
}