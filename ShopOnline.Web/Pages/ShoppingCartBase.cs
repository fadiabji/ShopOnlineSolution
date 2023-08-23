﻿using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;
using System.Runtime.InteropServices;

namespace ShopOnline.Web.Pages
{
    public class ShoppingCartBase:ComponentBase
    {
        [Inject]
        public IShoppingCartService _ShoppingCartService { get; set; }

        public List<CartItemDto> ShoppingCartItems { get; set; }

        public string ErrorMessage { get; set; }
        protected string TotalPrice { get; set; }
        protected int TotalQuantity { get; set; }

        // implement  code  for assigning the relevent items that return from the server to a shoppingCartItems property when the razor component first randerd
        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await _ShoppingCartService.GetItems(HardCodded.UserId);
                CalculateCartSummaryTotals();
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
            CalculateCartSummaryTotals();
        }

        protected async Task UpdateQtyCartItem_Click(int id, int qty)
        {
            try
            {
                if (qty > 0)
                {
                    var updateItemDto = new CartItemQtyUpdateDto
                    {
                        Qty = qty,
                        CartItemId = id,
                    };
                    var returnedUpdateItemDto = await _ShoppingCartService.UpdateQty(updateItemDto);

                    UpdateItemTotalPrice(returnedUpdateItemDto);

                    CalculateCartSummaryTotals();
                }
                else
                {
                    var item = ShoppingCartItems.FirstOrDefault(i  => i.Id == id);
                    if (item != null)
                    {
                        item.Qty = 1;
                        item.TotalPrice = item.Price;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void SetTotalPrice()
        {
            TotalPrice = ShoppingCartItems.Sum(i => i.TotalPrice).ToString("C"); // C means Currency format
        }

        private void SetTotalQuantity()
        {
            TotalQuantity = ShoppingCartItems.Sum(i => i.Qty);
        }

        private void CalculateCartSummaryTotals()
        {
            SetTotalPrice();
            SetTotalQuantity();
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

        private void UpdateItemTotalPrice(CartItemDto cartItemDto)
        {
            var item = GetCartItem(cartItemDto.Id);
            if(item != null)
            {
                item.TotalPrice = cartItemDto.Price * cartItemDto.Qty;
            }
        }
    }
}
