using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Extentions;
using ShopOnline.Models.Dtos;
using ShopOnline.Api.Repositories.Contracts;

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
                var productCategories = await _productRepositiories.GetCategories();
                // make this instade of calling database two times which is not properate
                //var products = await _productRepositiories.GetItems();
                if (products == null || productCategories == null)
                {
                        return NotFound();
                }
                else
                {
                    var productDtos = products.ConvertToDto(productCategories);

                    return Ok(productDtos);
                }
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

    }
}
