using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ProductDetailsBase : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }
        [Inject]
        public IProductService _ProductService { get; set; }

        [Inject]
        public IShoppingCartService _ShoppingCartService { get; set; }

        //This service from runtime
        [Inject]
        public NavigationManager _NavigationManager { get; set; }

        public ProductDto Product { get; set; }

        public string ErrorMessage { get; set; }

        // write  just override first  then the list will come to choose OnInitializedAsync()
        protected override async Task OnInitializedAsync()
        {
            try
            {
                Product = await _ProductService.GetItem(Id);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        protected async Task AddToCart_Click(CartItemToAddDto cartItemToAddDto)
        {
            try
            {
                var carItemDto = await _ShoppingCartService.AddItem(cartItemToAddDto);
                _NavigationManager.NavigateTo("/ShoppingCart");
            }
            catch (Exception)
            {

               // log excetpion
            }
        }
    }
}
