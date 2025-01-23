namespace EducationalPracticeApp.Models;

public class Voyage
{
    public int? IdVoyage { get; set; }

    public int TransportId { get; set; }

    public int OrderId { get; set; }

    public int DriverId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public string SendPoint { get; set; } = null!;

    public string ArrivalPoint { get; set; } = null!;

    public int Status { get; set; }

    public virtual Driver? Driver { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Transport? Transport { get; set; }
}