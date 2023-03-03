using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Server.Services.Product;
using BlazorEcommerce.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProducts()
        {
            var products = await _productService.GetProducts();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProduct(int id)
        {
            var product = await _productService.GetProduct(id);
            return Ok(product);
        }

        [HttpGet("category/{categoryUrl}")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProductsByCategory(string categoryUrl)
        {
            var products = await _productService.GetProductsByCategory(categoryUrl);
            return Ok(products);
        }

        [HttpGet("search/{searchText}/{page}")]
        public async Task<ActionResult<ServiceResponse<ProductSearchResultDto>>> SearchProducts(string searchText, int page)
        {
            var products = await _productService.SearchProducts(searchText, page);
            return Ok(products);
        }

        [HttpGet("search-suggestions/{searchText}")]
        public async Task<ActionResult<ServiceResponse<List<string>>>> GetSearchSuggestions(string searchText)
        {
            var suggestions = await _productService.GetProductSerachSuggestions(searchText);
            return Ok(suggestions);
        }

        [HttpGet("featured")]
        public async Task<ActionResult<ServiceResponse<List<string>>>> GetFeaturedProducts()
        {
            var products = await _productService.GetFeaturedProducts();
            return Ok(products);
        }
    }
}
