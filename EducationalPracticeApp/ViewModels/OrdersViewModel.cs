using System.Collections.ObjectModel;
using EducationalPracticeApp.Models;

namespace EducationalPracticeApp.ViewModels;

public partial class OrdersViewModel: ObservableObject
{
    [ObservableProperty] private ObservableCollection<Order> _orders = new();
    [ObservableProperty] private StatusWork _selectedStatusWork;
    public Array StatusWorks { get; } = Enum.GetValues(typeof(StatusWork));
    
    public OrdersViewModel()
    {
        
    }
}