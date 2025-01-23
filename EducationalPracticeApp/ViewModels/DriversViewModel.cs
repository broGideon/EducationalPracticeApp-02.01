using System.Collections.ObjectModel;
using EducationalPracticeApp.Models;

namespace EducationalPracticeApp.ViewModels;

public partial class DriversViewModel: ObservableObject
{
    [ObservableProperty] private ObservableCollection<Driver> _drivers = new();
    [ObservableProperty] private Driver _selectedDriver = new();
    
}