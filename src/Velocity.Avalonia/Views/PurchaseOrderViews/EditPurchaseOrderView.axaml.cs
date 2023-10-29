using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Velocity.Avalonia.ViewModels.PurchaseOrders;

namespace Velocity.Avalonia.Views.PurchaseOrderViews;

public partial class EditPurchaseOrderView : ViewBase<EditPurchaseOrderViewModel>
{
    public EditPurchaseOrderView()
    {
        DataContext = App.ServiceProvider.GetRequiredService<EditPurchaseOrderViewModel>();
        InitializeComponent();
    }
}