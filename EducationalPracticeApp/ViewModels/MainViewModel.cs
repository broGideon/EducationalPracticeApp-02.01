using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using EducationalPracticeApp.Helper;
using EducationalPracticeApp.Models;

namespace EducationalPracticeApp.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty] private bool _isAutoparkActive = true;
    [ObservableProperty] private bool _isDriverActive = true;
    [ObservableProperty] private bool _isOrderActive = true;
    [ObservableProperty] private bool _isVoyageActive = true;
    [ObservableProperty] private bool _isReportActive = true;
    [ObservableProperty] private bool _isClientActive = true;

    public MainViewModel()
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(TokenHelper.LoadRefreshToken());
        var role = jwtToken.Claims.FirstOrDefault(t => t.Type == "role")?.Value;
        switch (role)
        {
            case "Administrator":
                IsVoyageActive = false;
                IsReportActive = false;
                break;
            case "Supervisor":
                break;
            case "Logistician":
                IsReportActive = false;
                IsAutoparkActive = false;
                IsDriverActive = false;
                IsClientActive = false;
                break;
        }
    }
}