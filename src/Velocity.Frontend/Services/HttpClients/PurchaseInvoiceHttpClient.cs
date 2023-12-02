using System.Net.Http;
using System.Net.Http.Json;
using Velocity.Shared.Requests.PurchaseInvoices;
using Velocity.Shared.Responses.PurchaseInvoices;
using Velocity.Shared.Wrapper;

namespace Velocity.Frontend.Services.HttpClients;

public class PurchaseInvoiceHttpClient
{
    private readonly HttpClient _httpClient;

    public PurchaseInvoiceHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public Task<PaginatedResult<PurchaseInvoiceResponse>> GetAllAsync(int pageNumber, int pageSize, string searchString)
    {
        return _httpClient.GetFromJsonAsync<PaginatedResult<PurchaseInvoiceResponse>>($"purchase-invoices?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}");
    }
    
    public Task<PurchaseInvoiceResponse> GetAsync(Guid id)
    {
        return _httpClient.GetFromJsonAsync<PurchaseInvoiceResponse>($"purchase-invoices/{id}");
    }
    
    public async Task<string> GetNextPurchaseNumberAsync(DateTime purchaseDate)
    {
        var response = await _httpClient.GetFromJsonAsync<Dictionary<string, string>>($"purchase-invoices/generate?purchaseDate={purchaseDate.Ticks}");
        return response["purchaseNumber"];
    }
    
    public async Task CreateAsync(CreatePurchaseInvoiceRequest purchaseInvoice)
    {
        var response = await _httpClient.PostAsJsonAsync("purchase-invoices", purchaseInvoice);
        response.EnsureSuccessStatusCode();
    }

    public async Task CompleteAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"purchase-invoices/{id}/complete");
        response.EnsureSuccessStatusCode();
    }
    
    public async Task CancelAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"purchase-invoices/{id}/cancel");
        response.EnsureSuccessStatusCode();
    }
    
    public async Task UpdateAsync(UpdatePurchaseInvoiceRequest purchaseInvoice)
    {
        var response = await _httpClient.PutAsJsonAsync($"purchase-invoices/{purchaseInvoice.Id}", purchaseInvoice);
        response.EnsureSuccessStatusCode();
    }
    
    public async Task DeleteAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"purchase-invoices/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<byte[]> PrintAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"purchase-invoices/{id}/print");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsByteArrayAsync();
    }
}