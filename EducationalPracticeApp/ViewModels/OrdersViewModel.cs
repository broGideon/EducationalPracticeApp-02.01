using System.Collections.ObjectModel;
using System.Windows;
using EducationalPracticeApp.Helper;
using EducationalPracticeApp.Models;

namespace EducationalPracticeApp.ViewModels;

public partial class OrdersViewModel : ObservableObject
{
    private readonly ApiHelper _apiHelper;
    [ObservableProperty] private DateTime? _arriveDate;
    [ObservableProperty] private ObservableCollection<Client> _clients = new();
    [ObservableProperty] private Order _editableOrder = new();
    [ObservableProperty] private ObservableCollection<Order> _filteredOrders = new();
    [ObservableProperty] private ObservableCollection<Order> _orders = new();

    [ObservableProperty] private Client? _selectedClientFilter;

    [ObservableProperty] private Order? _selectedOrder = new();
    [ObservableProperty] private string? _selectedStatusFilter;
    [ObservableProperty] private DateTime? _sendDate;

    public OrdersViewModel()
    {
        _apiHelper = new ApiHelper();
        _ = LoadData();
    }

    public string[] StatusWorks { get; } = ["В работе", "Выполнено"];

    private async Task LoadData()
    {
        await Task.WhenAll(LoadOrders(), LoadClients());
    }

    private async Task LoadOrders()
    {
        var orders = await _apiHelper.Get<List<Order>>("order");
        Orders = new ObservableCollection<Order>(orders ?? new List<Order>());
        FilteredOrders = new ObservableCollection<Order>(orders ?? new List<Order>());
    }

    private async Task LoadClients()
    {
        var clients = await _apiHelper.Get<List<Client>>("client");
        Clients = new ObservableCollection<Client>(clients ?? new List<Client>());
    }

    partial void OnSelectedOrderChanged(Order? value)
    {
        if (value == null) return;
        EditableOrder = new Order
        {
            IdOrder = value?.IdOrder,
            Client = value?.Client,
            OrderNum = value?.OrderNum,
            Description = value?.Description,
            Weight = value?.Weight ?? 0,
            Status = value?.Status
        };
        SendDate = value?.SendDate.ToDateTime(TimeOnly.MinValue);
        ArriveDate = value?.ArriveDate == null ? null : value.ArriveDate.Value.ToDateTime(TimeOnly.MinValue);
    }

    private bool CheckInputs()
    {
        if (EditableOrder.Client == null)
        {
            MessageBox.Show("Выберите клиента");
            return false;
        }

        if (string.IsNullOrWhiteSpace(EditableOrder.Description))
        {
            MessageBox.Show("Введите описание");
            return false;
        }

        if (SendDate == null)
        {
            MessageBox.Show("Неправильная дата");
        }
        else if (EditableOrder.Weight <= 0)
        {
            MessageBox.Show("Укажите вес");
            return false;
        }
        else if (string.IsNullOrWhiteSpace(EditableOrder.Status))
        {
            MessageBox.Show("Укажите статус заказа");
            return false;
        }

        Random random = new();
        EditableOrder.ClientId = (int)EditableOrder.Client.IdClient!;
        EditableOrder.OrderNum = $"{random.Next(100, 1000)}-{random.Next(100, 1000)}-{random.Next(100, 1000)}";
        EditableOrder.SendDate = DateOnly.FromDateTime((DateTime)SendDate!);
        EditableOrder.ArriveDate = ArriveDate == null ? null : DateOnly.FromDateTime((DateTime)ArriveDate);
        return true;
    }

    private void ClearInputs()
    {
        SelectedOrder = null;
        EditableOrder = new Order();
        SendDate = null;
        ArriveDate = null;
        ApplyFilters();
    }

    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task AddOrder()
    {
        if (!CheckInputs())
            return;

        var order = await _apiHelper.Post<Order>(EditableOrder, "order");
        if (order == null)
        {
            MessageBox.Show("Произошла ошибка при добавлении заказа");
            return;
        }

        order.Client = EditableOrder.Client;
        Orders.Add(order);
        ClearInputs();
    }

    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task UpdateOrder()
    {
        if (!CheckInputs())
            return;

        if (EditableOrder.IdOrder == null)
        {
            MessageBox.Show("Заказ не выбран");
            return;
        }

        var isUpdated = await _apiHelper.Put<Order>(EditableOrder, "order", (int)EditableOrder.IdOrder);
        if (!isUpdated)
        {
            MessageBox.Show("Произошла ошибка при обновлении заказа");
            return;
        }

        var existingOrder = Orders.First(o => o.IdOrder == EditableOrder.IdOrder);
        existingOrder.ClientId = EditableOrder.ClientId;
        existingOrder.OrderNum = EditableOrder.OrderNum;
        existingOrder.Description = EditableOrder.Description;
        existingOrder.Weight = EditableOrder.Weight;
        existingOrder.SendDate = EditableOrder.SendDate;
        existingOrder.ArriveDate = EditableOrder.ArriveDate;
        existingOrder.Status = EditableOrder.Status;
        existingOrder.Client = EditableOrder.Client;
        var index = Orders.IndexOf(existingOrder);
        Orders.RemoveAt(index);
        Orders.Insert(index, existingOrder);
        ClearInputs();
    }

    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task DeleteOrder()
    {
        if (SelectedOrder.IdOrder == null)
        {
            MessageBox.Show("Заказ не выбран");
            return;
        }

        var isDeleted = await _apiHelper.Delete("order", (int)SelectedOrder.IdOrder);
        if (!isDeleted)
        {
            MessageBox.Show("Произошла ошибка при удалении заказа");
            return;
        }

        Orders.Remove(SelectedOrder);
        ClearInputs();
    }

    private void ApplyFilters()
    {
        var filtered = Orders.AsEnumerable();

        if (SelectedClientFilter != null) filtered = filtered.Where(o => o.ClientId == SelectedClientFilter.IdClient);

        if (SelectedStatusFilter != null) filtered = filtered.Where(o => o.Status == SelectedStatusFilter);

        FilteredOrders = new ObservableCollection<Order>(filtered);
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