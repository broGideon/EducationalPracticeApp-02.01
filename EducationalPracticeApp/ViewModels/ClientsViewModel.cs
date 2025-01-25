using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using EducationalPracticeApp.Helper;
using EducationalPracticeApp.Models;

namespace EducationalPracticeApp.ViewModels;

public partial class ClientsViewModel: ObservableObject
{
    [ObservableProperty] private ObservableCollection<Client> _clients = new();
    [ObservableProperty] private Client _selectedClient = new();
    [ObservableProperty] private Client _editableClient = new();

    private readonly ApiHelper _apiHelper;

    public ClientsViewModel()
    {
        _apiHelper = new ApiHelper();
        _ = LoadData();
        OnSelectedClientChanged(SelectedClient);
    }

    private async Task LoadData()
    {
        await LoadClients();
    }

    private async Task LoadClients()
    {
        var clients = await _apiHelper.Get<List<Client>>("client");
        Clients = new ObservableCollection<Client>(clients ?? new List<Client>());
    }

    partial void OnSelectedClientChanged(Client value)
    {
        EditableClient = new Client
        {
            IdClient = value?.IdClient,
            FullName = value?.FullName,
            Phone = value?.Phone,
            Email = value?.Email
        };
    }

    private bool CheckInputs()
    {
        if (string.IsNullOrWhiteSpace(EditableClient.FullName))
        {
            MessageBox.Show("Введите полное имя клиента");
            return false;
        }
        else if (string.IsNullOrWhiteSpace(EditableClient.Phone))
        {
            MessageBox.Show("Введите номер телефона клиента");
            return false;
        }
        else if (Regex.IsMatch(EditableClient.Phone, @"^\d \(\d{3}\) \d{3}-\d{4}$"))
        {
            MessageBox.Show("Некоректный формат номера телефона");
            return false;
        } 
        else if (string.IsNullOrWhiteSpace(EditableClient.Email))
        {
            MessageBox.Show("Введите email клиента");
            return false;
        }
        else if (Regex.IsMatch(EditableClient.Email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
        {
            MessageBox.Show("Некоректный формат почты");
            return false;
        }
        return true;
    }

    private void ClearInputs()
    {
        EditableClient = new();
    }

    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task AddClient()
    {
        if (!CheckInputs())
            return;

        var client = await _apiHelper.Post<Client>(EditableClient, "client");
        if (client == null)
        {
            MessageBox.Show("Произошла ошибка при добавлении клиента");
            return;
        }

        Clients.Add(client);
        ClearInputs();
    }

    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task UpdateClient()
    {
        if (!CheckInputs())
            return;

        if (EditableClient.IdClient == null)
        {
            MessageBox.Show("Клиент не выбран");
            return;
        }

        var isUpdated = await _apiHelper.Put<Client>(EditableClient, "client", (int)EditableClient.IdClient);
        if (!isUpdated)
        {
            MessageBox.Show("Произошла ошибка при обновлении клиента");
            return;
        }

        var existingClient = Clients.First(c => c.IdClient == EditableClient.IdClient);
        existingClient.FullName = EditableClient.FullName;
        existingClient.Phone = EditableClient.Phone;
        existingClient.Email = EditableClient.Email;

        int index = Clients.IndexOf(existingClient);
        Clients.RemoveAt(index);
        Clients.Insert(index, existingClient);

        ClearInputs();
    }

    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task DeleteClient()
    {
        if (SelectedClient.IdClient == null)
        {
            MessageBox.Show("Клиент не выбран");
            return;
        }

        var isDeleted = await _apiHelper.Delete("client", (int)SelectedClient.IdClient);
        if (!isDeleted)
        {
            MessageBox.Show("Произошла ошибка при удалении клиента");
            return;
        }

        Clients.Remove(SelectedClient);
        ClearInputs();
    }
}