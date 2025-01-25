namespace EducationalPracticeApp.Models;

public class AuthRequest
{
    public AuthRequest(string login, string password)
    {
        Login = login;
        Password = password;
    }

    public string Login { get; set; }
    public string Password { get; set; }
}