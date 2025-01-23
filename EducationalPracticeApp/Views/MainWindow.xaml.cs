using System.Windows;

namespace EducationalPracticeApp.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        MainFrame.Content = new MainPage();
    }

    private void MainPageClick(object sender, RoutedEventArgs e)
    {
        MainFrame.Content = new MainPage();
    }

    private void AutoparkClick(object sender, RoutedEventArgs e)
    {
        MainFrame.Content = new AutoparkPage();
    }

    private void DriversClick(object sender, RoutedEventArgs e)
    {
        MainFrame.Content = new DriversPage();
    }

    private void OrdersClick(object sender, RoutedEventArgs e)
    {
        MainFrame.Content = new OrdersPage();
    }

    private void VoyageClick(object sender, RoutedEventArgs e)
    {
        MainFrame.Content = new VoyagePage();
    }

    private void ReportsClick(object sender, RoutedEventArgs e)
    {
        MainFrame.Content = new ReportsPage();
    }

    private void ClientsClick(object sender, RoutedEventArgs e)
    {
        MainFrame.Content = new ClientsPage();
    }
}