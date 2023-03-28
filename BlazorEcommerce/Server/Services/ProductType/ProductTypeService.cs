using BlazorEcommerce.Server.Data;

namespace BlazorEcommerce.Server.Services.ProductType
{
    public class ProductTypeService : ServiceBase, IProductTypeService
    {
        public ProductTypeService(DataContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
        }

        public async Task<ServiceResponse<List<Shared.ProductType>>> AddProductType(Shared.ProductType productType)
        {
            productType.Editing = productType.IsNew = false;
            _dbContext.ProductTypes.Add(productType);
            await _dbContext.SaveChangesAsync();

            return await GetProductTypes();
        }

        public async Task<ServiceResponse<List<Shared.ProductType>>> GetProductTypes()
        {
            var productTypes = await _dbContext.ProductTypes.ToListAsync();

            return new ServiceResponse<List<Shared.ProductType>> { Data = productTypes };
        }

        public async Task<ServiceResponse<List<Shared.ProductType>>> UpdateProductType(Shared.ProductType productType)
        {
            var dbProductType = await _dbContext.ProductTypes.FindAsync(productType.Id);
            if (dbProductType is null)
            {
                return new ServiceResponse<List<Shared.ProductType>>
                {
                    IsSuccess = false,
                    Message = "Product Type not found"
                };
            }

            dbProductType.Name = productType.Name;
            await _dbContext.SaveChangesAsync();

            return await GetProductTypes();
        }
    }
}
