using BlazorEcommerce.Shared;

namespace BlazorEcommerce.Server.Services.Category
{
    public interface ICategoryService
    {
        public Task<ServiceResponse<List<Shared.Category>>> GetCategories();
        public Task<ServiceResponse<List<Shared.Category>>> GetAdminCategories();
        public Task<ServiceResponse<List<Shared.Category>>> AddCategory(Shared.Category category);
        public Task<ServiceResponse<List<Shared.Category>>> UpdateCategory(Shared.Category category);
        public Task<ServiceResponse<List<Shared.Category>>> DeleteCategory(int id);
    }
}
