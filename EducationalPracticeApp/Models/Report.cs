namespace EducationalPracticeApp.Models;

public class Report
{
    public int? IdReport { get; set; }

    public int EmployeeId { get; set; }

    public int ReportType { get; set; }

    public DateOnly ReportDate { get; set; }

    public string ReportContent { get; set; } = null!;

    // public virtual Employee? Employee { get; set; }
}
