using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Server.Services.Product;
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
    }
}
