namespace ShopOnline.Api.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; }


        public virtual User User { get; set; }
        public virtual ICollection<CartItem> CartItemsList{ get; set; }
        
    }
}
