using BlazorEcommerce.Shared;
using BlazorEcommerce.Shared.Dtos;
using System.Net.Http.Json;

namespace BlazorEcommerce.Client.Services.Product
{
    public class ProductService : ServiceBase, IProductService
    {
        public List<BlazorEcommerce.Shared.Product> Products { get; set; } = new List<BlazorEcommerce.Shared.Product>();
        public string Message { get; set; } = "Loading products...";
        public int CurrentPage { get; set; } = 1;
        public int PageCount { get; set; } = 0;
        public string LastSearchText { get; set; } = string.Empty;

        public event Action ProductsChanged;

        public ProductService(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<ServiceResponse<BlazorEcommerce.Shared.Product>> GetProduct(int id)
        {
            var res = await _httpClient.GetFromJsonAsync<ServiceResponse<BlazorEcommerce.Shared.Product>>($"api/product/{id}");
            return res;
        }

        public async Task GetProducts(string? categoryUrl)
        {
            var url = categoryUrl is null ? "api/product/featured" : $"api/product/category/{categoryUrl}";
            var res = await _httpClient.GetFromJsonAsync<ServiceResponse<List<BlazorEcommerce.Shared.Product>>>(url);

            if (res is not null && res.IsSuccess)
                Products = res.Data;

            CurrentPage = 1;
            PageCount = 0;

            if (Products?.Count <= 0)
                Message = "No products found";

            ProductsChanged.Invoke();
        }

        public async Task SearchProducts(string searchText, int page)
        {
            LastSearchText = searchText;
            var result = await _httpClient.GetFromJsonAsync<ServiceResponse<ProductSearchResultDto>>($"api/product/search/{searchText}/{page}");

            if (result is not null && result.Data is not null)
            {
                Products = result.Data.Products;
                CurrentPage = result.Data.CurrentPage;
                PageCount = result.Data.Pages;
            }

            if (Products.Count == 0)
                Message = "No products found.";

            ProductsChanged.Invoke();
        }

        public async Task<List<string>> GetProductSearchSuggestions(string searchText)
        {
            var result = await _httpClient.GetFromJsonAsync<ServiceResponse<List<string>>>($"api/product/search-suggestions/{searchText}");

            return result.Data;
        }
    }
}
