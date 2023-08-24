using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.JSInterop;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace ShopOnline.Web.Pages
{
    [IgnoreAntiforgeryToken]
    public class CheckOutBase : ComponentBase
    {
        [Inject]
        public IJSRuntime Js { get; set; }

        [Inject]
        public IShoppingCartService _shoppingCartService { get; set; }
        protected IEnumerable<CartItemDto> ShoppingCartItems { get; set; }

        protected int TotalQty { get; set; }

        protected string PaymentDescription { get; set; }   
        protected decimal PaymentAmount{ get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await _shoppingCartService.GetItems(HardCodded.UserId);
                if(ShoppingCartItems != null)
                {
                    Guid orderGuid = Guid.NewGuid();
                    PaymentAmount = ShoppingCartItems.Sum(x => x.TotalPrice);
                    TotalQty = ShoppingCartItems.Sum(p => p.Qty);
                    PaymentDescription = $"{HardCodded.CartId}_{orderGuid}";
                }
            }
            catch (Exception)
            {
                //Log exception
                throw;
            }
        }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                if (firstRender)
                {
                    await Js.InvokeVoidAsync("initializePayPalButton");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

  

        [JSInvokable]
        public JsonResult CreateOrder()
        {
            //int TotalQty;
            //string PaymentDescription 
            //decimal PaymentAmount 

            if(TotalQty == 0 || PaymentAmount == 0 || PaymentDescription == "")
            {
                return new JsonResult("");
            }

            // create the request body like this:

            //{
            //        "purchase_units":
            //    [
            //        {
            //            "amount":
            //            { 
            //                "currency_code":"SEK",
            //                 "value":"100.00"
            //            }
            //        }
            //    ],
            //     "intent":"CAPTURE"
            //}
            JsonObject createOrderRequest = new JsonObject();
            createOrderRequest.Add("intent", "CAPTURE");


            JsonObject amount = new JsonObject();
            amount.Add("currency_code", "SEK");
            amount.Add("value", PaymentAmount);

            JsonObject purchaseUnit1 = new JsonObject();
            purchaseUnit1.Add("amount", amount);

            JsonArray purchaseUnits = new JsonArray();
            purchaseUnits.Add(purchaseUnit1);

            createOrderRequest.Add("Purchase_units", purchaseUnits);

            // get access token
            string accessToken = GetPaypalAccessToken();

            // send request
            var paypalUrl = "https://api-m.sandbox.paypal.com";
            string url = paypalUrl + "/v2/checkout/orders";

            string orderId = "";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer" + accessToken);
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Content = new StringContent(createOrderRequest.ToString(), null, "application/json");

                var responseTask = client.SendAsync(requestMessage);
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStreamAsync();
                    readTask.Wait();

                    var strResponse = readTask.Result;
                    var jsonResponse = JsonNode.Parse(strResponse);

                    if (jsonResponse != null)
                    {
                        accessToken = jsonResponse["id"]?.ToString() ?? "";

                        // save the order in the database
                    }
                }
            }

           var response = new
           {
                 Id = orderId
           };
            return   new JsonResult(response);
        }


        [JSInvokable]
        public JsonResult CompleteOrder([FromBody] JsonObject data)
        {

            if (data == null || data["orderID"] == null) return new JsonResult("");
            var orderID = data["orderID"]!.ToString();

            return new JsonResult("");
        }
        
        
        [JSInvokable]
        public JsonResult CancelOrder([FromBody] JsonObject data)
        {

            if (data == null || data["orderID"] == null) return new JsonResult("");
            var orderID = data["orderID"]!.ToString();

            return new JsonResult("");
        }


        private string GetPaypalAccessToken()
        {
            var paypalClientId = "AUQ4gb9Qih2EGoSCMnXik1Tws1LMd3bBxaw0Al1dX3yfY2O03dRVPRv-WvOS0jmG9qE7JoJHj_ncjWBL";
            var paypalSecret = "EGevTdSKVvAT1EvHBoBgHyZz-veLuXyxzKz7Wym6gjesHcJoGHD9RSAEZTOlS9gfeZzGUVxhnMDysxPU";
            var paypalUrl = "https://api-m.sandbox.paypal.com";

            string accessToken = "";
            string url = paypalUrl + "/v1/oauth2/token";

            using (var client = new HttpClient())
            {
                string credentials64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(paypalClientId + ":" + paypalSecret));

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Content = new StringContent("grant_type=client-credentials", null, 
                    "application/x-www-form-urlencoded");

                var responseTask = client.SendAsync(requestMessage);
                responseTask.Wait();

                var result = responseTask.Result;  
                if(result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStreamAsync();
                    readTask.Wait();
                    var strResponse = readTask.Result;

                    var jsonResponse = JsonNode.Parse(strResponse); 

                    if(jsonResponse != null)
                    {
                        accessToken = jsonResponse["access_token"]?.ToString() ?? "";
                    }
                }
            }

            return accessToken;
        }

    }
}
