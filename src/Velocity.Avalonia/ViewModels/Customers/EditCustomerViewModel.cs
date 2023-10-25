using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Velocity.Avalonia.Helpers;
using Velocity.Avalonia.Interfaces;
using Velocity.Avalonia.Services;
using Velocity.Shared.Requests.Customers;
using Velocity.Shared.Responses.Customers;

namespace Velocity.Avalonia.ViewModels.Customers;

public partial class EditCustomerViewModel : ViewModelBase, IViewModelInitialize
{
    private readonly HttpClient _httpClient;
    private readonly NavigationService _navigationService;

    public EditCustomerViewModel(HttpClient httpClient, LocalStorageService localStorageService,
        NavigationService navigationService)
    {
        _httpClient = httpClient;
        _navigationService = navigationService;
        var token = localStorageService.GetItem<string>("token");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    [ObservableProperty] private UpdateCustomerRequest _customer = new();

    public async ValueTask Initialize(object parameter)
    {
        if (parameter is Guid id)
        {
            try
            {
                var customerResponse = await _httpClient.GetFromJsonAsync<CustomerResponse>($"customers/{id}");
                Customer = new UpdateCustomerRequest
                {
                    Id = customerResponse.Id,
                    Name = customerResponse.Name,
                    ContactName = customerResponse.ContactName,
                    ContactEmail = customerResponse.ContactEmail,
                    ContactPhone = customerResponse.ContactPhone,
                    Address = customerResponse.Address,
                    City = customerResponse.City,
                    State = customerResponse.State,
                    ZipCode = customerResponse.ZipCode,
                    Gstn = customerResponse.Gstn
                };
            }
            catch (Exception e)
            {
                if (e is HttpRequestException { StatusCode: HttpStatusCode.NotFound })
                {
                    await MessageBox.ShowDialogAsync("Error", "Customer not found");
                }
                else
                {
                    await MessageBox.ShowDialogAsync("Error", e.Message);
                }
            }
        }
        else
        {
            await MessageBox.ShowDialogAsync("Error", "Invalid parameter");
        }
    }

    [RelayCommand]
    private async Task SaveCustomerAsync()
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"customers/{Customer.Id}", Customer);
            response.EnsureSuccessStatusCode();
            await MessageBox.ShowDialogAsync("Success", "Customer updated successfully!");
            _navigationService.NavigateTo<CustomersTableViewModel>();
        }
        catch (Exception e)
        {
            await MessageBox.ShowDialogAsync("Error", e.Message);
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        _navigationService.NavigateTo<CustomersTableViewModel>();
    }
}