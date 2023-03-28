

using BlazorEcommerce.Shared.Dtos;

namespace BlazorEcommerce.Server.Services.Product
{
    public interface IProductService
    {
        Task<ServiceResponse<List<Shared.Product>>> GetProducts();
        Task<ServiceResponse<Shared.Product>> GetProduct(int id);
        Task<ServiceResponse<List<Shared.Product>>> GetProductsByCategory(string categoryUrl);
        Task<ServiceResponse<ProductSearchResultDto>> SearchProducts(string searchText, int page);
        Task<ServiceResponse<List<string>>> GetProductSerachSuggestions(string searchText);
        Task<ServiceResponse<List<Shared.Product>>> GetFeaturedProducts();
        Task<ServiceResponse<List<Shared.Product>>> GetAdminProducts();
        Task<ServiceResponse<Shared.Product>> CreateProduct(Shared.Product product);
        Task<ServiceResponse<Shared.Product>> UpdateProduct(Shared.Product product);
        Task<ServiceResponse<bool>> DeleteProduct(int id);
    }
}
