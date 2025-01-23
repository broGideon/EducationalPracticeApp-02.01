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
}