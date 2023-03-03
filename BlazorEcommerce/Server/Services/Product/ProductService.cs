using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Shared.Dtos;

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
            var product = await _dbContext.Products.Include(x => x.Variants).ThenInclude(v => v.ProductType)
                                .FirstOrDefaultAsync(x => x.Id == id);

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
                Data = await _dbContext.Products.Include(x => x.Variants).ThenInclude(v => v.ProductType).ToListAsync()
            };
        }

        public async Task<ServiceResponse<List<Shared.Product>>> GetProductsByCategory(string categoryUrl)
        {
            var res = _dbContext.Products.Include(x => x.Category).Include(x => x.Variants).ThenInclude(v => v.ProductType)
                                            .ToList().Where(x => x.Category.Url.Equals(categoryUrl, StringComparison.InvariantCultureIgnoreCase));
                                                                           

            return new ServiceResponse<List<Shared.Product>>()
            {
                Data = res.ToList()
            };
        }

        public async Task<ServiceResponse<List<string>>> GetProductSerachSuggestions(string searchText)
        {
            var searchedProducts = _dbContext.Products.Include(x => x.Variants).ThenInclude(v => v.ProductType).ToList()
                                                     .Where(x =>
                                                      x.Title.Contains(searchText, StringComparison.InvariantCultureIgnoreCase) ||
                                                      x.Description.Contains(searchText, StringComparison.InvariantCultureIgnoreCase));

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

            var searchedProducts = _dbContext.Products.Include(x => x.Variants).ThenInclude(v => v.ProductType).ToList()
                                                      .Where(x =>
                                                       x.Title.Contains(searchText, StringComparison.InvariantCultureIgnoreCase) ||
                                                       x.Description.Contains(searchText, StringComparison.InvariantCultureIgnoreCase));

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
                Data = await _dbContext.Products.Where(x => x.Featured).Include(p => p.Variants).ToListAsync()
            };
        }

    }
}
