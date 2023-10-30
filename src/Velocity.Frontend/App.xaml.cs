using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Velocity.Frontend.Extensions;

namespace Velocity.Frontend;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    public static IServiceProvider ServiceProvider { get; private set; }
    public const string BaseAddress = "https://localhost:7205";
    
    public App()
    {
        ServiceProvider = DependencyInjection.Initialize();
    }
    
    public static void ChangeMainWindowAs<T>() where T : Window
    {
        var oldWindow = Current.MainWindow;
        Current.MainWindow = ServiceProvider.GetRequiredService<T>();
        Current.MainWindow?.Show();
        oldWindow?.Close();
    }

    private void App_OnStartup(object sender, StartupEventArgs e)
    {
        MainWindow = ServiceProvider.GetRequiredService<LoginWindow>();
        if (MainWindow == null)
        {
            throw new NullReferenceException("MainWindow is null");
        }
        MainWindow.Show();
    }
}