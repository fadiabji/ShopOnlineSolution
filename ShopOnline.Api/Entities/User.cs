namespace ShopOnline.Api.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        public int CartId { get; set; }

        //public virtual Cart Cart { get; set; }

    }
}
