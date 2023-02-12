using BlazorEcommerce.Shared;

namespace BlazorEcommerce.Client.Services.Product
{
    public interface IProductService
    {
        event Action ProductsChanged;
        List<BlazorEcommerce.Shared.Product> Products { get; set; }
        Task GetProducts(string? categoryUrl = null);
        Task<ServiceResponse<BlazorEcommerce.Shared.Product>> GetProduct(int id);
    }
}
