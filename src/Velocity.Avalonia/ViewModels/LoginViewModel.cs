using System.Net.Http.Headers;
using System.Net.Http.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia.Enums;
using Velocity.Avalonia.Extensions;
using Velocity.Avalonia.Helpers;
using Velocity.Avalonia.Services;
using Velocity.Avalonia.Views;
using Velocity.Shared.Requests;
using Velocity.Shared.Responses;

namespace Velocity.Avalonia.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    private readonly HttpClient _httpClient;
    private readonly LocalStorageService _localStorageService;

    public LoginViewModel(HttpClient httpClient, LocalStorageService localStorageService)
    {
        _httpClient = httpClient;
        _localStorageService = localStorageService;
    }
    
    [ObservableProperty] 
    private string _userName;
    
    [ObservableProperty]
    private string _password;

    [RelayCommand]
    private async Task LoginAsync()
    {
        var request = new LoginRequest
        {
            UserName = UserName,
            Password = Password
        };
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/login", request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.ToResult<LoginResponse>();
                _localStorageService.SetItem("token", result.Data.Token);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Data.Token);
                App.GetApplicationLifetime().ChangeMainWindowAs<MainWindow>();
            }
            else
            {
                await MessageBox.ShowDialogAsync("Error", "Invalid username or password", ButtonEnum.Ok);
            }
        }
        catch (Exception e)
        {
            await MessageBox.ShowDialogAsync("Error", e.Message, ButtonEnum.Ok);
        }
    }

    [RelayCommand]
    private static void Exit()
    {
        App.Shutdown();
    }
}