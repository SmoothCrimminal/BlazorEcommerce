using BlazorEcommerce.Server.Data;

namespace BlazorEcommerce.Server.Services.Product
{
    public class ProductService : ServiceBase, IProductService
    {
        public ProductService(DataContext dbContext) : base(dbContext)
        {
        }

        public async Task<ServiceResponse<Shared.Product>> GetProduct(int id)
        {
            var response = new ServiceResponse<Shared.Product>();
            var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (product is null) 
            {
                response.IsSuccess = false;
                response.Message = "Sorry, product does not exist in the database";
            }

            else
            {
                response.Data = product;
            }
            
            return response;
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
