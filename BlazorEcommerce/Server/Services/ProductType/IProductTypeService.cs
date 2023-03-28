using BlazorEcommerce.Shared;

namespace BlazorEcommerce.Server.Services.ProductType
{
    public interface IProductTypeService
    {
        Task<ServiceResponse<List<Shared.ProductType>>> GetProductTypes();
        Task<ServiceResponse<List<Shared.ProductType>>> AddProductType(Shared.ProductType productType);
        Task<ServiceResponse<List<Shared.ProductType>>> UpdateProductType(Shared.ProductType productType);
    }
}
