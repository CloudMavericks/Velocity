using System;
using System.Net.Http;

namespace Velocity.Frontend.Http;

public class HttpApi : IDisposable
{
    private HttpClient _httpClient;
    private static HttpApi _instance;
    private bool _disposed;

    private HttpClient Client
    {
        get => _httpClient ??= new HttpClient() { BaseAddress = new Uri("https://localhost:7205") };
        set => _httpClient = value;
    }


    private static HttpApi Instance
    {
        get => _instance ??= new HttpApi();
        set => _instance = value;
    }

    public static HttpClient GetClient()
    {
        return Instance.Client;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }
        if (disposing)
        {
            Client.Dispose();
        }
        _disposed = true;
    }
}