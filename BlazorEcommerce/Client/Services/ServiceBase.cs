namespace BlazorEcommerce.Client.Services
{
    public abstract class ServiceBase
    {
        internal readonly HttpClient _httpClient;

        public ServiceBase(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
    }
}
