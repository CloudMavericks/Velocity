using System.Net.Http;
using System.Net.Http.Json;
using Velocity.Frontend.Extensions;
using Velocity.Shared.Requests.PurchaseOrders;
using Velocity.Shared.Responses.PurchaseOrders;
using Velocity.Shared.Wrapper;

namespace Velocity.Frontend.Services.HttpClients;

public class PurchaseOrderHttpClient
{
    private readonly HttpClient _httpClient;

    public PurchaseOrderHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<PaginatedResult<PurchaseOrderResponse>> GetAllAsync(int pageNumber = 1, int pageSize = 10,
        string searchString = "")
    {
        return _httpClient.GetFromJsonAsync<PaginatedResult<PurchaseOrderResponse>>($"purchase-orders?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}");
    }
    
    public Task<PurchaseOrderResponse> GetAsync(Guid id)
    {
        return _httpClient.GetFromJsonAsync<PurchaseOrderResponse>($"purchase-orders/{id}");
    }
    
    public async Task<PaginatedResult<PurchaseOrderResponse>> GetAllAsync(GetPurchaseOrdersRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("purchase-orders/get", request, cancellationToken: cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.ToPaginatedResult<PurchaseOrderResponse>(cancellationToken: cancellationToken);
    }
    
    public async Task<string> GetNextOrderNumberAsync(DateTime orderDate)
    {
        var response = await _httpClient.GetFromJsonAsync<Dictionary<string, string>>($"purchase-orders/generate?orderDate={orderDate.Ticks}");
        return response["orderNumber"];
    }
    
    public async Task CreateAsync(CreatePurchaseOrderRequest purchaseOrder)
    {
        var response = await _httpClient.PostAsJsonAsync("purchase-orders", purchaseOrder);
        response.EnsureSuccessStatusCode();
    }
    
    public async Task UpdateAsync(UpdatePurchaseOrderRequest purchaseOrder)
    {
        var response = await _httpClient.PutAsJsonAsync($"purchase-orders/{purchaseOrder.Id}", purchaseOrder);
        response.EnsureSuccessStatusCode();
    }
    
    public async Task DeleteAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"purchase-orders/{id}");
        response.EnsureSuccessStatusCode();
    }
}