using System.Collections.ObjectModel;
using EducationalPracticeApp.Helper;
using EducationalPracticeApp.Models;

namespace EducationalPracticeApp.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    private readonly ApiHelper _apiHelper;
    [ObservableProperty] private ObservableCollection<Order> _orders = new();
    [ObservableProperty] private ObservableCollection<Transport> _transports = new();

    public HomeViewModel()
    {
        _apiHelper = new ApiHelper();
        _ = InitializeDataAsync();
    }

    private async Task InitializeDataAsync()
    {
        await Task.WhenAll(LoadTransports(), LoadOrders());
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