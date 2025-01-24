namespace EducationalPracticeApp.Models;

public class Driver
{
    public int? IdDriver { get; set; }

    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? MiddleName { get; set; }

    public int Experience { get; set; }

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public override bool Equals(object? obj)
    {
        if (obj is not Driver other)
            return false;

        return IdDriver == other.IdDriver && Surname == other.Surname && Name == other.Name &&
               MiddleName == other.MiddleName && Experience == other.Experience && Phone == other.Phone &&
               Email == other.Email;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(IdDriver, Surname, Name, MiddleName, Experience, Phone, Email);
    }
}