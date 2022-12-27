namespace BlazorEcommerce.Client.Services
{
    public abstract class BaseService
    {
        internal readonly HttpClient _httpClient;

        public BaseService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
    }
}
