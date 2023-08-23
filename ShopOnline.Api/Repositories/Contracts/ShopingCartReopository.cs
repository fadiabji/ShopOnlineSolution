using Microsoft.EntityFrameworkCore;
using ShopOnlie.Api.Data;
using ShopOnline.Api.Entities;
using ShopOnline.Models.Dtos;
using System.Text.Encodings.Web;

namespace ShopOnline.Api.Repositories.Contracts
{
    public class ShopingCartReopository : IShopingCartReopository
    {
        private readonly ShopOnlineDbContext _db;
       
        public ShopingCartReopository(ShopOnlineDbContext db)
        {
            _db = db;
        }

        private async Task<bool> CartItemsExist(int cartId, int productId)
        {
            var test = await _db.CartItems.AnyAsync(c => c.CartId == cartId &&
                                                c.ProductId == productId);
            return test ;
        }
        public async Task<CartItem> AddItem(CartItemToAddDto cartItemToAddDto)
        {
            if(await CartItemsExist(cartItemToAddDto.CartId, cartItemToAddDto.ProductId) == false)
            {
                var item = await (from product in _db.Products
                                  where product.Id == cartItemToAddDto.ProductId
                                  select new CartItem
                                  {
                                      CartId = cartItemToAddDto.CartId,
                                      ProductId = cartItemToAddDto.ProductId,
                                      Qty = cartItemToAddDto.Qty,
                                  }).SingleOrDefaultAsync();
                if(item != null)
                {
                    var result = await _db.CartItems.AddAsync(item);
                    await _db.SaveChangesAsync();
                    return result.Entity;
                }
            }
            return null;
        }

        public async Task<IEnumerable<CartItem>> GetItems(int userId)
        {

             var test = await (from cart in _db.Carts
                          join cartItem in _db.CartItems
                          on cart.Id equals cartItem.CartId
                          where cart.UserId == userId
                          select new CartItem
                          {
                              Id = cartItem.Id,
                              ProductId = cartItem.ProductId,
                              Qty = cartItem.Qty,
                              CartId = cartItem.CartId,
                          }).ToListAsync();
            return test;
        }
        public async Task<CartItem> GetItem(int id)
        {
            return await (from cart in _db.Carts
                          join cartItem in _db.CartItems
                          on cart.Id equals cartItem.CartId
                          where cartItem.Id == id
                          select new CartItem
                          {
                              Id = cartItem.Id,
                              ProductId = cartItem.ProductId,
                              Qty = cartItem.Qty,
                              CartId = cartItem.CartId
                          }).SingleOrDefaultAsync();
        }
        public async Task<CartItem> DeleteItem(int id)
        {
            var item = await _db.CartItems.FindAsync(id);
            if(item != null)
            {
                _db.CartItems.Remove(item);
                await _db.SaveChangesAsync();
            }
            return item;
        }



        public async Task<CartItem> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            var item = await _db.CartItems.FindAsync(id);
            if(item != null)
            {
                item.Qty = cartItemQtyUpdateDto.Qty; // cartItemUpdateDto comes from post method from UI
                //_db.CartItems.Update(item);
                await _db.SaveChangesAsync();
                return item;
            }
            return null;
        }
    }
}
