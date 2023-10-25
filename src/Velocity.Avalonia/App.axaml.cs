using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Velocity.Avalonia.Views;

namespace Velocity.Avalonia;

public partial class App : Application
{
    public static IServiceProvider ServiceProvider { get; set; }
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = ServiceProvider.GetRequiredService<LoginWindow>();
        }

        base.OnFrameworkInitializationCompleted();
    }
    
    public static IApplicationLifetime GetApplicationLifetime()
    {
        return Current?.ApplicationLifetime ?? throw new NullReferenceException("ApplicationLifetime is null"); 
    }
    
    public static Window GetMainWindow()
    {
        return GetApplicationLifetime() switch
        {
            IClassicDesktopStyleApplicationLifetime classic => classic.MainWindow,
            _ => throw new NotSupportedException("Only ClassicDesktopStyleApplicationLifetime is supported")
        };
    }
    
    public static void Shutdown()
    {
        if (GetApplicationLifetime() is IClassicDesktopStyleApplicationLifetime lifetime)
        {
            lifetime.Shutdown();
        }
        else
        {
            Environment.Exit(0);
        }
    }
}