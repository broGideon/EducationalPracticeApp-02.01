namespace EducationalPracticeApp.Models;

public class Employee
{
    public int? IdEmployee { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int RoleId { get; set; }

    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string Email { get; set; } = null!;

    public virtual Role? Role { get; set; }
}