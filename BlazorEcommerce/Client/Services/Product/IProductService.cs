using BlazorEcommerce.Shared;

namespace BlazorEcommerce.Client.Services.Product
{
    public interface IProductService
    {
        event Action ProductsChanged;
        List<BlazorEcommerce.Shared.Product> Products { get; set; }
        public string Message { get; set; }
        Task GetProducts(string? categoryUrl = null);
        Task<ServiceResponse<BlazorEcommerce.Shared.Product>> GetProduct(int id);
        Task SearchProducts(string searchText);
        Task<List<string>> GetProductSearchSuggestions(string searchText);
    }
}
