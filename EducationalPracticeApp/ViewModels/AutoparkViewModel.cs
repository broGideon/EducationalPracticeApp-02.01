using System.Collections.ObjectModel;
using EducationalPracticeApp.Helper;
using EducationalPracticeApp.Models;

namespace EducationalPracticeApp.ViewModels;

public partial class AutoparkViewModel: ObservableObject
{
    [ObservableProperty] private ObservableCollection<Status> _statuses = new();
    [ObservableProperty] private ObservableCollection<Transport> _transports = new();
    [ObservableProperty] private Transport _selectedTransport = new();
    [ObservableProperty] private Status _selectedStatus;
    private readonly ApiHelper _apiHelper;

    public AutoparkViewModel()
    {
        _apiHelper = new ApiHelper();
    }
    private async Task LoadTransports()
    {
        var transports = await _apiHelper.Get<List<Transport>>("transport");

        if (transports != null)
            foreach (var transport in transports)
                transport.Status = Statuses.FirstOrDefault(s => s.IdStatus == transport.StatusId);

        Transports = new ObservableCollection<Transport>(transports ?? new List<Transport>());
    }

}