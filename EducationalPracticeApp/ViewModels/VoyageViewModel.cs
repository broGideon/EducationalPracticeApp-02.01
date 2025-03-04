using System.Collections.ObjectModel;
using System.Windows;
using EducationalPracticeApp.Helper;
using EducationalPracticeApp.Models;

namespace EducationalPracticeApp.ViewModels;

public partial class VoyageViewModel : ObservableObject
{
    private readonly ApiHelper _apiHelper;
    [ObservableProperty] private ObservableCollection<Client> _clients = new();
    [ObservableProperty] private ObservableCollection<Driver> _drivers = new();
    [ObservableProperty] private Voyage _editableVoyage = new();
    [ObservableProperty] private DateTime? _endDate;
    [ObservableProperty] private ObservableCollection<Voyage> _filteredVoyages = new();
    [ObservableProperty] private ObservableCollection<Order> _orders = new();

    [ObservableProperty] private Client? _selectedClientFilter;
    [ObservableProperty] private string? _selectedStatusFilter;

    [ObservableProperty] private Voyage? _selectedVoyage = new();
    [ObservableProperty] private DateTime? _startDate;
    [ObservableProperty] private ObservableCollection<Transport> _transports = new();
    [ObservableProperty] private ObservableCollection<Voyage> _voyages = new();

    public VoyageViewModel()
    {
        _apiHelper = new ApiHelper();
        _ = LoadData();
    }

    public string[] StatusWorks { get; } = ["В работе", "Выполнено"];

    private async Task LoadData()
    {
        await Task.WhenAll(LoadVoyages(), LoadClients(), LoadTransports(), LoadOrders(), LoadTransports(),
            LoadDrivers());
    }

    private async Task LoadVoyages()
    {
        var voyages = await _apiHelper.Get<List<Voyage>>("voyage");
        Voyages = new ObservableCollection<Voyage>(voyages ?? new List<Voyage>());
        FilteredVoyages = new ObservableCollection<Voyage>(voyages ?? new List<Voyage>());
    }

    private async Task LoadClients()
    {
        var clients = await _apiHelper.Get<List<Client>>("client");
        Clients = new ObservableCollection<Client>(clients ?? new List<Client>());
    }

    private async Task LoadOrders()
    {
        var orders = await _apiHelper.Get<List<Order>>("order");
        Orders = new ObservableCollection<Order>(orders ?? new List<Order>());
    }

    private async Task LoadDrivers()
    {
        var drivers = await _apiHelper.Get<List<Driver>>("driver");
        Drivers = new ObservableCollection<Driver>(drivers ?? new List<Driver>());
    }

    private async Task LoadTransports()
    {
        var transports = await _apiHelper.Get<List<Transport>>("transport");
        Transports = new ObservableCollection<Transport>(transports ?? new List<Transport>());
    }

    partial void OnSelectedVoyageChanged(Voyage? value)
    {
        if (value == null) return;
        EditableVoyage = new Voyage
        {
            IdVoyage = value?.IdVoyage,
            Driver = value.Driver,
            Order = value.Order,
            Transport = value.Transport,
            SendPoint = value?.SendPoint,
            ArrivalPoint = value?.ArrivalPoint,
            Status = value?.Status
        };
        StartDate = value.StartDate.ToDateTime(TimeOnly.MinValue);
        EndDate = value.EndDate == null ? null : value.EndDate.Value.ToDateTime(TimeOnly.MinValue);
    }

    private bool CheckInputs()
    {
        if (EditableVoyage.Transport == null)
        {
            MessageBox.Show("Выберите транспорт");
            return false;
        }

        if (EditableVoyage.Order == null)
        {
            MessageBox.Show("Выберите заказ");
            return false;
        }

        if (EditableVoyage.Driver == null)
        {
            MessageBox.Show("Выберите водителя");
            return false;
        }

        if (StartDate == null)
        {
            MessageBox.Show("Введите корректную дату");
            return false;
        }

        if (string.IsNullOrWhiteSpace(EditableVoyage.Status))
        {
            MessageBox.Show("Укажите статус рейса");
            return false;
        }

        if (string.IsNullOrWhiteSpace(EditableVoyage.SendPoint))
        {
            MessageBox.Show("Введите точку отправления");
            return false;
        }

        if (string.IsNullOrWhiteSpace(EditableVoyage.ArrivalPoint))
        {
            MessageBox.Show("Введите точку назначения");
            return false;
        }

        EditableVoyage.DriverId = (int)EditableVoyage.Driver.IdDriver!;
        EditableVoyage.OrderId = (int)EditableVoyage.Order.IdOrder!;
        EditableVoyage.TransportId = (int)EditableVoyage.Transport.IdTransport!;
        EditableVoyage.StartDate = DateOnly.FromDateTime((DateTime)StartDate);
        EditableVoyage.EndDate = EndDate != null ? DateOnly.FromDateTime((DateTime)EndDate) : null;
        return true;
    }

    private void ClearInputs()
    {
        EditableVoyage = new Voyage();
        SelectedVoyage = null;
        StartDate = null;
        EndDate = null;
        ApplyFilters();
    }

    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task AddVoyage()
    {
        if (!CheckInputs())
            return;

        var voyage = await _apiHelper.Post<Voyage>(EditableVoyage, "voyage");
        if (voyage == null)
        {
            MessageBox.Show("Произошла ошибка при добавлении рейса");
            return;
        }

        voyage.Transport = EditableVoyage.Transport;
        voyage.Order = EditableVoyage.Order;
        voyage.Driver = EditableVoyage.Driver;
        Voyages.Add(voyage);
        ClearInputs();
    }

    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task UpdateVoyage()
    {
        if (!CheckInputs())
            return;

        if (EditableVoyage.IdVoyage == null)
        {
            MessageBox.Show("Рейс не выбран");
            return;
        }

        var isUpdated = await _apiHelper.Put<Voyage>(EditableVoyage, "voyage", (int)EditableVoyage.IdVoyage);
        if (!isUpdated)
        {
            MessageBox.Show("Произошла ошибка при обновлении рейса");
            return;
        }

        var existingVoyage = Voyages.First(v => v.IdVoyage == EditableVoyage.IdVoyage);
        existingVoyage.TransportId = EditableVoyage.TransportId;
        existingVoyage.Transport = EditableVoyage.Transport;
        existingVoyage.OrderId = EditableVoyage.OrderId;
        existingVoyage.Order = EditableVoyage.Order;
        existingVoyage.DriverId = EditableVoyage.DriverId;
        existingVoyage.Driver = EditableVoyage.Driver;
        existingVoyage.StartDate = EditableVoyage.StartDate;
        existingVoyage.EndDate = EditableVoyage.EndDate;
        existingVoyage.SendPoint = EditableVoyage.SendPoint;
        existingVoyage.ArrivalPoint = EditableVoyage.ArrivalPoint;
        existingVoyage.Status = EditableVoyage.Status;
        var index = Voyages.IndexOf(existingVoyage);
        Voyages.RemoveAt(index);
        Voyages.Insert(index, existingVoyage);
        ClearInputs();
    }

    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task DeleteVoyage()
    {
        if (SelectedVoyage.IdVoyage == null)
        {
            MessageBox.Show("Рейс не выбран");
            return;
        }

        var isDeleted = await _apiHelper.Delete("voyage", (int)SelectedVoyage.IdVoyage);
        if (!isDeleted)
        {
            MessageBox.Show("Произошла ошибка при удалении рейса");
            return;
        }

        Voyages.Remove(SelectedVoyage);
        ClearInputs();
    }

    private void ApplyFilters()
    {
        var filtered = Voyages.AsEnumerable();

        if (SelectedClientFilter != null)
            filtered = filtered.Where(v => v.Order?.ClientId == SelectedClientFilter.IdClient);

        if (SelectedStatusFilter != null) filtered = filtered.Where(v => v.Status == SelectedStatusFilter);

        FilteredVoyages = new ObservableCollection<Voyage>(filtered);
    }

    partial void OnSelectedClientFilterChanged(Client? value)
    {
        ApplyFilters();
    }

    partial void OnSelectedStatusFilterChanged(string? value)
    {
        ApplyFilters();
    }
}