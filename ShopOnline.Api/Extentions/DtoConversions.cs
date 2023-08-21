using ShopOnline.Models.Dtos;
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

    }
}
