﻿using ShopOnline.Models.Dtos;
using ShopOnline.Api.Entities;


namespace ShopOnline.Api.Extentions
{
    public static class DtoConversions
    {
        public static IEnumerable<ProductDto> ConvertToDto(this IEnumerable<Product> products, IEnumerable<ProductCategory> productCategories)
        {
            return from product in products
                   join productCategory in productCategories
                   on product.CategoryId equals productCategory.Id
                   select new ProductDto
                   {
                       Id = product.Id,
                       Name = product.Name,
                       ImageURL = product.ImageURL,
                       Description = product.Description,
                       Price = product.Price,
                       Qty = product.Qty,
                       CategroyId = product.CategoryId,
                       CategoryName = productCategory.Name
                   };
        }



        public static ProductDto ConvertToDto(this Product product, ProductCategory productCategory)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                ImageURL = product.ImageURL,
                Description = product.Description,
                Price = product.Price,
                Qty = product.Qty,
                CategroyId = product.CategoryId,
                CategoryName = productCategory.Name
            };
        }

        public static IEnumerable<CartItemDto> ConvertToDto(this IEnumerable<CartItem> cartItems, IEnumerable<Product> products)
        {
            return (from cartItem in cartItems
                    join product in products
                    on cartItem.ProductId equals product.Id
                    select new CartItemDto
                    {
                        Id = cartItem.Id,
                        ProductId = cartItem.ProductId,
                        ProductName = product.Name,
                        ProductDescription = product.Description,
                        ProductImageURL = product.ImageURL,
                        Price = product.Price,
                        CartId = cartItem.CartId,
                        Qty = cartItem.Qty,
                        TotalPrice = product.Price * cartItem.Qty,
                    }).ToList();
        }


        public static CartItemDto ConvertToDto(this CartItem cartItem, Product product)
        {
            return  new CartItemDto
                    {
                        Id = cartItem.Id,
                        ProductId = cartItem.ProductId,
                        ProductName = product.Name,
                        ProductDescription = product.Description,
                        ProductImageURL = product.ImageURL,
                        Price = product.Price,
                        CartId = cartItem.CartId,
                        Qty = cartItem.Qty,
                        TotalPrice = product.Price * cartItem.Qty,
                    };
        }
    }
}
