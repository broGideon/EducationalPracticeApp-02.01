using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace EducationalPracticeApp.ViewModels;

public class EnumDescriptionConverter: IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Enum enumValue)
        {
            var description = enumValue.GetType()
                .GetField(enumValue.ToString())
                ?.GetCustomAttributes(typeof(DescriptionAttribute), false)
                .FirstOrDefault() as DescriptionAttribute;

            return description?.Description ?? enumValue.ToString();
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string stringValue && targetType.IsEnum)
        {
            foreach (var field in targetType.GetFields())
            {
                var description = field.GetCustomAttributes(typeof(DescriptionAttribute), false)
                    .FirstOrDefault() as DescriptionAttribute;

                if ((description?.Description ?? field.Name) == stringValue)
                {
                    return Enum.Parse(targetType, field.Name);
                }
            }
        }
        return Binding.DoNothing;
    }
}