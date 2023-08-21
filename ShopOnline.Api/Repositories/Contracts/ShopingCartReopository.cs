using Microsoft.EntityFrameworkCore;
using ShopOnlie.Api.Data;
using ShopOnline.Api.Entities;
using ShopOnline.Models.Dtos;

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
            return await _db.CartItems.AnyAsync(c => c.CartId == cartId && 
                                                c.ProductId == productId);
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
            return await (from cart in _db.Carts
                          join cartItem in _db.CartItems
                          on cart.Id equals cartItem.Id
                          where cart.UserId == userId
                          select new CartItem
                          {
                              Id = cartItem.Id,
                              ProductId = cartItem.ProductId,
                              Qty = cartItem.Qty,
                              CartId = cartItem.CartId,
                          }).ToListAsync();
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
        public Task<CartItem> DeleteItem(int id)
        {
            throw new NotImplementedException();
        }



        public Task<CartItem> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            throw new NotImplementedException();
        }
    }
}
