using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;
using System.Diagnostics.CodeAnalysis;

namespace ShopOnline.Web.Pages
{
    public class ProductDetailsBase : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }
        [Inject]
        public IProductService _ProductService { get; set; }

        [Inject]
        public IShoppingCartService _ShoppingCartService { get; set; }

        [Inject]
        public IManageProductsLocalStorageService _ManageProductsLocalStorageService { get; set; }

        [Inject]
        public IManageCartItemsLocalStorageService _ManageCartItemsLocalStorageService { get; set; }


        //This service from runtime
        [Inject]
        public NavigationManager _NavigationManager { get; set; }

        public ProductDto Product { get; set; }

        public string ErrorMessage { get; set; }

        private List<CartItemDto> ShoppingCartItems { get; set; } 


        // write  just override first  then the list will come to choose OnInitializedAsync()
        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await _ManageCartItemsLocalStorageService.GetCollection();
                Product = await GetProductById(Id);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        protected async Task AddToCart_Click(CartItemToAddDto cartItemToAddDto)
        {
            try
            {
                var cartItemDto = await _ShoppingCartService.AddItem(cartItemToAddDto);

                if (cartItemDto != null)
                {
                    ShoppingCartItems.Add(cartItemDto);
                    await _ManageCartItemsLocalStorageService.SaveCollection(ShoppingCartItems);
                }
                _NavigationManager.NavigateTo("/ShoppingCart");
            }
            catch (Exception)
            {

                // log excetpion
            }
        }



        private async Task<ProductDto> GetProductById(int id)
        {
            var productDtos = await _ManageProductsLocalStorageService.GetCollection();

            if(productDtos != null)
            {
                return productDtos.SingleOrDefault(p => p.Id == id);
            }
            return null;
        }

       
    }
}
