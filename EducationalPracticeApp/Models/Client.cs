namespace EducationalPracticeApp.Models;

public class Client
{
    public int? IdClient { get; set; }

    public string FullName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;
}
