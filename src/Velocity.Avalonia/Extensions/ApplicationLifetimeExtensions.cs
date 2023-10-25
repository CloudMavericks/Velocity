using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.Extensions.DependencyInjection;

namespace Velocity.Avalonia.Extensions;

internal static class ApplicationLifetimeExtensions
{
    public static void ChangeMainWindowAs<T>(this IApplicationLifetime lifetime) where T : Window
    {
        if (lifetime is not IClassicDesktopStyleApplicationLifetime classic) 
            return;
        var oldWindow = classic.MainWindow;
        classic.MainWindow = App.ServiceProvider.GetRequiredService<T>();
        classic.MainWindow?.Show();
        oldWindow?.Close();
    }
}