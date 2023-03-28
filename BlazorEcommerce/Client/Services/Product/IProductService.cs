using BlazorEcommerce.Shared;

namespace BlazorEcommerce.Client.Services.Product
{
    public interface IProductService
    {
        event Action ProductsChanged;
        List<BlazorEcommerce.Shared.Product> Products { get; set; }
        List<BlazorEcommerce.Shared.Product> AdminProducts { get; set; }
        public string Message { get; set; }
        int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public string LastSearchText { get; set; }
        Task GetProducts(string? categoryUrl = null);
        Task<ServiceResponse<BlazorEcommerce.Shared.Product>> GetProduct(int id);
        Task SearchProducts(string searchText, int page);
        Task<List<string>> GetProductSearchSuggestions(string searchText);
        Task GetAdminProducts();
        Task<BlazorEcommerce.Shared.Product> CreateProduct(BlazorEcommerce.Shared.Product product);
        Task<BlazorEcommerce.Shared.Product> UpdateProduct(BlazorEcommerce.Shared.Product product);
        Task DeleteProduct(BlazorEcommerce.Shared.Product product);
    }
}
