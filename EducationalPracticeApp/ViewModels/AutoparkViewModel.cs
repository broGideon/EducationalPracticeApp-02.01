using System.Collections.ObjectModel;
using System.Windows;
using EducationalPracticeApp.Helper;
using EducationalPracticeApp.Models;

namespace EducationalPracticeApp.ViewModels;

public partial class AutoparkViewModel : ObservableObject
{
    private readonly ApiHelper _apiHelper;
    [ObservableProperty] private Transport _editableTransport = new();
    [ObservableProperty] private ObservableCollection<Transport> _filteredTransports = new();
    [ObservableProperty] private Status? _selectedStatus;
    [ObservableProperty] private Transport? _selectedTransport = new();
    [ObservableProperty] private ObservableCollection<Status> _statuses = new();
    [ObservableProperty] private ObservableCollection<Transport> _transports = new();

    public AutoparkViewModel()
    {
        _apiHelper = new ApiHelper();
        _ = LoadData();
    }

    private async Task LoadData()
    {
        await Task.WhenAll(LoadTransports(), LoadStatuses());
    }

    private async Task LoadTransports()
    {
        var transports = await _apiHelper.Get<List<Transport>>("transport");
        Transports = new ObservableCollection<Transport>(transports ?? new List<Transport>());
        FilteredTransports = new ObservableCollection<Transport>(transports ?? new List<Transport>());
    }

    private async Task LoadStatuses()
    {
        var statuses = await _apiHelper.Get<List<Status>>("status");
        Statuses = new ObservableCollection<Status>(statuses ?? new List<Status>());
    }

    public void FilterTransport()
    {
        if (SelectedStatus != null)
            FilteredTransports =
                new ObservableCollection<Transport>(Transports.Where(t => t.Status == SelectedStatus).ToList());
        else
            FilteredTransports = new ObservableCollection<Transport>(Transports);
    }

    partial void OnSelectedTransportChanged(Transport? value)
    {
        if (value == null) return;
        EditableTransport = new Transport
        {
            IdTransport = value.IdTransport,
            Maker = value.Maker,
            Model = value.Model,
            StNumber = value.StNumber,
            StatusId = value.StatusId,
            Status = value.Status,
            MaxPayload = value.MaxPayload
        };
    }

    private bool CheckInputs()
    {
        if (string.IsNullOrWhiteSpace(EditableTransport.Maker))
        {
            MessageBox.Show("Введите производителя");
            return false;
        }

        if (string.IsNullOrWhiteSpace(EditableTransport.Model))
        {
            MessageBox.Show("Введите модель");
            return false;
        }

        if (string.IsNullOrWhiteSpace(EditableTransport.StNumber))
        {
            MessageBox.Show("Введите госномер");
            return false;
        }

        if (EditableTransport.MaxPayload < 1)
        {
            MessageBox.Show("Максимальная нагрузка должна быть больше нуля");
            return false;
        }

        if (EditableTransport.Status == null)
        {
            MessageBox.Show("Выберите состояние");
            return false;
        }

        EditableTransport.StatusId = (int)EditableTransport.Status.IdStatus!;
        return true;
    }

    private void ClearInputs()
    {
        FilterTransport();
        EditableTransport = new Transport();
    }

    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task AddTransport()
    {
        var check = CheckInputs();
        if (!check)
            return;
        var transport = await _apiHelper.Post<Transport>(EditableTransport, "transport");
        if (transport == null)
        {
            MessageBox.Show("Произошла ошибка");
            return;
        }

        transport.Status = EditableTransport.Status;
        Transports.Add(transport);
        ClearInputs();
    }

    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task UpdateTransport()
    {
        var check = CheckInputs();
        if (!check)
            return;
        if (EditableTransport.IdTransport == null)
        {
            MessageBox.Show("Транспорт не выбран");
            return;
        }

        var isUpdate =
            await _apiHelper.Put<Transport>(EditableTransport, "transport", (int)EditableTransport.IdTransport);
        if (!isUpdate)
        {
            MessageBox.Show("Произошла ошибка");
            return;
        }

        var existingTransport = Transports.First(t => t.IdTransport == EditableTransport.IdTransport);
        existingTransport.Status = EditableTransport.Status;
        existingTransport.Maker = EditableTransport.Maker;
        existingTransport.Model = EditableTransport.Model;
        existingTransport.StNumber = EditableTransport.StNumber;
        existingTransport.MaxPayload = EditableTransport.MaxPayload;
        existingTransport.StatusId = EditableTransport.StatusId;
        
        var index = Transports.IndexOf(existingTransport);
        Transports.RemoveAt(index);
        Transports.Insert(index, existingTransport);
        ClearInputs();
    }

    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task DeleteTransport()
    {
        if (SelectedTransport.IdTransport == null)
        {
            MessageBox.Show("Транспорт не выбран");
            return;
        }

        Console.WriteLine(SelectedTransport.IdTransport);
        var isDel = await _apiHelper.Delete("transport", (int)SelectedTransport.IdTransport);
        if (!isDel)
        {
            MessageBox.Show("Произошла ошибка");
            return;
        }

        Transports.Remove(SelectedTransport);
        ClearInputs();
    }
}