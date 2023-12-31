﻿using Microsoft.EntityFrameworkCore;
using ShopOnlie.Api.Data;
using ShopOnline.Api.Entities;

namespace ShopOnline.Api.Repositories.Contracts
{
    public class ProductRepostiories : IProductRepostiories
    {
        private readonly ShopOnlineDbContext _db;
        public ProductRepostiories(ShopOnlineDbContext db)
        {
            _db = db;

        }
        public async Task<IEnumerable<ProductCategory>> GetCategories()
        {
            return await _db.ProductCategories.ToListAsync();
        }

        public async Task<ProductCategory> GetCateogy(int id)
        {
            return await _db.ProductCategories.SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Product> GetItem(int id)
        {
            return await _db.Products.Include(p => p.ProductCategory).SingleOrDefaultAsync(p=>p.Id == id);
        }


        public async Task<IEnumerable<Product>> GetItems()
        {
            //return await _db.Products.ToListAsync();
            return await _db.Products.Include(p => p.ProductCategory).ToListAsync();

        }
        public async Task<IEnumerable<Product>> GetItemByCategory(int id)
        {
            var products = await _db.Products
                                    .Include(p => p.ProductCategory)
                                    .Where(p=>p.CategoryId == id).ToListAsync();
            return products;
        }
    }
}
