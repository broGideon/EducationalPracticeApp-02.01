using System.Windows;

namespace EducationalPracticeApp.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        MainFraim.Content = new MainPage();
    }

    private void MainPageClick(object sender, RoutedEventArgs e)
    {
        MainFraim.Content = new MainPage();
    }

    private void AutoparkClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void DriversClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OrdersClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void VoyageClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void ReportsClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void ClientsClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }
}