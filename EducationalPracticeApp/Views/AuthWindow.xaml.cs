using System.Windows;
using EducationalPracticeApp.ViewModels;
using EducationalPracticeApp.Views;

namespace EducationalPracticeApp;

public partial class AuthWindow : Window
{
    public AuthWindow()
    {
        InitializeComponent();
        var viewModel = new AuthViewModel();
        DataContext = viewModel;
        viewModel.OpenMainWindow += (_, _) => WindowLoaded();
    }

    private void WindowLoaded()
    {
        var window = new MainWindow();
        window.Show();
        Close();
    }
}