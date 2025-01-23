namespace EducationalPracticeApp.ViewModels;

public partial class MainViewModel: ObservableObject
{
    [ObservableProperty]
    private string message;

    public MainViewModel()
    {
        Message = "Hello";
    }

    [RelayCommand]
    private async Task UpdateMessageAsync()
    {
        await Task.Delay(2000);
        Message = "Сообщение обновлено!";
    }
}