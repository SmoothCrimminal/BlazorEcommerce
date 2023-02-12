using BlazorEcommerce.Shared;
using System.Net.Http.Json;

namespace BlazorEcommerce.Client.Services.Product
{
    public class ProductService : BaseService, IProductService
    {
        public ProductService(HttpClient httpClient) : base(httpClient)
        {
        }

        public List<BlazorEcommerce.Shared.Product> Products { get; set; } = new List<BlazorEcommerce.Shared.Product>();

        public async Task<ServiceResponse<BlazorEcommerce.Shared.Product>> GetProduct(int id)
        {
            var res = await _httpClient.GetFromJsonAsync<ServiceResponse<BlazorEcommerce.Shared.Product>>($"api/product/{id}");
            return res;
        }

        public async Task GetProducts()
        {
            var res = await _httpClient.GetFromJsonAsync<ServiceResponse<List<BlazorEcommerce.Shared.Product>>>("api/product");

            if (res is not null && res.IsSuccess)
                Products = res.Data;
        }
    }
}
