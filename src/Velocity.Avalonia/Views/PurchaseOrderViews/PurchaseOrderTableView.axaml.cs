using Avalonia.Interactivity;
using Microsoft.Extensions.DependencyInjection;
using Velocity.Avalonia.ViewModels.PurchaseOrders;

namespace Velocity.Avalonia.Views.PurchaseOrderViews;

public partial class PurchaseOrderTableView : ViewBase<PurchaseOrderTableViewModel>
{
    public PurchaseOrderTableView()
    {
        DataContext = App.ServiceProvider.GetRequiredService<PurchaseOrderTableViewModel>();
        InitializeComponent();
    }

    private async void Control_OnLoaded(object sender, RoutedEventArgs e)
    {
        await ViewModel.ReloadPurchaseOrders();
    }
}