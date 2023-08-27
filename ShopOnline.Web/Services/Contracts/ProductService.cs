using ShopOnline.Models.Dtos;
using System.Net.Http.Json;

namespace ShopOnline.Web.Services.Contracts
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ProductDto> GetItem(int id)
        {
            try
            {
                // I have to send the Id to the server in order to make the serach for this product
                var response = await _httpClient.GetAsync($"api/Product/{id}");
                if (response.IsSuccessStatusCode)
                {
                    // if response come with no content
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        // empty object of ProductDto
                        return default(ProductDto);
                    }

                    return await response.Content.ReadFromJsonAsync<ProductDto>();

                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception(message);
                }
            }
            catch (Exception)
            {
                // log exception

                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetItems()
        {
            try
            {
                var products = await _httpClient.GetFromJsonAsync<IEnumerable<ProductDto>>("api/Product");
                //return products;
                var response = await _httpClient.GetAsync($"api/Product");
                if (response.IsSuccessStatusCode)
                {
                    // if response come with no content
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        // empty object of ProductDto
                        return Enumerable.Empty<ProductDto>();
                    }

                    return await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();

                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception(message);
                }

            }
            catch (Exception ex)
            {
                // log exception
                var message = ex.Message;
                throw;
            }
        }

        public async Task<IEnumerable<ProductCategoryDto>> GetProductCategories()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Product/GetProductCategories");

                if (response.IsSuccessStatusCode)
                {
                    if(response.StatusCode == System.Net.HttpStatusCode.NoContent) { return Enumerable.Empty<ProductCategoryDto>();}
                    return await response.Content.ReadFromJsonAsync<IEnumerable<ProductCategoryDto>>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http status code - {response.StatusCode} message - {message}");
                }
            }
            catch (Exception)
            {
                // log execption 
                throw;
            }
        }
    }
}
