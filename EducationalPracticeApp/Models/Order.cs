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

    public string Status { get; set; }

    public virtual Client? Client { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is not Order other)
            return false;

        return IdOrder == other.IdOrder && ClientId == other.ClientId && OrderNum == other.OrderNum &&
               Weight == other.Weight && SendDate == other.SendDate && ArriveDate == other.ArriveDate &&
               Status == other.Status;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(IdOrder, ClientId, OrderNum, Weight, SendDate, ArriveDate, Status);
    }
}