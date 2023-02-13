

namespace BlazorEcommerce.Server.Services.Product
{
    public interface IProductService
    {
        Task<ServiceResponse<List<Shared.Product>>> GetProducts();
        Task<ServiceResponse<Shared.Product>> GetProduct(int id);
        Task<ServiceResponse<List<Shared.Product>>> GetProductsByCategory(string categoryUrl);
        Task<ServiceResponse<List<Shared.Product>>> SearchProducts(string searchText);
        Task<ServiceResponse<List<string>>> GetProductSerachSuggestions(string searchText);
    }
}
