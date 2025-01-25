using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using EducationalPracticeApp.Helper;
using EducationalPracticeApp.Models;

namespace EducationalPracticeApp.ViewModels;

public partial class DriversViewModel: ObservableObject
{
    [ObservableProperty] private ObservableCollection<Driver> _drivers = new();
    [ObservableProperty] private Driver _selectedDriver = new();
    [ObservableProperty] private Driver _editableDriver = new();

    private readonly ApiHelper _apiHelper;

    public DriversViewModel()
    {
        _apiHelper = new ApiHelper();
        _ = LoadData();
        OnSelectedDriverChanged(SelectedDriver);
    }

    private async Task LoadData()
    {
        await LoadDrivers();
    }

    private async Task LoadDrivers()
    {
        var drivers = await _apiHelper.Get<List<Driver>>("driver");
        Drivers = new ObservableCollection<Driver>(drivers ?? new List<Driver>());
    }

    partial void OnSelectedDriverChanged(Driver value)
    {
        EditableDriver = new Driver
        {
            IdDriver = value?.IdDriver,
            Surname = value?.Surname,
            Name = value?.Name,
            MiddleName = value?.MiddleName,
            Experience = value?.Experience ?? 0,
            Phone = value?.Phone,
            Email = value?.Email
        };
    }

    private bool CheckInputs()
    {
        if (string.IsNullOrWhiteSpace(EditableDriver.Surname))
        {
            MessageBox.Show("Введите фамилию водителя");
            return false;
        }
        else if (string.IsNullOrWhiteSpace(EditableDriver.Name))
        {
            MessageBox.Show("Введите имя водителя");
            return false;
        }
        else if (EditableDriver.Experience < 0)
        {
            MessageBox.Show("Стаж водителя не может быть отрицательным");
            return false;
        }
        else if (string.IsNullOrWhiteSpace(EditableDriver.Phone))
        {
            MessageBox.Show("Введите номер телефона водителя");
            return false;
        }
        else if (Regex.IsMatch(EditableDriver.Phone, @"^\d \(\d{3}\) \d{3}-\d{4}$"))
        {
            MessageBox.Show("Некоректный формат номера телефона");
            return false;
        } 
        else if (string.IsNullOrWhiteSpace(EditableDriver.Email))
        {
            MessageBox.Show("Введите email водителя");
            return false;
        }
        else if (Regex.IsMatch(EditableDriver.Email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
        {
            MessageBox.Show("Некоректный формат почты");
            return false;
        }
        return true;
    }

    private void ClearInputs()
    {
        EditableDriver = new();
    }

    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task AddDriver()
    {
        if (!CheckInputs())
            return;

        var driver = await _apiHelper.Post<Driver>(EditableDriver, "driver");
        if (driver == null)
        {
            MessageBox.Show("Произошла ошибка при добавлении водителя");
            return;
        }

        Drivers.Add(driver);
        ClearInputs();
    }

    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task UpdateDriver()
    {
        if (!CheckInputs())
            return;

        if (EditableDriver.IdDriver == null)
        {
            MessageBox.Show("Водитель не выбран");
            return;
        }

        var isUpdated = await _apiHelper.Put<Driver>(EditableDriver, "driver", (int)EditableDriver.IdDriver);
        if (!isUpdated)
        {
            MessageBox.Show("Произошла ошибка при обновлении водителя");
            return;
        }

        var existingDriver = Drivers.First(d => d.IdDriver == EditableDriver.IdDriver);
        existingDriver.Surname = EditableDriver.Surname;
        existingDriver.Name = EditableDriver.Name;
        existingDriver.MiddleName = EditableDriver.MiddleName;
        existingDriver.Experience = EditableDriver.Experience;
        existingDriver.Phone = EditableDriver.Phone;
        existingDriver.Email = EditableDriver.Email;

        int index = Drivers.IndexOf(existingDriver);
        Drivers.RemoveAt(index);
        Drivers.Insert(index, existingDriver);

        ClearInputs();
    }

    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task DeleteDriver()
    {
        if (SelectedDriver.IdDriver == null)
        {
            MessageBox.Show("Водитель не выбран");
            return;
        }

        var isDeleted = await _apiHelper.Delete("driver", (int)SelectedDriver.IdDriver);
        if (!isDeleted)
        {
            MessageBox.Show("Произошла ошибка при удалении водителя");
            return;
        }

        Drivers.Remove(SelectedDriver);
        ClearInputs();
    }
}