﻿using ShopOnline.Api.Entities;
using ShopOnline.Models.Dtos;


namespace ShopOnline.Api.Extentions
{
    public static class DtoConversions
    {


        public static IEnumerable<ProductCategoryDto> ConvertToDto(this IEnumerable<ProductCategory> productCategories)
        {
            return (from productCategory in productCategories
                    select new ProductCategoryDto
                    {
                        Id = productCategory.Id,
                        Name = productCategory.Name,
                        IconCss = productCategory.IconCss,
                    }
                    ).ToList();
        }
        public static IEnumerable<ProductDto> ConvertToDto(this IEnumerable<Product> products)
        {
            return from product in products
                   select new ProductDto
                   {
                       Id = product.Id,
                       Name = product.Name,
                       ImageURL = product.ImageURL,
                       Description = product.Description,
                       Price = product.Price,
                       Qty = product.Qty,
                       CategroyId = product.ProductCategory.Id,
                       CategoryName = product.ProductCategory.Name
                   };
        }



        public static ProductDto ConvertToDto(this Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                ImageURL = product.ImageURL,
                Description = product.Description,
                Price = product.Price,
                Qty = product.Qty,
                CategroyId = product.ProductCategory.Id,
                CategoryName = product.ProductCategory.Name
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
            return new CartItemDto
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
