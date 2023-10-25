using Microsoft.Extensions.DependencyInjection;
using Velocity.Avalonia.Services;
using Velocity.Avalonia.ViewModels;
using Velocity.Avalonia.ViewModels.Customers;
using Velocity.Avalonia.ViewModels.Products;
using Velocity.Avalonia.ViewModels.Suppliers;
using Velocity.Avalonia.Views;
using Velocity.Avalonia.Views.CustomerViews;
using Velocity.Avalonia.Views.ProductViews;
using Velocity.Avalonia.Views.SupplierViews;
using ProductsView = Velocity.Avalonia.Views.ProductViews.ProductsView;

namespace Velocity.Avalonia.Extensions;

internal static class DependencyInjection
{
    public static IServiceCollection Initialize()
    {
        return new ServiceCollection().ConfigureServices();
    }
    
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        // Main Window
        services.AddSingleton<LoginWindow>();
        services.AddSingleton<MainWindow>();
        
        // Views
        services.AddTransient<CustomersView>();
        services.AddTransient<CustomersTableView>();
        services.AddTransient<AddCustomerView>();
        services.AddTransient<EditCustomerView>();
        services.AddTransient<SuppliersView>();
        services.AddTransient<SuppliersTableView>();
        services.AddTransient<AddSupplierView>();
        services.AddTransient<EditSupplierView>();
        services.AddTransient<ProductsView>();
        services.AddTransient<ProductsTableView>();
        services.AddTransient<AddProductView>();
        services.AddTransient<EditProductView>();
        
        // ViewModels
        services.AddSingleton<LoginViewModel>();
        services.AddSingleton<MainViewModel>();
        services.AddTransient<CustomerViewModel>();
        services.AddTransient<CustomersTableViewModel>();
        services.AddTransient<AddCustomerViewModel>();
        services.AddTransient<EditCustomerViewModel>();
        services.AddTransient<SupplierViewModel>();
        services.AddTransient<SuppliersTableViewModel>();
        services.AddTransient<AddSupplierViewModel>();
        services.AddTransient<EditSupplierViewModel>();
        services.AddTransient<ProductsViewModel>();
        services.AddTransient<ProductsTableViewModel>();
        services.AddTransient<AddProductViewModel>();
        services.AddTransient<EditProductViewModel>();
        
        // Services
        services.AddScoped<NavigationService>();
        services.AddSingleton<LocalStorageService>();
        services.AddTransient<AuthorizationHeaderHandler>();
        services.AddTransient(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Backend API"));
        services.AddHttpClient("Backend API", client =>
        {
            client.BaseAddress = new Uri("https://localhost:7205");
        }).AddHttpMessageHandler<AuthorizationHeaderHandler>();
        return services;
    }

    public static IServiceProvider Build(this IServiceCollection services)
    {
        return services.BuildServiceProvider();
    }
}