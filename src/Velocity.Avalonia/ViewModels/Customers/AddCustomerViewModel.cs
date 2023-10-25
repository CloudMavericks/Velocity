using System.Net.Http.Headers;
using System.Net.Http.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Velocity.Avalonia.Helpers;
using Velocity.Avalonia.Services;
using Velocity.Shared.Requests.Customers;

namespace Velocity.Avalonia.ViewModels.Customers;

public partial class AddCustomerViewModel : ViewModelBase
{
    private readonly HttpClient _httpClient;
    private readonly NavigationService _navigationService;

    public AddCustomerViewModel(HttpClient httpClient, LocalStorageService localStorageService, NavigationService navigationService)
    {
        _httpClient = httpClient;
        _navigationService = navigationService;
        var token = localStorageService.GetItem<string>("token");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
    
    [ObservableProperty]
    private CreateCustomerRequest _customer = new();

    [RelayCommand]
    private async Task SaveCustomerAsync()
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("customers", Customer);
            response.EnsureSuccessStatusCode();
            await MessageBox.ShowDialogAsync("Success", "Customer added successfully");
           _navigationService.NavigateTo<CustomersTableViewModel>();
        }
        catch (Exception e)
        {
            await MessageBox.ShowDialogAsync("Error", e.Message);
        }
    }
    
    [RelayCommand]
    private void GoBack()
    {
        _navigationService.NavigateTo<CustomersTableViewModel>();
    }
}