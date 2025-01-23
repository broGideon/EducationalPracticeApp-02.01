using System.Collections.ObjectModel;
using EducationalPracticeApp.Models;

namespace EducationalPracticeApp.ViewModels;

public partial class VoyageViewModel: ObservableObject
{
    [ObservableProperty] private ObservableCollection<Voyage> _voyages = new();
    [ObservableProperty] private ObservableCollection<Client> _clients = new();
    [ObservableProperty] private Client _selectedClient = new();
    [ObservableProperty] private Voyage _selectedVoyage = new();
    [ObservableProperty] private StatusWork _selectedStatusWork;
    public Array StatusWorks { get; } = Enum.GetValues(typeof(StatusWork));
    public VoyageViewModel()
    {
        for (int i = 0; i < 20; i++)
        {
            var voyage = new Voyage();
            voyage.IdVoyage = i;
            voyage.Status = "asd";
            voyage.StartDate = DateOnly.FromDateTime(DateTime.Now);
            voyage.EndDate = DateOnly.FromDateTime(DateTime.Now);
            voyage.SendPoint = "asdfg";
            voyage.ArrivalPoint = "asdfg";
            var transport = new Transport();
            transport.Model = "asdfg";
            transport.Maker = "iuhg";
            voyage.Transport = transport;
            var driver = new Driver();
            driver.Surname = "Кирилов";
            driver.Name = "Дмитрий";
            driver.MiddleName = "Сергеевич";
            voyage.Driver = driver;
            _voyages.Add(voyage);
        }
    }
}