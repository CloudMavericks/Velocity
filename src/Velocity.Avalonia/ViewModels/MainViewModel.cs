using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Velocity.Avalonia.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private IEnumerable<string> TabNames { get; } = new [] {"Products", "Suppliers", "Customers", "Purchase Orders", "Purchase Invoices", "Sales Invoices", "Reports"};
    
    public TabItem[] Tabs => TabNames.Select(x => new TabItem {Header = x}).ToArray();
    
    [ObservableProperty]
    private TabItem _selectedTab;
    
    [RelayCommand]
    private static void Exit()
    {
        App.Shutdown();
    }
}