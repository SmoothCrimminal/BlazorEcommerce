namespace BlazorEcommerce.Client.Services.Product
{
    public interface IProductService
    {
        List<BlazorEcommerce.Shared.Product> Products { get; set; }
        Task GetProducts();
    }
}
