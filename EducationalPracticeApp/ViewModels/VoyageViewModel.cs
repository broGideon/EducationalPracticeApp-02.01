using System.Collections.ObjectModel;
using EducationalPracticeApp.Models;

namespace EducationalPracticeApp.ViewModels;

public partial class VoyageViewModel: ObservableObject
{
    [ObservableProperty] private ObservableCollection<Voyage> _voyage = new();
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
            _voyage.Add(voyage);
        }
    }
}