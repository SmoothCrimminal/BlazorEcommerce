using BlazorEcommerce.Shared;

namespace BlazorEcommerce.Client.Services.Product
{
    public interface IProductService
    {
        List<BlazorEcommerce.Shared.Product> Products { get; set; }
        Task GetProducts();
        Task<ServiceResponse<BlazorEcommerce.Shared.Product>> GetProduct(int id);
    }
}
