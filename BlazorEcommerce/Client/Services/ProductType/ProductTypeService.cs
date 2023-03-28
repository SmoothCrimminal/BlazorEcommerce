using BlazorEcommerce.Shared;
using System.Net.Http.Json;

namespace BlazorEcommerce.Client.Services.ProductType
{
    public class ProductTypeService : ServiceBase, IProductTypeService
    {
        public ProductTypeService(HttpClient httpClient) : base(httpClient)
        {
        }

        public List<BlazorEcommerce.Shared.ProductType> ProductTypes { get; set; } = new List<BlazorEcommerce.Shared.ProductType>();

        public event Action OnChange;

        public async Task AddProductType(BlazorEcommerce.Shared.ProductType productType)
        {
            var res = await _httpClient.PostAsJsonAsync("api/producttype", productType);
            ProductTypes = (await res.Content.ReadFromJsonAsync<ServiceResponse<List<BlazorEcommerce.Shared.ProductType>>>()).Data;
            OnChange.Invoke();
        }

        public BlazorEcommerce.Shared.ProductType CreateNewProductType()
        {
            var newProductType = new BlazorEcommerce.Shared.ProductType { IsNew = true, Editing = true };

            ProductTypes.Add(newProductType);
            OnChange.Invoke();

            return newProductType;
        }

        public async Task GetProductTypes()
        {
            var res = await _httpClient.GetFromJsonAsync<ServiceResponse<List<BlazorEcommerce.Shared.ProductType>>>("api/producttype");

            ProductTypes = res.Data;
        }

        public async Task UpdateProductType(BlazorEcommerce.Shared.ProductType productType)
        {
            var res = await _httpClient.PutAsJsonAsync("api/producttype", productType);
            ProductTypes = (await res.Content.ReadFromJsonAsync<ServiceResponse<List<BlazorEcommerce.Shared.ProductType>>>()).Data;
            OnChange.Invoke();
        }
    }
}
