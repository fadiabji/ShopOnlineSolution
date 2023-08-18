namespace ShopOnline.Web.Server.Entities
{
    public class cartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Qty { get; set; }

    }
}
