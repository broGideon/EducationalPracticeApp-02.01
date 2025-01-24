namespace EducationalPracticeApp.Models;

public class Transport
{
    public int? IdTransport { get; set; }

    public string Maker { get; set; } = null!;

    public string Model { get; set; } = null!;

    public string StNumber { get; set; } = null!;

    public int MaxPayload { get; set; }

    public int StatusId { get; set; }

    public virtual Status? Status { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is not Transport other)
            return false;

        return IdTransport == other.IdTransport && Maker == other.Maker && Model == other.Model &&
               StNumber == other.StNumber && MaxPayload == other.MaxPayload && StatusId == other.StatusId;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(IdTransport, Maker, Model, StNumber, MaxPayload, StatusId);
    }
}