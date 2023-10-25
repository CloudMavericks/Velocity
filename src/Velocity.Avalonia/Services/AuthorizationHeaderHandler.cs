using System.Net.Http.Headers;

namespace Velocity.Avalonia.Services;

public class AuthorizationHeaderHandler : DelegatingHandler
{
    private readonly LocalStorageService _localStorageService;

    public AuthorizationHeaderHandler(LocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }
    
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Headers.Authorization?.Scheme == "Bearer")
        {
            var token = _localStorageService.GetItem<string>("token");
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
        return base.SendAsync(request, cancellationToken);
    }
}