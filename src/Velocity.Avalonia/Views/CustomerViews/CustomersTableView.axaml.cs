using Avalonia.Interactivity;
using Microsoft.Extensions.DependencyInjection;
using Velocity.Avalonia.ViewModels.Customers;

namespace Velocity.Avalonia.Views.CustomerViews;

public partial class CustomersTableView : ViewBase<CustomersTableViewModel>
{
    public CustomersTableView()
    {
        DataContext = App.ServiceProvider.GetRequiredService<CustomersTableViewModel>();
        InitializeComponent();
    }

    private async void Control_OnLoaded(object sender, RoutedEventArgs e)
    {
        await ViewModel.ReloadCustomers();
    }
}