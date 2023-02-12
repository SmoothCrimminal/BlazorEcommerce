namespace BlazorEcommerce.Server.Services.Category
{
    public interface ICategoryService
    {
        public Task<ServiceResponse<List<Shared.Category>>> GetCategories();
    }
}
