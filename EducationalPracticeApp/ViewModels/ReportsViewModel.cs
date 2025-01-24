using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using EducationalPracticeApp.Helper;
using EducationalPracticeApp.Models;

namespace EducationalPracticeApp.ViewModels;

public partial class ReportsViewModel: ObservableObject
{
    [ObservableProperty] private ObservableCollection<Report> _reports = new();
    [ObservableProperty] private Report _selectedReport = new();
    [ObservableProperty] private string _message = "{тип отчета}, {дата генерации}";
    [ObservableProperty] private string? _selectedReportType;
    private readonly ApiHelper _apiHelper;

    public ReportsViewModel()
    {
        _apiHelper = new ApiHelper();
    }

    private async Task LoadReports()
    {
        List<Report>? reports = await _apiHelper.Get<List<Report>>("report");
        Reports = new ObservableCollection<Report>(reports ?? new List<Report>());
    }
    
    [RelayCommand]
    private void PrintReport()
    {
        // if (SelectedReport.IdReport == null)
        //     return;
        //TODO ЗДЕСЬ ЖИЗНИ НЕТ
    }

    [RelayCommand]
    private void OpenReport()
    {
        if (SelectedReport.IdReport == null)
        {
            MessageBox.Show("Выберите отчёт");   
            return;
        }
        string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)+ @"\Downloads\AutoLogistics\";
        string pathFilePdf = path + SelectedReport.ReportContent + ".pdf";
        if (File.Exists(pathFilePdf))
            Process.Start(new ProcessStartInfo(pathFilePdf) { UseShellExecute = true });
        else
            MessageBox.Show("Выбранного файла не существует");
    }

    partial void OnSelectedReportChanged(Report? value)
    {
        Message = $"{value?.ReportType}, {value?.ReportDate}";
    }

    partial void OnSelectedReportTypeChanged(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return;
        switch (value)
        {
            case "Загруженность автопарка":
                break;
            case "Статистика выполненных заказов за месяц":
                break;
        }
    }
}