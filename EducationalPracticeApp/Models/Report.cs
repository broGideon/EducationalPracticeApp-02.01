namespace EducationalPracticeApp.Models;

public class Report
{
    public int? IdReport { get; set; }

    public int EmployeeId { get; set; }

    public string ReportType { get; set; } = null!;

    public DateOnly ReportDate { get; set; }

    public string ReportContent { get; set; } = null!;

    // public virtual Employee? Employee { get; set; }
}