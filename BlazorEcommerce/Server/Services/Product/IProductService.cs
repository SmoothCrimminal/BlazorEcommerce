

namespace BlazorEcommerce.Server.Services.Product
{
    public interface IProductService
    {
        Task<ServiceResponse<List<Shared.Product>>> GetProducts();
        Task<ServiceResponse<Shared.Product>> GetProduct(int id);
    }
}
