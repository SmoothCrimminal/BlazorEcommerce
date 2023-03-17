using BlazorEcommerce.Shared;
using System.Net.Http.Json;

namespace BlazorEcommerce.Client.Services.Address
{
    public class AddressService : ServiceBase, IAddressService
    {
        public AddressService(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<BlazorEcommerce.Shared.Address> AddOrUpdateAddress(BlazorEcommerce.Shared.Address address)
        {
            var response = await _httpClient.PostAsJsonAsync("api/address", address);
            return response.Content.ReadFromJsonAsync<ServiceResponse<BlazorEcommerce.Shared.Address>>().Result.Data;
        }

        public async Task<BlazorEcommerce.Shared.Address> GetAddress()
        {
            var response = await _httpClient.GetFromJsonAsync<ServiceResponse<BlazorEcommerce.Shared.Address>>("api/address");
            return response.Data;
        }
    }
}
