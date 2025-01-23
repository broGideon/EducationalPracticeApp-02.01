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
}