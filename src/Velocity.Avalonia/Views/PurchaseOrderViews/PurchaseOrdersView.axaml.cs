using Microsoft.Extensions.DependencyInjection;
using Velocity.Avalonia.ViewModels.PurchaseOrders;

namespace Velocity.Avalonia.Views.PurchaseOrderViews;

public partial class PurchaseOrdersView : ViewBase<PurchaseOrdersViewModel>
{
    public PurchaseOrdersView()
    {
        DataContext = App.ServiceProvider.GetRequiredService<PurchaseOrdersViewModel>();
        InitializeComponent();
    }
}