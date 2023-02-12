using BlazorEcommerce.Shared;
using System.Net.Http.Json;

namespace BlazorEcommerce.Client.Services.Category
{
    public class CategoryService : ServiceBase, ICategoryService
    {
        public CategoryService(HttpClient httpClient) : base(httpClient)
        {
        }

        public List<BlazorEcommerce.Shared.Category> Categories { get; set; }

        public async Task GetCategories()
        {
            var res = await _httpClient.GetFromJsonAsync<ServiceResponse<List<BlazorEcommerce.Shared.Category>>>("api/category");

            if (res != null && res.Data != null)
                Categories = res.Data;
        }
    }
}
