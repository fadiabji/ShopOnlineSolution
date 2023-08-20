namespace ShopOnline.Api.Entities
{
    public class CartItem
    {
        public int Id { get; set; }
        public int Qty { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }

        public virtual Cart Cart { get; set; }
        public virtual Product Product { get; set; }

    }
}
