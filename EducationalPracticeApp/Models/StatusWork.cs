using System.ComponentModel;

namespace EducationalPracticeApp.Models;

public enum StatusWork
{
    [Description("В работе")]
    Work = 1,
    [Description("Выполнено")]
    Completed = 2
}