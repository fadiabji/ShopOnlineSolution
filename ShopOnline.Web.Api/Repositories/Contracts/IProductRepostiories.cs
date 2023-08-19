using ShopOnline.Web.Server.Entities;

namespace ShopOnline.Web.Server.Repositories.Contracts
{
    public interface IProductRepostiories
    {
        Task<IEnumerable<Product>> GetItems();
        Task<IEnumerable<ProductCategory>> GetCategories();
        Task<Product> GetItem(int id);
        Task<ProductCategory> GetCateogy(int id);
        
    }
}
