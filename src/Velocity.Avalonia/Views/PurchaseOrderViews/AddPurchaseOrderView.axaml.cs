using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Velocity.Avalonia.ViewModels.PurchaseOrders;

namespace Velocity.Avalonia.Views.PurchaseOrderViews;

public partial class AddPurchaseOrderView : ViewBase<AddPurchaseOrderViewModel>
{
    public AddPurchaseOrderView()
    {
        DataContext = App.ServiceProvider.GetRequiredService<AddPurchaseOrderViewModel>();
        InitializeComponent();
    }
}