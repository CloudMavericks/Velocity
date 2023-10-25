using Velocity.Avalonia.ViewModels.Customers;

namespace Velocity.Avalonia.Views.CustomerViews;

public partial class CustomersView : ViewBase<CustomerViewModel>
{
    public CustomersView()
    {
        DataContext = App.ServiceProvider.GetService(typeof(CustomerViewModel));
        InitializeComponent();
    }
}