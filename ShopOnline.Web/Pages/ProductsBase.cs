using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ProductsBase : ComponentBase
    {
        [Inject]
        public IProductService _ProductService { get; set; }

        [Inject]
        public IShoppingCartService _ShoppingCartService { get; set; }

        [Inject]
        public IManageProductsLocalStorageService _ManageProductsLocalStorageService { get; set; }

        [Inject]
        public IManageCartItemsLocalStorageService _ManageCartItemsLocalStorageService { get; set; }

        public IEnumerable<ProductDto> Products { get; set; }

        protected override async Task OnInitializedAsync()
        {

            try
            {
                await ClearLocalStorage();

                Products = await _ManageProductsLocalStorageService.GetCollection(); 

                var shoppingCartItems = await _ManageCartItemsLocalStorageService.GetCollection();

                var totalQty = shoppingCartItems.Sum(i => i.Qty);

                _ShoppingCartService.RaiseEventOnShoppingCartChanged(totalQty);

            }
            catch (Exception)
            {

                throw;
            }
        }

        protected IOrderedEnumerable<IGrouping<int, ProductDto>> GetGroupedProductsByCategory()
        {
            return from product in Products
                   group product by product.CategroyId into productByCatGroup
                   orderby productByCatGroup.Key
                   select productByCatGroup;
        }

        protected string GetCatCategroyName(IGrouping<int, ProductDto> groupedProductDtos)
        {
            return groupedProductDtos.FirstOrDefault(pg => pg.CategroyId == groupedProductDtos.Key).CategoryName;
        }

         private async Task ClearLocalStorage()
        {
            await _ManageCartItemsLocalStorageService.RemoveCollection();
            await _ManageProductsLocalStorageService.RemoveCollection();
        }

    }
}
