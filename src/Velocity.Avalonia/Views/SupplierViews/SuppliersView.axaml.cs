using Microsoft.Extensions.DependencyInjection;
using Velocity.Avalonia.ViewModels.Suppliers;

namespace Velocity.Avalonia.Views.SupplierViews;

public partial class SuppliersView : ViewBase<SupplierViewModel>
{
    public SuppliersView()
    {
        DataContext = App.ServiceProvider.GetRequiredService<SupplierViewModel>();
        InitializeComponent();
    }
}