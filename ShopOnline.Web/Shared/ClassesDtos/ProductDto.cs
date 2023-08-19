using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Web.Shared.ClassesDtos
{
    public class ProductDto
    {
        public int Id{ get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagteURL { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public int CategroyId { get; set; }
        public string CategoryName { get; set; }
    }
}
