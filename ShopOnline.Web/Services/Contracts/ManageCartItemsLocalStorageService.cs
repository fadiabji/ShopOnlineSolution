using Blazored.LocalStorage;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Web.Services.Contracts
{
    public class ManageCartItemsLocalStorageService : IManageCartItemsLocalStorageService
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly IShoppingCartService _shoppingCartService;

        private const string key = "CartItemcollection";

        public ManageCartItemsLocalStorageService( ILocalStorageService localStorageService,
                                                    IShoppingCartService shoppingCartService)
        {
            _localStorageService = localStorageService;
            _shoppingCartService = shoppingCartService;
        }
        public async Task<List<CartItemDto>> GetCollection()
        {
            return await _localStorageService.GetItemAsync < List<CartItemDto>>(key) 
                ?? await AddCollection();

        }

        public async Task RemoveCollection()
        {
            await _localStorageService.RemoveItemAsync(key);
        }

        public async Task SaveCollection(List<CartItemDto> cartItemDtos)
        {
            await _localStorageService.SetItemAsync(key, cartItemDtos);
        }


        private async Task<List<CartItemDto>> AddCollection()
        {
            var shopingCartCollection = await _shoppingCartService.GetItems(HardCodded.UserId);

            if (shopingCartCollection != null)
            {
                 await _localStorageService.SetItemAsync(key, shopingCartCollection);
            }
            return shopingCartCollection;

        }
    }
}
