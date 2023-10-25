using Microsoft.Extensions.DependencyInjection;
using Velocity.Avalonia.ViewModels;

namespace Velocity.Avalonia.Views;

public partial class LoginWindow : WindowBase<LoginViewModel>
{
    public LoginWindow()
    {
        DataContext = App.ServiceProvider.GetRequiredService<LoginViewModel>();
        InitializeComponent();
    }
}