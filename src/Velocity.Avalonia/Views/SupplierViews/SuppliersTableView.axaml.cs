using Avalonia.Interactivity;
using Microsoft.Extensions.DependencyInjection;
using Velocity.Avalonia.ViewModels.Suppliers;

namespace Velocity.Avalonia.Views.SupplierViews;

public partial class SuppliersTableView : ViewBase<SuppliersTableViewModel>
{
    public SuppliersTableView()
    {
        DataContext = App.ServiceProvider.GetRequiredService<SuppliersTableViewModel>();
        InitializeComponent();
    }

    private async void Control_OnLoaded(object sender, RoutedEventArgs e)
    {
        await ViewModel.ReloadSuppliers();
    }
}