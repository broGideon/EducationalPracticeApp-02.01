using System.Net.Http;
using System.Text;
using EducationalPracticeApp.Helper;
using EducationalPracticeApp.Models;
using Newtonsoft.Json;

namespace EducationalPracticeApp.ViewModels;

public partial class AuthViewModel : ObservableObject
{
    private readonly string _baseUrl = "http://localhost:5166";
    [ObservableProperty] private string? _errorMessage;
    [ObservableProperty] private string? _login;
    [ObservableProperty] private string? _password;
    public event EventHandler? OpenMainWindow;

    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task Authenticate()
    {
        if (string.IsNullOrWhiteSpace(Login) && string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Введите логин и пароль";
            return;
        }

        var client = new HttpClient();
        var authRequest = new AuthRequest(Login!, Password!);
        var json = JsonConvert.SerializeObject(authRequest);
        HttpContent body = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync($"{_baseUrl}/auth", body);
        if (response.IsSuccessStatusCode)
        {
            var auth =
                JsonConvert.DeserializeObject<AuthResponse>(await response.Content.ReadAsStringAsync())!;
            TokenHelper.SaveRefreshToken(auth.RefreshToken);
            OpenMainWindow!(this, EventArgs.Empty);
        }
        else
        {
            ErrorMessage = "Неверный логин или пароль";
        }
    }
}