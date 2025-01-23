using System.Collections.ObjectModel;
using EducationalPracticeApp.Models;

namespace EducationalPracticeApp.ViewModels;

public partial class ClientsViewModel: ObservableObject
{
    [ObservableProperty] private ObservableCollection<Client> _clients = new();
    [ObservableProperty] private Client _selectedClient = new();

    public ClientsViewModel()
    {
        
    }
}