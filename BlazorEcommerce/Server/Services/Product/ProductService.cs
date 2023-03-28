using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Shared.Dtos;

namespace BlazorEcommerce.Server.Services.Product
{
    public class ProductService : ServiceBase, IProductService
    {
        public ProductService(DataContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
        }

        public async Task<ServiceResponse<Shared.Product>> GetProduct(int id)
        {
            var response = new ServiceResponse<Shared.Product>();
            Shared.Product product = null;

            if (_httpContextAccessor.HttpContext.User.IsInRole("Admin"))
            {
                product = await _dbContext.Products.Include(x => x.Variants.Where(v => !v.Deleted)).ThenInclude(v => v.ProductType)
                                .FirstOrDefaultAsync(x => x.Id == id && !x.Deleted);

            }
            else
            {
                product = await _dbContext.Products.Include(x => x.Variants.Where(v => v.Visible && !v.Deleted)).ThenInclude(v => v.ProductType)
                                                .FirstOrDefaultAsync(x => x.Id == id && x.Visible && !x.Deleted);
            }

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
                Data = await _dbContext.Products
                .Where(p => p.Visible && !p.Deleted)
                .Include(x => x.Variants.Where(v => v.Visible && !v.Deleted)).ThenInclude(v => v.ProductType).ToListAsync()
            };
        }

        public async Task<ServiceResponse<List<Shared.Product>>> GetProductsByCategory(string categoryUrl)
        {
            var res = _dbContext.Products.Include(x => x.Category).Include(x => x.Variants.Where(v => v.Visible && !v.Deleted)).ThenInclude(v => v.ProductType)
                                            .ToList().Where(x => x.Category.Url.Equals(categoryUrl, StringComparison.InvariantCultureIgnoreCase)
                                            && x.Visible && !x.Deleted);
                                                                           

            return new ServiceResponse<List<Shared.Product>>()
            {
                Data = res.ToList()
            };
        }

        public async Task<ServiceResponse<List<string>>> GetProductSerachSuggestions(string searchText)
        {
            var searchedProducts = _dbContext.Products.Include(x => x.Variants.Where(v => v.Visible && !v.Deleted)).ThenInclude(v => v.ProductType).ToList()
                                                     .Where(x =>
                                                      x.Title.Contains(searchText, StringComparison.InvariantCultureIgnoreCase) || 
                                                      x.Description.Contains(searchText, StringComparison.InvariantCultureIgnoreCase) &&
                                                      x.Visible && !x.Deleted);

            var results = new List<string>();

            foreach (var product in searchedProducts)
            {
                if (product.Title.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
                {
                    results.Add(product.Title);
                }

                if (product.Description is not null)
                {
                    var punctuation = product.Description.Where(char.IsPunctuation)
                        .Distinct().ToArray();
                    var words = product.Description.Split()
                        .Select(s => s.Trim(punctuation));

                    foreach (var word in words)
                    {
                        if (word.Contains(searchText, StringComparison.InvariantCultureIgnoreCase) && !results.Contains(word))
                        {
                            results.Add(word);
                        }
                    }
                }
            }

            return new ServiceResponse<List<string>> { Data = results };
        }

        public async Task<ServiceResponse<ProductSearchResultDto>> SearchProducts(string searchText, int page)
        {

            var searchedProducts = _dbContext.Products.Include(x => x.Variants.Where(v => v.Visible && !v.Deleted)).ThenInclude(v => v.ProductType).ToList()
                                                      .Where(x =>
                                                       x.Title.Contains(searchText, StringComparison.InvariantCultureIgnoreCase) ||
                                                       x.Description.Contains(searchText, StringComparison.InvariantCultureIgnoreCase) 
                                                       && x.Visible && !x.Deleted);

            var pageResults = 2f;
            var pageCount = Math.Ceiling(searchedProducts.Count() / pageResults);
            var products = searchedProducts.Skip((page - 1) * (int)pageResults)
                                           .Take((int)pageResults)
                                           .ToList();


            return new ServiceResponse<ProductSearchResultDto>()
            {
                Data = new ProductSearchResultDto()
                {
                    Products = products,
                    CurrentPage = page,
                    Pages = (int)pageCount
                }
            };
        }

        public async Task<ServiceResponse<List<Shared.Product>>> GetFeaturedProducts()
        {
            return new ServiceResponse<List<Shared.Product>>()
            {
                Data = await _dbContext.Products.Where(x => x.Featured && x.Visible && !x.Deleted)
                .Include(p => p.Variants.Where(v => v.Visible && !v.Deleted)).ToListAsync()
            };
        }

        public async Task<ServiceResponse<List<Shared.Product>>> GetAdminProducts()
        {
            return new ServiceResponse<List<Shared.Product>>()
            {
                Data = await _dbContext.Products
                .Where(p => !p.Deleted)
                .Include(x => x.Variants.Where(v => !v.Deleted)).ThenInclude(v => v.ProductType).ToListAsync()
            };
        }

        public async Task<ServiceResponse<Shared.Product>> CreateProduct(Shared.Product product)
        {
            foreach (var variant in product.Variants)
            {
                variant.ProductType = null;
            }

            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();

            return new ServiceResponse<Shared.Product> { Data = product };
        }

        public async Task<ServiceResponse<Shared.Product>> UpdateProduct(Shared.Product product)
        {
            var dbProduct = await _dbContext.Products.FindAsync(product.Id);
            if (dbProduct is null)
            {
                return new ServiceResponse<Shared.Product> { IsSuccess = false, Message = "Product not found" };
            }

            dbProduct.Title = product.Title;
            dbProduct.Description = product.Description;
            dbProduct.ImageUrl = product.ImageUrl;
            dbProduct.CategoryId = product.CategoryId;
            dbProduct.Visible = product.Visible;
            dbProduct.Featured = product.Featured;

            foreach (var variant in product.Variants)
            {
                var dbVariant = await _dbContext.ProductVariants.FirstOrDefaultAsync(v => variant.ProductId == v.ProductId 
                && variant.ProductTypeId == v.ProductTypeId);

                if (dbVariant is null)
                {
                    variant.ProductType = null;
                    await _dbContext.ProductVariants.AddAsync(variant);
                }
                else
                {
                    dbVariant.ProductTypeId = variant.ProductTypeId;
                    dbVariant.Price = variant.Price;
                    dbVariant.OriginalPrice = variant.OriginalPrice;
                    dbVariant.Visible = variant.Visible;
                    dbVariant.Deleted = variant.Deleted;
                }
            }

            await _dbContext.SaveChangesAsync();
            return new ServiceResponse<Shared.Product> { Data = product };
        }

        public async Task<ServiceResponse<bool>> DeleteProduct(int id)
        {
            var dbProduct = await _dbContext.Products.FindAsync(id);
            if (dbProduct is null)
            {
                return new ServiceResponse<bool> { IsSuccess = false, Data = false, Message = "Product not found" };
            }

            dbProduct.Deleted = true;
            await _dbContext.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }
    }
}
