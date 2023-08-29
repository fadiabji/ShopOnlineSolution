﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;
using System.Reflection.Metadata.Ecma335;

namespace ShopOnline.Web.Pages
{
    public class ProductByCategoryBase : ComponentBase
    {

        [Parameter]
        public int CategoryId { get; set; }


        [Inject]
        public IProductService _ProductService { get; set; }

        [Inject]
        public IManageProductsLocalStorageService _ManageProductsLocalStorageService { get; set; }


        public IEnumerable<ProductDto> Products { get; set; }


        public string CategoryName { get; set; }

        public string ErrorMessage { get; set; }


        protected override async Task OnParametersSetAsync()
        {

            try
            {
                Products = await GetProductCollectioByCategoryId(CategoryId);
                if(Products != null && Products.Count()>0) 
                {
                    var productDto = Products.FirstOrDefault(p =>p.CategroyId == CategoryId);   
                    if(productDto != null)
                    {
                        CategoryName = productDto.CategoryName;
                    }
                    
                }

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }


        private async Task<IEnumerable<ProductDto>> GetProductCollectioByCategoryId(int categoryId)
        {
            var productCollection = await _ManageProductsLocalStorageService.GetCollection();
            if(productCollection != null)
            {
                return productCollection.Where(p => p.Id == categoryId);
            }
            else
            {
                return await _ProductService.GetItemsByCateogry(categoryId);
            }
        }
    }
}
