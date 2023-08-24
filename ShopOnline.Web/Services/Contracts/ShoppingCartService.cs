using Newtonsoft.Json;
using ShopOnline.Models.Dtos;
using System.Net.Http.Json;
using System.Text;

namespace ShopOnline.Web.Services.Contracts
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly HttpClient _httpClient;
        // this property to store the count of the cart appears on the top menu

        public event Action<int> OnShoppingCartChanged;

        public ShoppingCartService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAddDto)
        {
            try
            {
                var respnse = await _httpClient.PostAsJsonAsync<CartItemToAddDto>("api/ShoppingCart", cartItemToAddDto);
                if (respnse.IsSuccessStatusCode)
                {
                    if (respnse.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(CartItemDto);
                    }
                    return await respnse.Content.ReadFromJsonAsync<CartItemDto>();
                }
                else
                {
                    var message = await respnse.Content.ReadAsStringAsync();
                    throw new Exception($"Http status:{respnse.StatusCode} -- {message}");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<CartItemDto>> GetItems(int userId)
        {
            try
            {
                var respnse = await _httpClient.GetAsync($"api/ShoppingCart/{userId}/GetItems");
                if (respnse.IsSuccessStatusCode)
                {
                    if (respnse.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<CartItemDto>().ToList();
                    }
                    return await respnse.Content.ReadFromJsonAsync<List<CartItemDto>>();
                }
                else
                {
                    var message = await respnse.Content.ReadAsStringAsync();
                    throw new Exception($"Http status code: {respnse.StatusCode} message: {message}");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<CartItemDto> DeleteItem(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/ShoppingCart/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CartItemDto>();
                }
                return default(CartItemDto);

            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<CartItemDto> UpdateQty(CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            try
            {
                // here you have to install newton soft package 
                var jsonRequest = JsonConvert.SerializeObject(cartItemQtyUpdateDto);

                // to send the data in a propriate format to the server as a string object
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");

                var response = await _httpClient.PatchAsync($"api/ShoppingCart/{cartItemQtyUpdateDto.CartItemId}", content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CartItemDto>();
                }
                return null;

            }
            catch (Exception)
            {
                // log exception
                throw;
            }
        }

        public void RaiseEventOnShoppingCartChanged(int totalQty)
        {
            if (OnShoppingCartChanged != null)
            {
                OnShoppingCartChanged.Invoke(totalQty);
            }
        }
    }
}
