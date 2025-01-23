namespace EducationalPracticeApp.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty] private string _message;

    public MainViewModel()
    {
        Message = "Hello";
    }

    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task UpdateMessageAsync()
    {
        await Task.Delay(2000);
        Message = "Сообщение обновлено!";
    }
}