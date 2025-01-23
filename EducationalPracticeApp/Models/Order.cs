namespace EducationalPracticeApp.Models;

public class Order
{
    public int? IdOrder { get; set; }

    public int ClientId { get; set; }

    public string OrderNum { get; set; } = null!;

    public string Description { get; set; } = null!;

    public double Weight { get; set; }

    public DateOnly SendDate { get; set; }

    public DateOnly? ArriveDate { get; set; }

    public int Status { get; set; }

    public virtual Client? Client { get; set; }
}