using BlazorEcommerce.Server.Data;

namespace BlazorEcommerce.Server.Services.Category
{
    public class CategoryService : ServiceBase, ICategoryService
    {
        public CategoryService(DataContext dbContext) : base(dbContext)
        {
        }

        public async Task<ServiceResponse<List<Shared.Category>>> GetCategories()
        {
            return new ServiceResponse<List<Shared.Category>>()
            {
                Data = await _dbContext.Categories.ToListAsync(),
            };
        }
    }
}
