using Blazored.LocalStorage;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Web.Services.Contracts
{
    public class ManageProductsLocalStorageService : IManageProductsLocalStorageService
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly IProductService _productService;
        private const string key = "ProductCollection";

        public ManageProductsLocalStorageService(ILocalStorageService localStorgeServeice
                                                ,IProductService productService)
        {
            _localStorageService = localStorgeServeice;
            _productService = productService;
        }
        public async Task<IEnumerable<ProductDto>> GetCollection()
        {
            // get the list of products from the server if they are not exist so pring them from the server
            return await _localStorageService.GetItemAsync<IEnumerable<ProductDto>>(key)
                ?? await AddCollection();
        }

        public async Task RemoveCollection()
        {
            await _localStorageService.RemoveItemAsync(key);
        }


        private async Task<IEnumerable<ProductDto>> AddCollection()
        {
            var productCollection = await _productService.GetItems(); 
            if(productCollection != null)
            {
                await _localStorageService.SetItemAsync(key, productCollection);
            }
            return productCollection;
        }
    }
}
