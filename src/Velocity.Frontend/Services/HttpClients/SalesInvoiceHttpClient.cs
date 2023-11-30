using System.Net.Http;
using System.Net.Http.Json;
using Velocity.Shared.Requests.SalesInvoices;
using Velocity.Shared.Responses.SalesInvoices;
using Velocity.Shared.Wrapper;

namespace Velocity.Frontend.Services.HttpClients;

public class SalesInvoiceHttpClient
{
    private readonly HttpClient _httpClient;

    public SalesInvoiceHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public Task<PaginatedResult<SalesInvoiceResponse>> GetAllAsync(int pageNumber, int pageSize, string searchString)
    {
        return _httpClient.GetFromJsonAsync<PaginatedResult<SalesInvoiceResponse>>($"sales-invoices?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}");
    }
    
    public Task<SalesInvoiceResponse> GetAsync(Guid id)
    {
        return _httpClient.GetFromJsonAsync<SalesInvoiceResponse>($"sales-invoices/{id}");
    }

    public Task<SaleProductResponse> GetProductDetailsAsync(Guid invoiceItemId)
    {
        return _httpClient.GetFromJsonAsync<SaleProductResponse>($"sales-invoices/product/{invoiceItemId}");
    }
    
    public async Task<string> GetNextSalesInvoiceNumberAsync(DateTime salesDate)
    {
        var response = await _httpClient.GetFromJsonAsync<Dictionary<string, string>>($"sales-invoices/generate?saleDate={salesDate.Ticks}");
        return response["invoiceNumber"];
    }
    
    public async Task CreateAsync(CreateSaleInvoiceRequest salesInvoice)
    {
        var response = await _httpClient.PostAsJsonAsync("sales-invoices", salesInvoice);
        response.EnsureSuccessStatusCode();
    }
    
    public async Task UpdateAsync(UpdateSaleInvoiceRequest salesInvoice)
    {
        var response = await _httpClient.PutAsJsonAsync($"sales-invoices/{salesInvoice.Id}", salesInvoice);
        response.EnsureSuccessStatusCode();
    }
    
    public async Task DeleteAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"sales-invoices/{id}");
        response.EnsureSuccessStatusCode();
    }
}