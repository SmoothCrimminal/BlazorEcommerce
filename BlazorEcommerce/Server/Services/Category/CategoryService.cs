using BlazorEcommerce.Server.Data;

namespace BlazorEcommerce.Server.Services.Category
{
    public class CategoryService : ServiceBase, ICategoryService
    {
        public CategoryService(DataContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
        }

        public async Task<ServiceResponse<List<Shared.Category>>> AddCategory(Shared.Category category)
        {
            category.Editing = category.IsNew = false;
            _dbContext.Categories.Add(category);

            await _dbContext.SaveChangesAsync();
            return await GetAdminCategories();
        }

        public async Task<ServiceResponse<List<Shared.Category>>> DeleteCategory(int id)
        {
            var category = await GetCategoryById(id);

            if (category is null)
                return new ServiceResponse<List<Shared.Category>> { IsSuccess = false, Message = "Category not found" };

            category.Deleted = true;
            await _dbContext.SaveChangesAsync();

            return await GetAdminCategories();
        }

        public async Task<ServiceResponse<List<Shared.Category>>> GetAdminCategories()
        {
            return new ServiceResponse<List<Shared.Category>>()
            {
                Data = await _dbContext.Categories.Where(x => !x.Deleted).ToListAsync(),
            };
        }

        public async Task<ServiceResponse<List<Shared.Category>>> GetCategories()
        {
            return new ServiceResponse<List<Shared.Category>>()
            {
                Data = await _dbContext.Categories.Where(x => !x.Deleted && x.Visible).ToListAsync(),
            };
        }

        public async Task<ServiceResponse<List<Shared.Category>>> UpdateCategory(Shared.Category category)
        {
            var dbCategory = await GetCategoryById(category.Id); 

            if (dbCategory is null)
                return new ServiceResponse<List<Shared.Category>> { IsSuccess = false, Message = "Category not found" };

            dbCategory.Name = category.Name;
            dbCategory.Url = category.Url;
            dbCategory.Visible = category.Visible;

            await _dbContext.SaveChangesAsync();

            return await GetAdminCategories();
        }

        private async Task<Shared.Category> GetCategoryById(int id) => await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
    }
}
