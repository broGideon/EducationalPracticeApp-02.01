using System.Windows;
using EducationalPracticeApp.ViewModels;
using EducationalPracticeApp.Views;

namespace EducationalPracticeApp;

public partial class AuthWindow : Window
{
    public AuthWindow()
    {
        InitializeComponent();
        AuthViewModel viewModel = new AuthViewModel();
        DataContext = viewModel;
        viewModel.OpenMainWindow += (_, _) => WindowLoaded();
    }

    private void WindowLoaded()
    {
        MainWindow window = new MainWindow();
        window.Show();
        this.Close();
    }
}