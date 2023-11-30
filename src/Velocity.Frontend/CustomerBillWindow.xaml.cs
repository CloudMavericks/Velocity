using System.Windows;
using Microsoft.AspNetCore.Components.WebView.Wpf;
using Velocity.Frontend.Pages.SalesInvoice;

namespace Velocity.Frontend;

public partial class CustomerBillWindow
{
    public CustomerBillWindow()
    {
        InitializeComponent();
        
        var webView = new BlazorWebView
        {
            HostPage = @"wwwroot\customer_bill.html",
            HorizontalAlignment = HorizontalAlignment.Stretch,
            HorizontalContentAlignment = HorizontalAlignment.Stretch
        };
        webView.Services = App.ServiceProvider;
        webView.RootComponents.Add(new RootComponent
        {
            ComponentType = typeof(NewSaleInvoice),
            Selector = "#customerBill",
            Parameters = new Dictionary<string, object>()
            {
                { "Window", this }
            }
        });
        Content = webView;
    }
}