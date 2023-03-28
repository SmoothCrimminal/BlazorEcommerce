using BlazorEcommerce.Shared;
using System.Net.Http.Json;

namespace BlazorEcommerce.Client.Services.Category
{
    public class CategoryService : ServiceBase, ICategoryService
    {
        public CategoryService(HttpClient httpClient) : base(httpClient)
        {
        }

        public List<BlazorEcommerce.Shared.Category> Categories { get; set; } = new List<BlazorEcommerce.Shared.Category>();
        public List<BlazorEcommerce.Shared.Category> AdminCategories { get; set; } = new List<BlazorEcommerce.Shared.Category>();

        public event Action OnChange;

        public async Task AddCategory(BlazorEcommerce.Shared.Category category)
        {
            var response = await _httpClient.PostAsJsonAsync("api/category/admin", category);
            AdminCategories = (await response.Content.ReadFromJsonAsync<ServiceResponse<List<BlazorEcommerce.Shared.Category>>>()).Data;

            await GetCategories();
            OnChange.Invoke();
        }

        public BlazorEcommerce.Shared.Category CreateNewCategory()
        {
            var newCategory = new BlazorEcommerce.Shared.Category
            {
                IsNew = true,
                Editing = true
            };

            AdminCategories.Add(newCategory);
            OnChange.Invoke();

            return newCategory;
        }

        public async Task DeleteCategory(int categoryId)
        {
            var response = await _httpClient.DeleteAsync($"api/category/admin/{categoryId}");
            AdminCategories = (await response.Content.ReadFromJsonAsync<ServiceResponse<List<BlazorEcommerce.Shared.Category>>>()).Data;

            await GetCategories();
            OnChange.Invoke();
        }

        public async Task GetAdminCategories()
        {
            var res = await _httpClient.GetFromJsonAsync<ServiceResponse<List<BlazorEcommerce.Shared.Category>>>("api/category/admin");

            if (res != null && res.Data != null)
                AdminCategories = res.Data;
        }

        public async Task GetCategories()
        {
            var res = await _httpClient.GetFromJsonAsync<ServiceResponse<List<BlazorEcommerce.Shared.Category>>>("api/category");

            if (res != null && res.Data != null)
                Categories = res.Data;
        }

        public async Task UpdateCategory(BlazorEcommerce.Shared.Category category)
        {
            var response = await _httpClient.PutAsJsonAsync("api/category/admin", category);
            AdminCategories = (await response.Content.ReadFromJsonAsync<ServiceResponse<List<BlazorEcommerce.Shared.Category>>>()).Data;

            await GetCategories();
            OnChange.Invoke();
        }
    }
}
