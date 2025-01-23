using System.Globalization;
using System.Windows.Data;

namespace EducationalPracticeApp.ViewModels;

public class DriverConverter: IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values == null || values.Length < 2)
            return string.Empty;

        string lastName = values[0]?.ToString() ?? string.Empty;
        string firstName = values[1]?.ToString() ?? string.Empty;
        string middleName = values.Length > 2 ? values[2]?.ToString() : string.Empty;

        // Форматируем как "Фамилия И.О."
        string initials = $"{lastName} {GetInitial(firstName)}{GetInitial(middleName)}".Trim();
        return initials;
    }

    private string GetInitial(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return string.Empty;

        return $"{name[0]}.";
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}