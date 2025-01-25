using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using EducationalPracticeApp.Helper;
using EducationalPracticeApp.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

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
        string pathFilePdf = GetPath() + SelectedReport.ReportContent + ".pdf";
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
                _ = GenerateOrdersReport();
                break;
        }
    }
    
    private async Task GenerateOrdersReport()
    {
        List<Order>? orders = await _apiHelper.Get<List<Order>>("order/report");
        if (orders == null)
        {
            MessageBox.Show("Произошла ошибка");
            return;
        }

        string path = GetPath();
        DirectoryIfExists(path);
        using var pdfWriter = new PdfWriter(path + "OrdersReport.pdf");
        using var pdfDocument = new PdfDocument(pdfWriter);
        var document = new Document(pdfDocument);

        document.Add(new Paragraph("Статистика выполненных заказов за месяц").SetBold().SetFontSize(18));
        document.Add(new Paragraph($"Дата создания отчета: {DateTime.Now:dd.MM.yyyy}").SetFontSize(12));

        var table = new Table(5);
        table.AddHeaderCell("Дата отправки");
        table.AddHeaderCell("Дата завершения");
        table.AddHeaderCell("Имя клиента");
        table.AddHeaderCell("Вес заказа");
        table.AddHeaderCell("Описание");

        foreach (var order in orders.Where(o => o.Status == "Выполнено"))
        {
            table.AddCell(order.SendDate.ToString("dd.MM.yyyy"));
            table.AddCell(order.ArriveDate?.ToString("dd.MM.yyyy") ?? "Не завершен");
            table.AddCell(order.Client.FullName);
            table.AddCell(order.Weight.ToString());
            table.AddCell(order.Description);
        }

        document.Add(table);
        document.Close();
    }
    
    private async Task GenerateTransportReport()
    {
        List<Voyage>? voyages = await _apiHelper.Get<List<Voyage>>("voyage/report");
        if (voyages == null)
        {
            MessageBox.Show("Произошла ошибка");
            return;
        }

        string path = GetPath();
        DirectoryIfExists(path);
        using var pdfWriter = new PdfWriter(path + "TransportReport.pdf");
        using var pdfDocument = new PdfDocument(pdfWriter);
        var document = new Document(pdfDocument);

        document.Add(new Paragraph("Загруженность автопарка").SetBold().SetFontSize(18));
        document.Add(new Paragraph($"Дата создания отчета: {DateTime.Now:dd.MM.yyyy}").SetFontSize(12));

        foreach (var group in voyages.GroupBy(v => v.Transport.StNumber))
        {
            document.Add(new Paragraph($"Транспорт: {group.Key}").SetBold());

            var table = new Table(4);
            table.AddHeaderCell("Рейс");
            table.AddHeaderCell("Дата начала");
            table.AddHeaderCell("Дата окончания");
            table.AddHeaderCell("Грузоподъемность");

            foreach (var voyage in group)
            {
                table.AddCell(voyage.IdVoyage.ToString());
                table.AddCell(voyage.StartDate.ToString("dd.MM.yyyy"));
                table.AddCell(voyage.EndDate?.ToString("dd.MM.yyyy") ?? "Не завершён");
                table.AddCell(voyage.Transport.MaxPayload.ToString());
            }
            
            var totalHours = group.Sum(v => 
            {
                var endDate = v.EndDate?.ToDateTime(new TimeOnly(0, 0)) ?? DateTime.Now;
                var startDate = v.StartDate.ToDateTime(new TimeOnly(0, 0));

                return (endDate - startDate).TotalHours;
            });
            document.Add(table);
            document.Add(new Paragraph($"Общее время работы: {totalHours:F2} часов"));
        }

        document.Close();
    }


    private void DirectoryIfExists(string path)
    {
        if (Directory.Exists(path)) Directory.CreateDirectory(path);
    }
    
    private string GetPath() => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)+ @"\Downloads\AutoLogistics\";
}