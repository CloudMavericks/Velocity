using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Velocity.Avalonia.Helpers;
using Velocity.Avalonia.Interfaces;
using Velocity.Avalonia.Services;
using Velocity.Shared.Requests.Suppliers;
using Velocity.Shared.Responses.Suppliers;

namespace Velocity.Avalonia.ViewModels.Suppliers;

public partial class EditSupplierViewModel : ViewModelBase, IViewModelInitialize
{
    private readonly HttpClient _httpClient;
    private readonly NavigationService _navigationService;

    public EditSupplierViewModel(HttpClient httpClient, LocalStorageService localStorageService,
        NavigationService navigationService)
    {
        _httpClient = httpClient;
        _navigationService = navigationService;
        var token = localStorageService.GetItem<string>("token");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
    
    [ObservableProperty]
    private UpdateSupplierRequest _supplier = new();
    
    public async ValueTask Initialize(object parameter)
    {
        if (parameter is Guid id)
        {
            try
            {
                var supplierResponse = await _httpClient.GetFromJsonAsync<SupplierResponse>($"suppliers/{id}");
                Supplier = new UpdateSupplierRequest
                {
                    Id = supplierResponse.Id,
                    Name = supplierResponse.Name,
                    ContactName = supplierResponse.ContactName,
                    ContactEmail = supplierResponse.ContactEmail,
                    ContactPhone = supplierResponse.ContactPhone,
                    Address = supplierResponse.Address,
                    City = supplierResponse.City,
                    State = supplierResponse.State,
                    ZipCode = supplierResponse.ZipCode,
                    Gstn = supplierResponse.Gstn
                };
            }
            catch (Exception e)
            {
                if (e is HttpRequestException { StatusCode: HttpStatusCode.NotFound })
                {
                    await MessageBox.ShowDialogAsync("Error", "Supplier not found");
                }
                else
                {
                    await MessageBox.ShowDialogAsync("Error", e.Message);
                }
            }
        }
    }
    
    [RelayCommand]
    private async Task SaveSupplierAsync()
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync("suppliers", Supplier);
            response.EnsureSuccessStatusCode();
            await MessageBox.ShowDialogAsync("Success", "Supplier updated successfully");
            _navigationService.NavigateTo<SuppliersTableViewModel>();
        }
        catch (Exception e)
        {
            await MessageBox.ShowDialogAsync("Error", e.Message);
        }
    }
    
    [RelayCommand]
    private void Cancel()
    {
        _navigationService.NavigateTo<SuppliersTableViewModel>();
    }
}