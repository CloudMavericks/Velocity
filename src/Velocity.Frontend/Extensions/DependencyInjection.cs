using System.Net.Http;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using Velocity.Frontend.Services;
using Velocity.Frontend.Services.HttpClients;

namespace Velocity.Frontend.Extensions;

public static class DependencyInjection
{
    public static IServiceProvider Initialize()
    {
        var services = new ServiceCollection();
        services.AddSingleton<LoginWindow>();
        services.AddSingleton<MainWindow>();
        services.AddWpfBlazorWebView();
        services.AddAuthorizationCore();
        services.AddBlazorWebViewDeveloperTools();
        services.AddHttpClient("VelocityAPI", client =>
        {
            client.BaseAddress = new Uri(App.BaseAddress);
        });
        services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("VelocityAPI"));
        services.AddTransient<AuthenticationStateProvider, VelocityAuthenticationStateProvider>();
        services.AddTransient<VelocityAuthenticationStateProvider>();
        services.AddSingleton<LocalStorageService>();
        services.AddMudServices();
        services.AddSingleton<AppThemeService>();
        services.RegisterHttpClients();
        return services.BuildServiceProvider();
    }

    private static void RegisterHttpClients(this IServiceCollection services)
    {
        services.AddTransient<CustomerHttpClient>();
        services.AddTransient<ProductHttpClient>();
        services.AddTransient<PurchaseInvoiceHttpClient>();
        services.AddTransient<PurchaseOrderHttpClient>();
        services.AddTransient<SupplierHttpClient>();
    }
}