namespace BlazorEcommerce.Client.Services.Category
{
    public interface ICategoryService
    {
        event Action OnChange;
        List<BlazorEcommerce.Shared.Category> Categories { get; set; }
        List<BlazorEcommerce.Shared.Category> AdminCategories { get; set; }
        Task GetCategories();
        Task GetAdminCategories();
        Task AddCategory(BlazorEcommerce.Shared.Category category);
        Task UpdateCategory(BlazorEcommerce.Shared.Category category);
        Task DeleteCategory(int categoryId);
        BlazorEcommerce.Shared.Category CreateNewCategory();
    }
}
