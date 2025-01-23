using System.Collections.ObjectModel;
using EducationalPracticeApp.Models;

namespace EducationalPracticeApp.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty] private string _message;
    [ObservableProperty] private ObservableCollection<Transport> _transports = new ObservableCollection<Transport>();

    public MainViewModel()
    {
        Message = "Hello";
        for (int i = 0; i < 30; i++)
        {
            var transport = new Transport();
            transport.IdTransport = i;
            transport.Maker = "asd";
            transport.Model = "asd";
            transport.MaxPayload = 5;
            transport.StNumber = "asd";
            transport.StatusId = i;
            var status = new Status();
            status.IdStatus = i;
            status.StatusName = "asd";
            transport.Status = status;
            _transports.Add(transport);
        }
    }

    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task UpdateMessageAsync()
    {
        await Task.Delay(2000);
        Message = "Сообщение обновлено!";
    }
}