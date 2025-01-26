namespace EducationalPracticeApp.Models;

public class Client
{
    public int? IdClient { get; set; }

    public string FullName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public override bool Equals(object? obj)
    {
        if (obj is not Client other)
            return false;

        return IdClient == other.IdClient && FullName == other.FullName && Phone == other.Phone;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(IdClient, FullName, Phone, Email);
    }
}