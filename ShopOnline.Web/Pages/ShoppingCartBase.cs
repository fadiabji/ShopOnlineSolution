using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ShoppingCartBase:ComponentBase
    {
        [Inject]
        public IShoppingCartService _ShoppingCartService { get; set; }

        public List<CartItemDto> ShoppingCartItems { get; set; }

        public string ErrorMessage { get; set; }

        // implement  code  for assigning the relevent items that return from the server to a shoppingCartItems property when the razor component first randerd
        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await _ShoppingCartService.GetItems(HardCodded.UserId);
            }
            catch (Exception ex)
            {

                ErrorMessage = ex.Message;
            }
        }

        protected async Task DeleteCartItem_Click(int id)
        {
            var cartItemDto = await _ShoppingCartService.DeleteItem(id);

            // need to reflex to UI
            // an solution for that we could remove it from the clinet side avoiding a trip to Server side 
            // we make an seperate method to delete the item here

            RemoveCartItem(id);
        }

        private CartItemDto GetCartItem(int id)
        {
            return ShoppingCartItems.FirstOrDefault(i => i.Id == id);
        }
        private void RemoveCartItem(int id)
        {
            var cartItemDto = GetCartItem(id);
            ShoppingCartItems.Remove(cartItemDto);
        }
    }
}
