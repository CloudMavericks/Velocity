using Microsoft.Extensions.DependencyInjection;
using Velocity.Avalonia.ViewModels;

namespace Velocity.Avalonia.Views;

public partial class MainWindow : WindowBase<MainViewModel>
{
    public MainWindow()
    {
        DataContext = App.ServiceProvider.GetRequiredService<MainViewModel>();
        InitializeComponent();
    }
}