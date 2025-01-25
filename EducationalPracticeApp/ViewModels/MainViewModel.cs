using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using EducationalPracticeApp.Helper;
using EducationalPracticeApp.Models;

namespace EducationalPracticeApp.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty] private ObservableCollection<Transport> _transports = new();
    [ObservableProperty] private ObservableCollection<Order> _orders = new();
    [ObservableProperty] private bool _isAutoparkActive = true;
    [ObservableProperty] private bool _isDriverActive = true;
    [ObservableProperty] private bool _isOrderActive = true;
    [ObservableProperty] private bool _isVoyageActive = true;
    [ObservableProperty] private bool _isReportActive = true;
    [ObservableProperty] private bool _isClientActive = true;
    private readonly ApiHelper _apiHelper; 

    public MainViewModel()
    {
        _apiHelper = new ApiHelper();
        _ = InitializeDataAsync();
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
    
    private async Task InitializeDataAsync()
    {
        List<Task> tasks = new List<Task> { LoadTransports(), LoadOrders()};
        await Task.WhenAll(tasks);
    }
    
    private async Task LoadTransports()
    {
        List<Transport>? transports = await _apiHelper.Get<List<Transport>>("transport");
        Transports = new ObservableCollection<Transport>(transports ?? new List<Transport>());
    }
    
    private async Task LoadOrders()
    {
        List<Order>? order = await _apiHelper.Get<List<Order>>("order");
        Orders = new ObservableCollection<Order>(order ?? new List<Order>());
    }
}