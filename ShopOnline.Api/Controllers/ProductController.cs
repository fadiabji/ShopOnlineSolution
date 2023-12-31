﻿using Microsoft.AspNetCore.Mvc;
using ShopOnline.Api.Extentions;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {


        private readonly IProductRepostiories _productRepositiories;
        public ProductController(IProductRepostiories productRepostiories)
        {
            _productRepositiories = productRepostiories;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetItems()
        {
            try
            {
                var products = await _productRepositiories.GetItems();// Get all products in database
                if (products == null )
                {
                    return NotFound();
                }
                else
                {
                    var productDtos = products.ConvertToDto();
                    return Ok(productDtos);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }



        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetItem(int id)
        {
            try
            {
                var product = await _productRepositiories.GetItem(id);

                if (product == null)
                {
                    return BadRequest();
                }
                else
                {
                    var productDto = product.ConvertToDto();

                    return Ok(productDto);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpGet]
        [Route(nameof(GetProductCategories))]
        public async Task<ActionResult<IEnumerable<ProductCategoryDto>>> GetProductCategories()
        {
            try
            {
                var productCategories = await _productRepositiories.GetCategories();
                var productCategorieyDtos = productCategories.ConvertToDto();

                return Ok(productCategorieyDtos);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }

        }

        [HttpGet]
        [Route("{categoryId}/GetItemsByCategory")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetItemsByCategory(int categoryId)
        {
            try
            {
                var products = await _productRepositiories.GetItemByCategory(categoryId);
                var productDtos = products.ConvertToDto();
                return Ok(productDtos);
            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}
