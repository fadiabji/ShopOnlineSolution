using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Web.Server.Entities;
using ShopOnline.Web.Server.Extentions;
using ShopOnline.Web.Server.Repositories.Contracts;
using ShopOnline.Web.Shared.ClassesDtos;

namespace ShopOnline.Web.Server.Controllers
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
                //var products = await _productRepositiories.GetItems();
                //var productCategories = await _productRepositiories.GetCategories();
                // make this instade of calling database two times which is not properate
                var products = await _productRepositiories.GetItems();
                if (products == null)
                    {
                        return NotFound();
                    }
                else
                {
                    var productDtos = products.ConvertToDto(products.Select(p=>p.ProductCategory));

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
