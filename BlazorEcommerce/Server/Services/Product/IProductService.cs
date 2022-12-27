

namespace BlazorEcommerce.Server.Services.Product
{
    public interface IProductService
    {
        Task<ServiceResponse<List<Shared.Product>>> GetProducts();
    }
}
