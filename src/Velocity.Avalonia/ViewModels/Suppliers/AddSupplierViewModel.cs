using System.Net.Http.Headers;
using System.Net.Http.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Velocity.Avalonia.Helpers;
using Velocity.Avalonia.Services;
using Velocity.Shared.Requests.Suppliers;

namespace Velocity.Avalonia.ViewModels.Suppliers;

public partial class AddSupplierViewModel : ViewModelBase
{
    private readonly HttpClient _httpClient;
    private readonly NavigationService _navigationService;

    public AddSupplierViewModel(HttpClient httpClient, LocalStorageService localStorageService, NavigationService navigationService)
    {
        _httpClient = httpClient;
        _navigationService = navigationService;
        var token = localStorageService.GetItem<string>("token");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
    
    [ObservableProperty]
    private CreateSupplierRequest _supplier = new();
    
    [RelayCommand]
    private async Task SaveSupplierAsync()
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("suppliers", Supplier);
            response.EnsureSuccessStatusCode();
            await MessageBox.ShowDialogAsync("Success", "Supplier added successfully");
            _navigationService.NavigateTo<SuppliersTableViewModel>();
        }
        catch (Exception e)
        {
            await MessageBox.ShowDialogAsync("Error", e.Message);
        }
    }
    
    [RelayCommand]
    private void GoBack()
    {
        _navigationService.NavigateTo<SuppliersTableViewModel>();
    }
}