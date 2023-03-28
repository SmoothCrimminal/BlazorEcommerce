namespace BlazorEcommerce.Client.Services.ProductType
{
    public interface IProductTypeService
    {
        event Action OnChange;
        public List<BlazorEcommerce.Shared.ProductType> ProductTypes { get; set; }
        Task GetProductTypes();
        Task AddProductType(BlazorEcommerce.Shared.ProductType productType);
        Task UpdateProductType(BlazorEcommerce.Shared.ProductType productType);
        BlazorEcommerce.Shared.ProductType CreateNewProductType();
    }
}
