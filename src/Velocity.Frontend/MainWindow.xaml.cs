using System.Windows;
using Microsoft.AspNetCore.Components.WebView.Wpf;

namespace Velocity.Frontend;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        var webView = new BlazorWebView
        {
            HostPage = @"wwwroot\index.html"
        };
        webView.Services = App.ServiceProvider;
        webView.RootComponents.Add(new RootComponent
        {
            ComponentType = typeof(Main),
            Selector = "#app"
        });
        Content = webView;
    }
}