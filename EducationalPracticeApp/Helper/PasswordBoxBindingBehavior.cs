using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace EducationalPracticeApp.Helper;

public class PasswordBoxBindingBehavior : Behavior<PasswordBox>
{
    public static readonly DependencyProperty PasswordProperty =
        DependencyProperty.Register(
            nameof(Password),
            typeof(string),
            typeof(PasswordBoxBindingBehavior),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public string Password
    {
        get => (string)GetValue(PasswordProperty);
        set => SetValue(PasswordProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.PasswordChanged += OnPasswordChanged;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.PasswordChanged -= OnPasswordChanged;
    }

    private void OnPasswordChanged(object sender, RoutedEventArgs e)
    {
        if (AssociatedObject.Password != Password) Password = AssociatedObject.Password;
    }
}