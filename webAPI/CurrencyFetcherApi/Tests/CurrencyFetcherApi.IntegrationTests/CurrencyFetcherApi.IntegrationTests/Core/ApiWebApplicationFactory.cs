using Microsoft.AspNetCore.Mvc.Testing;

namespace CurrencyFetcherApi.IntegrationTests.Core
{
    /// <summary>
    /// Create application factory for used with HttpClient
    /// </summary>
    public class ApiWebApplicationFactory : WebApplicationFactory<Startup>
    {
    }
}