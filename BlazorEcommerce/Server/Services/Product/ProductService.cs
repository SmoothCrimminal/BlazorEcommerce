using BlazorEcommerce.Server.Data;

namespace BlazorEcommerce.Server.Services.Product
{
    public class ProductService : ServiceBase, IProductService
    {
        public ProductService(DataContext dbContext) : base(dbContext)
        {
        }

        public async Task<ServiceResponse<List<Shared.Product>>> GetProducts()
        {
            return new ServiceResponse<List<Shared.Product>>()
            {
                Data = await _dbContext.Products.ToListAsync()
            };
        }
    }
}
