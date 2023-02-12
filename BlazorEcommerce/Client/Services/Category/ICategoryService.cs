namespace BlazorEcommerce.Client.Services.Category
{
    public interface ICategoryService
    {
        List<BlazorEcommerce.Shared.Category> Categories { get; set; }

        Task GetCategories();
    }
}
